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
        // Old APIs
        [Export("userDefinedName")]
        string UserDefinedName { get; set; }

        [Export("ssid")]
        string Ssid { get; set; }

        [Export("username")]
        string Username { get; set; }

        [Export("password")]
        string Password { get; set; }

        [Export("alwaysPromptForPassword")]
        bool AlwaysPromptForPassword{ get; set; }

        [Static]
        [Export("profile")]
        CW8021XProfile Profile { get; }

        [Export("isEqualToProfile:")]
        bool IsEqualToProfile(CW8021XProfile profile);

        [Static]
        [Export("allUser8021XProfiles")]
        CW8021XProfile[] AllUser8021XProfiles { get; }
    }

    [BaseType(typeof(NSObject))]
    interface CWChannel
    {
        // New APIs
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
        // New APIs
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
        // Old APIs
        [Export("rememberedNetworks")]
        NSSet RememberedNetworks { get; set; }

        [Export("preferredNetworks")]
        CWWirelessProfile[] PreferredNetworks { get; set; }

        [Export("alwaysRememberNetworks")]
        bool AlwaysRememberNetworks{ get; set; }

        [Export("disconnectOnLogout")]
        bool DisconnectOnLogout{ get; set; }

        [Export("requireAdminForNetworkChange")]
        bool RequireAdminForNetworkChange { get; set; }

        [Export("requireAdminForPowerChange")]
        bool RequireAdminForPowerChange { get; set; }

        [Export("requireAdminForIBSSCreation")]
        bool RequireAdminForIBSSCreation { get; set; }

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
        // New APIs
        [Export("supportsWoW")]
        bool SupportsWow { get; }

        [Export("supportsWEP")]
        bool SupportsWep { get; }

        [Export("supportsAES_CCM")]
        bool SupportsAesCcm { get; }

        [Export("supportsIBSS")]
        bool SupportsIbss { get; }

        [Export("supportsTKIP")]
        bool SupportsTkip { get; }

        [Export("supportsPMGT")]
        bool SupportsPmgt { get; }

        [Export("supportsHostAP")]
        bool SupportsHostAP { get; }

        [Export("supportsMonitorMode")]
        bool SupportsMonitorMode { get; }

        [Export("supportsWPA")]
        bool SupportsWpa { get; }

        [Export("supportsWPA2")]
        bool SupportsWpa2 { get; }

        [Export("supportsWME")]
        bool SupportsWme { get; }

        [Export("supportsShortGI40MHz")]
        bool SupportsShortGI40MHz { get; }

        [Export("supportsShortGI20MHz")]
        bool SupportsShortGI20MHz { get; }

        [Export("supportsTSN")]
        bool SupportsTsn { get; }

        [Export("power")]
        bool Power { get; }

        [Export("powerSave")]
        bool PowerSave { get; }

        [Export("name")]
        string Name { get; }

        [Export("supportedChannels")]
        NSNumber[] SupportedChannels { get; }

        [Export("supportedPHYModes")]
        NSNumber[] SupportedPhyModes { get; }

        [Export("channel")]
        NSNumber Channel { get; }

        [Export("phyMode")]
        NSNumber PhyMode { get; }

        [Export("bssidData")]
        NSData BssidData { get; }

        [Export("rssi")]
        NSNumber Rssi { get; }

        [Export("noise")]
        NSNumber Noise { get; }

        [Export("txRate")]
        NSNumber TxRate { get; }

        [Export("securityMode")]
        NSNumber SecurityMode { get; }

        [Export("interfaceState")]
        NSNumber InterfaceState { get; }

        [Export("opMode")]
        NSNumber OpMode { get; }

        [Export("txPower")]
        NSNumber TxPower { get; }

        [Static]
        [Export("supportedInterfaces")]
        string[] SupportedInterfaces { get; }

        [Static]
        [Export("interface")]
        CWInterface MainInterface { get; }

        [Static]
        [Export("interfaceWithName:")]
        CWInterface FromName(string name);

        [Export("initWithInterfaceName:")]
        IntPtr Constructor(string name);

        [Export("isEqualToInterface:")]
        bool IsEqualToInterface(CWInterface intface);

        [Export("setPower:error:")]
        bool SetPower(bool power, out NSError error);

        [Export("setChannel:error:")]
        bool SetChannel(uint channel, out NSError error);

        [Export("scanForNetworksWithParameters:error:")]
        CWNetwork[] ScanForNetworksWithParameters([NullAllowed] NSDictionary parameters, out NSError error);

        [Export("associateToNetwork:parameters:error:")]
        bool AssociateToNetwork(CWNetwork network, [NullAllowed] NSDictionary parameters, out NSError error);

        [Export("enableIBSSWithParameters:error:")]
        bool EnableIBSSWithParameters([NullAllowed] NSDictionary parameters, out NSError error);

        [Export("commitConfiguration:error:")]
        bool CommitConfiguration(CWConfiguration config, out NSError error);
        // Old APIs
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
        // New APIs
        [Export("bssidData")]
        NSData BssidData { get; }

        [Export("securityMode")]
        NSNumber SecurityMode { get; }

        [Export("phyMode")]
        NSNumber PhyMode { get; }

        [Export("channel")]
        NSNumber Channel { get; }

        [Export("rssi")]
        NSNumber Rssi { get; }

        [Export("noise")]
        NSNumber Noise { get; }

        [Export("ieData")]
        NSData IeData { get; }

        [Export("isIBSS")]
        bool IsIBSS { get; }

        [Export("wirelessProfile")]
        CWWirelessProfile WirelessProfile { get; }

        // Old APIs
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
        // Old APIs
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

