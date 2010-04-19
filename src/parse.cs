//
// Copyright 2010, Novell, Inc.
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

//
// NOT FOR USE IN PRODUCTION
// 
// THIS IS SAMPLE CODE, IT WILL HELP YOU GET STARTED, BUT NOT MUCH MORE THAN THAT
//
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

class SourceStream : Stream {
	Stream source;
	
	public SourceStream (Stream source)
	{
		this.source = source;
	}

	public override bool CanRead { get {return true; } }
	public override bool CanWrite { get { return false; } }
	public override bool CanSeek { get { return false; } }
	public override long Length { get { return source.Length; }}
	public override long Position { get { throw new Exception (); } set { throw new Exception (); }}
	public override void Flush () { throw new Exception (); }

	public override int Read (byte [] buf, int offset, int count)
	{
		int n = 0;
		
		for (int i = 0; i < count; i++){
			int c = ReadByte ();
			if (c == -1)
				return n;
			buf [offset + n] = (byte) c;
			n++;
		}
		return n;
	}

	public override long Seek (long p, SeekOrigin o)
	{
		throw new Exception ();
	}

	public override void SetLength (long l)
	{
		throw new Exception ();
	}

	public override void Write (byte [] b, int a, int c)
	{
		throw new Exception ();
	}
	
	public override int ReadByte ()
	{
	restart:
		int n = source.ReadByte ();
		if (n == -1)
			return -1;
		
		if (n == '/'){
			int p = source.ReadByte ();
			if (p == '/'){
				while (true) {
					n = source.ReadByte ();
					if (n == -1)
						return -1;
					if (n == '\n')
						return n;
				} 
			} else if (p == '*'){
				while (true){
					n = source.ReadByte ();
					if (n == -1)
						return -1;
					while (n == '*'){
						n = source.ReadByte ();
						if (n == -1)
							return -1;
						if (n == '/')
							goto restart;
					}
				}
			}
			source.Position = source.Position - 1;
			return '/';
		}
		return n;
	}
}

class Declaration {
	public string selector, retval, parameters;
	public bool is_abstract, is_static;
	
	public Declaration (string selector, string retval, string parameters, bool is_abstract, bool is_static)
	{
		this.selector = selector;
		this.retval = retval;
		this.parameters = parameters;
		this.is_abstract = is_abstract;
		this.is_static = is_static;
	}
	
}

class Declarations {
	List<Declaration> decls = new List<Declaration> ();
	StreamWriter gencs;
	
	public Declarations (StreamWriter gencs)
	{
		this.gencs = gencs;
	}

	public void Add (Declaration d)
	{
		if (d == null)
			return;
		
		decls.Add (d);
	}

	int Count (string s, char k)
	{
		int count = 0;
		foreach (char c in s)
			if (c == k)
				count++;
		return count;
	}

	string HasGetter (string getter1, string getter2)
	{
		if (HasGetter (getter1))
			return getter1;
		if (HasGetter (getter2))
			return getter2;
		return null;
	}

	bool HasGetter (string getter)
	{
		var found = (from d in decls
			let sel = d.selector
			where Count (sel, ':') == 0 && sel == getter
			     select d).FirstOrDefault ();
		return found != null;
	}

	bool Remove (string sel)
	{
		ignore.Add (sel);
		return true;
	}

	List<string> ignore = new List<string> ();
	
	public void Generate ()
	{
		var copy = decls;
		var properties = (from d in copy
				  let sel = d.selector
				  where sel.StartsWith ("set") && sel.EndsWith (":") && Count (sel, ':') == 1
				  let getter1 = Char.ToLower (sel [3]) + sel.Substring (4).Trim (':')
				  let getter2 = "is" + sel.Substring (3).Trim (':')
				  let getter = HasGetter (getter1, getter2)
				  where getter != null
				  let r = Remove (sel)
				  select getter).ToList ();
		
		foreach (var d in decls){
			if (ignore.Contains (d.selector) || properties.Contains (d.selector))
				continue;

			if (d.is_abstract)
				gencs.WriteLine ("\t\t[Abstract]");
			if (d.is_static)
				gencs.WriteLine ("\t\t[Static]");
			gencs.WriteLine ("\t\t[Export (\"{0}\")]", d.selector);
			gencs.WriteLine ("\t\t{0} {1} ({2});", d.retval, TrivialParser.AsMethod (TrivialParser.CleanSelector (d.selector)), d.parameters);
			gencs.WriteLine ();
		}

		if (properties.Count > 0)
			gencs.WriteLine ("\t\t//Detected properties");
		foreach (var d in properties){
			var decl = (from x in decls where x.selector == d select x).FirstOrDefault ();
			var sel = decl.selector;
			if (sel.StartsWith ("is"))
				sel = Char.ToLower (sel [2]) + sel.Substring (3);
			
			if (decl.is_abstract)
				gencs.WriteLine ("\t\t[Abstract]");
			if (decl.is_static)
				gencs.WriteLine ("\t\t[Static]");
			gencs.WriteLine ("\t\t[Export (\"{0}\")]", sel);
			gencs.WriteLine ("\t\t{0} {1} {{ {2}get; set; }}", decl.retval, TrivialParser.AsMethod (sel),
					 d.StartsWith ("is") ? "[Bind (\"" + d + "\")]" : "");
			gencs.WriteLine ();
		}
	}
}

class TrivialParser {
	StreamWriter gencs = File.CreateText ("gen.cs");
	StreamWriter other = File.CreateText ("other.c");
	StreamReader r;
	ArrayList types = new ArrayList ();
	
	void ProcessProperty (string line)
	{
		bool ro = false;
		string getter = null;
		
		line = CleanDeclaration (line);
		if (line.Length == 0)
			return;

		int p = line.IndexOf (')');
		var sub = line.Substring (0, p+1);
		if (sub.IndexOf ("readonly") != -1){
			ro = true;
		}
		int j = sub.IndexOf ("getter=");
		if (j != -1){
			int k = sub.IndexOfAny (new char [] { ',', ')'}, j +1);
			//Console.WriteLine ("j={0} k={1} str={2}", j, k, sub);
			getter = sub.Substring (j + 7, k-(j+7));
		}
		
		var type = new StringBuilder ();
		int i = p+1;
		for (; i < line.Length; i++){
			char c = line [i];
			if (!Char.IsWhiteSpace (c))
				break;
		}
		for (; i < line.Length; i++){
			char c = line [i];
			if (Char.IsWhiteSpace (c))
				break;
			type.Append (c);
		}
		
		for (; i < line.Length; i++){
			char c = line [i];
			if (Char.IsWhiteSpace (c) || c == '*')
				continue;
			else
				break;
		}
		var selector = new StringBuilder ();
		for (; i < line.Length; i++){
			char c = line [i];
			if (Char.IsWhiteSpace (c) || c == ';')
				break;
			selector.Append (c);
		}
		gencs.WriteLine ("\t\t[Export (\"{0}\")]", selector);
		gencs.WriteLine ("\t\t{0} {1} {{ {2} {3} }}",
				 type, selector,
				 getter != null ? "[Bind (\"" + getter + "\")] get;" : "get;",
				 ro ? "" : "set; ");
		gencs.WriteLine ();
	}

	public static string AsMethod (string msg)
	{
		return Char.ToUpper (msg [0]) + msg.Substring (1);
	}
	
	string MakeSelector (string sig)
	{
		StringBuilder sb = new StringBuilder ();
		for (int i = 0; i < sig.Length; i++){
			char c = sig [i];
			if (c == ' ')
				continue;
			if (c == ';')
				break;
			else if (c == ':'){
				sb.Append (c);
				i++;
				for (; i < sig.Length; i++){
					c = sig [i];
					if (c == ')'){
						for (++i; i < sig.Length; i++){
							if (!Char.IsLetterOrDigit (sig [i]))
								break;
						}
						break;
					}
				}
			} else
				sb.Append (c);
		}
		return sb.ToString ();
	}

	enum State {
		SkipToType,
		EndOfType,
		Parameter,
			
	}
	
	string MakeParameters (string sig)
	{
		//Console.WriteLine ("Making Parameters: [{0}]", sig);
		int colon = sig.IndexOf (':');
		if (colon == -1)
			return "";
		
		var sb = new StringBuilder ();
		var tsb = new StringBuilder ();
		State state = State.SkipToType;
		for (int i = 0; i < sig.Length; i++){
			char c = sig [i];

			switch (state){
			case State.SkipToType:
				if (Char.IsWhiteSpace (c))
					continue;
				if (c == '('){
					tsb = new StringBuilder ();
					state = State.EndOfType;
				}
				break;
			case State.EndOfType:
				if (c == ')'){
					state = State.Parameter;
					sb.Append (RemapType (tsb.ToString ()));
					sb.Append (' ');
				} else {
					if (c != '*')
						tsb.Append (c);
				}
				break;
				
			case State.Parameter:
				if (Char.IsWhiteSpace (c)){
					state = State.SkipToType;
					sb.Append (", ");
				} else {
					if (c != ';')
						sb.Append (c);
				}
				break;
			}
			
		}

		//Console.WriteLine ("  -> {0}", sb);
		return sb.ToString ();
	}

	string RemapType (string type)
	{
		if (type.EndsWith ("*"))
			type = type.Substring (0, type.Length-1);
		type = type.Trim ();
		switch (type){
		case "NSInteger":
			return "int";
		case "CGFloat":
			return "float";
		case "NSTextAlignment":
			return "uint";
			
		case "NSString":
		case "NSString *":
			return "string";
		case "NSSize":
			return "SizeF";
		case "NSRect":
			return "RectangleF";
		case "NSPoint":
			return "PointF";
		case "NSGlyph":
			return "uint";
		case "NSUInteger":
			return "uint";
		case "id":
			return "NSObject";
		case "BOOL":
			return "bool";
		case "SEL":
			return "Selector";
		case "NSURL":
			return "NSUrl";
		case "NSTimeInterval":
			return "double";
		}
		return type;
	}
	
	Regex rx = new Regex ("__OSX_AVAILABLE_STARTING\\(.*\\)");
	Regex rx2 = new Regex ("AVAILABLE_MAC_OS_X_VERSION[_A-Z0-9]*");
	
	string CleanDeclaration (string line)
	{
		return rx2.Replace (rx.Replace (line, ""), "");
	}

	public static string CleanSelector (string selector)
	{
		return selector.Replace (":", "");
	}
	
	Declaration ProcessDeclaration (bool isProtocol, string line, bool is_optional)
	{
		line = CleanDeclaration (line);
		if (line.Length == 0)
			return null;

		bool is_abstract = isProtocol && !is_optional;

		if (line.StartsWith ("@property")){
			if (is_abstract)
				gencs.WriteLine ("\t\t[Abstract]");

			ProcessProperty (line);
			return null;
		}
		//Console.WriteLine ("PROCESSING: {0}", line);
		bool is_static = line.StartsWith ("+");
		int p, q;
		p = line.IndexOf ('(');
		if (p == -1)
			return null;
		q = line.IndexOf (')');
		//Console.WriteLine ("->{0}\np={1} q-p={2}", line, p, q-p);
		string retval = RemapType (line.Substring (p+1, q-p-1));
		p = line.IndexOf (';');
		string signature = line.Substring (q+1, p-q);
		string selector = MakeSelector (signature);
		string parameters = MakeParameters (signature);

		//Console.WriteLine ("signature: {0}", signature);
		//Console.WriteLine ("selector: {0}", selector);
		return new Declaration (selector, retval, parameters, is_abstract, is_static);
	}
	
	void ProcessInterface (string iface)
	{
		var cols = iface.Split ();
		string line;

		//Console.WriteLine ("**** {0} ", iface);
		types.Add (cols [1]);
		if (cols.Length >= 4)
			gencs.WriteLine ("\n\t[BaseType (typeof ({0}))]", cols [3]);
		gencs.WriteLine ("\tinterface {0} {{", cols [1]);
		
		while ((line = r.ReadLine ()) != null && !line.StartsWith ("}"))
			;
			
		var decl = new Declarations (gencs);
		while ((line = r.ReadLine ()) != null && !line.StartsWith ("@end")){
			string full = "";
				
			while ((line = r.ReadLine ()) != null && !line.StartsWith ("@end")){
				full += line;
				if (full.IndexOf (';') != -1){
					full = full.Replace ('\n', ' ');
					decl.Add (ProcessDeclaration (false, full, false));
					full = "";
				}
			}
			break;
		}
		decl.Generate ();
		gencs.WriteLine ("\t}");
	}

	void ProcessProtocol (string proto)
	{
		string [] d = proto.Split (new char [] { ' ', '<', '>'});
		string line;

		types.Add (d [1]);
		gencs.WriteLine ("\n\t[BaseType (typeof ({0}))]", d.Length > 2 ? d [2] : "NSObject");
		gencs.WriteLine ("\t[Model]");
		gencs.WriteLine ("\tinterface {0} {{", d [1]);
		bool optional = false;
		
		var decl = new Declarations (gencs);
		while ((line = r.ReadLine ()) != null && !line.StartsWith ("@end")){
			if (line.StartsWith ("@optional"))
				optional = true;

			string full = "";
			while ((line = r.ReadLine ()) != null && !line.StartsWith ("@end")){
				full += line;
				if (full.IndexOf (';') != -1){
					full = full.Replace ('\n', ' ');
					decl.Add (ProcessDeclaration (true, full, optional));
					full = "";
				}
			}
			if (line.StartsWith ("@end"))
				break;
		}
		decl.Generate ();
		gencs.WriteLine ("\t}");
	}

	TrivialParser () {}

	void Run (string [] args)
	{
		foreach (string f in args){
			using (var fs = File.OpenRead (f)){
				r = new StreamReader (new SourceStream (fs));
				string line;
				while ((line = r.ReadLine ()) != null){
					if (line.StartsWith ("#"))
						continue;
					if (line.Length == 0)
						continue;
					if (line.StartsWith ("@class"))
						continue;

					if (line.StartsWith ("@interface"))
						ProcessInterface (line);
					if (line.StartsWith ("@protocol") && !line.EndsWith (";")) // && line.IndexOf ("<") != -1)
						ProcessProtocol (line);
					
					other.WriteLine (line);
				}
			}
		}
		foreach (string s in types){
			Console.WriteLine ("\t\ttypeof ({0}),", s);
		}
		gencs.Close ();
		other.Close ();
	}
	
	public static void Main (string [] args)
	{
		var tp = new TrivialParser ();
		tp.Run (args);
		
	}
}
