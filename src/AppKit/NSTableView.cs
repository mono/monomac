//
// NSTableView.cs: Extensions to the API for NSTableView
//
// Copyright 2010, Novell, Inc.
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
using MonoMac.Foundation;

#if MAC64
using NSInteger = System.Int64;
using NSUInteger = System.UInt64;
using CGFloat = System.Double;
#else
using NSInteger = System.Int32;
using NSUInteger = System.UInt32;
using NSPoint = System.Drawing.PointF;
using NSSize = System.Drawing.SizeF;
using NSRect = System.Drawing.RectangleF;
using CGFloat = System.Single;
#endif


namespace MonoMac.AppKit {

	public partial class NSTableView {
		public NSTableViewSource Source {
			get {
				var d = WeakDelegate as NSTableViewSource;
                                if (d != null)
                                        return d;
                                return null;
			}

			set {
                                WeakDelegate = value;
                                WeakDataSource = value;
			}
		}

		public void SelectRow (NSUInteger row, bool byExtendingSelection)
		{
			SelectRows (NSIndexSet.FromIndex (row), byExtendingSelection);
		}

		public void SelectColumn (NSUInteger column, bool byExtendingSelection)
		{
			SelectColumns (NSIndexSet.FromIndex (column), byExtendingSelection);
		}
	}
}