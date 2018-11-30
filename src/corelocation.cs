
// This file describes the API that the generator will produce
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//
// Copyright 2009, Novell, Inc.
// Copyright 2011, Xamarin, Inc.
//
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.CoreLocation;
#if !MONOMAC
using MonoMac.UIKit;
#endif
using System;

namespace MonoMac.CoreLocation {

#if !MONOMAC
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // will crash, see CoreLocation.cs for compatibility stubs
	partial interface CLHeading {
		[Export ("magneticHeading")]
		double MagneticHeading { get;  }
	
		[Export ("trueHeading")]
		double TrueHeading { get;  }
	
		[Export ("headingAccuracy")]
		double HeadingAccuracy { get;  }
	
		[Export ("x")]
		double X { get;  }
	
		[Export ("y")]
		double Y { get;  }
	
		[Export ("z")]
		double Z { get;  }
	
		[Export ("timestamp")]
		NSDate Timestamp { get;  }
	
		[Export ("description")]
		string Description ();
	
	}
#endif
	
	[BaseType (typeof (NSObject))]
	partial interface CLLocation {
		[Export ("coordinate")]
		CLLocationCoordinate2D Coordinate { get;  }
	
		[Export ("altitude")]
		double Altitude { get;  }
	
		[Export ("horizontalAccuracy")]
		double HorizontalAccuracy { get;  }
	
		[Export ("verticalAccuracy")]
		double VerticalAccuracy { get;  }
	
		[Export ("course")]
		double Course { get;  }
	
		[Export ("speed")]
		double Speed { get;  }
	
		[Export ("timestamp")]
		NSDate Timestamp { get;  }
	
		[Export ("initWithLatitude:longitude:")]
		IntPtr Constructor (double latitude, double longitude);
	
		[Export ("initWithCoordinate:altitude:horizontalAccuracy:verticalAccuracy:timestamp:")]
		IntPtr Constructor (CLLocationCoordinate2D coordinate, double altitude, double hAccuracy, double vAccuracy, NSDate timestamp);
	
		[Export ("description")]
		string Description ();
	
		[Export ("getDistanceFrom:")]
		[Obsolete ("Replaced by DistanceFrom")]
		double Distancefrom (CLLocation  location);

		// NOTE: The old selector was renamed to this guy in 3.2
		[Since (3,2)]
		[Export ("distanceFromLocation:")]
		double DistanceFrom (CLLocation location);

		[Since (4,2)]
		[Export ("initWithCoordinate:altitude:horizontalAccuracy:verticalAccuracy:course:speed:timestamp:")]
		IntPtr Constructor (CLLocationCoordinate2D coordinate, double altitude, double hAccuracy, double vAccuracy, double course, double speed, NSDate timestamp);

		[Since (5,0)][MountainLion]
		[Field ("kCLErrorUserInfoAlternateRegionKey")]
		NSString ErrorUserInfoAlternateRegionKey { get; }
	}
	
	[BaseType (typeof (NSObject), Delegates=new string [] {"WeakDelegate"}, Events=new Type [] {typeof (CLLocationManagerDelegate)})]
	partial interface CLLocationManager {
		[Wrap ("WeakDelegate")]
		CLLocationManagerDelegate Delegate { get; set;  }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set;  }
	
		[Export ("locationServicesEnabled"), Internal]
		bool _LocationServicesEnabledInstance { get;  }
	
		[Export ("distanceFilter", ArgumentSemantic.Assign)]
		double DistanceFilter { get; set;  }
	
		[Export ("desiredAccuracy", ArgumentSemantic.Assign)]
		double DesiredAccuracy { get; set;  }
	
		[Export ("location")]
		CLLocation Location { get;  }
	
		[Export ("startUpdatingLocation")]
		void StartUpdatingLocation ();
	
		[Export ("stopUpdatingLocation")]
		void StopUpdatingLocation ();

		[Since (4,0)]
		[Export ("locationServicesEnabled"), Static, Internal]
		bool _LocationServicesEnabledStatic { get; }

#if !MONOMAC
		[Export ("headingFilter", ArgumentSemantic.Assign)]
		double HeadingFilter { get; set;  }
	
		[Export ("headingAvailable"), Internal]
		bool _HeadingAvailableInstance { get;  }
	
		[Export ("startUpdatingHeading")]
		void StartUpdatingHeading ();
	
		[Export ("stopUpdatingHeading")]
		void StopUpdatingHeading ();
	
		[Export ("dismissHeadingCalibrationDisplay")]
		void DismissHeadingCalibrationDisplay ();
	
		[Since (3,2)]
		[Obsolete ("Deprecated in iOS 6.0")]
		[Export ("purpose", ArgumentSemantic.Copy)]
		string Purpose { get; set; }

		[Since (4,0)]
		[Export ("headingAvailable"), Static, Internal]
		bool _HeadingAvailableStatic { get; }

		[Since (4,0)]
		[Export ("significantLocationChangeMonitoringAvailable"), Static]
		bool SignificantLocationChangeMonitoringAvailable { get; }

		[Since (4,0)]
		[Export ("regionMonitoringAvailable"), Static]
		bool RegionMonitoringAvailable { get; }

		[Since (4,0)]
		[Obsolete ("Replaced by RegionMonitoringAvailable in iOS 6.0")]
		[Export ("regionMonitoringEnabled"), Static]
		bool RegionMonitoringEnabled { get; }

		[Since (4,0)]
		[Export ("headingOrientation")]
		CLDeviceOrientation HeadingOrientation { get; set; }

		[Export ("heading")]
		[Since (4,0)]
		CLHeading Heading { get; }

		[Export ("maximumRegionMonitoringDistance")]
		[Since (4,0)]
		double MaximumRegionMonitoringDistance { get; }

		[Export ("monitoredRegions")]
		[Since (4,0)]
		NSSet MonitoredRegions { get; }

		[Since (4,0)]
		[Export ("startMonitoringSignificantLocationChanges")]
		void StartMonitoringSignificantLocationChanges ();

		[Since (4,0)]
		[Export ("stopMonitoringSignificantLocationChanges")]
		void StopMonitoringSignificantLocationChanges ();

		[Since (4,0)]
		[Obsolete ("Deprecated in iOS 6.0")]
		[Export ("startMonitoringForRegion:desiredAccuracy:")]
		void StartMonitoring (CLRegion region, double desiredAccuracy);

		[Since (4,0)]
		[Export ("stopMonitoringForRegion:")]
		void StopMonitoring (CLRegion region);

		[Since (4,2)]
		[Export ("authorizationStatus")][Static]
		CLAuthorizationStatus Status { get; }

		[Export ("startMonitoringForRegion:")]
		void StartMonitoring (CLRegion region);

		[Since (6,0)]
		[Export ("activityType", ArgumentSemantic.Assign)]
		CLActivityType ActivityType  { get; set; }

		[Since (6,0)]
		[Export ("pausesLocationUpdatesAutomatically", ArgumentSemantic.Assign)]
		bool PausesLocationUpdatesAutomatically { get; set; }

		[Since (6,0)]
		[Export ("allowDeferredLocationUpdatesUntilTraveled:timeout:")]
		void AllowDeferredLocationUpdatesUntil (double distance, double timeout);

		[Since (6,0)]
		[Export ("disallowDeferredLocationUpdates")]
		void DisallowDeferredLocationUpdates ();

		[Since (6,0)]
		[Static]
		[Export ("deferredLocationUpdatesAvailable")]
		bool DeferredLocationUpdatesAvailable { get; }

		[Since (6,0)]
		[Field ("CLTimeInternalMax")]
		double MaxTimeInterval { get; }
#endif
	}
	
	[BaseType (typeof (NSObject))]
	[Model]
	[Synthetic]
	partial interface CLLocationManagerDelegate
	{
		[Obsolete ("Deprecated in iOS 6.0")]
		[Export ("locationManager:didUpdateToLocation:fromLocation:"), EventArgs ("CLLocationUpdated")]
		void UpdatedLocation (CLLocationManager  manager, CLLocation newLocation, CLLocation oldLocation);
	
#if !MONOMAC
		[Export ("locationManager:didUpdateHeading:"), EventArgs ("CLHeadingUpdated")]
		void UpdatedHeading (CLLocationManager  manager, CLHeading newHeading);
#endif
	
		[Export ("locationManagerShouldDisplayHeadingCalibration:"), DelegateName ("CLLocationManagerEventArgs"), DefaultValue (true)]
		bool ShouldDisplayHeadingCalibration (CLLocationManager manager);
	
		[Export ("locationManager:didFailWithError:"), EventArgs ("NSError", SkipGeneration = true)]
		void Failed (CLLocationManager manager, NSError error);

#if !MONOMAC
		[Since (4,0)]
		[Export ("locationManager:didEnterRegion:"), EventArgs ("CLRegion")]
		void RegionEntered (CLLocationManager manager, CLRegion region);

		[Since (4,0)]
		[Export ("locationManager:didExitRegion:"), EventArgs ("CLRegion")]
		void RegionLeft (CLLocationManager manager, CLRegion region);

		[Since (4,0)]
		[Export ("locationManager:monitoringDidFailForRegion:withError:"), EventArgs ("CLRegionError")]
		void MonitoringFailed (CLLocationManager manager, CLRegion region, NSError error);

		[Since(5,0)]
		[Export ("locationManager:didStartMonitoringForRegion:"), EventArgs ("CLRegion")]
		void DidStartMonitoringForRegion (CLLocationManager manager, CLRegion region);
#endif

		[Since (4,2)]
		[Export ("locationManager:didChangeAuthorizationStatus:"), EventArgs ("CLAuthorizationChanged")]
		void AuthorizationChanged (CLLocationManager manager, CLAuthorizationStatus status);

		[Since (6,0)]
		[Export ("locationManager:didUpdateLocations:"), EventArgs ("CLLocationsUpdated")]
		void LocationsUpdated (CLLocationManager manager, CLLocation[] locations);

		[Since (6,0)]
		[Export ("locationManagerDidPauseLocationUpdates:"), EventArgs ("")]
		void LocationUpdatesPaused (CLLocationManager manager);

		[Since (6,0)]
		[Export ("locationManagerDidResumeLocationUpdates:"), EventArgs ("")]
		void LocationUpdatesResumed (CLLocationManager manager);

		[Since (6,0)]
		[Export ("locationManager:didFinishDeferredUpdatesWithError:"), EventArgs ("NSError", SkipGeneration = true)]
		void DeferredUpdatesFinished (CLLocationManager manager, NSError error);
	}

#if !MONOMAC
	[Since (4,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // will crash, see CoreLocation.cs for compatibility stubs
	partial interface CLRegion {
		[Export ("center")]
		CLLocationCoordinate2D Center { get;  }

		[Export ("radius")]
		double Radius { get;  }

		[Export ("identifier")]
		string Identifier { get;  }

		[Export ("initCircularRegionWithCenter:radius:identifier:")]
		IntPtr Constructor (CLLocationCoordinate2D center, double radius, string identifier);

		[Export ("containsCoordinate:")]
		bool Contains (CLLocationCoordinate2D coordinate);
	}

	[Since (5,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // will crash, see CoreLocation.cs for compatibility stubs
	interface CLPlacemark {
		[Export("addressDictionary")]
		NSDictionary AddressDictionary { get; }

		[Export("administrativeArea")]
		string AdministrativeArea { get; }

		[Export("subAdministrativeArea")]
		string SubAdministrativeArea { get; }

		[Export("subLocality")]
		string SubLocality { get; }

		[Export("locality")]
		string Locality { get; }

		[Export("country")]
		string Country { get; }
	
		[Export("postalCode")]
		string PostalCode { get; }

		[Export("thoroughfare")]
		string Thoroughfare { get; }

		[Export("subThoroughfare")]
		string SubThoroughfare { get; }

		[Export ("ISOcountryCode")]
		string IsoCountryCode { get;  }

		[Export ("areasOfInterest")]
		string [] AreasOfInterest { get;  }

		[Export ("initWithPlacemark:")]
		IntPtr Constructor (CLPlacemark placemark);

		[Export ("inlandWater")]
		string InlandWater { get;  }

		[Export ("location")]
		CLLocation Location { get; }

		[Export ("name")]
		string Name { get;  }

		[Export ("ocean")]
		string Ocean { get;  }

		[Export ("region")]
		CLRegion Region { get; }
	}

	delegate void CLGeocodeCompletionHandler (CLPlacemark [] placemarks, NSError error);

	[BaseType (typeof (NSObject))]
	interface CLGeocoder {
		[Export ("isGeocoding")]
		bool Geocoding { get; }

		[Export ("reverseGeocodeLocation:completionHandler:")]
		[Async]
		void ReverseGeocodeLocation (CLLocation location, CLGeocodeCompletionHandler completionHandler);

		[Export ("geocodeAddressDictionary:completionHandler:")]
		[Async]
		void GeocodeAddress (NSDictionary addressDictionary, CLGeocodeCompletionHandler completionHandler);

		[Export ("geocodeAddressString:completionHandler:")]
		[Async]
		void GeocodeAddress (string addressString, CLGeocodeCompletionHandler completionHandler);

		[Export ("geocodeAddressString:inRegion:completionHandler:")]
		[Async]
		void GeocodeAddress (string addressString, CLRegion region, CLGeocodeCompletionHandler completionHandler);

		[Export ("cancelGeocode")]
		void CancelGeocode ();
	}

#endif
}

