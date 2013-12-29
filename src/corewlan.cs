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

namespace MonoMac.CoreWlan
{
    [BaseType(typeof(NSObject))]
    interface CW8021XProfile
    {
    }

    [BaseType(typeof(NSObject))]
    interface CWChannel
    {
        [Export("channelNumber")]
        uint ChannelNumber { [Bind("channelNumber")] get; }

        [Export("channelWidth")]
        CWChannelWidth ChannelWidth { [Bind("channelWidth")] get; }

        [Export("channelBand")]
        CWChannelBand ChannelBand { [Bind("channelBand")] get; }

        [Export("isEqualToChannel:")]
        bool IsEqualToChannel(CWChannel channel);
    }

    [BaseType(typeof(NSObject))]
    interface CWConfiguration
    {
        [Export("networkProfiles")]
        NSOrderedSet NetworkProfiles { [Bind("networkProfiles")] get; }

        [Export("requireAdministratorForAssociation")]
        bool RequireAdministratorForAssociation { [Bind ("requireAdministratorForAssociation")] get; }

        [Export("requireAdministratorForPower")]
        bool RequireAdministratorForPower { [Bind ("requireAdministratorForPower")] get; }

        [Export("requireAdministratorForIBSSMode")]
        bool RequireAdministratorForIBSSMode { [Bind ("requireAdministratorForIBSSMode")] get; }

        [Export("rememberJoinedNetworks")]
        bool RememberJoinedNetworks { [Bind ("rememberJoinedNetworks")] get; }

        [Static]
        [Export("configuration")]
        NSObject Configuration();

        [Export("init")]
        NSObject Init();

        [Export("initWithConfiguration:")]
        NSObject InitWithConfiguration(CWConfiguration configuration);

        [Static]
        [Export("configurationWithConfiguration:")]
        NSObject ConfigurationWithConfiguration(CWConfiguration configuration);

        [Export("isEqualToConfiguration:")]
        bool IsEqualToConfiguration(CWConfiguration configuration);
    }

    [BaseType(typeof(CWConfiguration))]
    interface CWMutableConfiguration
    {
        [Export("requireAdministratorForPower")]
        bool RequireAdministratorForPower { get; set; }

        [Export("requireAdministratorForIBSSMode")]
        bool RequireAdministratorForIBSSMode { get; set; }

        [Export("rememberJoinedNetworks")]
        bool RememberJoinedNetworks { get; set; }
    }

    [BaseType(typeof(NSObject))]
    interface CWInterface
    {
        [Export("powerOn")]
        bool PowerOn { [Bind ("powerOn")] get; }

        [Export("interfaceName")]
        string InterfaceName { [Bind ("interfaceName")] get; }

        [Export("supportedWLANChannels")]
        NSSet SupportedWLANChannels { [Bind ("supportedWLANChannels")] get; }

        [Export("wlanChannel")]
        CWChannel WlanChannel { [Bind ("wlanChannel")] get; }

        [Export("activePHYMode")]
        CWPHYMode ActivePHYMode { [Bind ("activePHYMode")] get; }

        [Export("ssid")]
        string Ssid { [Bind ("ssid")] get; }

        [Export("ssidData")]
        NSData SsidData { [Bind ("ssidData")] get; }

        [Export("bssid")]
        string Bssid { [Bind ("bssid")] get; }

        [Export("rssiValue")]
        int RssiValue { [Bind ("rssiValue")] get; }

        [Export("noiseMeasurement")]
        int NoiseMeasurement { [Bind ("noiseMeasurement")] get; }

        [Export("security")]
        CWSecurity Security { [Bind ("security")] get; }

        [Export("transmitRate")]
        double TransmitRate { [Bind ("transmitRate")] get; }

        [Export("countryCode")]
        string CountryCode { [Bind ("countryCode")] get; }

        [Export("interfaceMode")]
        CWInterfaceMode InterfaceMode { [Bind ("interfaceMode")] get; }

        [Export("transmitPower")]
        uint TransmitPower { [Bind ("transmitPower")] get; }

        [Export("hardwareAddress")]
        string HardwareAddress { [Bind ("hardwareAddress")] get; }

        [Export("deviceAttached")]
        bool DeviceAttached { [Bind ("deviceAttached")] get; }

        [Export("serviceActive")]
        bool ServiceActive { [Bind ("serviceActive")] get; }

        [Export("cachedScanResults")]
        NSSet CachedScanResults { [Bind ("cachedScanResults")] get; }

        [Export("configuration")]
        CWConfiguration Configuration { [Bind ("configuration")] get; }

        [Static]
        [Export("interfaceNames")]
        NSSet InterfaceNames();

        [Static]
        [Export("interface")]
        CWInterface Interface();

        [Static]
        [Export("interfaceWithName:")]
        CWInterface InterfaceWithName(string name);

        [Export("initWithInterfaceName:")]
        NSObject InitWithInterfaceName(string name);

        [Export("setPower:error:")]
        bool SetPowererror(bool power, out NSError error);

        [Export("setWLANChannel:error:")]
        bool SetWLANChannelerror(CWChannel channel, out NSError error);

        [Export("setPairwiseMasterKey:error:")]
        bool SetPairwiseMasterKey(NSData key, out NSError error);

        [Export("setWEPKey:flags:index:error:")]
        bool SetWEPKey(NSData key, CWCipherKeyFlags flags, uint index, out NSError error);

        [Export("scanForNetworksWithSSID:error:")]
        NSSet ScanForNetworksWithSSID([NullAllowed] NSData ssid, out NSError error);

        [Export("scanForNetworksWithName:error:")]
        NSSet ScanForNetworksWithName([NullAllowed] string networkName, out NSError error);

        [Export("associateToNetwork:password:error:")]
        bool AssociateToNetwork(CWNetwork network, string password, out NSError error);

        [Export("associateToEnterpriseNetwork:identity:username:password:error:")]
        bool AssociateToEnterpriseNetwork(CWNetwork network, NSObject identity, string username, string password, out NSError error);

        [Export("startIBSSModeWithSSID:security:channel:password:error:")]
        bool StartIBSSModeWithSSID(NSData ssidData, CWIBSSModeSecurity security, uint channel, string password, out NSError error);

        [Export("disassociate")]
        void Disassociate();

        [Export("commitConfiguration:authorization:error:")]
        bool CommitConfiguration(CWConfiguration configuration, NSObject authorization, out NSError error);
    }

    [BaseType(typeof(NSObject))]
    interface CWNetwork
    {
        [Export("ssid")]
        string Ssid { [Bind ("ssid")] get; }

        [Export("ssidData")]
        NSData SsidData { [Bind ("ssidData")] get; }

        [Export("bssid")]
        string Bssid { [Bind ("bssid")] get; }

        [Export("wlanChannel")]
        CWChannel WlanChannel { [Bind ("wlanChannel")] get; }

        [Export("rssiValue")]
        int RssiValue { [Bind ("rssiValue")] get; }

        [Export("noiseMeasurement")]
        int NoiseMeasurement { [Bind ("noiseMeasurement")] get; }

        [Export("informationElementData")]
        NSData InformationElementData { [Bind ("informationElementData")] get; }

        [Export("countryCode")]
        string CountryCode { [Bind ("countryCode")] get; }

        [Export("beaconInterval")]
        uint BeaconInterval { [Bind ("beaconInterval")] get; }

        [Export("ibss")]
        bool Ibss { [Bind ("ibss")] get; }

        [Export("isEqualToNetwork:")]
        bool IsEqualToNetwork(CWNetwork network);

        [Export("supportsSecurity:")]
        bool SupportsSecurity(CWSecurity security);

        [Export("supportsPHYMode:")]
        bool SupportsPHYMode(CWPHYMode phyMode);
    }

    [BaseType(typeof(NSObject))]
    interface CWNetworkProfile
    {
        [Export("ssid")]
        string Ssid { [Bind ("ssid")] get; }

        [Export("ssidData")]
        NSData SsidData { [Bind ("ssidData")] get; }

        [Export("security")]
        CWSecurity Security { [Bind ("security")] get; }

        [Static]
        [Export("networkProfile")]
        NSObject NetworkProfile();

        [Export("init")]
        NSObject Init();

        [Export("initWithNetworkProfile:")]
        NSObject InitWithNetworkProfile(CWNetworkProfile networkProfile);

        [Static]
        [Export("networkProfileWithNetworkProfile:")]
        NSObject NetworkProfileWithNetworkProfile(CWNetworkProfile networkProfile);

        [Export("isEqualToNetworkProfile:")]
        bool IsEqualToNetworkProfile(CWNetworkProfile networkProfile);
    }

    [BaseType(typeof(CWNetworkProfile))]
    interface CWMutableNetworkProfile
    {
    }

    [BaseType(typeof(NSObject))]
    interface CWWirelessProfile
    {
        [Export("ssid")]
        string Ssid { get; set; }

        [Export("securityMode")]
        NSNumber SecurityMode { get; set; }

        [Export("passphrase")]
        string Passphrase { get; set; }

        [Export("user8021XProfile")]
        CW8021XProfile User8021XProfile { get; set; }

        [Export("init")]
        CWWirelessProfile Init();

        [Static]
        [Export("profile")]
        CWWirelessProfile Profile();

        [Export("isEqualToProfile:")]
        bool IsEqualToProfile(CWWirelessProfile profile);
    }
}

