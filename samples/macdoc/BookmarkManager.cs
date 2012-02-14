using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace macdoc
{
	public class BookmarkManagerEventsArgs : EventArgs
	{
		public BookmarkManager.Entry Entry { get; set; }
		public bool WasDeleted { get; set; }
	}
	
	public class BookmarkManager
	{
		public class Entry
		{
			public string Name { get; set; }
			public string Url { get; set; }
			public string Notes { get; set; }
		}
		
		readonly string storagePath;
		List<Entry> bookmarks;
		IList<Entry> readonlyVersion;
		XmlSerializer serializer = new XmlSerializer (typeof (List<Entry>));
		
		public event EventHandler<BookmarkManagerEventsArgs> BookmarkListChanged;
		
		public BookmarkManager (string storagePath)
		{
			this.storagePath = storagePath;
			LoadBookmarks ();
		}
		
		public IList<Entry> GetAllBookmarks ()
		{
			return readonlyVersion;
		}
		
		public void AddBookmark (Entry entry)
		{
			if (entry == null)
				throw new ArgumentNullException ("entry");
			bookmarks.Add (entry);
			FireChangedEvent (entry, false);
		}
		
		public bool DeleteBookmark (Entry entry)
		{
			var result = bookmarks.Remove (entry);
			if (result)
				FireChangedEvent (entry, true);
			return result;
		}
		
		public bool LoadBookmarks ()
		{
			var path = Path.Combine (storagePath, "bookmarks.xml");
			if (!File.Exists (path)) {
				bookmarks = new List<Entry> ();
				return false;
			}
			using (var file = File.OpenRead (path))
				bookmarks = (List<Entry>)serializer.Deserialize (file);
			
			readonlyVersion = bookmarks.AsReadOnly ();
			
			return true;
		}
		
		public void SaveBookmarks ()
		{
			if (!Directory.Exists (storagePath))
				Directory.CreateDirectory (storagePath);
			var path = Path.Combine (storagePath, "bookmarks.xml");
			using (var file = File.Create (path))
				serializer.Serialize (file, bookmarks);
		}
		
		void FireChangedEvent (Entry entry, bool wasRemoved)
		{
			var temp = BookmarkListChanged;
			if (temp != null)
				temp (this, new BookmarkManagerEventsArgs () { Entry = entry, WasDeleted = wasRemoved });
		}
	}
}

