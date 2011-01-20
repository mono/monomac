// corewlan.cs: bindings for CoreWLAN
//
// Author:
//   Ashok Gelal
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
using MonoMac.CoreFoundation;
using System;

namespace MonoMac.CoreWlan {

	[BaseType (typeof (NSObject))]
	interface CW8021XProfile {
		[Export ("userDefinedName")]
		string UserDefinedName { get; set;  }

		[Export ("ssid")]
		string Ssid { get; set;  }

		[Export ("username")]
		string Username { get; set;  }

		[Export ("password")]
		string Password { get; set;  }

		[Export ("alwaysPromptForPassword")]
		bool AlwaysPromptForPassword{ get; set;  }

		[Static]
		[Export ("profile")]
		CW8021XProfile Profile { get; }

		[Export ("isEqualToProfile:")]
		bool IsEqualToProfile (CW8021XProfile profile);

		[Static]
		[Export ("allUser8021XProfiles")]
		CW8021XProfile[] AllUser8021XProfiles { get; }
	}

	[BaseType (typeof (NSObject))]
	interface CWConfiguration {
		[Export ("rememberedNetworks")]
		NSSet RememberedNetworks { get; set;  }

		[Export ("preferredNetworks")]
		CWWirelessProfile[] PreferredNetworks { get; set;  }

		[Export ("alwaysRememberNetworks")]
		bool AlwaysRememberNetworks{ get; set;  }

		[Export ("disconnectOnLogout")]
		bool DisconnectOnLogout{ get; set;  }

		[Export ("requireAdminForNetworkChange")]
		bool RequireAdminForNetworkChange { get; set;  }

		[Export ("requireAdminForPowerChange")]
		bool RequireAdminForPowerChange { get; set;  }

		[Export ("requireAdminForIBSSCreation")]
		bool RequireAdminForIBSSCreation { get; set;  }

		[Export ("isEqualToConfiguration:")]
		bool IsEqualToConfiguration (CWConfiguration configuration);
	}

	[BaseType (typeof (NSObject))]
	interface CWInterface {
		[Export ("supportsWoW")]
		bool SupportsWow { get;  }

		[Export ("supportsWEP")]
		bool SupportsWep { get;  }

		[Export ("supportsAES_CCM")]
		bool SupportsAesCcm { get;  }

		[Export ("supportsIBSS")]
		bool SupportsIbss { get;  }

		[Export ("supportsTKIP")]
		bool SupportsTkip { get;  }

		[Export ("supportsPMGT")]
		bool SupportsPmgt { get;  }

		[Export ("supportsHostAP")]
		bool SupportsHostAP { get;  }

		[Export ("supportsMonitorMode")]
		bool SupportsMonitorMode { get;  }

		[Export ("supportsWPA")]
		bool SupportsWpa { get;  }

		[Export ("supportsWPA2")]
		bool SupportsWpa2 { get;  }

		[Export ("supportsWME")]
		bool SupportsWme { get;  }

		[Export ("supportsShortGI40MHz")]
		bool SupportsShortGI40MHz { get;  }

		[Export ("supportsShortGI20MHz")]
		bool SupportsShortGI20MHz { get;  }

		[Export ("supportsTSN")]
		bool SupportsTsn { get;  }

		[Export ("power")]
		bool Power { get;  }

		[Export ("powerSave")]
		bool PowerSave { get;  }

		[Export ("name")]
		string Name { get;  }

		[Export ("supportedChannels")]
		NSNumber[] SupportedChannels { get;  }

		[Export ("supportedPHYModes")]
		NSNumber[] SupportedPhyModes { get;  }

		[Export ("channel")]
		NSNumber Channel { get;  }

		[Export ("phyMode")]
		NSNumber PhyMode { get;  }

		[Export ("ssid")]
		string Ssid { get;  }

		[Export ("bssid")]
		string Bssid { get;  }

		[Export ("bssidData")]
		NSData BssidData { get;  }

		[Export ("rssi")]
		NSNumber Rssi { get;  }

		[Export ("noise")]
		NSNumber Noise { get;  }

		[Export ("txRate")]
		NSNumber TxRate { get;  }

		[Export ("securityMode")]
		NSNumber SecurityMode { get;  }

		[Export ("interfaceState")]
		NSNumber InterfaceState { get;  }

		[Export ("countryCode")]
		string CountryCode { get;  }

		[Export ("opMode")]
		NSNumber OpMode { get;  }

		[Export ("txPower")]
		NSNumber TxPower { get;  }

		[Export ("configuration")]
		CWConfiguration Configuration { get;  }

		[Static]
		[Export ("supportedInterfaces")]
		string[] SupportedInterfaces { get; }

		[Static]
		[Export ("interface")]
		CWInterface MainInterface { get; }

		[Static]
		[Export ("interfaceWithName:")]
		CWInterface FromName (string name);

		[Export ("initWithInterfaceName:")]
		IntPtr Constructor (string name);

		[Export ("isEqualToInterface:")]
		bool IsEqualToInterface (CWInterface intface);

		[Export ("setPower:error:")]
		bool SetPower (bool power, out NSError error);

		[Export ("setChannel:error:")]
		bool SetChannel (uint channel, out NSError error);

		[Export ("scanForNetworksWithParameters:error:")]
		CWNetwork[] ScanForNetworksWithParameters([NullAllowed] NSDictionary parameters, out NSError error);

		[Export ("associateToNetwork:parameters:error:")]
		bool AssociateToNetwork(CWNetwork network, [NullAllowed] NSDictionary parameters, out NSError error);

		[Export ("disassociate")]
		void Disassociate ();

		[Export ("enableIBSSWithParameters:error:")]
		bool EnableIBSSWithParameters([NullAllowed] NSDictionary parameters, out NSError error);

		[Export ("commitConfiguration:error:")]
		bool CommitConfiguration (CWConfiguration config, out NSError error);

	}

	[BaseType (typeof (NSObject))]
	interface CWWirelessProfile {
		[Export ("ssid")]
		string Ssid { get; set;  }

		[Export ("securityMode")]
		NSNumber SecurityMode { get; set;  }

		[Export ("passphrase")]
		string Passphrase { get; set;  }

		[Export ("user8021XProfile")]
		CW8021XProfile User8021XProfile { get; set;  }

		[Export ("isEqualToProfile:")]
		bool IsEqualToProfile (CWWirelessProfile profile);
	}

	[BaseType (typeof (NSObject))]
	interface CWNetwork {
		[Export ("ssid")]
		string Ssid { get;  }

		[Export ("bssid")]
		string Bssid { get;  }

		[Export ("bssidData")]
		NSData BssidData { get;  }

		[Export ("securityMode")]
		NSNumber SecurityMode { get;  }

		[Export ("phyMode")]
		NSNumber PhyMode { get;  }

		[Export ("channel")]
		NSNumber Channel { get;  }

		[Export ("rssi")]
		NSNumber Rssi { get;  }

		[Export ("noise")]
		NSNumber Noise { get;  }

		[Export ("ieData")]
		NSData IeData { get;  }

		[Export ("isIBSS")]
		bool IsIBSS { get;  }

		[Export ("wirelessProfile")]
		CWWirelessProfile WirelessProfile { get;  }

		[Export ("isEqualToNetwork:")]
		bool IsEqualToNetwork (CWNetwork network);
	}

}

