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

	[BaseType (typeof (NSObject))]
	interface NSLayoutManager {
		[Export ("init")]
		IntPtr Constructor ();

		[Export ("attributedString")]
		NSAttributedString AttributedString { get; }

		[Export ("replaceTextStorage:")]
		void ReplaceTextStorage (NSTextStorage newTextStorage);

		[Export ("textContainers")]
		NSTextContainer [] TextContainers { get; }

		[Export ("addTextContainer:")]
		void AddTextContainer (NSTextContainer container);

		[Export ("insertTextContainer:atIndex:")]
		void InsertTextContainer (NSTextContainer container, nuint index);

		[Export ("removeTextContainerAtIndex:")]
		void RemoveTextContainer (nuint index);

		[Export ("textContainerChangedGeometry:")]
		void TextContainerChangedGeometry (NSTextContainer container);

		[Export ("textContainerChangedTextView:")]
		void TextContainerChangedTextView (NSTextContainer container);

		[Export ("layoutOptions")]
		NSGlyphStorageOptions LayoutOptions { get; }

		[Export ("hasNonContiguousLayout")]
		bool HasNonContiguousLayout { get; }

		// FIXME: binding
		//[Export ("invalidateGlyphsForCharacterRange:changeInLength:actualCharacterRange:")]
		//void InvalidateGlyphs (NSRange charRange, nint delta, NSRangePointer actualCharRange);
		//
		//[Export ("invalidateLayoutForCharacterRange:actualCharacterRange:")]
		//void InvalidateLayout (NSRange charRange, NSRangePointer actualCharRange);
		//
		//[Export ("invalidateLayoutForCharacterRange:isSoft:actualCharacterRange:")]
		//void InvalidateLayout (NSRange charRange, bool flag, NSRangePointer actualCharRange);

		[Export ("invalidateDisplayForCharacterRange:")]
		void InvalidateDisplayForCharRange (NSRange charRange);

		[Export ("invalidateDisplayForGlyphRange:")]
		void InvalidateDisplayForGlyphRange (NSRange glyphRange);

		[Export ("textStorage:edited:range:changeInLength:invalidatedRange:")]
		void TextStorageEdited (NSTextStorage str, NSTextStorageEditedFlags editedMask, NSRange newCharRange, nint delta, NSRange invalidatedCharRange);

		[Export ("ensureGlyphsForCharacterRange:")]
		void EnsureGlyphsForCharacterRange (NSRange charRange);

		[Export ("ensureGlyphsForGlyphRange:")]
		void EnsureGlyphsForGlyphRange (NSRange glyphRange);

		[Export ("ensureLayoutForCharacterRange:")]
		void EnsureLayoutForCharacterRange (NSRange charRange);

		[Export ("ensureLayoutForGlyphRange:")]
		void EnsureLayoutForGlyphRange (NSRange glyphRange);

		[Export ("ensureLayoutForTextContainer:")]
		void EnsureLayoutForTextContainer (NSTextContainer container);

		[Export ("ensureLayoutForBoundingRect:inTextContainer:")]
		void EnsureLayout (CGRect bounds, NSTextContainer container);

		// FIXME: binding
		//[Export ("insertGlyphs:length:forStartingGlyphAtIndex:characterIndex:")]
		//void InsertGlyphs (const NSGlyph glyphs, nuint length, nuint glyphIndex, nuint charIndex);

		[Export ("insertGlyph:atGlyphIndex:characterIndex:")]
		void InsertGlyph (uint glyph, nuint glyphIndex, nuint charIndex); // 32-bit

		[Export ("replaceGlyphAtIndex:withGlyph:")]
		void ReplaceGlyph (nuint glyphIndex, uint newGlyph); // 32-bit

		[Export ("deleteGlyphsInRange:")]
		void DeleteGlyphs (NSRange glyphRange);

		[Export ("setCharacterIndex:forGlyphAtIndex:")]
		void SetCharacter (nuint charIndex, nuint glyphIndex);

		[Export ("setIntAttribute:value:forGlyphAtIndex:")]
		void SetIntAttribute (nint attributeTag, nint val, nuint glyphIndex);

		[Export ("invalidateGlyphsOnLayoutInvalidationForGlyphRange:")]
		void InvalidateGlyphsOnLayout (NSRange glyphRange);

		[Export ("numberOfGlyphs")]
		nuint NumberOfGlyphs { get; }

		[Export ("glyphAtIndex:isValidIndex:")]
		uint GlyphAtIndex (nuint glyphIndex, bool isValidIndex); // 32-bit

		[Export ("glyphAtIndex:")]
		uint GlyphAtIndex (nuint glyphIndex); // 32-bit

		[Export ("isValidGlyphIndex:")]
		bool IsValidGlyphIndex (uint glyphIndex);

		[Export ("characterIndexForGlyphAtIndex:")]
		uint CharacterIndexForGlyphAtIndex (uint glyphIndex);

		[Export ("glyphIndexForCharacterAtIndex:")]
		uint GlyphIndexForCharacterAtIndex (uint charIndex);

		[Export ("intAttribute:forGlyphAtIndex:")]
		int IntAttributeForGlyph (int attributeTag, uint glyphIndex);

		// FIXME: bidning
		//[Export ("getGlyphsInRange:glyphs:characterIndexes:glyphInscriptions:elasticBits:")]
		//uint GetGlyphsInRange (NSRange glyphRange, uint glyphBuffer, uint charIndexBuffer, NSGlyphInscription inscribeBuffer, bool elasticBuffer);

		// FIXME: binding
		//[Export ("getGlyphsInRange:glyphs:characterIndexes:glyphInscriptions:elasticBits:bidiLevels:")]
		//uint GetGlyphsInRangeglyphscharacterIndexesglyphInscriptionselasticBitsbidiLevels (NSRange glyphRange, uint glyphBuffer, uint charIndexBuffer, NSGlyphInscription inscribeBuffer, bool elasticBuffer, unsigned char bidiLevelBuffer);
		//
		//[Export ("getGlyphs:range:")]
		//uint GetGlyphsrange (uint glyphArray, NSRange glyphRange);

		[Export ("setTextContainer:forGlyphRange:")]
		void SetTextContainer (NSTextContainer container, NSRange glyphRange);

		[Export ("setLineFragmentRect:forGlyphRange:usedRect:")]
		void SetLineFragmentRect (CGRect fragmentRect, NSRange glyphRange, CGRect usedRect);

		[Export ("setExtraLineFragmentRect:usedRect:textContainer:")]
		void SetExtraLineFragment (CGRect fragmentRect, CGRect usedRect, NSTextContainer container);

		[Export ("setLocation:forStartOfGlyphRange:")]
		void SetLocationforStartOfGlyphRange (CGPoint location, NSRange glyphRange);

		// FIXME: binding
		//[Export ("setLocations:startingGlyphIndexes:count:forGlyphRange:")]
		//void SetLocations (NSPointArray locations, uint glyphIndexes, uint count, NSRange glyphRange);

		[Export ("setNotShownAttribute:forGlyphAtIndex:")]
		void SetNotShownAttribute (bool flag, uint glyphIndex);

		[Export ("setDrawsOutsideLineFragment:forGlyphAtIndex:")]
		void SetDrawsOutsideLineFragment (bool flag, uint glyphIndex);

		[Export ("setAttachmentSize:forGlyphRange:")]
		void SetAttachmentSizeforGlyphRange (CGSize attachmentSize, NSRange glyphRange);

		[Export ("getFirstUnlaidCharacterIndex:glyphIndex:")]
		void GetFirstUnlaidCharacterIndex (uint charIndex, uint glyphIndex);

		[Export ("firstUnlaidCharacterIndex")]
		uint FirstUnlaidCharacterIndex { get; }

		[Export ("firstUnlaidGlyphIndex")]
		uint FirstUnlaidGlyphIndex { get; }

		[Export ("textContainerForGlyphAtIndex:effectiveRange:")]
		NSTextContainer TextContainerForGlyph (uint glyphIndex, NSRangePointer effectiveGlyphRange);

		[Export ("usedRectForTextContainer:")]
		CGRect UsedRectForTextContainer (NSTextContainer container);

		[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:")]
		CGRect LineFragmentRectForGlyph (uint glyphIndex, NSRangePointer effectiveGlyphRange);

		[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:")]
		CGRect LineFragmentUsedRectForGlyph (uint glyphIndex, NSRangePointer effectiveGlyphRange);

		[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		CGRect LineFragmentRectForGlyph (uint glyphIndex, NSRangePointer effectiveGlyphRange, bool flag);

		[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		CGRect LineFragmentUsedRectForGlyph (uint glyphIndex, NSRangePointer effectiveGlyphRange, bool flag);

		[Export ("textContainerForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		NSTextContainer TextContainerForGlyph (uint glyphIndex, NSRangePointer effectiveGlyphRange, bool flag);

		[Export ("extraLineFragmentRect")]
		CGRect ExtraLineFragmentRect { get; }

		[Export ("extraLineFragmentUsedRect")]
		CGRect ExtraLineFragmentUsedRect { get; }

		[Export ("extraLineFragmentTextContainer")]
		NSTextContainer ExtraLineFragmentTextContainer { get; }

		[Export ("locationForGlyphAtIndex:")]
		CGPoint LocationForGlyph (nuint glyphIndex);

		[Export ("notShownAttributeForGlyphAtIndex:")]
		bool NotShownAttributeForGlyph (nuint glyphIndex);

		[Export ("drawsOutsideLineFragmentForGlyphAtIndex:")]
		bool DrawsOutsideLineFragmentForGlyph (nuint glyphIndex);

		[Export ("attachmentSizeForGlyphAtIndex:")]
		CGSize AttachmentSizeForGlyph (nuint glyphIndex);

		[Export ("setLayoutRect:forTextBlock:glyphRange:")]
		void SetLayoutRectforTextBlock (CGRect rect, NSTextBlock block, NSRange glyphRange);

		[Export ("setBoundsRect:forTextBlock:glyphRange:")]
		void SetBoundsRectforTextBlock (CGRect rect, NSTextBlock block, NSRange glyphRange);

		[Export ("layoutRectForTextBlock:glyphRange:")]
		CGRect LayoutRectForTextBlock (NSTextBlock block, NSRange glyphRange);

		[Export ("boundsRectForTextBlock:glyphRange:")]
		CGRect BoundsRectForTextBlock (NSTextBlock block, NSRange glyphRange);

		[Export ("layoutRectForTextBlock:atIndex:effectiveRange:")]
		CGRect LayoutRectForTextBlock (NSTextBlock block, nuint glyphIndex, NSRangePointer effectiveGlyphRange);

		[Export ("boundsRectForTextBlock:atIndex:effectiveRange:")]
		CGRect BoundsRectForTextBlock (NSTextBlock block, uint glyphIndex, NSRangePointer effectiveGlyphRange);

		[Export ("glyphRangeForCharacterRange:actualCharacterRange:")]
		NSRange GlyphRangeForCharacterRange (NSRange charRange, NSRangePointer actualCharRange);

		[Export ("characterRangeForGlyphRange:actualGlyphRange:")]
		NSRange CharacterRangeForGlyphRange (NSRange glyphRange, NSRangePointer actualGlyphRange);

		[Export ("glyphRangeForTextContainer:")]
		NSRange GlyphRangeForTextContainer (NSTextContainer container);

		[Export ("rangeOfNominallySpacedGlyphsContainingIndex:")]
		NSRange RangeOfNominallySpacedGlyphsContainingIndex (nuint glyphIndex);

		// FIXME: binding
		//[Export ("rectArrayForCharacterRange:withinSelectedCharacterRange:inTextContainer:rectCount:")]
		//NSRectArray RectArrayForCharacterRangewithinSelectedCharacterRangeinTextContainerrectCount (NSRange charRange, NSRange selCharRange, NSTextContainer container, uint rectCount);
		//
		//[Export ("rectArrayForGlyphRange:withinSelectedGlyphRange:inTextContainer:rectCount:")]
		//NSRectArray RectArrayForGlyphRangewithinSelectedGlyphRangeinTextContainerrectCount (NSRange glyphRange, NSRange selGlyphRange, NSTextContainer container, uint rectCount);

		[Export ("boundingRectForGlyphRange:inTextContainer:")]
		CGRect BoundingRectForGlyphRange (NSRange glyphRange, NSTextContainer container);

		[Export ("glyphRangeForBoundingRect:inTextContainer:")]
		NSRange GlyphRangeForBoundingRect (CGRect bounds, NSTextContainer container);

		[Export ("glyphRangeForBoundingRectWithoutAdditionalLayout:inTextContainer:")]
		NSRange GlyphRangeForBoundingRectWithoutAdditionalLayout (CGRect bounds, NSTextContainer container);

		[Export ("glyphIndexForPoint:inTextContainer:fractionOfDistanceThroughGlyph:")]
		nuint GlyphIndexForPoint (CGPoint point, NSTextContainer container, nfloat partialFraction);

		[Export ("glyphIndexForPoint:inTextContainer:")]
		nuint GlyphIndexForPoint (CGPoint point, NSTextContainer container);

		[Export ("fractionOfDistanceThroughGlyphForPoint:inTextContainer:")]
		nfloat FractionOfDistance (CGPoint point, NSTextContainer container);

		[Export ("characterIndexForPoint:inTextContainer:fractionOfDistanceBetweenInsertionPoints:")]
		nuint CharacterIndexForPoint (CGPoint point, NSTextContainer container, nfloat partialFraction);

		// FIXME: binding
		//[Export ("getLineFragmentInsertionPointsForCharacterAtIndex:alternatePositions:inDisplayOrder:positions:characterIndexes:")]
		//nuint GetLineFragmentInsertionPoints (nuint charIndex, bool aFlag, bool dFlag, nfloat positions, ref nuint[] charIndexes);

		[Export ("temporaryAttributesAtCharacterIndex:effectiveRange:")]
		NSDictionary TemporaryAttributes (nuint charIndex, NSRangePointer effectiveCharRange);

		[Export ("setTemporaryAttributes:forCharacterRange:")]
		void SetTemporaryAttributes (NSDictionary attrs, NSRange charRange);

		[Export ("addTemporaryAttributes:forCharacterRange:")]
		void AddTemporaryAttributes (NSDictionary attrs, NSRange charRange);

		[Export ("removeTemporaryAttribute:forCharacterRange:")]
		void RemoveTemporaryAttribute (string attrName, NSRange charRange);

		// FIXME: binding
		//[Export ("temporaryAttribute:atCharacterIndex:effectiveRange:")]
		//NSObject TemporaryAttributeatCharacterIndexeffectiveRange (string attrName, nuint location, NSRangePointer range);
		//
		//[Export ("temporaryAttribute:atCharacterIndex:longestEffectiveRange:inRange:")]
		//NSObject TemporaryAttributeatCharacterIndexlongestEffectiveRangeinRange (string attrName, nuint location, NSRangePointer range, NSRange rangeLimit);

		[Export ("temporaryAttributesAtCharacterIndex:longestEffectiveRange:inRange:")]
		NSDictionary TemporaryAttributes (nuint location, NSRangePointer range, NSRange rangeLimit);

		[Export ("addTemporaryAttribute:value:forCharacterRange:")]
		void AddTemporaryAttribute (string attrName, NSObject value, NSRange charRange);

		[Export ("substituteFontForFont:")]
		NSFont SubstituteFontForFont (NSFont originalFont);

		[Export ("defaultLineHeightForFont:")]
		nfloat DefaultLineHeightForFont (NSFont theFont);

		[Export ("defaultBaselineOffsetForFont:")]
		nfloat DefaultBaselineOffsetForFont (NSFont theFont);

		//Detected properties
		[Export ("textStorage")]
		NSTextStorage TextStorage { get; set; }

		[Export ("glyphGenerator")]
		NSGlyphGenerator GlyphGenerator { get; set; }

		[Export ("typesetter")]
		NSTypesetter Typesetter { get; set; }

		[Export ("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		NSLayoutManagerDelegate Delegate { get; set; }

		[Export ("backgroundLayoutEnabled")]
		bool BackgroundLayoutEnabled { get; set; }

		[Export ("usesScreenFonts")]
		bool UsesScreenFonts { get; set; }

		[Export ("showsInvisibleCharacters")]
		bool ShowsInvisibleCharacters { get; set; }

		[Export ("showsControlCharacters")]
		bool ShowsControlCharacters { get; set; }

		[Export ("hyphenationFactor")]
		float HyphenationFactor { get; set; } // 32-bit

		[Export ("defaultAttachmentScaling")]
		NSImageScaling DefaultAttachmentScaling { get; set; }

		[Export ("typesetterBehavior")]
		NSTypesetterBehavior TypesetterBehavior { get; set; }

		[Export ("allowsNonContiguousLayout")]
		bool AllowsNonContiguousLayout { get; set; }

		[Export ("usesFontLeading")]
		bool UsesFontLeading { get; set; }

	}
