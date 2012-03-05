// Based on docfixer.exe

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;

using SQLite;
using HtmlAgilityPack;

namespace macdoc
{
	public class AppleDocMerger
	{
		public class Options
		{
			// Default options
			public Options ()
			{
				ImportSamples = true;
				QuickSummaries = true;
			}
			
			// Instruct the merger to take samples from a known repository and use them while merging to replace inline apple samples
			// If true, when we encounter a obj-c sample we try to get it's equivalent from sample repository (user point of view)
			// If false, we register it as a new sample (xamarinista point of view)
			public bool ImportSamples { get; set; }
			
			// Tell where to find or create the samples repository
			public string SamplesRepositoryPath { get; set; }
			
			// If true, it extracts the first sentence from the remarks and sticks it in the summary.
			public bool QuickSummaries { get; set; }
			
			// Where the Apple documentation is stored
			public string DocBase { get; set; }
			
			// Unused
			public bool DebugDocs { get; set; }
			
			// This is an abstraction over a .zip or a raw directory of ecma doc archive
			public IMdocArchive MonodocArchive { get; set; }
			
			// Point to a reflected-only version of the MonoTouch|MonoMac|... assembly
			public Assembly Assembly { get; set; }
			
			// Depending on the above assembly, the namespace prefix is stored there
			public string BaseAssemblyNamespace { get; set; }
			
			// People can supply a callback that will be called when a new file is being processed
			public Action<string> MergingPathCallback { get; set; }
		}
		
		class ProcessingContext
		{
			public string CurrentAppleDocPath { get; set; }
			public Type CurrentProcessedType { get; set; }
		}
		
		Options options;
		SampleRepository samples;
		Dictionary<string, Type> assemblyTypesLookup;
		int numOfMissingAppleDocumentation;
		string exportAttributeFullName;
		Action<string> pathCallback;
		
		[ThreadStatic]
		static ProcessingContext context;
		
		// This is a cache of the apple documentation path (value) corresponding to a type name (key)
		ConcurrentDictionary<string, string> docPaths = new ConcurrentDictionary<string, string> ();
		// Cache of precreated Apple documentation XML
		ConcurrentDictionary<string, XElement> loadedAppleDocs = new ConcurrentDictionary<string, XElement> ();

		static Regex selectorInHref = new Regex ("(?<type>[^/]+)/(?<selector>[^/]+)$");
		static Regex propertyAttrs = new Regex (@"^@property\((?<attrs>[^)]*)\)");
		
		// We use Apple own documentation database to locate the right documentation file
		SQLiteConnection db;
	
		Dictionary<string, Func<XElement, bool, object>> HtmlToMdocElementMapping;
		
		public AppleDocMerger (Options options)
		{
			this.options = options;
			
			if (options.Assembly == null)
				throw new ArgumentNullException ("options.Assembly");
			if (string.IsNullOrEmpty (options.BaseAssemblyNamespace))
				throw new ArgumentNullException ("options.BaseAssemblyNamespace");
			assemblyTypesLookup = new Dictionary<string, Type> ();
			foreach (var t in GetTypesSafe (options.Assembly)) {
				if (assemblyTypesLookup.ContainsKey (t.Name))
					Console.WriteLine ("Duplicated name {0} between {1} and {2}", t.Name, t.FullName, assemblyTypesLookup[t.Name].FullName);
				else
					assemblyTypesLookup[t.Name] = t;
			}
			
			if (options.MonodocArchive == null)
				throw new ArgumentNullException ("options.MonodocArchive");
			
			if (string.IsNullOrEmpty (options.DocBase) || !Directory.Exists (options.DocBase))
				throw new ArgumentException ("DocBase isn't valid", "options.DocBase");
			
			var dbPath = Path.Combine (options.DocBase, "..", "..", "docSet.dsidx");
			if (!File.Exists (dbPath))
				throw new ArgumentException ("DocBase doesn't contain a valid database file", "options.DocBase");
			db = new SQLiteConnection (dbPath);
			
			var samplesPath = string.IsNullOrEmpty (options.SamplesRepositoryPath) ? Path.Combine (options.DocBase, "samples.zip") : options.SamplesRepositoryPath;
			if (options.ImportSamples && !File.Exists (samplesPath))
				throw new ArgumentException ("We were asked to import samples but repository doesn't exist", samplesPath);
			samples = File.Exists (samplesPath) ? SampleRepository.LoadFrom (samplesPath) : new SampleRepository (samplesPath);
			
			pathCallback = options.MergingPathCallback;
			
			HtmlToMdocElementMapping = new Dictionary<string, Func<XElement, bool, object>> {
				{ "section",(e, i) => new [] {new XElement ("para", HtmlToMdoc ((XElement)e.FirstNode))}.Concat (HtmlToMdoc (e.Nodes ().Skip (1), i)) },
				{ "a",      (e, i) => ConvertLink (e, i) },
				{ "code",   (e, i) => ConvertCode (e, i) },
				{ "div",    (e, i) => ConvertDiv (e, i) },
				{ "em",     (e, i) => new XElement ("i", HtmlToMdoc (e.Nodes (), i)) },
				{ "li",     (e, i) => new XElement ("item", new XElement ("term", HtmlToMdoc (e.Nodes (), i))) },
				{ "ol",     (e, i) => new XElement ("list", new XAttribute ("type", "number"), HtmlToMdoc (e.Nodes ())) },
				{ "p",      (e, i) => new XElement ("para", HtmlToMdoc (e.Nodes (), i)) },
				{ "span",   (e, i) => HtmlToMdoc (e.Nodes (), i) },
				{ "strong", (e, i) => new XElement ("i", HtmlToMdoc (e.Nodes (), i)) },
				{ "tt",     (e, i) => new XElement ("c", HtmlToMdoc (e.Nodes (), i)) },
				{ "ul",     (e, i) => new XElement ("list", new XAttribute ("type", "bullet"), HtmlToMdoc (e.Nodes ())) },
			};
		}
		
		public void MergeDocumentation ()
		{
			Console.WriteLine ("Starting merge");
			var baseNamespace = options.BaseAssemblyNamespace;
			Type nso = options.Assembly.GetType (baseNamespace + ".Foundation.NSObject");
			exportAttributeFullName = baseNamespace + ".Foundation.ExportAttribute";

			//if (nso == null || export_attribute_type == null)
			if (nso == null)
				throw new ApplicationException (string.Format ("Incomplete {0} assembly", baseNamespace));
	
			//Parallel.ForEach (options.Assembly.GetTypes (), t => {
			foreach (var t in GetTypesSafe (options.Assembly)) {
				if (t.IsNotPublic || t.IsNested)
					continue;
	
				/*if (debug != null && t.FullName != debug)
					continue;*/
				
				if (t == nso || t.IsSubclassOf (nso)) {
					try {
						ProcessNSO (t);
					} catch (Exception e){
						Console.WriteLine ("Problem with {0} {1}", t.FullName, e);
					}
					Console.WriteLine ("Processed {0}", t.Name);
				}
			}
			//});
	
			// Only clean up if we are doing a full scan
			if (!options.ImportSamples)
				samples.Close (!options.DebugDocs);

			if (numOfMissingAppleDocumentation > 90) {
				throw new ApplicationException (string.Format ("Too many types were not found on this run ({0}), should be around 60-70 (mostly CoreImage, 3 UIKits, 2 CoreAnimation, 1 Foundation, 1 Bluetooth, 1 iAd",
				                                               numOfMissingAppleDocumentation));
			}
		}
		
		public void ProcessNSO (Type t)
		{
			if (context == null)
				context = new ProcessingContext ();
			context.CurrentProcessedType = t;
			
			var appledocpath = GetRealAppleDocPath (t);
			if (appledocpath == null) {
				Console.WriteLine ("No apple doc for {0} found", t.FullName);
				return;
			}
			AdvertiseNewPath (appledocpath);
			
			string xmldocpath = GetMdocPath (t);
			if (!File.Exists (xmldocpath)) {
				Console.WriteLine ("DOC REGEN PENDING for type: {0}", t.FullName);
				return;
			}
	
			XDocument xmldoc;
			using (var f = File.OpenText (xmldocpath))
				xmldoc = XDocument.Load (f);
	
			//Console.WriteLine ("Opened {0}", appledocpath);
			var appledocs = LoadAppleDocumentation (appledocpath);
			
			var typeRemarks = xmldoc.Element ("Type").Element ("Docs").Element ("remarks");
			var typeSummary = xmldoc.Element ("Type").Element ("Docs").Element ("summary");
			
			if (typeRemarks != null || (options.QuickSummaries && typeSummary != null)) {
				if (typeRemarks.Value == "To be added.")
					typeRemarks.Value = "";
				var overview = ExtractTypeOverview (appledocs);
				typeRemarks.Add (overview);
				if (overview != null && options.QuickSummaries && typeSummary.Value == "To be added."){
					foreach (var x in (System.Collections.IEnumerable) overview){
						var xe = x as XElement;
						if (xe == null)
							continue;
	
						if (xe.Name == "para"){
							var value = xe.Value;
							var dot = value.IndexOf ('.');
							if (dot == -1)
								typeSummary.Value = value;
							else
								typeSummary.Value = value.Substring (0, dot+1);
							break;
						}
					}
				}
			}
	
			var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
			foreach (var method in 
					t.GetMethods (flags).Cast<MethodBase> ()
					.Concat(t.GetConstructors (flags).Cast<MethodBase> ())) {
				bool prop = false;
				if (method.IsSpecialName)
					prop = true;
	
				// Skip methods from the base class
				if (method.DeclaringType != t)
					continue;
	
				//var attrs = method.GetCustomAttributes (export_attribute_type, true);
				var attrs = method.GetCustomAttributesData ();
				if (attrs.Count == 0)
					continue;
				var selector = GetSelector (attrs);
				if (selector == null) {
					Console.WriteLine ("Null selector for {0}::{1}", t.FullName, method.Name);
					continue;
				}
	
				if (selector == "init")
					continue;
				
				bool overrides = 
					(method.Attributes & MethodAttributes.Virtual) != 0 &&
					(method.Attributes & MethodAttributes.NewSlot) == 0;
	
				string keyFormat = "<h3 class=\"verytight\">{0}</h3>";
				string key = string.Format (keyFormat, selector);
	
				var mDoc = GetAppleMemberDocs (t, selector);
				//Console.WriteLine ("{0}", selector);
				if (mDoc == null){
					// Don't report known issues
					if (!AppleDocKnownIssues.IsKnown (t.Name, selector) && 
							// don't report property setters
							!(prop && method.Name.StartsWith ("set_")) &&
							// don't report overriding methods
					    !overrides) {
						Console.WriteLine ("While getting Apple Doc");
						ReportProblem (t, appledocpath, selector, key);
					}
					continue;
				}
				//Console.WriteLine ("Contents at {0}", p);
	
				//
				// Now, plug the docs
				//
				var member = GetMdocMember (xmldoc, selector);
				if (member == null){
					Console.WriteLine ("DOC REGEN PENDING for {0}.{1}", method.DeclaringType.Name, selector);
					continue;
				}
	
				//
				// Summary
				//
				var summaryNode = member.XPathSelectElement ("Docs/summary");
				if (summaryNode.Value == "To be added."){
					var summary = ExtractSummary (mDoc);
					if (summary == null)
						ReportProblem (t, appledocpath, selector, key);
					
					summaryNode.Value = "";
					summaryNode.Add (summary);
				}
	
				VerifyArgumentSemantic (mDoc, member, t, selector);
	
				//
				// Wipe out the value if it says "to be added"
				//
				var valueNode = member.XPathSelectElement ("Docs/value");
				if (valueNode != null){
					if (valueNode.Value == "To be added.")
						valueNode.Value = "";
				}
				
				//
				// Merge parameters
				//
				var eParamNodes = member.XPathSelectElements ("Docs/param").GetEnumerator ();
				//Console.WriteLine ("{0}", selector);
				var eAppleParams= ExtractParams (mDoc).GetEnumerator ();
				for ( ; eParamNodes.MoveNext () && eAppleParams.MoveNext (); ) {
					eParamNodes.Current.Value = "";
					eParamNodes.Current.Add (eAppleParams.Current);
				}
	
				//
				// Only extract the return value if there is a return in the type
				//
				var return_type = member.XPathSelectElement ("ReturnValue/ReturnType");
				if (return_type != null && return_type.Value != "System.Void" && member.XPathSelectElement ("MemberType").Value == "Method") {
					//Console.WriteLine ("Scanning for return {0} {1}", t.FullName, selector);
					var ret = ExtractReturn (mDoc);
					if (ret == null && !AppleDocKnownIssues.IsKnownMissingReturnValue (t, selector))
						Console.WriteLine ("Problem extracting a return value for type=\"{0}\" selector=\"{1}\"", t.FullName, selector);
					else {
						var retNode = prop
							? member.XPathSelectElement ("Docs/value")
							: member.XPathSelectElement ("Docs/returns");
						if (retNode != null && ret != null){
							retNode.Value = "";
							retNode.Add (ret);
						}
					}
				}
				
				var remarks = ExtractDiscussion (mDoc);
				
				if (remarks != null){
					var remarksNode = member.XPathSelectElement ("Docs/remarks");
					if (remarksNode.Value == "To be added.")
						remarksNode.Value = "";
					remarksNode.Add (remarks);
				}
			}
			
			var s = new XmlWriterSettings ();
			s.Indent = true;
			s.Encoding = Encoding.UTF8;
			s.OmitXmlDeclaration = true;
			using (var output = File.CreateText (xmldocpath)){
				var xmlw = XmlWriter.Create (output, s);
				xmldoc.Save (xmlw);
				output.WriteLine ();
			}
		}
		
		string GetSelector (IList<CustomAttributeData> attrs)
		{
			/*return attrs
				.Where (attr => attr.Constructor.DeclaringType.FullName.Equals (exportAttributeFullName, StringComparison.InvariantCultureIgnoreCase))
				.Select (attr => attr.ConstructorArguments[0].Value.ToString ()).FirstOrDefault ();*/
			return attrs.Where (a => a != null && a.ConstructorArguments.Count > 0).Select (a => a.ConstructorArguments[0].Value.ToString ()).FirstOrDefault ();
		}

		// Extract the path from Apple's Database
		public string GetAppleDocFor (Type t)
		{
			var path = db.CreateCommand ("select zkpath from znode join ztoken on znode.z_pk == ztoken.zparentnode where ztoken.ztokenname like \"" + t.Name + "\"").ExecuteScalar<string> ();
			
			return Path.Combine (options.DocBase, "..", path);
		}
	
		public void ReportProblem (Type t, string docpath, string selector, string key)
		{
			Console.WriteLine (t);
			Console.WriteLine ("    Error: did not find selector \"{0}\"", selector);
			Console.WriteLine ("     File: {0}", docpath);
			Console.WriteLine ("      key: {0}", key);
			Console.WriteLine ();
		}
	
		object ConvertLink (XElement e, bool insideFormat)
		{
			var href = e.Attribute ("href");
			if (href == null){
				return "";
			}
			var m = selectorInHref.Match (href.Value);
			if (!m.Success)
				return "";
	
			var selType   = m.Groups ["type"].Value;
			var selector  = m.Groups ["selector"].Value;

			Type type = null;
			if (assemblyTypesLookup.TryGetValue (selType, out type)) {
				var typedocpath = GetMdocPath (type);
				if (File.Exists (typedocpath)) {
					XDocument typedocs;
					using (var f = File.OpenText (typedocpath))
						typedocs = XDocument.Load (f);
					var member = GetMdocMember (typedocs, selector);
					if (member != null)
						return new XElement ("see",
								new XAttribute ("cref", CreateCref (typedocs, member)));
				}
			}
			if (!href.Value.StartsWith ("#")) {
				var r = Path.GetFullPath (Path.Combine (Path.GetDirectoryName (context.CurrentAppleDocPath), href.Value));
	
				href.Value = r.Replace (options.DocBase, "http://developer.apple.com/iphone/library/documentation");
			}
			
			return insideFormat
				? e
				: new XElement ("format",
						new XAttribute ("type", "text/html"), e);
		}
	
		string CreateCref (XDocument typedocs, XElement member)
		{
			var cref = new StringBuilder ();
			var memberType = member.Element ("MemberType").Value;
			switch (memberType) {
				case "Constructor": cref.Append ("C"); break;
				case "Event":       cref.Append ("E"); break;
				case "Field":       cref.Append ("F"); break;
				case "Method":      cref.Append ("M"); break;
				case "Property":    cref.Append ("P"); break;
				default:
					throw new InvalidOperationException (string.Format ("Unsupported member type '{0}' for member {1}.{2}.",
								memberType,
								typedocs.Root.Attribute ("FullName").Value, 
								member.Attribute("MemberName").Value));
			}
			cref.Append (":");
			cref.Append (typedocs.Root.Attribute ("FullName").Value);
			if (memberType != "Constructor") {
				cref.Append (".");
				cref.Append (member.Attribute ("MemberName").Value.Replace (".", "#"));
			}
	
			var p = member.Element ("Parameters");
			if (p != null && p.Descendants ().Any ()) {
				cref.Append ("(");
				bool first = true;
				var ps = p.Descendants ();
				foreach (var pi in ps) {
					cref.AppendFormat ("{0}{1}", first ? "" : ",", pi.Attribute ("Type").Value);
					first = false;
				}
				cref.Append (")");
			}
	
			return cref.ToString ();
		}
	
		XElement ConvertCode (XElement e, bool insideFormat)
		{
			if (e.Value == "YES")
				return new XElement ("see", new XAttribute ("langword", "true"));
			if (e.Value == "NO")
				return new XElement ("see", new XAttribute ("langword", "false"));
			if (e.Value == "nil")
				return new XElement ("see", new XAttribute ("langword", "null"));
			return new XElement ("c", HtmlToMdoc (e.Nodes (), insideFormat));
		}
	
		// This method checks if the div is a code sample and if it's the case process it in a special way.
		// if it's not the case it just delegate to HtmlToMdoc
		object ConvertDiv (XElement e, bool insideFormat)
		{
			var cls = e.Attribute ("class");
			if (cls == null || !cls.Value.Contains ("codesample"))
				return HtmlToMdoc (e.Nodes (), insideFormat);
	
			string content = e.Descendants ("tr").Select (tr => tr.Value).Aggregate ((l1, l2) => l1 + Environment.NewLine + l2);
			XElement result = null;
	
			if (options.ImportSamples) {
				SampleDesc desc;
				string snippet = samples.GetSampleFromContent (content, out desc);
				result = new XElement ("example", new XElement ("code", new XAttribute ("lang", desc.Language), new XAttribute (XNamespace.Xml + "space", "preserve"), snippet));
				Console.WriteLine ("Imported sample {0}", desc.ID);
			} else {
				var id = samples.RegisterSample (content, new SampleDesc { DocumentationFilePath = context.CurrentAppleDocPath, Language = "obj-c", FullTypeName = context.CurrentProcessedType.FullName } );
				result = new XElement ("sample", new XAttribute ("external-id", id));
			}
			return result;
		}
	
		IEnumerable<object> HtmlToMdoc (IEnumerable<XNode> rest)
		{
			return HtmlToMdoc (rest, false);
		}
	
		IEnumerable<object> HtmlToMdoc (IEnumerable<XNode> rest, bool insideFormat)
		{
			foreach (var e in rest)
				yield return HtmlToMdoc (e, insideFormat);
		}
	
		object HtmlToMdoc (XElement e)
		{
			return HtmlToMdoc (e, false);
		}
	
		object HtmlToMdoc (XNode n, bool insideFormat)
		{
			// Try to intelligently convert HTML into mdoc(5).
			object r = null;
			var e = n as XElement;
			if (e != null && HtmlToMdocElementMapping.ContainsKey (e.Name.LocalName))
				r = HtmlToMdocElementMapping [e.Name.LocalName] (e, insideFormat);
			else if (e != null && !insideFormat)
				r = new XElement ("format",
						new XAttribute ("type", "text/html"),
						HtmlToMdoc (e, true));
			else if (e != null)
				r = new XElement (e.Name,
						e.Attributes (),
						HtmlToMdoc (e.Nodes (), insideFormat));
			else
				r = n;
			return r;
		}
	
		object HtmlToMdoc (XElement e, IEnumerable<XElement> rest)
		{
			return HtmlToMdoc (new[]{e}.Concat (rest).Cast<XNode> (), false);
		}
	
		class XElementDocumentOrderComparer : IComparer<XElement>
		{
			public static readonly IComparer<XElement> Default = new XElementDocumentOrderComparer ();
	
			public int Compare (XElement a, XElement b)
			{
				if (object.ReferenceEquals (a, b))
					return 0;
				if (a.IsBefore (b))
					return -1;
				return 1;
			}
		}
	
		XElement FirstInDocument (params XElement[] elements)
		{
			IEnumerable<XElement> e = elements;
			return FirstInDocument (e);
		}
	
		XElement FirstInDocument (IEnumerable<XElement> elements)
		{
			return elements
				.Where (e => e != null)
				.OrderBy (e => e, XElementDocumentOrderComparer.Default)
				.FirstOrDefault ();
		}
	
		public object ExtractTypeOverview (XElement appledocs)
		{
			var overview  = appledocs.Descendants("h2").Where(e => e.Value == "Overview").FirstOrDefault();
			if (overview == null)
				return null;
	
			var end = FirstInDocument (
					GetDocSections (appledocs)
					.Concat (new[]{
						overview.ElementsAfterSelf ().Descendants ("hr").FirstOrDefault ()
					}));
			if (end == null)
				return null;
	
			var contents = overview.ElementsAfterSelf().Where(e => e.IsBefore(end));
			return HtmlToMdoc (contents.FirstOrDefault (), contents.Skip (1));
		}
	
		IEnumerable<XElement> GetDocSections (XElement appledocs)
		{
			foreach (var e in appledocs.Descendants ("h2")) {
				if (e.Value == "Class Methods" ||
						e.Value == "Instance Methods" ||
						e.Value == "Properties")
					yield return e;
			}
		}
		
		public object ExtractSummary (XElement member)
		{
			try {
				return HtmlToMdoc (member.ElementsAfterSelf ("p").First ());
			} catch {
				return null;
			}
		}
	
		public object ExtractSection (XElement member)
		{
			try {
				return HtmlToMdoc (member.ElementsAfterSelf ("section").First ());
			} catch {
				return null;
			}
		}
		
		XElement GetMemberDocEnd (XElement member)
		{
			return FirstInDocument (
					member.ElementsAfterSelf ("h3").FirstOrDefault (),
					member.ElementsAfterSelf ("hr").FirstOrDefault ());
		}
	
		IEnumerable<XElement> ExtractSection (XElement member, string section)
		{
			return from x in member.ElementsAfterSelf ("div")
				let h5 = x.Descendants ("h5").FirstOrDefault (e => e.Value == section)
				where h5 != null
				from j in h5.ElementsAfterSelf () select j;
		}
	
		public IEnumerable<object> ExtractParams (XElement member)
		{
			var param = ExtractSection (member, "Parameters");
			if (param == null || !param.Any ()){
				return new object[0];
			}
	
			return param.Elements ("dd").Select (d => HtmlToMdoc (d));
		}
	
		public object ExtractReturn (XElement member)
		{
			var e = ExtractSection (member, "Return Value");
			if (e == null)
				return null;
			return HtmlToMdoc (e.Cast<XNode> ());
		}
	
		public object ExtractDiscussion (XElement member)
		{
			var discussion = from x in member.ElementsAfterSelf ("div")
				let h5 = x.Descendants ("h5").FirstOrDefault (e => e.Value == "Discussion")
				where h5 != null
				from j in h5.ElementsAfterSelf () select j;
	
			return HtmlToMdoc (discussion.Cast<XNode> ());
		}
	
		string GetMdocPath (Type t)
		{
			return options.MonodocArchive.GetPathForType (t);
		}
			
		XElement GetMdocMember (XDocument mdoc, string selector)
		{
			var exportAttr = options.BaseAssemblyNamespace + ".Foundation.Export(\"" + selector + "\"";
			return
				(from m in mdoc.XPathSelectElements ("Type/Members/Member")
					where m.Descendants ("Attributes").Descendants ("Attribute").Descendants ("AttributeName")
						.Where (n => n.Value.StartsWith (exportAttr) || 
							n.Value.StartsWith ("get: " + exportAttr) || 
							n.Value.StartsWith ("set: " + exportAttr)).Any ()
					select m
				).FirstOrDefault ();
		}

		string FixAppleDocPath (Type t, string appledocpath)
		{
			var indexContent = File.ReadAllText (appledocpath);
			if (indexContent.IndexOf ("<meta id=\"refresh\"") != -1){
				var p = indexContent.IndexOf ("0; URL=");
				if (p == -1){
					Interlocked.Increment (ref numOfMissingAppleDocumentation);
					Console.WriteLine ("Error, got an index.html file but can not find its refresh page for {0} and {1}", t.Name, appledocpath);
					return appledocpath;
				}
				p += 7;
				var l = indexContent.IndexOf ("\"", p);
				appledocpath = Path.Combine (Path.GetDirectoryName (appledocpath), indexContent.Substring (p, l-p));
				Console.WriteLine ("Fixed URL: {0}", appledocpath);
				docPaths[t.FullName] = appledocpath;
				return appledocpath;
			}
			return appledocpath;
		}
		
		string GetRealAppleDocPath (Type t)
		{
			string appledocpath = null;
			if (docPaths.ContainsKey (t.FullName)) {
				appledocpath = docPaths[t.FullName];
				if (appledocpath == null)
					return null;
			} else {
				appledocpath = GetAppleDocFor (t);
				if (appledocpath == null || !File.Exists (appledocpath)){
					Interlocked.Increment (ref numOfMissingAppleDocumentation);
					return null;
				}
				appledocpath = FixAppleDocPath (t, appledocpath);
			}
			context.CurrentAppleDocPath = appledocpath;
			
			return appledocpath;
		}
	
		void VerifyArgumentSemantic (XElement mDoc, XElement member, Type t, string selector)
		{
			// ArgumentSemantic validation
			XElement code;
			var codeDeclaration = mDoc.ElementsAfterSelf ("pre").FirstOrDefault ();
			if (codeDeclaration == null || codeDeclaration.Attribute ("class") == null ||
					codeDeclaration.Attribute ("class").Value != "declaration" ||
					(code = codeDeclaration.Elements ("code").FirstOrDefault ()) == null)
				return;
			var decl = code.Value;
	
			var m = propertyAttrs.Match (decl);
			string attrs;
			if (!m.Success || string.IsNullOrEmpty (attrs = m.Groups ["attrs"].Value))
				return;
	
			string semantic = null;
	
			if (attrs.Contains ("assign"))
				semantic = "ArgumentSemantic.Assign";
			else if (attrs.Contains ("copy"))
				semantic = "ArgumentSemantic.Copy";
			else if (attrs.Contains ("retain"))
				semantic = "ArgumentSemantic.Retain";
	
			if (semantic != null &&
					!member.XPathSelectElements ("Attributes/Attribute/AttributeName").Any (a => a.Value.Contains (semantic))) {
				Console.WriteLine ("Missing [Export (\"{0}\", {1})] on Type={2} Member='{3}'", selector, semantic, t.FullName, 
						member.XPathSelectElement ("MemberSignature[@Language='C#']").Attribute ("Value").Value);
			}
		}
	
		public XElement GetAppleMemberDocs(Type t, string selector)
		{
			foreach (var appledocs in GetAppleDocumentationSources (t)) {
				
				var mDoc = appledocs.Descendants ("h3").Where (e => e.Value == selector).FirstOrDefault ();
				if (mDoc == null) {
					// Many read-only properties have an 'is' prefix on the selector
					// (which is removed on the docs), so try w/o the prefix, e.g. 
					//   @property(getter=isDoubleSided) BOOL doubleSided;
					var newSelector = char.ToLower (selector [2]) + selector.Substring (3);
					mDoc = appledocs.Descendants ("h3").Where (e => e.Value == newSelector).FirstOrDefault ();
				}
	
				if (mDoc != null)
					return mDoc;
			}
			return null;
		}
	
		public IEnumerable<XElement> GetAppleDocumentationSources (Type t)
		{
			string path = GetRealAppleDocPath (t);
			if (path != null)
				yield return LoadAppleDocumentation (path);
			while ((t = t.BaseType) != typeof (object) && t != null) {
				path = GetRealAppleDocPath (t);
				if (path != null)
					yield return LoadAppleDocumentation (path);
			}
		}

		public XElement LoadAppleDocumentation (string path)
		{
			XElement appledocs;
			if (loadedAppleDocs.TryGetValue (path, out appledocs))
				return appledocs;
	
			var doc = new HtmlDocument();
			doc.Load (path, Encoding.UTF8);
			doc.OptionOutputAsXml = true;
			var sw = new StringWriter ();
			doc.Save (sw);
	
			//doc.Save ("/tmp/foo-" + Path.GetFileName (path));
			
			// minor global fixups
			var contents = sw.ToString ()
				.Replace ("&amp;#160;",   " ")
				.Replace ("&amp;#8211;",  "-")
				.Replace ("&amp;#xA0;",   " ")
				.Replace ("&amp;nbsp;",   " ");
	
			// HtmlDocument wraps the <html/> with a <span/>; skip the <span/>.
			appledocs = XElement.Parse (contents).Elements().First();
	
			// remove the xmlns element from everything...
			foreach (var e in appledocs.DescendantsAndSelf ()) {
				e.Name = XName.Get (e.Name.LocalName);
			}
			loadedAppleDocs [path] = appledocs;
			return appledocs;
		}
		
		// Only let one thread at a time advertise a new path
		void AdvertiseNewPath (string path)
		{
			Action<string> callback;
			if (pathCallback == null || (callback = Interlocked.Exchange (ref pathCallback, null)) == null)
				return;
			callback (path);
			pathCallback = callback;
		}
		
		IEnumerable<Type> typesCache = null;
		
		IEnumerable<Type> GetTypesSafe (Assembly assembly)
		{
			if (typesCache != null)
				return typesCache;
			
			// ReflectionOnly load doesn't load dependencies automatically
			// but since we aren't interested in them as we just use MT types
			// load them in this bizarre way
			Type[] types = null;
			try {
				assembly.GetTypes ();
			} catch (ReflectionTypeLoadException e) {
				types = e.Types;
			}
			typesCache = types.Where (t => t != null).ToList ();
			//Console.WriteLine (types.Where (t => t != null).Select (t => t.Name).Aggregate ((e1, e2) => e1 + ", " + e2));
			return typesCache;
		}
	}
}

