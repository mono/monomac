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
        static public string GROWL_APP_NAME				            	= "ApplicationName";
        static public string GROWL_APP_ID				                	= "ApplicationId";
        static public string GROWL_APP_ICON				            	= "ApplicationIcon";
        static public string GROWL_NOTIFICATIONS_DEFAULT	            	= "DefaultNotifications";
        static public string GROWL_NOTIFICATIONS_ALL		            	= "AllNotifications";
        static public string GROWL_NOTIFICATIONS_HUMAN_READABLE_NAMES      = "HumanReadableNames";
        static public string GROWL_NOTIFICATIONS_DESCRIPTIONS              = "NotificationDescriptions";
        static public string GROWL_TICKET_VERSION		                	= "TicketVersion";      
    }
    
    public static class NotificationUserInfoKeys {
        static public string GROWL_NOTIFICATION_NAME		    	= "NotificationName";
        static public string GROWL_NOTIFICATION_TITLE	        	= "NotificationTitle";
        static public string GROWL_NOTIFICATION_DESCRIPTION    	= "NotificationDescription";
        static public string GROWL_NOTIFICATION_ICON		    	= "NotificationIcon";
        static public string GROWL_NOTIFICATION_APP_ICON	    	= "NotificationAppIcon";
        static public string GROWL_NOTIFICATION_PRIORITY	    	= "NotificationPriority";
        static public string GROWL_NOTIFICATION_STICKY	        	= "NotificationSticky";
        static public string GROWL_NOTIFICATION_CLICK_CONTEXT      = "NotificationClickContext";
        static public string GROWL_DISPLAY_PLUGIN				    = "NotificationDisplayPlugin";
        static public string GROWL_NOTIFICATION_IDENTIFIER   	    = "GrowlNotificationIdentifier";
        static public string GROWL_APP_PID				    	    = "ApplicationPID";
        static public string GROWL_NOTIFICATION_PROGRESS	    	= "NotificationProgress";
    }
    
    public static class GrowlNotifications {
        static public string GROWL_APP_REGISTRATION			= "GrowlApplicationRegistrationNotification";
        static public string GROWL_APP_REGISTRATION_CONF		= "GrowlApplicationRegistrationConfirmationNotification";
        static public string GROWL_NOTIFICATION				= "GrowlNotification";
        static public string GROWL_SHUTDOWN					= "GrowlShutdown";
        static public string GROWL_PING						= "Honey, Mind Taking Out The Trash";
        static public string GROWL_PONG						= "What Do You Want From Me, Woman";
        static public string GROWL_IS_READY					= "Lend Me Some Sugar; I Am Your Neighbor!";
        static public string GROWL_NOTIFICATION_CLICKED		= "GrowlClicked!";
        static public string GROWL_NOTIFICATION_TIMED_OUT	    = "GrowlTimedOut!";
        static public string GROWL_KEY_CLICKED_CONTEXT		    = "ClickedContext";
        static public string GROWL_REG_DICT_EXTENSION		    = "growlRegDict";
        static public string GROWL_POSITION_PREFERENCE_KEY		= "GrowlSelectedPosition";
    }
}