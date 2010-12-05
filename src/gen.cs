
	[BaseType (typeof (NSObject))]
	interface CISampler {
		[Static, Export ("samplerWithImage:")]
		CISampler FromImage (CIImage im);

		[Internal, Static]
		[Export ("samplerWithImage:options:")]
		CISampler FromImage (CIImage im, NSDictionary options);

		[Export ("initWithImage:")]
		NSObject InitWithImage (CIImage im);

		[Export ("initWithImage:keysAndValues:")]
		NSObject InitWithImagekeysAndValues (CIImage im, );

		[Export ("initWithImage:options:")]
		NSObject InitWithImageoptions (CIImage im, NSDictionary dict);

		[Export ("definition")]
		CIFilterShape Definition ();

		[Export ("extent")]
		CGRect Extent ();

	}
