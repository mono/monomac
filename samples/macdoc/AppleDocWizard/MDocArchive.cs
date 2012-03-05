using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using Ionic.Zip;

namespace macdoc
{
	public interface IMonodocArchive
	{
		string GetPathForType (Type t);
	}
	
	// This is an uncompiled 'en' directory of ECMA documentation
	public class DirectoryDocArchive : IMonodocArchive
	{
		string baseDir;
		
		public DirectoryDocArchive (string baseDir)
		{
			this.baseDir = baseDir;
		}
		
		public string GetPathForType (Type t)
		{
			
		}
	}
	
	// This represent a .zip archive of a ECMA API doc set
	public class EcmaDocArchive : IMonodocArchive
	{
		string baseDir;
		string originalArchivePath;
		// Provide a mapping between a type full name (e.g. System.String) and the file name of its documentation (in ecma doc case, a number)
		Dictionary<string, string> typeMapping = new Dictionary<string, string> ();
		
		private EcmaDocArchive (string originalArchivePath, string baseDir)
		{
			this.baseDir = baseDir;
			this.originalArchivePath = originalArchivePath;
			BuildTypeMapping ();
		}
		
		void BuildTypeMapping ()
		{
			int id;
			foreach (var file in Directory.EnumerateFiles (baseDir)) {
				var name = Path.GetFileName (file);
				if (!int.TryParse (name, out id))
					continue;
				var reader = XmlReader.Create (file);
				if (!reader.Read ())
					continue;
				if (!reader.MoveToAttribute ("FullName"))
					continue;
				var typeFullName = reader.ReadContentAsString ();
				if (string.IsNullOrEmpty (typeFullName))
					continue;
				typeMapping[typeFullName] = id.ToString ();
			}
		}
		
		public string GetPathForType (Type t)
		{
			string path;
			bool result = typeMapping.TryGetValue (t.FullName, out path);
			return result ? path : null;
		}
		
		public static EcmaDocArchive ExtractAndLoad (string archivePath)
		{
			if (!File.Exists (archivePath))
				throw new ArgumentException ("Archive file doesn't exists", "archivePath");
			
			var extractionDir = Path.Combine (Path.GetTempPath (), Path.GetFileNameWithoutExtension (archivePath));
			if (Directory.Exists (extractionDir))
				Directory.Delete (extractionDir, true);
			Directory.CreateDirectory (extractionDir);
			using (var zip = ZipFile.Read (archivePath))
				zip.ExtractAll (extractionDir);
			
			return new EcmaDocArchive (archivePath, extractionDir);
		}
		
		public void SaveBack ()
		{
			File.Copy (originalArchivePath, originalArchivePath + ".origin");
			File.Delete (originalArchivePath);
			using (var zip = new ZipArchive (originalArchivePath)) {
				zip.AddDirectory (baseDir);
				zip.Save ();
			}
		}
	}
}

