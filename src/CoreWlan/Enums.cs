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
    public enum CWErr
    {
        NoErr = 0,
        EAPOLErr = 1,
        InvalidParameterErr = -3900,
        NoMemoryErr = -3901,
        UnknownErr = -3902,
        NotSupportedErr = -3903,
        InvalidFormatErr = -3904,
        TimeoutErr = -3905,
        UnspecifiedFailureErr = -3906,
        UnsupportedCapabilitiesErr = -3907,
        ReassociationDeniedErr = -3908,
        AssociationDeniedErr = -3909,
        AuthenticationAlgorithmUnsupportedErr = -3910,
        InvalidAuthenticationSequenceNumberErr = -3911,
        ChallengeFailureErr = -3912,
        APFullErr = -3913,
        UnsupportedRateSetErr = -3914,
        ShortSlotUnsupportedErr = -3915,
        DSSSOFDMUnsupportedErr = -3916,
        InvalidInformationElementErr = -3917,
        InvalidGroupCipherErr = -3918,
        InvalidPairwiseCipherErr = -3919,
        InvalidAKMPErr = -3920,
        UnsupportedRSNVersionErr = -3921,
        InvalidRSNCapabilitiesErr = -3922,
        CipherSuiteRejectedErr = -3923,
        InvalidPMKErr = -3924,
        SupplicantTimeoutErr = -3925,
        HTFeaturesNotSupportedErr = -3926,
        PCOTransitionTimeNotSupportedErr = -3927,
        ReferenceNotBoundErr = -3928,
        IPCFailureErr = -3929,
        OperationNotPermittedErr = -3930,
        Err = -3931,
    };

    public enum CWPHYMode
    {
        PHYModeNone = 0,
        PHYMode11a = 1,
        PHYMode11b = 2,
        PHYMode11g = 3,
        PHYMode11n = 4,
        PHYMode11ac = 5,
    };

    public enum CWInterfaceMode
    {
        InterfaceModeNone = 0,
        InterfaceModeStation = 1,
        InterfaceModeIBSS = 2,
        InterfaceModeHostAP = 3,
    };

    public enum CWSecurity
    {
        SecurityNone = 0,
        SecurityWEP = 1,
        SecurityWPAPersonal = 2,
        SecurityWPAPersonalMixed = 3,
        SecurityWPA2Personal = 4,
        SecurityPersonal = 5,
        SecurityDynamicWEP = 6,
        SecurityWPAEnterprise = 7,
        SecurityWPAEnterpriseMixed = 8,
        SecurityWPA2Enterprise = 9,
        SecurityEnterprise = 10,
        SecurityUnknown = int.MaxValue,
    };

    public enum CWIBSSModeSecurity
    {
        IBSSModeSecurityNone = 0,
        IBSSModeSecurityWEP40 = 1,
        IBSSModeSecurityWEP104 = 2,
    };

    public enum CWChannelWidth
    {
        ChannelWidthUnknown = 0,
        ChannelWidth20MHz = 1,
        ChannelWidth40MHz = 2,
        ChannelWidth80MHz = 3,
        ChannelWidth160MHz = 4,
    };

    public enum CWChannelBand
    {
        ChannelBandUnknown = 0,
        ChannelBand2GHz = 1,
        ChannelBand5GHz = 2,
    };

    public enum CWCipherKeyFlags : ulong
    {
        CipherKeyFlagsNone = 0,
        CipherKeyFlagsUnicast = (1UL << 1),
        CipherKeyFlagsMulticast = (1UL << 2),
        CipherKeyFlagsTx = (1UL << 3),
        CipherKeyFlagsRx = (1UL << 4),
    };

    public enum CWKeychainDomain
    {
        KeychainDomainNone = 0,
        KeychainDomainUser = 1,
        KeychainDomainSystem = 2,
    };
}

