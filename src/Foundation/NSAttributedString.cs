// 
// NSAttributedString.cs: Implements the managed NSAttributedString
//
// Authors: Marek Safar (marek.safar@gmail.com)
//     
// Copyright 2012, Xamarin Inc.
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
using MonoMac.CoreText;
using MonoMac.ObjCRuntime;
#if !MONOMAC
using MonoMac.UIKit;
#endif

#if MAC64
using nint = System.Int64;
using nuint = System.UInt64;
using nfloat = System.Double;
#else
using nint = System.Int32;
using nuint = System.UInt32;
using nfloat = System.Single;
#if SDCOMPAT
using CGPoint = System.Drawing.PointF;
using CGSize = System.Drawing.SizeF;
using CGRect = System.Drawing.RectangleF;
#endif
#endif

namespace MonoMac.Foundation {
	public partial class NSAttributedString {

		public NSAttributedString (string str, CTStringAttributes attributes)
			: this (str, attributes != null ? attributes.Dictionary : null)
		{
		}

        public string Value
        {
            get
            {
                return NSString.FromHandle(LowLevelValue);
            }
        }

        public NSDictionary GetAttributes(nint location, out NSRange effectiveRange)
        {
            return Runtime.GetNSObject<NSDictionary>(LowLevelGetAttributes(location, out effectiveRange));
        }

        public CTStringAttributes GetCoreTextAttributes (nint location, out NSRange effectiveRange)
		{
			var attr = GetAttributes (location, out effectiveRange);
			return attr == null ? null : new CTStringAttributes (attr);
		}

		public CTStringAttributes GetCoreTextAttributes (nint location, out NSRange longestEffectiveRange, NSRange rangeLimit)
		{
			var attr = GetAttributes (location, out longestEffectiveRange, rangeLimit);
			return attr == null ? null : new CTStringAttributes (attr);			
		}

		public NSAttributedString Substring (int start, int len)
		{
			return Substring (new NSRange (start, len));
		}

#if MONOMAC
        internal NSAttributedString(NSData data, NSAttributedStringDataType type, out NSDictionary resultDocumentAttributes)
        {
            switch (type)
            {
                case NSAttributedStringDataType.DocFormat:
                    Handle = new NSAttributedString(data, out resultDocumentAttributes).Handle;
                    break;
                case NSAttributedStringDataType.HTML:
                    Handle = InitWithHTML(data, out resultDocumentAttributes);
                    break;
                case NSAttributedStringDataType.RTF:
                    Handle = InitWithRtf(data, out resultDocumentAttributes);
                    break;
                case NSAttributedStringDataType.RTFD:
                    Handle = InitWithRtfd(data, out resultDocumentAttributes);
                    break;
                default:
                    throw new ArgumentException("Error creating NSAttributedString.");
            }

            if (Handle == IntPtr.Zero)
                throw new ArgumentException("Error creating NSAttributedString.");
        }

        public static NSAttributedString CreateWithRTF(NSData rtfData, out NSDictionary resultDocumentAttributes)
        {
            return new NSAttributedString(rtfData, NSAttributedStringDataType.RTF, out resultDocumentAttributes);
        }

        public static NSAttributedString CreateWithRTFD(NSData rtfdData, out NSDictionary resultDocumentAttributes)
        {
            return new NSAttributedString(rtfdData, NSAttributedStringDataType.RTFD, out resultDocumentAttributes);
        }

        public static NSAttributedString CreateWithHTML(NSData htmlData, out NSDictionary resultDocumentAttributes)
        {
            return new NSAttributedString(htmlData, NSAttributedStringDataType.HTML, out resultDocumentAttributes);
        }

        public static NSAttributedString CreateWithDocFormat(NSData wordDocFormat, out NSDictionary docAttributes)
        {
            return new NSAttributedString(wordDocFormat, NSAttributedStringDataType.DocFormat, out docAttributes);
        }

        //public NSStringAttributes GetAppKitAttributes(nint location, out NSRange effectiveRange)
        //{
        //    var attr = GetAttributes(location, out effectiveRange);
        //    return attr == null ? null : new NSStringAttributes(attr);
        //}

        //public NSStringAttributes GetAppKitAttributes(nint location, out NSRange longestEffectiveRange, NSRange rangeLimit)
        //{
        //    var attr = GetAttributes(location, out longestEffectiveRange, rangeLimit);
        //    return attr == null ? null : new NSStringAttributes(attr);
        //}
#endif

#if !MONOMAC
		public NSAttributedString (string str, UIStringAttributes attributes)
			: this (str, attributes != null ? attributes.Dictionary : null)
		{
		}

		public UIStringAttributes GetUIKitAttributes (int location, out NSRange effectiveRange)
		{
			var attr = GetAttributes (location, out effectiveRange);
			return attr == null ? null : new UIStringAttributes (attr);
		}

		public UIStringAttributes GetUIKitAttributes (int location, out NSRange longestEffectiveRange, NSRange rangeLimit)
		{
			var attr = GetAttributes (location, out longestEffectiveRange, rangeLimit);
			return attr == null ? null : new UIStringAttributes (attr);			
		}

		static internal NSDictionary ToDictionary (
						  UIFont font,
						  UIColor foregroundColor,
						  UIColor backgroundColor,
						  UIColor strokeColor,
						  NSParagraphStyle paragraphStyle,
						  NSLigatureType ligature,
						  float kerning,
						  NSUnderlineStyle underlineStyle,
						  NSShadow shadow,
						  float strokeWidth,
						  NSUnderlineStyle strikethroughStyle)
		{
			var attr = new UIStringAttributes ();
			if (font != null){
				attr.Font = font;
			}
			if (foregroundColor != null){
				attr.ForegroundColor = foregroundColor;
			}
			if (backgroundColor != null){
				attr.BackgroundColor = backgroundColor;
			}
			if (strokeColor != null){
				attr.StrokeColor = strokeColor;
			}
			if (paragraphStyle != null){
				attr.ParagraphStyle = paragraphStyle;
			}
			if (ligature != NSLigatureType.Default){
				attr.Ligature = ligature;
			}
			if (kerning != 0){
				attr.KerningAdjustment = kerning;
			}
			if (underlineStyle != NSUnderlineStyle.None){
				attr.UnderlineStyle = underlineStyle;
			}
			if (shadow != null){
				attr.Shadow = shadow;
			}
			if (strokeWidth != 0){
				attr.StrokeWidth = strokeWidth;
			}
			if (strikethroughStyle != NSUnderlineStyle.None){
				attr.StrikethroughStyle = strikethroughStyle;
			}
			var dict = attr.Dictionary;
			return dict.Count == 0 ? null : dict;
		}				

		public NSAttributedString (string str,
					   UIFont font = null,
					   UIColor foregroundColor = null,
					   UIColor backgroundColor = null,
					   UIColor strokeColor = null,
					   NSParagraphStyle paragraphStyle = null,
					   NSLigatureType ligatures = NSLigatureType.Default,
					   float kerning = 0,
					   NSUnderlineStyle underlineStyle = NSUnderlineStyle.None,
					   NSShadow shadow = null,
					   float strokeWidth = 0,
					   NSUnderlineStyle strikethroughStyle = NSUnderlineStyle.None)
		: this (str, ToDictionary (font, foregroundColor, backgroundColor, strokeColor, paragraphStyle, ligatures, kerning, underlineStyle, shadow, strokeWidth, strikethroughStyle))
		{
		}
#endif
    }
}
