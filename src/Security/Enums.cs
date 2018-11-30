// Copyright 2011, 2012 Xamarin Inc

namespace MonoMac.Security {

	public enum SecStatusCode {
		Success = 0,
		Unimplemented = -4,
		Param = -50,
		Allocate = -108,
		NotAvailable = -25291,
		ReadOnly = -25292,
		AuthFailed = -25293,
		NoSuchKeyChain = -25294,
		InvalidKeyChain = -25295,
		DuplicateKeyChain = -25296,
		DuplicateItem = -25299,
		ItemNotFound = -25300,
		InteractionNotAllowed = -25308,
		Decode = -26275,
	}

	public enum SecPadding {
		None      = 0,
		PKCS1     = 1,
		OAEP      = 2,
		PKCS1MD2  = 0x8000,
		PKCS1MD5  = 0x8001,
		PKCS1SHA1 = 0x8002,
		PKCS1SHA224 = 0x8003,
		PKCS1SHA256 = 0x8004,
		PKCS1SHA384 = 0x8005,
		PKCS1SHA512 = 0x8006,
	}
	
	public enum SecTrustResult {
		Invalid,
		Proceed,
		Confirm,
		Deny,
		Unspecified,
		RecoverableTrustFailure,
		FatalTrustFailure,
		ResultOtherError,
	}
}