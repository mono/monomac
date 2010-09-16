//
// Copyright 2010, Novell, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software";, to deal in the Software without restriction, including
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

//
// Constants.cs: Constants for the Growl Framework
//
using System;
using MonoMac.Foundation;
using MonoMac.ObjCRuntime;
using MonoMac.AppKit;

namespace MonoMac.Growl {
    public static class RegistrationUserInfoKeys {
        static public string ApplicationName = "ApplicationName";
        static public string ApplicationId = "ApplicationId";
        static public string ApplicationIcon = "ApplicationIcon";
        static public string DefaultNotifications = "DefaultNotifications";
        static public string AllNotifications = "AllNotifications";
        static public string HumanReadableNames = "HumanReadableNames";
        static public string NotificationDescriptions = "NotificationDescriptions";
        static public string TicketVersion = "TicketVersion";      
    }
    
    public static class NotificationUserInfoKeys {
        static public string NotificationName = "NotificationName";
        static public string NotificationTitle = "NotificationTitle";
        static public string NotificationDescription = "NotificationDescription";
        static public string NotificationIcon = "NotificationIcon";
        static public string NotificationAppIcon = "NotificationAppIcon";
        static public string NotificationPriority = "NotificationPriority";
        static public string NotificationSticky = "NotificationSticky";
        static public string NotificationClickContext = "NotificationClickContext";
        static public string NotificationDisplayPlugin = "NotificationDisplayPlugin";
        static public string GrowlNotificationIdentifier = "GrowlNotificationIdentifier";
        static public string ApplicationPID = "ApplicationPID";
        static public string NotificationProgress = "NotificationProgress";
    }
    
    public static class GrowlNotifications {
        static public string ApplicationRegistrationNotification = "GrowlApplicationRegistrationNotification";
        static public string ApplicationRegistrationConfirmationNotification = "GrowlApplicationRegistrationConfirmationNotification";
        static public string Notification = "GrowlNotification";
        static public string Shutdown = "GrowlShutdown";
        static public string Ping = "Honey, Mind Taking Out The Trash";
        static public string Pong = "What Do You Want From Me, Woman";
        static public string IsReady = "Lend Me Some Sugar; I Am Your Neighbor!";
        static public string Clicked = "GrowlClicked!";
        static public string TimedOut = "GrowlTimedOut!";
        static public string ClickedContext = "ClickedContext";
        static public string RegDictExtension = "growlRegDict";
        static public string GrowlSelectedPosition = "GrowlSelectedPosition";
    }
}