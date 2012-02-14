
namespace CustomNSFormatter
{
	
	// Should subclass MonoMac.AppKit.NSWindow
	[MonoMac.Foundation.Register("MainWindow")]
	public partial class MainWindow
	{
	}
	
	// Should subclass MonoMac.AppKit.NSWindowController
	[MonoMac.Foundation.Register("MainWindowController")]
	public partial class MainWindowController
	{
	}
	
	[MonoMac.Foundation.Register ("CustomURIFormatter")]
	partial class CustomURIFormatter
	{
	}
}

