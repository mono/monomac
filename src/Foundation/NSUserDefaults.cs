using System;

namespace MonoMac.Foundation {

	public enum NSUserDefaultsType {
		UserName,
		SuiteName
	}

	public partial class NSUserDefaults {

		public NSUserDefaults(string name) : this (name, NSUserDefaultsType.UserName)
		{
		}

		public NSUserDefaults(string name, NSUserDefaultsType type)
		{
			// two different `init*` would share the same C# signature
			switch (type) {
			case NSUserDefaultsType.UserName:
				Handle = InitWithUserName (name);
				break;
			case NSUserDefaultsType.SuiteName:
				Handle = InitWithSuiteName (name);
				break;
			default:
				throw new ArgumentException ("type");
			}
		}

        public void SetString (string value, string defaultName) {
			NSString str = new NSString (value);

			SetObjectForKey (str, defaultName);
			
			str.Dispose ();
		}

		public NSObject this [string key] {
			get {
				return ObjectForKey (key);
			}

			set {
				SetObjectForKey (value, key);
			}
		}
	}
	
}
