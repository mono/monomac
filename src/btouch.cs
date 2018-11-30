//
// Authors:
//   Miguel de Icaza
//
// Copyright 2011 Xamarin Inc.
// Copyright 2009-2010 Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Mono.Options;

#if !MONOMAC
using MonoTouch.ObjCRuntime;
#endif

class BindingTouch {
#if MONOMAC
	static string baselibdll = "MonoMac.dll";
	static string RootNS = "MonoMac";
	static Type CoreObject = typeof (MonoMac.Foundation.NSObject);
	static string tool_name = "bmac";
	static string compiler = "csc";
	static string net_sdk = null;
#else
	static string baselibdll = "/Developer/MonoTouch/usr/lib/mono/2.1/monotouch.dll";
	static string RootNS = "MonoTouch";
	static Type CoreObject = typeof (MonoTouch.Foundation.NSObject);
	static string tool_name = "btouch";
	static string compiler = "/Developer/MonoTouch/usr/bin/smcs";
	static string net_sdk = null;
#endif

	public static string ToolName {
		get { return tool_name; }
	}

	static void ShowHelp (OptionSet os)
	{
		Console.WriteLine ("{0} - Mono Objective-C API binder", tool_name);
		Console.WriteLine ("Usage is:\n {0} [options] apifile1.cs [apifileN] [-s=core1.cs [-s=core2.cs]] [-x=extra1.cs [-x=extra2.cs]]", tool_name);
		
		os.WriteOptionDescriptions (Console.Out);
	}
	
	static int Main (string [] args)
	{
		try {
			return Main2 (args);
		} catch (Exception ex) {
			ErrorHelper.Show (ex);
			return 1;
		}
	}
	
	static int Main2 (string [] args)
	{
		bool show_help = false;
		bool zero_copy = false;
		bool alpha = false;
		string basedir = null;
		string tmpdir = null;
		string ns = null;
		string outfile = null;
		bool delete_temp = true, debug = false;
		bool verbose = false;
		bool unsafef = true;
		bool external = false;
		bool pmode = true;
		bool nostdlib = false;
		bool clean_mono_path = false;
		bool native_exception_marshalling = false;
		bool inline_selectors = false;
		List<string> sources;
		var resources = new List<string> ();
#if !MONOMAC
		var linkwith = new List<string> ();
#endif
		var references = new List<string> ();
		var libs = new List<string> ();
		var core_sources = new List<string> ();
		var extra_sources = new List<string> ();
		var defines = new List<string> ();
		bool binding_third_party = true;
		string generate_file_list = null;
		
		var os = new OptionSet () {
			{ "h|?|help", "Displays the help", v => show_help = true },
			{ "a", "Include alpha bindings", v => alpha = true },
			{ "outdir=", "Sets the output directory for the temporary binding files", v => { basedir = v; }},
			{ "o|out=", "Sets the name of the output library", v => outfile = v },
			{ "tmpdir=", "Sets the working directory for temp files", v => { tmpdir = v; delete_temp = false; }},
			{ "debug", "Generates a debugging build of the binding", v => debug = true },
			{ "sourceonly=", "Only generates the source", v => generate_file_list = v },
			{ "ns=", "Sets the namespace for storing helper classes", v => ns = v },
			{ "unsafe", "Sets the unsafe flag for the build", v=> unsafef = true },
#if MONOMAC
			{ "core", "Use this to build monomac.dll", v => binding_third_party = false },
#else
			{ "core", "Use this to build monotouch.dll", v => binding_third_party = false },
#endif
			{ "r=", "Adds a reference", v => references.Add (v) },
			{ "lib=", "Adds the directory to the search path for the compiler", v => libs.Add (v) },
			{ "compiler=", "Sets the compiler to use", v => compiler = v },
			{ "sdk=", "Sets the .NET SDK to use", v => net_sdk = v },
			{ "d=", "Defines a symbol", v => defines.Add (v) },
			{ "s=", "Adds a source file required to build the API", v => core_sources.Add (v) },
			{ "v", "Sets verbose mode", v => verbose = true },
			{ "x=", "Adds the specified file to the build, used after the core files are compiled", v => extra_sources.Add (v) },
			{ "e", "Generates smaller classes that can not be subclassed (previously called 'external mode')", v => external = true },
			{ "p", "Sets private mode", v => pmode = false },
			{ "baselib=", "Sets the base library", v => baselibdll = v },
			{ "use-zero-copy", v=> zero_copy = true },
			{ "nostdlib", "Does not reference mscorlib.dll library", l => nostdlib = true },
			{ "no-mono-path", "Launches compiler with empty MONO_PATH", l => clean_mono_path = true },
			{ "native-exception-marshalling", "Enable the marshalling support for Objective-C exceptions", l => native_exception_marshalling = true },
			{ "inline-selectors:", "If Selector.GetHandle is inlined and does not need to be cached (default: false)", v => inline_selectors = string.Equals ("true", v, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty (v) },
#if !MONOMAC
			{ "link-with=,", "Link with a native library {0:FILE} to the binding, embedded as a resource named {1:ID}",
				(path, id) => {
					if (path == null || path.Length == 0)
						throw new Exception ("-link-with=FILE,ID requires a filename.");
					
					if (id == null || id.Length == 0)
						id = Path.GetFileName (path);
					
					if (linkwith.Contains (id))
						throw new Exception ("-link-with=FILE,ID cannot assign the same resource id to multiple libraries.");
					
					resources.Add (string.Format ("-res:{0},{1}", path, id));
					linkwith.Add (id);
				}
			},
#endif
		};

		try {
			sources = os.Parse (args);
		} catch (Exception e){
			Console.Error.WriteLine ("{0}: {1}", tool_name, e.Message);
			Console.Error.WriteLine ("see {0} --help for more information", tool_name);
			return 1;
		}

		if (show_help || sources.Count == 0){
			Console.WriteLine ("Error: no api file provided");
			ShowHelp (os);
			return 0;
		}

		if (alpha)
			defines.Add ("ALPHA");
		
		if (tmpdir == null)
			tmpdir = GetWorkDir ();

		if (outfile == null)
			outfile = Path.GetFileNameWithoutExtension (sources [0]) + ".dll";

		string refs = (references.Count > 0 ? "-r:" + String.Join (" -r:", references.ToArray ()) : "");
		string paths = (libs.Count > 0 ? "-lib:" + String.Join (" -lib:", libs.ToArray ()) : "");

		try {
			var api_file = sources [0];
			var tmpass = Path.Combine (tmpdir, "temp.dll");

			// -nowarn:436 is to avoid conflicts in definitions between core.dll and the sources
			var cargs = String.Format ("{10} -debug -unsafe -target:library {0} -nowarn:436 -out:{1} -r:{2} {3} {4} {5} -r:{6} {7} {8} {9}",
						   string.Join (" ", sources.ToArray ()),
						   tmpass, Environment.GetCommandLineArgs ()[0],
						   string.Join (" ", core_sources.ToArray ()), refs, unsafef ? "-unsafe" : "",
						   baselibdll, string.Join (" ", defines.Select (x=> "-define:" + x).ToArray ()), paths,
						   nostdlib ? "-nostdlib" : null,
						   !String.IsNullOrEmpty (net_sdk) ? "-sdk:" + net_sdk : null);

			var si = new ProcessStartInfo (compiler, cargs) {
				UseShellExecute = false,
			};

			if (clean_mono_path) {
				// HACK: We are calling btouch with forced 2.1 path but we need working mono for compiler
				si.EnvironmentVariables.Remove ("MONO_PATH");
			}

			if (verbose)
				Console.WriteLine ("{0} {1}", si.FileName, si.Arguments);
			
			var p = Process.Start (si);
			p.WaitForExit ();
			if (p.ExitCode != 0){
				Console.WriteLine ("{0}: API binding contains errors.", tool_name);
				return 1;
			}

			Assembly api;
			try {
				api = Assembly.LoadFrom (tmpass);
			} catch (Exception e) {
				if (verbose)
					Console.WriteLine (e);
				
				Console.Error.WriteLine ("Error loading API definition from {0}", tmpass);
				return 1;
			}

			Assembly baselib;
			try {
				baselib = Assembly.LoadFrom (baselibdll);
			} catch (Exception e){
				if (verbose)
					Console.WriteLine (e);

				Console.Error.WriteLine ("Error loading base library {0}", baselibdll);
				return 1;
			}

#if !MONOMAC
			foreach (object attr in api.GetCustomAttributes (typeof (LinkWithAttribute), true)) {
				LinkWithAttribute linkWith = (LinkWithAttribute) attr;
				
				if (!linkwith.Contains (linkWith.LibraryName)) {
					Console.Error.WriteLine ("Missing native library {0}, please use `--link-with' to specify the path to this library.", linkWith.LibraryName);
					return 1;
				}
			}
#endif

			var types = new List<Type> ();
			foreach (var t in api.GetTypes ()){
				if (t.GetCustomAttributes (typeof (BaseTypeAttribute), true).Length > 0 ||
				    t.GetCustomAttributes (typeof (StaticAttribute), true).Length > 0)
					types.Add (t);
			}

			var g = new Generator (pmode, external, debug, types.ToArray ()){
				MessagingNS = ns == null ? Path.GetFileNameWithoutExtension (api_file) : ns,
				CoreMessagingNS = RootNS + ".ObjCRuntime",
				BindThirdPartyLibrary = binding_third_party,
				CoreNSObject = CoreObject,
				BaseDir = basedir != null ? basedir : tmpdir,
				ZeroCopyStrings = zero_copy,
				NativeExceptionMarshalling = native_exception_marshalling,
#if MONOMAC
				OnlyX86 = true,
#endif
				Alpha = alpha,
				InlineSelectors = inline_selectors,
			};

			foreach (var mi in baselib.GetType (RootNS + ".ObjCRuntime.Messaging").GetMethods ()){
				if (mi.Name.IndexOf ("_objc_msgSend") != -1)
					g.RegisterMethodName (mi.Name);
			}

			g.Go ();

			if (generate_file_list != null){
				using (var f = File.CreateText (generate_file_list)){
					g.GeneratedFiles.ForEach (x => f.WriteLine (x));
				}
				return 0;
			}

			cargs = String.Format ("{0} -target:library -out:{1} {2} {3} {4} {5} {6} {7} -r:{8} {9} {10}",
					       unsafef ? "-unsafe" : "", /* 0 */
					       outfile, /* 1 */
					       string.Join (" ", defines.Select (x=> "-define:" + x).ToArray ()), /* 2 */
					       String.Join (" ", g.GeneratedFiles.ToArray ()), /* 3 */
					       String.Join (" ", core_sources.ToArray ()), /* 4 */
					       String.Join (" ", sources.Skip (1).ToArray ()), /* 5 */
					       String.Join (" ", extra_sources.ToArray ()), /* 6 */
					       refs, /* 7 */
					       baselibdll, /* 8 */
					       String.Join (" ", resources.ToArray ()), /* 9 */
					       nostdlib ? "-nostdlib" : null
				);

			si = new ProcessStartInfo (compiler, cargs) {
				UseShellExecute = false,
			};

			if (verbose)
				Console.WriteLine ("{0} {1}", si.FileName, si.Arguments);

			p = Process.Start (si);
			p.WaitForExit ();
			if (p.ExitCode != 0){
				Console.WriteLine ("{0}: API binding contains errors.", tool_name);
				return 1;
			}
		} finally {
			if (delete_temp)
				Directory.Delete (tmpdir, true);
		}
		return 0;
	}

	static string GetWorkDir ()
	{
		while (true){
			string p = Path.Combine (Path.GetTempPath(), Path.GetRandomFileName());
			if (Directory.Exists (p))
				continue;
			
			var di = Directory.CreateDirectory (p);
			return di.FullName;
		}
	}
}

