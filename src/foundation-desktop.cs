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
// foundation-desktop.cs: Defines the API for the MonoMac specific
// Foundation APIs
// 
using System;
using System.Drawing;
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;

namespace MonoMac.Foundation {
	
	[BaseType (typeof (NSObject))]
	public interface NSAffineTransform {
		[Static]
		[Export ("transform")]
		NSAffineTransform Transform ();

		[Export ("initWithTransform:")]
		IntPtr Constructor (NSAffineTransform transform);

		[Export ("translateXBy:yBy:")]
		void Translate (float deltaX, float deltaY);

		[Export ("rotateByDegrees:")]
		void RotateByDegrees (float angle);

		[Export ("rotateByRadians:")]
		void RotateByRadians (float angle);

		[Export ("scaleBy:")]
		void Scale (float scale);

		[Export ("scaleXBy:yBy:")]
		void Scale (float scaleX, float scaleY);

		[Export ("invert")]
		void Invert ();

		[Export ("appendTransform:")]
		void AppendTransform (NSAffineTransform transform);

		[Export ("prependTransform:")]
		void PrependTransform (NSAffineTransform transform);

		[Export ("transformPoint:")]
		PointF TransformPoint (PointF aPoint);

		[Export ("transformSize:")]
		SizeF TransformSize (SizeF aSize);
		
		// FAK Leave this off for now as it requiers a forward ref to AppKit
		//[Export ("transformBezierPath:")]
		//NSBezierPath TransformBezierPath (NSBezierPath path);

		[Export ("set")]
		void Set ();

		[Export ("concat")]
		void Concat ();

		//Detected properties
		// FAK Left off until I understand how to do structs
		//[Export ("transformStruct")]
		//NSAffineTransformStruct TransformStruct { get; set; }
	}
	
	// FAK Left off until I understand how to do structs
	//struct NSAffineTransformStruct {
	//	public float M11, M12, M21, M22;
	//	public float tX, tY;
	//}

	[BaseType (typeof (NSCharacterSet))]
	public interface NSMutableCharacterSet {
		[Export ("removeCharactersInRange:")]
		void RemoveCharacters (NSRange aRange);

		[Export ("addCharactersInString:")]
		void AddCharacters (string aString);

		[Export ("removeCharactersInString:")]
		void RemoveCharacters (string aString);

		[Export ("formUnionWithCharacterSet:")]
		void UnionWith (NSCharacterSet otherSet);

		[Export ("formIntersectionWithCharacterSet:")]
		void IntersectWith (NSCharacterSet otherSet);

		[Export ("invert")]
		void Invert ();

	}


	[BaseType (typeof (NSObject))]
	public interface NSOrthography {
		[Export ("dominantScript")]
		NSString DominantScript { get;  }

		[Export ("languageMap")]
		NSDictionary LanguageMap { get;  }

		[Export ("dominantLanguage")]
		string DominantLanguage { get;  }

		[Export ("allScripts")]
		string [] AllScripts { get;  }

		[Export ("allLanguages")]
		string [] AllLanguages { get;  }

		[Export ("languagesForScript:")]
		string [] LanguagesForScript (string script);

		[Export ("dominantLanguageForScript:")]
		string GetDominantLanguageForScript (string forScript);

		[Export ("initWithDominantScript:languageMap:")]
		IntPtr Constructor (string script, NSDictionary map);

	}

	[BaseType (typeof (NSObject))]
	public interface NSTextCheckingResult {
		[Export ("resultType")]
		NSTextCheckingType ResultType { get;  }

		[Export ("range")]
		NSRange Range { get;  }

	}

	[BaseType (typeof (NSObject))]
	public interface NSAppleEventDescriptor {
		[Export ("nullDescriptor"), Static]
		NSAppleEventDescriptor Null { get; }
	}
}
