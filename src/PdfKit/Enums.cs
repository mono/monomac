//
// Copyright 2011, Novell, Inc.
// Copyright 2011, Regan Sarwas
//
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
// Enums.cs: Enums for PdfKit
//
using System;

namespace MonoMac.PdfKit {

	public enum PdfActionNamedName {
		None         = 0,
		NextPage     = 1,
		PreviousPage = 2,
		FirstPage    = 3,
		LastPage     = 4,
		GoBack       = 5,
		GoForward    = 6,
		GoToPage     = 7,
		Find         = 8,
		Print        = 9,
		ZoomIn       = 10,
		ZoomOut      = 11
	}

	public enum PdfWidgetControlType {
		Unknown    = -1,
		PushButton  = 0,
		RadioButton = 1,
		CheckBox    = 2
	}

	public enum PdfLineStyle {
		None        = 0,
		Square      = 1,
		Circle      = 2,
		Diamond     = 3,
		OpenArrow   = 4,
		ClosedArrow = 5
	}

	public enum PdfMarkupType {
		Highlight = 0,
		StrikeOut = 1,
		Underline = 2
	}

	public enum PdfTextAnnotationIconType {
		Comment      = 0,
		Key          = 1,
		Note         = 2,
		Help         = 3,
		NewParagraph = 4,
		Paragraph    = 5,
		Insert       = 6
	}

	public enum PdfBorderStyle {
		Solid     = 0,
		Dashed    = 1,
		Beveled   = 2,
		Inset     = 3,
		Underline = 4
	}

	public enum PdfPrintScalingMode {
		None      = 0,
		ToFit     = 1,
		DownToFit = 2
	}

	public enum PdfDocumentPermissions {
		None  = 0,
		User  = 1,
		Owner = 2
	}

	public enum PdfDisplayBox {
		Media = 0,
		Crop  = 1,
		Bleed = 2,
		Trim  = 3,
		Art   = 4
	}

	public enum PdfDisplayMode {
		SinglePage           = 0,
		SinglePageContinuous = 1,
		TwoUp                = 2,
		TwoUpContinuous      = 3
	}

	[Flags]
	public enum PdfAreaOfInterest {
		NoArea         = 0,
		PageArea       = 1 << 0,
		TextArea       = 1 << 1,
		AnnotationArea = 1 << 2,
		LinkArea       = 1 << 3,
		ControlArea    = 1 << 4,
		TextFieldArea  = 1 << 5,
		IconArea       = 1 << 6,
		PopupArea      = 1 << 7
	}
}
