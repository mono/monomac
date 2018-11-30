//
// This is the binding generator for the MonoTouch API, it uses the
// contract in API.cs to generate the binding.
//
// Authors:
//   Geoff Norton
//   Miguel de Icaza
//   Marek Safar (marek.safar@gmail.com)
//
// Copyright 2009-2010, Novell, Inc.
// Copyright 2011-2013 Xamarin, Inc.
//
//
// This generator produces various */*.g.cs files based on the
// interface-based type description on this file, see the 
// embedded `MonoTouch.UIKit' namespace here for an example
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
// TODO:
//   * Add support for wrapping "ref" and "out" NSObjects (WrappedTypes)
//     Typically this is necessary for things like NSError.
//
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;

#if MONOMAC
using MonoMac.ObjCRuntime;
using MonoMac.Foundation;
using MonoMac.CoreFoundation;
using MonoMac.CoreGraphics;
using MonoMac.CoreVideo;
using MonoMac.OpenGL;
using MonoMac.CoreMidi;
using MonoMac.CoreMedia;

using DictionaryContainerType = MonoMac.Foundation.DictionaryContainer;

#else
using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.CoreFoundation;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreMedia;
using MonoTouch.CoreVideo;
using MonoTouch.CoreMidi;
using MonoTouch.MediaToolbox;

using DictionaryContainerType = MonoTouch.Foundation.DictionaryContainer;

#endif

public static class ReflectionExtensions {
	public static Type GetBaseType (Type type)
	{
		object [] btype = type.GetCustomAttributes (typeof (BaseTypeAttribute), true);
		BaseTypeAttribute bta = btype.Length > 0 ? ((BaseTypeAttribute) btype [0]) : null;
		Type base_type = bta != null ?  bta.BaseType : typeof (object);

		return base_type;
	}

	public static List <PropertyInfo> GatherProperties (this Type type) {
		List <PropertyInfo> properties = new List <PropertyInfo> (type.GetProperties ());

		if (Generator.IsBtouch)
			return properties;

		Type parent_type = GetBaseType (type);
		string owrap;
		string nwrap;

		if (parent_type != typeof (NSObject)) {
			if (Attribute.IsDefined (parent_type, typeof (ModelAttribute), false)) {
				foreach (PropertyInfo pinfo in parent_type.GetProperties ()) {
					bool toadd = true;
					var modelea = Generator.GetExportAttribute (pinfo, out nwrap);

					if (modelea == null)
						continue;

					foreach (PropertyInfo exists in properties) {
						var origea = Generator.GetExportAttribute (exists, out owrap);
						if (origea.Selector == modelea.Selector)
							toadd = false;
					}

					if (toadd)
						properties.Add (pinfo);
				}
			}
	                parent_type = GetBaseType (parent_type);
		}

		return properties;
	}

	public static List <PropertyInfo> GatherProperties (this Type type, BindingFlags flags) {
		List <PropertyInfo> properties = new List <PropertyInfo> (type.GetProperties (flags));

		if (Generator.IsBtouch)
			return properties;

		Type parent_type = GetBaseType (type);
		string owrap;
		string nwrap;

		if (parent_type != typeof (NSObject)) {
			if (Attribute.IsDefined (parent_type, typeof (ModelAttribute), false)) {
				foreach (PropertyInfo pinfo in parent_type.GetProperties (flags)) {
					bool toadd = true;
					var modelea = Generator.GetExportAttribute (pinfo, out nwrap);

					if (modelea == null)
						continue;

					foreach (PropertyInfo exists in properties) {
						var origea = Generator.GetExportAttribute (exists, out owrap);
						if (origea.Selector == modelea.Selector)
							toadd = false;
					}

					if (toadd)
						properties.Add (pinfo);
				}
			}
	                parent_type = GetBaseType (parent_type);
		}

		return properties;
	}

	public static List <MethodInfo> GatherMethods (this Type type) {
		List <MethodInfo> methods = new List <MethodInfo> (type.GetMethods ());

		if (Generator.IsBtouch)
			return methods;

		Type parent_type = GetBaseType (type);

		if (parent_type != typeof (NSObject)) {
			if (Attribute.IsDefined (parent_type, typeof (ModelAttribute), false))
				foreach (MethodInfo minfo in parent_type.GetMethods ())
					if (minfo.GetCustomAttributes (typeof (ExportAttribute), false).Length > 0)
						methods.Add (minfo);
			parent_type = GetBaseType (parent_type);
		}

		return methods;

	}

	public static List <MethodInfo> GatherMethods (this Type type, BindingFlags flags) {
		List <MethodInfo> methods = new List <MethodInfo> (type.GetMethods (flags));

		if (Generator.IsBtouch)
			return methods;

		Type parent_type = GetBaseType (type);

		if (parent_type != typeof (NSObject)) {
			if (Attribute.IsDefined (parent_type, typeof (ModelAttribute), false))
				foreach (MethodInfo minfo in parent_type.GetMethods ())
					if (minfo.GetCustomAttributes (typeof (ExportAttribute), false).Length > 0)
						methods.Add (minfo);
			parent_type = GetBaseType (parent_type);
		}

		return methods;
	}
}

// Used to mark if a type is not a wrapper type.
public class SyntheticAttribute : Attribute {
	public SyntheticAttribute () { }
}

public class NeedsAuditAttribute : Attribute {
	public NeedsAuditAttribute (string reason)
	{
		Reason = reason;
	}

	public string Reason { get; set; }
}

public class MarshalNativeExceptionsAttribute : Attribute {
}

public class RetainListAttribute : Attribute {
	public RetainListAttribute (bool doadd, string name)
	{
		Add = doadd;
		WrapName = name;
	}

	public string WrapName { get; set; }
	public bool Add { get; set; }
}

public class RetainAttribute : Attribute {
	public RetainAttribute ()
	{
	}

	public RetainAttribute (string wrap)
	{
		WrapName = wrap;
	}
	public string WrapName { get; set; }
}

public class ReleaseAttribute : Attribute {
}

[AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
public class PostGetAttribute : Attribute {
	public PostGetAttribute (string name)
	{
		MethodName = name;
	}

	public string MethodName { get; set; }
}

public class BaseTypeAttribute : Attribute {
	public BaseTypeAttribute (Type t)
	{
		BaseType = t;
	}
	public Type BaseType { get; set; }
	public string Name { get; set; }
	public Type [] Events { get; set; }
	public string [] Delegates { get; set; }
	public bool Singleton { get; set; }

	// If set, the code will keep a reference in the EnsureXXX method for
	// delegates and will clear the reference to the object in the method
	// referenced by KeepUntilRef.   Currently uses an ArrayList, so this
	// is not really designed as a workaround for systems that create
	// too many objects, but two cases in particular that users keep
	// trampling on: UIAlertView and UIActionSheet
	public string KeepRefUntil { get; set; }
}

//
// Used for methods that invoke other targets, not this.Handle
//
public class BindAttribute : Attribute {
	public BindAttribute (string sel)
	{
		Selector = sel;
	}
	public string Selector { get; set; }

	// By default [Bind] makes non-virtual methods
	public bool Virtual { get; set; }
}

public class WrapAttribute : Attribute {
	public WrapAttribute (string methodname)
	{
		MethodName = methodname;
	}
	public string MethodName { get; set; }
}

// When applied instructs the generator to call Release on the returned objects
// this happens when factory methods in Objetive-C return objects with refcount=1
public class FactoryAttribute : Attribute {
	public FactoryAttribute () {}
}

// When applied, it instructs the generator to not use NSStrings for marshalling.
public class PlainStringAttribute : Attribute {
	public PlainStringAttribute () {}
}

public class AutoreleaseAttribute : Attribute {
	public AutoreleaseAttribute () {}
}

// When applied, the generator generates a check for the Handle being valid on the main object, to
// ensure that the user did not Dispose() the object.
//
// This is typically used in scenarios where the user might be tempted to dispose
// the object in a callback:
//
//     foo.FinishedDownloading += delegate { foo.Dispose (); }
//
// This would invalidate "foo" and force the code to return to a destroyed/freed
// object
public class CheckDisposedAttribute : Attribute {
	public CheckDisposedAttribute () {}
}

//
// When applied, instructs the generator to use this object as the
// target, instead of the implicit Handle Can only be used in methods
// that are [Bind] instead of [Export]
//
public class TargetAttribute : Attribute {
	public TargetAttribute () {}
}

public class ProxyAttribute : Attribute {
	public ProxyAttribute () {}
}

// When applied to a member, generates the member as static
public class StaticAttribute : Attribute {
	public StaticAttribute () {}
}

// flags the backing field for the property to with .NET's [ThreadStatic] property
public class IsThreadStaticAttribute : Attribute {
	public IsThreadStaticAttribute () {}
}

// When applied to a member, generates the member as static
// and passes IntPtr.Zero or null if the parameter is null
public class NullAllowedAttribute : Attribute {
	public NullAllowedAttribute () {}
}

// When applied to a method or property, flags the resulting generated code as internal
public class InternalAttribute : Attribute {
	public InternalAttribute () {}
}

// When applied to a method or property, flags the resulting generated code as internal
public sealed class ProtectedAttribute : Attribute {
}

// When this attribute is applied to the interface definition it will
// flag the default constructor as private.  This means that you can
// still instantiate object of this class internally from your
// extension file, but it just wont be accessible to users of your
// class.
public class PrivateDefaultCtorAttribute : DefaultCtorVisibilityAttribute {
	public PrivateDefaultCtorAttribute () : base (Visibility.Private) {}
}

public enum Visibility {
	Public,
	Protected,
	Internal,
	ProtectedInternal,
	Private,
	Disabled
}

// When this attribute is applied to the interface definition it will
// flag the default ctor with the corresponding visibility (or disabled
// altogether if Visibility.Disabled is used).
public class DefaultCtorVisibilityAttribute : Attribute {
	public DefaultCtorVisibilityAttribute (Visibility visibility)
	{
		this.Visibility = visibility;
	}

	public Visibility Visibility { get; set; }
}

// When this attribute is applied to the interface definition it will
// prevent the generator from producing the default constructor.
public class DisableDefaultCtorAttribute : DefaultCtorVisibilityAttribute {
	public DisableDefaultCtorAttribute () : base (Visibility.Disabled) {}
}

//
// If this attribute is applied to a property, we do not generate a
// backing field.   See bugzilla #3359 and Assistly 7032 for some
// background information
//
public class TransientAttribute : Attribute {
	public TransientAttribute () {}
}

// Used for mandatory methods that must be implemented in a [Model].
public class AbstractAttribute : Attribute {
	public AbstractAttribute () {} 
}

// Used for mandatory methods that must be implemented in a [Model].
public class OverrideAttribute : Attribute {
	public OverrideAttribute () {} 
}

// Makes the result use the `new' attribtue
public class NewAttribute : Attribute {
	public NewAttribute () {} 
}

// Makes the result sealed
public class SealedAttribute : Attribute {
	public SealedAttribute () {} 
}

// Flags the object as being thread safe
public class ThreadSafeAttribute : Attribute {
	public ThreadSafeAttribute () {}
}

// Marks a struct parameter/return value as requiring a certain alignment.
public class AlignAttribute : Attribute {
	public int Align { get; set; }
	public AlignAttribute (int align)
	{
		Align = align;
	}
	public int Bits {
		get {
			int bits = 0;
			int tmp = Align;
			while (tmp > 1) {
				bits++;
				tmp /= 2;
			}
			return bits;
		}
	}
}

//
// When applied, flags the [Flags] as a notification and generates the
// code to strongly type the notification.
//
// The type has information about the strong type notification, while the
// NotificationCenter if not null, indicates how to get the notification center.
//
// If you do not specify it, it will use NSNotificationCenter.DefaultCenter,
// you would typically use this to specify the code needed to get to it.
//
[AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
public class NotificationAttribute : Attribute {
	public NotificationAttribute (Type t) { Type = t; }
	public NotificationAttribute (Type t, string notificationCenter) { Type = t; NotificationCenter = notificationCenter; }
	public NotificationAttribute (string notificationCenter) { NotificationCenter = notificationCenter; }
	public NotificationAttribute () {}
	
	public Type Type { get; set; }
	public string NotificationCenter { get; set; }
}

//
// Applied to attributes in the notification EventArgs
// to generate code that merely probes for the existance of
// the key, instead of extracting a value out of the
// userInfo dictionary
//
[AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
public class ProbePresenceAttribute : Attribute {
	public ProbePresenceAttribute () {}
}

public class EventArgsAttribute : Attribute {
	public EventArgsAttribute (string s)
	{
		ArgName = s;
	}
	public EventArgsAttribute (string s, bool skip)
	{
		ArgName = s;
		SkipGeneration = skip;
	}
	public EventArgsAttribute (string s, bool skip, bool fullname)
	{
		ArgName = s;
		SkipGeneration = skip;
		FullName = fullname;
	}

	public string ArgName { get; set; }
	public bool SkipGeneration { get; set; }
	public bool FullName { get; set; }
}

//
// Used to specify the delegate type that will be created when
// the generator creates the delegate properties on the host
// class that holds events
//
// example:
// interface SomeDelegate {
//     [Export ("foo"), DelegateName ("GetBoolean"), DefaultValue (false)]
//     bool Confirm (Some source);
//
public class DelegateNameAttribute : Attribute {
	public DelegateNameAttribute (string s)
	{
		Name = s;
	}

	public string Name { get; set; }
}

public class EventNameAttribute : Attribute {
	public EventNameAttribute (string s)
	{
		EvtName = s;
	}
	public string EvtName { get; set; }
}

public class DefaultValueAttribute : Attribute {
	public DefaultValueAttribute (object o){
		Default = o;
	}
	public object Default { get; set; }
}

public class DefaultValueFromArgumentAttribute : Attribute {
	public DefaultValueFromArgumentAttribute (string s){
		Argument = s;
	}
	public string Argument { get; set; }
}

public class NoDefaultValueAttribute : Attribute {
}

// Apply to strings parameters that are merely retained or assigned,
// not copied this is an exception as it is advised in the coding
// standard for Objective-C to avoid this, but a few properties do use
// this.  Use this attribtue for properties flagged with `retain' or
// `assign', which look like this:
//
// @property (retain) NSString foo;
// @property (assign) NSString assigned;
//
// This forced the generator to create an NSString before calling the
// API instead of using the fast string marshalling code.
public class DisableZeroCopyAttribute : Attribute {
	public DisableZeroCopyAttribute () {}
}

//
// By default, the generator will not do Zero Copying of strings, as most
// third party libraries do not follow Apple's design guidelines of making
// string properties and parameters copy parameters, instead many libraries
// "retain" as a broken optimization [1].
//
// The consumer of the genertor can force this by passing
// --use-zero-copy or setting the [assembly:ZeroCopyStrings] attribute.
// When these are set, the generator assumes the library perform
// copies over any NSStrings it keeps instead of retains/assigns and
// that any property that happens to be a retain/assign has the
// [DisableZeroCopyAttribute] attribute applied.
//
// [1] It is broken becase consumer code can pass an NSMutableString, the
// library retains the value, but does not have a way of noticing changes
// that might happen to the mutable string behind its back.
//
// In the ZeroCopy case it is a problem because we pass handles to stack-allocated
// strings that stop existing after the invocation is over.
//
[AttributeUsage(AttributeTargets.Assembly|AttributeTargets.Method|AttributeTargets.Interface, AllowMultiple=true)]
public class ZeroCopyStringsAttribute : Attribute {
}

[AttributeUsage(AttributeTargets.Method|AttributeTargets.Property, AllowMultiple=true)]
public class SnippetAttribute : Attribute {
	public SnippetAttribute (string s)
	{
		Code = s;
	}
	public string Code { get; set; }
}

//
// PreSnippet code is inserted after the parameters have been validated/marshalled
// 
public class PreSnippetAttribute : SnippetAttribute {
	public PreSnippetAttribute (string s) : base (s) {}
}

//
// PrologueSnippet code is inserted before any code is generated
// 
public class PrologueSnippetAttribute : SnippetAttribute {
	public PrologueSnippetAttribute (string s) : base (s) {}
}

//
// PostSnippet code is inserted before returning, before paramters are disposed/released
// 
public class PostSnippetAttribute : SnippetAttribute {
	public PostSnippetAttribute (string s) : base (s) {}
}

//
// Code to run from a generated Dispose method
//
[AttributeUsage(AttributeTargets.Interface, AllowMultiple=true)]
public class DisposeAttribute : SnippetAttribute {
	public DisposeAttribute (string s) : base (s) {}
}

//
// This attribute is used to flag properties that should be exposed on the strongly typed
// nested Appearance class.   It is usually a superset of what Apple has labeled with
// UI_APPEARANCE_SELECTOR because they do support more selectors than those flagged in
// the UIApperance proxies, so we must label all the options.   This will be a list that
// is organically grown as we find them
//
[AttributeUsage (AttributeTargets.Property|AttributeTargets.Method, AllowMultiple=false)]
public class AppearanceAttribute : Attribute {
	public AppearanceAttribute () {}
}

//
// This is designed to be applied to setter methods in
// a base class `Foo' when a `MutableFoo' exists.
//
// This allows the Foo.set_XXX to exists but throw an exception 
// but derived classes would then override the property
//
[AttributeUsage (AttributeTargets.Method, AllowMultiple=false)]
public class NotImplementedAttribute : Attribute {
	public NotImplementedAttribute () {}
}

//
// Apply this attribute to a class to add methods that in Objective-c
// are added as categories
//
// Use the BaseType attribute to reference which class this is extending
//
// Like this:
//   [Category]
//   [BaseType (typeof (UIView))]
//   interface UIViewExtensions {
//     [Export ("method_in_the_objective_c_category")]
//     void ThisWillBecome_a_c_sharp_extension_method_in_class_UIViewExtensions ();
// }
[AttributeUsage (AttributeTargets.Interface, AllowMultiple=false)]
public class CategoryAttribute : Attribute {
	public CategoryAttribute () {}
}

//
// Apply this attribute to a method that you want an async version of a callback method.
//
// Use the ResultType or ResultTypeName attribute to describe any composite value to be by the Task object.
// Use MethodName to customize the name of the generated method
//
// Note that this only supports the case where the callback is the last parameter of the method.
//
// Like this:
//[Export ("saveAccount:withCompletionHandler:")] [Async]
//void SaveAccount (ACAccount account, ACAccountStoreSaveCompletionHandler completionHandler);
// }
[AttributeUsage (AttributeTargets.Method, AllowMultiple=false)]
public class AsyncAttribute : Attribute {

	//This will automagically generate the async method.
	//This works with 4 kinds of callbacks: (), (NSError), (result), (result, NSError)
	public AsyncAttribute () {}

	//This works with 2 kinds of callbacks: (...) and (..., NSError).
	//Parameters are passed in order to a constructor in resultType
	public AsyncAttribute (Type resultType) {
		ResultType = resultType;
	}

	//This works with 2 kinds of callbacks: (...) and (..., NSError).
	//Parameters are passed in order to a result type that is automatically created if size > 1
	//The generated method is named after the @methodName
	public AsyncAttribute (string methodName) {
		MethodName = methodName;
	}

	public Type ResultType { get; set; }
	public string MethodName { get; set; }
	public string ResultTypeName { get; set; }
}

//
// Used to encapsulate flags about types in either the parameter or the return value
// For now, it only supports the [PlainString] attribute on strings.
//
public class MarshalInfo {
	public bool PlainString;
	public Type Type;

 	// This is set on a string parameter if the argument parameters are set to
 	// Copy.   This means that we can do fast string passing.
	public bool ZeroCopyStringMarshal;

	public bool IsAligned;

	// Used for parameters
	public MarshalInfo (MethodInfo mi, ParameterInfo pi)
	{
		PlainString = pi.GetCustomAttributes (typeof (PlainStringAttribute), true).Length > 0;
		Type = pi.ParameterType;
		ZeroCopyStringMarshal = (Type == typeof (string)) && PlainString == false && !Generator.HasAttribute (pi, (typeof (DisableZeroCopyAttribute))) && Generator.SharedGenerator.type_wants_zero_copy;
		IsAligned = Generator.HasAttribute (pi, typeof (AlignAttribute));
		if (IsAligned)
			Type = typeof (IntPtr);
		if (ZeroCopyStringMarshal && Generator.HasAttribute (mi, typeof (DisableZeroCopyAttribute)))
			ZeroCopyStringMarshal = false;
	}

	// Used to return values
	public MarshalInfo (MethodInfo mi)
	{
		PlainString = mi.ReturnTypeCustomAttributes.GetCustomAttributes (typeof (PlainStringAttribute), true).Length > 0;
		Type = mi.ReturnType;
		IsAligned = Generator.HasAttribute (mi, typeof (AlignAttribute));
		if (IsAligned)
			Type = typeof (IntPtr);
	}

	public static bool UseString (MethodInfo mi, ParameterInfo pi)
	{
		return new MarshalInfo (mi, pi).PlainString;
	}

	public static implicit operator MarshalInfo (MethodInfo mi)
	{
		return new MarshalInfo (mi);
	}
}

public class Tuple<A,B> {
	public Tuple (A a, B b)
	{
		Item1 = a;
		Item2 = b;
	}
	public A Item1;
	public B Item2;
}
//
// Encapsulates the information necessary to create a block delegate
//
// The Name is the internal generated name we use for the delegate
// The Parameters is used for the internal delegate signature
// The Invoke contains the invocation steps necessary to invoke the method
//
public class TrampolineInfo {
	public string UserDelegate, DelegateName, TrampolineName, Parameters, Invoke, ReturnType, DelegateReturnType, ReturnFormat, Clear;

	public TrampolineInfo (string userDelegate, string delegateName, string trampolineName, string pars, string invoke, string returnType, string delegateReturnType, string returnFormat, string clear)
	{
		UserDelegate = userDelegate;
		DelegateName = delegateName;
		Parameters = pars;
		TrampolineName = trampolineName;
		Invoke = invoke;
		ReturnType = returnType;
		DelegateReturnType = delegateReturnType;
		ReturnFormat = returnFormat;
		Clear = clear;
	}

	public string StaticName {
		get {
			return "S" + DelegateName;
		}
	}
}

//
// This class is used to generate a graph of the type hierarchy of the
// generated types and required by the UIApperance support to determine
// which types need to have Appearance methods created
//
public class GeneratedType {
	static Dictionary<Type,GeneratedType> knownTypes = new Dictionary<Type,GeneratedType> ();

	public static GeneratedType Lookup (Type t)
	{
		if (knownTypes.ContainsKey (t))
			return knownTypes [t];
		var n = new GeneratedType (t);
		knownTypes [t] = n;
		return n;
	}
	
	public GeneratedType (Type t)
	{
		Type = t;
		foreach (var iface in Type.GetInterfaces ()){
			if (iface.Name == "UIAppearance")
				ImplementsAppearance = true;
		}
		var btype = ReflectionExtensions.GetBaseType (Type);
		if (btype != typeof (object)){
			Parent = btype;
			ParentGenerated = Lookup (Parent);

			// If our parent had UIAppearance, we flag this class as well
			if (ParentGenerated.ImplementsAppearance)
				ImplementsAppearance = true;
			ParentGenerated.Children.Add (this);
		}

		if (t.GetCustomAttributes (typeof (CategoryAttribute), true).Length != 0)
			ImplementsAppearance = false;
	}
	public Type Type;
	public List<GeneratedType> Children = new List<GeneratedType> (1);
	public Type Parent;
	public GeneratedType ParentGenerated;
	public bool ImplementsAppearance;

	List<MemberInfo> appearance_selectors;
	
	public List<MemberInfo> AppearanceSelectors {
		get {
			if (appearance_selectors == null)
				appearance_selectors = new List<MemberInfo> ();
			return appearance_selectors;
		}
	}
}

public class MemberInformation
{
	public readonly bool is_abstract, is_protected, is_internal, is_override, is_new, is_sealed, is_static, is_thread_static, is_autorelease, is_wrapper;
	public readonly Generator.ThreadCheck threadCheck;
	public bool is_unsafe, is_virtual_method, is_export, is_category_extension, is_variadic;
	public string selector, wrap_method;

	MemberInformation (MemberInfo mi, Type type)
	{
		is_abstract = Generator.HasAttribute (mi, typeof (AbstractAttribute)) && mi.DeclaringType == type;
		is_protected = Generator.HasAttribute (mi, typeof (ProtectedAttribute));
		is_internal = Generator.HasAttribute (mi, typeof (InternalAttribute));
		is_override = Generator.HasAttribute (mi, typeof (OverrideAttribute)) || !Generator.MemberBelongsToType (mi.DeclaringType, type);
		is_new = Generator.HasAttribute (mi, typeof (NewAttribute));
		is_sealed = Generator.HasAttribute (mi, typeof (SealedAttribute));
		is_static = Generator.HasAttribute (mi, typeof (StaticAttribute));
		is_thread_static = Generator.HasAttribute (mi, typeof (IsThreadStaticAttribute));
		is_autorelease = Generator.HasAttribute (mi, typeof (AutoreleaseAttribute));
		is_wrapper = !Generator.HasAttribute (mi.DeclaringType, typeof(SyntheticAttribute));
		threadCheck = Generator.HasAttribute (mi, typeof (ThreadSafeAttribute)) ? Generator.ThreadCheck.Off : Generator.ThreadCheck.On;

	}

	public MemberInformation (MethodInfo mi, Type type, Type category_extension_type) : this ((MemberInfo)mi, type)
	{
		foreach (ParameterInfo pi in mi.GetParameters ())
			if (pi.ParameterType.IsSubclassOf (typeof (Delegate)))
				is_unsafe = true;

		object [] attr = mi.GetCustomAttributes (typeof (ExportAttribute), true);
		if (attr.Length != 1){
			attr = mi.GetCustomAttributes (typeof (BindAttribute), true);
			if (attr.Length != 1) {
				attr = mi.GetCustomAttributes (typeof (WrapAttribute), true);
				if (attr.Length != 1)
					throw new BindingException (1012, true, "No Export or Bind attribute defined on {0}.{1}", type, mi.Name);

				wrap_method = ((WrapAttribute) attr [0]).MethodName;
			} else {
				BindAttribute ba = (BindAttribute) attr [0];
				selector = ba.Selector;
				is_virtual_method = ba.Virtual;
			}
		} else {
			ExportAttribute ea = (ExportAttribute) attr [0];
			selector = ea.Selector;
			is_variadic = ea.IsVariadic;

			if (!is_sealed || !is_wrapper) {
				is_virtual_method = mi.Name != "Constructor";
				is_export = true;
			}
		}

		if (category_extension_type != null)
			is_category_extension = true;

		if (is_static || is_category_extension)
			is_virtual_method = false;
	}

	public MemberInformation (PropertyInfo pi, Type type) : this ((MemberInfo)pi, type)
	{
		if (pi.PropertyType.IsSubclassOf (typeof (Delegate)))
			is_unsafe = true;

		var export = Generator.GetExportAttribute (pi, out wrap_method);
		if (export != null)
			selector = export.Selector;

		if (wrap_method != null)
			is_virtual_method = false;
		else
			is_virtual_method = !is_static;
	}

	public string GetVisibility ()
	{
		var mod = is_protected ? "protected" : null;
		mod += is_internal ? "internal" : null;
		if (string.IsNullOrEmpty (mod))
			mod = "public";
		return mod;
	}

	public string GetModifiers ()
	{
		string mods = "";

		mods += is_unsafe ? "unsafe " : null;
		mods += is_new ? "new " : "";

		if (is_sealed) {
			mods += "";
		} else if (is_static || is_category_extension) {
			mods += "static ";
		} else if (is_abstract) {
			mods += "abstract ";
		} else if (is_virtual_method) {
			mods += is_override ? "override " : "virtual ";
		}

	    return mods;
	}
}

public class Generator {
	internal static bool IsBtouch;

	Dictionary<Type,IEnumerable<string>> selectors = new Dictionary<Type,IEnumerable<string>> ();
	Dictionary<Type,bool> need_static = new Dictionary<Type,bool> ();
	Dictionary<Type,bool> need_abstract = new Dictionary<Type,bool> ();
	Dictionary<string,int> selector_use = new Dictionary<string, int> ();
	Dictionary<string,string> selector_names = new Dictionary<string,string> ();
	Dictionary<string,string> send_methods = new Dictionary<string,string> ();
	Dictionary<string,string> original_methods = new Dictionary<string,string> ();
	List<MarshalType> marshal_types = new List<MarshalType> ();
	Dictionary<Type,TrampolineInfo> trampolines = new Dictionary<Type,TrampolineInfo> ();
	Dictionary<Type,int> trampolines_generic_versions = new Dictionary<Type,int> ();
	Dictionary<Type,Type> delegates_emitted = new Dictionary<Type, Type> ();
	Dictionary<Type,Type> notification_event_arg_types = new Dictionary<Type,Type> ();
	List <string> libraries = new List <string> ();

	List<Tuple<string, ParameterInfo[]>> async_result_types = new List<Tuple <string, ParameterInfo[]>> ();
	HashSet<string> async_result_types_emitted = new HashSet<string> ();

	//
	// This contains delegates that are referenced in the source and need to be generated.
	//
	Dictionary<string,MethodInfo> delegate_types = new Dictionary<string,MethodInfo> ();
	
	public bool Alpha;
	public bool OnlyX86;
	
	Type [] types;
	bool debug;
	bool external;
	StreamWriter sw, m;
	int indent;

	public class MarshalType {
		public Type Type;
		public string Encoding;
		public string ParameterMarshal;
		public string CreateFromRet;
		
		public MarshalType (Type t, string encode, string fetch, string create)
		{
			Type = t;
			Encoding = encode;
			ParameterMarshal = fetch;
			CreateFromRet = create;
		}
	}

	public bool LookupMarshal (Type t, out MarshalType res)
	{
		res = null;
		foreach (var mt in marshal_types){
			if (mt.Type == t){
				res = mt;
				return true;
			}
		}
		return false;
	}

	//
	// Properties and definitions to support binding third-party Objective-C libraries
	//
	string init_binding_type;

	// Where the assembly messaging is located (core)
	public string CoreMessagingNS = "MonoTouch.ObjCRuntime";

	// Whether to use ZeroCopy for strings, defaults to false
	public bool ZeroCopyStrings;
	
	// This can be plugged by the user when using btouch/bmac for their own bindings
	public string MessagingNS = "MonoTouch.ObjCRuntime";
	
	public bool BindThirdPartyLibrary = false;
	public bool InlineSelectors;
	public bool NativeExceptionMarshalling = false;
	public string BaseDir { get { return basedir; } set { basedir = value; }}
	string basedir;
	public List<string> GeneratedFiles = new List<string> ();
	public Type CoreNSObject = typeof (NSObject);
#if MONOMAC
	public Type MessagingType = typeof (MonoMac.ObjCRuntime.Messaging);
	public Type SampleBufferType = typeof (MonoMac.CoreMedia.CMSampleBuffer);
	string [] standard_namespaces = new string [] { "MonoMac.Foundation", "MonoMac.ObjCRuntime", "MonoMac.CoreGraphics" };
	const string MainPrefix = "MonoMac";
	const string CoreImageMap = "Quartz";
	string [] UINamespaces = new string [] {
		"MonoMac.AppKit"
	};
	string thread_check_call = "global::MonoMac.AppKit.NSApplication.EnsureUIThread ();";

#else
	public Type MessagingType = typeof (MonoTouch.ObjCRuntime.Messaging);
	public Type SampleBufferType = typeof (MonoTouch.CoreMedia.CMSampleBuffer);
	string [] standard_namespaces = new string [] { "MonoTouch.Foundation", "MonoTouch.ObjCRuntime", "MonoTouch.CoreGraphics" };
	const string MainPrefix = "MonoTouch";
	const string CoreImageMap = "CoreImage";
	string [] UINamespaces = new string [] {
		"MonoTouch.UIKit",
		"MonoTouch.Twitter",
		"MonoTouch.GameKit",
		"MonoTouch.NewsstandKit",
		"MonoTouch.iAd",
		"MonoTouch.QuickLook",
		"MonoTouch.EventKitUI",
		"MonoTouch.AddressBookUI",
		"MonoTouch.MapKit",
		"MonoTouch.MessageUI",
	};
	string thread_check_call = "global::MonoTouch.UIKit.UIApplication.EnsureUIThread ();";
#endif

	//
	// We inject thread checks to MonoTouch.UIKit types, unless there is a [ThreadSafe] attribuet on the type.
	// Set on every call to Generate
	//
	bool type_needs_thread_checks;

	//
	// If set, the members of this type will get zero copy
	// 
	internal bool type_wants_zero_copy;
	
	//
	// Used by the public binding generator to populate the
	// class with types that do not exist
	//
	public void RegisterMethodName (string method_name)
	{
		send_methods [method_name] = method_name;
		original_methods [method_name] = method_name;
	}

	//
	// Helpers
	//
	string MakeSig (MethodInfo mi, bool stret) { return MakeSig ("objc_msgSend", stret, mi); }
	string MakeSuperSig (MethodInfo mi, bool stret) { return MakeSig ("objc_msgSendSuper", stret, mi); }

	bool IsNativeType (Type pt)
	{
		return (pt == typeof (int) || pt == typeof (long) || pt == typeof (byte) || pt == typeof (short));
	}

	public string PrimitiveType (Type t, bool formatted = false)
	{
		if (t == typeof (void))
			return "void";

		if (t.IsEnum)
			t = Enum.GetUnderlyingType (t);
		
		if (t == typeof (int))
			return "int";
		if (t == typeof (short))
			return "short";
		if (t == typeof (byte))
			return "byte";
		if (t == typeof (float))
			return "float";
		if (t == typeof (bool))
			return "bool";

		return formatted ? FormatType (null, t) : t.Name;
	}

	// Is this a wrapped type of NSObject from the MonoTouch/MonoMac binding world?
	public bool IsWrappedType (Type t)
	{
		if (t.IsInterface) 
			return true;
		if (CoreNSObject != null)
			return t.IsSubclassOf (CoreNSObject) || t == CoreNSObject; 
		return false;
	}

	//
	// Returns the type that we use to marshal the given type as a string
	// for example "UIView" -> "IntPtr"
	string ParameterGetMarshalType (MarshalInfo mai, bool formatted = false)
	{
		if (mai.Type.IsEnum)
			return PrimitiveType (mai.Type, formatted);

		if (IsWrappedType (mai.Type))
			return "IntPtr";

		if (IsNativeType (mai.Type))
			return PrimitiveType (mai.Type, formatted);

		if (mai.Type == typeof (string)){
			if (mai.PlainString)
				return "string";

			// We will do NSString
			return "IntPtr";
		} 

		MarshalType mt;
		if (LookupMarshal (mai.Type, out mt))
			return mt.Encoding;
		
		if (mai.Type.IsValueType)
			return PrimitiveType (mai.Type, formatted);

		// Arrays are returned as NSArrays
		if (mai.Type.IsArray)
			return "IntPtr";

		//
		// Pass "out ValueType" directly
		//
		if (mai.Type.IsByRef && mai.Type.GetElementType ().IsValueType){
			Type elementType = mai.Type.GetElementType ();
			return "out " + (formatted ? FormatType (null, elementType) : elementType.Name);
		}

		if (mai.Type.IsSubclassOf (typeof (Delegate))){
			return "IntPtr";
		}

		if (mai.Type.IsSubclassOf (typeof (DictionaryContainerType))){
			return "IntPtr";
		}
		
		//
		// Edit the table in the "void Go ()" routine
		//
		
		if (mai.Type.IsByRef && mai.Type.GetElementType ().IsValueType == false)
			return "IntPtr";
		
		throw new BindingException (1017, true, "Do not know how to make a signature for {0}", mai.Type);
	}

	//
	// This probably should use MarshalInfo to find the correct way of turning
	// the native types into managed types instead of hardcoding the limited
	// values we know about here
	//
	public string MakeTrampoline (Type t)
	{
		if (trampolines.ContainsKey (t))
			return trampolines [t].StaticName;

		var mi = t.GetMethod ("Invoke");
		var pars = new StringBuilder ();
		var invoke = new StringBuilder ();
		var clear = new StringBuilder  ();
		string returntype;
		var returnformat = "return {0};";

		if (mi.ReturnType.IsArray && IsWrappedType (mi.ReturnType.GetElementType())) {
			returntype = "IntPtr";
			returnformat = "return NSArray.FromNSObjects({0}).Handle;";
		}
		else if (IsWrappedType (mi.ReturnType)) {
			returntype = "IntPtr";
			returnformat = "return {0} != null ? {0}.Handle : IntPtr.Zero;";
		} else {
			returntype = FormatType (mi.DeclaringType, mi.ReturnType);
		}
		
		pars.Append ("IntPtr block");
		var parameters = mi.GetParameters ();
		foreach (var pi in parameters){
			pars.Append (", ");
			if (pi != parameters [0])
				invoke.Append (", ");
			
			if (IsWrappedType (pi.ParameterType)){
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("Runtime.GetNSObject<{1}> ({0})", pi.Name, pi.ParameterType);
				continue;
			}

			if (pi.ParameterType.IsSubclassOf (typeof (INativeObject))){
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("new {0} ({1})", pi.ParameterType, pi.Name);
				continue;
			}

			if (pi.ParameterType.IsByRef){
				var nt = pi.ParameterType.GetElementType ();
				if (pi.IsOut){
					clear.AppendFormat ("{0} = {1};", pi.Name, nt.IsValueType ? "default (" + FormatType (null, nt) + ")" : "null");
				}
				if (nt.IsValueType){
					string marshal = string.Empty;
					if (nt == typeof (bool))
						marshal = "[System.Runtime.InteropServices.MarshalAs (System.Runtime.InteropServices.UnmanagedType.I1)] ";
					pars.AppendFormat ("{3}{0} {1} {2}", pi.IsOut ? "out" : "ref", FormatType (null, nt), pi.Name, marshal);
					invoke.AppendFormat ("{0} {1}", pi.IsOut ? "out" : "ref", pi.Name);
					continue;
				}
			} else if (pi.ParameterType.IsValueType){
				pars.AppendFormat ("{0} {1}", FormatType (null, pi.ParameterType), pi.Name);
				invoke.AppendFormat ("{0}", pi.Name);
				continue;
			}
		
			if (pi.ParameterType == typeof (string [])){
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("NSArray.StringArrayFromHandle ({0})", pi.Name);
				continue;
			}
			if (pi.ParameterType == typeof (string)){
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("NSString.FromHandle ({0})", pi.Name);
				continue;
			}
			if (pi.ParameterType == SampleBufferType){
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("new CMSampleBuffer ({0}, false)", pi.Name);
				continue;
			}

			if (pi.ParameterType == typeof (string [])){
				pars.AppendFormat ("string [] {0}", pi.Name);
				invoke.AppendFormat ("{0}", pi.Name);
				continue;
			}
			
			if (pi.ParameterType.IsArray){
				Type et = pi.ParameterType.GetElementType ();
				if (IsWrappedType (et)){
					pars.AppendFormat ("IntPtr {0}", pi.Name);
					invoke.AppendFormat ("NSArray.ArrayFromHandle<{0}> ({1})", FormatType (null, et), pi.Name);
					continue;
				}
			}

			if (pi.ParameterType.IsSubclassOf (typeof (Delegate))){
				if (!delegate_types.ContainsKey (pi.ParameterType.Name)){
					delegate_types [pi.ParameterType.FullName] = pi.ParameterType.GetMethod ("Invoke");
				}
				pars.AppendFormat ("IntPtr {0}", pi.Name);
				invoke.AppendFormat ("({0}) Marshal.GetDelegateForFunctionPointer ({1}, typeof ({0}))", pi.ParameterType, pi.Name);
				continue;
			}
			
			throw new BindingException (1001, true, "Do not know how to make a trampoline for {0}", pi);
		}

		var trampoline_name = t.Name.Replace ("`", "Arity");
		if (t.IsGenericType) {
			var gdef = t.GetGenericTypeDefinition ();

			if (!trampolines_generic_versions.ContainsKey (gdef))
				trampolines_generic_versions.Add (gdef, 0);

			trampoline_name = trampoline_name + "V" + trampolines_generic_versions [gdef]++;
		}

		var ti = new TrampolineInfo (FormatType (null, t),
					     "D" + trampoline_name,
					     "T" + trampoline_name,
					     pars.ToString (), invoke.ToString (),
					     returntype,
					     mi.ReturnType.ToString (),
					     returnformat, clear.ToString ());

		trampolines [t] = ti;
		return ti.StaticName;
	}
	
	//
	// Returns the actual way in which the type t must be marshalled
	// for example "UIView foo" is generated as  "foo.Handle"
	//
	public string MarshalParameter (MethodInfo mi, ParameterInfo pi, bool null_allowed_override)
	{
		if (pi.ParameterType.IsByRef && pi.ParameterType.GetElementType ().IsValueType == false){
			return pi.Name + "Ptr";
		}

		if (IsWrappedType (pi.ParameterType)){
			if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute)))
				return String.Format ("{0} == null ? IntPtr.Zero : {0}.Handle", pi.Name);
			return pi.Name + ".Handle";
		}
		
		if (pi.ParameterType.IsEnum){
			return "(" + PrimitiveType (pi.ParameterType) + ")" + pi.Name;
		}
		
		if (IsNativeType (pi.ParameterType))
			return pi.Name;

		if (pi.ParameterType == typeof (string)){
			var mai = new MarshalInfo (mi, pi);
			if (mai.PlainString)
				return pi.Name;
			else {
				bool allow_null = null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute));
				
				if (mai.ZeroCopyStringMarshal){
					if (allow_null)
						return String.Format ("{0} == null ? IntPtr.Zero : (IntPtr)(&_s{0})", pi.Name);
					else
						return String.Format ("(IntPtr)(&_s{0})", pi.Name);
				} else {
#if false
					if (allow_null)
						return String.Format ("ns{0} == null ? IntPtr.Zero : ns{0}.Handle", pi.Name);
					else 
						return "ns" + pi.Name + ".Handle";
#else
					return "ns" + pi.Name;
#endif
				}
			}
		}

		if (pi.ParameterType.IsValueType)
			return pi.Name;

		MarshalType mt;
		if (LookupMarshal (pi.ParameterType, out mt)){
			string access = String.Format (mt.ParameterMarshal, pi.Name);
			if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute)))
				return String.Format ("{0} == null ? IntPtr.Zero : {1}", pi.Name, access);
			return access;
		}

		if (pi.ParameterType.IsArray){
			//Type etype = pi.ParameterType.GetElementType ();

			if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute)))
				return String.Format ("nsa_{0} == null ? IntPtr.Zero : nsa_{0}.Handle", pi.Name);
			return "nsa_" + pi.Name + ".Handle";
		}

		//
		// Handle (out ValeuType foo)
		//
		if (pi.ParameterType.IsByRef && pi.ParameterType.GetElementType ().IsValueType){
			return "out " + pi.Name;
		}

		if (pi.ParameterType.IsSubclassOf (typeof (Delegate))){
			return String.Format ("(IntPtr) block_ptr_{0}", pi.Name);
		}

		if (pi.ParameterType.IsSubclassOf (typeof (DictionaryContainerType))){
			if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute)))
				return String.Format ("{0} == null ? IntPtr.Zero : {0}.Dictionary.Handle", pi.Name);
			return pi.Name + ".Dictionary.Handle";
		}
		
		throw new BindingException (1002, true, "Unknown kind {0} in method '{1}.{2}'", pi, mi.DeclaringType.FullName, mi	.Name);
	}

	public bool ParameterNeedsNullCheck (ParameterInfo pi, MethodInfo mi)
	{
		if (HasAttribute (pi, typeof (NullAllowedAttribute)))
			return false;

		if (mi.IsSpecialName && mi.Name.StartsWith ("set_")){
			if (HasAttribute (mi, typeof (NullAllowedAttribute))){
				return false;
			}
		}
		if (IsWrappedType (pi.ParameterType))
			return true;

		if (pi.ParameterType.IsArray)
			return true;
		
		if (pi.ParameterType == typeof (Selector))
			return true;
		
		if (pi.ParameterType == typeof (Class))
			return true;
		
		if (pi.ParameterType == typeof (NSObject))
			return true;
		
		if (IsNativeType (pi.ParameterType))
			return false;

		if (pi.ParameterType == typeof (string))
			return true;

		if (pi.ParameterType.IsSubclassOf (typeof (Delegate)))
			return true;
		return false;
	}

	public object GetAttribute (ICustomAttributeProvider mi, Type t)
	{
		object [] a = mi.GetCustomAttributes (t, true);
		if (a.Length > 0)
			return a [0];
		return null;
	}
	
	public static T GetAttribute<T> (ICustomAttributeProvider mi) where T: class
	{
		object [] a = mi.GetCustomAttributes (typeof (T), true);
		if (a.Length > 0)
			return (T) a [0];
		return null;
	}

	public BindAttribute GetBindAttribute (MethodInfo mi)
	{
		return GetAttribute (mi, typeof (BindAttribute)) as BindAttribute;
	}
	
	public static bool HasAttribute (ICustomAttributeProvider i, Type t)
	{
		return i.GetCustomAttributes (t, true).Length > 0;
	}

	public bool IsTarget (ParameterInfo pi)
	{
		return HasAttribute (pi, typeof (TargetAttribute)); 
	}
	
	//
	// Makes the method name for a objcSend call
	//
	string MakeSig (string send, bool stret, MethodInfo mi)
	{
		var sb = new StringBuilder ();
		
		if (NativeExceptionMarshalling && HasAttribute (mi, typeof (MarshalNativeExceptionsAttribute)))
#if MONOMAC
			sb.Append ("monomac_");
#else
			sb.Append ("monotouch_");
#endif
		
		try {
			sb.Append (ParameterGetMarshalType (mi));
		} catch (BindingException ex) {
			throw new BindingException (ex.Code, ex.Error, ex,  "{0} in method `{1}'", ex.Message, mi.Name);
		}

		sb.Append ("_");
		sb.Append (send);
		if (stret)
			sb.Append ("_stret");
		
		foreach (var pi in mi.GetParameters ()){
			if (IsTarget (pi))
				continue;
			sb.Append ("_");
			try {
				sb.Append (ParameterGetMarshalType (new MarshalInfo (mi, pi)).Replace (' ', '_'));
			} catch (BindingException ex) {
				throw new BindingException (ex.Code, ex.Error, ex, "{0} in parameter `{1}' from {2}.{3}", ex.Message, pi.Name, mi.DeclaringType, mi.Name);
			}
		}

		return sb.ToString ();
	}

	void RegisterMethod (bool need_stret, MethodInfo mi, string method_name)
	{
		if (send_methods.ContainsKey (method_name))
			return;
		send_methods [method_name] = method_name;

		var b = new StringBuilder ();
		int n = 0;
		
		foreach (var pi in mi.GetParameters ()){
			if (IsTarget (pi))
				continue;

			b.Append (", ");

			try {
				b.Append (ParameterGetMarshalType (new MarshalInfo (mi, pi), true));
			} catch (BindingException ex) {
				throw new BindingException (ex.Code, ex.Error, ex, "{0} in parameter {1} of {2}.{3}", ex.Message, pi.Name, mi.DeclaringType, mi.Name);
			}
			b.Append (" ");
			b.Append ("arg" + (++n));
		}

		string entry_point;
		if (method_name.IndexOf ("objc_msgSendSuper") != -1){
			entry_point = need_stret ? "objc_msgSendSuper_stret" : "objc_msgSendSuper";
		} else
			entry_point = need_stret ? "objc_msgSend_stret" : "objc_msgSend";

		//
		// These exist in Messaging.cs and we can not remove them from there
		//
		switch (method_name){
		case "void_objc_msgSend":
		case "void_objc_msgSendSuper":
			return;
		}

		if (method_name.StartsWith ("monotouch_") || method_name.StartsWith ("monomac_")) {
			print (m, "\t\t[DllImport (\"__Internal\", EntryPoint=\"{0}\")]", method_name);
		} else {
			print (m, "\t\t[DllImport (LIBOBJC_DYLIB, EntryPoint=\"{0}\")]", entry_point);
		}
		print (m, "\t\tpublic extern static {0} {1} ({3}IntPtr receiver, IntPtr selector{2});",
		       need_stret ? "void" : ParameterGetMarshalType (mi, true), method_name, b.ToString (),
		       need_stret ? (HasAttribute (mi, typeof (AlignAttribute)) ? "IntPtr" : "out " + FormatType (MessagingType, mi.ReturnType)) + " retval, " : "");
		       
	}

	bool ArmNeedStret (MethodInfo mi)
	{
		Type t = mi.ReturnType;

		if (!t.IsValueType || t.IsEnum || t.Assembly == typeof (object).Assembly)
			return false;

		return true;
	}

	bool X86NeedStret (MethodInfo mi)
	{
		Type t = mi.ReturnType;
		
		if (!t.IsValueType || t.IsEnum || t.Assembly == typeof (object).Assembly)
			return false;

#if MAC64
		return Marshal.SizeOf (t) > 16;
#else
		return Marshal.SizeOf (t) > 8;
#endif
	}

	bool NeedStret (MethodInfo mi)
	{
		return ArmNeedStret (mi) || X86NeedStret (mi); 
	}
		       
	
	void DeclareInvoker (MethodInfo mi)
	{
		bool arm_stret = ArmNeedStret (mi);
		try {
			RegisterMethod (arm_stret, mi, MakeSig (mi, arm_stret));
			RegisterMethod (arm_stret, mi, MakeSuperSig (mi, arm_stret));
			
			bool x86_stret = X86NeedStret (mi);
			if (x86_stret != arm_stret){
				RegisterMethod (x86_stret, mi, MakeSig (mi, x86_stret));
				RegisterMethod (x86_stret, mi, MakeSuperSig (mi, x86_stret));
			}
		} catch {
			Console.WriteLine ("   in Method: {0}", mi);
		}
	}
	static char [] invalid_selector_chars = new char [] { '*', '^', '(', ')' };
	
	//
	// Either we have an [Export] attribute, or we have a [Wrap] one
	//
	public static ExportAttribute GetExportAttribute (PropertyInfo pi, out string wrap)
	{
		wrap = null;
#if debug
		object [] jattrs = pi.GetCustomAttributes (true);
		Console.WriteLine ("On: {0}", pi);
		foreach (var x in jattrs){
			Console.WriteLine ("    -> {0} ", x);
			Console.WriteLine ("   On: {0} ", x.GetType ().Assembly);
			Console.WriteLine ("   Ex: {0}", typeof (ExportAttribute).Assembly);
		}
#endif
		object [] attrs = pi.GetCustomAttributes (typeof (ExportAttribute), true);
		if (attrs.Length == 0){
			attrs = pi.GetCustomAttributes (typeof (WrapAttribute), true);
			if (attrs.Length != 0){
				wrap = ((WrapAttribute) attrs [0]).MethodName;
				return null;
			}
			return null;
		}
		
		var export = (ExportAttribute) attrs [0];

		if (export.Selector.IndexOfAny (invalid_selector_chars) != -1){
			Console.Error.WriteLine ("Export attribute contains invalid selector name: {0}", export.Selector);
			Environment.Exit (1);
		}
		
		return export;
	}

	public ExportAttribute MakeSetAttribute (ExportAttribute source)
	{
		return new ExportAttribute ("set" + Char.ToUpper (source.Selector [0]) + source.Selector.Substring (1) + ":");
	}

	public static Generator SharedGenerator;
	
	public Generator (bool btouch, bool external, bool debug, Type [] types)
	{
		Generator.IsBtouch = btouch;
		this.external = external;
		this.debug = debug;
		this.types = types;
		basedir = ".";
		SharedGenerator = this;
	}

	public void Go ()
	{
		marshal_types.Add (new MarshalType (typeof (CGColor), "IntPtr", "{0}.Handle", "new CGColor ("));
		marshal_types.Add (new MarshalType (typeof (CGPath), "IntPtr", "{0}.Handle", "new CGPath ("));
		marshal_types.Add (new MarshalType (typeof (CGContext), "IntPtr", "{0}.Handle", "new CGContext ("));
		marshal_types.Add (new MarshalType (typeof (CGImage), "IntPtr", "{0}.Handle", "new CGImage ("));
		marshal_types.Add (new MarshalType (typeof (NSObject), "IntPtr", "{0}.Handle", "Runtime.GetNSObject ("));
		marshal_types.Add (new MarshalType (typeof (Selector), "IntPtr", "{0}.Handle", "Selector.FromHandle ("));
		marshal_types.Add (new MarshalType (typeof (Class), "IntPtr", "{0}.Handle", "new Class ("));
		marshal_types.Add (new MarshalType (typeof (NSString), "IntPtr", "{0}.Handle", "new NSString ("));
		marshal_types.Add (new MarshalType (typeof (CFRunLoop), "IntPtr", "{0}.Handle", "new CFRunLoop ("));
		marshal_types.Add (new MarshalType (typeof (CGColorSpace), "IntPtr", "{0}.Handle", "new CGColorSpace ("));
		marshal_types.Add (new MarshalType (typeof (DispatchQueue), "IntPtr", "{0}.Handle", "new DispatchQueue ("));
		marshal_types.Add (new MarshalType (typeof (MidiEndpoint), "IntPtr", "{0}.Handle", "new MidiEndpoint ("));
		marshal_types.Add (new MarshalType (typeof (CMTimebase), "IntPtr", "{0}.Handle", "new CMTimebase ("));
		marshal_types.Add (new MarshalType (typeof (CMClock), "IntPtr", "{0}.Handle", "new CMClock ("));
#if MONOMAC
		marshal_types.Add (new MarshalType (typeof (CGLContext), "IntPtr", "{0}.Handle", "new CGLContext ("));
		marshal_types.Add (new MarshalType (typeof (CGLPixelFormat), "IntPtr", "{0}.Handle", "new CGLPixelFormat ("));
		marshal_types.Add (new MarshalType (typeof (CVImageBuffer), "IntPtr", "{0}.Handle", "new CVImageBuffer ("));
#endif
		marshal_types.Add (new MarshalType (typeof (CVPixelBuffer), "IntPtr", "{0}.Handle", "new CVPixelBuffer ("));
		marshal_types.Add (new MarshalType (typeof (CGLayer), "IntPtr", "{0}.Handle", "new CGLayer ("));
#if MONOMAC
		marshal_types.Add (new MarshalType (typeof (MonoMac.CoreMedia.CMSampleBuffer), "IntPtr", "{0}.Handle", "new MonoMac.CoreMedia.CMSampleBuffer ("));
		marshal_types.Add (new MarshalType (typeof (MonoMac.CoreVideo.CVImageBuffer), "IntPtr", "{0}.Handle", "new MonoMac.CoreVideo.CMImageBuffer ("));
		marshal_types.Add (new MarshalType (typeof (MonoMac.CoreVideo.CVPixelBufferPool), "IntPtr", "{0}.Handle", "new MonoMac.CoreVideo.CVPixelBufferPool ("));
		marshal_types.Add (new MarshalType (typeof (MonoMac.CoreMedia.CMFormatDescription), "IntPtr", "{0}.Handle", "new MonoMac.CoreMedia.CMFormatDescription ("));
#else
		marshal_types.Add (new MarshalType (typeof (MTAudioProcessingTap), "IntPtr", "{0}.Handle", "new MonoTouch.MediaToolbox.MTAudioProcessingTap ("));
		marshal_types.Add (new MarshalType (typeof (MonoTouch.CoreMedia.CMSampleBuffer), "IntPtr", "{0}.Handle", "new MonoTouch.CoreMedia.CMSampleBuffer ("));
		marshal_types.Add (new MarshalType (typeof (MonoTouch.CoreVideo.CVImageBuffer), "IntPtr", "{0}.Handle", "new MonoTouch.CoreVideo.CMImageBuffer ("));
		marshal_types.Add (new MarshalType (typeof (MonoTouch.CoreVideo.CVPixelBufferPool), "IntPtr", "{0}.Handle", "new MonoTouch.CoreVideo.CVPixelBufferPool ("));			
		marshal_types.Add (new MarshalType (typeof (MonoTouch.CoreMedia.CMFormatDescription), "IntPtr", "{0}.Handle", "new MonoTouch.CoreMedia.CMFormatDescription ("));
#endif

		marshal_types.Add (new MarshalType (typeof (BlockLiteral), "BlockLiteral", "{0}", "THIS_IS_BROKEN"));

		init_binding_type = String.Format ("IsDirectBinding = GetType ().Assembly == global::{0}.Messaging.this_assembly;", MessagingNS);

		Directory.CreateDirectory (Path.Combine (basedir, "ObjCRuntime"));

		var messaging_cs = Path.Combine (basedir, "ObjCRuntime/Messaging.g.cs");
		GeneratedFiles.Add (messaging_cs);
		m = new StreamWriter (messaging_cs);
		Header (m);
		print (m, "namespace {0} {{", MessagingNS);
		print (m, "\tpublic partial class Messaging {");

		if (BindThirdPartyLibrary){
			print (m, "\t\tstatic internal System.Reflection.Assembly this_assembly = typeof (Messaging).Assembly;\n");
			print (m, "\t\tconst string LIBOBJC_DYLIB = \"/usr/lib/libobjc.dylib\";\n");
		}
		
		foreach (Type t in types){
			if (HasAttribute (t, typeof (AlphaAttribute)) && Alpha == false)
				continue;

			// We call lookup to build the hierarchy graph
			GeneratedType.Lookup (t);
			
			var tselectors = new List<string> ();
			
			foreach (var pi in GetTypeContractProperties (t)){
				if (HasAttribute (pi, typeof (AlphaAttribute)) && Alpha == false)
					continue;

				if (HasAttribute (pi, typeof (IsThreadStaticAttribute)) && !HasAttribute (pi, typeof (StaticAttribute)))
					throw new BindingException (1008, true, "[IsThreadStatic] is only valid on properties that are also [Static]");

				string wrapname;
				var export = GetExportAttribute (pi, out wrapname);
				if (export == null){
					if (wrapname != null)
						continue;

					// Let properties with the [Field] attribute through as well.
					var attrs = pi.GetCustomAttributes (typeof (FieldAttribute), true);
					if (attrs.Length != 0)
						continue;
					
					throw new BindingException (1018, true, "No [Export] attribute on property {0}.{1}", pi.DeclaringType, pi.Name);
				}
				if (HasAttribute (pi, typeof (StaticAttribute)))
					need_static [t] = true;

				bool is_abstract = HasAttribute (pi, typeof (AbstractAttribute)) && pi.DeclaringType == t;
				
				if (pi.CanRead){
					MethodInfo getter = pi.GetGetMethod ();
					BindAttribute ba = GetBindAttribute (getter);

					if (!is_abstract)
						tselectors.Add (ba != null ? ba.Selector : export.Selector);
					DeclareInvoker (getter);
				}
				
				if (pi.CanWrite){
					MethodInfo setter = pi.GetSetMethod ();
					BindAttribute ba = GetBindAttribute (setter);
					
					if (!is_abstract)
						tselectors.Add (ba != null ? ba.Selector : MakeSetAttribute (export).Selector);
					DeclareInvoker (setter);
				}
			}
			
			foreach (var mi in GetTypeContractMethods (t)){
				// Skip properties
				if (mi.IsSpecialName)
					continue;

				if (HasAttribute (mi, typeof (AlphaAttribute)) && Alpha == false)
					continue;

				bool seenAbstract = false;
				bool seenDefaultValue = false;
				bool seenNoDefaultValue = false;
				
				foreach (Attribute attr in mi.GetCustomAttributes (typeof (Attribute), true)){
					string selector = null;
					ExportAttribute ea = attr as ExportAttribute;
					BindAttribute ba = attr as BindAttribute;
					if (ea != null){
						selector = ea.Selector;
					} else if (ba != null){
						selector = ba.Selector;
					} else if (attr is StaticAttribute){
						need_static [t] = true;
						continue;
					} else if (attr is InternalAttribute || attr is ProtectedAttribute){
						continue;
					} else if (attr is NeedsAuditAttribute) {
						continue;
					} else if (attr is FactoryAttribute){
						continue;
					} else  if (attr is AbstractAttribute){
						if (mi.DeclaringType == t)
							need_abstract [t] = true;
						seenAbstract = true;
						continue;
					} else if (attr is DefaultValueAttribute || attr is DefaultValueFromArgumentAttribute) {
						seenDefaultValue = true;
						continue;
					} else if (attr is NoDefaultValueAttribute) {
						seenNoDefaultValue = true;
						continue;
					} else if (attr is SealedAttribute || attr is EventArgsAttribute || attr is DelegateNameAttribute || attr is EventNameAttribute || attr is ObsoleteAttribute || attr is AlphaAttribute || attr is NewAttribute || attr is SinceAttribute || attr is PostGetAttribute || attr is NullAllowedAttribute || attr is CheckDisposedAttribute || attr is SnippetAttribute || attr is AvailabilityBaseAttribute || attr is AppearanceAttribute || attr is ThreadSafeAttribute || attr is AutoreleaseAttribute || attr is EditorBrowsableAttribute || attr is AdviceAttribute)
						continue;
					else if (attr is MarshalNativeExceptionsAttribute)
						continue;
					else if (attr is WrapAttribute)
						continue;
					else if (attr is AsyncAttribute)
						continue;
					else 
						throw new BindingException (1007, true, "Unknown attribute {0} on {1}", attr.GetType (), t);

					if (selector == null)
						throw new BindingException (1009, true, "No selector specified for method `{0}.{1}'", mi.DeclaringType, mi.Name);
					
					tselectors.Add (selector);
					if (selector_use.ContainsKey (selector)){
						selector_use [selector]++;
					} else
						selector_use [selector] = 1;
				}

				if (seenNoDefaultValue && seenAbstract)
					throw new BindingException (1019, true, "Cannot use [NoDefaultValue] on abstract method `{0}.{1}'", mi.DeclaringType, mi.Name);
				else if (seenNoDefaultValue && seenDefaultValue)
					throw new BindingException (1019, true, "Cannot use both [NoDefaultValue] and [DefaultValue] on method `{0}.{1}'", mi.DeclaringType, mi.Name);

				DeclareInvoker (mi);
			}

			foreach (var pi in t.GatherProperties (BindingFlags.Instance | BindingFlags.Public)){
				if (HasAttribute (pi, typeof (AlphaAttribute)) && Alpha == false)
					continue;

				if (HasAttribute (pi, typeof (AbstractAttribute)) && pi.DeclaringType == t)
					need_abstract [t] = true;
			}
			
			selectors [t] = tselectors.Distinct ();
		}

		foreach (Type t in types){
			if (HasAttribute (t, typeof (AlphaAttribute)) && Alpha == false)
				continue;
			
			Generate (t);
		}

		//DumpChildren (0, GeneratedType.Lookup (typeof (NSObject)));
		
		print (m, "\t}\n}");
		m.Close ();

		// Generate the event arg mappings
		if (notification_event_arg_types.Count > 0)
			GenerateEventArgsFile ();

		if (delegate_types.Count > 0)
			GenerateIndirectDelegateFile ();

		if (trampolines.Count > 0)
			GenerateTrampolines ();

		if (libraries.Count > 0)
			GenerateLibraryHandles ();
	}

	static string GenerateNSValue (string propertyToCall)
	{
		return "using (var nsv = new NSValue (value))\n\treturn nsv." + propertyToCall + ";";
	}

	static string GenerateNSNumber (string cast, string propertyToCall)
	{
		return "using (var nsn = new NSNumber (value))\n\treturn " + cast + "nsn." + propertyToCall + ";";
	}

	void GenerateIndirectDelegateFile ()
	{
		var support_delegate_file = Path.Combine (basedir, "SupportDelegates.g.cs");
		GeneratedFiles.Add (support_delegate_file);
		sw = new StreamWriter (support_delegate_file);
		Header (sw);
		RenderDelegates (delegate_types);
		sw.Close ();
	}

	void GenerateTrampolines ()
	{
		var library_file = Path.Combine (basedir, "ObjCRuntime/Trampolines.g.cs");
		GeneratedFiles.Add (library_file);
		sw = new StreamWriter (library_file);
		
		Header (sw);
#if MONOMAC
		print ("namespace MonoMac {"); indent++;
#else
		print ("namespace MonoTouch {"); indent++;
#endif
		print ("");
		//print ("[CompilerGenerated]");
		print ("static class Trampolines {"); indent++;
		foreach (var ti in trampolines.Values) {
			print ("");
			print ("internal delegate {0} {1} ({2});", ti.ReturnType, ti.DelegateName, ti.Parameters);
			print ("");
			print ("static internal class {0} {{", ti.StaticName); indent++;
			print ("");
			print ("static internal readonly {0} Handler = {1};", ti.DelegateName, ti.TrampolineName);
			print ("");
			print ("[MonoPInvokeCallback (typeof ({0}))]", ti.DelegateName);
			print ("static unsafe {0} {1} ({2}) {{", ti.ReturnType, ti.TrampolineName, ti.Parameters);
			indent++;
			print ("var descriptor = (BlockLiteral *) block;");
			print ("var del = ({0}) (descriptor->global_handle != IntPtr.Zero ? GCHandle.FromIntPtr (descriptor->global_handle).Target : GCHandle.FromIntPtr (descriptor->local_handle).Target);", ti.UserDelegate);
			bool is_void = ti.ReturnType == "void";
			// FIXME: right now we only support 'null' when the delegate does not return a value
			// otherwise we will need to know the default value to be returned (likely uncommon)
			if (is_void) {
				print ("if (del != null)");
				indent++;
				print ("del ({0});", ti.Invoke);
				indent--;
				if (ti.Clear.Length > 0){
					print ("else");
					indent++;
					print (ti.Clear);
					indent--;
				}
			} else {
				print ("{0} retval = del ({1});", ti.DelegateReturnType, ti.Invoke);
				print (ti.ReturnFormat, "retval");
			}
			indent--;
			print ("}"); 
			indent--;
			print ("}");
		}
		indent--; print ("}");
		indent--; print ("}");
		sw.Close ();
	}
	
	void GenerateLibraryHandles ()
	{
		var library_file = Path.Combine (basedir, "ObjCRuntime/Libraries.g.cs");
		GeneratedFiles.Add (library_file);
		sw = new StreamWriter (library_file);
		
		Header (sw);
#if MONOMAC
		print ("namespace MonoMac {"); indent++;
#else
		print ("namespace MonoTouch {"); indent++;
#endif
		//print ("[CompilerGenerated]");
		print ("static class Libraries {"); indent++;
		foreach (string library_name in libraries) {
			print ("static public class {0} {{", library_name); indent++;
			if (BindThirdPartyLibrary && library_name == "__Internal") {
				print ("static public readonly IntPtr Handle = Dlfcn.dlopen (null, 0);");
			} else {
				print ("static public readonly IntPtr Handle = Dlfcn.dlopen (Constants.{0}Library, 0);", library_name);
			}
			indent--; print ("}");
		}
		indent--; print ("}");
		indent--; print ("}");
		sw.Close ();
	}

	void GenerateEventArgsFile ()
	{
		var event_args_file = Path.Combine (basedir, "ObjCRuntime/EventArgs.g.cs");
		GeneratedFiles.Add (event_args_file);
		sw = new StreamWriter (event_args_file);

		Header (sw);
		foreach (Type eventType in notification_event_arg_types.Keys){
			// Do not generate events for stuff with no arguments
			if (eventType == null)
				continue;
		
			print ("namespace {0} {{", eventType.Namespace); indent++;
			print ("public partial class {0} : NSNotificationEventArgs {{", eventType.Name); indent++;
			print ("public {0} (NSNotification notification) : base (notification) \n{{\n}}\n", eventType.Name);
			int i = 0;
			foreach (var prop in eventType.GetProperties (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)){
				var attrs = prop.GetCustomAttributes (typeof (ExportAttribute), true);
				if (attrs.Length == 0)
					throw new BindingException (1010, true, "No Export attribute on {0}.{1} property", eventType, prop.Name);

				var @internal = prop.GetCustomAttributes (typeof (InternalAttribute), true).FirstOrDefault ();
				var export = attrs [0] as ExportAttribute;
				var use_export_as_string_constant = export.ArgumentSemantic != ArgumentSemantic.None;
				var null_allowed = HasAttribute (prop, typeof (NullAllowedAttribute));
				var nullable_type = prop.PropertyType.IsValueType && null_allowed;
				var propertyType = prop.PropertyType;
				var propNamespace = prop.DeclaringType.Namespace;
				var probe_presence = HasAttribute (prop, typeof (ProbePresenceAttribute));

				string kn = "k" + (i++);
				if (use_export_as_string_constant){
					print ("{0} {1}{2} {3} {{\n\tget {{\n",
						@internal == null ? "public" : "internal",
						propertyType,
						nullable_type ? "?" : "",
						prop.Name);
					indent += 2;
					print ("IntPtr value;");
					print ("using (var str = new NSString (\"{0}\")){{", export.Selector);
					kn = "str.Handle";
					indent++;
				} else {
					var lib = propNamespace.Substring (propNamespace.IndexOf (".") + 1);
					print ("static IntPtr {0};", kn);
					print ("{0} {1}{2} {3} {{\n\tget {{\n",
						@internal == null ? "public" : "internal",
						propertyType,
						nullable_type ? "?" : "",
						prop.Name);
					indent += 2;
					if (BindThirdPartyLibrary)
						print ("IntPtr value; if ({0} == IntPtr.Zero)\n\t{0} = {1}.ObjCRuntime.Dlfcn.GetIntPtr (Libraries.__Internal.Handle, \"{2}\");", kn, MainPrefix, export.Selector);
					else
						print ("IntPtr value; if ({0} == IntPtr.Zero)\n\t{0} = {1}.ObjCRuntime.Dlfcn.GetIntPtr (Libraries.{2}.Handle, \"{3}\");", kn, MainPrefix, lib, export.Selector);
				}
				if (null_allowed || probe_presence){
					if (probe_presence)
						print ("if (Notification.UserInfo == null)\n\treturn false;");
					else
						print ("if (Notification.UserInfo == null)\n\treturn null;");
				}
				print ("value = Notification.UserInfo.LowlevelObjectForKey ({0});", kn);
				if (use_export_as_string_constant){
					indent--;
					print ("}");
				} else
					print ("");
						
				if (probe_presence)
					print ("return value != IntPtr.Zero;");
				else {
					if (null_allowed)
						print ("if (value == IntPtr.Zero)\n\treturn null;");
					else if (propertyType.IsArray)
						print ("if (value == IntPtr.Zero)\n\treturn new {0} [0];", RenderType (propertyType.GetElementType ()));
					else
						print ("if (value == IntPtr.Zero)\n\treturn default({0});", RenderType (propertyType));

					var fullname = propertyType.FullName;

					if (propertyType.IsArray && IsWrappedType (propertyType.GetElementType ())){
						print ("return NSArray.ArrayFromHandle<{0}> (value);", RenderType (propertyType.GetElementType ()));
					} else if (IsWrappedType (propertyType)){
						print ("return Runtime.GetNSObject<{0}> (value);", RenderType (propertyType));
					} else if (fullname == "System.Drawing.RectangleF")
						print (GenerateNSValue ("RectangleFValue"));
					else if (propertyType == typeof (double))
						print (GenerateNSNumber ("", "DoubleValue"));
					else if (propertyType == typeof (float))
						print (GenerateNSNumber ("", "FloatValue"));
					else if (fullname == "System.Drawing.PointF")
						print (GenerateNSValue ("PointFValue"));
					else if (fullname == "System.Drawing.SizeF")
						print (GenerateNSValue ("SizeFValue"));
					else if (propertyType == typeof (string))
						print ("return NSString.FromHandle (value);");
					else if (propertyType == typeof (NSString))
						print ("return new NSString (value);");
					else if (propertyType == typeof (string [])){
						print ("return NSArray.StringArrayFromHandle (value);");
					} else {
						Type underlying = propertyType.IsEnum ? Enum.GetUnderlyingType (propertyType) : propertyType;
						string cast = propertyType.IsEnum ? "(" + propertyType.FullName + ") " : "";
					
						if (underlying == typeof (int))
							print (GenerateNSNumber (cast, "Int32Value"));
						else if (underlying == typeof (long))
							print (GenerateNSNumber (cast, "Int64Value"));
						else if (underlying == typeof (short))
							print (GenerateNSNumber (cast, "Int16Value"));
						else if (underlying == typeof (byte))
							print (GenerateNSNumber (cast, "ByteValue"));
						else if (underlying == typeof (bool))
							print (GenerateNSNumber (cast, "BoolValue"));
						else
							throw new BindingException (1011, true, "Do not know how to extract type {0}/{1} from an NSDictionary", propertyType, underlying);
					}
				}
				indent -= 2;
				print ("\t}\n}\n");
			}

			indent--; print ("}");
			indent--; print ("}");
		}
		sw.Close ();
	}
	
		
	public void DumpChildren (int level, GeneratedType gt)
	{
		string prefix = new string ('\t', level);
		Console.WriteLine ("{2} {0} - {1}", gt.Type.Name, gt.ImplementsAppearance ? "APPEARANCE" : "", prefix);
		foreach (var c in (from s in gt.Children orderby s.Type.FullName select s))
			DumpChildren (level+1, c);
	}
	
	// this attribute allows the linker to be more clever in removing unused code in bindings - without risking breaking user code
	// only generate those for monotouch now since we can ensure they will be linked away before reaching the devices
	public void GeneratedCode (StreamWriter sw, int tabs)
	{
		for (int i=0; i < tabs; i++)
			sw.Write ('\t');
		//sw.WriteLine ("[CompilerGenerated]");
	}
	
	public void print_generated_code ()
	{
		GeneratedCode (sw, indent);
	}

	public void print (string format)
	{
		print (sw, format);
	}

	public void print (string format, params object [] args)
	{
		print (sw, format, args);
	}

	public void print (StreamWriter w, string format)
	{
		string[] lines = format.Split (new char [] { '\n' });
		string lwsp = new string ('\t', indent);
		
		for (int i = 0; i < lines.Length; i++)
			w.WriteLine (lwsp + lines[i]);
	}

	public void print (StreamWriter w, string format, params object [] args)
	{
		string[] lines = String.Format (format, args).Split (new char [] { '\n' });
		string lwsp = new string ('\t', indent);
		
		for (int i = 0; i < lines.Length; i++)
			w.WriteLine (lwsp + lines[i]);
	}

	public void print (StreamWriter w, IEnumerable e)
	{
		foreach (var a in e)
			w.WriteLine (a);
	}

	public void PrintPlatformAttributes (MemberInfo mi)
	{
		if (mi == null)
			return;

		var availabilityAttributes = mi.GetCustomAttributes<AvailabilityBaseAttribute> (false);
		foreach (var attr in availabilityAttributes)
		{
			var sb = new StringBuilder();
			sb.Append("[");
			sb.Append(attr is IntroducedAttribute ? "Introduced" : "Deprecated");
			sb.Append("(");
			var ver = attr.Version;
			sb.Append(ver.Build > 0 ? $"{ver.Major}, {ver.Minor}, {ver.Build}" : $"{ver.Major}, {ver.Minor}");
			if (!string.IsNullOrEmpty(attr.Message))
			sb.Append($", \"{attr.Message.Replace("\"", "\\\"")}\"");
			sb.Append(")]");
			print (sb.ToString());
		}
	}

	public string SelectorField (string s, bool ignore_inline_directive = false)
	{
		string name;
		
		if (InlineSelectors && !ignore_inline_directive)
			return "Selector.GetHandle (\"" + s + "\")";

		if (selector_names.TryGetValue (s, out name))
			return name;
		
		StringBuilder sb = new StringBuilder ();
		bool up = true;
		sb.Append ("sel");
		
		foreach (char c in s){
			if (up && c != ':'){
				sb.Append (Char.ToUpper (c));
				up = false;
			} else if (c == ':') {
				up = true;
			} else
				sb.Append (c);
		}
		// if we finish with a ':' then we could have another identical 
		// selector that does not finish with ':' and that would cause 
		// an error when compiling the generated code. 
		// See http://bugzilla.xamarin.com/show_bug.cgi?id=2626
		if (up)
			sb.Append ('_');
		if (!InlineSelectors)
			sb.Append ("Handle");
		name = sb.ToString ();
		selector_names [s] = name;
		return name;
	}

	public string FormatType (Type usedIn, Type type)
	{
		if (type == typeof (void))
			return "void";
		if (type == typeof (int))
			return "int";
		if (type == typeof (short))
			return "short";
		if (type == typeof (byte))
			return "byte";
		if (type == typeof (float))
			return "float";
		if (type == typeof (bool))
			return "bool";
		if (type == typeof (string))
			return "string";

		string tname;
		if ((usedIn != null && type.Namespace == usedIn.Namespace) || standard_namespaces.Contains (type.Namespace))
			tname = type.Name;
		else
			tname = "global::" + type.FullName;

		var targs = type.GetGenericArguments ();
		if (targs.Length > 0) {
			return RemoveArity (tname) + "<" + string.Join (", ", targs.Select (l => FormatType (usedIn, l)).ToArray ()) + ">";
		}

		return tname;
	}

	static string RemoveArity (string typeName)
	{
		var arity = typeName.IndexOf ('`');
		return arity > 0 ? typeName.Substring (0, arity) : typeName;
	}

	//
	// Makes the public signature for an exposed method
	//
	public string MakeSignature (MethodInfo mi, out bool ctor, Type category_class)
	{
		return MakeSignature (mi, out ctor, false, category_class, mi.GetParameters ());
	}

	public string GetAsyncName (MethodInfo mi)
	{
		var attr = GetAttribute<AsyncAttribute> (mi);
		if (attr.MethodName != null)
			return attr.MethodName;
		return mi.Name + "Async";
	}

	public string MakeSignature (MethodInfo mi, out bool ctor, bool is_async, Type category_class, ParameterInfo[] parameters)
	{
		StringBuilder sb = new StringBuilder ();
		ctor = mi.Name == "Constructor";
		string name =  ctor ? mi.DeclaringType.Name : is_async ? GetAsyncName (mi) : mi.Name;

		if (mi.Name == "AutocapitalizationType"){
		}
		if (!ctor && !is_async){
			sb.Append (FormatType (mi.DeclaringType, mi.ReturnType));
			sb.Append (" ");
		}
		sb.Append (name);
		sb.Append (" (");

		bool comma = false;
		if (category_class != null){
			sb.Append ("this ");
			Console.WriteLine ("Gto {0} and {1}", mi.DeclaringType, category_class);
			sb.Append (FormatType (mi.DeclaringType, category_class));
			sb.Append (" This");
			comma = true;
		}
		foreach (var pi in parameters){
			if (comma)
				sb.Append (", ");
			comma = true;

			// Format nicely the type, as succinctly as possible
			Type parType = pi.ParameterType;
			if (parType.IsByRef){
				string reftype = HasAttribute (pi, typeof (OutAttribute)) ? "out " : "ref ";
				sb.Append (reftype);
				parType = parType.GetElementType ();
			}		
			sb.Append (FormatType (mi.DeclaringType, parType));
			sb.Append (" ");
			sb.Append (pi.Name);
		}
		sb.Append (")");
		return sb.ToString ();
	}

	string [] implicit_ns = new string [] {
		"System", 
#if !COREFX
		"System.Drawing", 
#endif
		"System.Linq",
		"System.Runtime.CompilerServices",
		"System.Runtime.InteropServices",
		"System.Diagnostics",
		"System.ComponentModel",
		"System.Threading.Tasks",
#if MONOMAC
		"MonoMac",
		"MonoMac.CoreFoundation",
		"MonoMac.Foundation",
		"MonoMac.ObjCRuntime",
		"MonoMac.CoreGraphics",
		"MonoMac.CoreAnimation",
		"MonoMac.CoreLocation", 
		"MonoMac.QTKit",
		"MonoMac.CoreVideo",
		"MonoMac.CoreMedia",
		"MonoMac.OpenGL",
#else
		"MonoTouch",
		"MonoTouch.CoreFoundation",
		"MonoTouch.CoreMedia",
		"MonoTouch.CoreMotion",
		"MonoTouch.Foundation", 
		"MonoTouch.ObjCRuntime", 
		"MonoTouch.CoreAnimation", 
		"MonoTouch.CoreLocation", 
		"MonoTouch.MapKit", 
		"MonoTouch.UIKit",
		"MonoTouch.CoreGraphics",
		"MonoTouch.NewsstandKit",
		"MonoTouch.GLKit",
		"MonoTouch.CoreVideo",
		"OpenTK"
#endif
	};
		
	void Header (StreamWriter w)
	{
		print (w, "//\n// Auto-generated from generator.cs, do not edit\n//");
		print (w, "// We keep references to objects, so warning 414 is expected\n");
		print (w, "#pragma warning disable 414\n");
		print (w, from ns in implicit_ns select "using " + ns + ";");
		print (w, "");
	}

	void GenerateInvoke (bool stret, bool supercall, MethodInfo mi, string selector, string args, bool assign_to_temp, bool is_static, Type category_type)
	{
		string target_name = category_type == null ? "this" : "This";
		string handle = supercall ? ".SuperHandle" : ".Handle";
		
		// If we have supercall == false, we can be a Bind methdo that has a [Target]
		if (supercall == false && !is_static){
			foreach (var pi in mi.GetParameters ()){
				if (IsTarget (pi)){
					if (pi.ParameterType == typeof (string)){
						var mai = new MarshalInfo (mi, pi);
						
						if (mai.PlainString)
							ErrorHelper.Show (new BindingException (1101, false, "Trying to use a string as a [Target]"));

						if (mai.ZeroCopyStringMarshal){
							target_name = "(IntPtr)(&_s" + pi.Name + ")";
							handle = "";
						} else {
							target_name = "ns" + pi.Name;
							handle = "";
						}
					} else
						target_name = pi.Name;
					break;
				}
			}
		}
		
		string sig = supercall ? MakeSuperSig (mi, stret) : MakeSig (mi, stret);

		// If we had this registered, it means that it existed in monotouch.dll, so fully namespace it
		if (original_methods.ContainsKey (sig))
			sig = CoreMessagingNS + ".Messaging." + sig;
		else
			sig = MessagingNS + ".Messaging." + sig;
		
		if (stret){
			string ret_val = HasAttribute (mi, typeof (AlignAttribute)) ? "ret" : "out ret";
			if (is_static)
				print ("{0} ({5}, class_ptr, {3}{4});", sig, "/*unusued*/", "/*unusued*/", selector, args, ret_val);
			else
				print ("{0} ({5}, {1}{2}, {3}{4});", sig, target_name, handle, selector, args, ret_val);
		} else {
			bool returns = mi.ReturnType != typeof (void) && mi.Name != "Constructor";

			string cast_a = "", cast_b = "";
			if (returns){
				MarshalInfo mai = new MarshalInfo (mi);
				MarshalType mt;
				
				if (mi.ReturnType.IsEnum){
					cast_a = "(" + FormatType (mi.DeclaringType, mi.ReturnType) + ") ";
					cast_b = "";
				} else if (IsWrappedType (mi.ReturnType)){
					cast_a = "Runtime.GetNSObject<" + FormatType (mi.DeclaringType, mi.ReturnType) + "> (";
					cast_b = ")";
				} else if (mai.Type == typeof (string) && !mai.PlainString){
					cast_a = "NSString.FromHandle (";
					cast_b = ")";
				} else if (mi.ReturnType.IsSubclassOf (typeof (Delegate))){
					cast_a = "(BlockLiteral *)";
					cast_b = "";
				} else if (LookupMarshal (mai.Type, out mt)){
					cast_a = mt.CreateFromRet;
					cast_b = ")";
				} else if (mai.Type.IsArray){
					Type etype = mai.Type.GetElementType ();
					if (etype == typeof (string)){
						cast_a = "NSArray.StringArrayFromHandle (";
						cast_b = ")";
					} else {
						cast_a = "NSArray.ArrayFromHandle<" + etype + ">(";
						cast_b = ")";
					}
				}
			} else if (mi.Name == "Constructor") {
				cast_a = "Handle = ";
			}

			if (is_static)
				print ("{0}{1}{2} (class_ptr, {5}{6}){7};",
				       returns ? (assign_to_temp ? "ret = " : "return ") : "",
				       cast_a, sig, target_name, 
				       "/*unusued3*/", //supercall ? "Super" : "",
				       selector, args, cast_b);
			else
				print ("{0}{1}{2} ({3}{4}, {5}{6}){7};",
				       returns ? (assign_to_temp ? "ret = " : "return ") : "",
				       cast_a, sig, target_name,
				       handle,
				       selector, args, cast_b);
		}
	}
	
	void GenerateInvoke (bool supercall, MethodInfo mi, string selector, string args, bool assign_to_temp, bool is_static, Type category_type)
	{
		bool arm_stret = ArmNeedStret (mi);
		bool x86_stret = X86NeedStret (mi);

		if (OnlyX86){
			GenerateInvoke (x86_stret, supercall, mi, selector, args, assign_to_temp, is_static, category_type);
			return;
		}
		
		bool need_two_paths = arm_stret != x86_stret;
		if (need_two_paths){
			print ("if (Runtime.Arch == Arch.DEVICE){");
			indent++;
			GenerateInvoke (arm_stret, supercall, mi, selector, args, assign_to_temp, is_static, category_type);
			indent--;
			print ("} else {");
			indent++;
			GenerateInvoke (x86_stret, supercall, mi, selector, args, assign_to_temp, is_static, category_type);
			indent--;
			print ("}");
		} else {
			GenerateInvoke (arm_stret, supercall, mi, selector, args, assign_to_temp, is_static, category_type);
		}
	}

	static char [] newlineTab = new char [] { '\n', '\t' };
	
	void Inject (MethodInfo method, Type snippetType)
	{
		var snippets = method.GetCustomAttributes (snippetType, false);
		if (snippets.Length == 0)
			return;
		foreach (SnippetAttribute snippet in snippets)
			Inject (snippet);
	}

	void Inject (SnippetAttribute snippet)
	{
		if (snippet.Code == null)
			return;
		var lines = snippet.Code.Split (newlineTab);
		foreach (var l in lines){
			if (l.Length == 0)
				continue;
			print (l);
		}
	}
	
	[Flags]
	public enum BodyOption {
		None = 0x0,
		NeedsTempReturn = 0x1,
		CondStoreRet    = 0x3,
		MarkRetDirty    = 0x5,
		StoreRet        = 0x7,
	}

	public enum ThreadCheck {
		Off,
		On,
	}
	
	//
	// generates the code to marshal a string from C# to Objective-C:
	//
	// @probe_null: determines whether null is allowed, and
	// whether we need to generate code to handle this
	//
	// @must_copy: determines whether to create a new NSString, necessary
	// for NSString properties that are flagged with "retain" instead of "copy"
	//
	// @prefix: prefix to prepend on each line
	//
	// @property: the name of the property
	//
	public string GenerateMarshalString (bool probe_null, bool must_copy)
	{
		if (must_copy){
#if false
			if (probe_null)
				return "var ns{0} = {0} == null ? null : new NSString ({0});\n";
			else
				return "var ns{0} = new NSString ({0});\n";
#else
			return "var ns{0} = NSString.CreateNative ({0});\n";
#endif
		}
		return
			CoreMessagingNS + ".NSStringStruct _s{0}; Console.WriteLine (\"" + CurrentMethod + ": Marshalling: {{0}}\", {0}); \n" +
			"_s{0}.ClassPtr = " + CoreMessagingNS + ".NSStringStruct.ReferencePtr;\n" +
			"_s{0}.Flags = 0x010007d1; // RefCount=1, Unicode, InlineContents = 0, DontFreeContents\n" +
			"_s{0}.UnicodePtr = _p{0};\n" + 
			"_s{0}.Length = " + (probe_null ? "{0} == null ? 0 : {0}.Length;" : "{0}.Length;\n");
	}
	
	public string GenerateDisposeString (bool probe_null, bool must_copy)
	{
		if (must_copy){
#if false
			if (probe_null)
				return "if (ns{0} != null)\n" + "\tns{0}.Dispose ();";
			else
				return "ns{0}.Dispose ();\n";
#else
			return "NSString.ReleaseNative (ns{0});\n";
#endif
		} else 
			return "if (_s{0}.Flags != 0x010007d1) throw new Exception (\"String was retained, not copied\");";
	}

	List<string> CollectFastStringMarshalParameters (MethodInfo mi)
	{
		List<string> stringParameters = null;
		
		foreach (var pi in mi.GetParameters ()){
 			var mai = new MarshalInfo (mi, pi);

 			if (mai.ZeroCopyStringMarshal){
 				if (stringParameters == null)
 					stringParameters = new List<string>();
 				stringParameters.Add (pi.Name);
 			}
 		}
		return stringParameters;
	}

	SinceAttribute GetSince (Type type, string methodName)
	{
		if (type == null)
			return null;

		var props = type.GetProperties ();
		foreach (var pi in props) {
			if (pi.Name != methodName)
				continue;
			var attrs = pi.GetCustomAttributes (typeof (SinceAttribute), false);
			return (attrs.Length == 0) ? null : (SinceAttribute) attrs [0];
		}
		return GetSince (ReflectionExtensions.GetBaseType (type), methodName);
	}

	SinceAttribute GetSince (MethodInfo mi, PropertyInfo pi)
	{
		var attrs = mi.GetCustomAttributes (typeof (SinceAttribute), false);
		if ((attrs.Length == 0) && (pi != null)) {
			attrs = pi.GetCustomAttributes (typeof (SinceAttribute), false);
		}
		return (attrs.Length == 0) ? null : (SinceAttribute) attrs [0];
	}

#if !MONOMAC
	// undecorated code is assumed to be iOS 2.0
	static SinceAttribute SinceDefault = new SinceAttribute (2,0);
#endif

	string CurrentMethod;
	
	//
	// The NullAllowed can be applied on a property, to avoid the ugly syntax, we allow it on the property
	// So we need to pass this as `null_allowed_override',   This should only be used by setters.
	//
	public void GenerateMethodBody (Type type, MemberInformation minfo, MethodInfo mi, string sel, bool null_allowed_override, string var_name, BodyOption body_options, PropertyInfo propInfo = null, bool is_appearance = false, Type category_type = null)
	{
		CurrentMethod = String.Format ("{0}.{1}", type.Name, mi.Name);

		bool needs_thread_check = type_needs_thread_checks && minfo.threadCheck == ThreadCheck.On;
		string selector = SelectorField (sel);
		var args = new StringBuilder ();
		var convs = new StringBuilder ();
		var disposes = new StringBuilder ();
		var byRefPostProcessing = new StringBuilder();

		indent++;

		if (needs_thread_check)
			print (thread_check_call);
		
		Inject (mi, typeof (PrologueSnippetAttribute));

		// Collect all strings that can be fast-marshalled
		List<string> stringParameters = CollectFastStringMarshalParameters (mi);
		
		foreach (var pi in mi.GetParameters ()){
			MarshalInfo mai = new MarshalInfo (mi, pi);

			if (!IsTarget (pi)){
				// Construct invocation
				args.Append (", ");
				args.Append (MarshalParameter (mi, pi, null_allowed_override));
			}

			// Construct conversions
			if (mai.Type == typeof (string) && !mai.PlainString){
				bool probe_null = null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute));

				convs.AppendFormat (GenerateMarshalString (probe_null, !mai.ZeroCopyStringMarshal), pi.Name);
				disposes.AppendFormat (GenerateDisposeString (probe_null, !mai.ZeroCopyStringMarshal), pi.Name);
			} else if (mai.Type.IsArray){
				Type etype = mai.Type.GetElementType ();
				if (etype == typeof (string)){
					if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute))){
						convs.AppendFormat ("var nsa_{0} = {0} == null ? null : NSArray.FromStrings ({0});\n", pi.Name);
						disposes.AppendFormat ("if (nsa_{0} != null)\n\tnsa_{0}.Dispose ();\n", pi.Name);
					} else {
						convs.AppendFormat ("var nsa_{0} = NSArray.FromStrings ({0});\n", pi.Name);
						disposes.AppendFormat ("nsa_{0}.Dispose ();\n", pi.Name);
					}
				} else {
					if (null_allowed_override || HasAttribute (pi, typeof (NullAllowedAttribute))){
						convs.AppendFormat ("var nsa_{0} = {0} == null ? null : NSArray.FromNSObjects ({0});\n", pi.Name);
						disposes.AppendFormat ("if (nsa_{0} != null)\n\tnsa_{0}.Dispose ();\n", pi.Name);
					} else {
						convs.AppendFormat ("var nsa_{0} = NSArray.FromNSObjects ({0});\n", pi.Name);
						disposes.AppendFormat ("nsa_{0}.Dispose ();\n", pi.Name);
					}
				}
			} else if (mai.Type.IsSubclassOf (typeof (Delegate))){
				string trampoline_name = MakeTrampoline (pi.ParameterType);
				string extra = "";
				bool null_allowed = HasAttribute (pi, typeof (NullAllowedAttribute));
				
				convs.AppendFormat ("BlockLiteral *block_ptr_{0};\n", pi.Name);
				convs.AppendFormat ("BlockLiteral block_{0};\n", pi.Name);
				if (null_allowed){
					convs.AppendFormat ("if ({0} == null){{\n", pi.Name);
					convs.AppendFormat ("\tblock_ptr_{0} = null;\n", pi.Name);
					convs.AppendFormat ("}} else {{\n");
					extra = "\t";
				}
				convs.AppendFormat (extra + "block_{0} = new BlockLiteral ();\n", pi.Name);
				convs.AppendFormat (extra + "block_ptr_{0} = &block_{0};\n", pi.Name);
				convs.AppendFormat (extra + "block_{0}.SetupBlock (Trampolines.{1}.Handler, {0});\n", pi.Name, trampoline_name);
				if (null_allowed)
					convs.AppendFormat ("}}");

				if (null_allowed){
					disposes.AppendFormat ("if (block_ptr_{0} != null)\n", pi.Name);
				}
				disposes.AppendFormat (extra + "block_ptr_{0}->CleanupBlock ();\n", pi.Name);
			} else {
				if (mai.Type.IsClass && !mai.Type.IsByRef && 
					(mai.Type != typeof (Selector) && mai.Type != typeof (Class) && mai.Type != typeof (string) && !typeof(INativeObject).IsAssignableFrom (mai.Type)))
					throw new BindingException (1020, true, "Unsupported type {0} used on exported method {1}.{2}'", mai.Type, mi.DeclaringType, mi.Name);
			}

			// Handle ByRef
			if (mai.Type.IsByRef && mai.Type.GetElementType ().IsValueType == false){
				print ("IntPtr {0}Ptr = Marshal.AllocHGlobal(4);", pi.Name);
				print ("Marshal.WriteInt32({0}Ptr, 0);", pi.Name);
				print ("");
				
				byRefPostProcessing.AppendLine();
				byRefPostProcessing.AppendFormat("IntPtr {0}Value = Marshal.ReadIntPtr({0}Ptr);", pi.Name);
				byRefPostProcessing.AppendLine();
				if (mai.Type.GetElementType () == typeof (string)){
					byRefPostProcessing.AppendFormat("{0} = {0}Value != IntPtr.Zero ? NSString.FromHandle ({0}Value) : null;", pi.Name, mai.Type.Name.Replace("&", ""));
				} else {
					byRefPostProcessing.AppendFormat("{0} = {0}Value != IntPtr.Zero ? Runtime.GetNSObject<{1}>({0}Value) : null;", pi.Name, mai.Type.Name.Replace("&", ""));
				}
				byRefPostProcessing.AppendLine();
				byRefPostProcessing.AppendFormat("Marshal.FreeHGlobal({0}Ptr);", pi.Name);
				byRefPostProcessing.AppendLine();
			}
			// Insert parameter checking
			else if (!null_allowed_override && ParameterNeedsNullCheck (pi, mi)){
				print ("if ({0} == null)", pi.Name);
				print ("\tthrow new ArgumentNullException (\"{0}\");", pi.Name);
			}
		}

		foreach (var pi in mi.GetParameters ()) {
			RetainAttribute [] attr = (RetainAttribute []) pi.GetCustomAttributes (typeof (RetainAttribute), true);
			var ra = attr.Length > 0 ? attr[0] : null;
			if (ra != null) {
				if (!string.IsNullOrEmpty (ra.WrapName))
					print ("__mt_{0}_var = {1};", ra.WrapName, pi.Name);
				else
					print ("__mt_{1}_{2} = {2};", pi.ParameterType, mi.Name, pi.Name);
			}
			RetainListAttribute [] lattr = (RetainListAttribute []) pi.GetCustomAttributes (typeof (RetainListAttribute), true);
			var rla = lattr.Length > 0 ? lattr[0] : null;
			if (rla != null) {
				if (rla.Add)
					print ("__mt_{0}_var.Add ({1});", rla.WrapName, pi.Name);
				else
					print ("__mt_{0}_var.Remove ({1});", rla.WrapName, pi.Name);
			}
		}

 		if (stringParameters != null){
 			print ("fixed (char * {0}){{",
 			       stringParameters.Select (name => "_p" + name + " = " + name).Aggregate ((first,second) => first + ", " + second));
 			indent++;
 		}

		if (convs.Length > 0)
			print (sw, convs.ToString ());

		Inject (mi, typeof (PreSnippetAttribute));
		AlignAttribute align = GetAttribute (mi, typeof (AlignAttribute)) as AlignAttribute;

		PostGetAttribute [] postget = null;
		// [PostGet] are not needed (and might not be available) when generating methods inside Appearance types
		// However we want them even if ImplementsAppearance is true (i.e. the original type needs them)
		if (!is_appearance) {
			if (HasAttribute (mi, typeof (PostGetAttribute)))
				postget = ((PostGetAttribute []) mi.GetCustomAttributes (typeof (PostGetAttribute), true));
			else if (propInfo != null)
				postget = ((PostGetAttribute []) propInfo.GetCustomAttributes (typeof (PostGetAttribute), true));

			if (postget != null && postget.Length == 0)
				postget = null;
		}

		bool release_return = HasAttribute (mi.ReturnTypeCustomAttributes, typeof (ReleaseAttribute));
		bool use_temp_return  =
			release_return ||
			(mi.Name != "Constructor" && (NeedStret (mi) || disposes.Length > 0 || postget != null) && mi.ReturnType != typeof (void)) ||
			(HasAttribute (mi, typeof (FactoryAttribute))) ||
			((body_options & BodyOption.NeedsTempReturn) == BodyOption.NeedsTempReturn) ||
			(mi.ReturnType.IsSubclassOf (typeof (Delegate))) ||
			(HasAttribute (mi.ReturnTypeCustomAttributes, typeof (ProxyAttribute))) ||
			(mi.Name != "Constructor" && byRefPostProcessing.Length > 0 && mi.ReturnType != typeof (void));

		if (use_temp_return) {
			if (mi.ReturnType.IsSubclassOf (typeof (Delegate)))
				print ("BlockLiteral *ret;");
			else if (align != null) {
				print ("{0} ret_real;", FormatType (mi.DeclaringType, mi.ReturnType));
				print ("IntPtr ret_alloced = Marshal.AllocHGlobal (sizeof ({0}) + {1});", FormatType (mi.DeclaringType, mi.ReturnType), align.Align);
				print ("IntPtr ret = new IntPtr (((ret_alloced.ToInt32 () + {0}) >> {1}) << {1});", align.Align - 1, align.Bits);
			} else
				print ("{0} ret;", FormatType (mi.DeclaringType, mi.ReturnType)); //  = new {0} ();"
		}
		
		bool needs_temp = use_temp_return || disposes.Length > 0;
		if (minfo.is_virtual_method || mi.Name == "Constructor"){
			//print ("if (this.GetType () == typeof ({0})) {{", type.Name);
			if (external) {
				GenerateInvoke (false, mi, selector, args.ToString (), needs_temp, minfo.is_static, category_type);
			} else {
				if (BindThirdPartyLibrary && mi.Name == "Constructor"){
					print (init_binding_type);
				}
				
				var may_throw = NativeExceptionMarshalling && HasAttribute (mi, typeof (MarshalNativeExceptionsAttribute));
				var null_handle = may_throw && mi.Name == "Constructor";
				if (null_handle) {
					print ("try {");
					indent++;
				}
				
				print ("if (IsDirectBinding) {{", type.Name);
				indent++;
				GenerateInvoke (false, mi, selector, args.ToString (), needs_temp, minfo.is_static, category_type);
				indent--;
				print ("} else {");
				indent++;
				GenerateInvoke (true, mi, selector, args.ToString (), needs_temp, minfo.is_static, category_type);
				indent--;
				print ("}");
				
				if (null_handle) {
					indent--;
					print ("} catch {");
					indent++;
					print ("Handle = IntPtr.Zero;");
					print ("throw;");
					indent--;
					print ("}");
				}
			}
		} else {
			GenerateInvoke (false, mi, selector, args.ToString (), needs_temp, minfo.is_static, category_type);
		}
		
		if (release_return)
			print ("Messaging.void_objc_msgSend (ret.Handle, Selector.GetHandle (Selector.Release));");
		
		Inject (mi, typeof (PostSnippetAttribute));

		if (disposes.Length > 0)
			print (sw, disposes.ToString ());
		if ((body_options & BodyOption.StoreRet) == BodyOption.StoreRet) {
			print ("{0} = ret;", var_name);
		} else if ((body_options & BodyOption.CondStoreRet) == BodyOption.CondStoreRet) {
#if !MONOMAC
			print ("if (!IsNewRefcountEnabled ())");
#endif
			print ("\t{0} = ret;", var_name);
		} else if ((body_options & BodyOption.MarkRetDirty) == BodyOption.MarkRetDirty) {
#if !MONOMAC
			print ("MarkDirty ();");
#endif
			print ("{0} = ret;", var_name);
		}

		if ((postget != null) && (postget.Length > 0)) {
			print ("#pragma warning disable 168");
			for (int i = 0; i < postget.Length; i++) {
#if !MONOMAC
				// bug #7742: if this code, e.g. existing in iOS 2.0, 
				// tries to call a property available since iOS 5.0, 
				// then it will fail when executing in iOS 4.3
				bool version_check = false;
				SinceAttribute postget_since = GetSince (type, postget [i].MethodName);
				if (postget_since != null) {
					SinceAttribute caller_since = GetSince (mi, propInfo) ?? SinceDefault;
					if ((caller_since.Major < postget_since.Major) || ((caller_since.Major == postget_since.Major) && (caller_since.Minor < postget_since.Minor))) {
						version_check = true;
						print ("var postget{0} = MonoTouch.UIKit.UIDevice.CurrentDevice.CheckSystemVersion ({1},{2}) ? {3} : null;", i, postget_since.Major, postget_since.Minor, postget [i].MethodName);
					}
				}
				if (!version_check)
#endif
					print ("var postget{0} = {1};", i, postget [i].MethodName);
			}
			print ("#pragma warning restore 168");
		}
		
		if (HasAttribute (mi, typeof (FactoryAttribute)))
			print ("ret.Release (); // Release implicit ref taken by GetNSObject");
		if (byRefPostProcessing.Length > 0)
			print (sw, byRefPostProcessing.ToString ());
		if (use_temp_return) {
			if (HasAttribute (mi.ReturnTypeCustomAttributes, typeof (ProxyAttribute)))
				print ("ret.SetAsProxy ();");

			if (mi.ReturnType.IsSubclassOf (typeof (Delegate))) {
				// BlockLiteral *ret; -> could be null and result in a NRE, e.g. the 'completionBlock' selector
				print ("if (ret == null)");
				indent++;
				print ("return null;");
				indent--;
				print ("return ({0}) (ret->global_handle != IntPtr.Zero ? GCHandle.FromIntPtr (ret->global_handle).Target : GCHandle.FromIntPtr (ret->local_handle).Target);", FormatType (mi.DeclaringType, mi.ReturnType));
			} else if (align != null) {
				print ("unsafe {{ ret_real = *({0} *) ret; }}", FormatType (mi.DeclaringType, mi.ReturnType));
				print ("Marshal.FreeHGlobal (ret_alloced);");
				print ("return ret_real;");
			} else {
				print ("return ret;");
			}
		}
		if (stringParameters != null){
			indent--;
			print ("}");
		}
		indent--;
	}

	public IEnumerable<MethodInfo> GetTypeContractMethods (Type source)
	{
		foreach (var method in source.GatherMethods (BindingFlags.Public | BindingFlags.Instance))
			yield return method;
		foreach (var parent in source.GetInterfaces ()){
			foreach (var method in parent.GatherMethods (BindingFlags.Public | BindingFlags.Instance))
				yield return method;
		}
	}

	public IEnumerable<PropertyInfo> GetTypeContractProperties (Type source)
	{
		foreach (var prop in source.GatherProperties ())
			yield return prop;
		foreach (var parent in source.GetInterfaces ()){
			foreach (var prop in parent.GatherProperties ())
				yield return prop;
		}
	}

	//
	// This is used to determine if the memberType is in the declaring type or in any of the
	// inherited versions of the type.   We use this now, since we support inlining protocols
	//
	public static bool MemberBelongsToType (Type memberType, Type hostType)
	{
		if (memberType == hostType)
			return true;
		foreach (var p in hostType.GetInterfaces ())
			if (memberType == p)
				return true;
		return false;
	}
	
	Dictionary<string,object> generatedEvents = new Dictionary<string,object> ();
	Dictionary<string,object> generatedDelegates = new Dictionary<string,object> ();

	bool DoesTypeNeedBackingField (Type type) {
		return IsWrappedType (type) || (type.IsArray && IsWrappedType (type.GetElementType ()));
	}

	bool DoesPropertyNeedBackingField (PropertyInfo pi) {
		return DoesTypeNeedBackingField (pi.PropertyType) && !HasAttribute (pi, typeof (TransientAttribute));
	}
	
	bool DoesPropertyNeedDirtyCheck (PropertyInfo pi, ExportAttribute ea) {
		bool needs_ref = ea.ArgumentSemantic == ArgumentSemantic.Copy || ea.ArgumentSemantic == ArgumentSemantic.Retain;
		return DoesPropertyNeedBackingField (pi) && needs_ref;		
	}

	void PrintPropertyAttributes (PropertyInfo pi)
	{
		foreach (ObsoleteAttribute oa in pi.GetCustomAttributes (typeof (ObsoleteAttribute), false)) 
			print ("[Obsolete (\"{0}\", {1})]", oa.Message, oa.IsError ? "true" : "false");

#if !COREFX
		foreach (DebuggerBrowsableAttribute ba in pi.GetCustomAttributes (typeof (DebuggerBrowsableAttribute), false)) 
			print ("[DebuggerBrowsable (DebuggerBrowsableState.{0})]", ba.State);
#endif

		foreach (DebuggerDisplayAttribute da in pi.GetCustomAttributes (typeof (DebuggerDisplayAttribute), false)) {
			var narg = da.Name != null ? string.Format (", Name = \"{0}\"", da.Name) : string.Empty;
			var targ = da.Type != null ? string.Format (", Type = \"{0}\"", da.Type) : string.Empty;
			print ("[DebuggerDisplay (\"{0}\"{1}{2})]", da.Value, narg, targ);
		}

		PrintPlatformAttributes (pi);

		foreach (SinceAttribute sa in pi.GetCustomAttributes (typeof (SinceAttribute), false)) 
			print ("[Since ({0},{1})]", sa.Major, sa.Minor);

		foreach (ThreadSafeAttribute sa in pi.GetCustomAttributes (typeof (ThreadSafeAttribute), false)) 
			print ("[ThreadSafe]");
	}

	void GenerateProperty (Type type, PropertyInfo pi, List<string> instance_fields_to_clear_on_dispose, bool is_model)
	{
		string wrap;
		var export = GetExportAttribute (pi, out wrap);
		var minfo = new MemberInformation (pi, type);

		var mod = minfo.GetVisibility ();

		if (wrap != null){
			print_generated_code ();
			PrintPropertyAttributes (pi);
			print ("{0} {1}{2} {3} {{",
			       mod,
			       minfo.GetModifiers (),
			       FormatType (pi.DeclaringType,  pi.PropertyType),
			       pi.Name);
			indent++;
			if (pi.CanRead) {
				PrintPlatformAttributes (pi);
				print ("get {");
				indent++;

				if (pi.PropertyType.IsSubclassOf (typeof (DictionaryContainerType))) {
					print ("var src = {0};", wrap);
					print ("return src == null ? null : new {0}(src);", FormatType (pi.DeclaringType, pi.PropertyType));
				} else
					print ("return {0} as {1};", wrap, FormatType (pi.DeclaringType, pi.PropertyType));

				indent--;
				print ("}");
			}
			if (pi.CanWrite) {
				PrintPlatformAttributes (pi);
				print ("set {");
				indent++;

				if (pi.PropertyType.IsSubclassOf (typeof (DictionaryContainerType)))
					print ("{0} = value == null ? null : value.Dictionary;", wrap);
				else
					print ("{0} = value;", wrap);

				indent--;
				print ("}");
			}

			indent--;
			print ("}\n");
			return;
		}

		string var_name = null;
		
		if (wrap == null) {
			// [Model] has properties that only throws, so there's no point in adding unused backing fields
			if (!is_model && DoesPropertyNeedBackingField (pi)) {
				var_name = string.Format ("__mt_{0}_var{1}", pi.Name, minfo.is_static ? "_static" : "");

				//print ("[CompilerGenerated]");

				if (minfo.is_thread_static)
					print ("[ThreadStatic]");
				print ("{1}object {0};", var_name, minfo.is_static ? "static " : "");

				if (!minfo.is_static){
					instance_fields_to_clear_on_dispose.Add (var_name);
				}
			}
		}

		print_generated_code ();
		PrintPropertyAttributes (pi);

		print ("{0} {1}{2} {3} {{",
		       mod,
		       minfo.GetModifiers (),
		       FormatType (pi.DeclaringType,  pi.PropertyType),
		       pi.Name);
		indent++;

		if (wrap != null) {
			if (pi.CanRead) {
				PrintPlatformAttributes (pi);
				print ("get {{ return {0} as {1}; }}", wrap, FormatType (pi.DeclaringType, pi.PropertyType));
			}
			if (pi.CanWrite) {
				PrintPlatformAttributes (pi);
				print ("set {{ {0} = value; }}", wrap);
			}
			indent--;
			print ("}\n");
			return;			
		}

		if (pi.CanRead){
			var getter = pi.GetGetMethod ();
			var ba = GetBindAttribute (getter);
			string sel = ba != null ? ba.Selector : export.Selector;

			PrintPlatformAttributes (pi);

			if (!minfo.is_sealed || !minfo.is_wrapper) {
				if (export.ArgumentSemantic != ArgumentSemantic.None)
					print ("[Export (\"{0}\", ArgumentSemantic.{1})]", sel, export.ArgumentSemantic);
				else
					print ("[Export (\"{0}\")]", sel);
			}
			if (minfo.is_abstract){
				print ("get; ");
			} else {
				print ("get {");
				if (debug)
					print ("Console.WriteLine (\"In {0}\");", pi.GetGetMethod ());
				if (is_model)
					print ("\tthrow new ModelNotImplementedException ();");
				else {
					if (minfo.is_autorelease) {
						indent++;
						print ("using (var autorelease_pool = new NSAutoreleasePool ()) {");
					}
					if (!DoesPropertyNeedBackingField (pi)) {
						GenerateMethodBody (type, minfo, getter, sel, false, null, BodyOption.None, pi);
					} else if (minfo.is_static) {
						GenerateMethodBody (type, minfo, getter, sel, false, var_name, BodyOption.StoreRet, pi);
					} else {
						if (DoesPropertyNeedDirtyCheck (pi, export))
							GenerateMethodBody (type, minfo, getter, sel, false, var_name, BodyOption.CondStoreRet, pi);
						else
							GenerateMethodBody (type, minfo, getter, sel, false, var_name, BodyOption.MarkRetDirty, pi);
					}
					if (minfo.is_autorelease) {
						print ("}");
						indent--;
					}
				}
				print ("}\n");
			}
		}
		if (pi.CanWrite){
			var setter = pi.GetSetMethod ();
			var ba = GetBindAttribute (setter);
			bool null_allowed = HasAttribute (pi, typeof (NullAllowedAttribute)) || HasAttribute (setter, typeof (NullAllowedAttribute));
			bool not_implemented = HasAttribute (setter, typeof (NotImplementedAttribute));
			string sel;

			if (ba == null){
				ExportAttribute setexport = MakeSetAttribute (export);
				sel = setexport.Selector;
			} else {
				sel = ba.Selector;
			}

			PrintPlatformAttributes (pi);

			if (!not_implemented && (!minfo.is_sealed || !minfo.is_wrapper)){
				if (export.ArgumentSemantic != ArgumentSemantic.None)
					print ("[Export (\"{0}\", ArgumentSemantic.{1})]", sel, export.ArgumentSemantic);
				else
					print ("[Export (\"{0}\")]", sel);
			}
			if (minfo.is_abstract){
				print ("set; ");
			} else {
				print ("set {");
				if (debug)
					print ("Console.WriteLine (\"In {0}\");", pi.GetSetMethod ());
				if (not_implemented)
					print ("\tthrow new NotImplementedException ();");
				else if (is_model)
					print ("\tthrow new ModelNotImplementedException ();");
				else {
					GenerateMethodBody (type, minfo, setter, sel, null_allowed, null, BodyOption.None, pi);
					if (!minfo.is_static && DoesPropertyNeedBackingField (pi)) {
						if (DoesPropertyNeedDirtyCheck (pi, export)) {
#if !MONOMAC
							print ("\tif (!IsNewRefcountEnabled ())");
#endif
							print ("\t\t{0} = value;", var_name);									
						} else {
#if !MONOMAC
							print ("\tMarkDirty ();");
#endif
							print ("\t{0} = value;", var_name);
						}
					}
				}
				print ("}");
			}
		}
		indent--;
		print ("}}\n", pi.Name);
	}

	class AsyncMethodInfo : MemberInformation {
		public Type type;
		public MethodInfo mi;
		public Type category_extension_type;
		public ParameterInfo[] async_initial_params, async_completion_params;
		public bool has_nserror, is_void_async, is_single_arg_async;

		public AsyncMethodInfo (Type type, MethodInfo mi, Type category_extension_type) : base (mi, type, category_extension_type)
		{
			this.mi = mi;
			this.type = type;
			this.category_extension_type = category_extension_type;
			this.async_initial_params = Generator.DropLast (mi.GetParameters ());

			//FIXME do proper error handling if the last parameter is not a delegate
			var cbParams = mi.GetParameters ().Last ().ParameterType.GetMethod ("Invoke").GetParameters ();
			async_completion_params = cbParams;

			//WTF this fails: cbParams.Last ().ParameterType.Name == typeof (NSError)
			if (cbParams.Length > 0 && cbParams.Last ().ParameterType.Name == "NSError") {
				has_nserror = true;
				cbParams = Generator.DropLast (cbParams);
			}
			if (cbParams.Length == 0)
				is_void_async = true;
			if (cbParams.Length == 1)
				is_single_arg_async = true;
		}
	}

	public static T[] DropLast<T> (T[] arr)
	{
		T[] res = new T [arr.Length - 1];
		Array.Copy (arr, res, res.Length);
		return res;
	}

	string GetReturnType (AsyncMethodInfo minfo)
	{
		if (minfo.is_void_async)
			return "Task";
		return "Task<" + GetAsyncTaskType (minfo) + ">";
	}

	string GetAsyncTaskType (AsyncMethodInfo minfo)
	{
		if (minfo.is_single_arg_async)
			return FormatType (minfo.type, minfo.async_completion_params [0].ParameterType);

		var attr = GetAttribute<AsyncAttribute> (minfo.mi);
		if (attr.ResultTypeName != null)
			return attr.ResultTypeName;
		if (attr.ResultType != null)
			return FormatType (minfo.type, attr.ResultType);

		throw new BindingException (1023, true, "Async method {0} with more than one result parameter in the callback by neither ResultTypeName or ResultType", minfo.mi);
	}

	string GetInvokeParamList (ParameterInfo[] parameters)
	{
		StringBuilder sb = new StringBuilder ();
		bool comma = false;
		foreach (var pi in parameters) {
			if (comma)
				sb.Append (", ");
			comma = true;
			sb.Append (pi.Name);
		}
		return sb.ToString ();
	}

	void PrintAsyncHeader (AsyncMethodInfo minfo)
	{
		bool ctor;

		print_generated_code ();
		print ("{0} {1}{2} {3}",
			minfo.GetVisibility (),
			minfo.GetModifiers (),
			GetReturnType (minfo),
			MakeSignature (minfo.mi, out ctor, true, minfo.category_extension_type, minfo.async_initial_params),
		    minfo.is_abstract ? ";" : "");
	}

	void GenerateAsyncMethod (Type type, MethodInfo mi, Type category_extension_type)
	{
		var minfo = new AsyncMethodInfo (type, mi, category_extension_type);

		PrintMethodAttributes (mi);

		PrintAsyncHeader (minfo);
		if (minfo.is_abstract)
			return;

		print ("{");
		indent++;

		if (minfo.is_void_async)
			print ("var tcs = new TaskCompletionSource<bool> ();");
		else
			print ("var tcs = new TaskCompletionSource<{0}> ();", GetAsyncTaskType (minfo));
		print ("{0}({1}{2}({3}) => {{",
			mi.Name,
			GetInvokeParamList (minfo.async_initial_params),
			minfo.async_initial_params.Length > 0 ? ", " : "",
			GetInvokeParamList (minfo.async_completion_params));
		indent++;

		int nesting_level = 1;
		if (minfo.has_nserror) {
			var var_name = minfo.async_completion_params.Last ().Name;
			print ("if ({0} != null)", var_name);
			print ("\ttcs.SetException (new NSErrorException({0}));", var_name);
			print ("else");
			++nesting_level; ++indent;
		}

		if (minfo.is_void_async)
			print ("tcs.SetResult (true);");
		else if (minfo.is_single_arg_async)
			print ("tcs.SetResult ({0});", minfo.async_completion_params [0].Name);
		else
			print ("tcs.SetResult (new {0} ({1}));",
				GetAsyncTaskType (minfo),
				GetInvokeParamList (minfo.has_nserror ? DropLast (minfo.async_completion_params) : minfo.async_completion_params));
		indent -= nesting_level;
		print ("});");
		print ("return tcs.Task;");
		indent--;
		print ("}\n");

		var attr = GetAttribute<AsyncAttribute> (mi);
		if (attr.ResultTypeName != null) {
			if (minfo.has_nserror)
				async_result_types.Add (new Tuple<string, ParameterInfo[]> (attr.ResultTypeName, DropLast (minfo.async_completion_params)));
			else
				async_result_types.Add (new Tuple<string, ParameterInfo[]> (attr.ResultTypeName, minfo.async_completion_params));
		}
	}

	void PrintMethodAttributes (MethodInfo mi)
	{
		foreach (ObsoleteAttribute oa in mi.GetCustomAttributes (typeof (ObsoleteAttribute), false)) {
			print ("[Obsolete (\"{0}\", {1})]",
			       oa.Message, oa.IsError ? "true" : "false");
		}

		foreach (ThreadSafeAttribute sa in mi.GetCustomAttributes (typeof (ThreadSafeAttribute), false)) 
			print ("[ThreadSafe]");
		
#if !COREFX
		foreach (EditorBrowsableAttribute ea in mi.GetCustomAttributes (typeof (EditorBrowsableAttribute), false)) {
			if (ea.State == EditorBrowsableState.Always) {
				print ("[EditorBrowsable]");
			} else {
				print ("[EditorBrowsable (EditorBrowsableState.{0})]", ea.State);
			}
		}
#endif
	}

	void GenerateMethod (Type type, MethodInfo mi, bool is_model, Type category_extension_type, bool is_appearance)
	{
		foreach (ParameterInfo pi in mi.GetParameters ())
			if (HasAttribute (pi, typeof (RetainAttribute))){
				print ("#pragma warning disable 168");
				print ("{0} __mt_{1}_{2};", pi.ParameterType, mi.Name, pi.Name);
				print ("#pragma warning restore 168");
			}


		var minfo = new MemberInformation (mi, type, category_extension_type);
		if (minfo.is_export)
			print ("[Export (\"{0}\"{1})]", minfo.selector, minfo.is_variadic ? ", IsVariadic = true" : string.Empty);

		PrintMethodAttributes (mi);
		PrintPlatformAttributes (mi);

		var mod = minfo.GetVisibility ();

		bool ctor;
		print_generated_code ();
		print ("{0} {1}{2}{3}",
		       mod,
		       minfo.GetModifiers (),
		       MakeSignature (mi, out ctor, category_extension_type),
		       minfo.is_abstract ? ";" : "");

		if (!minfo.is_abstract){
			if (ctor) {
				indent++;
				print (": {0}", minfo.wrap_method == null ? "base (NSObjectFlag.Empty)" : minfo.wrap_method);
				indent--;
			}

			print ("{");
			if (debug)
				print ("\tConsole.WriteLine (\"In {0}\");", mi);
					
			if (is_model)
				print ("\tthrow new You_Should_Not_Call_base_In_This_Method ();");
			else if (minfo.wrap_method != null) {
				if (!ctor) {
					indent++;

					string ret = mi.ReturnType == typeof (void) ? null : "return ";
					print ("{0}{1};", ret, minfo.wrap_method);
					indent--;
				}
			} else {
				if (minfo.is_autorelease) {
					indent++;
					print ("using (var autorelease_pool = new NSAutoreleasePool ()) {");
				}
				GenerateMethodBody (type, minfo, mi, minfo.selector, false, null, BodyOption.None, null, is_appearance, category_extension_type);
				if (minfo.is_autorelease) {
					print ("}");
					indent--;
				}
			}
			print ("}\n");
		}

		if (mi.IsDefined (typeof (AsyncAttribute), false))
			GenerateAsyncMethod (type, mi, category_extension_type);

	}
	
	public string GetGeneratedTypeName (Type type)
	{
		object [] bindOnType = type.GetCustomAttributes (typeof (BindAttribute), true);
		if (bindOnType.Length > 0)
			return ((BindAttribute) bindOnType [0]).Selector;
		else
			return type.Name;
	}

	void RenderDelegates (Dictionary<string,MethodInfo> delegateTypes)
	{
		// Group the delegates by namespace
		var groupedTypes = from fullname in delegateTypes.Keys
			let p = fullname.LastIndexOf (".")
			let ns = fullname.Substring (0, p)
			group fullname by ns into g
			select new {Namespace = g.Key, Fullname=g};
		
		foreach (var group in groupedTypes){
			print ("namespace {0} {{", group.Namespace);
			indent++;
			foreach (var deltype in group.Fullname){
				int p = deltype.LastIndexOf (".");
				var shortName = deltype.Substring (p+1);
				var mi = delegateTypes [deltype];
				
				print ("public delegate {0} {1} ({2});",
				       RenderType (mi.ReturnType),
				       shortName,
				       RenderParameterDecl (mi.GetParameters ()));
			}
			indent--;
			print ("}\n");
		}
	}
	
	public void Generate (Type type)
	{
		type_wants_zero_copy = HasAttribute (type, typeof (ZeroCopyStringsAttribute)) || ZeroCopyStrings;
		type_needs_thread_checks = UINamespaces.Contains (type.Namespace) && !HasAttribute (type, typeof (ThreadSafeAttribute));
		string TypeName = GetGeneratedTypeName (type);

		string dir = Path.Combine (basedir, type.Namespace.Replace (MainPrefix + ".", ""));
		string file = TypeName + ".g.cs";

		if (!Directory.Exists (dir))
			Directory.CreateDirectory (dir);

		indent = 0;
		string output_file = Path.Combine (dir, file);
		GeneratedFiles.Add (output_file);
		var instance_fields_to_clear_on_dispose = new List<string> ();
		var gtype = GeneratedType.Lookup (type);
		var appearance_selectors = gtype.ImplementsAppearance ? gtype.AppearanceSelectors : null;
		
		using (var sw = new StreamWriter (output_file)){
			this.sw = sw;
			var category_attribute = type.GetCustomAttributes (typeof (CategoryAttribute), true);
			bool is_category_class = category_attribute.Length > 0;
			bool is_static_class = type.GetCustomAttributes (typeof (StaticAttribute), true).Length > 0 || is_category_class;
			bool is_model = type.GetCustomAttributes (typeof (ModelAttribute), true).Length > 0;
			bool is_protocol = HasAttribute (type, typeof (ProtocolAttribute));
			string class_visibility = HasAttribute (type, typeof (InternalAttribute)) ? "internal" : "public";

			var default_ctor_visibility = GetAttribute<DefaultCtorVisibilityAttribute> (type);
			object [] btype = type.GetCustomAttributes (typeof (BaseTypeAttribute), true);
			BaseTypeAttribute bta = btype.Length > 0 ? ((BaseTypeAttribute) btype [0]) : null;
			Type base_type = bta != null ?  bta.BaseType : typeof (object);
			string objc_type_name = bta != null ? (bta.Name != null ? bta.Name : TypeName) : TypeName;
			Header (sw);

			print ("namespace {0} {{", type.Namespace);
			indent++;

			string class_mod = null;
			if (is_static_class || is_category_class){
				base_type = typeof (object);
				class_mod = "static ";
			} else {
				if (is_protocol)
					print ("[Protocol]");
				print ("[Register(\"{0}\", {1})]", objc_type_name, HasAttribute (type, typeof (SyntheticAttribute)) ? "false" : "true");
				if (need_abstract.ContainsKey (type))
					class_mod = "abstract ";
			} 
			
			if (is_model){
				if (is_category_class)
					ErrorHelper.Show (new BindingException (1022, true, "Category classes can not use the [Model] attribute"));
				print ("[Model]");
			}

			PrintPlatformAttributes (type);

			var baseTypes = base_type != typeof (object) && TypeName != "NSObject" && !is_category_class ? ": " + FormatType (type, base_type) : "";
			print ("{0} unsafe {1}partial class {2} {3} {{",
			       class_visibility,
			       class_mod,
			       TypeName,
			       baseTypes
			       );

			indent++;
			
			if (!is_model){
				foreach (var ea in selectors [type]){
					var selectorField = SelectorField (ea, true);
					if (!InlineSelectors)
						selectorField = selectorField.Substring (0, selectorField.Length - 6 /* Handle */);
					//print ("[CompilerGenerated]");
					//print ("const string {0} = \"{1}\";", selectorField, ea);
					if (!InlineSelectors)
						print ("static readonly IntPtr {0} = Selector.GetHandle (\"{1}\");", SelectorField (ea), ea);
				}
			}
			print ("");

			// Regular bindings (those that are not-static) or categories need this
			if (!is_static_class || is_category_class){
				if (is_category_class)
					objc_type_name = FormatType (null, bta.BaseType);
				
				if (!is_model && !external) {
					//print ("[CompilerGenerated]");
					print ("static readonly IntPtr class_ptr = Class.GetHandle (\"{0}\");\n", objc_type_name);
				}
			}
			
			if (!is_static_class){
				if (!is_model && !external) {
					print ("public {1} IntPtr ClassHandle {{ get {{ return class_ptr; }} }}\n", objc_type_name, TypeName == "NSObject" ? "virtual" : "override");
				}

				var ctor_visibility = "public";
				var disable_default_ctor = false;
				if (default_ctor_visibility != null) {
					switch (default_ctor_visibility.Visibility) {
					case Visibility.Public:
						break; // default
					case Visibility.Internal: 
						ctor_visibility = "internal";
						break;
					case Visibility.Protected:
						ctor_visibility = "protected";
						break;
					case Visibility.ProtectedInternal:
						ctor_visibility = "protected internal";
						break;
					case Visibility.Private:
						ctor_visibility = string.Empty;
						break;
					case Visibility.Disabled:
						disable_default_ctor = true;
						break;
					}
				}
				
				if (TypeName != "NSObject"){
					var initSelector = InlineSelectors ? "Selector.GetHandle (\"init\")" : "Selector.Init";
					var initWithCoderSelector = InlineSelectors ? "Selector.GetHandle (\"initWithCoder:\")" : "Selector.InitWithCoder";
					if (external) {
						if (!disable_default_ctor) {
							GeneratedCode (sw, 2);
#if !COREFX
							sw.WriteLine ("\t\t[EditorBrowsable (EditorBrowsableState.Advanced)]");
#endif
							sw.WriteLine ("\t\t[Export (\"init\")]");
							sw.WriteLine ("\t\t{1} {0} () : base (NSObjectFlag.Empty)", TypeName, ctor_visibility);
							sw.WriteLine ("\t\t{");
							if (debug)
								sw.WriteLine ("\t\t\tConsole.WriteLine (\"{0}.ctor ()\");", TypeName);
							sw.WriteLine ("\t\t\tHandle = " + MainPrefix + ".ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, {0});", initSelector);
							sw.WriteLine ("\t\t\t");
							sw.WriteLine ("\t\t}");
						}
					} else {
						if (!disable_default_ctor) {
							GeneratedCode (sw, 2);
#if !COREFX
							sw.WriteLine ("\t\t[EditorBrowsable (EditorBrowsableState.Advanced)]");
#endif
							sw.WriteLine ("\t\t[Export (\"init\")]");
							sw.WriteLine ("\t\t{1} {0} () : base (NSObjectFlag.Empty)", TypeName, ctor_visibility);
							sw.WriteLine ("\t\t{");
							if (BindThirdPartyLibrary)
								sw.WriteLine ("\t\t\t{0}", init_binding_type);
							if (debug)
								sw.WriteLine ("\t\t\tConsole.WriteLine (\"{0}.ctor ()\");", TypeName);
							sw.WriteLine ("\t\t\tif (IsDirectBinding) {");
							sw.WriteLine ("\t\t\t\tHandle = " + MainPrefix + ".ObjCRuntime.Messaging.IntPtr_objc_msgSend (this.Handle, {0});", initSelector);
							sw.WriteLine ("\t\t\t} else {");
							sw.WriteLine ("\t\t\t\tHandle = " + MainPrefix + ".ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper (this.SuperHandle, {0});", initSelector);
							sw.WriteLine ("\t\t\t}");
							sw.WriteLine ("\t\t}");
							sw.WriteLine ();
						}
						GeneratedCode (sw, 2);
#if !COREFX
						sw.WriteLine ("\t\t[EditorBrowsable (EditorBrowsableState.Advanced)]");
#endif
						sw.WriteLine ("\t\t[Export (\"initWithCoder:\")]");
						sw.WriteLine ("\t\tpublic {0} (NSCoder coder) : base (NSObjectFlag.Empty)", TypeName);
						sw.WriteLine ("\t\t{");
						if (BindThirdPartyLibrary)
							sw.WriteLine ("\t\t\t{0}", init_binding_type);
						if (debug)
							sw.WriteLine ("\t\t\tConsole.WriteLine (\"{0}.ctor (NSCoder)\");", TypeName);
						sw.WriteLine ("\t\t\tif (IsDirectBinding) {");
						sw.WriteLine ("\t\t\t\tHandle = " + MainPrefix + ".ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr (this.Handle, {0}, coder.Handle);", initWithCoderSelector);
						sw.WriteLine ("\t\t\t} else {");
						sw.WriteLine ("\t\t\t\tHandle = " + MainPrefix + ".ObjCRuntime.Messaging.IntPtr_objc_msgSendSuper_IntPtr (this.SuperHandle, {0}, coder.Handle);", initWithCoderSelector);
						sw.WriteLine ("\t\t\t}");
						sw.WriteLine ("\t\t}");
						sw.WriteLine ();
					}
					GeneratedCode (sw, 2);
#if !COREFX
					sw.WriteLine ("\t\t[EditorBrowsable (EditorBrowsableState.Advanced)]");
#endif
					sw.WriteLine ("\t\tpublic {0} (NSObjectFlag t) : base (t)", TypeName);
					sw.WriteLine ("\t\t{");
					if (BindThirdPartyLibrary)
						sw.WriteLine ("\t\t\t{0}", init_binding_type);
					sw.WriteLine ("\t\t}");
					sw.WriteLine ();
					GeneratedCode (sw, 2);
#if !COREFX
					sw.WriteLine ("\t\t[EditorBrowsable (EditorBrowsableState.Advanced)]");
#endif
					sw.WriteLine ("\t\tpublic {0} (IntPtr handle) : base (handle)", TypeName);
					sw.WriteLine ("\t\t{");
					if (BindThirdPartyLibrary)
						sw.WriteLine ("\t\t\t{0}", init_binding_type);
					sw.WriteLine ("\t\t}");
					sw.WriteLine ();
				}
			}
			
			foreach (var mi in GetTypeContractMethods (type)){
				if (mi.IsSpecialName || (mi.Name == "Constructor" && type != mi.DeclaringType))
					continue;

#if RETAIN_AUDITING
				if (mi.Name.StartsWith ("Set"))
					foreach (ParameterInfo pi in mi.GetParameters ())
						if (IsWrappedType (pi.ParameterType) || pi.ParameterType.IsArray) {
							Console.WriteLine ("AUDIT: {0}", mi);
						}
#endif

				if (HasAttribute (mi, typeof (AlphaAttribute)) && Alpha == false)
					continue;

				if (appearance_selectors != null && HasAttribute (mi, typeof (AppearanceAttribute)))
					appearance_selectors.Add (mi);

				GenerateMethod (type, mi, is_model,
						category_extension_type: is_category_class ? bta.BaseType : null,
						is_appearance: false);
			}

			var field_exports = new List<PropertyInfo> ();
			var notifications = new List<PropertyInfo> ();
			foreach (var pi in GetTypeContractProperties (type)){
				if (HasAttribute (pi, typeof (AlphaAttribute)) && Alpha == false)
					continue;

				if (HasAttribute (pi, typeof (FieldAttribute))){
					field_exports.Add (pi);

					if (HasAttribute (pi, typeof (NotificationAttribute)))
						notifications.Add (pi);
					continue;
				}

				if (appearance_selectors != null && HasAttribute (pi, typeof (AppearanceAttribute)))
					appearance_selectors.Add (pi);
				
				GenerateProperty (type, pi, instance_fields_to_clear_on_dispose, is_model);
			}
			
			if (field_exports.Count != 0){
				foreach (var field_pi in field_exports){
					var fieldAttr = (FieldAttribute) field_pi.GetCustomAttributes (typeof (FieldAttribute), true) [0];
					string library_name; 

					if (fieldAttr.LibraryName != null){
						// Remapped
						library_name = fieldAttr.LibraryName;
						if (library_name [0] == '+'){
							switch (library_name){
							case "+CoreImage":
								library_name = CoreImageMap;
								break;
							}
						} 
					} else {
						library_name = type.Namespace;
						// note: not every binding namespace will start with MainPrefix (e.g. MonoTouch.)
						if (library_name.StartsWith (MainPrefix))
							library_name = library_name.Substring (MainPrefix.Length + 1);
					}

					if (!libraries.Contains (library_name)) {
						libraries.Add (library_name);
					}

					string fieldTypeName = FormatType (field_pi.DeclaringType, field_pi.PropertyType);
					// Value types we dont cache for now, to avoid Nullable<T>
					if (!field_pi.PropertyType.IsValueType) {
						//print ("[CompilerGenerated]");
						print ("static {0} _{1};", fieldTypeName, field_pi.Name);
					}

					print ("[Field (\"{0}\",  \"{1}\")]", fieldAttr.SymbolName, library_name);
					PrintPlatformAttributes (field_pi);
					print ("{0} static {1} {2} {{", HasAttribute (field_pi, typeof (InternalAttribute)) ? "internal" : "public", fieldTypeName, field_pi.Name);
					indent++;

					PrintPlatformAttributes (field_pi);
					print ("get {");
					indent++;
					if (field_pi.PropertyType == typeof (NSString)){
						print ("if (_{0} == null)", field_pi.Name);
						indent++;
						print ("_{0} = Dlfcn.GetStringConstant (Libraries.{2}.Handle, \"{1}\");", field_pi.Name, fieldAttr.SymbolName, library_name);
						indent--;
						print ("return _{0};", field_pi.Name);
					} else if (field_pi.PropertyType.Name == "NSArray"){
						print ("if (_{0} == null)", field_pi.Name);
						indent++;
						print ("_{0} = new NSArray (Dlfcn.GetIndirect (Libraries.{2}.Handle, \"{1}\"));", field_pi.Name, fieldAttr.SymbolName, library_name);
						indent--;
						print ("return _{0};", field_pi.Name);
					} else if (field_pi.PropertyType == typeof (int)){
						print ("return Dlfcn.GetInt32 (Libraries.{2}.Handle, \"{1}\");", field_pi.Name, fieldAttr.SymbolName, library_name);
					} else if (field_pi.PropertyType == typeof (double)){
						print ("return Dlfcn.GetDouble (Libraries.{2}.Handle, \"{1}\");", field_pi.Name, fieldAttr.SymbolName, library_name);
					} else if (field_pi.PropertyType == typeof (float)){
						print ("return Dlfcn.GetFloat (Libraries.{2}.Handle, \"{1}\");", field_pi.Name, fieldAttr.SymbolName, library_name);
					} else if (field_pi.PropertyType == typeof (IntPtr)){
						print ("return Dlfcn.GetIntPtr (Libraries.{2}.Handle, \"{1}\");", field_pi.Name, fieldAttr.SymbolName, library_name);
					} else if (field_pi.PropertyType == typeof (SizeF)){
						print ("return Dlfcn.GetSizeF (Libraries.{2}.Handle, \"{1}\");", field_pi.Name, fieldAttr.SymbolName, library_name);
					} else if (field_pi.PropertyType == typeof (long)){
						print ("return Dlfcn.GetInt64 (Libraries.{2}.Handle, \"{1}\");", field_pi.Name, fieldAttr.SymbolName, library_name);
					} else {
						if (field_pi.PropertyType == typeof (string))
							throw new BindingException (1013, true, "Unsupported type for Fields (string), you probably meant NSString");
						else
							throw new BindingException (1014, true, "Unsupported type for Fields: {0}", fieldTypeName);
					}
					
					indent--;
					print ("}");

					if (field_pi.CanWrite) {
						PrintPlatformAttributes (field_pi);
						print ("set {");
						indent++;
						if (field_pi.PropertyType == typeof (int)) {
							print ("Dlfcn.SetInt32 (Libraries.{2}.Handle, \"{1}\", value);", field_pi.Name, fieldAttr.SymbolName, library_name);
						} else if (field_pi.PropertyType == typeof (double)) {
							print ("Dlfcn.SetDouble (Libraries.{2}.Handle, \"{1}\", value);", field_pi.Name, fieldAttr.SymbolName, library_name);
						} else if (field_pi.PropertyType == typeof (float)) {
							print ("Dlfcn.SetFloat (Libraries.{2}.Handle, \"{1}\", value);", field_pi.Name, fieldAttr.SymbolName, library_name);
						} else if (field_pi.PropertyType == typeof (IntPtr)) {
							print ("Dlfcn.SetIntPtr (Libraries.{2}.Handle, \"{1}\", value);", field_pi.Name, fieldAttr.SymbolName, library_name);
						} else if (field_pi.PropertyType == typeof (SizeF)) {
							print ("Dlfcn.SetSizeF (Libraries.{2}.Handle, \"{1}\", value);", field_pi.Name, fieldAttr.SymbolName, library_name);
						} else if (field_pi.PropertyType == typeof (long)) {
							print ("Dlfcn.SetInt64 (Libraries.{2}.Handle, \"{1}\", value);", field_pi.Name, fieldAttr.SymbolName, library_name);
						} else if (field_pi.PropertyType == typeof (NSString)){
							print ("Dlfcn.SetString (Libraries.{2}.Handle, \"{1}\", value);", field_pi.Name, fieldAttr.SymbolName, library_name);
						} else if (field_pi.PropertyType.Name == "NSArray"){
							print ("Dlfcn.SetArray (Libraries.{2}.Handle, \"{1}\", value);", field_pi.Name, fieldAttr.SymbolName, library_name);
						} else
							throw new BindingException (1021, true, "Unsupported type for read/write Fields: {0} for {1}.{2}", fieldTypeName, field_pi.DeclaringType.FullName, field_pi.Name);
						indent--;
						print ("}");
					}
					indent--;
					print ("}");
				}
			}

			var eventArgTypes = new Dictionary<string,ParameterInfo[]> ();

			if (bta != null && bta.Events != null){
				if (bta.Delegates == null)
					throw new BindingException (1015, true, "In class {0} You specified the Events property, but did not bind those to names with Delegates", type.FullName);
				
				print ("//");
				print ("// Events and properties from the delegate");
				print ("//\n");


				int delidx = 0;
				foreach (var dtype in bta.Events){
					string delName = bta.Delegates [delidx++];

					// The ensure method
					if (bta.KeepRefUntil == null)
						print ("_{0} Ensure{0} ()", dtype.Name);
					else {
						print ("static System.Collections.ArrayList instances;");
						print ("_{0} Ensure{0} (object oref)", dtype.Name);
					}
					
					print ("{"); indent++;
					print ("var del = {0};", delName);
					print ("if (del == null || (!(del is _{0}))){{", dtype.Name);
					print ("\tdel = new _{0} ({1});", dtype.Name, bta.KeepRefUntil == null ? "" : "oref");
					if (bta.KeepRefUntil != null){
						print ("\tif (instances == null) instances = new System.Collections.ArrayList ();");
						print ("\tif (!instances.Contains (this)) instances.Add (this);");
					}
					print ("\t{0} = del;", delName);
					print ("}");
					print ("return (_{0}) del;", dtype.Name);
					indent--; print ("}\n");

					var noDefaultValue = new List<MethodInfo> ();
					
					print ("#pragma warning disable 672");
					print ("[Register]");
					print ("sealed class _{0} : {1} {{ ", dtype.Name, RenderType (dtype));
					indent++;
					if (bta.KeepRefUntil != null){
						print ("object reference;");
						print ("public _{0} (object reference) {{ this.reference = reference; }}\n", dtype.Name);
					} else 
						print ("public _{0} () {{}}\n", dtype.Name);
						

					foreach (var mi in dtype.GatherMethods ()){
						if (HasAttribute (mi, typeof (AlphaAttribute)) && Alpha == false)
							continue;

						// Skip property getter/setters
						if (mi.IsSpecialName && (mi.Name.StartsWith ("get_") || mi.Name.StartsWith ("set_")))
							continue;
						
						var pars = mi.GetParameters ();
						int minPars = bta.Singleton ? 0 : 1;

						if (mi.GetCustomAttributes (typeof (NoDefaultValueAttribute), false).Length > 0)
							noDefaultValue.Add (mi);

						if (pars.Length < minPars)
							throw new BindingException (1003, true, "The delegate method {0}.{1} needs to take at least one parameter", dtype.FullName, mi.Name);
						
						var sender = pars.Length == 0 ? "this" : pars [0].Name;

						if (mi.ReturnType == typeof (void)){
							if (bta.Singleton || mi.GetParameters ().Length == 1)
								print ("internal EventHandler {0};", PascalCase (mi.Name));
							else
								print ("internal EventHandler<{0}> {1};", GetEventArgName (mi), PascalCase (mi.Name));
						} else
							print ("internal {0} {1};", GetDelegateName (mi), PascalCase (mi.Name));
						
						print ("[Preserve (Conditional = true)]");
						print ("public override {0} {1} ({2})", RenderType (mi.ReturnType), mi.Name, RenderParameterDecl (pars));
						print ("{"); indent++;

						if (mi.Name == bta.KeepRefUntil)
							print ("instances.Remove (reference);");
						
						if (mi.ReturnType == typeof (void)){
							string eaname;

							if (debug)
								print ("Console.WriteLine (\"Method {0}.{1} invoked\");", dtype.Name, mi.Name);
							if (pars.Length != minPars){
								eaname = GetEventArgName (mi);
								if (!generatedEvents.ContainsKey (eaname) && !eventArgTypes.ContainsKey (eaname)){
									eventArgTypes.Add (eaname, pars);
									generatedEvents.Add (eaname, pars);
								}
							} else
								eaname = "<NOTREACHED>";
							
							if (bta.Singleton || mi.GetParameters ().Length == 1)
								print ("EventHandler handler = {0};", PascalCase (mi.Name));
							else
								print ("EventHandler<{0}> handler = {1};", GetEventArgName (mi), PascalCase (mi.Name));

							print ("if (handler != null){");
							indent++;
							string eventArgs;
							if (pars.Length == minPars)
								eventArgs = "EventArgs.Empty";
							else {
								print ("var args = new {0} ({1});", eaname, RenderArgs (pars.Skip (minPars), true));
								eventArgs = "args";
							}

							print ("handler ({0}, {1});", sender, eventArgs);
							if (pars.Length != minPars && MustPullValuesBack (pars.Skip (minPars))){
								foreach (var par in pars.Skip (minPars)){
									if (!par.ParameterType.IsByRef)
										continue;

									print ("{0} = args.{1};", par.Name, GetPublicParameterName (par));
								}
							}
							if (HasAttribute (mi, typeof (CheckDisposedAttribute))){
								var arg = RenderArgs (pars.Take (1));
								print ("if ({0}.Handle == IntPtr.Zero)", arg);
								print ("\tthrow new ObjectDisposedException (\"{0}\", \"The object was disposed on the event, you should not call Dispose() inside the handler\");", arg);
							}
							indent--;
							print ("}");
						} else {
							var delname = GetDelegateName (mi);

							if (!generatedDelegates.ContainsKey (delname) && !delegate_types.ContainsKey (delname)){
								generatedDelegates.Add (delname, null);
								delegate_types.Add (type.Namespace + "." + delname, mi);
							}
							if (debug)
								print ("Console.WriteLine (\"Method {0}.{1} invoked\");", dtype.Name, mi.Name);

							print ("{0} handler = {1};", delname, PascalCase (mi.Name));
							print ("if (handler != null)");
							print ("	return handler ({0}{1});",
							       sender,
							       pars.Length == minPars ? "" : String.Format (", {0}", RenderArgs (pars.Skip (1))));

							if (mi.GetCustomAttributes (typeof (NoDefaultValueAttribute), false).Length > 0)
								print ("throw new You_Should_Not_Call_base_In_This_Method ();");
							else {
								var def = GetDefaultValue (mi);
								if ((def is string) && ((def as string) == "null") && mi.ReturnType.IsValueType)
									print ("throw new Exception (\"No event handler has been added to the {0} event.\");", mi.Name);
								else {
									foreach (var j in pars){
										if (j.ParameterType.IsByRef && j.IsOut){
											print ("{0} = null;", j.Name);
										}
									}
										
									print ("return {0};", def);
								}
							}
						}
						
						indent--;
						print ("}\n");
					}

					if (noDefaultValue.Count > 0) {
						string selRespondsToSelector = "Selector.GetHandle (\"respondsToSelector:\")";

						if (!InlineSelectors) {
							foreach (var mi in noDefaultValue) {
								var eattrs = mi.GetCustomAttributes (typeof (ExportAttribute), false);
								var export = (ExportAttribute)eattrs[0];
								print ("static IntPtr sel{0}Handle = Selector.GetHandle (\"{1}\");", mi.Name, export.Selector);
							}
							print ("static IntPtr selRespondsToSelector = " + selRespondsToSelector + ";");
							selRespondsToSelector = "selRespondsToSelector";
						}

						print ("public override bool RespondsToSelector (Selector sel)");
						print ("{");
						++indent;
						print ("IntPtr selHandle = sel.Handle;");
						foreach (var mi in noDefaultValue) {
							if (InlineSelectors) {
								var eattrs = mi.GetCustomAttributes (typeof (ExportAttribute), false);
								var export = (ExportAttribute)eattrs[0];
								print ("if (selHandle.Equals (Selector.GetHandle (\"{0}\")))", export.Selector);
							} else {
								print ("if (selHandle.Equals (sel{0}Handle))", mi.Name);
							}
							++indent;
							print ("return {0} != null;", PascalCase (mi.Name));
							--indent;
						}
						print ("return Messaging.bool_objc_msgSendSuper_intptr (SuperHandle, " + selRespondsToSelector + ", selHandle);");
						--indent;
						print ("}");
					}

					indent--;
					print ("}");

					print ("#pragma warning restore 672");
				}
				print ("");

				
				// Now add the instance vars and event handlers
				foreach (var dtype in bta.Events){
					foreach (var mi in dtype.GatherMethods ()){
						if (HasAttribute (mi, typeof (AlphaAttribute)) && Alpha == false)
							continue;

						// Skip property getter/setters
						if (mi.IsSpecialName && (mi.Name.StartsWith ("get_") || mi.Name.StartsWith ("set_")))
							continue;

						string ensureArg = bta.KeepRefUntil == null ? "" : "this";
						
						if (mi.ReturnType == typeof (void)){
							foreach (ObsoleteAttribute oa in mi.GetCustomAttributes (typeof (ObsoleteAttribute), false))
								print ("[Obsolete (\"{0}\", {1})]", oa.Message, oa.IsError ? "true" : "false");

							if (bta.Singleton && mi.GetParameters ().Length == 0 || mi.GetParameters ().Length == 1)
								print ("public event EventHandler {0} {{", CamelCase (GetEventName (mi)));
							else 
								print ("public event EventHandler<{0}> {1} {{", GetEventArgName (mi), CamelCase (GetEventName (mi)));
							print ("\tadd {{ Ensure{0} ({1}).{2} += value; }}", dtype.Name, ensureArg, PascalCase (mi.Name));
							print ("\tremove {{ Ensure{0} ({1}).{2} -= value; }}", dtype.Name, ensureArg, PascalCase (mi.Name));
							print ("}\n");
						} else {
							print ("public {0} {1} {{", GetDelegateName (mi), CamelCase (mi.Name));
							print ("\tget {{ return Ensure{0} ({1}).{2}; }}", dtype.Name, ensureArg, PascalCase (mi.Name));
							print ("\tset {{ Ensure{0} ({1}).{2} = value; }}", dtype.Name, ensureArg, PascalCase (mi.Name));
							print ("}\n");
						}
					}
				}
			}

			//
			// Do we need a dispose method?
			//
			if (!is_static_class){
				object [] disposeAttr = type.GetCustomAttributes (typeof (DisposeAttribute), true);
				if (disposeAttr.Length > 0 || instance_fields_to_clear_on_dispose.Count > 0){
					//print ("[CompilerGenerated]");
					print ("protected override void Dispose (bool disposing)");
					print ("{");
					indent++;
					if (disposeAttr.Length > 0){
						var snippet = disposeAttr [0] as DisposeAttribute;
						Inject (snippet);
					}
					
					print ("base.Dispose (disposing);");
					
					if (instance_fields_to_clear_on_dispose.Count > 0) {
						print ("if (Handle == IntPtr.Zero) {");
						indent++;
						foreach (var field in instance_fields_to_clear_on_dispose){
							print ("{0} = null;", field);
						}
						indent--;
						print ("}");
					}
					
					indent--;
					print ("}");
				}
			}

			//
			// Appearance class
			//
			var gt = GeneratedType.Lookup (type);
			if (gt.ImplementsAppearance){
				var parent_implements_appearance = gt.Parent != null && gt.ParentGenerated.ImplementsAppearance;
				string base_class;
				
				if (parent_implements_appearance){
					var parent = GetGeneratedTypeName (gt.Parent);
					base_class = parent + "." + parent + "Appearance";
				} else
					base_class = "UIAppearance";

				string appearance_type_name = TypeName + "Appearance";
				print ("public partial class {0} : {1} {{", appearance_type_name, base_class);
				indent++;
				print ("internal {0} (IntPtr handle) : base (handle) {{}}", appearance_type_name);

				if (appearance_selectors != null){
					var currently_ignored_fields = new List<string> ();
					
					foreach (MemberInfo mi in appearance_selectors){
						if (mi is MethodInfo)
							GenerateMethod (type, mi as MethodInfo,
									is_model: false,
									category_extension_type: is_category_class ? base_type : null,
									is_appearance: true);
						else
							GenerateProperty (type, mi as PropertyInfo, currently_ignored_fields, false);
					}
				}
				
				indent--;
				print ("}\n");
				print ("public static {0}{1} Appearance {{", parent_implements_appearance ? "new " : "", appearance_type_name);
				indent++;
				print ("get {{ return new {0} (MonoTouch.ObjCRuntime.Messaging.IntPtr_objc_msgSend (class_ptr, UIAppearance.SelectorAppearance)); }}", appearance_type_name);
				indent--;
				print ("}\n");
				print ("public static {0}{1} GetAppearance<T> () {{", parent_implements_appearance ? "new " : "", appearance_type_name);
				indent++;
				print ("return new {0} (MonoTouch.ObjCRuntime.Messaging.IntPtr_objc_msgSend (Class.GetHandle (typeof (T)), UIAppearance.SelectorAppearance));", appearance_type_name);
				indent--;
				print ("}\n");
				print ("public static {0}{1} AppearanceWhenContainedIn (params Type [] containers)", parent_implements_appearance ? "new " : "", appearance_type_name);
				print ("{");
				indent++;
				print ("return new {0} (UIAppearance.GetAppearance (class_ptr, containers));", appearance_type_name);
				indent--;
				print ("}");
				print ("");
			}
			//
			// Notification extensions
			//
			if (notifications.Count > 0){
				print ("\n");
				print ("//");
				print ("// Notifications");
				print ("//");
			
				print ("public static partial class Notifications {\n");
				foreach (var property in notifications){
					string notification_name = GetNotificationName (property);
					string notification_center = GetNotificationCenter (property);
					Type event_args_type = GetNotificationArgType (property);
					string event_name = event_args_type == null ? "NSNotificationEventArgs" : event_args_type.FullName;

					if (event_args_type != null)
						notification_event_arg_types [event_args_type] = event_args_type;
					print ("\tpublic static NSObject Observe{0} (EventHandler<{1}> handler)", notification_name, event_name);
					print ("\t{");
					print ("\t\treturn {0}.AddObserver ({1}, notification => handler (null, new {2} (notification)));", notification_center, property.Name, event_name);
					print ("\t}");
				}
				print ("\n}");
			}

			indent--;
			print ("}} /* class {0} */", TypeName);
			
			//
			// Copy delegates from the API files into the output if they were declared there
			//
			var rootAssembly = types [0].Assembly;
			foreach (var deltype in trampolines.Keys){
				if (deltype.Assembly != rootAssembly)
					continue;

				if (delegates_emitted.ContainsKey (deltype))
					continue;

				delegates_emitted [deltype] = deltype;

				// This formats the delegate 
				var delmethod = deltype.GetMethod ("Invoke");
				var del = new StringBuilder ();

				// Propagate MonoTouch.MonoNativeFunctionWrapperAttribute
				var attrs = deltype.GetCustomAttributes (false);
				foreach (var a in attrs){
					if (a.GetType ().FullName.IndexOf ("MonoNativeFunctionWrapperAttribute") != -1){
						del.Append ("[MonoNativeFunctionWrapper]\n");
						break;
					}
				}
				del.Append ("public delegate ");
				del.Append (FormatType (type, delmethod.ReturnType));
				del.Append (" ");
				del.Append (deltype.Name);
				del.Append (" (");
				var delpars = delmethod.GetParameters ();
				for (int dmi = 0; dmi < delpars.Length; dmi++){
					Type ptype = delpars [dmi].ParameterType;
					string modifier = "";
					if (ptype.IsByRef){
						if (delpars [dmi].IsOut)
							modifier = "out ";
						else
							modifier = "ref ";
						ptype = ptype.GetElementType ();
					}
					
					del.AppendFormat ("{0}{1} {2}{3}", modifier, FormatType (type, ptype), delpars [dmi].Name, dmi+1 == delpars.Length ? "" : ", ");
				}
				del.Append (");");
				print (del.ToString ());
			}

			if (eventArgTypes.Count > 0){
				print ("\n");
				print ("//");
				print ("// EventArgs classes");
				print ("//");
			}
			// Now add the EventArgs classes
			foreach (var eaclass in eventArgTypes.Keys){
				if (skipGeneration.ContainsKey (eaclass)){
					continue;
				}
				int minPars = bta.Singleton ? 0 : 1;
				
				var pars = eventArgTypes [eaclass];

				print ("public partial class {0} : EventArgs {{", eaclass); indent++;
				print ("public {0} ({1})", eaclass, RenderParameterDecl (pars.Skip (1), true));
				print ("{");
				indent++;
				foreach (var p in pars.Skip (minPars)){
					print ("this.{0} = {1};", GetPublicParameterName (p), p.Name);
				}
				indent--;
				print ("}");
				
				// Now print the properties
				foreach (var p in pars.Skip (minPars)){
					var bareType = p.ParameterType.IsByRef ? p.ParameterType.GetElementType () : p.ParameterType;

					print ("public {0} {1} {{ get; set; }}", RenderType (bareType), GetPublicParameterName (p));
				}
				indent--; print ("}\n");
			}

			if (async_result_types.Count > 0) {
				print ("\n");
				print ("//");
				print ("// Async result classes");
				print ("//");
			}

			foreach (var async_type in async_result_types) {
				if (async_result_types_emitted.Contains (async_type.Item1))
					continue;
				async_result_types_emitted.Add (async_type.Item1);

				print ("public class {0} {{", async_type.Item1); indent++;

				StringBuilder ctor = new StringBuilder ();

				bool comma = false;
				foreach (var pi in async_type.Item2) {
					print ("public {0} {1} {{ get; set; }}",
						FormatType (type, pi.ParameterType),
						Capitalize (pi.Name));

					if (comma)
						ctor.Append (", ");
					comma = true;
					ctor.Append (FormatType (type, pi.ParameterType)).Append (" ").Append (pi.Name);
				}

				print ("\npublic {0} ({1}) {{", async_type.Item1, ctor); indent++;
				foreach (var pi in async_type.Item2) {
					print ("this.{0} = {1};", Capitalize (pi.Name), pi.Name);
				}
				indent--; print ("}");

				indent--; print ("}\n");
			}
			async_result_types.Clear ();

			indent--;
			print ("}");
		}
	}

	static string Capitalize (string str)
	{
		return char.ToUpper (str[0]) + str.Substring (1);
	}

	string GetNotificationCenter (PropertyInfo pi)
	{
		object [] a = pi.GetCustomAttributes (typeof (NotificationAttribute), true);
		var str =  (a [0] as NotificationAttribute).NotificationCenter;
		if (str == null)
			str = "NSNotificationCenter.DefaultCenter";
		return str;
	}
		
	string GetNotificationName (PropertyInfo pi)
	{
		// TODO: fetch the NotificationAttribute, see if there is an override there.
		var name = pi.Name;
		if (name.EndsWith ("Notification"))
			return name.Substring (0, name.Length-"Notification".Length);
		return name;
	}

	Type GetNotificationArgType (PropertyInfo pi)
	{
		object [] a = pi.GetCustomAttributes (typeof (NotificationAttribute), true);
		return (a [0] as NotificationAttribute).Type;
	}
	
	//
	// Support for the automatic delegate/event generation
	//
	string RenderParameterDecl (IEnumerable<ParameterInfo> pi)
	{
		return RenderParameterDecl (pi, false);
	}
	
	string RenderParameterDecl (IEnumerable<ParameterInfo> pi, bool removeRefTypes)
	{
		return String.Join (", ", pi.Select (p =>
			(p.ParameterType.IsByRef ? (removeRefTypes ? "" : (p.IsOut ? "out " : "ref ")) + RenderType (p.ParameterType.GetElementType ())
			 		         : RenderType (p.ParameterType)) + " " + p.Name).ToArray ());
						     
	}

	string GetPublicParameterName (ParameterInfo pi)
	{
		object [] attrs = pi.GetCustomAttributes (typeof (EventNameAttribute), true);
		if (attrs.Length == 0)
			return CamelCase (pi.Name);

		var a = (EventNameAttribute) attrs [0];
		return CamelCase (a.EvtName);
	}
	
	string RenderArgs (IEnumerable<ParameterInfo> pi)
	{
		return RenderArgs (pi, false);
	}
	
	string RenderArgs (IEnumerable<ParameterInfo> pi, bool removeRefTypes)
	{
		return String.Join (", ", pi.Select (p => (p.ParameterType.IsByRef ? (removeRefTypes ? "" : (p.IsOut ? "out " : "ref ")) : "")+ p.Name).ToArray ());
	}

	bool MustPullValuesBack (IEnumerable<ParameterInfo> parameters)
	{
		return parameters.Any (pi => pi.ParameterType.IsByRef);
	}
	
	string CamelCase (string ins)
	{
		return Char.ToUpper (ins [0]) + ins.Substring (1);
	}

	string PascalCase (string ins)
	{
		return Char.ToLower (ins [0]) + ins.Substring (1);
	}

	Dictionary<string,bool> skipGeneration = new Dictionary<string,bool> ();
	string GetEventName (MethodInfo mi)
	{
		var a = GetAttribute (mi, typeof (EventNameAttribute));
		if (a == null)
			return mi.Name;
		var ea = (EventNameAttribute) a;
		
		return ea.EvtName;
	}

	string GetEventArgName (MethodInfo mi)
	{
		if (mi.GetParameters ().Length == 1)
			return "EventArgs";
		
		var a = GetAttribute (mi, typeof (EventArgsAttribute));
		if (a == null)
			throw new BindingException (1004, true, "The delegate method {0}.{1} is missing the [EventArgs] attribute (has {2} parameters)", mi.DeclaringType.FullName, mi.Name, mi.GetParameters ().Length);

		var ea = (EventArgsAttribute) a;
		if (ea.ArgName.EndsWith ("EventArgs"))
			throw new BindingException (1005, true, "EventArgs in {0}.{1} attribute should not include the text `EventArgs' at the end", mi.DeclaringType.FullName, mi.Name);
		
		if (ea.SkipGeneration){
			skipGeneration [ea.FullName ? ea.ArgName : ea.ArgName + "EventArgs"] = true;
		}
		
		if (ea.FullName)
			return ea.ArgName;

		return ea.ArgName + "EventArgs";
	}

	string GetDelegateName (MethodInfo mi)
	{
		var a = GetAttribute (mi, typeof (DelegateNameAttribute));
		if (a != null)
			return ((DelegateNameAttribute) a).Name;

		a = GetAttribute (mi, typeof (EventArgsAttribute));
		if (a == null)
			throw new BindingException (1006, true, "The delegate method {0}.{1} is missing the [DelegateName] attribute (or EventArgs)", mi.DeclaringType.FullName, mi.Name);

		ErrorHelper.Show (new BindingException (1102, false, "Using the deprecated EventArgs for a delegate signature in {0}.{1}, please use DelegateName instead", mi.DeclaringType.FullName, mi.Name));
		return ((EventArgsAttribute) a).ArgName;
	}
	
	object GetDefaultValue (MethodInfo mi)
	{
		var a = GetAttribute (mi, typeof (DefaultValueAttribute));
		if (a == null){
			a = GetAttribute (mi, typeof (DefaultValueFromArgumentAttribute));
			if (a != null){
				var fvfa = (DefaultValueFromArgumentAttribute) a;
				return fvfa.Argument;
			}
			
			throw new BindingException (1016, true, "The delegate method {0}.{1} is missing the [DefaultValue] attribute", mi.DeclaringType.FullName, mi.Name);
		}
		var def = ((DefaultValueAttribute) a).Default;
		if (def == null)
			return "null";

		if ((def as Type) == typeof (System.Drawing.RectangleF))
			return "System.Drawing.RectangleF.Empty";
		
		if (def is bool)
			return (bool) def ? "true" : "false";

		if (def is Enum)
			return def.GetType ().FullName + "." + def;

		return def;
	}
	
	string RenderType (Type t)
	{
		if (!t.IsEnum){
			switch (Type.GetTypeCode (t)){
			case TypeCode.Char:
				return "char";
			case TypeCode.String:
				return "string";
			case TypeCode.Int32:
				return "int";
			case TypeCode.UInt32:
				return "uint";
			case TypeCode.Int64:
				return "long";
			case TypeCode.UInt64:
				return "ulong";
			case TypeCode.Single:
				return "float";
			case TypeCode.Double:
				return "double";
			case TypeCode.Decimal:
				return "decimal";
			case TypeCode.SByte:
				return "sbyte";
			case TypeCode.Byte:
				return "byte";
			case TypeCode.Boolean:
				return "bool";
			}
		}
		
		if (t == typeof (void))
			return "void";

		string ns = t.Namespace;
		if (implicit_ns.Contains (ns))
			return t.Name;
		else
			return t.FullName;
		
	}
	
}
