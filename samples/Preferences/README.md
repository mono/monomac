Preferences Window - MonoMac sample
=========
This is a sample **MonoMac** application that implements the preferences window as seen in many Mac applications.

How preferences windows work in Mac:
* Window uses NSToolbar as a tab bar
* Window title is the title of the currently selected tab
* Window is resize with animation when the tab is changed
* Every tab (view) should have same width, but height may vary

##How to use
You can copy the code to your own project if you will. Creating a new tab to preferences window is easy.
Just create a new view with contoller and make the controller to implement **IPreferencesTab** interface.
After this you can add your new controller to the tabs in **AwakeFromNib()** method of the **PreferencesWindowController** class.

##Licence
Copyright (C) 2013 Lauri Taimila

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
