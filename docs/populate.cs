using System;
using System.Reflection;
using System.IO;
using MonoMac.Foundation;
using System.Xml.Linq;
using System.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Text;
using System.Collections.Generic;

class CtorUpdater {

	static Dictionary<string,Dictionary<string,List<string>>> eventArgsUsage = new Dictionary<string,Dictionary<string,List<string>>>();
	
	static int Record (Type t, string evtsig, string last)
	{
		string evt = evtsig.Substring (evtsig.IndexOf ('<')+1);
		evt = evt.Substring (0, evt.IndexOf ('>'));
		if (last.EndsWith (";"))
			last = last.Substring (0, last.Length-1);

		//Console.WriteLine ("Recording: {0} {1} {2}", t, evt, last);
		if (!eventArgsUsage.ContainsKey (evt)){
			eventArgsUsage [evt] = new Dictionary<string,List<string>>();
		}
		
		var v = eventArgsUsage [evt];
		if (!v.ContainsKey (t.FullName)){
			v [t.FullName] = new List<string> ();
		}
		v [t.FullName].Add (last);

		return 1;
	}

	public static void Save (XDocument xmldoc, Type t)
	{
		string xmldocpath = String.Format ("{0}/{1}/{2}.xml", monotouch_dir, t.Namespace, t.Name);
		
		var s = new XmlWriterSettings ();
		s.Indent = true;
		s.Encoding = new UTF8Encoding (false);
		s.OmitXmlDeclaration = true;
		using (var stream = File.Create (xmldocpath)){
			using (var xmlw = XmlWriter.Create (stream, s)){
				xmldoc.Save (xmlw);
			}
			stream.Write (new byte [] { 10 }, 0, 1);
		}
	}
	
	public static void ProcessEventArgs (Type t)
	{
		var xmldoc = LoadDoc (t);
		
		var summary = xmldoc.XPathSelectElement ("Type/Docs/summary");
			
		var sb = new StringBuilder ();
		int count = 0;
		if (eventArgsUsage.ContainsKey (t.FullName)){
			foreach (var e in eventArgsUsage [t.FullName]){
				int n = e.Value.Count;
				int i = 0;
				foreach (var s in e.Value){
					i++;
					if (sb.Length != 0){
						if (i == n)
							sb.Append (" and ");
						else
							sb.Append (", ");
					}
					sb.Append (String.Format ("<see cref=\"E:{0}.{1}\"/>", t.FullName, s));
					count++;
				}
			}
			
		} // else Console.WriteLine ("NO users for {0}", t);
		
		summary.ReplaceAll (XElement.Parse ("<Root>Provides data for the " + sb.ToString () + (count > 1 ? " events." : " event.") + "</Root>").DescendantNodes ());

		var remarks = xmldoc.XPathSelectElement ("Type/Docs/remarks");
		if (remarks.Value == "To be added.")
			remarks.Value = "";
		
		var ctorSummary = xmldoc.XPathSelectElement ("Type/Members/Member[@MemberName='.ctor']/Docs/summary");
		if (ctorSummary != null)
			ctorSummary.Value = "Initializes a new instance of the " + t.Name + " class.";
		// else  Console.WriteLine ("NO ctor for {0}", t.Name);
			
		var ctorRemarks = xmldoc.XPathSelectElement ("Type/Members/Member[@MemberName='.ctor']/Docs/remarks");
		if (ctorRemarks != null){
			if (ctorRemarks.Value == "To be added.")
				ctorRemarks.Value = "";
		}
			
		Save (xmldoc, t);
	}
	
	public static void ProcessNSO (Type t)
	{
		var xmldoc = LoadDoc (t);

		var flagctor = from el in xmldoc.XPathSelectElements ("Type/Members/Member")
			let y = el.XPathSelectElements ("MemberSignature").FirstOrDefault ()
			where y != null
			where y.Attribute ("Value").Value.IndexOf ("MonoMac.Foundation.NSObjectFlag") != -1
			select el;
		
		var coderctor = from el in xmldoc.XPathSelectElements ("Type/Members/Member")
			let y = el.XPathSelectElements ("MemberSignature").FirstOrDefault ()
			where y != null
			where y.Attribute ("Value").Value.IndexOf ("NSCoder") != -1
			select el;
		
		var intptrctor = from el in xmldoc.XPathSelectElements ("Type/Members/Member")
			let y = el.XPathSelectElements ("MemberSignature").FirstOrDefault ()
			where y != null
			where y.Attribute ("Value").Value.IndexOf ("IntPtr handle") != -1
			select el;
		
		var classhandles = from el in xmldoc.XPathSelectElements ("Type/Members/Member[@MemberName='ClassHandle']")
			let y = el.XPathSelectElements ("MemberSignature").FirstOrDefault ()
			where y != null
			let value = y.Attribute ("Value").Value
			where value == "public virtual IntPtr ClassHandle { get; }" || value == "public override IntPtr ClassHandle { get; }"
			select el;

		var delegates = from el in xmldoc.XPathSelectElements ("Type/Members/Member[@MemberName='Delegate']")
			select el;

		var weakdelegates = from el in xmldoc.XPathSelectElements ("Type/Members/Member[@MemberName='WeakDelegate']")
			select el;

		foreach (var x in classhandles){
			var e = x.XPathSelectElement ("Docs/summary");
			e.Value = "The handle for this class.";
			
			e = x.XPathSelectElement ("Docs/value");
			e.Value = "The pointer to the Objective-C class.";
			
			e = x.XPathSelectElement ("Docs/remarks");
			e.Value = "Each MonoMac class mirrors an unmanaged Objective-C class.   This value contains the pointer to the Objective-C class, it is similar to calling objc_getClass with the object name.";
			
		}
		
		foreach (var x in flagctor){
			var e = x.XPathSelectElement ("Docs/param");
			e.Value = "Unused sentinel value, pass NSObjectFlag.Empty.";
			
			e = x.XPathSelectElement ("Docs/summary");
			e.Value = "Constructor to call on derived classes when the derived class has an [Export] constructor.";
			
			e = x.XPathSelectElement ("Docs/remarks");
			e.RemoveAll ();
			e.Add (XElement.Parse ("<para>This constructor should be called by derived classes when they are initialized using an [Export] attribute. " +
					       "The argument value is ignore, typically the chaining would look like this:</para>"));
			e.Add (XElement.Parse ("<example><code lang=\"C#\">\n" +
					       "public class MyClass : BaseClass {\n"+
					       "    [Export (\"initWithFoo:\")]\n" +
					       "    public MyClass (string foo) : base (NSObjectFlag.Empty)\n"+
					       "    {\n" +
					       "        ...\n" +
					       "    }\n" +
					       "</code></example>"));
		}
		
		foreach (var x in coderctor){
			var e = x.XPathSelectElement ("Docs/param");
			if (e != null)
				e.Value = "The unarchiver object.";
			
			e = x.XPathSelectElement ("Docs/summary");
			e.Value = "A constructor that initializes the object from the data stored in the unarchiver object.";
			
			e = x.XPathSelectElement ("Docs/remarks");
			e.RemoveAll ();
			e.Value = "This constructor is provided to allow the class to be initialized from an unarchiver (for example, during NIB deserialization).";
		}
		
		
		foreach (var x in intptrctor){
			var e = x.XPathSelectElement ("Docs/param");
			e.Value = "Pointer (handle) to the unmanaged object.";
			
			e = x.XPathSelectElement ("Docs/summary");
			e.Value = "A constructor used when creating managed representations of unmanaged objects;  Called by the runtime.";
			
			e = x.XPathSelectElement ("Docs/remarks");
			e.RemoveAll ();
			e.Add (XElement.Parse ("<para>This constructor is invoked by the runtime infrastructure (<see cref=\"M:MonoMac.ObjCRuntime.GetNSObject (System.IntPtr)\"/>) to create a new managed representation for a pointer to an unmanaged Objective-C object.    You should not invoke this method directly, instead you should call the GetNSObject method as it will prevent two instances of a managed object to point to the same native object.</para>"));
		}

		foreach (var x in delegates){
			var dtype = x.XPathSelectElement ("ReturnValue/ReturnType").Value;
			
			var e = x.XPathSelectElement ("Docs/summary");
			e.Value = String.Format ("An instance of the {0} model class which acts as the class delegate.", dtype);

			e = x.XPathSelectElement ("Docs/value");
			e.Value = String.Format ("The instance of the {0} model class", dtype);

			e = x.XPathSelectElement ("Docs/remarks");
			e.RemoveAll ();
			e.Add (XElement.Parse ("<para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>"));
			e.Add (XElement.Parse ("<para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>"));
			e.Add (XElement.Parse ("<para>This is the strongly typed version of the object, use the WeakDelegate property instead if you want to merely assign a class derived from NSObject that has been decorated with [Export] attributes.</para>"));
		}

		foreach (var x in weakdelegates){
			var e = x.XPathSelectElement ("Docs/summary");
			e.Value = "An object that can respond to the delegate protocol for this type";

			e = x.XPathSelectElement ("Docs/value");
			e.Value = "The instance that will respond to events and data requests.";

			e = x.XPathSelectElement ("Docs/remarks");
			e.RemoveAll ();
			e.Add (XElement.Parse ("<para>The delegate instance assigned to this object will be used to handle events or provide data on demand to this class.</para>"));
			e.Add (XElement.Parse ("<para>When setting the Delegate or WeakDelegate values events will be delivered to the specified instance instead of being delivered to the C#-style events</para>"));
			e.Add (XElement.Parse ("<para>   Methods must be decorated with the [Export (\"selectorName\")] attribute to respond to each method from the protocol.   Alternatively use the Delegate method which is strongly typed and does not require the [Export] attributes on methods.</para>"));
		}
		
		Save (xmldoc, t);
	}

	static string monotouch_dir;

	static public XDocument LoadDoc (Type t)
	{
		string xmldocpath = String.Format ("{0}/{1}/{2}.xml", monotouch_dir, t.Namespace, t.Name);
		
		return XDocument.Load (xmldocpath);
	}
	
	public static void ScanEvents (Type t)
	{
		var xmldoc = LoadDoc (t);
		
		var events = from el in xmldoc.XPathSelectElements ("Type/Members/Member")
			let p = el.XPathSelectElements ("MemberType").FirstOrDefault ()
			where p != null && p.Value == "Event" || p.Value == "Field"
			let y = el.XPathSelectElements ("MemberSignature").FirstOrDefault ().Attribute ("Value")
			where y.Value.IndexOf ("EventArgs>") != -1 && y.Value.IndexOf ("<EventArgs>") == -1
			select Record (t, y.Value, y.Value.Substring (y.Value.LastIndexOf ('>')+1).Trim ());
		
		// Run query
		events.ToList ();
	}

	public static void DocumentHandle (Type t)
	{
		var xmldoc = LoadDoc (t);
		
		var h = xmldoc.XPathSelectElement ("Type/Members/Member[@MemberName='Handle']");
		if (h == null)
			return;

		var e = h.XPathSelectElement ("Docs/summary");
		e.Value = "Handle (pointer) to the unmanaged object representation.";
		
		e = h.XPathSelectElement ("Docs/value");
		e.Value = "A pointer";

		e = h.XPathSelectElement ("Docs/remarks");
		e.Value = "This IntPtr is a handle to the underlying unmanaged representation for this object.";
		
		Save (xmldoc, t);
	}
	
	public static void DocumentDisposable (Type t)
	{
		var xmldoc = LoadDoc (t);

		var dispose = from el in xmldoc.XPathSelectElements ("Type/Members/Member[@MemberName='Dispose' or @MemberName='System.IDisposable.Dispose']")
			let c = el.XPathSelectElements ("MemberSignature").FirstOrDefault ().Attribute ("Value")
			where c.Value.IndexOf ("Dispose ()") != -1
			select el;

		var disposevirt = from el in xmldoc.XPathSelectElements ("Type/Members/Member[@MemberName='Dispose']")
			let c = el.XPathSelectElements ("MemberSignature").FirstOrDefault ().Attribute ("Value")
			where c.Value.IndexOf ("Dispose (bool") != -1
			select el;

		var finalize = from el in xmldoc.XPathSelectElements ("Type/Members/Member[@MemberName='Finalize']")
			select el;

		foreach (var f in finalize){
			var e = f.XPathSelectElement ("Docs/summary");
			e.Value = "Finalizer for the " + t.Name + " object";
			e = f.XPathSelectElement ("Docs/remarks");
			e.Value = "";
		}
		
		foreach (var d in dispose){
			var e = d.XPathSelectElement ("Docs/summary");
			e.Value = "Releases the resources used by the " + t.Name + " object.";

			e = d.XPathSelectElement ("Docs/remarks");
			e.RemoveAll ();
			e.Add (XElement.Parse ("<para>The Dispose method releases the resources used by the " + t.Name + " class.</para>"));
			e.Add (XElement.Parse ("<para>Calling the Dispose method when you are finished using the " +t.Name + " ensures that all external resources used by this managed object are released as soon as possible.  Once you have invoked the Dispose method, the object is no longer useful and you should no longer make any calls to it.  For more information on releasing resources see ``Cleaning up Unmananaged Resources'' at http://msdn.microsoft.com/en-us/library/498928w2.aspx</para>"));
			
		}

		foreach (var d in disposevirt){
			var e = d.XPathSelectElement ("Docs/summary");
			e.Value = "Releases the resources used by the " + t.Name + " object.";

			e = d.XPathSelectElement ("Docs/param[@name='disposing']");
			//e = d.XPathSelectElement ("Docs/param");
			if (e != null){
				e.RemoveAll ();
				// Add it back after removeall
				e.Add (new XAttribute ("name", "disposing"));
				e.Add (XElement.Parse ("<para>If set to <see langword=\"true\"/>, the method is invoked directly and will dispose manage and unmanaged resources;   If set to <see langword=\"false\"/> the method is being called by the garbage collector finalizer and should only release unmanaged resources.</para>"));
			}
			
			e = d.XPathSelectElement ("Docs/remarks");
			e.RemoveAll ();
			e.Add (XElement.Parse ("<para>This Dispose method releases the resources used by the " + t.Name + " class.</para>"));
			e.Add (XElement.Parse ("<para>This method is called by both the Dispose() method and the object finalizer (Finalize).    When invoked by the Dispose method, the parameter disposting <paramref name=\"disposing\"/> is set to <see langword=\"true\"/> and any managed object references that this object holds are also disposed or released;  when invoked by the object finalizer, on the finalizer thread the value is set to <see langword=\"false\"/>. </para>"));
			e.Add (XElement.Parse ("<para>Calling the Dispose method when you are finished using the " +t.Name + " ensures that all external resources used by this managed object are released as soon as possible.  Once you have invoked the Dispose method, the object is no longer useful and you should no longer make any calls to it.</para>"));
			e.Add (XElement.Parse ("<para>  For more information on how to override this method and on the Dispose/IDisposable pattern, read the ``Implementing a Dispose Method'' document at http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx</para>"));
			
		}
		
		Save (xmldoc, t);
	}
	
	public static int Main (string [] args)
	{
		Assembly monotouch = typeof (MonoMac.Foundation.NSObject).Assembly;
		bool update_events = false;
		
		var dir = args [0];
		if (File.Exists (Path.Combine (dir, "en"))){
			Console.WriteLine ("The directory does not seem to be the root for documentation (missing en directory)");
			return 1;
		}
		monotouch_dir = Path.Combine (dir, "en");
		Type nso = monotouch.GetType ("MonoMac.Foundation.NSObject");

		foreach (Type t in monotouch.GetTypes ()){
			if (t.IsNotPublic || t.IsNested)
				continue;

			if (typeof (IDisposable).IsAssignableFrom (t)){
				DocumentDisposable (t);
			}
			
			DocumentHandle (t);
			if (typeof (MonoMac.ObjCRuntime.INativeObject).IsAssignableFrom (t)){
			}
			
			if (update_events)
				ScanEvents (t);

			if (t == nso || t.IsSubclassOf (nso))
				ProcessNSO (t);
		}

		// Now go and populate EventArgs
		if (update_events){
			foreach (Type t in monotouch.GetTypes ()){
				if (t.IsNotPublic || t.IsNested)
					continue;
				if (t.IsSubclassOf (typeof (EventArgs))){
					ProcessEventArgs (t);
				}
			}
		}
		
		return 0;
	}
}
