using System;

namespace macdoc
{
	public static class AppleDocKnownIssues
	{
		public static bool IsKnown (string type, string selector)
		{
			// generator always generates this constructor from the NSCoding protocol
			if (selector == "initWithCoder:")
				return true;
			switch (type){
			case "ABUnknownPersonViewControllerDelegate":
				switch (selector) {
					// Documented online, but not in the SDK docs.
					case "unknownPersonViewController:shouldPerformDefaultActionForPerson:property:identifier:":
						return true;
				}
				break;
			case "AVAudioSession":
				switch (selector) {
					// Documented online, but not in the SDK docs.
					case "delegate":
						return true;
				}
				break;
			case "CALayer":
				if (CAMediaTiming_Selector (selector))
					return true;
				switch (selector) {
				// renamed property getter; TODO: make it grab the 'doubleSided' property docs
				case "isDoubleSided":
				// renamed property getter; TODO: make it grab the 'geometryFlipped' property docs
				case "isGeometryFlipped":
				// renamed property getter; TODO: make it grab the 'hidden' property docs
				case "isHidden":
				// renamed property getter; TODO: make it grab the 'opaque' property docs
				case "isOpaque":
					return true;
				}
				break;
			case "CAPropertyAnimation":
				switch (selector) {
				case "isAdditive":    // TODO: additive property
				case "isCumulative":  // TODO: cumulative property
					return true;
				}
				break;
			case "MPMoviePlayerController":
				switch (selector) {
					// Documentation online, but not locally
					case "play":
					case "stop":
						return true;
				}
				break;
			case "NSBundle":
				switch (selector) {
				// Extension method from UIKit
				case "loadNibNamed:owner:options:":
	
				// Extension methods from AppKit 
				case "pathForImageResource:":
				case "pathForSoundResource:":
					return true;
				}
				break;
			case "NSIndexPath":
				// Documented in a separate file, .../NSIndexPath_UIKitAdditions/Reference/Reference.html
				switch (selector) {
				case "indexPathForRow:inSection:":
				case "row":
				case "section":
					return true;
				}
				break;
			case "UIDevice":
				// see TODO wrt Property handling.
				if (selector == "isGeneratingDeviceOrientationNotifications")
					return true;
				break;
			case "NSMutableUrlRequest":
				switch (selector) {
				// NSMutableUrlRequest provides setURL, but URL is provided from the
				// base NSUrlRequest method.  This is a "wart" in our binding to make
				// for nicer code.
				case "URL":
				case "cachePolicy":
				case "timeoutInterval":
				case "mainDocumentURL":
				case "HTTPMethod":
				case "allHTTPHeaderFields":
				case "HTTPBody":
				case "HTTPBodyStream":
				case "HTTPShouldHandleCookies":
					return true;
				}
				break;
			case "NSUrlConnection":
				switch (selector) {
				// NSURLConnection adopts the NSUrlAuthenticationChallengeSender, but
				// docs don't mention it
				case "useCredential:forAuthenticationChallenge:":
				case "continueWithoutCredentialForAuthenticationChallenge:":
				case "cancelAuthenticationChallenge:":
					return true;
				}
				break;
			case "NSUserDefaults":
				switch (selector) {
				// Documentation online, but not locally
				case "doubleForKey:":
				case "setDouble:forKey:":
					return true;
				}
				break;
			case "NSValue":
				switch (selector) {
				// extension methods from various places...
				case "valueWithCGPoint:":
				case "valueWithCGRect:":
				case "valueWithCGSize:":
				case "valueWithCGAffineTransform:":
				case "valueWithUIEdgeInsets:":
				case "CGPointValue":
				case "CGRectValue":
				case "CGSizeValue":
				case "CGAffineTransformValue":
				case "UIEdgeInsetsValue":
					return true;
				}
				break;
			case "SKPaymentQueue":
				switch (selector) {
				// Documented online, but not locally
				case "restoreCompletedTransactions":
					return true;
				}
				break;
			case "SKPaymentTransaction":
				switch (selector) {
				// Documented online, but not locally
				case "originalTransaction":
				case "transactionDate":
					return true;
				}
				break;
			case "SKPaymentTransactionObserver":
				switch (selector) {
				// Documented online, but not locally
				case "paymentQueue:restoreCompletedTransactionsFailedWithError:":
				case "paymentQueueRestoreCompletedTransactionsFinished:":
					return true;
				}
				break;
			case "UIApplication":
				switch (selector) {
				// deprecated in iPhoneOS 3.2
				case "setStatusBarHidden:animated:":
					return true;
				}
				break;
			case "UITableViewDelegate":
				// This is documented on a separate HTML file, deprecated file
				if (selector == "tableView:accessoryTypeForRowWithIndexPath:")
					return true;
				break;
	
			case "UISearchBar":
				// Online, but not local
				if (selector == "isTranslucent")
					return true;
				break;
			case "UISearchBarDelegate":
				// This was added to the 3.0 API, but was not documented.
				if (selector == "searchBar:shouldChangeTextInRange:replacementText:")
					return true;
				break;
			case "UITextField":
			case "UITextView":
				// These are from the UITextInputTraits protocol
				switch (selector) {
				case "autocapitalizationType":
				case "autocorrectionType":
				case "keyboardType":
				case "keyboardAppearance":
				case "returnKeyType":
				case "enablesReturnKeyAutomatically":
				case "isSecureTextEntry":
					return true;
				}
				break;
			case "UIImagePickerController":
				switch (selector) {
				// present online, but not locally
				case "allowsEditing":
				case "cameraOverlayView":
				case "cameraViewTransform":
				case "showsCameraControls":
				case "takePicture":
				case "videoMaximumDuration":
				case "videoQuality":
					return true;
				}
				break;
			case "UIImagePickerControllerDelegate":
				// Deprecated, but available
				if (selector == "imagePickerController:didFinishPickingImage:editingInfo:")
					return true;
				// Deprecated in iPhone 3.0
				if (selector == "imagePickerController:didFinishPickingMediaWithInfo:")
					return true;
				break;
			case "UIViewController":
				switch (selector) {
				// present online, but not locally
				case "searchDisplayController":
					return true;
				}
				break;
	
			case "CLLocation":
				switch (selector) {
				// deprecated and moved into CLLocation_Class/DeprecationAppendix/AppendixADeprecatedAPI.html
				case "getDistanceFrom:":
					return true;
				}
				break;
			case "CLLocationManagerDelegate":
				// Added in 3.0, Not yet documented by apple
				if (selector == "locationManager:didUpdateHeading:")
					return true;
	
				// Added in 3.0, Not yet documented by apple
				if (selector == "locationManagerShouldDisplayHeadingCalibration:")
					return true;
				break;
				
			case "CLLocationManager":
				// Added in 3.0, but not documented 
				if (selector == "startUpdatingHeading" || selector == "stopUpdatingHeading" || selector == "dismissHeadingCalibrationDisplay")
					return true;
				// present online, but not on local disk?
				if (selector == "headingFilter" || selector == "headingAvailable")
					return true;
				break;
				
			case "CAAnimation":
				// Not in the docs.
				if (selector == "willChangeValueForKey:" || selector == "didChangeValueForKey:")
					return true;
				if (CAMediaTiming_Selector (selector))
					return true;
				break;
	
			case "NSObject":
				// Defined in the NSObject_UIKitAdditions/Introduction/Introduction.html instead of NSObject.html
				if (selector == "awakeFromNib")
					return true;
				// Kind of a hack; NSObject doesn't officially implement NSCoding, but
				// most types do, and it's a NOP on NSObject, so it's easier for
				// MonoTouch users if MonoTouch.Foundation.NSObject provides the method.
				if (selector == "encodeWithCoder:")
					return true;
				break;
			}
			return false;
		}
	
		static bool CAMediaTiming_Selector (string selector)
		{
			switch (selector) {
				case "autoreverses":
				case "beginTime":
				case "duration":
				case "fillMode":
				case "repeatCount":
				case "repeatDuration":
				case "speed":
				case "timeOffset":
					return true;
			}
			return false;
		}
	
		public static bool IsKnownMissingReturnValue (Type type, string selector)
		{
			switch (type.Name) {
			case "AVAudioSession":
				switch (selector) {
				case "sharedInstance":
					return true;
				}
				break;
			case "GKPeerPickerControllerDelegate":
				switch (selector) {
				case "peerPickerController:sessionForConnectionType:":
					return true;
				}
				break;
			case "MPMediaItemArtwork":
				switch (selector) {
				case "imageWithSize:":
					return true;
				}
				break;
			case "NSCoder":
				switch (selector) {
				case "containsValueForKey:":
				case "decodeBoolForKey:":
				case "decodeDoubleForKey:":
				case "decodeFloatForKey:":
				case "decodeInt32ForKey:":
				case "decodeInt64ForKey:":
				case "decodeObject":
				case "decodeObjectForKey:":
					return true;
				}
				break;
			case "NSDecimalNumber":
				switch (selector) {
				case "decimalNumberByAdding:withBehavior:":
				case "decimalNumberBySubtracting:withBehavior:":
				case "decimalNumberByMultiplyingBy:withBehavior:":
				case "decimalNumberByDividingBy:withBehavior:":
				case "decimalNumberByRaisingToPower:withBehavior:":
				case "decimalNumberByMultiplyingByPowerOf10:":
				case "decimalNumberByMultiplyingByPowerOf10:withBehavior:":
				case "decimalNumberByRoundingAccordingToBehavior:":
					return true;
				}
				break;
			case "NSDictionary":
				switch (selector) {
				case "objectsForKeys:notFoundMarker:":
					return true;
				}
				break;
			case "NSNotification":
				switch (selector) {
				case "notificationWithName:object:":
				case "notificationWithName:object:userInfo:":
					return true;
				}
				break;
			case "NSUrlCredential":
				switch (selector) {
				case "credentialForTrust:":
					return true;
				}
				break;
				
			case "UIResponder":
				switch (selector) {
				case "resignFirstResponder":
					return true;
				}
				break;
				
			case "UIFont":
				switch (selector){
				case "leading":
					// This one was deprecated
					return true;
				}
				break;
				
			case "UIImagePickerController":
				switch (selector){
				case "allowsImageEditing":
					// This one was deprecated.
					return true;
				}
				break;
				
			}
			return false;
		}
	}
}

