//
// coremidi.cs: API definition for CoreMidi's Foundation based types
//
// Authors:
//   Miguel de Icaza
//
// Copyright 2012 Xamarin Inc
//
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
using System;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.ObjCRuntime;

namespace MonoMac.CoreMidi {
	
#if !MONOMAC
	[BaseType (typeof (NSObject), Name="MIDINetworkHost")]
	interface MidiNetworkHost {
		[Export ("name")]
		string Name { get;  }

		[Export ("address")]
		string Address { get;  }

		[Export ("port")]
		int Port { get;  }

		[Export ("netServiceName")]
		string NetServiceName { get;  }

		[Export ("netServiceDomain")]
		string NetServiceDomain { get;  }

		[Static]
		[Export ("hostWithName:netService:")]
		MidiNetworkHost Create (string hostName, NSNetService netService);

		[Static]
		[Export ("hostWithName:netServiceName:netServiceDomain:")]
		MidiNetworkHost Create (string hostName, string netServiceName, string netServiceDomain);

		[Static]
		[Export ("hostWithName:address:port:")]
		MidiNetworkHost Create (string hostName, string address, int port);

		[Export ("hasSameAddressAs:")]
		bool HasSameAddressAs (MidiNetworkHost other);
	}

	[Static]
	interface Midi {
		[Field ("MIDINetworkNotificationContactsDidChange")]
		[Notification]
		NSString NetworkNotificationContactsDidChange { get; }

		[Field ("MIDINetworkNotificationSessionDidChange")]
		[Notification]
		NSString NetworkNotificationSessionDidChange { get; }

		[Field ("MIDINetworkBonjourServiceType")]
		NSString NetworkBonjourServiceType { get; }
	}

	[BaseType (typeof (NSObject), Name="MIDINetworkConnection")]
	interface MidiNetworkConnection {
		[Export ("host")]
		MidiNetworkHost Host { get;  }

		[Static, Export ("connectionWithHost:")]
		MidiNetworkConnection FromHost (MidiNetworkHost host);
	}

	[BaseType (typeof (NSObject), Name="MIDINetworkSession")]
	// default 'init' crash the application
	[DisableDefaultCtor]
	interface MidiNetworkSession {
		[Export ("enabled")]
		bool Enabled { [Bind ("isEnabled")] get; set;  }

		[Export ("networkPort")]
		int NetworkPort { get;  }

		[Export ("networkName")]
		string NetworkName { get;  }

		[Export ("localName")]
		string LocalName { get;  }

		[Export ("connectionPolicy")]
		MidiNetworkConnectionPolicy ConnectionPolicy { get; set;  }

		[Static]
		[Export ("defaultSession")]
		MidiNetworkSession DefaultSession { get; }

		[Export ("contacts")]
		NSSet Contacts { get; }

		[Export ("addContact:")]
		bool AddContact (MidiNetworkHost contact);

		[Export ("removeContact:")]
		bool RemoveContact (MidiNetworkHost contact);

		[Export ("connections")]
		NSSet Connections { get; }

		[Export ("addConnection:")]
		bool AddConnection (MidiNetworkConnection connection);

		[Export ("removeConnection:")]
		bool RemoveConnection (MidiNetworkConnection connection);

		[Export ("sourceEndpoint")]
		MidiEndpoint SourceEndpoint { get; }

		[Export ("destinationEndpoint")]
		MidiEndpoint DestinationEndpoint { get; }
	}
#endif
}