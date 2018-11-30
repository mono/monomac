//
// This file describes the API that the generator will produce
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//   Aaron Bockover
//
// Copyright 2009, Novell, Inc.
// Copyright 2010, Novell, Inc.
// Copyright 2011-2013 Xamarin Inc.
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
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.CoreMedia;

#if MONOMAC
using MonoMac.AppKit;
#else
using MonoMac.CoreLocation;
using MonoMac.UIKit;
#endif

using System;
using System.ComponentModel;

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


namespace MonoMac.Foundation
{
	public delegate nint NSComparator (NSObject obj1, NSObject obj2);
	public delegate void NSAttributedRangeCallback (NSDictionary attrs, NSRange range, ref bool stop);
	public delegate void NSAttributedStringCallback (NSObject value, NSRange range, ref bool stop);

	public delegate bool NSEnumerateErrorHandler (NSUrl url, NSError error);

	[BaseType (typeof (NSObject))]
	public interface NSArray {
		[Export ("count")]
		nuint Count { get; }

		[Export ("objectAtIndex:")]
		IntPtr ValueAt (nuint idx);

		[Export ("arrayWithObjects:count:")][Static][Internal]
		NSArray FromObjects (IntPtr array, nuint count);

		[Export ("valueForKey:")]
		[MarshalNativeExceptions]
		NSObject ValueForKey (NSString key);

		[Export ("setValue:forKey:")]
		void SetValueForKey (NSObject value, NSString key);

		[Export ("writeToFile:atomically:")]
		bool WriteToFile (string path, bool useAuxiliaryFile);

		[Export ("arrayWithContentsOfFile:")][Static]
		NSArray FromFile (string path);
		
		[Export ("sortedArrayUsingComparator:")]
		NSArray Sort (NSComparator cmptr);
		
		[Export ("filteredArrayUsingPredicate:")]
		NSArray Filter (NSPredicate predicate);
	}

	[Since (3,2)]
	[BaseType (typeof (NSObject))]
	public partial interface NSAttributedString {
		[Export ("string")]
		string Value { get; }

		[Export ("attributesAtIndex:effectiveRange:")]
		NSDictionary GetAttributes (nuint location, out NSRange effectiveRange);

		[Export ("length")]
		nuint Length { get; }

		// TODO: figure out the type, this deserves to be strongly typed if possble
		[Export ("attribute:atIndex:effectiveRange:")]
		NSObject GetAttribute (string attribute, nuint location, out NSRange effectiveRange);

		[Export ("attributedSubstringFromRange:"), Internal]
		NSAttributedString Substring (NSRange range);

		[Export ("attributesAtIndex:longestEffectiveRange:inRange:")]
		NSDictionary GetAttributes (nuint location, out NSRange longestEffectiveRange, NSRange rangeLimit);

		[Export ("attribute:atIndex:longestEffectiveRange:inRange:")]
		NSObject GetAttribute (string attribute, nuint location, out NSRange longestEffectiveRange, NSRange rangeLimit);

		[Export ("isEqualToAttributedString:")]
		bool IsEqual (NSAttributedString other);

		[Export ("initWithString:")]
		IntPtr Constructor (string str);
		
		[Export ("initWithString:attributes:")]
		[EditorBrowsable (EditorBrowsableState.Advanced)]
		IntPtr Constructor (string str, NSDictionary attributes);

		[Export ("initWithAttributedString:")]
		IntPtr Constructor (NSAttributedString other);

		[Export ("enumerateAttributesInRange:options:usingBlock:")]
		void EnumerateAttributes (NSRange range, NSAttributedStringEnumeration options, NSAttributedRangeCallback callback);

		[Export ("enumerateAttribute:inRange:options:usingBlock:")]
		void EnumerateAttribute (NSString attributeName, NSRange inRange, NSAttributedStringEnumeration options, NSAttributedStringCallback callback);

#if MONOMAC
		[Export("size")]
		CGSize Size { get; }

		[Field ("NSFontAttributeName", "AppKit")]
		NSString FontAttributeName { get; }

		[Field ("NSLinkAttributeName", "AppKit")]
		NSString LinkAttributeName { get; }

		[Field ("NSUnderlineStyleAttributeName", "AppKit")]
		NSString UnderlineStyleAttributeName { get; }

		[Field ("NSStrikethroughStyleAttributeName", "AppKit")]
		NSString StrikethroughStyleAttributeName { get; }

		[Field ("NSStrokeWidthAttributeName", "AppKit")]
		NSString StrokeWidthAttributeName { get; }

		[Field ("NSParagraphStyleAttributeName", "AppKit")]
		NSString ParagraphStyleAttributeName { get; }

		[Field ("NSForegroundColorAttributeName", "AppKit")]
		NSString ForegroundColorAttributeName { get; }

		[Field ("NSBackgroundColorAttributeName", "AppKit")]
		NSString BackgroundColorAttributeName { get; }

		[Field ("NSLigatureAttributeName", "AppKit")]
		NSString LigatureAttributeName { get; } 

		[Field ("NSObliquenessAttributeName", "AppKit")]
		NSString ObliquenessAttributeName { get; }

		[Field ("NSSuperscriptAttributeName", "AppKit")]
		NSString SuperscriptAttributeName { get; }

		[Field ("NSAttachmentAttributeName", "AppKit")]
		NSString AttachmentAttributeName { get; }
		
		[Field ("NSBaselineOffsetAttributeName", "AppKit")]
		NSString BaselineOffsetAttributeName { get; }
		
		[Field ("NSKernAttributeName", "AppKit")]
		NSString KernAttributeName { get; }
		
		[Field ("NSStrokeColorAttributeName", "AppKit")]
		NSString StrokeColorAttributeName { get; }
		
		[Field ("NSUnderlineColorAttributeName", "AppKit")]
		NSString UnderlineColorAttributeName { get; }
		
		[Field ("NSStrikethroughColorAttributeName", "AppKit")]
		NSString StrikethroughColorAttributeName { get; }
		
		[Field ("NSShadowAttributeName", "AppKit")]
		NSString ShadowAttributeName { get; }
		
		[Field ("NSExpansionAttributeName", "AppKit")]
		NSString ExpansionAttributeName { get; }
		
		[Field ("NSCursorAttributeName", "AppKit")]
		NSString CursorAttributeName { get; }
		
		[Field ("NSToolTipAttributeName", "AppKit")]
		NSString ToolTipAttributeName { get; }
		
		[Field ("NSMarkedClauseSegmentAttributeName", "AppKit")]
		NSString MarkedClauseSegmentAttributeName { get; }
		
		[Field ("NSWritingDirectionAttributeName", "AppKit")]
		NSString WritingDirectionAttributeName { get; }
		
		[Field ("NSVerticalGlyphFormAttributeName", "AppKit")]
		NSString VerticalGlyphFormAttributeName { get; }
		
		[Export ("initWithData:options:documentAttributes:error:")]
		IntPtr Constructor (NSData data, NSDictionary options, out NSDictionary docAttributes, out NSError error);

		[Export ("initWithDocFormat:documentAttributes:")]
		IntPtr Constructor(NSData wordDocFormat, out NSDictionary docAttributes);

		[Export ("initWithHTML:baseURL:documentAttributes:")]
		IntPtr Constructor (NSData htmlData, NSUrl baseUrl, out NSDictionary docAttributes);
		
		[Export ("drawAtPoint:")]
		void DrawString (CGPoint point);
		
		[Export ("drawInRect:")]
		void DrawString (CGRect rect);
		
		[Export ("drawWithRect:options:")]
		void DrawString (CGRect rect, NSStringDrawingOptions options);
		
#else
		[Since (6,0)]
		[Export ("size")]
		CGSize Size { get; }

		[Since (6,0)]
		[Export ("drawAtPoint:")]
		void DrawString (CGPoint point);

		[Since (6,0)]
		[Export ("drawInRect:")]
		void DrawString (CGRect rect);

		[Since (6,0)]
		[Export ("drawWithRect:options:context:")]
		void DrawString (CGRect rect, NSStringDrawingOptions options, [NullAllowed] NSStringDrawingContext context);

		[Since (6,0)]
		[Export ("boundingRectWithSize:options:context:")]
		CGRect GetBoundingRect (CGSize size, NSStringDrawingOptions options, [NullAllowed] NSStringDrawingContext context);
#endif
	}

	[BaseType (typeof (NSObject),
		   Delegates=new string [] { "WeakDelegate" },
		   Events=new Type [] { typeof (NSCacheDelegate)} )]
	[Since (4,0)]
	public interface NSCache {
		[Export ("objectForKey:")]
		NSObject ObjectForKey (NSObject key);

		[Export ("setObject:forKey:")]
		void SetObjectforKey (NSObject obj, NSObject key);

		[Export ("setObject:forKey:cost:")]
		void SetCost (NSObject obj, NSObject key, nuint cost);

		[Export ("removeObjectForKey:")]
		void RemoveObjectForKey (NSObject key);

		[Export ("removeAllObjects")]
		void RemoveAllObjects ();

		//Detected properties
		[Export ("name")]
		string Name { get; set; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSCacheDelegate Delegate { get; set; }

		[Export ("totalCostLimit")]
		nuint TotalCostLimit { get; set; }

		[Export ("countLimit")]
		nuint CountLimit { get; set; }

		[Export ("evictsObjectsWithDiscardedContent")]
		bool EvictsObjectsWithDiscardedContent { get; set; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	public interface NSCacheDelegate {
		[Export ("cache:willEvictObject:"), EventArgs ("NSObject")]
		void WillEvictObject (NSCache cache, NSObject obj);
	}

	[BaseType (typeof (NSObject), Name="NSCachedURLResponse")]
	// instance created with 'init' will crash when Dispose is called
	[DisableDefaultCtor]
	public interface NSCachedUrlResponse {
		[Export ("initWithResponse:data:userInfo:storagePolicy:")]
		IntPtr Constructor (NSUrlResponse response, NSData data, [NullAllowed] NSDictionary userInfo, NSUrlCacheStoragePolicy storagePolicy);

		[Export ("initWithResponse:data:")]
		IntPtr Constructor (NSUrlResponse response, NSData data);
          
		[Export ("response")]
		NSUrlResponse Response { get; }

		[Export ("data")]
		NSData Data { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; }

		[Export ("storagePolicy")]
		NSUrlCacheStoragePolicy StoragePolicy { get; }
	}
	
	[BaseType (typeof (NSObject))]
	// 'init' returns NIL
	[DisableDefaultCtor]
	public interface NSCalendar {
		[Export ("initWithCalendarIdentifier:")]
		IntPtr Constructor (NSString identifier);

		[Export ("calendarIdentifier")]
		string Identifier { get; }

		[Export ("currentCalendar")] [Static]
		NSCalendar CurrentCalendar { get; }

		[Export ("locale")]
		NSLocale Locale { get; set; }

		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; } 

		[Export ("firstWeekday")]
		nuint FirstWeekDay { get; set; } 

		[Export ("minimumDaysInFirstWeek")]
		nuint MinimumDaysInFirstWeek { get; set; }

		//- (NSRange)minimumRangeOfUnit:(NSCalendarUnit)unit;
		//- (NSRange)maximumRangeOfUnit:(NSCalendarUnit)unit;
		//- (NSRange)rangeOfUnit:(NSCalendarUnit)smaller inUnit:(NSCalendarUnit)larger forDate:(NSDate *)date;
		//- (nuint)ordinalityOfUnit:(NSCalendarUnit)smaller inUnit:(NSCalendarUnit)larger forDate:(NSDate *)date;
		//- (BOOL)rangeOfUnit:(NSCalendarUnit)unit startDate:(NSDate **)datep interval:(NSTimeInterval *)tip forDate:(NSDate *)date AVAILABLE_MAC_OS_X_VERSION_10_5_AND_LATER;

		[Export ("components:fromDate:")]
		NSDateComponents Components (NSCalendarUnit unitFlags, NSDate fromDate);

		[Export ("components:fromDate:toDate:options:")]
		NSDateComponents Components (NSCalendarUnit unitFlags, NSDate fromDate, NSDate toDate, NSDateComponentsWrappingBehavior opts);

		[Export ("dateByAddingComponents:toDate:options:")]
		NSDate DateByAddingComponents (NSDateComponents comps, NSDate date, NSDateComponentsWrappingBehavior opts);

		[Export ("dateFromComponents:")]
		NSDate DateFromComponents (NSDateComponents comps);

		[Field ("NSGregorianCalendar"), Internal]
		NSString NSGregorianCalendar { get; }
		
		[Field ("NSBuddhistCalendar"), Internal]
		NSString NSBuddhistCalendar { get; }
		
		[Field ("NSChineseCalendar"), Internal]
		NSString NSChineseCalendar { get; }
		
		[Field ("NSHebrewCalendar"), Internal]
		NSString NSHebrewCalendar { get; }
		
		[Field ("NSIslamicCalendar"), Internal]
		NSString NSIslamicCalendar { get; }
		
		[Field ("NSIslamicCivilCalendar"), Internal]
		NSString NSIslamicCivilCalendar { get; }
		
		[Field ("NSJapaneseCalendar"), Internal]
		NSString NSJapaneseCalendar { get; }
		
		[Field ("NSRepublicOfChinaCalendar"), Internal]
		NSString NSRepublicOfChinaCalendar { get; }
		
		[Field ("NSPersianCalendar"), Internal]
		NSString NSPersianCalendar { get; }
		
		[Field ("NSIndianCalendar"), Internal]
		NSString NSIndianCalendar { get; }
		
		[Field ("NSISO8601Calendar"), Internal]
		NSString NSISO8601Calendar { get; }
	}

	[Since (3,2)]
	[BaseType (typeof (NSObject))]
	public interface NSCharacterSet {
		[Static, Export ("alphanumericCharacterSet")]
		NSCharacterSet Alphanumerics {get;}

		[Static, Export ("capitalizedLetterCharacterSet")]
		NSCharacterSet Capitalized {get;}

		// TODO/FIXME: constructor?
		[Static, Export ("characterSetWithBitmapRepresentation:")]
		NSCharacterSet FromBitmap (NSData data);

		// TODO/FIXME: constructor?
		[Static, Export ("characterSetWithCharactersInString:")]
		NSCharacterSet FromString (string aString);

		[Static, Export ("characterSetWithContentsOfFile:")]
		NSCharacterSet FromFile (string path);

		[Static, Export ("characterSetWithRange:")]
		NSCharacterSet FromRange (NSRange aRange);

		[Static, Export ("controlCharacterSet")]
		NSCharacterSet Controls {get;}

		[Static, Export ("decimalDigitCharacterSet")]
		NSCharacterSet DecimalDigits {get;}

		[Static, Export ("decomposableCharacterSet")]
		NSCharacterSet Decomposables {get;}

		[Static, Export ("illegalCharacterSet")]
		NSCharacterSet Illegals {get;}

		[Static, Export ("letterCharacterSet")]
		NSCharacterSet Letters {get;}

		[Static, Export ("lowercaseLetterCharacterSet")]
		NSCharacterSet LowercaseLetters {get;}

		[Static, Export ("newlineCharacterSet")]
		NSCharacterSet Newlines {get;}

		[Static, Export ("nonBaseCharacterSet")]
		NSCharacterSet Marks {get;}

		[Static, Export ("punctuationCharacterSet")]
		NSCharacterSet Punctuation {get;}

		[Static, Export ("symbolCharacterSet")]
		NSCharacterSet Symbols {get;}

		[Static, Export ("uppercaseLetterCharacterSet")]
		NSCharacterSet UppercaseLetters {get;}

		[Static, Export ("whitespaceAndNewlineCharacterSet")]
		NSCharacterSet WhitespaceAndNewlines {get;}

		[Static, Export ("whitespaceCharacterSet")]
		NSCharacterSet Whitespaces {get;}

		[Export ("bitmapRepresentation")]
		NSData GetBitmapRepresentation ();

		[Export ("characterIsMember:")]
		bool Contains (char aCharacter);

		[Export ("hasMemberInPlane:")]
		bool HasMemberInPlane (byte thePlane);

		[Export ("invertedSet")]
		NSCharacterSet InvertedSet {get;}

		[Export ("isSupersetOfSet:")]
		bool IsSupersetOf (NSCharacterSet theOtherSet);

		[Export ("longCharacterIsMember:")]
		bool Contains (uint theLongChar);
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSCoder {

		//
		// Encoding and decoding
		//
		[Export ("encodeObject:")]
		void Encode (NSObject obj);
		
		[Export ("encodeRootObject:")]
		void EncodeRoot (NSObject obj);

		[Export ("decodeObject")]
		NSObject DecodeObject ();

		//
		// Encoding and decoding with keys
		// 
		[Export ("encodeConditionalObject:forKey:")]
		void EncodeConditionalObject (NSObject val, string key);
		
		[Export ("encodeObject:forKey:")]
		void Encode (NSObject val, string key);
		
		[Export ("encodeBool:forKey:")]
		void Encode (bool val, string key);
		
		[Export ("encodeDouble:forKey:")]
		void Encode (double val, string key);
		
		[Export ("encodeFloat:forKey:")]
		void Encode (float val, string key);
		
		[Export ("encodeInt32:forKey:")]
		void Encode (int val, string key);
		
		[Export ("encodeInt64:forKey:")]
		void Encode (long val, string key);
		
		[Export ("encodeBytes:length:forKey:")]
		void EncodeBlock (IntPtr bytes, int length, string key);

		[Export ("containsValueForKey:")]
		bool ContainsKey (string key);
		
		[Export ("decodeBoolForKey:")]
		bool DecodeBool (string key);

		[Export ("decodeDoubleForKey:")]
		double DecodeDouble (string key);

		[Export ("decodeFloatForKey:")]
		float DecodeFloat (string key);

		[Export ("decodeInt32ForKey:")]
		int DecodeInt (string key);

		[Export ("decodeInt64ForKey:")]
		long DecodeLong (string key);

		[Export ("decodeObjectForKey:")]
		NSObject DecodeObject (string key);

		[Internal, Export ("decodeBytesForKey:returnedLength:")]
		IntPtr DecodeBytes (string key, IntPtr length_ptr);

		[Since (6,0)]
		[Export ("allowedClasses")]
		NSSet AllowedClasses { get; }

		[Since (6,0)]
		[Export ("requiresSecureCoding")]
		bool RequiresSecureCoding ();

		
	}
	
	[BaseType (typeof (NSPredicate))]
	public interface NSComparisonPredicate {
		[Static, Export ("predicateWithLeftExpression:rightExpression:modifier:type:options:")]
		NSPredicate Create (NSExpression leftExpression, NSExpression rightExpression, NSComparisonPredicateModifier comparisonModifier, NSPredicateOperatorType operatorType, NSComparisonPredicateOptions comparisonOptions);

		[Static, Export ("predicateWithLeftExpression:rightExpression:customSelector:")]
		NSPredicate FromSelector (NSExpression leftExpression, NSExpression rightExpression, Selector selector);

		[Export ("initWithLeftExpression:rightExpression:modifier:type:options:")]
		IntPtr Constructor (NSExpression leftExpression, NSExpression rightExpression, NSComparisonPredicateModifier comparisonModifier, NSPredicateOperatorType operatorType, NSComparisonPredicateOptions comparisonOptions);
		
		[Export ("initWithLeftExpression:rightExpression:customSelector:")]
		IntPtr Constructor (NSExpression leftExpression, NSExpression rightExpression, Selector selector);

		[Export ("predicateOperatorType")]
		NSPredicateOperatorType PredicateOperatorType { get; }

		[Export ("comparisonPredicateModifier")]
		NSComparisonPredicateModifier ComparisonPredicateModifier { get; }

		[Export ("leftExpression")]
		NSExpression LeftExpression { get; }

		[Export ("rightExpression")]
		NSExpression RightExpression { get; }

		[Export ("customSelector")]
		Selector CustomSelector { get; }

		[Export ("options")]
		NSComparisonPredicateOptions Options { get; }
	}

	[BaseType (typeof (NSPredicate))]
	[DisableDefaultCtor] // An uncaught exception was raised: Can't have a NOT predicate with no subpredicate.
	public interface NSCompoundPredicate {
		[Export ("initWithType:subpredicates:")]
		IntPtr Constructor (NSCompoundPredicateType type, NSPredicate[] subpredicates);

		[Export ("compoundPredicateType")]
		NSCompoundPredicateType Type { get; }

		[Export ("subpredicates")]
		NSPredicate[] Subpredicates { get; } 

		[Static]
		[Export ("andPredicateWithSubpredicates:")]
		NSPredicate CreateAndPredicate (NSPredicate[] subpredicates);

		[Static]
		[Export ("orPredicateWithSubpredicates:")]
		NSPredicate CreateOrPredicate (NSPredicate [] subpredicates);

		[Static]
		[Export ("notPredicateWithSubpredicate:")]
		NSPredicate CreateNotPredicate (NSPredicate predicate);

	}

	[BaseType (typeof (NSObject))]
	public interface NSData {
		[Export ("dataWithContentsOfURL:")]
		[Static]
		NSData FromUrl (NSUrl url);

		[Export ("dataWithContentsOfURL:options:error:")]
		[Static]
		NSData FromUrl (NSUrl url, NSDataReadingOptions mask, out NSError error);

		[Export ("dataWithContentsOfFile:")][Static]
		NSData FromFile (string path);
		
		[Export ("dataWithContentsOfFile:options:error:")]
		[Static]
		NSData FromFile (string path, NSDataReadingOptions mask, out NSError error);

		[Export ("dataWithData:")]
		[Static]
		NSData FromData (NSData source);

		[Export ("dataWithBytes:length:"), Static]
		NSData FromBytes (IntPtr bytes, nuint size);

		[Export ("bytes")]
		IntPtr Bytes { get; }

		[Export ("length")]
		nuint Length { get; [NotImplemented] set; }

		[Export ("writeToFile:options:error:")]
		bool _Save (string file, nuint options, IntPtr addr);
		
		[Export ("writeToURL:options:error:")]
		bool _Save (NSUrl url, nuint options, IntPtr addr);

		[Export ("rangeOfData:options:range:")]
		[Since (4,0)]
		NSRange Find (NSData dataToFind, NSDataSearchOptions searchOptions, NSRange searchRange);
	}

	[BaseType (typeof (NSObject))]
	public interface NSDateComponents {
		[Since (4,0)]
		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; }

		[Export ("calendar")]
		[Since (4,0)]
		NSCalendar Calendar { get; set; }

		[Export ("quarter")]
		[Since (4,0)]
		nint Quarter { get; set; }

		[Export ("date")]
		[Since (4,0)]
		NSDate Date { get; }

		//Detected properties
		[Export ("era")]
		nint Era { get; set; }

		[Export ("year")]
		nint Year { get; set; }

		[Export ("month")]
		nint Month { get; set; }

		[Export ("day")]
		nint Day { get; set; }

		[Export ("hour")]
		nint Hour { get; set; }

		[Export ("minute")]
		nint Minute { get; set; }

		[Export ("second")]
		nint Second { get; set; }

		[Export ("week")]
		nint Week { get; set; }

		[Export ("weekday")]
		nint Weekday { get; set; }

		[Export ("weekdayOrdinal")]
		nint WeekdayOrdinal { get; set; }

		[Since (5,0)]
		[Export ("weekOfMonth")]
		nint WeekOfMonth { get; set; }

		[Since (5,0)]
		[Export ("weekOfYear")]
		nint WeekOfYear { get; set; }
		
		[Since (5,0)]
		[Export ("yearForWeekOfYear")]
		nint YearForWeekOfYear { get; set; }

		[Since (6,0)]
		[Export ("leapMonth")]
		bool IsLeapMonth { [Bind ("isLeapMonth")] get; set; }
	}
	
	[Since (6,0)]
	[BaseType (typeof (NSFormatter))]
	interface NSByteCountFormatter {
		[Export ("allowsNonnumericFormatting")]
		bool AllowsNonnumericFormatting { get; set; }

		[Export ("includesUnit")]
		bool IncludesUnit { get; set; }

		[Export ("includesCount")]
		bool IncludesCount { get; set; }

		[Export ("includesActualByteCount")]
		bool IncludesActualByteCount { get; set; }
		
		[Export ("adaptive")]
		bool Adaptive { [Bind ("isAdaptive")] get; set;  }

		[Export ("zeroPadsFractionDigits")]
		bool ZeroPadsFractionDigits { get; set;  }

		[Static]
		[Export ("stringFromByteCount:countStyle:")]
		string Format (long byteCount, NSByteCountFormatterCountStyle countStyle);

		[Export ("stringFromByteCount:")]
		string Format (long byteCount);

		[Export ("allowedUnits")]
		NSByteCountFormatterUnits AllowedUnits { get; set; }

		[Export ("countStyle")]
		NSByteCountFormatterCountStyle CountStyle { get; set; }
	}

	[BaseType (typeof (NSFormatter))]
	public interface NSDateFormatter {
		[Export ("stringFromDate:")]
		string ToString (NSDate date);

		[Export ("dateFromString:")]
		NSDate Parse (string date);

		[Export ("dateFormat")]
		string DateFormat { get; set; }

		[Export ("dateStyle")]
		NSDateFormatterStyle DateStyle { get; set; }

		[Export ("timeStyle")]
		NSDateFormatterStyle TimeStyle { get; set; }

		[Export ("locale")]
		NSLocale Locale { get; set; }

		[Export ("generatesCalendarDates")]
		bool GeneratesCalendarDates { get; set; }

		[Export ("formatterBehavior")]
		NSDateFormatterBehavior Behavior { get; set; }

		[Export ("defaultFormatterBehavior"), Static]
		NSDateFormatterBehavior DefaultBehavior { get; set; }

		[Export ("timeZone")]
		NSTimeZone TimeZone { get; set; }

		[Export ("calendar")]
		NSCalendar Calendar { get; set; }

		// not exposed as a property in documentation
		[Export ("isLenient")]
		bool IsLenient { get; [Bind ("setLenient:")] set; } 

		[Export ("twoDigitStartDate")]
		NSDate TwoDigitStartDate { get; set; }

		[Export ("defaultDate")]
		NSDate DefaultDate { get; set; }

		[Export ("eraSymbols")]
		string [] EraSymbols { get; set; }

		[Export ("monthSymbols")]
		string [] MonthSymbols { get; set; }

		[Export ("shortMonthSymbols")]
		string [] ShortMonthSymbols { get; set; }

		[Export ("weekdaySymbols")]
		string [] WeekdaySymbols { get; set; }

		[Export ("shortWeekdaySymbols")]
		string [] ShortWeekdaySymbols { get; set; } 

		[Export ("AMSymbol")]
		string AMSymbol { get; set; }

		[Export ("PMSymbol")]
		string PMSymbol { get; set; }

		[Export ("longEraSymbols")]
		string [] LongEraSymbols { get; set; }

		[Export ("veryShortMonthSymbols")]
		string [] VeryShortMonthSymbols { get; set; }
		
		[Export ("standaloneMonthSymbols")]
		string [] StandaloneMonthSymbols { get; set; }

		[Export ("shortStandaloneMonthSymbols")]
		string [] ShortStandaloneMonthSymbols { get; set; }

		[Export ("veryShortStandaloneMonthSymbols")]
		string [] VeryShortStandaloneMonthSymbols { get; set; }
		
		[Export ("veryShortWeekdaySymbols")]
		string [] VeryShortWeekdaySymbols { get; set; }

		[Export ("standaloneWeekdaySymbols")]
		string [] StandaloneWeekdaySymbols { get; set; }

		[Export ("shortStandaloneWeekdaySymbols")]
		string [] ShortStandaloneWeekdaySymbols { get; set; }
		
		[Export ("veryShortStandaloneWeekdaySymbols")]
		string [] VeryShortStandaloneWeekdaySymbols { get; set; }
		
		[Export ("quarterSymbols")]
		string [] QuarterSymbols { get; set; }

		[Export ("shortQuarterSymbols")]
		string [] ShortQuarterSymbols { get; set; }
		
		[Export ("standaloneQuarterSymbols")]
		string [] StandaloneQuarterSymbols { get; set; }

		[Export ("shortStandaloneQuarterSymbols")]
		string [] ShortStandaloneQuarterSymbols { get; set; }

		[Export ("gregorianStartDate")]
		NSDate GregorianStartDate { get; set; }
	}

	public delegate void NSFileHandleUpdateHandler (NSFileHandle handle);

	public interface NSFileHandleReadEventArgs {
		[Export ("NSFileHandleNotificationDataItem")]
		NSData AvailableData { get; }

		[Export ("NSFileHandleError", ArgumentSemantic.Assign)]
		int UnixErrorCode { get; }
	}

	public interface NSFileHandleConnectionAcceptedEventArgs {
		[Export ("NSFileHandleNotificationFileHandleItem")]
		NSFileHandle NearSocketConnection { get; }
		
		[Export ("NSFileHandleError", ArgumentSemantic.Assign)]
		int UnixErrorCode { get; }
	}
	
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // return invalid handle
	public interface NSFileHandle 
	{
		[Export ("availableData")]
		NSData AvailableData ();
		
		[Export ("readDataToEndOfFile")]
		NSData ReadDataToEndOfFile ();

		[Export ("readDataOfLength:")]
		NSData ReadDataOfLength (nuint length);

		[Export ("writeData:")]
		void WriteData (NSData data);

		[Export ("offsetInFile")]
		ulong OffsetInFile ();

		[Export ("seekToEndOfFile")]
		ulong SeekToEndOfFile ();

		[Export ("seekToFileOffset:")]
		void SeekToFileOffset (ulong offset);

		[Export ("truncateFileAtOffset:")]
		void TruncateFileAtOffset (ulong offset);

		[Export ("synchronizeFile")]
		void SynchronizeFile ();

		[Export ("closeFile")]
		void CloseFile ();
		
		[Static]
		[Export ("fileHandleWithStandardInput")]
		NSFileHandle FromStandardInput ();
		
		[Static]
		[Export ("fileHandleWithStandardOutput")]
		NSFileHandle FromStandardOutput ();

		[Static]
		[Export ("fileHandleWithStandardError")]
		NSFileHandle FromStandardError ();

		[Static]
		[Export ("fileHandleWithNullDevice")]
		NSFileHandle FromNullDevice ();

		[Static]
		[Export ("fileHandleForReadingAtPath:")]
		NSFileHandle OpenRead (string path);

		[Static]
		[Export ("fileHandleForWritingAtPath:")]
		NSFileHandle OpenWrite (string path);

		[Static]
		[Export ("fileHandleForUpdatingAtPath:")]
		NSFileHandle OpenUpdate (string path);

		[Static]
		[Export ("fileHandleForReadingFromURL:error:")]
		NSFileHandle OpenReadUrl (NSUrl url, out NSError error);

		[Static]
		[Export ("fileHandleForWritingToURL:error:")]
		NSFileHandle OpenWriteUrl (NSUrl url, out NSError error);

		[Static]
		[Export ("fileHandleForUpdatingURL:error:")]
		NSFileHandle OpenUpdateUrl (NSUrl url, out NSError error);
		
		[Export ("readInBackgroundAndNotifyForModes:")]
		void ReadInBackground (NSString [] notifyRunLoopModes);
		
		[Export ("readInBackgroundAndNotify")]
		void ReadInBackground ();

		[Export ("readToEndOfFileInBackgroundAndNotifyForModes:")]
		void ReadToEndOfFileInBackground (NSString [] notifyRunLoopModes);

		[Export ("readToEndOfFileInBackgroundAndNotify")]
		void ReadToEndOfFileInBackground ();

		[Export ("acceptConnectionInBackgroundAndNotifyForModes:")]
		void AcceptConnectionInBackground (NSString [] notifyRunLoopModes);

		[Export ("acceptConnectionInBackgroundAndNotify")]
		void AcceptConnectionInBackground ();

		[Export ("waitForDataInBackgroundAndNotifyForModes:")]
		void WaitForDataInBackground (NSString [] notifyRunLoopModes);

		[Export ("waitForDataInBackgroundAndNotify")]
		void WaitForDataInBackground ();
		
		[Export ("initWithFileDescriptor:closeOnDealloc:")]
		IntPtr Constructor (int fd, bool closeOnDealloc);
		
		[Export ("initWithFileDescriptor:")]
		IntPtr Constructor (int fd);

		[Export ("fileDescriptor")]
		int FileDescriptor { get; }

		[Export ("setReadabilityHandler:")]
		void SetReadabilityHandler ([NullAllowed] NSFileHandleUpdateHandler readCallback);

		[Export ("setWriteabilityHandler:")]
		void SetWriteabilityHandle ([NullAllowed] NSFileHandleUpdateHandler writeCallback);

		[Field ("NSFileHandleOperationException")]
		NSString OperationException { get; }

		[Field ("NSFileHandleReadCompletionNotification")]
		[Notification (typeof (NSFileHandleReadEventArgs))]
		NSString ReadCompletionNotification { get; }
		
		[Field ("NSFileHandleReadToEndOfFileCompletionNotification")]
		[Notification (typeof (NSFileHandleReadEventArgs))]
		NSString ReadToEndOfFileCompletionNotification { get; }
		
		[Field ("NSFileHandleConnectionAcceptedNotification")]
		[Notification (typeof (NSFileHandleConnectionAcceptedEventArgs))]
		NSString ConnectionAcceptedNotification { get; }

		
		[Field ("NSFileHandleDataAvailableNotification")]
		[Notification]
		NSString DataAvailableNotification { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSPipe {
		
		[Export ("fileHandleForReading")]
		NSFileHandle ReadHandle { get; }
		
		[Export ("fileHandleForWriting")]
		NSFileHandle WriteHandle { get; }

		[Static]
		[Export ("pipe")]
		NSPipe Create ();
	}

	//@interface NSFormatter : NSObject <NSCopying, NSCoding>
	[BaseType (typeof (NSObject))]
	public interface NSFormatter {
		[Export ("stringForObjectValue:")]
		string StringFor ( [NullAllowed] NSObject value);

		// - (NSAttributedString *)attributedStringForObjectValue:(id)obj withDefaultAttributes:(NSDictionary *)attrs;

		[Export ("editingStringForObjectValue:")]
		string EditingStringFor (NSObject value);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	public interface NSKeyedArchiverDelegate {
		[Export ("archiver:didEncodeObject:"), EventArgs ("NSObject")]
		void EncodedObject (NSKeyedArchiver archiver, NSObject obj);
		
		[Export ("archiverDidFinish:")]
		void Finished (NSKeyedArchiver archiver);
		
		[Export ("archiver:willEncodeObject:"), DelegateName ("NSEncodeHook"), DefaultValue (null)]
		NSObject WillEncode (NSKeyedArchiver archiver, NSObject obj);
		
		[Export ("archiverWillFinish:")]
		void Finishing (NSKeyedArchiver archiver);
		
		[Export ("archiver:willReplaceObject:withObject:"), EventArgs ("NSArchiveReplace")]
		void ReplacingObject (NSKeyedArchiver archiver, NSObject oldObject, NSObject newObject);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	public interface NSKeyedUnarchiverDelegate {
		[Export ("unarchiver:didDecodeObject:"), DelegateName ("NSDecoderCallback"), DefaultValue (null)]
		NSObject DecodedObject (NSKeyedUnarchiver unarchiver, NSObject obj);
		
		[Export ("unarchiverDidFinish:")]
		void Finished (NSKeyedUnarchiver unarchiver);
		
		[Export ("unarchiver:cannotDecodeObjectOfClassName:originalClasses:"), DelegateName ("NSDecoderHandler"), DefaultValue (null)]
		Class CannotDecodeClass (NSKeyedUnarchiver unarchiver, string klass, string [] classes);
		
		[Export ("unarchiverWillFinish:")]
		void Finishing (NSKeyedUnarchiver unarchiver);
		
		[Export ("unarchiver:willReplaceObject:withObject:"), EventArgs ("NSArchiveReplace")]
		void ReplacingObject (NSKeyedUnarchiver unarchiver, NSObject oldObject, NSObject newObject);
	}

	[BaseType (typeof (NSCoder),
		   Delegates=new string [] {"WeakDelegate"},
		   Events=new Type [] { typeof (NSKeyedArchiverDelegate) })]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -[NSKeyedArchiver init]: cannot use -init for initialization
	[DisableDefaultCtor]
	public interface NSKeyedArchiver {
		[Export ("initForWritingWithMutableData:")]
		IntPtr Constructor (NSMutableData data);
	
		[Export ("archivedDataWithRootObject:")]
		[Static]
		NSData ArchivedDataWithRootObject (NSObject root);
		
		[Export ("archiveRootObject:toFile:")]
		[Static]
		bool ArchiveRootObjectToFile (NSObject root, string file);

		[Export ("finishEncoding")]
		void FinishEncoding ();

		[Export ("outputFormat")]
		NSPropertyListFormat PropertyListFormat { get; set; }

		[Wrap ("WeakDelegate")]
		NSKeyedArchiverDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Export ("setClassName:forClass:")]
		void SetClassName (string name, Class kls);

		[Export ("classNameForClass:")]
		string GetClassName (Class kls);
	}
	
	[BaseType (typeof (NSCoder),
		   Delegates=new string [] {"WeakDelegate"},
		   Events=new Type [] { typeof (NSKeyedUnarchiverDelegate) })]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -[NSKeyedUnarchiver init]: cannot use -init for initialization
	[DisableDefaultCtor]
	public interface NSKeyedUnarchiver {
		[Export ("initForReadingWithData:")]
		[MarshalNativeExceptions]
		IntPtr Constructor (NSData data);
	
		[Static, Export ("unarchiveObjectWithData:")]
		[MarshalNativeExceptions]
		NSObject UnarchiveObject (NSData data);
		
		[Static, Export ("unarchiveObjectWithFile:")]
		[MarshalNativeExceptions]
		NSObject UnarchiveFile (string file);

		[Export ("finishDecoding")]
		void FinishDecoding ();

		[Wrap ("WeakDelegate")]
		NSKeyedUnarchiverDelegate Delegate { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Export ("setClass:forClassName:")]
		void SetClass (Class kls, string codedName);

		[Export ("classForClassName:")]
		Class GetClass (string codedName);
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSMetadataQueryDelegate)})]
	public interface NSMetadataQuery {
		[Export ("startQuery")]
		bool StartQuery ();

		[Export ("stopQuery")]
		void StopQuery ();

		[Export ("isStarted")]
		bool IsStarted { get; }

		[Export ("isGathering")]
		bool IsGathering { get; }

		[Export ("isStopped")]
		bool IsStopped { get; }

		[Export ("disableUpdates")]
		void DisableUpdates ();

		[Export ("enableUpdates")]
		void EnableUpdates ();

		[Export ("resultCount")]
		nuint ResultCount { get; }

		[Export ("resultAtIndex:")]
		NSObject ResultAtIndex (nuint idx);

		[Export ("results")]
		NSMetadataItem[] Results { get; }

		[Export ("indexOfResult:")]
		nuint IndexOfResult (NSObject result);

		[Export ("valueLists")]
		NSDictionary ValueLists { get; }

		[Export ("groupedResults")]
		NSObject [] GroupedResults { get; }

		[Export ("valueOfAttribute:forResultAtIndex:")]
		NSObject ValueOfAttribute (string attribyteName, nuint atIndex);

		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSMetadataQueryDelegate WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSMetadataQueryDelegate Delegate { get; set; }
		
		[Export ("predicate")]
		NSPredicate Predicate { get; set; }

		[Export ("sortDescriptors")]
		NSSortDescriptor[] SortDescriptors { get; set; }

		[Export ("valueListAttributes")]
		NSObject[] ValueListAttributes { get; set; }

		[Export ("groupingAttributes")]
		NSArray GroupingAttributes { get; set; }

		[Export ("notificationBatchingInterval")]
		double NotificationBatchingInterval { get; set; }

		[Export ("searchScopes")]
		NSObject [] SearchScopes { get; set; }
		
		// There is no info associated with these notifications
		[Field ("NSMetadataQueryDidStartGatheringNotification")]
		[Notification]
		NSString DidStartGatheringNotification { get; }
	
		[Field ("NSMetadataQueryGatheringProgressNotification")]
		[Notification]
		NSString GatheringProgressNotification { get; }
		
		[Field ("NSMetadataQueryDidFinishGatheringNotification")]
		[Notification]
		NSString DidFinishGatheringNotification { get; }
		
		[Field ("NSMetadataQueryDidUpdateNotification")]
		[Notification]
		NSString DidUpdateNotification { get; }
		
		[Field ("NSMetadataQueryResultContentRelevanceAttribute")]
		NSString ResultContentRelevanceAttribute { get; }
		
		// Scope constants for defined search locations
#if MONOMAC
		[Field ("NSMetadataQueryUserHomeScope")]
		NSString UserHomeScope { get; }
		
		[Field ("NSMetadataQueryLocalComputerScope")]
		NSString LocalComputerScope { get; }
		
		[Field ("NSMetadataQueryNetworkScope")]
		NSString QueryNetworkScope { get; }

		[Field ("NSMetadataQueryLocalDocumentsScope")]
		NSString QueryLocalDocumentsScope { get; }
#endif
		[Field ("NSMetadataQueryUbiquitousDocumentsScope")]
		NSString QueryUbiquitousDocumentsScope { get; }

		[Field ("NSMetadataQueryUbiquitousDataScope")]
		NSString QueryUbiquitousDataScope { get; }

		[Field ("NSMetadataItemFSNameKey")]
		NSString ItemFSNameKey { get; }

		[Field ("NSMetadataItemDisplayNameKey")]
		NSString ItemDisplayNameKey { get; }

		[Field ("NSMetadataItemURLKey")]
		NSString ItemURLKey { get; }

		[Field ("NSMetadataItemPathKey")]
		NSString ItemPathKey { get; }

		[Field ("NSMetadataItemFSSizeKey")]
		NSString ItemFSSizeKey { get; }

		[Field ("NSMetadataItemFSCreationDateKey")]
		NSString ItemFSCreationDateKey { get; }

		[Field ("NSMetadataItemFSContentChangeDateKey")]
		NSString ItemFSContentChangeDateKey { get; }

		[Field ("NSMetadataItemIsUbiquitousKey")]
		NSString ItemIsUbiquitousKey { get; }

		[Field ("NSMetadataUbiquitousItemHasUnresolvedConflictsKey")]
		NSString UbiquitousItemHasUnresolvedConflictsKey { get; }

		[Field ("NSMetadataUbiquitousItemIsDownloadedKey")]
		NSString UbiquitousItemIsDownloadedKey { get; }

		[Field ("NSMetadataUbiquitousItemIsDownloadingKey")]
		NSString UbiquitousItemIsDownloadingKey { get; }

		[Field ("NSMetadataUbiquitousItemIsUploadedKey")]
		NSString UbiquitousItemIsUploadedKey { get; }

		[Field ("NSMetadataUbiquitousItemIsUploadingKey")]
		NSString UbiquitousItemIsUploadingKey { get; }

		[Field ("NSMetadataUbiquitousItemPercentDownloadedKey")]
		NSString UbiquitousItemPercentDownloadedKey { get; }

		[Field ("NSMetadataUbiquitousItemPercentUploadedKey")]
		NSString UbiquitousItemPercentUploadedKey { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	public interface NSMetadataQueryDelegate {
		[Export ("metadataQuery:replacementObjectForResultObject:"), DelegateName ("NSMetadataQueryObject"), DefaultValue(null)]
		NSObject ReplacementObjectForResultObject (NSMetadataQuery query, NSMetadataItem result);

		[Export ("metadataQuery:replacementValueForAttribute:value:"), DelegateName ("NSMetadataQueryValue"), DefaultValue(null)]
		NSObject ReplacementValueForAttributevalue (NSMetadataQuery query, string attributeName, NSObject value);
	}

	[BaseType (typeof (NSObject))]
	public interface NSMetadataItem {
		[Export ("valueForAttribute:")]
		NSObject ValueForAttribute (string key);

		[Export ("valuesForAttributes:")]
		NSDictionary ValuesForAttributes (NSArray keys);

		[Export ("attributes")]
		NSObject [] Attributes { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSMetadataQueryAttributeValueTuple {
		[Export ("attribute")]
		string Attribute { get; }

		[Export ("value")]
		NSObject Value { get; }

		[Export ("count")]
		nuint Count { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSMetadataQueryResultGroup {
		[Export ("attribute")]
		string Attribute { get; }

		[Export ("value")]
		NSObject Value { get; }

		[Export ("subgroups")]
		NSObject [] Subgroups { get; }

		[Export ("resultCount")]
		nuint ResultCount { get; }

		[Export ("resultAtIndex:")]
		NSObject ResultAtIndex (nuint idx);

		[Export ("results")]
		NSObject [] Results { get; }

	}

	// This API is only supported because a bunch of third-party
	// APIs are so poorly designed that they expose NSMutableArray
	// as a public property.
	[Obsolete ("You really should avoid using NSMutableArray in MonoTouch/MonoMac, this is usually used by poorly designed public APIs")]
	[BaseType (typeof (NSArray))]
	public interface NSMutableArray {
		[Export ("initWithCapacity:")]
		IntPtr Constructor (nuint capacity);
		
		[Export ("addObject:")]
		void Add (NSObject obj);

		[Export ("insertObject:atIndex:")]
		void Insert (NSObject obj, nuint index);

		[Export ("removeLastObject")]
		void RemoveLastObject ();

		[Export ("removeObjectAtIndex:")]
		void RemoveObject (nuint index);

		[Export ("replaceObjectAtIndex:withObject:")]
		void ReplaceObject (nuint index, NSObject withObject);

		[Export ("removeAllObjects")]
		void RemoveAllObjects ();

		[Export ("addObjectsFromArray:")]
		void AddObjects (NSObject [] source);

		[Export ("insertObjects:atIndexes:")]
		void InsertObjects (NSObject [] objects, NSIndexSet atIndexes);

		[Export ("removeObjectsAtIndexes:")]
		void RemoveObjectsAtIndexes (NSIndexSet indexSet);
	}
	
	[Since (3,2)]
	[BaseType (typeof (NSAttributedString))]
	public interface NSMutableAttributedString {
		[Export ("initWithString:")]
		IntPtr Constructor (string str);
		
		[Export ("initWithString:attributes:")]
		IntPtr Constructor (string str, NSDictionary attributes);

		[Export ("initWithAttributedString:")]
		IntPtr Constructor (NSAttributedString other);

		[Export ("replaceCharactersInRange:withString:")]
		void Replace (NSRange range, string newValue);

		[Export ("setAttributes:range:")]
		void SetAttributes (NSDictionary attrs, NSRange range);

		[Export ("addAttribute:value:range:")]
		void AddAttribute (NSString attributeName, NSObject value, NSRange range);

		[Export ("addAttributes:range:")]
		void AddAttributes (NSDictionary attrs, NSRange range);

		[Export ("removeAttribute:range:")]
		void RemoveAttribute (string name, NSRange range);
		
		[Export ("replaceCharactersInRange:withAttributedString:")]
		void Replace (NSRange range, NSAttributedString value);
		
		[Export ("insertAttributedString:atIndex:")]
		void Insert (NSAttributedString attrString, nuint location);

		[Export ("appendAttributedString:")]
		void Append (NSAttributedString attrString);

		[Export ("deleteCharactersInRange:")]
		void DeleteRange (NSRange range);

		[Export ("setAttributedString:")]
		void SetString (NSAttributedString attrString);

		[Export ("beginEditing")]
		void BeginEditing ();

		[Export ("endEditing")]
		void EndEditing ();
	}

	[BaseType (typeof (NSData))]
	public interface NSMutableData {
		[Static, Export ("dataWithCapacity:")] [Autorelease]
		NSMutableData FromCapacity (nuint capacity);

		[Static, Export ("dataWithLength:")] [Autorelease]
		NSMutableData FromLength (nuint length);
		
		[Static, Export ("data")] [Autorelease]
		NSMutableData Create ();
		
		[Export ("setLength:")]
		void SetLength (nuint len);

		[Export ("mutableBytes")]
		IntPtr MutableBytes { get; }

		[Export ("initWithCapacity:")]
		IntPtr Constructor (nuint len);

		[Export ("appendData:")]
		void AppendData (NSData other);

		[Export ("appendBytes:length:")]
		void AppendBytes (IntPtr bytes, nuint len);

		[Export ("setData:")]
		void SetData (NSData data);

		[Export ("length")]
		[Override]
		nuint Length { get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSDate {
		[Export ("timeIntervalSinceReferenceDate")]
		double SecondsSinceReferenceDate { get; }

		[Export ("dateWithTimeIntervalSinceReferenceDate:")]
		[Static]
		NSDate FromTimeIntervalSinceReferenceDate (double secs);

		[Static, Export ("dateWithTimeIntervalSince1970:")]
		NSDate FromTimeIntervalSince1970 (double secs);

		[Export ("date")]
		[Static]
		NSDate Now { get; }
		
		[Export ("distantPast")]
		[Static]
		NSDate DistantPast { get; }
		
		[Export ("distantFuture")]
		[Static]
		NSDate DistantFuture { get; }

		[Export ("dateByAddingTimeInterval:")]
		NSDate AddSeconds (double seconds);

		[Export ("dateWithTimeIntervalSinceNow:")]
		[Static]
		NSDate FromTimeIntervalSinceNow (double secs);
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSDictionary {
		[Export ("dictionaryWithContentsOfFile:")]
		[Static]
		NSDictionary FromFile (string path);

		[Export ("dictionaryWithContentsOfURL:")]
		[Static]
		NSDictionary FromUrl (NSUrl url);

		[Export ("dictionaryWithObject:forKey:")]
		[Static]
		NSDictionary FromObjectAndKey (NSObject obj, NSObject key);

		[Export ("dictionaryWithDictionary:")]
		[Static]
		NSDictionary FromDictionary (NSDictionary source);

		[Export ("dictionaryWithObjects:forKeys:count:")]
		[Static, Internal]
		NSDictionary FromObjectsAndKeysInternal ([NullAllowed] NSArray objects, [NullAllowed] NSArray keys, nuint count);

		[Export ("dictionaryWithObjects:forKeys:")]
		[Static, Internal]
		NSDictionary FromObjectsAndKeysInternal ([NullAllowed] NSArray objects, [NullAllowed] NSArray keys);

		[Export ("initWithDictionary:")]
		IntPtr Constructor (NSDictionary other);

		[Export ("initWithContentsOfFile:")]
		IntPtr Constructor (string fileName);

		[Export ("initWithObjects:forKeys:"), Internal]
		IntPtr Constructor (NSArray objects, NSArray keys);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);
		
		[Export ("count")]
		nuint Count { get; }

		[Export ("objectForKey:")]
		NSObject ObjectForKey (NSObject key);

		[Export ("allKeys")][Autorelease]
		NSObject [] Keys { get; }

		[Export ("allKeysForObject:")][Autorelease]
		NSObject [] KeysForObject (NSObject obj);

		[Export ("allValues")][Autorelease]
		NSObject [] Values { get; }

		[Export ("descriptionInStringsFileFormat")]
		string DescriptionInStringsFileFormat { get; }

		[Export ("isEqualToDictionary:")]
		bool IsEqualToDictionary (NSDictionary other);
		
		[Export ("objectEnumerator")]
		NSEnumerator ObjectEnumerator { get; }

		[Export ("objectsForKeys:notFoundMarker:")][Autorelease]
		NSObject [] ObjectsForKeys (NSArray keys, NSObject marker);
		
		[Export ("writeToFile:atomically:")]
		bool WriteToFile (string path, bool useAuxiliaryFile);

		[Export ("writeToURL:atomically:")]
		bool WriteToUrl (NSUrl url, bool atomically);

		[Since (6,0)]
		[Static]
		[Export ("sharedKeySetForKeys:")]
		NSObject GetSharedKeySetForKeys (NSObject [] keys);

	}

	[BaseType (typeof (NSObject))]
	public interface NSEnumerator {
		[Export ("nextObject")]
		NSObject NextObject (); 
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	public interface NSError {
		[Static, Export ("errorWithDomain:code:userInfo:")]
		NSError FromDomain (NSString domain, nint code, [NullAllowed] NSDictionary userInfo);

		[Export ("initWithDomain:code:userInfo:")]
		IntPtr Constructor (NSString domain, nint code, [NullAllowed] NSDictionary userInfo);
		
		[Export ("domain")]
		string Domain { get; }

		[Export ("code")]
		nint Code { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; }

		[Export ("localizedDescription")]
		string LocalizedDescription { get; }

		[Field ("NSCocoaErrorDomain")]
		NSString CocoaErrorDomain { get;}
		[Field ("NSPOSIXErrorDomain")]
		NSString PosixErrorDomain { get; }
		[Field ("NSOSStatusErrorDomain")]
		NSString OsStatusErrorDomain { get; }
		[Field ("NSMachErrorDomain")]
		NSString MachErrorDomain { get; }

		[Field ("NSUnderlyingErrorKey")]
		NSString UnderlyingErrorKey { get; }

		[Field ("NSLocalizedDescriptionKey")]
		NSString LocalizedDescriptionKey { get; }

		[Field ("NSLocalizedFailureReasonErrorKey")]
		NSString LocalizedFailureReasonErrorKey { get; }

		[Field ("NSLocalizedRecoverySuggestionErrorKey")]
		NSString LocalizedRecoverySuggestionErrorKey { get; }

		[Field ("NSLocalizedRecoveryOptionsErrorKey")]
		NSString LocalizedRecoveryOptionsErrorKey { get; }

		[Field ("NSRecoveryAttempterErrorKey")]
		NSString RecoveryAttempterErrorKey { get; }

		[Field ("NSHelpAnchorErrorKey")]
		NSString HelpAnchorErrorKey { get; }

		[Field ("NSStringEncodingErrorKey")]
		NSString StringEncodingErrorKey { get; }

		[Field ("NSURLErrorKey")]
		NSString UrlErrorKey { get; }

		[Field ("NSFilePathErrorKey")]
		NSString FilePathErrorKey { get; }
	}

	[BaseType (typeof (NSObject))]
	// 'init' returns NIL
	[DisableDefaultCtor]
	public interface NSException {
		[Export ("initWithName:reason:userInfo:")]
		IntPtr Constructor (string name, string reason, [NullAllowed] NSDictionary userInfo);

		[Export ("name")]
		string Name { get; }
	
		[Export ("reason")]
		string Reason { get; }
		
		[Export ("userInfo")]
		NSObject UserInfo { get; }

		[Export ("callStackReturnAddresses")]
		NSNumber[] CallStackReturnAddresses { get; }
	}

	public delegate void NSExpressionHandler (NSObject evaluatedObject, NSExpression [] expressions, NSMutableDictionary context);
	
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -predicateFormat cannot be sent to an abstract object of class NSExpression: Create a concrete instance!
	[DisableDefaultCtor]
	public interface NSExpression {
		[Static, Export ("expressionForConstantValue:")]
		NSExpression FromConstant (NSObject obj);

		[Static, Export ("expressionForEvaluatedObject")]
		NSExpression ExpressionForEvaluatedObject { get; }

		[Static, Export ("expressionForVariable:")]
		NSExpression FromVariable (string string1);

		[Static, Export ("expressionForKeyPath:")]
		NSExpression FromKeyPath (string keyPath);

		[Static, Export ("expressionForFunction:arguments:")]
		NSExpression FromFunction (string name, NSExpression[] parameters);

		[Static, Export ("expressionWithFormat:argumentArray:")]
		NSExpression FromFormat (string format, NSExpression [] parameters);
		
		//+ (NSExpression *)expressionForAggregate:(NSArray *)subexpressions; 
		[Static, Export ("expressionForAggregate:")]
		NSExpression FromAggregate (NSExpression [] subexpressions);

		[Static, Export ("expressionForUnionSet:with:")]
		NSExpression FromUnionSet (NSExpression left, NSExpression right);

		[Static, Export ("expressionForIntersectSet:with:")]
		NSExpression FromIntersectSet (NSExpression left, NSExpression right);

		[Static, Export ("expressionForMinusSet:with:")]
		NSExpression FromMinusSet (NSExpression left, NSExpression right);

		//+ (NSExpression *)expressionForSubquery:(NSExpression *)expression usingIteratorVariable:(NSString *)variable predicate:(id)predicate; 
		[Static, Export ("expressionForSubquery:usingIteratorVariable:predicate:")]
		NSExpression FromSubquery (NSExpression expression, string variable, NSObject predicate);

		[Static, Export ("expressionForFunction:selectorName:arguments:")]
		NSExpression FromFunction (NSExpression target, string name, NSExpression[] parameters);

		[Static, Export ("expressionForBlock:arguments:")]
		NSExpression FromFunction (NSExpressionHandler target, NSExpression[] parameters);

		[Export ("initWithExpressionType:")]
		IntPtr Constructor (NSExpressionType type);

		[Export ("expressionType")]
		NSExpressionType ExpressionType { get; }

		[Export ("constantValue")]
		NSObject ConstantValue { get; }

		[Export ("keyPath")]
		string KeyPath { get; }

		[Export ("function")]
		string Function { get; }

		[Export ("variable")]
		string Variable { get; }

		[Export ("operand")]
		NSExpression Operand { get; }

		[Export ("arguments")]
		NSExpression[] Arguments { get; }

		[Export ("collection")]
		NSObject Collection { get; }

		[Export ("predicate")]
		NSPredicate Predicate { get; }

		[Export ("leftExpression")]
		NSExpression LeftExpression { get; }

		[Export ("rightExpression")]
		NSExpression RightExpression { get; }

		[Export ("expressionValueWithObject:context:")]
		NSExpression ExpressionValueWithObject (NSObject object1, NSMutableDictionary context);
	}

	[BaseType (typeof (NSObject))]
	public interface NSNull {
		[Export ("null"), Static]
		NSNull Null { get; }
	}

	delegate void NSLingusticEnumerator (NSString tag, NSRange tokenRange, NSRange sentenceRange, ref bool stop);
	
	[BaseType (typeof (NSObject))]
	interface NSLinguisticTagger {
		[Export ("initWithTagSchemes:options:")]
		IntPtr Constructor (NSString [] tagSchemes, NSLinguisticTaggerOptions opts);

		[Export ("tagSchemes")]
		NSString [] TagSchemes { get; }

		[Static]
		[Export ("availableTagSchemesForLanguage:")]
		NSString [] GetAvailableTagSchemesForLanguage (string language);

		[Export ("setOrthography:range:")]
		void SetOrthographyrange (NSOrthography orthography, NSRange range);

		[Export ("orthographyAtIndex:effectiveRange:")]
		NSOrthography GetOrthography (nuint charIndex, ref NSRange effectiveRange);

		[Export ("stringEditedInRange:changeInLength:")]
		void StringEditedInRange (NSRange newRange, nint delta);

		[Export ("enumerateTagsInRange:scheme:options:usingBlock:")]
		void EnumerateTagsInRange (NSRange range, NSString tagScheme, NSLinguisticTaggerOptions opts, NSLingusticEnumerator enumerator);

		[Export ("sentenceRangeForRange:")]
		NSRange GetSentenceRangeForRange (NSRange range);

		[Export ("tagAtIndex:scheme:tokenRange:sentenceRange:")]
		string GetTag (nuint charIndex, NSString tagScheme, ref NSRange tokenRange, ref NSRange sentenceRange);

		[Export ("tagsInRange:scheme:options:tokenRanges:"), Internal]
		NSString [] GetTagsInRange (NSRange range, NSString tagScheme, NSLinguisticTaggerOptions opts, ref NSArray tokenRanges);

		[Export ("possibleTagsAtIndex:scheme:tokenRange:sentenceRange:scores:"), Internal]
		NSString [] GetPossibleTags (nuint charIndex, NSString tagScheme, ref NSRange tokenRange, ref NSRange sentenceRange, ref NSArray scores);

		//Detected properties
		[Export ("string")]
		string AnalysisString { get; set; }
	}

	[Static]
	public interface NSLinguisticTag {
		[Field ("NSLinguisticTagSchemeTokenType")]
		NSString SchemeTokenType { get; }

		[Field ("NSLinguisticTagSchemeLexicalClass")]
		NSString SchemeLexicalClass { get; }

		[Field ("NSLinguisticTagSchemeNameType")]
		NSString SchemeNameType { get; }

		[Field ("NSLinguisticTagSchemeNameTypeOrLexicalClass")]
		NSString SchemeNameTypeOrLexicalClass { get; }

		[Field ("NSLinguisticTagSchemeLemma")]
		NSString SchemeLemma { get; }

		[Field ("NSLinguisticTagSchemeLanguage")]
		NSString SchemeLanguage { get; }

		[Field ("NSLinguisticTagSchemeScript")]
		NSString SchemeScript { get; }

		[Field ("NSLinguisticTagWord")]
		NSString Word { get; }

		[Field ("NSLinguisticTagPunctuation")]
		NSString Punctuation { get; }

		[Field ("NSLinguisticTagWhitespace")]
		NSString Whitespace { get; }

		[Field ("NSLinguisticTagOther")]
		NSString Other { get; }

		[Field ("NSLinguisticTagNoun")]
		NSString Noun { get; }

		[Field ("NSLinguisticTagVerb")]
		NSString Verb { get; }

		[Field ("NSLinguisticTagAdjective")]
		NSString Adjective { get; }

		[Field ("NSLinguisticTagAdverb")]
		NSString Adverb { get; }

		[Field ("NSLinguisticTagPronoun")]
		NSString Pronoun { get; }

		[Field ("NSLinguisticTagDeterminer")]
		NSString Determiner { get; }

		[Field ("NSLinguisticTagParticle")]
		NSString Particle { get; }

		[Field ("NSLinguisticTagPreposition")]
		NSString Preposition { get; }

		[Field ("NSLinguisticTagNumber")]
		NSString Number { get; }

		[Field ("NSLinguisticTagConjunction")]
		NSString Conjunction { get; }

		[Field ("NSLinguisticTagInterjection")]
		NSString Interjection { get; }

		[Field ("NSLinguisticTagClassifier")]
		NSString Classifier { get; }

		[Field ("NSLinguisticTagIdiom")]
		NSString Idiom { get; }

		[Field ("NSLinguisticTagOtherWord")]
		NSString OtherWord { get; }

		[Field ("NSLinguisticTagSentenceTerminator")]
		NSString SentenceTerminator { get; }

		[Field ("NSLinguisticTagOpenQuote")]
		NSString OpenQuote { get; }

		[Field ("NSLinguisticTagCloseQuote")]
		NSString CloseQuote { get; }

		[Field ("NSLinguisticTagOpenParenthesis")]
		NSString OpenParenthesis { get; }

		[Field ("NSLinguisticTagCloseParenthesis")]
		NSString CloseParenthesis { get; }

		[Field ("NSLinguisticTagWordJoiner")]
		NSString WordJoiner { get; }

		[Field ("NSLinguisticTagDash")]
		NSString Dash { get; }

		[Field ("NSLinguisticTagOtherPunctuation")]
		NSString OtherPunctuation { get; }

		[Field ("NSLinguisticTagParagraphBreak")]
		NSString ParagraphBreak { get; }

		[Field ("NSLinguisticTagOtherWhitespace")]
		NSString OtherWhitespace { get; }

		[Field ("NSLinguisticTagPersonalName")]
		NSString PersonalName { get; }

		[Field ("NSLinguisticTagPlaceName")]
		NSString PlaceName { get; }

		[Field ("NSLinguisticTagOrganizationName")]
		NSString OrganizationName { get; }
	}
	
	[BaseType (typeof (NSObject))]
	// 'init' returns NIL so it's not usable evenif it does not throw an ObjC exception
	[DisableDefaultCtor]
	public interface NSLocale {
		[Static]
		[Export ("systemLocale")]
		NSLocale SystemLocale { get; }

		[Static]
		[Export ("currentLocale")]
		NSLocale CurrentLocale { get; }

		[Static]
		[Export ("autoupdatingCurrentLocale")]
		NSLocale AutoUpdatingCurrentLocale { get; }
		

		[Export ("initWithLocaleIdentifier:")]
		IntPtr Constructor (string identifier);

		[Export ("localeIdentifier")]
		string LocaleIdentifier { get; }

		[Export ("availableLocaleIdentifiers")][Static]
		string [] AvailableLocaleIdentifiers { get; }

		[Export ("ISOLanguageCodes")][Static]
		string [] ISOLanguageCodes { get; }

		[Export ("ISOCurrencyCodes")][Static]
		string [] ISOCurrencyCodes { get; }

		[Export ("ISOCountryCodes")][Static]
		string [] ISOCountryCodes { get; }

		[Export ("commonISOCurrencyCodes")][Static]
		string [] CommonISOCurrencyCodes { get; }

		[Export ("preferredLanguages")][Static]
		string [] PreferredLanguages { get; }

		[Export ("componentsFromLocaleIdentifier:")][Static]
		NSDictionary ComponentsFromLocaleIdentifier (string identifier);

		[Export ("localeIdentifierFromComponents:")][Static]
		string LocaleIdentifierFromComponents (NSDictionary dict);

		[Export ("canonicalLanguageIdentifierFromString:")][Static]
		string CanonicalLanguageIdentifierFromString (string str);

		[Export ("canonicalLocaleIdentifierFromString:")][Static]
		string CanonicalLocaleIdentifierFromString (string str);

		[Export ("characterDirectionForLanguage:")][Static]
		NSLocaleLanguageDirection GetCharacterDirection (string isoLanguageCode);

		[Export ("lineDirectionForLanguage:")][Static]
		NSLocaleLanguageDirection GetLineDirection (string isoLanguageCode);

		[Field ("NSCurrentLocaleDidChangeNotification")]
		[Notification]
		NSString CurrentLocaleDidChangeNotification { get; }

		[Export ("objectForKey:"), Internal]
		NSObject ObjectForKey (NSString key);

		[Export ("displayNameForKey:value:"), Internal]
		NSString DisplayNameForKey (NSString key, string value);

		[Internal, Field ("NSLocaleIdentifier")]
		NSString _Identifier { get; }
		
		[Internal, Field ("NSLocaleLanguageCode")]
		NSString _LanguageCode { get; }
		
		[Internal, Field ("NSLocaleCountryCode")]
		NSString _CountryCode { get; }
		
		[Internal, Field ("NSLocaleScriptCode")]
		NSString _ScriptCode { get; }
		
		[Internal, Field ("NSLocaleVariantCode")]
		NSString _VariantCode { get; }
		
		[Internal, Field ("NSLocaleExemplarCharacterSet")]
		NSString _ExemplarCharacterSet { get; }
		
		[Internal, Field ("NSLocaleCalendar")]
		NSString _Calendar { get; }
		
		[Internal, Field ("NSLocaleCollationIdentifier")]
		NSString _CollationIdentifier { get; }
		
		[Internal, Field ("NSLocaleUsesMetricSystem")]
		NSString _UsesMetricSystem { get; }
		
		[Internal, Field ("NSLocaleMeasurementSystem")]
		NSString _MeasurementSystem { get; }
		
		[Internal, Field ("NSLocaleDecimalSeparator")]
		NSString _DecimalSeparator { get; }
		
		[Internal, Field ("NSLocaleGroupingSeparator")]
		NSString _GroupingSeparator { get; }
		
		[Internal, Field ("NSLocaleCurrencySymbol")]
		NSString _CurrencySymbol { get; }
		
		[Internal, Field ("NSLocaleCurrencyCode")]
		NSString _CurrencyCode { get; }
		
		[Internal, Field ("NSLocaleCollatorIdentifier")]
		NSString _CollatorIdentifier { get; }
		
		[Internal, Field ("NSLocaleQuotationBeginDelimiterKey")]
		NSString _QuotationBeginDelimiterKey { get; }
		
		[Internal, Field ("NSLocaleQuotationEndDelimiterKey")]
		NSString _QuotationEndDelimiterKey { get; }
		
		[Internal, Field ("NSLocaleAlternateQuotationBeginDelimiterKey")]
		NSString _AlternateQuotationBeginDelimiterKey { get; }
		
		[Internal, Field ("NSLocaleAlternateQuotationEndDelimiterKey")]
		NSString _AlternateQuotationEndDelimiterKey { get; }
	}

	
	[BaseType (typeof (NSObject))]
	// init returns NIL
	[DisableDefaultCtor]
	public interface NSRunLoop {
		[Export ("currentRunLoop")][Static][IsThreadStatic]
		NSRunLoop Current { get; }

		[Export ("mainRunLoop")][Static]
		NSRunLoop Main { get; }

		[Export ("currentMode")]
		NSString CurrentMode { get; }

		[Export ("getCFRunLoop")]
		CFRunLoop GetCFRunLoop ();

		[Export ("addTimer:forMode:")]
		void AddTimer (NSTimer timer, NSString forMode);

		[Export ("limitDateForMode:")]
		NSDate LimitDateForMode (NSString mode);

		[Export ("acceptInputForMode:beforeDate:")]
		void AcceptInputForMode (NSString mode, NSDate limitDate);

		[Export ("run")]
		void Run ();

		[Export ("runUntilDate:")]
		void RunUntil (NSDate date);

		[Export ("runMode:beforeDate:")]
		bool RunUntil (NSString runLoopMode, NSDate limitdate);
		
		[Field ("NSDefaultRunLoopMode")]
		NSString NSDefaultRunLoopMode { get; }

		[Field ("NSRunLoopCommonModes")]
		NSString NSRunLoopCommonModes { get; }

#if MONOMAC
		[Field ("NSConnectionReplyMode")]
		NSString NSRunLoopConnectionReplyMode { get; }

		[Field ("NSModalPanelRunLoopMode", "AppKit")]
		NSString NSRunLoopModalPanelMode { get; }

		[Field ("NSEventTrackingRunLoopMode", "AppKit")]
		NSString NSRunLoopEventTracking { get; }
#else
		[Field ("UITrackingRunLoopMode", "UIKit")]
		NSString UITrackingRunLoopMode { get; }
#endif
	}

	[BaseType (typeof (NSObject))]
	public interface NSSet {
		[Export ("set")][Static]
		NSSet CreateSet ();

		[Export ("initWithSet:")]
		IntPtr Constructor (NSSet other);
		
		[Export ("initWithArray:")]
		IntPtr Constructor (NSArray other);
		
		[Export ("count")]
		nuint Count { get; }

		[Export ("anyObject")]
		NSObject AnyObject { get; }

		[Export ("containsObject:")]
		bool Contains (NSObject id);

		[Export ("allObjects")][Internal]
		IntPtr _AllObjects ();

		[Export ("isEqualToSet:")]
		bool IsEqualToSet (NSSet other);

		[Export ("objectEnumerator"), Internal]
		NSEnumerator _GetEnumerator ();
		
		[Export ("isSubsetOfSet:")]
		bool IsSubsetOf (NSSet other);
		
		[Export ("enumerateObjectsUsingBlock:")]
		[Since (4,0)]
		void Enumerate (NSSetEnumerator enumerator);

		[Export ("setByAddingObjectsFromSet:"), Internal]
		NSSet SetByAddingObjectsFromSet (NSSet other);

		[Export ("intersectsSet:")]
		bool IntersectsSet (NSSet other);
	}

	[BaseType (typeof (NSObject))]
	public interface NSSortDescriptor {
		[Export ("initWithKey:ascending:")]
		IntPtr Constructor (string key, bool ascending);

		[Export ("initWithKey:ascending:selector:")]
		IntPtr Constructor (string key, bool ascending, Selector selector);

		[Export ("key")]
		string Key { get; }

		[Export ("ascending")]
		bool Ascending { get; }

		[Export ("selector")]
		Selector Selector { get; }

		[Export ("compareObject:toObject:")]
		NSComparisonResult Compare (NSObject object1, NSObject object2);

		[Export ("reversedSortDescriptor")]
		NSObject ReversedSortDescriptor { get; }
	}

	[BaseType (typeof(NSObject))]
	[Dispose ("if (disposing) { Invalidate (); } ")]
	// init returns NIL
	[DisableDefaultCtor]
	public interface NSTimer {
		// TODO: scheduledTimerWithTimeInterval:invocation:repeats:

		[Static, Export ("scheduledTimerWithTimeInterval:target:selector:userInfo:repeats:")]
		NSTimer CreateScheduledTimer (double seconds, NSObject target, Selector selector, [NullAllowed] NSObject userInfo, bool repeats);

		// TODO: timerWithTimeInterval:invocation:repeats:

		[Static, Export ("timerWithTimeInterval:target:selector:userInfo:repeats:")]
		NSTimer CreateTimer (double seconds, NSObject target, Selector selector, [NullAllowed] NSObject userInfo, bool repeats);

		[Export ("initWithFireDate:interval:target:selector:userInfo:repeats:")]
		IntPtr Constructor (NSDate date, double seconds, NSObject target, Selector selector, [NullAllowed] NSObject userInfo, bool repeats);

		[Export ("fire")]
		void Fire ();

		[Export ("fireDate")]
		NSDate FireDate { get; set; }

		[Export ("invalidate")]
		void Invalidate ();

		[Export ("isValid")]
		bool IsValid { get; }

		[Export ("timeInterval")]
		double TimeInterval { get; }

		[Export ("userInfo")]
		NSObject UserInfo { get; }
	}

	[BaseType (typeof(NSObject))]
	// NSTimeZone is an abstract class that defines the behavior of time zone objects. -> http://developer.apple.com/library/ios/#documentation/Cocoa/Reference/Foundation/Classes/NSTimeZone_Class/Reference/Reference.html
	// calling 'init' returns a NIL pointer, i.e. an unusable instance
	[DisableDefaultCtor]
	public interface NSTimeZone {
		[Export ("initWithName:")]
		IntPtr Constructor (string name);
		
		[Export ("initWithName:data:")]
		IntPtr Constructor (string name, NSData data);

		[Export ("name")]
		string Name { get; } 

		[Export ("data")]
		NSData Data { get; }

		[Export ("secondsFromGMTForDate:")]
		nint SecondsFromGMT (NSDate date);
		
		[Export ("abbreviationForDate:")]
		string Abbreviation (NSDate date);

		[Export ("isDaylightSavingTimeForDate:")]
		bool IsDaylightSavingsTime (NSDate date);

		[Export ("daylightSavingTimeOffsetForDate:")]
		double DaylightSavingTimeOffset (NSDate date);

		[Export ("nextDaylightSavingTimeTransitionAfterDate:")]
		NSDate NextDaylightSavingTimeTransitionAfter (NSDate date);

		[Static, Export ("timeZoneWithName:")]
		NSTimeZone FromName (string tzName);

		[Static, Export ("timeZoneWithName:data:")]
		NSTimeZone FromName (string tzName, NSData data);
		
		[Static, Export ("localTimeZone")]
		NSTimeZone LocalTimeZone { get; }

		[Export ("secondsFromGMT")]
		nint GetSecondsFromGMT { get; }

		[Export ("defaultTimeZone"), Static]
		NSTimeZone DefaultTimeZone { get; set; }

		[Export ("resetSystemTimeZone"), Static]
		void ResetSystemTimeZone ();

		[Export ("systemTimeZone"), Static]
		NSTimeZone SystemTimeZone { get; }
		
		[Export ("timeZoneWithAbbreviation:"), Static]
		NSTimeZone FromAbbreviation (string abbreviation);

		[Export ("knownTimeZoneNames"), Static, Internal]
		string[] _KnownTimeZoneNames { get; }

		[Export ("timeZoneDataVersion"), Static]
		string DataVersion { get; }
	}

	interface NSUbiquitousKeyValueStoreChangeEventArgs {
		[Export ("NSUbiquitousKeyValueStoreChangedKeysKey")]
		string [] ChangedKeys { get; }
	
		[Export ("NSUbiquitousKeyValueStoreChangeReasonKey")]
		NSUbiquitousKeyValueStoreChangeReason ChangeReason { get; }
	}

	[BaseType (typeof (NSObject))]
	interface NSUbiquitousKeyValueStore {
		[Static]
		[Export ("defaultStore")]
		NSUbiquitousKeyValueStore DefaultStore { get; }

		[Export ("objectForKey:"), Internal]
		NSObject ObjectForKey (string aKey);

		[Export ("setObject:forKey:"), Internal]
		void SetObjectForKey (NSObject anObject, string aKey);

		[Export ("removeObjectForKey:")]
		void Remove (string aKey);

		[Export ("stringForKey:")]
		string GetString (string aKey);

		[Export ("arrayForKey:")]
		NSObject [] GetArray (string aKey);

		[Export ("dictionaryForKey:")]
		NSDictionary GetDictionary (string aKey);

		[Export ("dataForKey:")]
		NSData GetData (string aKey);

		[Export ("longLongForKey:")]
		long GetLong (string aKey);

		[Export ("doubleForKey:")]
		double GetDouble (string aKey);

		[Export ("boolForKey:")]
		bool GetBool (string aKey);

		[Export ("setString:forKey:"), Internal]
		void _SetString (string aString, string aKey);

		[Export ("setData:forKey:"), Internal]
		void _SetData (NSData data, string key);

		[Export ("setArray:forKey:"), Internal]
		void _SetArray (NSObject [] array, string key);

		[Export ("setDictionary:forKey:"), Internal]
		void _SetDictionary (NSDictionary aDictionary, string aKey);

		[Export ("setLongLong:forKey:"), Internal]
		void _SetLong (long value, string aKey);

		[Export ("setDouble:forKey:"), Internal]
		void _SetDouble (double value, string aKey);

		[Export ("setBool:forKey:"), Internal]
		void _SetBool (bool value, string aKey);

		[Export ("dictionaryRepresentation")]
		NSDictionary DictionaryRepresentation ();

		[Export ("synchronize")]
		bool Synchronize ();

		[Field ("NSUbiquitousKeyValueStoreDidChangeExternallyNotification")]
		[Notification (typeof (NSUbiquitousKeyValueStoreChangeEventArgs))]
		NSString DidChangeExternallyNotification { get; }

		[Field ("NSUbiquitousKeyValueStoreChangeReasonKey")]
		NSString ChangeReasonKey { get; }

		[Field ("NSUbiquitousKeyValueStoreChangedKeysKey")]
		NSString ChangedKeysKey { get; }
	}
	
	[BaseType (typeof (NSObject), Name="NSUUID")]
	public interface NSUuid {
		[Export ("initWithUUIDString:")]
		IntPtr Constructor (string str);

		// bound manually to keep the managed/native signatures identical
		//[Export ("initWithUUIDBytes:"), Internal]
		//IntPtr Constructor (IntPtr bytes, bool unused);

		[Export ("getUUIDBytes:"), Internal]
		void GetUuidBytes (IntPtr uuid);

		[Export ("UUIDString")]
		string AsString ();
	}

	[BaseType (typeof (NSObject))]
	public interface NSUserDefaults {
		[Static]
		[Export ("standardUserDefaults")]
		NSUserDefaults StandardUserDefaults { get; }
	
		[Static]
		[Export ("resetStandardUserDefaults")]
		void ResetStandardUserDefaults ();
	
		[Export ("initWithUser:")]
		IntPtr InitWithUserName (string  username);
	
		[Export ("initWithSuiteName:")]
		IntPtr InitWithSuiteName (string  suiteName);

		[Export ("objectForKey:")][Internal]
		NSObject ObjectForKey (string defaultName);
	
		[Export ("setObject:forKey:")][Internal]
		void SetObjectForKey (NSObject value, string  defaultName);
	
		[Export ("removeObjectForKey:")]
		void RemoveObject (string defaultName);
	
		[Export ("stringForKey:")]
		string StringForKey (string defaultName);
	
		[Export ("arrayForKey:")]
		NSObject [] ArrayForKey (string defaultName);
	
		[Export ("dictionaryForKey:")]
		NSDictionary DictionaryForKey (string defaultName);
	
		[Export ("dataForKey:")]
		NSData DataForKey (string defaultName);
	
		[Export ("stringArrayForKey:")]
		string [] StringArrayForKey (string defaultName);
	
		[Export ("integerForKey:")]
		int IntForKey (string defaultName);
	
		[Export ("floatForKey:")]
		float FloatForKey (string defaultName);
	
		[Export ("doubleForKey:")]
		double DoubleForKey (string defaultName);
	
		[Export ("boolForKey:")]
		bool BoolForKey (string defaultName);
	
		[Export ("setInteger:forKey:")]
		void SetInt (int value, string defaultName);
	
		[Export ("setFloat:forKey:")]
		void SetFloat (float value, string defaultName);
	
		[Export ("setDouble:forKey:")]
		void SetDouble (double value, string defaultName);
	
		[Export ("setBool:forKey:")]
		void SetBool (bool value, string  defaultName);
	
		[Export ("registerDefaults:")]
		void RegisterDefaults (NSDictionary registrationDictionary);
	
		[Export ("addSuiteNamed:")]
		void AddSuite (string suiteName);
	
		[Export ("removeSuiteNamed:")]
		void RemoveSuite (string suiteName);
	
		[Export ("dictionaryRepresentation")]
		NSDictionary AsDictionary ();
	
		[Export ("volatileDomainNames")]
		string [] VolatileDomainNames ();
	
		[Export ("volatileDomainForName:")]
		NSDictionary GetVolatileDomain (string domainName);
	
		[Export ("setVolatileDomain:forName:")]
		void SetVolatileDomain (NSDictionary domain, string domainName);
	
		[Export ("removeVolatileDomainForName:")]
		void RemoveVolatileDomain (string domainName);
	
		[Export ("persistentDomainNames")]
		string [] PersistentDomainNames ();
	
		[Export ("persistentDomainForName:")]
		NSDictionary PersistentDomainForName (string domainName);
	
		[Export ("setPersistentDomain:forName:")]
		void SetPersistentDomain (NSDictionary domain, string domainName);
	
		[Export ("removePersistentDomainForName:")]
		void RemovePersistentDomain (string domainName);
	
		[Export ("synchronize")]
		bool Synchronize ();
	
		[Export ("objectIsForcedForKey:")]
		bool ObjectIsForced (string key);
	
		[Export ("objectIsForcedForKey:inDomain:")]
		bool ObjectIsForced (string key, string domain);

		[Field ("NSGlobalDomain")]
		NSString GlobalDomain { get; }

		[Field ("NSArgumentDomain")]
		NSString ArgumentDomain { get; }

		[Field ("NSRegistrationDomain")]
		NSString RegistrationDomain { get; }
	}
	
	[BaseType (typeof (NSObject), Name="NSURL")]
	// init returns NIL
	[DisableDefaultCtor]
	public interface NSUrl {
		[Export ("initWithScheme:host:path:")]
		IntPtr Constructor (string scheme, string host, string path);

		[Export ("initFileURLWithPath:isDirectory:")]
		IntPtr Constructor (string path, bool isDir);

		[Export ("initWithString:")]
		IntPtr Constructor (string path);

		[Export ("initWithString:relativeToURL:")]
		IntPtr Constructor (string path, NSUrl relativeToUrl);		

		[Export ("URLWithString:")][Static]
		NSUrl FromString (string s);

		[Export ("URLWithString:relativeToURL:")][Internal][Static]
		NSUrl _FromStringRelative (string url, NSUrl relative);
		
		[Export ("absoluteString")]
		string AbsoluteString { get; }

		[Export ("absoluteURL")]
		NSUrl AbsoluteUrl { get; }

		[Export ("baseURL")]
		NSUrl BaseUrl { get; }

		[Export ("fragment")]
		string Fragment { get; }

		[Export ("host")]
		string Host { get; }

		[Export ("isEqual:")]
		bool IsEqual (NSUrl other);

		[Export ("isFileURL")]
		bool IsFileUrl { get; }

		[Export ("parameterString")]
		string ParameterString { get;}

		[Export ("password")]
		string Password { get;}

		[Export ("path")]
		string Path { get;}

		[Export ("query")]
		string Query { get;}

		[Export ("relativePath")]
		string RelativePath { get;}

		[Export ("relativeString")]
		string RelativeString { get;}

		[Export ("resourceSpecifier")]
		string ResourceSpecifier { get;}

		[Export ("scheme")]
		string Scheme { get;}

		[Export ("user")]
		string User { get;}

		[Export ("standardizedURL")]
		NSUrl StandardizedUrl { get; }

		[Export ("URLByAppendingPathComponent:isDirectory:")]
		NSUrl Append (string pathComponent, bool isDirectory);
		
#if MONOMAC && !MONOMAC_BOOTSTRAP

		/* These methods come from NURL_AppKitAdditions */

		[Export ("URLFromPasteboard:")]
		[Static]
		NSUrl FromPasteboard (NSPasteboard pasteboard);

		[Export ("writeToPasteboard:")]
		void WriteToPasteboard (NSPasteboard pasteboard);
		
		[Export("bookmarkDataWithContentsOfURL:error:")]
		[Static]
		NSData GetBookmarkData (NSUrl bookmarkFileUrl, out NSError error);

		[Export("URLByResolvingBookmarkData:options:relativeToURL:bookmarkDataIsStale:error:")]
		[Static]
		NSUrl FromBookmarkData (NSData data, NSUrlBookmarkResolutionOptions options, [NullAllowed] NSUrl relativeToUrl, out bool isStale, out NSError error);

		[Export("writeBookmarkData:toURL:options:error:")]
		[Static]
		bool WriteBookmarkData (NSData data, NSUrl bookmarkFileUrl, NSUrlBookmarkCreationOptions options, out NSError error);

		[Export("startAccessingSecurityScopedResource")]
		bool StartAccessingSecurityScopedResource();

		[Export("stopAccessingSecurityScopedResource")]
		void StopAccessingSecurityScopedResource();
	
		[Export("filePathURL")]
		NSUrl FilePathUrl { get; }

		[Export("fileReferenceURL")]
		NSUrl FileReferenceUrl { get; }		

#endif

		[Export ("getResourceValue:forKey:error:"), Internal]
		bool GetResourceValue (out NSObject value, string key, out NSError error);

		[Export ("resourceValuesForKeys:error:")]
		NSDictionary GetResourceValues (NSString [] keys, out NSError error);

		[Export ("setResourceValue:forKey:error:"), Internal]
		bool SetResourceValue (NSObject value, string key, out NSError error);
		
		//[Export ("port")]
		//NSNumber Port { get;}

		[Field ("NSURLNameKey")]
		NSString NameKey { get; }

		[Field ("NSURLLocalizedNameKey")]
		NSString LocalizedNameKey { get; }

		[Field ("NSURLIsRegularFileKey")]
		NSString IsRegularFileKey { get; }

		[Field ("NSURLIsDirectoryKey")]
		NSString IsDirectoryKey { get; }

		[Field ("NSURLIsSymbolicLinkKey")]
		NSString IsSymbolicLinkKey { get; }

		[Field ("NSURLIsVolumeKey")]
		NSString IsVolumeKey { get; }

		[Field ("NSURLIsPackageKey")]
		NSString IsPackageKey { get; }

		[Field ("NSURLIsSystemImmutableKey")]
		NSString IsSystemImmutableKey { get; }

		[Field ("NSURLIsUserImmutableKey")]
		NSString IsUserImmutableKey { get; }

		[Field ("NSURLIsHiddenKey")]
		NSString IsHiddenKey { get; }

		[Field ("NSURLHasHiddenExtensionKey")]
		NSString HasHiddenExtensionKey { get; }

		[Field ("NSURLCreationDateKey")]
		NSString CreationDateKey { get; }

		[Field ("NSURLContentAccessDateKey")]
		NSString ContentAccessDateKey { get; }

		[Field ("NSURLContentModificationDateKey")]
		NSString ContentModificationDateKey { get; }

		[Field ("NSURLAttributeModificationDateKey")]
		NSString AttributeModificationDateKey { get; }

		[Field ("NSURLLinkCountKey")]
		NSString LinkCountKey { get; }

		[Field ("NSURLParentDirectoryURLKey")]
		NSString ParentDirectoryURLKey { get; }

		[Field ("NSURLVolumeURLKey")]
		NSString VolumeURLKey { get; }

		[Field ("NSURLTypeIdentifierKey")]
		NSString TypeIdentifierKey { get; }

		[Field ("NSURLLocalizedTypeDescriptionKey")]
		NSString LocalizedTypeDescriptionKey { get; }

		[Field ("NSURLLabelNumberKey")]
		NSString LabelNumberKey { get; }

		[Field ("NSURLLabelColorKey")]
		NSString LabelColorKey { get; }

		[Field ("NSURLLocalizedLabelKey")]
		NSString LocalizedLabelKey { get; }

		[Field ("NSURLEffectiveIconKey")]
		NSString EffectiveIconKey { get; }

		[Field ("NSURLCustomIconKey")]
		NSString CustomIconKey { get; }

		[Field ("NSURLFileSizeKey")]
		NSString FileSizeKey { get; }

		[Field ("NSURLFileAllocatedSizeKey")]
		NSString FileAllocatedSizeKey { get; }

		[Field ("NSURLIsAliasFileKey")]
		NSString IsAliasFileKey	{ get; }

		[Field ("NSURLVolumeLocalizedFormatDescriptionKey")]
		NSString VolumeLocalizedFormatDescriptionKey { get; }

		[Field ("NSURLVolumeTotalCapacityKey")]
		NSString VolumeTotalCapacityKey { get; }

		[Field ("NSURLVolumeAvailableCapacityKey")]
		NSString VolumeAvailableCapacityKey { get; }

		[Field ("NSURLVolumeResourceCountKey")]
		NSString VolumeResourceCountKey { get; }

		[Field ("NSURLVolumeSupportsPersistentIDsKey")]
		NSString VolumeSupportsPersistentIDsKey { get; }

		[Field ("NSURLVolumeSupportsSymbolicLinksKey")]
		NSString VolumeSupportsSymbolicLinksKey { get; }

		[Field ("NSURLVolumeSupportsHardLinksKey")]
		NSString VolumeSupportsHardLinksKey { get; }

		[Field ("NSURLVolumeSupportsJournalingKey")]
		NSString VolumeSupportsJournalingKey { get; }

		[Field ("NSURLVolumeIsJournalingKey")]
		NSString VolumeIsJournalingKey { get; }

		[Field ("NSURLVolumeSupportsSparseFilesKey")]
		NSString VolumeSupportsSparseFilesKey { get; }

		[Field ("NSURLVolumeSupportsZeroRunsKey")]
		NSString VolumeSupportsZeroRunsKey { get; }

		[Field ("NSURLVolumeSupportsCaseSensitiveNamesKey")]
		NSString VolumeSupportsCaseSensitiveNamesKey { get; }

		[Field ("NSURLVolumeSupportsCasePreservedNamesKey")]
		NSString VolumeSupportsCasePreservedNamesKey { get; }

		// 5.0 Additions
		[Since (5,0)]
		[Field ("NSURLKeysOfUnsetValuesKey")]
		NSString KeysOfUnsetValuesKey { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceIdentifierKey")]
		NSString FileResourceIdentifierKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeIdentifierKey")]
		NSString VolumeIdentifierKey { get; }

		[Since (5,0)]
		[Field ("NSURLPreferredIOBlockSizeKey")]
		NSString PreferredIOBlockSizeKey { get; }

		[Since (5,0)]
		[Field ("NSURLIsReadableKey")]
		NSString IsReadableKey { get; }

		[Since (5,0)]
		[Field ("NSURLIsWritableKey")]
		NSString IsWritableKey { get; }

		[Since (5,0)]
		[Field ("NSURLIsExecutableKey")]
		NSString IsExecutableKey { get; }

		[Since (5,0)]
		[Field ("NSURLIsMountTriggerKey")]
		NSString IsMountTriggerKey { get; }

		[Since (5,0)]
		[Field ("NSURLFileSecurityKey")]
		NSString FileSecurityKey { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceTypeKey")]
		NSString FileResourceTypeKey { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceTypeNamedPipe")]
		NSString FileResourceTypeNamedPipe { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceTypeCharacterSpecial")]
		NSString FileResourceTypeCharacterSpecial { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceTypeDirectory")]
		NSString FileResourceTypeDirectory { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceTypeBlockSpecial")]
		NSString FileResourceTypeBlockSpecial { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceTypeRegular")]
		NSString FileResourceTypeRegular { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceTypeSymbolicLink")]
		NSString FileResourceTypeSymbolicLink { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceTypeSocket")]
		NSString FileResourceTypeSocket { get; }

		[Since (5,0)]
		[Field ("NSURLFileResourceTypeUnknown")]
		NSString FileResourceTypeUnknown { get; }

		[Since (5,0)]
		[Field ("NSURLTotalFileSizeKey")]
		NSString TotalFileSizeKey { get; }

		[Since (5,0)]
		[Field ("NSURLTotalFileAllocatedSizeKey")]
		NSString TotalFileAllocatedSizeKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeSupportsRootDirectoryDatesKey")]
		NSString VolumeSupportsRootDirectoryDatesKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeSupportsVolumeSizesKey")]
		NSString VolumeSupportsVolumeSizesKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeSupportsRenamingKey")]
		NSString VolumeSupportsRenamingKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeSupportsAdvisoryFileLockingKey")]
		NSString VolumeSupportsAdvisoryFileLockingKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeSupportsExtendedSecurityKey")]
		NSString VolumeSupportsExtendedSecurityKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeIsBrowsableKey")]
		NSString VolumeIsBrowsableKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeMaximumFileSizeKey")]
		NSString VolumeMaximumFileSizeKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeIsEjectableKey")]
		NSString VolumeIsEjectableKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeIsRemovableKey")]
		NSString VolumeIsRemovableKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeIsInternalKey")]
		NSString VolumeIsInternalKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeIsAutomountedKey")]
		NSString VolumeIsAutomountedKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeIsLocalKey")]
		NSString VolumeIsLocalKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeIsReadOnlyKey")]
		NSString VolumeIsReadOnlyKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeCreationDateKey")]
		NSString VolumeCreationDateKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeURLForRemountingKey")]
		NSString VolumeURLForRemountingKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeUUIDStringKey")]
		NSString VolumeUUIDStringKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeNameKey")]
		NSString VolumeNameKey { get; }

		[Since (5,0)]
		[Field ("NSURLVolumeLocalizedNameKey")]
		NSString VolumeLocalizedNameKey { get; }

		[Since (5,0)]
		[Field ("NSURLIsUbiquitousItemKey")]
		NSString IsUbiquitousItemKey { get; }

		[Since (5,0)]
		[Field ("NSURLUbiquitousItemHasUnresolvedConflictsKey")]
		NSString UbiquitousItemHasUnresolvedConflictsKey { get; }

		[Since (5,0)]
		[Field ("NSURLUbiquitousItemIsDownloadedKey")]
		NSString UbiquitousItemIsDownloadedKey { get; }

		[Since (5,0)]
		[Field ("NSURLUbiquitousItemIsDownloadingKey")]
		NSString UbiquitousItemIsDownloadingKey { get; }

		[Since (5,0)]
		[Field ("NSURLUbiquitousItemIsUploadedKey")]
		NSString UbiquitousItemIsUploadedKey { get; }

		[Since (5,0)]
		[Field ("NSURLUbiquitousItemIsUploadingKey")]
		NSString UbiquitousItemIsUploadingKey { get; }

		[Since (5,0)]
		[Field ("NSURLUbiquitousItemPercentDownloadedKey")]
		[Obsolete ("Deprecated in iOS 6.0. Use NSMetadataQuery.UbiquitousItemPercentDownloadedKey on NSMetadataItem")]
		NSString UbiquitousItemPercentDownloadedKey { get; }

		[Since (5,0)]
		[Obsolete ("Deprecated in iOS 6.0. Use NSMetadataQuery.NSMetadataUbiquitousItemPercentUploadedKey on NSMetadataItem")]
		[Field ("NSURLUbiquitousItemPercentUploadedKey")]
		NSString UbiquitousItemPercentUploadedKey { get; }

		[Since (5,1)]
		[MountainLion]
		[Field ("NSURLIsExcludedFromBackupKey")]
		NSString IsExcludedFromBackupKey { get; }

		[Export ("bookmarkDataWithOptions:includingResourceValuesForKeys:relativeToURL:error:")]
		NSData CreateBookmarkData (NSUrlBookmarkCreationOptions options, string [] resourceValues, [NullAllowed] NSUrl relativeUrl, out NSError error);

		[Export ("initByResolvingBookmarkData:options:relativeToURL:bookmarkDataIsStale:error:")]
		IntPtr Constructor (NSData bookmarkData, NSUrlBookmarkResolutionOptions resolutionOptions, [NullAllowed] NSUrl relativeUrl, out bool bookmarkIsStale, out NSError error);

		[Field ("NSURLPathKey")]
		[Since (6,0)][MountainLion]
		NSString PathKey { get; }
	}

	[BaseType (typeof (NSObject), Name="NSURLCache")]
	public interface NSUrlCache {
		[Export ("sharedURLCache"), Static]
		NSUrlCache SharedCache { get; set; }

		[Export ("initWithMemoryCapacity:diskCapacity:diskPath:")]
		IntPtr Constructor (nuint memoryCapacity, nuint diskCapacity, string diskPath);

		[Export ("cachedResponseForRequest:")]
		NSCachedUrlResponse CachedResponseForRequest (NSUrlRequest request);

		[Export ("storeCachedResponse:forRequest:")]
		void StoreCachedResponse (NSCachedUrlResponse cachedResponse, NSUrlRequest forRequest);

		[Export ("removeCachedResponseForRequest:")]
		void RemoveCachedResponse (NSUrlRequest request);

		[Export ("removeAllCachedResponses")]
		void RemoveAllCachedResponses ();

		[Export ("memoryCapacity")]
		nuint MemoryCapacity { get; set; }

		[Export ("diskCapacity")]
		nuint DiskCapacity { get; set; }

		[Export ("currentMemoryUsage")]
		nuint CurrentMemoryUsage { get; }

		[Export ("currentDiskUsage")]
		nuint CurrentDiskUsage { get; }
	}
	
	[BaseType (typeof (NSObject), Name="NSURLAuthenticationChallenge")]
	// 'init' returns NIL
	[DisableDefaultCtor]
	public interface NSUrlAuthenticationChallenge {
		[Export ("initWithProtectionSpace:proposedCredential:previousFailureCount:failureResponse:error:sender:")]
		IntPtr Constructor (NSUrlProtectionSpace space, NSUrlCredential credential, nint previousFailureCount, NSUrlResponse response, [NullAllowed] NSError error, NSUrlConnection sender);
		
		[Export ("initWithAuthenticationChallenge:sender:")]
		IntPtr Constructor (NSUrlAuthenticationChallenge  challenge, NSUrlConnection sender);
	
		[Export ("protectionSpace")]
		NSUrlProtectionSpace ProtectionSpace { get; }
	
		[Export ("proposedCredential")]
		NSUrlCredential ProposedCredential { get; }
	
		[Export ("previousFailureCount")]
		nint PreviousFailureCount { get; }
	
		[Export ("failureResponse")]
		NSUrlResponse FailureResponse { get; }
	
		[Export ("error")]
		NSError Error { get; }
	
		[Export ("sender")]
		NSUrlConnection Sender { get; }
	}

	public delegate void NSUrlConnectionDataResponse (NSUrlResponse response, NSData data, NSError error);
	
	[BaseType (typeof (NSObject), Name="NSURLConnection")]
	public interface NSUrlConnection {
		[Export ("canHandleRequest:")][Static]
		bool CanHandleRequest (NSUrlRequest request);
	
		[Export ("connectionWithRequest:delegate:")][Static]
		NSUrlConnection FromRequest (NSUrlRequest request, NSUrlConnectionDelegate connectionDelegate);
	
		[Export ("initWithRequest:delegate:")]
		IntPtr Constructor (NSUrlRequest request, NSUrlConnectionDelegate connectionDelegate);
	
		[Export ("initWithRequest:delegate:startImmediately:")]
		IntPtr Constructor (NSUrlRequest request, NSUrlConnectionDelegate connectionDelegate, bool startImmediately);
	
		[Export ("start")]
		void Start ();
	
		[Export ("cancel")]
		void Cancel ();
	
		[Export ("scheduleInRunLoop:forMode:")]
		void Schedule (NSRunLoop aRunLoop, NSString forMode);
	
		[Export ("unscheduleFromRunLoop:forMode:")]
		void Unschedule (NSRunLoop aRunLoop, NSString forMode);

		/* Adopted by the NSUrlAuthenticationChallengeSender protocol */
		[Export ("useCredential:forAuthenticationChallenge:")]
		void UseCredentials (NSUrlCredential credential, NSUrlAuthenticationChallenge challenge);
	
		[Export ("continueWithoutCredentialForAuthenticationChallenge:")]
		void ContinueWithoutCredentialForAuthenticationChallenge (NSUrlAuthenticationChallenge  challenge);
	
		[Export ("cancelAuthenticationChallenge:")]
		void CancelAuthenticationChallenge (NSUrlAuthenticationChallenge  challenge);

		[Since (5,0)]
		[Export ("performDefaultHandlingForAuthenticationChallenge:")]
		void PerformDefaultHandlingForChallenge (NSUrlAuthenticationChallenge challenge);
		
		[Since (5,0)]
		[Export ("rejectProtectionSpaceAndContinueWithChallenge:")]
		void RejectProtectionSpaceAndContinueWithChallenge (NSUrlAuthenticationChallenge challenge);

#if !MONOMAC
		[Since (5,0)]
		[Export ("originalRequest")]
		NSUrlRequest OriginalRequest { get; }

		[Since (5,0)]
		[Export ("currentRequest")]
		NSUrlRequest CurrentRequest { get; }
#endif
		[Export ("setDelegateQueue:")]
		[Since (5,0)]
		void SetDelegateQueue (NSOperationQueue queue);
		
		[Since (5,0)]
		[Static]
		[Export ("sendAsynchronousRequest:queue:completionHandler:")]
		[Async (ResultTypeName = "NSUrlAsyncResult", MethodName="SendRequestAsync")]
		void SendAsynchronousRequest (NSUrlRequest request, NSOperationQueue queue, NSUrlConnectionDataResponse completionHandler);
		
#if !MONOMAC
		// Extension from iOS5, NewsstandKit
		[Export ("newsstandAssetDownload")]
		MonoTouch.NewsstandKit.NKAssetDownload NewsstandAssetDownload { get; }
#endif
	}

	[BaseType (typeof (NSObject), Name="NSURLConnectionDelegate")]
	[Model]
	[Protocol]
	public interface NSUrlConnectionDelegate {
		[Export ("connection:willSendRequest:redirectResponse:")]
		NSUrlRequest WillSendRequest (NSUrlConnection connection, NSUrlRequest request, NSUrlResponse response);

		[Export ("connection:canAuthenticateAgainstProtectionSpace:")]
		bool CanAuthenticateAgainstProtectionSpace (NSUrlConnection connection, NSUrlProtectionSpace protectionSpace);

		[Export ("connection:needNewBodyStream:")]
		NSInputStream NeedNewBodyStream (NSUrlConnection connection, NSUrlRequest request);

		[Export ("connection:didReceiveAuthenticationChallenge:")]
		void ReceivedAuthenticationChallenge (NSUrlConnection connection, NSUrlAuthenticationChallenge challenge);

		[Export ("connection:didCancelAuthenticationChallenge:")]
		void CanceledAuthenticationChallenge (NSUrlConnection connection, NSUrlAuthenticationChallenge challenge);

		[Export ("connectionShouldUseCredentialStorage:")]
		bool ConnectionShouldUseCredentialStorage (NSUrlConnection connection);

		[Export ("connection:didReceiveResponse:")]
		void ReceivedResponse (NSUrlConnection connection, NSUrlResponse response);

		[Export ("connection:didReceiveData:")]
		void ReceivedData (NSUrlConnection connection, NSData data);

		[Export ("connection:didSendBodyData:totalBytesWritten:totalBytesExpectedToWrite:")]
		void SentBodyData (NSUrlConnection connection, nint bytesWritten, nint totalBytesWritten, nint totalBytesExpectedToWrite);

		[Export ("connectionDidFinishLoading:")]
		void FinishedLoading (NSUrlConnection connection);

		[Export ("connection:didFailWithError:")]
		void FailedWithError (NSUrlConnection connection, NSError error);

		[Export ("connection:willCacheResponse:")]
		NSCachedUrlResponse WillCacheResponse (NSUrlConnection connection, NSCachedUrlResponse cachedResponse);
	}

	[BaseType (typeof (NSUrlConnectionDelegate), Name="NSUrlConnectionDownloadDelegate")]
	[Model]
	[Protocol]
	public interface NSUrlConnectionDownloadDelegate {
		[Export ("connection:didWriteData:totalBytesWritten:expectedTotalBytes:")]
		void WroteData (NSUrlConnection connection, long bytesWritten, long totalBytesWritten, long expectedTotalBytes);
		
		[Export ("connectionDidResumeDownloading:totalBytesWritten:expectedTotalBytes:")]
		void ResumedDownloading (NSUrlConnection connection, long totalBytesWritten, long expectedTotalBytes);
		
		[Abstract]
		[Export ("connectionDidFinishDownloading:destinationURL:")]
		void FinishedDownloading (NSUrlConnection connection, NSUrl destinationUrl);
	}
		
	[BaseType (typeof (NSObject), Name="NSURLCredential")]
	// crash when calling NSObjecg.get_Description (and likely other selectors)
	[DisableDefaultCtor]
	public interface NSUrlCredential {
		[Export ("persistence")]
		NSUrlCredentialPersistence Persistence { get; }

		[Export ("initWithUser:password:persistence:")]
		IntPtr Constructor (string  user, string password, NSUrlCredentialPersistence persistence);
	
		[Static]
		[Export ("credentialWithUser:password:persistence:")]
		NSUrlCredential FromUserPasswordPersistance (string user, string password, NSUrlCredentialPersistence persistence);

		[Export ("user")]
		string User { get; }
	
		[Export ("password")]
		string Password { get; }
	
		[Export ("hasPassword")]
		bool HasPassword {get; }
	
		//[Export ("initWithIdentity:certificates:persistence:")]
		//IntPtr Constructor (IntPtr SecIdentityRef, IntPtr [] secCertificateRefArray, NSUrlCredentialPersistence persistance);
	
		//[Static]
		//[Export ("credentialWithIdentity:certificates:persistence:")]
		//NSUrlCredential FromIdentityCertificatesPersistance (IntPtr SecIdentityRef, IntPtr [] secCertificateRefArray, NSUrlCredentialPersistence persistence);
	
		[Export ("identity")]
		IntPtr Identity  {get; }
	
		//[Export ("certificates")]
		//IntPtr [] Certificates { get; }
	
		// bound manually to keep the managed/native signatures identical
		//[Export ("initWithTrust:")]
		//IntPtr Constructor (IntPtr SecTrustRef_trust, bool ignored);
	
		[Static]
		[Export ("credentialForTrust:")]
		NSUrlCredential FromTrust (IntPtr SecTrustRef_trust);
	
	}

	[BaseType (typeof (NSObject), Name="NSURLCredentialStorage")]
	// init returns NIL -> SharedCredentialStorage
	[DisableDefaultCtor]
	public interface NSUrlCredentialStorage {
		[Static]
		[Export ("sharedCredentialStorage")]
		NSUrlCredentialStorage SharedCredentialStorage { get; }

		[Export ("credentialsForProtectionSpace:")]
		NSDictionary GetCredentials (NSUrlProtectionSpace forProtectionSpace);

		[Export ("allCredentials")]
		NSDictionary AllCredentials { get; }

		[Export ("setCredential:forProtectionSpace:")]
		void SetCredential (NSUrlCredential credential, NSUrlProtectionSpace forProtectionSpace);

		[Export ("removeCredential:forProtectionSpace:")]
		void RemoveCredential (NSUrlCredential credential, NSUrlProtectionSpace forProtectionSpace);

		[Export ("defaultCredentialForProtectionSpace:")]
		NSUrlCredential GetDefaultCredential (NSUrlProtectionSpace forProtectionSpace);

		[Export ("setDefaultCredential:forProtectionSpace:")]
		void SetDefaultCredential (NSUrlCredential credential, NSUrlProtectionSpace forProtectionSpace);
	}

	interface NSUndoManagerCloseUndoGroupEventArgs {
		// Bug in docs, see header file
		[Export ("NSUndoManagerGroupIsDiscardableKey")]
		[NullAllowed]
		bool Discardable { get; }
	}
	
	[BaseType (typeof (NSObject))]
	public interface NSUndoManager {
		[Export ("beginUndoGrouping")]
		void BeginUndoGrouping ();
		
		[Export ("endUndoGrouping")]
		void EndUndoGrouping ();
		
		[Export ("groupingLevel")]
		nint GroupingLevel { get; }
		
		[Export ("disableUndoRegistration")]
		void DisableUndoRegistration ();

		[Export ("enableUndoRegistration")]
		void EnableUndoRegistration ();

		[Export ("isUndoRegistrationEnabled")]
		bool IsUndoRegistrationEnabled { get; }
		
		[Export ("groupsByEvent")]
		bool GroupsByEvent { get; set; }
		
		[Export ("levelsOfUndo")]
		nint LevelsOfUndo { get; set; }
		
		[Export ("runLoopModes")]
		string [] RunLoopModes { get; set; } 
		
		[Export ("undo")]
		void Undo ();
		
		[Export ("redo")]
		void Redo ();
		
		[Export ("undoNestedGroup")]
		void UndoNestedGroup ();
		
		[Export ("canUndo")]
		bool CanUndo { get; }
		
		[Export ("canRedo")]
		bool CanRedo { get; }

		[Export ("isUndoing")]
		bool IsUndoing { get; }

		[Export ("isRedoing")]
		bool IsRedoing { get; }

		[Export ("removeAllActions")]
		void RemoveAllActions ();

		[Export ("removeAllActionsWithTarget:")]
		void RemoveAllActions (NSObject target);

		[Export ("registerUndoWithTarget:selector:object:")]
		void RegisterUndoWithTarget (NSObject target, Selector selector, NSObject anObject);

		[Export ("prepareWithInvocationTarget:")]
		NSObject PrepareWithInvocationTarget (NSObject target);

		[Export ("undoActionName")]
		string UndoActionName { get; }

		[Export ("redoActionName")]
		string RedoActionName { get; }

		[Export ("setActionName:")]
		void SetActionname (string actionName);

		[Export ("undoMenuItemTitle")]
		string UndoMenuItemTitle { get; }

		[Export ("redoMenuItemTitle")]
		string RedoMenuItemTitle { get; }

		[Export ("undoMenuTitleForUndoActionName:")]
		string UndoMenuTitleForUndoActionName (string name);

		[Export ("redoMenuTitleForUndoActionName:")]
		string RedoMenuTitleForUndoActionName (string name);

		[Field ("NSUndoManagerCheckpointNotification")]
		[Notification]
		NSString CheckpointNotification { get; }

		[Field ("NSUndoManagerDidOpenUndoGroupNotification")]
		[Notification]
		NSString DidOpenUndoGroupNotification { get; }

		[Field ("NSUndoManagerDidRedoChangeNotification")]
		[Notification]
		NSString DidRedoChangeNotification { get; }

		[Field ("NSUndoManagerDidUndoChangeNotification")]
		[Notification]
		NSString DidUndoChangeNotification { get; }

		[Field ("NSUndoManagerWillCloseUndoGroupNotification")]
		[Notification (typeof (NSUndoManagerCloseUndoGroupEventArgs))]
		NSString WillCloseUndoGroupNotification { get; }

		[Field ("NSUndoManagerWillRedoChangeNotification")]
		[Notification]
		NSString WillRedoChangeNotification { get; }

		[Field ("NSUndoManagerWillUndoChangeNotification")]
		[Notification]
		NSString WillUndoChangeNotification { get; }

		[Since (5,0)]
		[Export ("setActionIsDiscardable:")]
		void SetActionIsDiscardable (bool discardable);

		[Since (5,0)]
		[Export ("undoActionIsDiscardable")]
		bool UndoActionIsDiscardable { get; }

		[Since (5,0)]
		[Export ("redoActionIsDiscardable")]
		bool RedoActionIsDiscardable { get; }

		[Field ("NSUndoManagerGroupIsDiscardableKey")]
		NSString GroupIsDiscardableKey { get; }

		[Field ("NSUndoManagerDidCloseUndoGroupNotification")]
		[Notification (typeof (NSUndoManagerCloseUndoGroupEventArgs))]
		NSString DidCloseUndoGroupNotification { get; }
	}
	
	[BaseType (typeof (NSObject), Name="NSURLProtectionSpace")]
	// 'init' returns NIL
	[DisableDefaultCtor]
	public interface NSUrlProtectionSpace {
		
		[Export ("initWithHost:port:protocol:realm:authenticationMethod:")]
		IntPtr Constructor (string host, nint port, string protocol, string realm, string authenticationMethod);
	
		//[Export ("initWithProxyHost:port:type:realm:authenticationMethod:")]
		//IntPtr Constructor (string  host, int port, string type, string  realm, string authenticationMethod);
	
		[Export ("realm")]
		string Realm { get; }
	
		[Export ("receivesCredentialSecurely")]
		bool ReceivesCredentialSecurely { get; }
	
		[Export ("isProxy")]
		bool IsProxy { get; }
	
		[Export ("host")]
		string Host { get; }
	
		[Export ("port")]
		nint  Port { get; }
	
		[Export ("proxyType")]
		string ProxyType { get; }
	
		[Export ("protocol")]
		string Protocol { get; }
	
		[Export ("authenticationMethod")]
		string AuthenticationMethod { get; }

		// NSURLProtectionSpace(NSClientCertificateSpace)

		[Export ("distinguishedNames")]
		NSData [] DistinguishedNames { get; }
		
		// NSURLProtectionSpace(NSServerTrustValidationSpace)
		[Export ("serverTrust")]
		IntPtr ServerTrust { get ; }

		[Field ("NSURLProtectionSpaceHTTP")]
		NSString HTTP { get; }
		[Field ("NSURLProtectionSpaceHTTPS")]
		NSString HTTPS { get; }
		[Field ("NSURLProtectionSpaceFTP")]
		NSString FTP { get; }
		[Field ("NSURLProtectionSpaceHTTPProxy")]
		NSString HTTPProxy { get; }
		[Field ("NSURLProtectionSpaceHTTPSProxy")]
		NSString HTTPSProxy { get; }
		[Field ("NSURLProtectionSpaceFTPProxy")]
		NSString FTPProxy { get; }
		[Field ("NSURLProtectionSpaceSOCKSProxy")]
		NSString SOCKSProxy { get; }

		[Field ("NSURLAuthenticationMethodDefault")]
		NSString AuthenticationMethodDefault { get; }

		[Field ("NSURLAuthenticationMethodHTTPBasic")]
		NSString AuthenticationMethodHTTPBasic { get; }

		[Field ("NSURLAuthenticationMethodHTTPDigest")]
		NSString AuthenticationMethodHTTPDigest { get; }

		[Field ("NSURLAuthenticationMethodHTMLForm")]
		NSString AuthenticationMethodHTMLForm { get; }

		[Field ("NSURLAuthenticationMethodNTLM")]
		NSString AuthenticationMethodNTL { get; }

		[Field ("NSURLAuthenticationMethodNegotiate")]
		NSString AuthenticationMethodNegotiat { get; }

		[Field ("NSURLAuthenticationMethodClientCertificate")]
		NSString AuthenticationMethodClientCertificate { get; }

		[Field ("NSURLAuthenticationMethodServerTrust")]
		NSString AuthenticationMethodServerTrus { get; }
	}
	
	[BaseType (typeof (NSObject), Name="NSURLRequest")]
	public interface NSUrlRequest {
		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("initWithURL:cachePolicy:timeoutInterval:")]
		IntPtr Constructor (NSUrl url, NSUrlRequestCachePolicy cachePolicy, double timeoutInterval);

		[Export ("requestWithURL:")][Static]
		NSUrlRequest FromUrl (NSUrl url);

		[Export ("URL")]
		NSUrl Url { get; }

		[Export ("cachePolicy")]
		NSUrlRequestCachePolicy CachePolicy { get; }

		[Export ("timeoutInterval")]
		double TimeoutInterval { get; }

		[Export ("mainDocumentURL")]
		NSUrl MainDocumentURL { get; }

		[Export ("networkServiceType")]
		NSUrlRequestNetworkServiceType NetworkServiceType { get; }

		[Export ("allowsCellularAccess")]
		bool AllowsCellularAccess { get; }
		
		[Export ("HTTPMethod")]
		string HttpMethod { get; }

		[Export ("allHTTPHeaderFields")]
		NSDictionary Headers { get; }

		[Internal][Export ("valueForHTTPHeaderField:")]
		string Header (string field);

		[Export ("HTTPBody")]
		NSData Body { get; }

		[Export ("HTTPBodyStream")]
		NSInputStream BodyStream { get; }

		[Export ("HTTPShouldHandleCookies")]
		bool ShouldHandleCookies { get; }
	}

	[BaseType (typeof (NSDictionary))]
	public interface NSMutableDictionary {
		[Export ("dictionaryWithContentsOfFile:")]
		[Static]
		NSMutableDictionary FromFile (string path);

		[Export ("dictionaryWithContentsOfURL:")]
		[Static]
		NSMutableDictionary FromUrl (NSUrl url);

		[Export ("dictionaryWithObject:forKey:")]
		[Static]
		NSMutableDictionary FromObjectAndKey (NSObject obj, NSObject key);

		[Export ("dictionaryWithDictionary:")]
		[Static,New]
		NSMutableDictionary FromDictionary (NSDictionary source);

		[Export ("dictionaryWithObjects:forKeys:count:")]
		[Static, Internal]
		NSMutableDictionary FromObjectsAndKeysInternalCount (NSArray objects, NSArray keys, nuint count);

		[Export ("dictionaryWithObjects:forKeys:")]
		[Static, Internal, New]
		NSMutableDictionary FromObjectsAndKeysInternal (NSArray objects, NSArray Keys);
		
		[Export ("initWithDictionary:")]
		IntPtr Constructor (NSDictionary other);

		[Export ("initWithContentsOfFile:")]
		IntPtr Constructor (string fileName);

		[Export ("initWithContentsOfURL:")]
		IntPtr Constructor (NSUrl url);
		
		[Export ("removeAllObjects"), Internal]
		void RemoveAllObjects ();

		[Export ("removeObjectForKey:"), Internal]
		void RemoveObjectForKey (NSObject key);

		[Export ("setObject:forKey:"), Internal]
		void SetObject (NSObject obj, NSObject key);

		[Since (6,0)]
		[Static]
		[Export ("dictionaryWithSharedKeySet:")]
		NSDictionary FromSharedKeySet (NSObject sharedKeyToken);
	}

	[BaseType (typeof (NSSet))]
	public interface NSMutableSet {
		[Export ("initWithArray:")]
		IntPtr Constructor (NSArray other);

		[Export ("initWithSet:")]
		IntPtr Constructor (NSSet other);
		
		[Export ("initWithCapacity:")]
		IntPtr Constructor (nuint capacity);

		[Export ("addObject:")]
		void Add (NSObject nso);

		[Export ("removeObject:")]
		void Remove (NSObject nso);

		[Export ("removeAllObjects")]
		void RemoveAll ();

		[Export ("addObjectsFromArray:")]
		void AddObjects (NSObject [] objects);

		[Internal, Export ("minusSet:")]
		void MinusSet (NSSet other);
	}
	
	[BaseType (typeof (NSUrlRequest), Name="NSMutableURLRequest")]
	public interface NSMutableUrlRequest {
		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl url);

		[Export ("initWithURL:cachePolicy:timeoutInterval:")]
		IntPtr Constructor (NSUrl url, NSUrlRequestCachePolicy cachePolicy, double timeoutInterval);

		[New][Export ("URL")]
		NSUrl Url { get; set; }

		[New][Export ("cachePolicy")]
		NSUrlRequestCachePolicy CachePolicy { get; set; }

		[New][Export ("timeoutInterval")]
		double TimeoutInterval { set; get; }

		[New][Export ("mainDocumentURL")]
		NSUrl MainDocumentURL { get; set; }

		[New][Export ("HTTPMethod")]
		string HttpMethod { get; set; }

		[New][Export ("allHTTPHeaderFields")]
		NSDictionary Headers { get; set; }

		[Internal][Export ("setValue:forHTTPHeaderField:")]
		void _SetValue (string value, string field);

		[New][Export ("HTTPBody")]
		NSData Body { get; set; }

		[New][Export ("HTTPBodyStream")]
		NSInputStream BodyStream { get; set; }

		[New][Export ("HTTPShouldHandleCookies")]
		bool ShouldHandleCookies { get; set; }

		[Export ("networkServiceType")]
		NSUrlRequestNetworkServiceType NetworkServiceType { set; get; }
		
		[New] [Export ("allowsCellularAccess")]
		bool AllowsCellularAccess { get; set; }
	}
	
	[BaseType (typeof (NSObject), Name="NSURLResponse")]
	public interface NSUrlResponse {
		[Export ("initWithURL:MIMEType:expectedContentLength:textEncodingName:")]
		IntPtr Constructor (NSUrl url, string mimetype, nint expectedContentLength, [NullAllowed] string textEncodingName);

		[Export ("URL")]
		NSUrl Url { get; }

		[Export ("MIMEType")]
		string MimeType { get; }

		[Export ("expectedContentLength")]
		long ExpectedContentLength { get; }

		[Export ("textEncodingName")]
		string TextEncodingName { get; }

		[Export ("suggestedFilename")]
		string SuggestedFilename { get; }
	}

	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSStreamDelegate)} )]
	public interface NSStream {
		[Export ("open")]
		void Open ();

		[Export ("close")]
		void Close ();
	
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSStreamDelegate Delegate { get; set; }

		[Export ("propertyForKey:"), Internal]
		NSObject PropertyForKey (NSString key);
	
		[Export ("setProperty:forKey:"), Internal]
		bool SetPropertyForKey (NSObject property, NSString key);
	
		[Export ("scheduleInRunLoop:forMode:")]
		void Schedule (NSRunLoop aRunLoop, string mode);
	
		[Export ("removeFromRunLoop:forMode:")]
		void Unschedule (NSRunLoop aRunLoop, string mode);
	
		[Export ("streamStatus")]
		NSStreamStatus Status { get; }
	
		[Export ("streamError")]
		NSError Error { get; }


		[Field ("NSStreamSocketSecurityLevelKey")]
		NSString SocketSecurityLevelKey { get; }

		[Field ("NSStreamSocketSecurityLevelNone")]
		NSString SocketSecurityLevelNone { get; }

		[Field ("NSStreamSocketSecurityLevelSSLv2")]
		NSString SocketSecurityLevelSslV2 { get; }

		[Field ("NSStreamSocketSecurityLevelSSLv3")]
		NSString SocketSecurityLevelSslV3 { get; }

		[Field ("NSStreamSocketSecurityLevelTLSv1")]
		NSString SocketSecurityLevelTlsV1 { get; }

		[Field ("NSStreamSocketSecurityLevelNegotiatedSSL")]
		NSString SocketSecurityLevelNegotiatedSsl { get; }

		[Field ("NSStreamSOCKSProxyConfigurationKey")]
		NSString SocksProxyConfigurationKey { get; }

		[Field ("NSStreamSOCKSProxyHostKey")]
		NSString SocksProxyHostKey { get; }

		[Field ("NSStreamSOCKSProxyPortKey")]
		NSString SocksProxyPortKey { get; }

		[Field ("NSStreamSOCKSProxyVersionKey")]
		NSString SocksProxyVersionKey { get; }

		[Field ("NSStreamSOCKSProxyUserKey")]
		NSString SocksProxyUserKey { get; }

		[Field ("NSStreamSOCKSProxyPasswordKey")]
		NSString SocksProxyPasswordKey { get; }

		[Field ("NSStreamSOCKSProxyVersion4")]
		NSString SocksProxyVersion4 { get; }

		[Field ("NSStreamSOCKSProxyVersion5")]
		NSString SocksProxyVersion5 { get; }

		[Field ("NSStreamDataWrittenToMemoryStreamKey")]
		NSString DataWrittenToMemoryStreamKey { get; }

		[Field ("NSStreamFileCurrentOffsetKey")]
		NSString FileCurrentOffsetKey { get; }

		[Field ("NSStreamSocketSSLErrorDomain")]
		NSString SocketSslErrorDomain { get; }

		[Field ("NSStreamSOCKSErrorDomain")]
		NSString SocksErrorDomain { get; }

		[Field ("NSStreamNetworkServiceType")]
		NSString NetworkServiceType { get; }

		[Field ("NSStreamNetworkServiceTypeVoIP")]
		NSString NetworkServiceTypeVoIP { get; }

		[Field ("NSStreamNetworkServiceTypeVideo")]
		NSString NetworkServiceTypeVideo { get; }

		[Field ("NSStreamNetworkServiceTypeBackground")]
		NSString NetworkServiceTypeBackground { get; }

		[Field ("NSStreamNetworkServiceTypeVoice")]
		NSString NetworkServiceTypeVoice { get; }
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	public interface NSStreamDelegate {
		[Export ("stream:handleEvent:"), EventArgs ("NSStream"), EventName ("OnEvent")]
		void HandleEvent (NSStream theStream, NSStreamEvent streamEvent);
	}

	[BaseType (typeof (NSObject)), Bind ("NSString")]
	public interface NSString2 {
#if MONOMAC
		[Bind ("sizeWithAttributes:")]
		CGSize StringSize ([NullAllowed] NSDictionary attributedStringAttributes);
		
		[Bind ("boundingRectWithSize:options:attributes:")]
		CGRect BoundingRectWithSize (CGSize size, NSStringDrawingOptions options, NSDictionary attributes);
		
		[Bind ("drawAtPoint:withAttributes:")]
		void DrawString (CGPoint point, NSDictionary attributes);
		
		[Bind ("drawInRect:withAttributes:")]
		void DrawString (CGRect rect, NSDictionary attributes);
		
		[Bind ("drawWithRect:options:attributes:")]
		void DrawString (CGRect rect, NSStringDrawingOptions options, NSDictionary attributes);
#else
		[Bind ("sizeWithFont:")]
		CGSize StringSize (UIFont font);
		
		[Bind ("sizeWithFont:forWidth:lineBreakMode:")]
		CGSize StringSize (UIFont font, float forWidth, UILineBreakMode breakMode);
		
		[Bind ("sizeWithFont:constrainedToSize:")]
		CGSize StringSize (UIFont font, CGSize constrainedToSize);
		
		[Bind ("sizeWithFont:constrainedToSize:lineBreakMode:")]
		CGSize StringSize (UIFont font, CGSize constrainedToSize, UILineBreakMode lineBreakMode);

		[Bind ("sizeWithFont:minFontSize:actualFontSize:forWidth:lineBreakMode:")]
		CGSize StringSize (UIFont font, float minFontSize, ref float actualFontSize, float forWidth, UILineBreakMode lineBreakMode);

		[Bind ("drawAtPoint:withFont:")]
		CGSize DrawString (CGPoint point, UIFont font);

		[Bind ("drawAtPoint:forWidth:withFont:lineBreakMode:")]
		CGSize DrawString (CGPoint point, float width, UIFont font, UILineBreakMode breakMode);

		[Bind ("drawAtPoint:forWidth:withFont:fontSize:lineBreakMode:baselineAdjustment:")]
		CGSize DrawString (CGPoint point, float width, UIFont font, float fontSize, UILineBreakMode breakMode, UIBaselineAdjustment adjustment);

		[Bind ("drawAtPoint:forWidth:withFont:minFontSize:actualFontSize:lineBreakMode:baselineAdjustment:")]
		CGSize DrawString (CGPoint point, float width, UIFont font, float minFontSize, ref float actualFontSize, UILineBreakMode breakMode, UIBaselineAdjustment adjustment);

		[Bind ("drawInRect:withFont:")]
		CGSize DrawString (CGRect rect, UIFont font);

		[Bind ("drawInRect:withFont:lineBreakMode:")]
		CGSize DrawString (CGRect rect, UIFont font, UILineBreakMode mode);

		[Bind ("drawInRect:withFont:lineBreakMode:alignment:")]
		CGSize DrawString (CGRect rect, UIFont font, UILineBreakMode mode, UITextAlignment alignment);
#endif
		[Export ("characterAtIndex:")]
		char _characterAtIndex (nuint index);

		[Export ("length")]
		nuint Length {get;}

		[Export ("hash"), Internal]
		nuint Hash ();

		[Export ("isEqualToString:"), Internal]
		bool IsEqualTo (IntPtr handle);
		
		[Export ("compare:")]
		NSComparisonResult Compare (NSString aString);

		[Export ("compare:options:")]
		NSComparisonResult Compare (NSString aString, NSStringCompareOptions mask);

		[Export ("compare:options:range:")]
		NSComparisonResult Compare (NSString aString, NSStringCompareOptions mask, NSRange range);

		[Export ("compare:options:range:locale:")]
		NSComparisonResult Compare (NSString aString, NSStringCompareOptions mask, NSRange range, [NullAllowed] NSLocale locale);
		
		[Export ("stringByReplacingCharactersInRange:withString:")]
		NSString Replace (NSRange range, NSString replacement);

		// start methods from NSStringPathExtensions category

		[Static]
		[Export("pathWithComponents:")]
		string[] PathWithComponents( string[] components );

		[Export("pathComponents")]
		string[] PathComponents { get; }

		[Export("isAbsolutePath")]
		bool IsAbsolutePath { get; }

		[Export("lastPathComponent")]
		NSString LastPathComponent { get; }

		[Export("stringByDeletingLastPathComponent")]
		NSString DeleteLastPathComponent();
 
 		[Export("stringByAppendingPathComponent:")]
 		NSString AppendPathComponent( NSString str );

 		[Export("pathExtension")]
 		NSString PathExtension { get; }

 		[Export("stringByDeletingPathExtension")]
 		NSString DeletePathExtension();

 		[Export("stringByAppendingPathExtension:")]
 		NSString AppendPathExtension( NSString str );
 
 		[Export("stringByAbbreviatingWithTildeInPath")]
 		NSString AbbreviateTildeInPath();

 		[Export("stringByExpandingTildeInPath")]
 		NSString ExpandTildeInPath();
 
 		[Export("stringByStandardizingPath")]
 		NSString StandarizePath();

 		[Export("stringByResolvingSymlinksInPath")]
 		NSString ResolveSymlinksInPath();

		[Export("stringsByAppendingPaths:")]
 		string[] AppendPaths( string[] paths );

		// end methods from NSStringPathExtensions category

		[Since (6,0)]
		[Export ("capitalizedStringWithLocale:")]
		string Capitalize (NSLocale locale);
		
		[Since (6,0)]
		[Export ("lowercaseStringWithLocale:")]
		string ToLower (NSLocale locale);
		
		[Since (6,0)]
		[Export ("uppercaseStringWithLocale:")]
		string ToUpper (NSLocale locale);
	}
#if !MONOMAC
	[Since (6,0)]
	[BaseType (typeof (NSObject))]
	public interface NSStringDrawingContext {
		[Export ("minimumScaleFactor")]
		float MinimumScaleFactor { get; set;  }

		[Export ("minimumTrackingAdjustment")]
		float MinimumTrackingAdjustment { get; set;  }

		[Export ("actualScaleFactor")]
		float ActualScaleFactor { get;  }

		[Export ("actualTrackingAdjustment")]
		float ActualTrackingAdjustment { get;  }

		[Export ("totalBounds")]
		CGRect TotalBounds { get;  }
	}
#endif
	[BaseType (typeof (NSStream))]
	[DefaultCtorVisibility (Visibility.Protected)]
	public interface NSInputStream {
		[Export ("hasBytesAvailable")]
		bool HasBytesAvailable ();
	
		[Export ("initWithFileAtPath:")]
		IntPtr Constructor (string path);

		[Export ("initWithData:")]
		IntPtr Constructor (NSData data);

		[Export ("initWithURL:")]
		IntPtr Constructor (NSUrl url);

		[Static]
		[Export ("inputStreamWithData:")]
		NSInputStream FromData (NSData data);
	
		[Static]
		[Export ("inputStreamWithFileAtPath:")]
		NSInputStream FromFile (string  path);

		[Static]
		[Export ("inputStreamWithURL:")]
		NSInputStream FromUrl (NSUrl url);
	}

	//
	// We expose NSString versions of these methods because it could
	// avoid an extra lookup in cases where there is a large volume of
	// calls being made and the keys are mostly tokens
	//
	[BaseType (typeof (NSObject)), Bind ("NSObject")]
	public interface NSObject2 {
		[Export ("observeValueForKeyPath:ofObject:change:context:")]
		void ObserveValue (NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context);

		[Export ("addObserver:forKeyPath:options:context:")]
		void AddObserver (NSObject observer, NSString keyPath, NSKeyValueObservingOptions options, IntPtr context);

		[Export ("removeObserver:forKeyPath:")]
		void RemoveObserver (NSObject observer, NSString keyPath);

		[Export ("willChangeValueForKey:")]
		void WillChangeValue (string forKey);

		[Export ("didChangeValueForKey:")]
		void DidChangeValue (string forKey);

		[Export ("willChange:valuesAtIndexes:forKey:")]
		void WillChange (NSKeyValueChange changeKind, NSIndexSet indexes, NSString forKey);

		[Export ("didChange:valuesAtIndexes:forKey:")]
		void DidChange (NSKeyValueChange changeKind, NSIndexSet indexes, NSString forKey);

		[Export ("willChangeValueForKey:withSetMutation:usingObjects:")]
		void WillChange (NSString forKey, NSKeyValueSetMutationKind mutationKind, NSSet objects);

		[Export ("didChangeValueForKey:withSetMutation:usingObjects:")]
		void DidChange (NSString forKey, NSKeyValueSetMutationKind mutationKind, NSSet objects);

		[Static, Export ("keyPathsForValuesAffectingValueForKey:")]
		NSSet GetKeyPathsForValuesAffecting (NSString key);

		[Static, Export ("automaticallyNotifiesObserversForKey:")]
		bool AutomaticallyNotifiesObserversForKey (string key);

		[Export ("valueForKey:")]
		[MarshalNativeExceptions]
		NSObject ValueForKey (NSString key);

		[Export ("setValue:forKey:")]
		void SetValueForKey (NSObject value, NSString key);

		[Export ("valueForKeyPath:")]
		NSObject ValueForKeyPath (NSString keyPath);

		[Export ("setValue:forKeyPath:")]
		void SetValueForKeyPath (NSObject value, NSString keyPath);

		[Export ("valueForUndefinedKey:")]
		NSObject ValueForUndefinedKey (NSString key);

		[Export ("setValue:forUndefinedKey:")]
		void SetValueForUndefinedKey (NSObject value, NSString undefinedKey);

		[Export ("setNilValueForKey:")]
		void SetNilValueForKey (NSString key);

		[Export ("dictionaryWithValuesForKeys:")]
		NSDictionary GetDictionaryOfValuesFromKeys (NSString [] keys);

		[Export ("setValuesForKeysWithDictionary:")]
		void SetValuesForKeysWithDictionary (NSDictionary keyedValues);
		
		[Field ("NSKeyValueChangeKindKey")]
		NSString ChangeKindKey { get; }

		[Field ("NSKeyValueChangeNewKey")]
		NSString ChangeNewKey { get; }

		[Field ("NSKeyValueChangeOldKey")]
		NSString ChangeOldKey { get; }

		[Field ("NSKeyValueChangeIndexesKey")]
		NSString ChangeIndexesKey { get; }

		[Field ("NSKeyValueChangeNotificationIsPriorKey")]
		NSString ChangeNotificationIsPriorKey { get; }
#if MONOMAC
		// Cocoa Bindings added by Kenneth J. Pouncey 2010/11/17
		[Export ("exposedBindings")]
		NSString[] ExposedBindings ();

		[Export ("valueClassForBinding:")]
		Class BindingValueClass (string binding);

		[Export ("bind:toObject:withKeyPath:options:")]
		void Bind (string binding, NSObject observable, string keyPath, [NullAllowed] NSDictionary options);

		[Export ("unbind:")]
		void Unbind (string binding);

		[Export ("infoForBinding:")]
		NSDictionary BindingInfo (string binding);

		[Export ("optionDescriptionsForBinding:")]
		NSObject[] BindingOptionDescriptions (string aBinding);

		// NSPlaceholders (informal) protocol
		[Static]
		[Export ("defaultPlaceholderForMarker:withBinding:")]
		NSObject GetDefaultPlaceholder (NSObject marker, string binding);

		[Static]
		[Export ("setDefaultPlaceholder:forMarker:withBinding:")]
		void SetDefaultPlaceholder (NSObject placeholder, NSObject marker, string binding);

		[Export ("objectDidEndEditing:")]
		void ObjectDidEndEditing (NSObject editor);

		[Export ("commitEditing")]
		bool CommitEditing ();

		[Export ("commitEditingWithDelegate:didCommitSelector:contextInfo:")]
		//void CommitEditingWithDelegateDidCommitSelectorContextInfo (NSObject objDelegate, Selector didCommitSelector, IntPtr contextInfo);
		void CommitEditing (NSObject objDelegate, Selector didCommitSelector, IntPtr contextInfo);
#endif
		[Export ("copy")]
		NSObject Copy ();
		
		[Export ("mutableCopy")]
		NSObject MutableCopy ();

		[Export ("description")]
		string Description { get; }

		[Export ("debugDescription")]
		string DebugDescription { get; }

		//
		// Extra Perform methods, with selectors
		//
		[Export ("performSelector:withObject:afterDelay:inModes:")]
		void PerformSelector (Selector selector, NSObject withObject, double afterDelay, NSString [] nsRunLoopModes);
		
		[Export ("performSelector:onThread:withObject:waitUntilDone:")]
		void PerformSelector (Selector selector, NSThread onThread, NSObject withObject, bool waitUntilDone);
		
		[Export ("performSelector:onThread:withObject:waitUntilDone:modes:")]
		void PerformSelector (Selector selector, NSThread onThread, NSObject withObject, bool waitUntilDone, NSString [] nsRunLoopModes);
		
		[Static, Export ("cancelPreviousPerformRequestsWithTarget:")]
		void CancelPreviousPerformRequest (NSObject aTarget);

		[Static, Export ("cancelPreviousPerformRequestsWithTarget:selector:object:")]
		void CancelPreviousPerformRequest (NSObject aTarget, Selector selector, [NullAllowed] NSObject argument);
	}
	
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	public interface NSOperation {
		[Export ("start")]
		void Start ();

		[Export ("main")]
		void Main ();

		[Export ("isCancelled")]
		bool IsCancelled { get; }

		[Export ("cancel")]
		void Cancel ();

		[Export ("isExecuting")]
		bool IsExecuting { get; }

		[Export ("isFinished")]
		bool IsFinished { get; }

		[Export ("isConcurrent")]
		bool IsConcurrent { get; }

		[Export ("isReady")]
		bool IsReady { get; }

		[Export ("addDependency:")][PostGet ("Dependencies")]
		void AddDependency (NSOperation op);

		[Export ("removeDependency:")][PostGet ("Dependencies")]
		void RemoveDependency (NSOperation op);

		[Export ("dependencies")]
		NSOperation [] Dependencies { get; }

		[Export ("waitUntilFinished")]
		void WaitUntilFinishedNS ();

		[Export ("threadPriority")]
		double ThreadPriority { get; set; }

		//Detected properties
		[Export ("queuePriority")]
		NSOperationQueuePriority QueuePriority { get; set; }
	}

	[BaseType (typeof (NSOperation))]
	[Since (4,0)]
	public interface NSBlockOperation {
		[Static]
		[Export ("blockOperationWithBlock:")]
		NSBlockOperation Create (/* non null */ Action method);

		[Export ("addExecutionBlock:")]
		void AddExecutionBlock (/* non null */ Action method);

		[Export ("executionBlocks")]
		NSObject [] ExecutionBlocks { get; }
	}

	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	public interface NSOperationQueue {
		[Export ("addOperation:")][PostGet ("Operations")]
		void AddOperation ([NullAllowed] NSOperation op);

		[Export ("addOperations:waitUntilFinished:")][PostGet ("Operations")]
		void AddOperations ([NullAllowed] NSOperation [] operations, bool waitUntilFinished);

		[Export ("addOperationWithBlock:")][PostGet ("Operations")]
		void AddOperation (/* non null */ Action operation);

		[Export ("operations")]
		NSOperation [] Operations { get; }

		[Export ("operationCount")]
		nuint OperationCount { get; }

		[Export ("name")]
		string Name { get; set; }

		[Export ("cancelAllOperations")][PostGet ("Operations")]
		void CancelAllOperations ();

		[Export ("waitUntilAllOperationsAreFinished")]
		void WaitUntilAllOperationsAreFinished ();

		[Static]
		[Export ("currentQueue")]
		NSOperationQueue CurrentQueue { get; }

		[Static]
		[Export ("mainQueue")]
		NSOperationQueue MainQueue { get; }

		//Detected properties
		[Export ("maxConcurrentOperationCount")]
		nint MaxConcurrentOperationCount { get; set; }

		[Export ("suspended")]
		bool Suspended { [Bind ("isSuspended")]get; set; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSOrderedSet {
		[Export ("initWithObject:")]
		IntPtr Constructor (NSObject start);

		[Export ("initWithArray:"), Internal]
		IntPtr Constructor (NSArray array);

		[Export ("initWithSet:")]
		IntPtr Constructor (NSSet source);
		
		[Export ("initWithOrderedSet:")]
		IntPtr Constructor (NSOrderedSet source);

		[Export ("count")]
		int Count { get; }

		[Export ("objectAtIndex:"), Internal]
		NSObject GetObject (int idx);

		[Export ("array"), Internal]
		IntPtr _ToArray ();

		[Export ("indexOfObject:")]
		int IndexOf (NSObject obj);
		
		[Export ("objectEnumerator"), Internal]
		NSEnumerator _GetEnumerator ();

		[Export ("set")]
		NSSet AsSet ();

		[Export ("containsObject:")]
		bool Contains (NSObject obj);

		[Export ("firstObject")]
		NSObject FirstObject ();

		[Export ("lastObject")]
		NSObject LastObject ();

		[Export ("isEqualToOrderedSet:")]
		bool IsEqualToOrderedSet (NSOrderedSet other);

		[Export ("intersectsOrderedSet:")]
		bool Intersects (NSOrderedSet other);

		[Export ("intersectsSet:")]
		bool Intersects (NSSet other);
		
		[Export ("isSubsetOfOrderedSet:")]
		bool IsSubset (NSOrderedSet other);
		
		[Export ("isSubsetOfSet:")]
		bool IsSubset (NSSet other);

		[Export ("reversedOrderedSet")]
		NSOrderedSet GetReverseOrderedSet ();
	}

	[BaseType (typeof (NSOrderedSet))]
	public interface NSMutableOrderedSet {
		[Export ("initWithObject:")]
		IntPtr Constructor (NSObject start);

		[Export ("initWithSet:")]
		IntPtr Constructor (NSSet source);

		[Export ("initWithOrderedSet:")]
		IntPtr Constructor (NSOrderedSet source);

		[Export ("initWithCapacity:")]
		IntPtr Constructor (int capacity);
		
		[Export ("initWithArray:"), Internal]
		IntPtr Constructor (NSArray array);

		[Export ("unionSet:"), Internal]
		void UnionSet (NSSet other);

		[Export ("minusSet:"), Internal]
		void MinusSet (NSSet other);

		[Export ("insertObject:atIndex:")]
		void Insert (NSObject obj, int atIndex);

		[Export ("removeObjectAtIndex:")]
		void Remove (int index);

		[Export ("replaceObjectAtIndex:withObject:")]
		void Replace (int objectAtIndex, NSObject newObject);

		[Export ("addObject:")]
		void Add (NSObject obj);

		[Export ("addObjectsFromArray:")]
		void AddObjects (NSObject [] source);

		[Export ("insertObjects:atIndexes:")]
		void InsertObjects (NSObject [] objects, NSIndexSet atIndexes);

		[Export ("removeObjectsAtIndexes:")]
		void RemoveObjects (NSIndexSet indexSet);
		
		[Export ("exchangeObjectAtIndex:withObjectAtIndex:")]
		void ExchangeObject (int first, int second);

		[Export ("moveObjectsAtIndexes:toIndex:")]
		void MoveObjects (NSIndexSet indexSet, int destination);

		[Export ("setObject:atIndex:")]
		void SetObject (NSObject obj, int index);

		[Export ("replaceObjectsAtIndexes:withObjects:")]
		void ReplaceObjects (NSIndexSet indexSet, NSObject [] replacementObjects);

		[Export ("removeObjectsInRange:")]
		void RemoveObjects (NSRange range);

		[Export ("removeAllObjects")]
		void RemoveAllObjects ();

		[Export ("removeObject:")]
		void RemoveObject (NSObject obj);

		[Export ("removeObjectsInArray:")]
		void RemoveObjects (NSObject [] objects);

		[Export ("intersectOrderedSet:")]
		void Intersect (NSOrderedSet intersectWith);

		[Export ("sortUsingComparator:")]
		void Sort (NSComparator comparator);

		[Export ("sortWithOptions:usingComparator:")]
		void Sort (NSSortOptions sortOptions, NSComparator comparator);

		[Export ("sortRange:options:usingComparator:")]
		void SortRange (NSRange range, NSSortOptions sortOptions, NSComparator comparator);
	}
	
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** -[__NSArrayM insertObject:atIndex:]: object cannot be nil
	[DisableDefaultCtor]
	public interface NSOrthography {
		[Export ("dominantScript")]
		string DominantScript { get;  }

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
		string DominantLanguageForScript (string script);

		[Export ("initWithDominantScript:languageMap:")]
		IntPtr Constructor (string dominantScript, [NullAllowed] NSDictionary languageMap);
	}
	
	[BaseType (typeof (NSStream))]
	[DisableDefaultCtor] // crash when used
	public interface NSOutputStream {
		[Export ("initToMemory")]
		IntPtr Constructor ();

		[Export ("hasSpaceAvailable")]
		bool HasSpaceAvailable ();
	
		//[Export ("initToBuffer:capacity:")]
		//IntPtr Constructor (uint8_t  buffer, nuint capacity);

		[Export ("initToFileAtPath:append:")]
		IntPtr Constructor (string  path, bool shouldAppend);

		[Static]
		[Export ("outputStreamToMemory")]
		NSObject OutputStreamToMemory ();

		//[Static]
		//[Export ("outputStreamToBuffer:capacity:")]
		//NSObject OutputStreamToBuffer (uint8_t  buffer, nuint capacity);

		[Static]
		[Export ("outputStreamToFileAtPath:append:")]
		NSOutputStream CreateFile (string path, bool shouldAppend);
	}

	[BaseType (typeof (NSObject), Name="NSHTTPCookie")]
	// default 'init' crash both simulator and devices
	[DisableDefaultCtor]
	public interface NSHttpCookie {
		[Export ("initWithProperties:")]
		IntPtr Constructor (NSDictionary properties);

		[Export ("cookieWithProperties:"), Static]
		NSHttpCookie CookieFromProperties (NSDictionary properties);

		[Export ("requestHeaderFieldsWithCookies:"), Static]
		NSDictionary RequestHeaderFieldsWithCookies (NSHttpCookie [] cookies);

		[Export ("cookiesWithResponseHeaderFields:forURL:"), Static]
		NSHttpCookie [] CookiesWithResponseHeaderFields (NSDictionary headerFields, NSUrl url);

		[Export ("properties")]
		NSDictionary Properties { get; }

		[Export ("version")]
		nuint Version { get; }

		[Export ("value")]
		string Value { get; }

		[Export ("expiresDate")]
		NSDate ExpiresDate { get; }

		[Export ("isSessionOnly")]
		bool IsSessionOnly { get; }

		[Export ("domain")]
		string Domain { get; }

		[Export ("name")]
		string Name { get; }

		[Export ("path")]
		string Path { get; }

		[Export ("isSecure")]
		bool IsSecure { get; }

		[Export ("isHTTPOnly")]
		bool IsHttpOnly { get; }

		[Export ("comment")]
		string Comment { get; }

		[Export ("commentURL")]
		NSUrl CommentUrl { get; }

		[Export ("portList")]
		NSNumber [] PortList { get; }
	}

	[BaseType (typeof (NSObject), Name="NSHTTPCookieStorage")]
	// NSHTTPCookieStorage implements a singleton object -> use SharedStorage since 'init' returns NIL
	[DisableDefaultCtor]
	public interface NSHttpCookieStorage {
		[Export ("sharedHTTPCookieStorage"), Static]
		NSHttpCookieStorage SharedStorage { get; }

		[Export ("cookies")]
		NSHttpCookie [] Cookies { get; }

		[Export ("setCookie:")]
		void SetCookie (NSHttpCookie cookie);

		[Export ("deleteCookie:")]
		void DeleteCookie (NSHttpCookie cookie);

		[Export ("cookiesForURL:")]
		NSHttpCookie [] CookiesForUrl (NSUrl url);

		[Export ("setCookies:forURL:mainDocumentURL:")]
		void SetCookies (NSHttpCookie [] cookies, NSUrl forUrl, NSUrl mainDocumentUrl);

		[Export ("cookieAcceptPolicy")]
		NSHttpCookieAcceptPolicy AcceptPolicy { get; set; }

		[Export ("sortedCookiesUsingDescriptors:")]
		NSHttpCookie [] GetSortedCookies (NSSortDescriptor [] sortDescriptors);
		
	}
	
	[BaseType (typeof (NSUrlResponse), Name="NSHTTPURLResponse")]
	public interface NSHttpUrlResponse {
		[Since (5,0)]
		[Export ("initWithURL:statusCode:HTTPVersion:headerFields:")]
		IntPtr Constructor (NSUrl url, nint statusCode, string httpVersion, NSDictionary headerFields);
		
		[Export ("statusCode")]
		nint StatusCode { get; }

		[Export ("allHeaderFields")]
		NSDictionary AllHeaderFields { get; }

		[Export ("localizedStringForStatusCode:")][Static]
		string LocalizedStringForStatusCode (nint statusCode);
	}
	
	[BaseType (typeof (NSObject))]
#if MONOMAC
	[DisableDefaultCtor] // An uncaught exception was raised: -[__NSCFDictionary removeObjectForKey:]: attempt to remove nil key
#endif
	public partial interface NSBundle {
		[Export ("mainBundle")][Static]
		NSBundle MainBundle { get; }

		[Export ("bundleWithPath:")][Static]
		NSBundle FromPath (string path);

		[Export ("initWithPath:")]
		IntPtr Constructor (string path);

		[Export ("bundleForClass:")][Static]
		NSBundle FromClass (Class c);

		[Export ("bundleWithIdentifier:")][Static]
		NSBundle FromIdentifier (string str);

		[Export ("allBundles")][Static]
		NSBundle [] _AllBundles { get; }

		[Export ("allFrameworks")][Static]
		NSBundle [] AllFrameworks { get; }

		[Export ("load")]
		bool Load ();

		[Export ("isLoaded")]
		bool IsLoaded { get; }

		[Export ("unload")]
		bool Unload ();

		[Export ("bundlePath")]
		string BundlePath { get; }
		
		[Export ("resourcePath")]
		string  ResourcePath { get; }
		
		[Export ("executablePath")]
		string ExecutablePath { get; }
		
		[Export ("pathForAuxiliaryExecutable:")]
		string PathForAuxiliaryExecutable (string s);
		

		[Export ("privateFrameworksPath")]
		string PrivateFrameworksPath { get; }
		
		[Export ("sharedFrameworksPath")]
		string SharedFrameworksPath { get; }
		
		[Export ("sharedSupportPath")]
		string SharedSupportPath { get; }
		
		[Export ("builtInPlugInsPath")]
		string BuiltinPluginsPath { get; }
		
		[Export ("bundleIdentifier")]
		string BundleIdentifier { get; }

		[Export ("classNamed:")]
		Class ClassNamed (string className);
		
		[Export ("principalClass")]
		Class PrincipalClass { get; }

		[Export ("pathForResource:ofType:inDirectory:")][Static]
		string PathForResourceAbsolute (string name, [NullAllowed] string ofType, string bundleDirectory);
		
		[Export ("pathForResource:ofType:")]
		string PathForResource (string name, [NullAllowed] string ofType);

		// TODO: this conflicts with the above with our generator.
		//[Export ("pathForResource:ofType:inDirectory:")]
		//string PathForResource (string name, string ofType, string subpath);
		
		[Export ("pathForResource:ofType:inDirectory:forLocalization:")]
		string PathForResource (string name, [NullAllowed] string ofType, string subpath, string localizationName);

		[Export ("localizedStringForKey:value:table:")]
		string LocalizedString ([NullAllowed] string key, [NullAllowed] string value, [NullAllowed] string table);

		[Export ("objectForInfoDictionaryKey:")]
		NSObject ObjectForInfoDictionary (string key);

		[Export ("developmentLocalization")]
		string DevelopmentLocalization { get; }
		
		[Export ("infoDictionary")]
		NSDictionary InfoDictionary{ get; }

		// Additions from AppKit
#if MONOMAC
		// https://developer.apple.com/library/mac/#documentation/Cocoa/Reference/ApplicationKit/Classes/NSBundle_AppKitAdditions/Reference/Reference.html
		[Static]
		[Export ("loadNibNamed:owner:")]
		bool LoadNib (string nibName, NSObject owner);

		[Export ("pathForImageResource:")]
		string PathForImageResource (string resource);

		[Export ("pathForSoundResource:")]
		string PathForSoundResource (string resource);

		[Export("appStoreReceiptURL")]
		NSUrl AppStoreReceiptUrl { get; }

#else
		// http://developer.apple.com/library/ios/#documentation/uikit/reference/NSBundle_UIKitAdditions/Introduction/Introduction.html
		[Export ("loadNibNamed:owner:options:")]
		NSArray LoadNib (string nibName, [NullAllowed] NSObject owner, [NullAllowed] NSDictionary options);
#endif
		[Export ("bundleURL")]
		[Since (4,0)]
		NSUrl BundleUrl { get; }
		
		[Export ("resourceURL")]
		[Since (4,0)]
		NSUrl ResourceUrl { get; }

		[Export ("executableURL")]
		[Since (4,0)]
		NSUrl ExecutableUrl { get; }

		[Export ("URLForAuxiliaryExecutable:")]
		[Since (4,0)]
		NSUrl UrlForAuxiliaryExecutable (string executable);

		[Export ("privateFrameworksURL")]
		[Since (4,0)]
		NSUrl PrivateFrameworksUrl { get; }

		[Export ("sharedFrameworksURL")]
		[Since (4,0)]
		NSUrl SharedFrameworksUrl { get; }

		[Export ("sharedSupportURL")]
		[Since (4,0)]
		NSUrl SharedSupportUrl { get; }

		[Export ("builtInPlugInsURL")]
		[Since (4,0)]
		NSUrl BuiltInPluginsUrl { get; }

		[Export ("initWithURL:")]
		[Since (4,0)]
		IntPtr Constructor (NSUrl url);
		
		[Static, Export ("bundleWithURL:")]
		[Since (4,0)]
		NSBundle FromUrl (NSUrl url);

		[Export ("preferredLocalizations")]
		string [] PreferredLocalizations { get; }

		[Export ("localizations")]
		string [] Localizations { get; }

		[Export ("pathsForResourcesOfType:inDirectory:")]
		string [] PathsForResources (string fileExtension, [NullAllowed] string subDirectory);

		[Export ("pathsForResourcesOfType:inDirectory:forLocalization:")]
		string [] PathsForResources (string fileExtension, [NullAllowed] string subDirectory, [NullAllowed] string localizationName);

		[Static, Export ("pathsForResourcesOfType:inDirectory:")]
		string [] GetPathsForResources (string fileExtension, string bundlePath);
	}

	[BaseType (typeof (NSObject))]
	public interface NSIndexPath {
		[Export ("indexPathWithIndex:")][Static]
		NSIndexPath FromIndex (nuint index);

		[Export ("indexPathWithIndexes:length:")][Internal][Static]
		NSIndexPath _FromIndex (IntPtr indexes, nuint len);

		[Export ("indexPathByAddingIndex:")]
		NSIndexPath IndexPathByAddingIndex (nuint index);
		
		[Export ("indexPathByRemovingLastIndex")]
		NSIndexPath IndexPathByRemovingLastIndex ();

		[Export ("indexAtPosition:")]
		nuint IndexAtPosition (nuint position);

		[Export ("length")]
		nuint Length { get; } 

		[Export ("getIndexes:")][Internal]
		void _GetIndexes (IntPtr target);

		[Export ("compare:")]
		nint Compare (NSIndexPath other);

#if !MONOMAC
		// NSIndexPath UIKit Additions Reference
		// https://developer.apple.com/library/ios/#documentation/UIKit/Reference/NSIndexPath_UIKitAdditions/Reference/Reference.html
		[Export ("row")]
		int Row { get; }

		[Export ("section")]
		int Section { get; }

		[Static]
		[Export ("indexPathForRow:inSection:")]
		NSIndexPath FromRowSection (int row, int section);

		[Export ("item")]
		[Since (6,0)]
		int Item { get; }

		[Static]
		[Since (6,0)]
		[Export ("indexPathForItem:inSection:")]
		NSIndexPath FromItemSection (int item, int section);
#endif
		
	}

	public delegate void NSRangeIterator (NSRange range, ref bool stop);
	
	[BaseType (typeof (NSObject))]
	public interface NSIndexSet {
		[Static, Export ("indexSetWithIndex:")]
		NSIndexSet FromIndex (nuint idx);

		[Static, Export ("indexSetWithIndexesInRange:")]
		NSIndexSet FromNSRange (NSRange indexRange);
		
		[Export ("initWithIndex:")]
		IntPtr Constructor (nuint index);

		[Export ("initWithIndexSet:")]
		IntPtr Constructor (NSIndexSet other);

		[Export ("count")]
		nuint Count { get; }

		[Export ("isEqualToIndexSet:")]
		bool IsEqual (NSIndexSet other);

		[Export ("firstIndex")]
		nuint FirstIndex { get; }

		[Export ("lastIndex")]
		nuint LastIndex { get; }

		[Export ("indexGreaterThanIndex:")]
		nuint IndexGreaterThan (nuint index);

		[Export ("indexLessThanIndex:")]
		nuint IndexLessThan (nuint index);

		[Export ("indexGreaterThanOrEqualToIndex:")]
		nuint IndexGreaterThanOrEqual (nuint index);

		[Export ("indexLessThanOrEqualToIndex:")]
		nuint IndexLessThanOrEqual (nuint index);

		[Export ("containsIndex:")]
		bool Contains (nuint index);

		[Export ("containsIndexes:")]
		bool Contains (NSIndexSet indexes);

		[Export ("enumerateRangesUsingBlock:")]
		void EnumerateRanges (NSRangeIterator iterator);

		[Export ("enumerateRangesWithOptions:usingBlock:")]
		void EnumerateRanges (NSEnumerationOptions opts, NSRangeIterator iterator);

		[Export ("enumerateRangesInRange:options:usingBlock:")]
		void EnumerateRanges (NSRange range, NSEnumerationOptions opts, NSRangeIterator iterator);
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // from the docs: " you should not create these objects using alloc and init."
	public interface NSInvocation {

		[Export ("selector")]
		Selector Selector { get; set; }

		[Export ("target")]
		NSObject Target { get; set; }

		// FIXME: We need some special marshaling support to handle these buffers...
		[Internal, Export ("setArgument:atIndex:")]
		void _SetArgument (IntPtr buffer, int index);

		[Internal, Export ("getArgument:atIndex:")]
		void _GetArgument (IntPtr buffer, int index);

		[Internal, Export ("setReturnValue:")]
		void _SetReturnValue (IntPtr buffer);

		[Internal, Export ("getReturnValue:")]
		void _GetReturnValue (IntPtr buffer);

		[Export ("invoke")]
		void Invoke ();

		[Export ("invokeWithTarget:")]
		void Invoke (NSObject target);

		[Export ("methodSignature")]
		NSMethodSignature MethodSignature { get; }
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // `init` returns a null handle
	public interface NSMethodSignature {
		[Export ("numberOfArguments")]
		uint NumberOfArguments { get; }

		[Export ("frameLength")]
		uint FrameLength { get; }

		[Export ("methodReturnLength")]
		uint MethodReturnLength { get; }

		[Export ("isOneway")]
		bool IsOneway { get; }
	}

	[BaseType (typeof (NSObject), Name="NSJSONSerialization")]
	// Objective-C exception thrown.  Name: NSInvalidArgumentException Reason: *** +[NSJSONSerialization allocWithZone:]: Do not create instances of NSJSONSerialization in this release
	[DisableDefaultCtor]
	interface NSJsonSerialization {
		[Static]
		[Export ("isValidJSONObject:")]
		bool IsValidJSONObject (NSObject obj);

		[Static]
		[Export ("dataWithJSONObject:options:error:")]
		NSData Serialize (NSObject obj, NSJsonWritingOptions opt, out NSError error);

		[Static]
		[Export ("JSONObjectWithData:options:error:")]
		NSObject Deserialize (NSData data, NSJsonReadingOptions opt, NSError error);

		[Static]
		[Export ("writeJSONObject:toStream:options:error:")]
		nint Serialize (NSObject obj, NSOutputStream stream, NSJsonWritingOptions opt, out NSError error);

		[Static]
		[Export ("JSONObjectWithStream:options:error:")]
		NSObject Deserialize (NSInputStream stream, NSJsonReadingOptions opt, out NSError error);

	}
	
	[BaseType (typeof (NSIndexSet))]
	public interface NSMutableIndexSet {
		[Export ("initWithIndex:")]
		IntPtr Constructor (nuint index);

		[Export ("initWithIndexSet:")]
		IntPtr Constructor (NSIndexSet other);

		[Export ("addIndexes:")]
		void Add (NSIndexSet other);

		[Export ("removeIndexes:")]
		void Remove (NSIndexSet other);

		[Export ("removeAllIndexes")]
		void Clear ();

		[Export ("addIndex:")]
		void Add (nuint index);

		[Export ("removeIndex:")]
		void Remove (nuint index);

		[Export ("shiftIndexesStartingAtIndex:by:")]
		void ShiftIndexes (nuint startIndex, nint delta);
	}
	
	[BaseType (typeof (NSObject), Delegates=new string [] { "WeakDelegate" }, Events=new Type [] { typeof (NSNetServiceDelegate)})]
	public interface NSNetService {
		[Export ("initWithDomain:type:name:port:")]
		IntPtr Constructor (string domain, string type, string name, int port);

		[Export ("initWithDomain:type:name:")]
		IntPtr Constructor (string domain, string type, string name);
		
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }
		
		[Wrap ("WeakDelegate")]
		NSNetServiceDelegate Delegate { get; set; }

		[Export ("scheduleInRunLoop:forMode:")]
		void Schedule (NSRunLoop aRunLoop, string forMode);

		// For consistency with other APIs (NSUrlConnection) we call this Unschedule
		[Export ("removeFromRunLoop:forMode:")]
		void Unschedule (NSRunLoop aRunLoop, string forMode);

		[Export ("domain")]
		string Domain { get; }

		[Export ("type")]
		string Type { get; }

		[Export ("name")]
		string Name { get; }

		[Export ("addresses")]
		NSData [] Addresses { get; }

		[Export ("port")]
		int Port { get; }

		[Export ("publish")]
		void Publish ();

		[Export ("publishWithOptions:")]
		void Publish (NSNetServiceOptions options);

		[Obsolete ("Deprecated in iOS 2.0 / OSX 10.4, use Resolve(double)")]
		[Export ("resolve")]
		void Resolve ();

		[Export ("resolveWithTimeout:")]
		void Resolve (double timeOut);

		[Export ("stop")]
		void Stop ();

		[Static, Export ("dictionaryFromTXTRecordData:")]
		NSDictionary DictionaryFromTxtRecord (NSData data);
		
		[Static, Export ("dataFromTXTRecordDictionary:")]
		NSData DataFromTxtRecord (NSDictionary dictionary);
		
		[Export ("hostName")]
		string HostName { get; }

		[Internal, Export ("getInputStream:outputStream:")]
		bool GetStreams (IntPtr ptrToInputStorage, IntPtr ptrToOutputStorage);
		
		[Export ("TXTRecordData")]
		NSData GetTxtRecordData ();

		[Export ("setTXTRecordData:")]
		bool SetTxtRecordData (NSData data);

		//NSData TxtRecordData { get; set; }

		[Export ("startMonitoring")]
		void StartMonitoring ();

		[Export ("stopMonitoring")]
		void StopMonitoring ();
	}

	[Model, BaseType (typeof (NSObject))]
	[Protocol]
	public interface NSNetServiceDelegate {
		[Export ("netServiceWillPublish:")]
		void WillPublish (NSNetService sender);

		[Export ("netServiceDidPublish:")]
		void Published (NSNetService sender);

		[Export ("netService:didNotPublish:"), EventArgs ("NSNetServiceError")]
		void PublishFailure (NSNetService sender, NSDictionary errors);

		[Export ("netServiceWillResolve:")]
		void WillResolve (NSNetService sender);

		[Export ("netServiceDidResolveAddress:")]
		void AddressResolved (NSNetService sender);

		[Export ("netService:didNotResolve:"), EventArgs ("NSNetServiceError")]
		void ResolveFailure (NSNetService sender, NSDictionary errors);

		[Export ("netServiceDidStop:")]
		void Stopped (NSNetService sender);

		[Export ("netService:didUpdateTXTRecordData:"), EventArgs ("NSNetServiceData")]
		void UpdatedTxtRecordData (NSNetService sender, NSData data);
	}
	
	[BaseType (typeof (NSObject),
		   Delegates=new string [] {"WeakDelegate"},
		   Events=new Type [] {typeof (NSNetServiceBrowserDelegate)})]
	public interface NSNetServiceBrowser {
		[Export ("delegate", ArgumentSemantic.Assign), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSNetServiceBrowserDelegate Delegate { get; set; }

		[Export ("scheduleInRunLoop:forMode:")]
		void Schedule (NSRunLoop aRunLoop, string forMode);

		// For consistency with other APIs (NSUrlConnection) we call this Unschedule
		[Export ("removeFromRunLoop:forMode:")]
		void Unschedule (NSRunLoop aRunLoop, string forMode);

		[Export ("searchForBrowsableDomains")]
		void SearchForBrowsableDomains ();

		[Export ("searchForRegistrationDomains")]
		void SearchForRegistrationDomains ();

		[Export ("searchForServicesOfType:inDomain:")]
		void SearchForServices (string type, string domain);

		[Export ("stop")]
		void Stop ();
	}

	[Model, BaseType (typeof (NSObject))]
	[Protocol]
	public interface NSNetServiceBrowserDelegate {
		[Export ("netServiceBrowserWillSearch:")]
		void SearchStarted (NSNetServiceBrowser sender);
		
		[Export ("netServiceBrowserDidStopSearch:")]
		void SearchStopped (NSNetServiceBrowser sender);
		
		[Export ("netServiceBrowser:didNotSearch:"), EventArgs ("NSNetServiceError")]
		void NotSearched (NSNetServiceBrowser sender, NSDictionary errors);
		
		[Export ("netServiceBrowser:didFindDomain:moreComing:"), EventArgs ("NSNetDomain")]
		void FoundDomain (NSNetServiceBrowser sender, string domain, bool moreComing);
		
		[Export ("netServiceBrowser:didFindService:moreComing:"), EventArgs ("NSNetService")]
		void FoundService (NSNetServiceBrowser sender, NSNetService service, bool moreComing);
		
		[Export ("netServiceBrowser:didRemoveDomain:moreComing:"), EventArgs ("NSNetDomain")]
		void DomainRemoved (NSNetServiceBrowser sender, string domain, bool moreComing);
		
		[Export ("netServiceBrowser:didRemoveService:moreComing:"), EventArgs ("NSNetService")]
		void ServiceRemoved (NSNetServiceBrowser sender, NSNetService service, bool moreComing);
	}
	
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSGenericException Reason: *** -[NSConcreteNotification init]: should never be used
	[DisableDefaultCtor]
	public interface NSNotification {
		[Export ("name")]
		string Name { get; }

		[Export ("object")]
		NSObject Object { get; }

		[Export ("userInfo")]
		NSDictionary UserInfo { get; }

		[Export ("notificationWithName:object:")][Static]
		NSNotification FromName (string name, NSObject obj);

		[Export ("notificationWithName:object:userInfo:")][Static]
		NSNotification FromName (string name, NSObject obj, NSDictionary userInfo);
	}

	[BaseType (typeof (NSObject))]
	public interface NSNotificationCenter {
		[Static][Export ("defaultCenter")]
		NSNotificationCenter DefaultCenter { get; }
	
		[Export ("addObserver:selector:name:object:")]
		[PostSnippet ("AddObserverToList (observer, aName, anObject);")]
		void AddObserver (NSObject observer, Selector aSelector, [NullAllowed] NSString aName, [NullAllowed] NSObject anObject);
	
		[Export ("postNotification:")]
		void PostNotification (NSNotification notification);
	
		[Export ("postNotificationName:object:")]
		void PostNotificationName (string aName, [NullAllowed] NSObject anObject);
	
		[Export ("postNotificationName:object:userInfo:")]
		void PostNotificationName (string aName, [NullAllowed] NSObject anObject, [NullAllowed] NSDictionary aUserInfo);
	
		[Export ("removeObserver:")]
		[PostSnippet ("RemoveObserversFromList (observer, null, null);")]
		void RemoveObserver (NSObject observer);
	
		[Export ("removeObserver:name:object:")]
		[PostSnippet ("RemoveObserversFromList (observer, aName, anObject);")]
		void RemoveObserver (NSObject observer, [NullAllowed] string aName, [NullAllowed] NSObject anObject);

		[Since (4,0)]
		[Export ("addObserverForName:object:queue:usingBlock:")]
		NSObject AddObserver (string name, NSObject obj, NSOperationQueue queue, NSNotificationHandler handler);
	}

#if MONOMAC
	[BaseType (typeof (NSNotificationCenter))]
	public interface NSDistributedNotificationCenter {
		[Static]
		[Export ("defaultCenter")]
		NSObject DefaultCenter { get; }

		[Export ("addObserver:selector:name:object:suspensionBehavior:")]
		void AddObserver (NSObject observer, Selector selector, [NullAllowed] string notificationName, [NullAllowed] string notificationSenderc, NSNotificationSuspensionBehavior suspensionBehavior);

		[Export ("postNotificationName:object:userInfo:deliverImmediately:")]
		void PostNotificationName (string name, [NullAllowed] string anObject, [NullAllowed] NSDictionary userInfo, bool deliverImmediately);
		
		[Export ("postNotificationName:object:userInfo:options:")]
		void PostNotificationName (string name, [NullAllowed] string anObjecb, [NullAllowed] NSDictionary userInfo, NSNotificationFlags options);

		[Export ("addObserver:selector:name:object:")]
		void AddObserver (NSObject observer, Selector aSelector, [NullAllowed] string aName, [NullAllowed] NSObject anObject);

		[Export ("postNotificationName:object:")]
		void PostNotificationName (string aName, [NullAllowed] string anObject);

		[Export ("postNotificationName:object:userInfo:")]
		void PostNotificationName (string aName, [NullAllowed] string anObject, [NullAllowed] NSDictionary aUserInfo);

		[Export ("removeObserver:name:object:")]
		void RemoveObserver (NSObject observer, [NullAllowed] string aName, [NullAllowed] NSObject anObject);

		//Detected properties
		[Export ("suspended")]
		bool Suspended { get; set; }
		
		[Field ("NSLocalNotificationCenterType")]
		NSString NSLocalNotificationCenterType {get;}
	}
#endif
	
	[BaseType (typeof (NSObject))]
	public interface NSNotificationQueue {
		[Static][IsThreadStatic]
		[Export ("defaultQueue")]
		NSObject DefaultQueue { get; }

		[Export ("initWithNotificationCenter:")]
		IntPtr Constructor (NSNotificationCenter notificationCenter);

		[Export ("enqueueNotification:postingStyle:")]
		void EnqueueNotification (NSNotification notification, NSPostingStyle postingStyle);

		[Export ("enqueueNotification:postingStyle:coalesceMask:forModes:")]
		void EnqueueNotification (NSNotification notification, NSPostingStyle postingStyle, NSNotificationCoalescing coalesceMask, string [] modes);

		[Export ("dequeueNotificationsMatching:coalesceMask:")]
		void DequeueNotificationsMatchingcoalesceMask (NSNotification notification, NSNotificationCoalescing coalesceMask);
	}

	public delegate void NSNotificationHandler (NSNotification notification);
	
	[BaseType (typeof (NSObject))]
	// init returns NIL
	[DisableDefaultCtor]
	public partial interface NSValue {
		[Export ("getValue:")]
		void StoreValueAtAddress (IntPtr value);

		[Export ("objCType")][Internal]
		IntPtr ObjCTypePtr ();
		
		//[Export ("initWithBytes:objCType:")][Internal]
		//NSValue InitFromBytes (IntPtr byte_ptr, IntPtr char_ptr_type);
		//[Export ("valueWithBytes:objCType:")][Static][Internal]
		//+ (NSValue *)valueWithBytes:(const void *)value objCType:(const char *)type;
		//+ (NSValue *)value:(const void *)value withObjCType:(const char *)type;

		[Static]
		[Export ("valueWithNonretainedObject:")]
		NSValue ValueFromNonretainedObject (NSObject anObject);
	
		[Export ("nonretainedObjectValue")]
		NSObject NonretainedObjectValue { get; }
	
		[Static]
		[Export ("valueWithPointer:")]
		NSValue ValueFromPointer (IntPtr pointer);
	
		[Export ("pointerValue")]
		IntPtr PointerValue { get; }
	
		[Export ("isEqualToValue:")]
		bool IsEqualTo (NSValue value);
		
		[Export ("valueWithRange:")][Static]
		NSValue FromRange(NSRange range);
	
#if MONOMAC
		[Static, Export ("valueWithCMTime:"), Lion]
		NSValue FromCMTime (CMTime time);
		
		[Export ("CMTimeValue"), Lion]
		CMTime CMTimeValue { get; }
		
		[Static, Export ("valueWithCMTimeMapping:"), Lion]
		NSValue FromCMTimeMapping (CMTimeMapping timeMapping);
		
		[Export ("CMTimeMappingValue"), Lion]
		CMTimeMapping CMTimeMappingValue { get; }
		
		[Static, Export ("valueWithCMTimeRange:"), Lion]
		NSValue FromCMTimeRange (CMTimeRange timeRange);
		
		[Export ("CMTimeRangeValue"), Lion]
		CMTimeRange CMTimeRangeValue { get; }

#if MAC64
		[Export ("valueWithRect:"), Static]
		NSValue FromRectangle (CGRect rect);
		
		[Export ("valueWithSize:")][Static]
		NSValue FromSize (CGSize size);
		
		[Export ("valueWithPoint:")][Static]
		NSValue FromPoint (CGPoint point);
		
		[Export ("rectValue")]
		CGRect RectangleValue { get; }
		
		[Export ("sizeValue")]
		CGSize SizeValue { get; }
		
		[Export ("pointValue")]
		CGPoint PointValue { get; }
#else
		[Export ("valueWithRect:"), Static]
		NSValue FromRectangleF (System.Drawing.RectangleF rect);

		[Export ("valueWithSize:")][Static]
		NSValue FromSizeF (System.Drawing.SizeF size);

		[Export ("valueWithPoint:")][Static]
		NSValue FromPointF (System.Drawing.PointF point);

		[Export ("rectValue")]
		System.Drawing.RectangleF RectangleFValue { get; }

		[Export ("sizeValue")]
		System.Drawing.SizeF SizeFValue { get; }

		[Export ("pointValue")]
		System.Drawing.PointF PointFValue { get; }
#endif

		[Export ("rangeValue")]
		NSRange RangeValue { get; }
#else
		[Static, Export ("valueWithCMTime:"), Since (4,0)]
		NSValue FromCMTime (CMTime time);
		
		[Export ("CMTimeValue"), Since (4,0)]
		CMTime CMTimeValue { get; }
		
		[Static, Export ("valueWithCMTimeMapping:"), Since (4,0)]
		NSValue FromCMTimeMapping (CMTimeMapping timeMapping);
		
		[Export ("CMTimeMappingValue"), Since (4,0)]
		CMTimeMapping CMTimeMappingValue { get; }
		
		[Static, Export ("valueWithCMTimeRange:"), Since (4,0)]
		NSValue FromCMTimeRange (CMTimeRange timeRange);
		
		[Export ("CMTimeRangeValue"), Since (4,0)]
		CMTimeRange CMTimeRangeValue { get; }
		
		[Export ("CGAffineTransformValue")]
		MonoMac.CoreGraphics.CGAffineTransform CGAffineTransformValue { get; }
		
		[Export ("UIEdgeInsetsValue")]
		MonoMac.UIKit.UIEdgeInsets UIEdgeInsetsValue { get; }

		[Export ("valueWithCGAffineTransform:")][Static]
		NSValue FromCGAffineTransform (MonoMac.CoreGraphics.CGAffineTransform tran);

		[Export ("valueWithUIEdgeInsets:")][Static]
		NSValue FromUIEdgeInsets (MonoMac.UIKit.UIEdgeInsets insets);

		[Export ("valueWithUIOffset:")][Static]
		NSValue FromUIOffset (MonoMac.UIKit.UIOffset insets);

		[Export ("UIOffsetValue")]
		UIOffset UIOffsetValue { get; }

		[Export ("valueWithCGRect:")][Static]
		NSValue FromRectangleF (System.Drawing.RectangleF rect);

		[Export ("CGRectValue")]
		System.Drawing.RectangleF RectangleFValue { get; }

		[Export ("valueWithCGSize:")][Static]
		NSValue FromSizeF (System.Drawing.SizeF size);

		[Export ("CGSizeValue")]
		System.Drawing.SizeF SizeFValue { get; }

		[Export ("CGPointValue")]
		System.Drawing.PointF PointFValue { get; }

		[Export ("valueWithCGPoint:")][Static]
		NSValue FromPointF (System.Drawing.PointF point);
		
		[Export ("valueWithCATransform3D:")][Static]
		NSValue FromCATransform3D (MonoMac.CoreAnimation.CATransform3D transform);

		[Export ("CATransform3DValue")]
		MonoMac.CoreAnimation.CATransform3D CATransform3DValue { get; }

		[Static, Export ("valueWithMKCoordinate:")]
		NSValue FromMKCoordinate (MonoTouch.CoreLocation.CLLocationCoordinate2D coordinate);

		[Static, Export ("valueWithMKCoordinateSpan:")]
		NSValue FromMKCoordinateSpan (MonoTouch.MapKit.MKCoordinateSpan coordinateSpan);

		[Export ("MKCoordinateValue")]
		MonoTouch.CoreLocation.CLLocationCoordinate2D CoordinateValue { get; }
		
		[Export ("MKCoordinateSpanValue")]
		MonoTouch.MapKit.MKCoordinateSpan CoordinateSpanValue { get; }
#endif
	}
	
	[BaseType (typeof (NSValue))]
	// init returns NIL
	[DisableDefaultCtor]
	public interface NSNumber {
		[Export ("charValue")]
		sbyte SByteValue { get; }
	
		[Export ("unsignedCharValue")]
		byte ByteValue { get; }
	
		[Export ("shortValue")]
		short Int16Value { get; }
	
		[Export ("unsignedShortValue")]
		ushort UInt16Value { get; }
	
		[Export ("intValue")]
		int Int32Value { get; }
	
		[Export ("unsignedIntValue")]
		uint UInt32Value { get; } 
	
		//[Export ("longValue")]
		//int LongValue ();
		//
		//[Export ("unsignedLongValue")]
		//uint UnsignedLongValue ();
	
		[Export ("longLongValue")]
		long Int64Value { get; }
	
		[Export ("unsignedLongLongValue")]
		ulong UInt64Value { get; }
	
		[Export ("floatValue")]
		float FloatValue { get; }
	
		[Export ("doubleValue")]
		double DoubleValue { get; }

		[Export ("decimalValue")]
		NSDecimal NSDecimalValue { get; }
	
		[Export ("boolValue")]
		bool BoolValue { get; }
	
		[Export ("integerValue")]
		nint IntValue { get; }
	
		[Export ("unsignedIntegerValue")]
		nuint UnsignedIntegerValue { get; }
	
		[Export ("stringValue")]
		string StringValue { get; }

		[Export ("compare:")]
		int Compare (NSNumber otherNumber);
	
		[Export ("isEqualToNumber:")]
		bool IsEqualToNumber (NSNumber number);
	
		[Export ("descriptionWithLocale:")]
		string DescriptionWithLocale (NSLocale locale);

		[Export ("initWithChar:")]
		IntPtr Constructor (sbyte value);
	
		[Export ("initWithUnsignedChar:")]
		IntPtr Constructor (byte value);
	
		[Export ("initWithShort:")]
		IntPtr Constructor (short value);
	
		[Export ("initWithUnsignedShort:")]
		IntPtr Constructor (ushort value);
	
		[Export ("initWithInt:")]
		IntPtr Constructor (int value);
	
		[Export ("initWithUnsignedInt:")]
		IntPtr Constructor (uint value);
	
		//[Export ("initWithLong:")]
		//IntPtr Constructor (long value);
		//
		//[Export ("initWithUnsignedLong:")]
		//IntPtr Constructor (ulong value);
	
		[Export ("initWithLongLong:")]
		IntPtr Constructor (long value);
	
		[Export ("initWithUnsignedLongLong:")]
		IntPtr Constructor (ulong value);
	
		[Export ("initWithFloat:")]
		IntPtr Constructor (float value);
	
		[Export ("initWithDouble:")]
		IntPtr Constructor (double value);
	
		[Export ("initWithBool:")]
		IntPtr Constructor (bool value);
	
		[Export ("numberWithChar:")][Static]
		NSNumber FromSByte (sbyte value);
	
		[Static]
		[Export ("numberWithUnsignedChar:")]
		NSNumber FromByte (byte value);
	
		[Static]
		[Export ("numberWithShort:")]
		NSNumber FromInt16 (short value);
	
		[Static]
		[Export ("numberWithUnsignedShort:")]
		NSNumber FromUInt16 (ushort value);
	
		[Static]
		[Export ("numberWithInt:")]
		NSNumber FromInt32 (int value);
	
		[Static]
		[Export ("numberWithUnsignedInt:")]
		NSNumber FromUInt32 (uint value);

		//[Static]
		//[Export ("numberWithLong:")]
		//NSNumber * numberWithLong: (long value);
		//
		//[Static]
		//[Export ("numberWithUnsignedLong:")]
		//NSNumber * numberWithUnsignedLong: (unsigned long value);
	
		[Static]
		[Export ("numberWithLongLong:")]
		NSNumber FromInt64 (long value);
	
		[Static]
		[Export ("numberWithUnsignedLongLong:")]
		NSNumber FromUInt64 (ulong value);
	
		[Static]
		[Export ("numberWithFloat:")]
		NSNumber FromFloat (float value);
	
		[Static]
		[Export ("numberWithDouble:")]
		NSNumber FromDouble (double value);
	
		[Static]
		[Export ("numberWithBool:")]
		NSNumber FromBoolean (bool value);
	}


	[BaseType (typeof (NSFormatter))]
	interface NSNumberFormatter {
		[Export ("stringFromNumber:")]
		string StringFromNumber (NSNumber number);

		[Export ("numberFromString:")]
		NSNumber NumberFromString (string text);

		[Static]
		[Export ("localizedStringFromNumber:numberStyle:")]
		string LocalizedStringFromNumbernumberStyle (NSNumber num, NSNumberFormatterStyle nstyle);

		//Detected properties
		[Export ("numberStyle")]
		NSNumberFormatterStyle NumberStyle { get; set; }

		[Export ("locale")]
		NSLocale Locale { get; set; }

		[Export ("generatesDecimalNumbers")]
		bool GeneratesDecimalNumbers { get; set; }

		[Export ("formatterBehavior")]
		NSNumberFormatterBehavior FormatterBehavior { get; set; }

		[Static]
		[Export ("defaultFormatterBehavior")]
		NSNumberFormatterBehavior DefaultFormatterBehavior { get; set; }

		[Export ("negativeFormat")]
		string NegativeFormat { get; set; }

		[Export ("textAttributesForNegativeValues")]
		NSDictionary TextAttributesForNegativeValues { get; set; }

		[Export ("positiveFormat")]
		string PositiveFormat { get; set; }

		[Export ("textAttributesForPositiveValues")]
		NSDictionary TextAttributesForPositiveValues { get; set; }

		[Export ("allowsFloats")]
		bool AllowsFloats { get; set; }

		[Export ("decimalSeparator")]
		string DecimalSeparator { get; set; }

		[Export ("alwaysShowsDecimalSeparator")]
		bool AlwaysShowsDecimalSeparator { get; set; }

		[Export ("currencyDecimalSeparator")]
		string CurrencyDecimalSeparator { get; set; }

		[Export ("usesGroupingSeparator")]
		bool UsesGroupingSeparator { get; set; }

		[Export ("groupingSeparator")]
		string GroupingSeparator { get; set; }

		[Export ("zeroSymbol")]
		string ZeroSymbol { get; set; }

		[Export ("textAttributesForZero")]
		NSDictionary TextAttributesForZero { get; set; }

		[Export ("nilSymbol")]
		string NilSymbol { get; set; }

		[Export ("textAttributesForNil")]
		NSDictionary TextAttributesForNil { get; set; }

		[Export ("notANumberSymbol")]
		string NotANumberSymbol { get; set; }

		[Export ("textAttributesForNotANumber")]
		NSDictionary TextAttributesForNotANumber { get; set; }

		[Export ("positiveInfinitySymbol")]
		string PositiveInfinitySymbol { get; set; }

		[Export ("textAttributesForPositiveInfinity")]
		NSDictionary TextAttributesForPositiveInfinity { get; set; }

		[Export ("negativeInfinitySymbol")]
		string NegativeInfinitySymbol { get; set; }

		[Export ("textAttributesForNegativeInfinity")]
		NSDictionary TextAttributesForNegativeInfinity { get; set; }

		[Export ("positivePrefix")]
		string PositivePrefix { get; set; }

		[Export ("positiveSuffix")]
		string PositiveSuffix { get; set; }

		[Export ("negativePrefix")]
		string NegativePrefix { get; set; }

		[Export ("negativeSuffix")]
		string NegativeSuffix { get; set; }

		[Export ("currencyCode")]
		string CurrencyCode { get; set; }

		[Export ("currencySymbol")]
		string CurrencySymbol { get; set; }

		[Export ("internationalCurrencySymbol")]
		string InternationalCurrencySymbol { get; set; }

		[Export ("percentSymbol")]
		string PercentSymbol { get; set; }

		[Export ("perMillSymbol")]
		string PerMillSymbol { get; set; }

		[Export ("minusSign")]
		string MinusSign { get; set; }

		[Export ("plusSign")]
		string PlusSign { get; set; }

		[Export ("exponentSymbol")]
		string ExponentSymbol { get; set; }

		[Export ("groupingSize")]
		uint GroupingSize { get; set; }

		[Export ("secondaryGroupingSize")]
		uint SecondaryGroupingSize { get; set; }

		[Export ("multiplier")]
		NSNumber Multiplier { get; set; }

		[Export ("formatWidth")]
		nuint FormatWidth { get; set; }

		[Export ("paddingCharacter")]
		string PaddingCharacter { get; set; }

		[Export ("paddingPosition")]
		NSNumberFormatterPadPosition PaddingPosition { get; set; }

		[Export ("roundingMode")]
		NSNumberFormatterRoundingMode RoundingMode { get; set; }

		[Export ("roundingIncrement")]
		NSNumber RoundingIncrement { get; set; }

		[Export ("minimumIntegerDigits")]
		nuint MinimumIntegerDigits { get; set; }

		[Export ("maximumIntegerDigits")]
		nuint MaximumIntegerDigits { get; set; }

		[Export ("minimumFractionDigits")]
		nuint MinimumFractionDigits { get; set; }

		[Export ("maximumFractionDigits")]
		nuint MaximumFractionDigits { get; set; }

		[Export ("minimum")]
		NSNumber Minimum { get; set; }

		[Export ("maximum")]
		NSNumber Maximum { get; set; }

		[Export ("currencyGroupingSeparator")]
		string CurrencyGroupingSeparator { get; set; }

		[Export ("lenient")]
		bool Lenient { [Bind ("isLenient")]get; set; }

		[Export ("usesSignificantDigits")]
		bool UsesSignificantDigits { get; set; }

		[Export ("minimumSignificantDigits")]
		nuint MinimumSignificantDigits { get; set; }

		[Export ("maximumSignificantDigits")]
		nuint MaximumSignificantDigits { get; set; }

		[Export ("partialStringValidationEnabled")]
		bool PartialStringValidationEnabled { [Bind ("isPartialStringValidationEnabled")]get; set; }
	}

	[BaseType (typeof (NSNumber))]
	public interface NSDecimalNumber {
		[Export ("initWithMantissa:exponent:isNegative:")]
		IntPtr Constructor (long mantissa, short exponent, bool isNegative);
		
		[Export ("initWithDecimal:")]
		IntPtr Constructor (NSDecimal dec);

		[Export ("initWithString:")]
		IntPtr Constructor (string numberValue);

		[Export ("initWithString:locale:")]
		IntPtr Constructor (string numberValue, NSObject locale);

		[Export ("descriptionWithLocale:")]
		string DescriptionWithLocale (NSLocale locale);

		[Export ("decimalValue")]
		NSDecimal NSDecimalValue { get; }

		[Export ("zero")][Static]
		NSDecimalNumber Zero { get; }

		[Export ("one")][Static]
		NSDecimalNumber One { get; }
		
		[Export ("minimumDecimalNumber")][Static]
		NSDecimalNumber MinValue { get; }
		
		[Export ("maximumDecimalNumber")][Static]
		NSDecimalNumber MaxValue { get; }

		[Export ("notANumber")][Static]
		NSDecimalNumber NaN { get; }

		//
		// All the behavior ones require:
		// id <NSDecimalNumberBehaviors>)behavior;

		[Export ("decimalNumberByAdding:")]
		NSDecimalNumber Add (NSDecimalNumber d);

		[Export ("decimalNumberByAdding:withBehavior:")]
		NSDecimalNumber Add (NSDecimalNumber d, NSObject Behavior);

		[Export ("decimalNumberBySubtracting:")]
		NSDecimalNumber Subtract (NSDecimalNumber d);

		[Export ("decimalNumberBySubtracting:withBehavior:")]
		NSDecimalNumber Subtract (NSDecimalNumber d, NSObject Behavior);
		
		[Export ("decimalNumberByMultiplyingBy:")]
		NSDecimalNumber Multiply (NSDecimalNumber d);

		[Export ("decimalNumberByMultiplyingBy:withBehavior:")]
		NSDecimalNumber Multiply (NSDecimalNumber d, NSObject Behavior);
		
		[Export ("decimalNumberByDividingBy:")]
		NSDecimalNumber Divide (NSDecimalNumber d);

		[Export ("decimalNumberByDividingBy:withBehavior:")]
		NSDecimalNumber Divide (NSDecimalNumber d, NSObject Behavior);

		[Export ("decimalNumberByRaisingToPower:")]
		NSDecimalNumber RaiseTo (nuint power);

		[Export ("decimalNumberByRaisingToPower:withBehavior:")]
		NSDecimalNumber RaiseTo (nuint power, NSObject Behavior);
		
		[Export ("decimalNumberByMultiplyingByPowerOf10:")]
		NSDecimalNumber MultiplyPowerOf10 (short power);

		[Export ("decimalNumberByMultiplyingByPowerOf10:withBehavior:")]
		NSDecimalNumber MultiplyPowerOf10 (short power, NSObject Behavior);

		[Export ("decimalNumberByRoundingAccordingToBehavior:")]
		NSDecimalNumber Rounding (NSObject behavior);

		[Export ("compare:")]
		nint Compare (NSNumber other);

		[Export ("defaultBehavior")][Static]
		NSObject DefaultBehavior { get; set; }

		[Export ("doubleValue")]
		double DoubleValue { get; }
	}

	[BaseType (typeof (NSObject))]
	public interface NSThread {
		[Static, Export ("currentThread")]
		NSThread Current { get; }

		//+ (void)detachNewThreadSelector:(SEL)selector toTarget:(id)target withObject:(id)argument;

		[Static, Export ("isMultiThreaded")]
		bool IsMultiThreaded { get; }

		//- (NSMutableDictionary *)threadDictionary;

		[Static, Export ("sleepUntilDate:")]
		void SleepUntil (NSDate date);
		
		[Static, Export ("sleepForTimeInterval:")]
		void SleepFor (double timeInterval);

		[Static, Export ("exit")]
		void Exit ();

		[Static, Export ("threadPriority"), Internal]
		double _GetPriority ();

		[Static, Export ("setThreadPriority:"), Internal]
		bool _SetPriority (double priority);

		//+ (NSArray *)callStackReturnAddresses;

		[Export ("name")]
		string Name { get; set; }

		[Export ("stackSize")]
		nuint StackSize { get; set; }

		[Export ("isMainThread")]
		bool IsMainThread { get; }

		// MainThread is already used for the instance selector and we can't reuse the same name
		[Static]
		[Export ("isMainThread")]
		bool IsMain { get; }

		[Static]
		[Export ("mainThread")]
		NSThread MainThread { get; }

		[Export ("initWithTarget:selector:object:")]
		IntPtr Constructor (NSObject target, Selector selector, [NullAllowed] NSObject argument);

		[Export ("isExecuting")]
		bool IsExecuting { get; }

		[Export ("isFinished")]
		bool IsFinished { get; }

		[Export ("isCancelled")]
		bool IsCancelled { get; }

		[Export ("cancel")]
		void Cancel ();

		[Export ("start")]
		void Start ();

		[Export ("main")]
		void Main ();
	}

	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	public interface NSPort {
		[Static, Export ("port")]
		NSPort Create ();

		[Export ("invalidate")]
		void Invalidate ();

		[Export ("isValid")]
		bool IsValid { get; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate"), NullAllowed]
		NSPortDelegate Delegate { get; set; }
	}

	[Model, BaseType (typeof (NSObject))]
	public interface NSPortDelegate {
		[Export ("handlePortMessage:")]
		void MessageReceived (NSPortMessage message);
	}

	[BaseType (typeof (NSObject))]
	public interface NSPortMessage {
		[Export ("initWithSendPort:receivePort:components:")]
		IntPtr Constructor (NSPort sendPort, NSPort recvPort, NSArray components);

		[Export ("sendBeforeDate:")]
		bool SendBefore (NSDate date);

		[Export ("components")]
		NSArray Components { get; }

		[Export ("receivePort")]
		NSPort ReceivePort { get; }

		[Export ("sendPort")]
		NSPort SendPort { get; }

		[Export ("msgid")]
		uint MsgId { get; set; }
	}

	[BaseType (typeof (NSPort))]
	public interface NSMachPort {
		[Static, Export ("portWithMachPort:")]
		NSPort FromMachPort (uint port);

		[Static, Export ("portWithMachPort:options:")]
		NSPort FromMachPort (uint port, NSMachPortRights options);

		[Export ("machPort")]
		uint MachPort { get; }

		[Export ("removeFromRunLoop:forMode:")]
		void RemoveFromRunLoop (NSRunLoop runLoop, NSString mode);

		[Export ("scheduleInRunLoop:forMode:")]
		void ScheduleInRunLoop (NSRunLoop runLoop, NSString mode);

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate"), NullAllowed]
		NSMachPortDelegate Delegate { get; set; }
	}

	[Model, BaseType (typeof (NSPortDelegate))]
	public interface NSMachPortDelegate {
		[Export ("handleMachMessage:")]
		void MachMessageReceived (IntPtr msgHeader);
	}

	[BaseType (typeof (NSObject))]
	public interface NSProcessInfo {
		[Export ("processInfo")][Static]
		NSProcessInfo ProcessInfo { get; }

		[Export ("arguments")]
		string [] Arguments { get; }

		[Export ("environment")]
		NSDictionary Environment { get; }

		[Export ("processIdentifier")]
		int ProcessIdentifier { get; }

		[Export ("globallyUniqueString")]
		string GloballyUniqueString { get; }

		[Export ("processName")]
		string ProcessName { get; set; }

		[Export ("hostName")]
		string HostName { get; }

		[Export ("operatingSystem")]
		nuint OperatingSystem { get; }

		[Export ("operatingSystemName")]
		string OperatingSystemName { get; }

		[Export ("operatingSystemVersionString")]
		string OperatingSystemVersionString { get; }

		[Export ("physicalMemory")]
		ulong PhysicalMemory { get; }
		
		[Export ("processorCount")]
		nuint ProcessorCount { get; }
		
		[Export ("activeProcessorCount")]
		nuint ActiveProcessorCount { get; }

		[Export ("systemUptime")]
		double SystemUptime { get; }

		[Export ("enableSuddenTermination")]
		void EnableSuddenTermination  ();
	
		[Export ("disableSuddenTermination")]
		void DisableSuddenTermination ();

		[Export ("enableAutomaticTermination:")]
		void EnableAutomaticTermination (string reason);

		[Export ("disableAutomaticTermination:")]
		void DisableAutomaticTermination (string reason);

		[Export ("automaticTerminationSupportEnabled")]
		bool AutomaticTerminationSupportEnabled { get; set; }
	}

	[BaseType (typeof (NSMutableData))]
	[Since (4,0)]
	public interface NSPurgeableData {
		
	}

	public delegate void NSFileCoordinatorWorker (NSUrl newUrl);
	public delegate void NSFileCoordinatorWorkerRW (NSUrl newReadingUrl, NSUrl newWritingUrl);
	
	[BaseType (typeof (NSObject))]
	interface NSFileCoordinator {
		[Static, Export ("addFilePresenter:")][PostGet ("FilePresenters")]
		void AddFilePresenter (NSFilePresenter filePresenter);

		[Static]
		[Export ("removeFilePresenter:")][PostGet ("FilePresenters")]
		void RemoveFilePresenter (NSFilePresenter filePresenter);

		[Static]
		[Export ("filePresenters")]
		NSFilePresenter [] FilePresenters { get; }

		[Export ("initWithFilePresenter:")]
		IntPtr Constructor ([NullAllowed] NSFilePresenter filePresenterOrNil);

		[Export ("coordinateReadingItemAtURL:options:error:byAccessor:")]
		void CoordinateRead (NSUrl itemUrl, NSFileCoordinatorReadingOptions options, out NSError error, /* non null */ NSFileCoordinatorWorker worker);

		[Export ("coordinateWritingItemAtURL:options:error:byAccessor:")]
		void CoordinateWrite (NSUrl url, NSFileCoordinatorWritingOptions options, out NSError error, /* non null */ NSFileCoordinatorWorker worker);

		[Export ("coordinateReadingItemAtURL:options:writingItemAtURL:options:error:byAccessor:")]
		void CoordinateReadWrite (NSUrl readingURL, NSFileCoordinatorReadingOptions readingOptions, NSUrl writingURL, NSFileCoordinatorWritingOptions writingOptions, out NSError error, /* non null */ NSFileCoordinatorWorkerRW readWriteWorker);
		
		[Export ("coordinateWritingItemAtURL:options:writingItemAtURL:options:error:byAccessor:")]
		void CoordinateWriteWrite (NSUrl writingURL, NSFileCoordinatorWritingOptions writingOptions, NSUrl writingURL2, NSFileCoordinatorWritingOptions writingOptions2, out NSError error, /* non null */ NSFileCoordinatorWorkerRW writeWriteWorker);

		[Export ("prepareForReadingItemsAtURLs:options:writingItemsAtURLs:options:error:byAccessor:")]
		void CoordinateBatc (NSUrl [] readingURLs, NSFileCoordinatorReadingOptions readingOptions, NSUrl [] writingURLs, NSFileCoordinatorWritingOptions writingOptions, out NSError error, /* non null */ Action batchHandler);

		[Export ("itemAtURL:didMoveToURL:")]
		void ItemMoved (NSUrl fromUrl, NSUrl toUrl);

		[Export ("cancel")]
		void Cancel ();

		[Since (6,0)]
		[MountainLion]
		[Export ("itemAtURL:willMoveToURL:")]
		void WillMove (NSUrl oldUrl, NSUrl newUrl);
	}
	
	[BaseType (typeof (NSObject))]
	public partial interface NSFileManager {
		[Field("NSFileType")]
		NSString NSFileType { get; }

		[Field("NSFileTypeDirectory")]
		NSString TypeDirectory { get; }

		[Field("NSFileTypeRegular")]
		NSString TypeRegular { get; }

		[Field("NSFileTypeSymbolicLink")]
		NSString TypeSymbolicLink { get; }

		[Field("NSFileTypeSocket")]
		NSString TypeSocket { get; }

		[Field("NSFileTypeCharacterSpecial")]
		NSString TypeCharacterSpecial { get; }

		[Field("NSFileTypeBlockSpecial")]
		NSString TypeBlockSpecial { get; }

		[Field("NSFileTypeUnknown")]
		NSString TypeUnknown { get; }

		[Field("NSFileSize")]
		NSString Size { get; }

		[Field("NSFileModificationDate")]
		NSString ModificationDate { get; }

		[Field("NSFileReferenceCount")]
		NSString ReferenceCount { get; }

		[Field("NSFileDeviceIdentifier")]
		NSString DeviceIdentifier { get; }

		[Field("NSFileOwnerAccountName")]
		NSString OwnerAccountName { get; }

		[Field("NSFileGroupOwnerAccountName")]
		NSString GroupOwnerAccountName { get; }

		[Field("NSFilePosixPermissions")]
		NSString PosixPermissions { get; }

		[Field("NSFileSystemNumber")]
		NSString SystemNumber { get; }

		[Field("NSFileSystemFileNumber")]
		NSString SystemFileNumber { get; }

		[Field("NSFileExtensionHidden")]
		NSString ExtensionHidden { get; }

		[Field("NSFileHFSCreatorCode")]
		NSString HfsCreatorCode { get; }

		[Field("NSFileHFSTypeCode")]
		NSString HfsTypeCode { get; }

		[Field("NSFileImmutable")]
		NSString Immutable { get; }

		[Field("NSFileAppendOnly")]
		NSString AppendOnly { get; }

		[Field("NSFileCreationDate")]
		NSString CreationDate { get; }

		[Field("NSFileOwnerAccountID")]
		NSString OwnerAccountID { get; }

		[Field("NSFileGroupOwnerAccountID")]
		NSString GroupOwnerAccountID { get; }

		[Field("NSFileBusy")]
		NSString Busy { get; }
#if !MONOMAC
		[Field ("NSFileProtectionKey")]
		NSString FileProtectionKey { get; }

		[Field ("NSFileProtectionNone")]
		NSString FileProtectionNone { get; }

		[Field ("NSFileProtectionComplete")]
		NSString FileProtectionComplete { get; }

		[Since (5,0)]
		[Field ("NSFileProtectionCompleteUnlessOpen")]
		NSString FileProtectionCompleteUnlessOpen { get; }

		[Since (5,0)]
		[Field ("NSFileProtectionCompleteUntilFirstUserAuthentication")]
		NSString FileProtectionCompleteUntilFirstUserAuthentication  { get; }
#endif
		[Field("NSFileSystemSize")]
		NSString SystemSize { get; }

		[Field("NSFileSystemFreeSize")]
		NSString SystemFreeSize { get; }

		[Field("NSFileSystemNodes")]
		NSString SystemNodes { get; }

		[Field("NSFileSystemFreeNodes")]
		NSString SystemFreeNodes { get; }

		[Static, Export ("defaultManager")]
		NSFileManager DefaultManager { get; }

		[Export ("delegate")]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSFileManagerDelegate Delegate { get; set; }

		[Export ("setAttributes:ofItemAtPath:error:")]
		bool SetAttributes (NSDictionary attributes, string path, out NSError error);

		[Export ("createDirectoryAtPath:withIntermediateDirectories:attributes:error:")]
		bool CreateDirectory (string path, bool createIntermediates, [NullAllowed] NSDictionary attributes, out NSError error);

		[Export ("contentsOfDirectoryAtPath:error:")]
		string[] GetDirectoryContent (string path, out NSError error);

		[Export ("subpathsOfDirectoryAtPath:error:")]
		string[] GetDirectoryContentRecursive (string path, out NSError error);

		[Export ("attributesOfItemAtPath:error:")][Internal]
		NSDictionary _GetAttributes (string path, out NSError error);

		[Export ("attributesOfFileSystemForPath:error:")][Internal]
		NSDictionary _GetFileSystemAttributes (String path, out NSError error);

		[Export ("createSymbolicLinkAtPath:withDestinationPath:error:")]
		bool CreateSymbolicLink (string path, string destPath, out NSError error);

		[Export ("destinationOfSymbolicLinkAtPath:error:")]
		string GetSymbolicLinkDestination (string path, out NSError error);

		[Export ("copyItemAtPath:toPath:error:")]
		bool Copy (string srcPath, string dstPath, out NSError error);

		[Export ("moveItemAtPath:toPath:error:")]
		bool Move (string srcPath, string dstPath, out NSError error);

		[Export ("linkItemAtPath:toPath:error:")]
		bool Link (string srcPath, string dstPath, out NSError error);

		[Export ("removeItemAtPath:error:")]
		bool Remove ([NullAllowed] string path, out NSError error);

#if DEPRECATED
		// These are not available on iOS, and deprecated on OSX.
		[Export ("linkPath:toPath:handler:")]
		bool LinkPath (string src, string dest, IntPtr handler);

		[Export ("copyPath:toPath:handler:")]
		bool CopyPath (string src, string dest, IntPtr handler);

		[Export ("movePath:toPath:handler:")]
		bool MovePath (string src, string dest, IntPtr handler);

		[Export ("removeFileAtPath:handler:")]
		bool RemoveFileAtPath (string path, IntPtr handler);
#endif
		[Export ("currentDirectoryPath")]
		string GetCurrentDirectory ();

		[Export ("changeCurrentDirectoryPath:")]
		bool ChangeCurrentDirectory (string path);

		[Export ("fileExistsAtPath:")]
		bool FileExists (string path);

		[Export ("fileExistsAtPath:isDirectory:")]
		bool FileExists (string path, ref bool isDirectory);

		[Export ("isReadableFileAtPath:")]
		bool IsReadableFile (string path);

		[Export ("isWritableFileAtPath:")]
		bool IsWritableFile (string path);

		[Export ("isExecutableFileAtPath:")]
		bool IsExecutableFile (string path);

		[Export ("isDeletableFileAtPath:")]
		bool IsDeletableFile (string path);

		[Export ("contentsEqualAtPath:andPath:")]
		bool ContentsEqual (string path1, string path2);

		[Export ("displayNameAtPath:")]
		string DisplayName (string path);

		[Export ("componentsToDisplayForPath:")]
		string[] ComponentsToDisplay (string path);

		[Export ("enumeratorAtPath:")]
		NSDirectoryEnumerator GetEnumerator (string path);

		[Export ("subpathsAtPath:")]
		string[] Subpaths (string path);

		[Export ("contentsAtPath:")]
		NSData Contents (string path);

		[Export ("createFileAtPath:contents:attributes:")]
		bool CreateFile (string path, NSData data, [NullAllowed] NSDictionary attr);

		[Since (4,0)]
		[Export ("contentsOfDirectoryAtURL:includingPropertiesForKeys:options:error:")]
		NSUrl[] GetDirectoryContent (NSUrl url, NSArray properties, NSDirectoryEnumerationOptions options, out NSError error);

		[Since (4,0)]
		[Export ("copyItemAtURL:toURL:error:")]
		bool Copy (NSUrl srcUrl, NSUrl dstUrl, out NSError error);

		[Since (4,0)]
		[Export ("moveItemAtURL:toURL:error:")]
		bool Move (NSUrl srcUrl, NSUrl dstUrl, out NSError error);

		[Since (4,0)]
		[Export ("linkItemAtURL:toURL:error:")]
		bool Link (NSUrl srcUrl, NSUrl dstUrl, out NSError error);

		[Since (4,0)]
		[Export ("removeItemAtURL:error:")]
		bool Remove ([NullAllowed] NSUrl url, out NSError error);

		[Since (4,0)]
		[Export ("enumeratorAtURL:includingPropertiesForKeys:options:errorHandler:")]
		NSDirectoryEnumerator GetEnumerator (NSUrl url, [NullAllowed] NSArray properties, NSDirectoryEnumerationOptions options, [NullAllowed] NSEnumerateErrorHandler handler);

		[Since (4,0)]
		[Export ("URLForDirectory:inDomain:appropriateForURL:create:error:")]
		NSUrl GetUrl (NSSearchPathDirectory directory, NSSearchPathDomain domain, [NullAllowed] NSUrl url, bool shouldCreate, out NSError error);

		[Since (4,0)]
		[Export ("URLsForDirectory:inDomains:")]
		NSUrl[] GetUrls (NSSearchPathDirectory directory, NSSearchPathDomain domains);

		[Since (4,0)]
		[Export ("replaceItemAtURL:withItemAtURL:backupItemName:options:resultingItemURL:error:")]
		bool Replace (NSUrl originalItem, NSUrl newItem, [NullAllowed] string backupItemName, NSFileManagerItemReplacementOptions options, out NSUrl resultingURL, out NSError error);

		[Since (4,0)]
		[Export ("mountedVolumeURLsIncludingResourceValuesForKeys:options:")]
		NSUrl[] GetMountedVolumes ([NullAllowed] NSArray properties, NSVolumeEnumerationOptions options);

		// Methods to convert paths to/from C strings for passing to system calls - Not implemented
		////- (const char *)fileSystemRepresentationWithPath:(NSString *)path;
		//[Export ("fileSystemRepresentationWithPath:")]
		//const char FileSystemRepresentationWithPath (string path);

		////- (NSString *)stringWithFileSystemRepresentation:(const char *)str length:(nuint)len;
		//[Export ("stringWithFileSystemRepresentation:length:")]
		//string StringWithFileSystemRepresentation (const char str, uint len);

		[Since (5,0)]
		[Export ("createDirectoryAtURL:withIntermediateDirectories:attributes:error:")]
		bool CreateDirectory (NSUrl url, bool createIntermediates, [NullAllowed] NSDictionary attributes, out NSError error);

		[Since (5,0)]
                [Export ("createSymbolicLinkAtURL:withDestinationURL:error:")]
                bool CreateSymbolicLink (NSUrl url, NSUrl destURL, out NSError error);

		[Since (5,0)]
                [Export ("setUbiquitous:itemAtURL:destinationURL:error:")]
                bool SetUbiquitous (bool flag, NSUrl url, NSUrl destinationUrl, out NSError error);

		[Since (5,0)]
                [Export ("isUbiquitousItemAtURL:")]
                bool IsUbiquitous (NSUrl url);

		[Since (5,0)]
                [Export ("startDownloadingUbiquitousItemAtURL:error:")]
                bool StartDownloadingUbiquitous (NSUrl url, out NSError error);

		[Since (5,0)]
                [Export ("evictUbiquitousItemAtURL:error:")]
                bool EvictUbiquitous (NSUrl url, out NSError error);

		[Since (5,0)]
                [Export ("URLForUbiquityContainerIdentifier:")]
                NSUrl GetUrlForUbiquityContainer ([NullAllowed] string containerIdentifier);

		[Since (5,0)]
                [Export ("URLForPublishingUbiquitousItemAtURL:expirationDate:error:")]
                NSUrl GetUrlForPublishingUbiquitousItem (NSUrl url, out NSDate expirationDate, out NSError error);

		[Since (6,0)]
		[MountainLion]
		[Export ("ubiquityIdentityToken")]
		NSObject UbiquityIdentityToken { get; }

		[Since (6,0)]
		[MountainLion]
		[Field ("NSUbiquityIdentityDidChangeNotification")]
		[Notification]
		NSString UbiquityIdentityDidChangeNotification { get; }
	}

	[BaseType(typeof(NSObject))]
	[Model]
	[Protocol]
	public interface NSFileManagerDelegate {
		[Export("fileManager:shouldCopyItemAtPath:toPath:")]
		bool ShouldCopyItemAtPath(NSFileManager fm, NSString srcPath, NSString dstPath);

#if MONOTOUCH
		[Export("fileManager:shouldCopyItemAtURL:toURL:")]
		bool ShouldCopyItemAtUrl(NSFileManager fm, NSUrl srcUrl, NSUrl dstUrl);
		
		[Export ("fileManager:shouldLinkItemAtURL:toURL:")]
		bool ShouldLinkItemAtUrl (NSFileManager fileManager, NSUrl srcUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldMoveItemAtURL:toURL:")]
		bool ShouldMoveItemAtUrl (NSFileManager fileManager, NSUrl srcUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldProceedAfterError:copyingItemAtURL:toURL:")]
		bool ShouldProceedAfterErrorCopyingItem (NSFileManager fileManager, NSError error, NSUrl srcUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldProceedAfterError:linkingItemAtURL:toURL:")]
		bool ShouldProceedAfterErrorLinkingItem (NSFileManager fileManager, NSError error, NSUrl srcUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldProceedAfterError:movingItemAtURL:toURL:")]
		bool ShouldProceedAfterErrorMovingItem (NSFileManager fileManager, NSError error, NSUrl srcUrl, NSUrl dstUrl);

		[Export ("fileManager:shouldRemoveItemAtURL:")]
		bool ShouldRemoveItemAtUrl (NSFileManager fileManager, NSUrl url);

		[Export ("fileManager:shouldProceedAfterError:removingItemAtURL:")]
		bool ShouldProceedAfterErrorRemovingItem (NSFileManager fileManager, NSError error, NSUrl url);
#endif
		
		[Export ("fileManager:shouldProceedAfterError:")]
		bool ShouldProceedAfterError (NSFileManager fm, NSDictionary errorInfo);

		// Deprecated
		//[Export ("fileManager:willProcessPath:")]
		//void WillProcessPath (NSFileManager fm, string path);

		[Export ("fileManager:shouldCopyItemAtPath:toPath:")]
		bool ShouldCopyItemAtPath (NSFileManager fileManager, string srcPath, string dstPath);

		[Export ("fileManager:shouldProceedAfterError:copyingItemAtPath:toPath:")]
		bool ShouldProceedAfterErrorCopyingItem (NSFileManager fileManager, NSError error, string srcPath, string dstPath);

		[Export ("fileManager:shouldMoveItemAtPath:toPath:")]
		bool ShouldMoveItemAtPath (NSFileManager fileManager, string srcPath, string dstPath);

		[Export ("fileManager:shouldProceedAfterError:movingItemAtPath:toPath:")]
		bool ShouldProceedAfterErrorMovingItem (NSFileManager fileManager, NSError error, string srcPath, string dstPath);

		[Export ("fileManager:shouldLinkItemAtPath:toPath:")]
		bool ShouldLinkItemAtPath (NSFileManager fileManager, string srcPath, string dstPath);

		[Export ("fileManager:shouldProceedAfterError:linkingItemAtPath:toPath:")]
		bool ShouldProceedAfterErrorLinkingItem (NSFileManager fileManager, NSError error, string srcPath, string dstPath);

		[Export ("fileManager:shouldRemoveItemAtPath:")]
		bool ShouldRemoveItemAtPath (NSFileManager fileManager, string path);

		[Export ("fileManager:shouldProceedAfterError:removingItemAtPath:")]
		bool ShouldProceedAfterErrorRemovingItem (NSFileManager fileManager, NSError error, string path);
	}

	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	partial interface NSFilePresenter {
		[Abstract]
		[Export ("presentedItemURL")]
		NSUrl PresentedItemURL { get; }

		[Export ("presentedItemOperationQueue")]
		NSOperationQueue PesentedItemOperationQueue { get; }

#if DOUBLE_BLOCKS
		[Export ("relinquishPresentedItemToReader:")]
		void RelinquishPresentedItem (Action readerAction);

		[Export ("relinquishPresentedItemToWriter:")]
		void RelinquishPresentedItem (Action writerAction);

		[Export ("savePresentedItemChangesWithCompletionHandler:")]
		void SavePresentedItemChanges (...);

		[Export ("accommodatePresentedItemDeletionWithCompletionHandler:")]
		void AccommodatePresentedItem (..);
#endif

		[Export ("presentedItemDidMoveToURL:")]
		void PresentedItemMoved (NSUrl newURL);

		[Export ("presentedItemDidChange")]
		void PresentedItemChanged ();

		[Export ("presentedItemDidGainVersion:")]
		void PresentedItemGainedVersion (NSFileVersion version);

		[Export ("presentedItemDidLoseVersion:")]
		void PresentedItemLostVersion (NSFileVersion version);

		[Export ("presentedItemDidResolveConflictVersion:")]
		void PresentedItemResolveConflictVersion (NSFileVersion version);

#if DOUBLE_BLOCKS
		[Export ("accommodatePresentedSubitemDeletionAtURL:completionHandler:NSError*errorOrNil))completionHandler")]
		void AccommodatePresentedSubitemDeletion (NSUrl url, 
#endif
		[Export ("presentedSubitemDidAppearAtURL:")]
		void PresentedSubitemAppeared (NSUrl atUrl);

		[Export ("presentedSubitemAtURL:didMoveToURL:")]
		void PresentedSubitemMoved (NSUrl oldURL, NSUrl newURL);

		[Export ("presentedSubitemDidChangeAtURL:")]
		void PresentedSubitemChanged (NSUrl url);

		[Export ("presentedSubitemAtURL:didGainVersion:")]
		void PresentedSubitemGainedVersion (NSUrl url, NSFileVersion version);

		[Export ("presentedSubitemAtURL:didLoseVersion:")]
		void PresentedSubitemLostVersion (NSUrl url, NSFileVersion version);

		[Export ("presentedSubitemAtURL:didResolveConflictVersion:")]
		void PresentedSubitemResolvedConflictVersion (NSUrl url, NSFileVersion version);
	}
		
	[BaseType (typeof (NSObject))]
	// Objective-C exception thrown.  Name: NSGenericException Reason: -[NSFileVersion init]: You have to use one of the factory methods to instantiate NSFileVersion.
	[DisableDefaultCtor]
	interface NSFileVersion {
		[Export ("URL")]
		NSUrl Url { get;  }

		[Export ("localizedName")]
		string LocalizedName { get;  }

		[Export ("localizedNameOfSavingComputer")]
		string LocalizedNameOfSavingComputer { get;  }

		[Export ("modificationDate")]
		NSDate ModificationDate { get;  }

		[Export ("persistentIdentifier")]
		NSObject PersistentIdentifier { get;  }

		[Export ("conflict")]
		bool IsConflict { [Bind ("isConflict")] get;  }

		[Export ("resolved")]
		bool Resolved { [Bind ("isResolved")] get; set;  }
#if MONOMAC
		[Export ("discardable")]
		bool Discardable { [Bind ("isDiscardable")] get; set;  }
#endif
		[Static]
		[Export ("currentVersionOfItemAtURL:")]
		NSFileVersion GetCurrentVersion (NSUrl url);

		[Static]
		[Export ("otherVersionsOfItemAtURL:")]
		NSFileVersion [] GetOtherVersions (NSUrl url);

		[Static]
		[Export ("unresolvedConflictVersionsOfItemAtURL:")]
		NSFileVersion [] GetUnresolvedConflictVersions (NSUrl url);

		[Static]
		[Export ("versionOfItemAtURL:forPersistentIdentifier:")]
		NSFileVersion GetSpecificVersion (NSUrl url, NSObject persistentIdentifier);

#if MONOMAC
		[Static]
		[Export ("addVersionOfItemAtURL:withContentsOfURL:options:error:")]
		NSFileVersion AddVersion (NSUrl url, NSUrl contentsURL, NSFileVersionAddingOptions options, out NSError outError);

		[Static]
		[Export ("temporaryDirectoryURLForNewVersionOfItemAtURL:")]
		NSUrl TemporaryDirectoryForItem (NSUrl url);
#endif

		[Export ("replaceItemAtURL:options:error:")]
		NSUrl ReplaceItem (NSUrl url, NSFileVersionReplacingOptions options, out NSError error);

		[Export ("removeAndReturnError:")]
		bool Remove (out NSError outError);

		[Static]
		[Export ("removeOtherVersionsOfItemAtURL:error:")]
		bool RemoveOtherVersions (NSUrl url, out NSError outError);
	}

	[BaseType (typeof (NSObject))]
	public interface NSFileWrapper {
		[Export ("initWithURL:options:error:")]
		IntPtr Constructor (NSUrl url, NSFileWrapperReadingOptions options, out NSError outError);

		[Export ("initDirectoryWithFileWrappers:")]
		IntPtr Constructor (NSDictionary childrenByPreferredName);

		[Export ("initRegularFileWithContents:")]
		IntPtr Constructor (NSData contents);

		[Export ("initSymbolicLinkWithDestinationURL:")]
		IntPtr Constructor (NSUrl urlToSymbolicLink);

		// Constructor clash
		//[Export ("initWithSerializedRepresentation:")]
		//IntPtr Constructor (NSData serializeRepresentation);

		[Export ("isDirectory")]
		bool IsDirectory { get; }

		[Export ("isRegularFile")]
		bool IsRegularFile { get; }

		[Export ("isSymbolicLink")]
		bool IsSymbolicLink { get; }

		[Export ("matchesContentsOfURL:")]
		bool MatchesContentsOfURL (NSUrl url);

		[Export ("readFromURL:options:error:")]
		bool Read (NSUrl url, NSFileWrapperReadingOptions options, out NSError outError);

		[Export ("writeToURL:options:originalContentsURL:error:")]
		bool Write (NSUrl url, NSFileWrapperWritingOptions options, NSUrl originalContentsURL, out NSError outError);

		[Export ("serializedRepresentation")]
		NSData GetSerializedRepresentation ();

		[Export ("addFileWrapper:")]
		string AddFileWrapper (NSFileWrapper child);

		[Export ("addRegularFileWithContents:preferredFilename:")]
		string AddRegularFile (NSData dataContents, string preferredFilename);

		[Export ("removeFileWrapper:")]
		void RemoveFileWrapper (NSFileWrapper child);

		[Export ("fileWrappers")]
		NSDictionary FileWrappers { get; }

		[Export ("keyForFileWrapper:")]
		string KeyForFileWrapper (NSFileWrapper child);

		[Export ("regularFileContents")]
		NSData GetRegularFileContents ();

		[Export ("symbolicLinkDestinationURL")]
		NSUrl SymbolicLinkDestinationURL { get; }

		//Detected properties
		[Export ("preferredFilename")]
		string PreferredFilename { get; set; }

		[Export ("filename")]
		string Filename { get; set; }

		[Export ("fileAttributes")]
		NSDictionary FileAttributes { get; set; }
	}

	[BaseType (typeof (NSEnumerator))]
	public interface NSDirectoryEnumerator {
		[Export ("fileAttributes")]
		NSDictionary FileAttributes { get; }

		[Export ("directoryAttributes")]
		NSDictionary DirectoryAttributes { get; }

		[Export ("skipDescendents")]
		void SkipDescendents ();

#if MONOTOUCH
		[Export ("level")]
		int Level { get; }
#endif
#if MONOMAC
		////- (unsigned long long)fileSize;
		//[Export ("fileSize")]
		//unsigned long long FileSize ([Target] NSDictionary fileAttributes);

		[Export ("fileModificationDate")]
		NSDate FileModificationDate ([Target] NSDictionary fileAttributes);

		[Export ("fileType")]
		string FileType ([Target] NSDictionary fileAttributes);

		[Export ("filePosixPermissions")]
		uint FilePosixPermissions ([Target] NSDictionary fileAttributes);

		[Export ("fileOwnerAccountName")]
		string FileOwnerAccountName ([Target] NSDictionary fileAttributes);

		[Export ("fileGroupOwnerAccountName")]
		string FileGroupOwnerAccountName ([Target] NSDictionary fileAttributes);

		[Export ("fileSystemNumber")]
		nint FileSystemNumber ([Target] NSDictionary fileAttributes);

		[Export ("fileSystemFileNumber")]
		nuint FileSystemFileNumber ([Target] NSDictionary fileAttributes);

		[Export ("fileExtensionHidden")]
		bool FileExtensionHidden ([Target] NSDictionary fileAttributes);

		[Export ("fileHFSCreatorCode")]
		uint FileHfsCreatorCode ([Target] NSDictionary fileAttributes);

		[Export ("fileHFSTypeCode")]
		uint FileHfsTypeCode ([Target] NSDictionary fileAttributes);

		[Export ("fileIsImmutable")]
		bool FileIsImmutable ([Target] NSDictionary fileAttributes);

		[Export ("fileIsAppendOnly")]
		bool FileIsAppendOnly ([Target] NSDictionary fileAttributes);

		[Export ("fileCreationDate")]
		NSDate FileCreationDate ([Target] NSDictionary fileAttributes);

		[Export ("fileOwnerAccountID")]
		NSNumber FileOwnerAccountID ([Target] NSDictionary fileAttributes);

		[Export ("fileGroupOwnerAccountID")]
		NSNumber FileGroupOwnerAccountID ([Target] NSDictionary fileAttributes);
#endif
	}

	public delegate bool NSPredicateEvaluator (NSObject evaluatedObject, NSDictionary bindings);
	
	[BaseType (typeof (NSObject))]
	[Since (4,0)]
	// 'init' returns NIL
	[DisableDefaultCtor]
	public interface NSPredicate {
		[Static]
		[Export ("predicateWithFormat:argumentArray:")]
		NSPredicate FromFormat (string predicateFormat, NSObject[] arguments);

		[Static, Export ("predicateWithValue:")]
		NSPredicate FromValue (bool value);

		[Static, Export ("predicateWithBlock:")]
		NSPredicate FromExpression (NSPredicateEvaluator evaluator);

		[Export ("predicateFormat")]
		string PredicateFormat { get; }

		[Export ("predicateWithSubstitutionVariables:")]
		NSPredicate PredicateWithSubstitutionVariables (NSDictionary substitutionVariables);

		[Export ("evaluateWithObject:")]
		bool EvaluateWithObject (NSObject obj);

		[Export ("evaluateWithObject:substitutionVariables:")]
		bool EvaluateWithObject (NSObject obj, NSDictionary substitutionVariables);
	}

#if MONOMAC
	[BaseType (typeof (NSObject), Name="NSURLDownload")]
	public interface NSUrlDownload {
		[Static, Export ("canResumeDownloadDecodedWithEncodingMIMEType:")]
		bool CanResumeDownloadDecodedWithEncodingMimeType (string mimeType);

		[Export ("initWithRequest:delegate:")]
		IntPtr Constructor (NSUrlRequest request, NSObject delegate1);

		[Export ("initWithResumeData:delegate:path:")]
		IntPtr Constructor (NSData resumeData, NSObject delegate1, string path);

		[Export ("cancel")]
		void Cancel ();

		[Export ("setDestination:allowOverwrite:")]
		void SetDestination (string path, bool allowOverwrite);

		[Export ("request")]
		NSUrlRequest Request { get; }

		[Export ("resumeData")]
		NSData ResumeData { get; }

		[Export ("deletesFileUponFailure")]
		bool DeletesFileUponFailure { get; set; }
	}

    	[BaseType (typeof (NSObject))]
    	[Model]
	public interface NSUrlDownloadDelegate {
		[Export ("downloadDidBegin:")]
		void DownloadBegan (NSUrlDownload download);

		[Export ("download:willSendRequest:redirectResponse:")]
		NSUrlRequest WillSendRequest (NSUrlDownload download, NSUrlRequest request, NSUrlResponse redirectResponse);

		[Export ("download:didReceiveAuthenticationChallenge:")]
		void ReceivedAuthenticationChallenge (NSUrlDownload download, NSUrlAuthenticationChallenge challenge);

		[Export ("download:didCancelAuthenticationChallenge:")]
		void CanceledAuthenticationChallenge (NSUrlDownload download, NSUrlAuthenticationChallenge challenge);

		[Export ("download:didReceiveResponse:")]
		void ReceivedResponse (NSUrlDownload download, NSUrlResponse response);

		//- (void)download:(NSUrlDownload *)download willResumeWithResponse:(NSUrlResponse *)response fromByte:(long long)startingByte;
		[Export ("download:willResumeWithResponse:fromByte:")]
		void Resume (NSUrlDownload download, NSUrlResponse response, long startingByte);

		//- (void)download:(NSUrlDownload *)download didReceiveDataOfLength:(nuint)length;
		[Export ("download:didReceiveDataOfLength:")]
		void ReceivedData (NSUrlDownload download, uint length);

		[Export ("download:shouldDecodeSourceDataOfMIMEType:")]
		bool DecodeSourceData (NSUrlDownload download, string encodingType);

		[Export ("download:decideDestinationWithSuggestedFilename:")]
		void DecideDestination (NSUrlDownload download, string suggestedFilename);

		[Export ("download:didCreateDestination:")]
		void CreatedDestination (NSUrlDownload download, string path);

		[Export ("downloadDidFinish:")]
		void Finished (NSUrlDownload download);

		[Export ("download:didFailWithError:")]
		void FailedWithError(NSUrlDownload download, NSError error);
	}
#endif

	interface NSUrlProtocolClient {
	}

	[BaseType (typeof (NSObject),
		   Name="NSURLProtocol",
		   Delegates=new string [] {"WeakClient"})]
	interface NSUrlProtocol {
		[Export ("initWithRequest:cachedResponse:client:")]
		IntPtr Constructor (NSUrlRequest request, [NullAllowed] NSCachedUrlResponse cachedResponse, NSUrlProtocolClient client);

		[Export ("client")]
		NSObject WeakClient { get; }

		[Export ("request")]
		NSUrlRequest Request { get; }

		[Export ("cachedResponse")]
		NSCachedUrlResponse CachedResponse { get; }

		[Static]
		[Export ("canInitWithRequest:")]
		bool CanInitWithRequest (NSUrlRequest request);

		[Static]
		[Export ("canonicalRequestForRequest:")]
		NSUrlRequest GetCanonicalRequest (NSUrlRequest forRequest);

		[Static]
		[Export ("requestIsCacheEquivalent:toRequest:")]
		bool IsRequestCacheEquivalent (NSUrlRequest first, NSUrlRequest second);

		[Export ("startLoading")]
		void StartLoading ();

		[Export ("stopLoading")]
		void StopLoading ();

		[Static]
		[Export ("propertyForKey:inRequest:")]
		NSObject GetProperty (string key, NSUrlRequest inRequest);

		[Static]
		[Export ("setProperty:forKey:inRequest:")]
		void SetProperty ([NullAllowed] NSObject value, string key, NSMutableUrlRequest inRequest);

		[Static]
		[Export ("removePropertyForKey:inRequest:")]
		void RemoveProperty (string propertyKey, NSMutableUrlRequest request);

		[Static]
		[Export ("registerClass:")]
		bool RegisterClass (Class protocolClass);

		[Static]
		[Export ("unregisterClass:")]
		void UnregisterClass (Class protocolClass);
	}

	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	public interface NSPropertyListSerialization {
		[Static, Export ("dataWithPropertyList:format:options:error:")]
		NSData DataWithPropertyList (NSObject plist, NSPropertyListFormat format,
			NSPropertyListWriteOptions options, out NSError error);

		[Static, Export ("writePropertyList:toStream:format:options:error:")]
		int WritePropertyList (NSObject plist, NSOutputStream stream, NSPropertyListFormat format,
			NSPropertyListWriteOptions options, out NSError error);

		[Static, Export ("propertyListWithData:options:format:error:")]
		NSObject PropertyListWithData (NSData data, NSPropertyListReadOptions options,
			ref NSPropertyListFormat format, out NSError error);

		[Static, Export ("propertyListWithStream:options:format:error:")]
		NSObject PropertyListWithStream (NSInputStream stream, NSPropertyListReadOptions options,
			ref NSPropertyListFormat format, out NSError error);

		[Static, Export ("propertyList:isValidForFormat:")]
		bool IsValidForFormat (NSObject plist, NSPropertyListFormat format);
	}
}

