using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

using Monodoc;

namespace macdoc
{
	public class IndexUpdateManager
	{
		readonly string baseUserDir; // This is e.g. .config/MonoDoc, a user-specific place where we can write stuff
		readonly IEnumerable<string> sourceFiles; // this is $prefix/monodoc/sources folder
		
		Dictionary<string, string> md5sums;
		const string sumFile = "index_freshness";
		
		public event EventHandler UpdaterChange;
		
		public IndexUpdateManager (IEnumerable<string> sourceFiles, string baseUserDir)
		{
			Logger.Log ("Going to verify [{0}]", sourceFiles.Aggregate ((e1, e2) => e1 + ", " + e2));
			this.baseUserDir = baseUserDir;
			this.sourceFiles = sourceFiles;
		}
		
		public Task<bool> CheckIndexIsFresh ()
		{
			return Task.Factory.StartNew (() => {
				var path = Path.Combine (baseUserDir, sumFile);
				
				// Two cases can trigger index creation/re-creation:
				//   1- there is no search_index folder or no monodoc.index file (i.e. GetIndex or GetSearchIndex returns null)
				//   2- one of the doc source we use is stale
				if (AppDelegate.Root.GetIndex () == null || AppDelegate.Root.GetSearchIndex () == null) {
					// force stale state
					md5sums = new Dictionary<string, string> ();
				} else {
					if (File.Exists (path)) {
						try {
							md5sums = DeserializeDictionary (path);
						} catch {}
					}
					if (md5sums == null)
						md5sums = new Dictionary<string, string> ();
				}
				
				bool isFresh = true;
				HashAlgorithm hasher = MD5.Create ();
				
				foreach (var source in sourceFiles) {
					var hash = StringHash (hasher, File.OpenRead (source));
					string originalHash;
					if (md5sums.TryGetValue (source, out originalHash))
						isFresh &= originalHash.Equals (hash, StringComparison.OrdinalIgnoreCase);
					else
						isFresh = false;
					md5sums[source] = hash;
				}
				
				Logger.Log ("Index fresh? {0}", isFresh ? "yes" : "no");
				
				return IsFresh = isFresh;
			});
		}
		
		string StringHash (HashAlgorithm hasher, Stream stream)
		{
			return hasher.ComputeHash (stream).Select (b => String.Format("{0:X2}", b)).Aggregate (string.Concat);
		}
		
		Dictionary<string, string> DeserializeDictionary (string path)
		{
			if (!File.Exists (path))
				return new Dictionary<string, string> ();
			return File.ReadAllLines (path)
				.Where (l => !string.IsNullOrEmpty (l) && l[0] != '#') // Take non-empty, non-comment lines
				.Select (l => l.Split ('='))
				.Where (a => a != null && a.Length == 2)
				.ToDictionary (t => t[0].Trim (), t => t[1].Trim ());
		}
		
		void SerializeDictionary (string path, Dictionary<string, string> dict)
		{
			File.WriteAllLines (path, dict.Select (kvp => string.Format ("{0} = {1}", kvp.Key, kvp.Value)));
		}
		
		public void PerformSearchIndexCreation ()
		{
			FireSearchIndexCreationEvent (true);
			try {
				RootTree.MakeSearchIndex ();
			} catch (Exception e) {
				Logger.LogError ("Error making search index", e);
			}
			try {
				RootTree.MakeIndex ();
			} catch (Exception e) {
				Logger.LogError ("Error making normal index", e);
			}
			IsFresh = true;
			FireSearchIndexCreationEvent (false);
			if (md5sums != null)
				SerializeDictionary (Path.Combine (baseUserDir, sumFile), md5sums);
		}
		
		public void AdvertiseFreshIndex ()
		{
			FireSearchIndexCreationEvent (false);
		}
		
		void FireSearchIndexCreationEvent (bool status)
		{
			IsCreatingSearchIndex = status;
			Thread.MemoryBarrier ();
			var evt = UpdaterChange;
			if (evt != null)
				evt (this, EventArgs.Empty);
		}
		
		public bool IsCreatingSearchIndex {
			get;
			set;
		}
		
		public bool IsFresh {
			get;
			set;
		}
	}
}

