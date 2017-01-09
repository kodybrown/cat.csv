/*!
	Copyright (C) 2005-2007 Kody Brown (kody@bricksoft.com).

	MIT License:

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to
	deal in the Software without restriction, including without limitation the
	rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
	sell copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
	DEALINGS IN THE SOFTWARE.
*/

using System;
using GuidFileType = System.Collections.Generic.KeyValuePair<System.Guid, Bricksoft.PowerCode.FileType>;

namespace Bricksoft.PowerCode
{

	// Form.LoadData():

	//} else if (DataType == typeof(Dictionary<string, FileType>)) {
	//   Dictionary<string, FileType> dictionary;
	//   if (DataNode.ChildNodes == null || DataNode.ChildNodes.Count == 0) {
	//      return DefaultValue ?? new Dictionary<string, FileType>();
	//   }
	//   dictionary = new Dictionary<string, FileType>();
	//   foreach (XmlNode childNode in DataNode.ChildNodes) {
	//      if (childNode.Name.Equals("KeyValuePair")) {
	//         string key = childNode.CreateAttribute("Key", string.Empty).Value;
	//         if (!string.IsNullOrEmpty(key)) {
	//            FileType value = (FileType)LoadData(doc, childNode, null, typeof(FileType));
	//            if (value != null && key.Equals(value.Name)) {
	//               dictionary.Add(key, value);
	//            }
	//         }
	//      }
	//   }
	//   return dictionary;
	//} else if (DataType == typeof(Dictionary<Guid, FileType>)) {
	//   Dictionary<Guid, FileType> dictionary;
	//   if (DataNode.ChildNodes == null || DataNode.ChildNodes.Count == 0) {
	//      return DefaultValue ?? new Dictionary<Guid, FileType>();
	//   }
	//   dictionary = new Dictionary<Guid, FileType>();
	//   foreach (XmlNode childNode in DataNode.ChildNodes) {
	//      if (childNode.Name.Equals("KeyValuePair")) {
	//         string key = childNode.CreateAttribute("Key", string.Empty).Value;
	//         if (!string.IsNullOrEmpty(key)) {
	//            Guid guid = new Guid(key);
	//            FileType value = (FileType)LoadData(doc, childNode, null, typeof(FileType));
	//            if (value != null && guid.Equals(value.Guid)) {
	//               dictionary.Add(guid, value);
	//            }
	//         }
	//      }
	//   }
	//   return dictionary;
	//} else if (DataType == typeof(List<FileType>)) {
	//   List<FileType> list;
	//   if (DataNode.ChildNodes == null || DataNode.ChildNodes.Count == 0) {
	//      return DefaultValue ?? new List<FileType>();
	//   }
	//   list = new List<FileType>();
	//   foreach (XmlNode childNode in DataNode.ChildNodes) {
	//      if (childNode.Name.Equals("FileType")) {
	//         FileType value = (FileType)LoadData(doc, childNode, null, typeof(FileType));
	//         if (value != null) {
	//            list.Add(value);
	//         }
	//      }
	//   }
	//   return list;
	//} else if (DataType == typeof(FileType)) {
	//   FileType fileType;
	//   if (DataNode.ChildNodes == null || DataNode.ChildNodes.Count == 0) {
	//      return DefaultValue ?? new FileType();
	//   }
	//   fileType = new FileType();
	//   foreach (XmlNode childNode in DataNode.ChildNodes) {
	//      if (childNode.Name.Equals("Guid")) {
	//         fileType.Guid = new Guid(childNode.InnerText);
	//      } else if (childNode.Name.Equals("Name")) {
	//         fileType.Name = childNode.InnerText;
	//      } else if (childNode.Name.Equals("Delimiter")) {
	//         fileType.Delimiter = ConvertFromASCII(childNode.InnerText); //.Replace("{09}", "\t");
	//      } else if (childNode.Name.Equals("Description")) {
	//         fileType.Description = childNode.InnerText;
	//      } else if (childNode.Name.Equals("FormatExtensions")) {
	//         fileType.FileExtensions = (string[])LoadData(doc, childNode, null, typeof(string[]));
	//      }
	//   }
	//   if (fileType.IsValid) {
	//      return fileType;
	//   }else{
	//      return DefaultValue;
	//   }


	// Form.SaveData():

	//} else if (DataType == typeof(Dictionary<string, FileType>)) {
	//   foreach (KeyValuePair<string, FileType> keyPair in (Dictionary<string, FileType>)DataObj) {
	//      XmlNode childNode = XmlExtensions.GetOrCreateChild(doc, DataNode, "KeyValuePair", "Key", keyPair.Key, false);
	//      SaveData(doc, childNode, keyPair.Value, typeof(FileType));
	//      XmlExtensions.CreateAttribute(doc, childNode, "Key", keyPair.Key);
	//   }
	//} else if (DataType == typeof(Dictionary<Guid, FileType>)) {
	//   foreach (KeyValuePair<Guid, FileType> keyPair in (Dictionary<Guid, FileType>)DataObj) {
	//      XmlNode childNode = XmlExtensions.GetOrCreateChild(doc, DataNode, "KeyValuePair", "Key", keyPair.Key.ToString(), false);
	//      SaveData(doc, childNode, keyPair.Value, typeof(FileType));
	//      XmlExtensions.CreateAttribute(doc, childNode, "Key", keyPair.Key.ToString());
	//   }
	//} else if (DataType == typeof(List<FileType>)) {
	//   foreach (FileType fileType in (List<FileType>)DataObj) {
	//      XmlNode childNode = XmlExtensions.GetOrCreateChild(doc, DataNode, "FileType", true);
	//      SaveData(doc, childNode, fileType, typeof(FileType));
	//   }
	//} else if (DataType == typeof(FileType)) {
	//   FileType fileType = (FileType)DataObj;
	//   XmlExtensions.GetOrCreateChild(doc, DataNode, "Guid").InnerText = fileType.Guid.ToString();
	//   XmlExtensions.GetOrCreateChild(doc, DataNode, "Name").InnerText = fileType.Name;
	//   XmlExtensions.GetOrCreateChild(doc, DataNode, "Delimiter").InnerText = ConvertToASCII(fileType.Delimiter); //.Replace("\t", "{09}");
	//   XmlExtensions.GetOrCreateChild(doc, DataNode, "Description").InnerText = fileType.Description;
	//   XmlNode extNode = XmlExtensions.GetOrCreateChild(doc, DataNode, "FormatExtensions");
	//   SaveData(doc, extNode, fileType.FileExtensions, typeof(string[]));



	/// <summary>
	/// DelimitedFile types.
	/// </summary>
	public class FileType
	{
		private Guid _guid = Guid.Empty;
		private string _name = string.Empty;
		private string _delimiter = string.Empty;
		private string[] _fileExts = new string[] { };
		private string _description = string.Empty;

		/// <summary>
		/// Gets or sets the guid.
		/// </summary>
		public Guid Guid { get { return _guid; } set { _guid = value; } }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get { return _name; } set { _name = (value != null) ? value.Trim() : string.Empty; } }

		/// <summary>
		/// Gets or sets the delimiter.
		/// </summary>
		public string Delimiter { get { return _delimiter; } set { _delimiter = (value != null) ? value : string.Empty; } }

		/// <summary>
		/// Gets or sets the file extensions.
		/// </summary>
		public string[] FileExtensions {
			get { return _fileExts; }
			set {
				if (value == null) {
					_fileExts = new string[] { };
				} else {
					_fileExts = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		public string Description { get { return _description; } set { _description = (value != null) ? value.Trim() : string.Empty; } }

		/// <summary>
		/// Creates a new instance of the class.
		/// </summary>
		public FileType()
		{
			_guid = Guid.NewGuid();
			this.Name = string.Empty;
			this.Delimiter = string.Empty;
			this.FileExtensions = new string[] { };
			this.Description = string.Empty;
		}

		/// <summary>
		/// Creates a new instance of the class.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="delimiter"></param>
		/// <param name="fileExtensions"></param>
		/// <param name="description"></param>
		public FileType( string name, string delimiter, string[] fileExtensions, string description )
		{
			_guid = Guid.NewGuid();
			this.Name = name;
			this.Delimiter = delimiter;
			this.FileExtensions = fileExtensions;
			this.Description = description;
		}

		/// <summary>
		/// Determines whether this instance and <paramref name="fileType"/> are the same.
		/// </summary>
		/// <param name="fileType"></param>
		/// <returns></returns>
		public bool Equals( FileType fileType )
		{
			if (fileType == null) {
				return false;
			}
			//if (!_guid.Equals(fileType.Guid))
			//{
			//   return false;
			//}
			if (!Name.Equals(fileType.Name, StringComparison.InvariantCultureIgnoreCase)) {
				return false;
			}
			if (!Delimiter.Equals(fileType.Delimiter, StringComparison.InvariantCultureIgnoreCase)) {
				return false;
			}
			if (!FileExtensions.CompareTo(fileType.FileExtensions, StringComparison.InvariantCultureIgnoreCase, true)) {
				return false;
			}
			if (!Description.Equals(fileType.Description, StringComparison.InvariantCultureIgnoreCase)) {
				return false;
			}

			return true;
		}

		/// <summary>
		/// Returns the current instance's <seealso cref="Description"/>.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			//return string.Format("{0}|{1}|{2}|{3}", Name, Delimiter, GetFileExtensions(), Description);
			return Description;
		}

		/// <summary>
		/// Returns whether the current FileType is valid.
		/// </summary>
		public bool IsValid { get { return (Name != null && Name.Length > 0) && (Delimiter != null && Delimiter.Length > 0) && FileExtensions.Length > 0; } }

		/// <summary>
		/// Returns a string to be used in a File Open dialog.
		/// </summary>
		/// <returns></returns>
		public string GetFileDialogString()
		{
			if (FileExtensions.Length == 1) {
				return string.Format("{0} (*{1})|*{1}", Description, FileExtensions[0]);
			} else {
				return string.Format("{0} ({1})|{1}", Description, GetFileExtensions());
			}
		}

		/// <summary>
		/// Returns a portion of the string to be used in a File Open dialog.
		/// </summary>
		/// <returns></returns>
		public string GetFileExtensions() { return GetFileExtensions(true); }

		/// <summary>
		/// Returns a portion of the string to be used in a File Open dialog.
		/// </summary>
		/// <param name="showStar"></param>
		/// <returns></returns>
		public string GetFileExtensions( bool showStar )
		{
			string exts = string.Empty;
			string star = showStar ? "*" : string.Empty;
			foreach (string ext in FileExtensions) {
				exts += string.Format(";{0}{1}", star, ext);
			}
			return exts.Substring(1);
		}
	}
}
