//
// This file describes the API that the generator will produce
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012 Xamarin Inc.
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
using System;
using MonoMac.ObjCRuntime;
using MonoMac.CoreFoundation;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.CoreMedia;

namespace MonoMac.Foundation {
	public enum NSCalendarType {
		Gregorian, Buddhist, Chinese, Hebrew, Islamic, IslamicCivil, Japanese, RepublicOfChina, Persian, Indian, ISO8601
	}
	
	public partial class NSCalendar {
		static NSString GetCalendarIdentifier (NSCalendarType type)
		{
			switch (type){
			case NSCalendarType.Gregorian:
				return NSGregorianCalendar;
			case NSCalendarType.Buddhist:
				return NSBuddhistCalendar;
			case NSCalendarType.Chinese:
				return NSChineseCalendar;
			case NSCalendarType.Hebrew:
				return NSHebrewCalendar;
			case NSCalendarType.Islamic:
				return NSIslamicCalendar; 
			case NSCalendarType.IslamicCivil:
				return NSIslamicCivilCalendar;
			case NSCalendarType.Japanese:
				return NSJapaneseCalendar;
			case NSCalendarType.RepublicOfChina:
				return NSRepublicOfChinaCalendar;
			case NSCalendarType.Persian:
				return NSPersianCalendar;
			case NSCalendarType.Indian:
				return NSIndianCalendar;
			case NSCalendarType.ISO8601:
				return NSISO8601Calendar;
			default:
				throw new ArgumentException ("Unknown NSCalendarType value");
			}
		}
		
		public NSCalendar (NSCalendarType calendarType) : this (GetCalendarIdentifier (calendarType)) {}
	}
}