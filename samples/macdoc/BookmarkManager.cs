using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace macdoc
{
	public enum BookmarkEventType {
		Added,
		Deleted,
		Modified
	}
	
	public class BookmarkManagerEventsArgs : EventArgs
	{
		public BookmarkManager.Entry Entry { get; set; }
		public BookmarkEventType EventType { get; set; }
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
			RenameIfDuplicated (entry);
			bookmarks.Add (entry);
			FireChangedEvent (entry, BookmarkEventType.Added);
		}
		
		void RenameIfDuplicated (Entry entry)
		{
			var duplicatedNames = new HashSet<string> (bookmarks.Where (b => b != entry && b.Name.StartsWith (entry.Name)).Select (b => b.Name));
			if (duplicatedNames.Count == 0)
				return;
			var nameTry = entry.Name;
			var suffixCount = 1;
			while (duplicatedNames.Contains (nameTry))
				nameTry = entry.Name + '_' + (suffixCount++);
			entry.Name = nameTry;
		}
		
		public bool DeleteBookmark (Entry entry)
		{
			var result = bookmarks.Remove (entry);
			if (result)
				FireChangedEvent (entry, BookmarkEventType.Deleted);
			return result;
		}
		
		public bool LoadBookmarks ()
		{
			var path = Path.Combine (storagePath, "bookmarks.xml");
			if (!File.Exists (path)) {
				bookmarks = new List<Entry> ();
				readonlyVersion = bookmarks.AsReadOnly ();
				return false;
			}
			using (var file = File.OpenRead (path))
				bookmarks = (List<Entry>)serializer.Deserialize (file);
			// Sanitize input to possibly remove buggy elements (i.e. missing url, name, etc..)
			bookmarks = bookmarks.Where (b => b != null && !string.IsNullOrWhiteSpace (b.Name) && !string.IsNullOrWhiteSpace (b.Url)).ToList ();
			readonlyVersion = bookmarks.AsReadOnly ();
			
			return true;
		}
		
		public void SaveBookmarks ()
		{
			if (!Directory.Exists (storagePath))
				Directory.CreateDirectory (storagePath);
			var path = Path.Combine (storagePath, "bookmarks.xml");
			try {
				using (var file = File.Create (path))
					serializer.Serialize (file, bookmarks);
			} catch (UnauthorizedAccessException) {}
		}
		
		public void CommitBookmarkChange (Entry entry)
		{
			RenameIfDuplicated (entry);
			FireChangedEvent (entry, BookmarkEventType.Modified);
		}
		
		public int FindIndexOfBookmarkFromUrl (string url)
		{
			return bookmarks.FindIndex (entry => entry.Url.Equals (url, StringComparison.InvariantCultureIgnoreCase));
		}
		
		void FireChangedEvent (Entry entry, BookmarkEventType evtType)
		{
			var temp = BookmarkListChanged;
			if (temp != null)
				temp (this, new BookmarkManagerEventsArgs () { Entry = entry, EventType = evtType });
		}
	}
}

