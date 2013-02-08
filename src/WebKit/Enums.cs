using System;

namespace MonoMac.WebKit {

	public enum DomCssRuleType : ushort {
		Unknown = 0,
		Style = 1,
		Charset = 2,
		Import = 3,
		Media = 4,
		FontFace = 5,
		Page = 6,
		Variables = 7,
		WebKitKeyFrames = 8,
		WebKitKeyFrame = 9
	}

	public enum DomCssValueType : ushort {
		Inherit = 0,
		PrimitiveValue = 1,
		ValueList = 2,
		Custom = 3
	}

	[Flags]
	public enum DomDocumentPosition : ushort {
		Disconnected = 0x01,
		Preceeding = 0x02,
		Following = 0x04,
		Contains = 0x08,
		ContainedBy = 0x10,
		ImplementationSpecific = 0x20
	}

	public enum DomNodeType : ushort {
		Element = 1,
		Attribute = 2,
		Text = 3,
		CData = 4,
		EntityReference = 5,
		Entity = 6,
		ProcessingInstruction = 7,
		Comment = 8,
		Document = 9,
		DocumentType = 10,
		DocumentFragment = 11,
		Notation = 12
	}

	public enum DomRangeCompareHow : ushort {
		StartToStart = 0, 
		StartToEnd = 1, 
		EndToEnd = 2, 
		EndToStart = 3
	}

	public enum WebCacheModel {
		DocumentViewer, DocumentBrowser, PrimaryWebBrowser
	}

	public enum DomEventPhase : ushort {
		Capturing = 1, AtTarget, Bubbling
	}

	[Flags]
	public enum WebDragSourceAction : uint {
		None = 0,
		DHTML = 1,
		Image = 2, 
		Link = 4,
		Selection = 8,
		Any = UInt32.MaxValue
	}

	[Flags]
	public enum WebDragDestinationAction : uint {
		None = 0,
		DHTML = 1,
		Image = 2, 
		Link = 4,
		Selection = 8,
		Any = UInt32.MaxValue
	}

	public enum WebNavigationType {
		LinkClicked, FormSubmitted, BackForward, Reload, FormResubmitted, Other
	}
}
