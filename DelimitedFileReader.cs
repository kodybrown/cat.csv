//
// Copyright (C) 2005-2007 Kody Brown (kody@bricksoft.com).
//
// MIT License:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GuidFileType = System.Collections.Generic.KeyValuePair<System.Guid, Bricksoft.PowerCode.FileType>;

namespace Bricksoft.PowerCode
{
	/// <summary>
	/// Base class for file readers where each line is parsed.
	/// </summary>
	public class DelimitedFileReader : IDelimitedFileReader
	{

		// ----- Private member variables -----------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// The file name.
		/// </summary>
		protected string _fileName = string.Empty;

		/// <summary>
		/// The file reader.
		/// </summary>
		protected StreamReader Reader { get; set; }

		// ----- Public properties -----------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the file name.
		/// </summary>
		public string FileName { get { return _fileName; } set { _fileName = (value != null) ? value.Trim() : string.Empty; } }

		/// <summary>
		/// Gets or sets the type of file.
		/// </summary>
		public FileType FileType { get; set; }

		/// <summary>
		/// Gets or sets the value lines to check in order to figure out 
		/// what format the file is in.
		/// </summary>
		public int AutoDetectMinLines { get; set; }

		/// <summary>
		/// Gets whether the file reader is open.
		/// </summary>
		public bool IsOpen { get { return Reader != null; } }

		/// <summary>
		/// Gets whether the end of the file has been reached.
		/// </summary>
		public bool EndOfStream
		{
			get
			{
				if (Reader == null) {
					return true;
				}
				return Reader.EndOfStream;
			}
		}

		// ----- Constructor(s) -----------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Creates an instance of the class.
		/// </summary>
		public DelimitedFileReader()
		{
			Reader = null;
			FileType = null;
			AutoDetectMinLines = 10;
		}

		/// <summary>
		/// 
		/// </summary>
		~DelimitedFileReader()
		{
			Close();
		}

		/// <summary>
		/// Releases all resources used by the instance.
		/// </summary>
		public void Dispose()
		{
			Close();
			Reader.Dispose();
			Reader = null;
		}

		// ----- OpenFile() -----------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Opens the file specified.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public FileType OpenFile( string fileName ) { return OpenFile(fileName, null, 10); }

		/// <summary>
		/// Opens the file specified.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileType"></param>
		/// <returns></returns>
		public FileType OpenFile( string fileName, FileType fileType ) { return OpenFile(fileName, fileType, 10); }

		/// <summary>
		/// Opens the file specified.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileType"></param>
		/// <param name="autoDetectMinLines"></param>
		/// <returns></returns>
		public FileType OpenFile( string fileName, FileType fileType, int autoDetectMinLines )
		{
			AutoDetectMinLines = autoDetectMinLines; // must be before call to GetFileType()

			if (fileType == null) {
				fileType = DelimitedFileReader.GetFileType(fileName, AutoDetectMinLines);
			}

			if (fileType == null) {
				throw new Exception("Unknown file type");
			}

			FileName = fileName;
			FileType = fileType;

			try {
				Reader = File.OpenText(FileName);
			} catch (Exception) {
				Reader = null;
			}

			return fileType;
		}

		// ----- Close() -----------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Closes the delimited file reader.
		/// </summary>
		public void Close()
		{
			if (Reader != null) {
				Reader.Close();
			}
		}

		// ----- ReadLine() -----------------------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Reads the next line from the file.
		/// Each line is trimmed.
		/// Empty lines are ignored.
		/// </summary>
		/// <returns></returns>
		public string[] ReadLine()
		{
			string line;

			if (Reader == null) {
				throw new Exception("Error reading file: reader is null");
			}
			if (Reader.EndOfStream) {
				throw new Exception("End of file was reached, check EndOfStream property before calling ReadLine");
			}

			line = Reader.ReadLine().Trim();

			while (line.Length == 0 && !Reader.EndOfStream) {
				line = Reader.ReadLine().Trim();
			}

			if (line.Length == 0 && Reader.EndOfStream) {
				return new string[] { };
			}

			return ParseLine(line);
		}

		/// <summary>
		/// Parses <paramref name="line"/> into a string[] based on the file type's delimiter.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		public string[] ParseLine( string line )
		{
			string delimiter;
			List<string> lines;
			string curChar;
			string lastChar;
			bool inString;
			StringBuilder output;
			int Count;
			bool newCol;

			delimiter = FileType.Delimiter;
			lines = new List<string>();
			inString = false;
			output = new StringBuilder();
			Count = 0;
			newCol = false;

			//
			// DOE;JOHN;9123 SOUTH PARK DRIVE;SANDY;UT;84092;8015551212;;1996;DODGE;STRATUS ES;13XXJ56XTXN1DD686;102712;34841;11292001;205;C;1;106;LOF;"LUBE; OIL; AND FILTER";109.8;1;11141995;0;;0;2763;7041956;9602940;;8015554255
			//

			//curChar = StringUtil.ELeft(ref line, 1);
			curChar = StringExtensions.RemoveStart(ref line, 1);

			// This is where a comma-separated line is parsed into an array (lines). It is all done by hand
			// because .net's intrinsic methods are inadaquate. For instance you can't tell when
			// you've reached the end of a line, and there's no support for finding out how many columns there
			// are in that line. There are also bugs in their code in dealing with multiple quotes in a column, etc.

			while (curChar != "") {
				if (curChar == delimiter) {
					if (inString) {
						output.Append(curChar);
					} else {
						Count++;
						lines.Add(output.ToString()); //.Trim()
						output.Length = 0;
						newCol = true;
					}
				} else if (curChar == "\"") {
					if (inString && (line.Trim().Equals("") || line.Substring(0, 1) != "\"")) // maybe add an line.Trim().Equals("") ??
					{
						while (!line.StartsWith(delimiter) && curChar != "") {
							lastChar = curChar;
							curChar = StringExtensions.RemoveStart(ref line, 1);
							output.Append(curChar);
						};
						Count++;
						lines.Add(output.ToString()); //.Trim()
						output.Length = 0;
						newCol = false;
						inString = false;
						// dump the delimiter, if one follows
						if (line.StartsWith(delimiter)) {
							newCol = true;
							lastChar = curChar;
							curChar = StringExtensions.RemoveStart(ref line, 1);
						}
					} else if (!inString) {
						inString = true; // do not insert the quotes into the output
					} else {
						output.Append(curChar);
						lastChar = curChar; // remove the next double quote
						curChar = StringExtensions.RemoveStart(ref line, 1);
					}
				} else {
					output.Append(curChar);
				}

				lastChar = curChar;
				curChar = StringExtensions.RemoveStart(ref line, 1);
			}

			// catch the last entry of the line (if it exists)
			if (output.Length > 0 || newCol) {
				Count++;
				lines.Add(output.ToString()); //.Trim()
				output.Length = 0;
			}

			return lines.ToArray();
		}

		// ----- Static properties -----------------------------------------------------------------------------------------------------------------------------

		private static Dictionary<Guid, FileType> ms_fileTypes = new Dictionary<Guid, FileType>();

		/// <summary>
		/// Gets or sets the file types.
		/// </summary>
		public static Dictionary<Guid, FileType> FileTypes
		{
			get
			{
				if (null == ms_fileTypes) {
					ms_fileTypes = new Dictionary<Guid, FileType>();
				}
				return ms_fileTypes;
			}
			set
			{
				ms_fileTypes = value;
				if (null == ms_fileTypes) {
					ms_fileTypes = new Dictionary<Guid, FileType>();
				}
			}
		}

		/// <summary>
		/// CSV, comma-separated file.
		/// </summary>
		public static FileType CsvFileType = null;

		/// <summary>
		/// TAB, tab-separated file.
		/// </summary>
		public static FileType TabFileType = null;

		/// <summary>
		/// ;, semicolon-separated file.
		/// </summary>
		public static FileType SemiColonFileType = null;

		/// <summary>
		/// |, pipe-separated file.
		/// </summary>
		public static FileType PipeFileType = null;

		/// <summary>
		/// #, pound-separated file.
		/// </summary>
		public static FileType PoundFileType = null;

		/// <summary>
		/// Sets the default file types.
		/// </summary>
		public static void SetDefaultFileTypes()
		{
			DelimitedFileReader.FileTypes = new Dictionary<Guid, FileType>();
			CsvFileType = new FileType("CSV", ",", new string[] { ".csv" }, "Comma separated values");
			DelimitedFileReader.FileTypes.Add(CsvFileType.Guid, CsvFileType);
			TabFileType = new FileType("TAB", "\t", new string[] { ".tab" }, "TAB delimited");
			DelimitedFileReader.FileTypes.Add(TabFileType.Guid, TabFileType);
			SemiColonFileType = new FileType("SemiColon", ";", new string[] { ".sem", ".semi" }, "Semi-colon delimited");
			DelimitedFileReader.FileTypes.Add(SemiColonFileType.Guid, SemiColonFileType);
			PipeFileType = new FileType("Pipe", "|", new string[] { ".pipe" }, "Pipe delimited");
			DelimitedFileReader.FileTypes.Add(PipeFileType.Guid, PipeFileType);
			PoundFileType = new FileType("Pound", "#", new string[] { ".pound" }, "Pound-sign delimited");
			DelimitedFileReader.FileTypes.Add(PoundFileType.Guid, PoundFileType);
		}

		/// <summary>
		/// Attempts to get the file type of the specified <paramref name="fileName"/>.
		/// Tries to use the file's extension.
		/// If it cannot match the extension to a specific type, it will attempt to 
		/// get the file type by searching the file for a matching structure.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="minRows"></param>
		/// <returns></returns>
		public static FileType GetFileType( string fileName, int minRows )
		{
			foreach (GuidFileType guidFileType in DelimitedFileReader.FileTypes) {
				foreach (string ext in guidFileType.Value.FileExtensions) {
					if (fileName.EndsWith(ext, StringComparison.CurrentCultureIgnoreCase)) {
						return guidFileType.Value;
					}
				}
			}
			return GetFileTypeFromContents(fileName, minRows);
		}

		/// <summary>
		/// Attempts to get the file type by searching the file for a matching structure.
		/// </summary>
		/// <param name="FileName"></param>
		/// <param name="minRows"></param>
		/// <returns></returns>
		private static FileType GetFileTypeFromContents( string FileName, int minRows )
		{
			FileType fileType = null;
			DelimitedFileReader fileReader;
			Dictionary<int, int> colCounts;
			int rowIndex;
			int colRecCount;

			if (3 > minRows) {
				minRows = 3;
			}

			foreach (GuidFileType guidFileType in DelimitedFileReader.FileTypes) {
				using (fileReader = new DelimitedFileReader()) {

					try {
						fileReader.OpenFile(FileName, guidFileType.Value);
					} catch (Exception) {
						continue;
					}

					colCounts = new Dictionary<int, int>();
					rowIndex = 0;

					while (!fileReader.EndOfStream && minRows > rowIndex++) {
						string[] lines = fileReader.ReadLine();
						int columns = lines.Length;
						if (colCounts.ContainsKey(columns)) {
							colCounts[columns]++;
						} else {
							if (1 < columns) {
								colCounts.Add(columns, 1);
							}
						}
					}

					fileReader.Close();
				}

				if (rowIndex < minRows) {
					throw new Exception("There must be at least " + minRows + " rows in the file for auto-testing to work");
				}

				if (colCounts.Keys.Count == 1) {
					colRecCount = 0;
					colCounts.TryGetValue(new List<int>(colCounts.Keys)[0], out colRecCount);
					if (colRecCount == rowIndex - 1) {
						return guidFileType.Value;
					}
				}
			}

			return fileType;
		}

		/// <summary>
		/// Returns a string to be used in a File Open dialog.
		/// </summary>
		/// <param name="fileType"></param>
		/// <param name="includeAllFiles"></param>
		/// <returns></returns>
		public static string GetFileFilter( FileType fileType, bool includeAllFiles )
		{
			string retVal = fileType.GetFileDialogString();
			if (includeAllFiles) {
				retVal += "|All files (*.*)|*.*";
			}
			return retVal;
		}

		/// <summary>
		/// Returns a string to be used in a File Open dialog.
		/// </summary>
		/// <param name="includeAllFiles"></param>
		/// <returns></returns>
		public static string GetAllFileFilters( bool includeAllFiles )
		{
			string retVal = string.Empty;
			foreach (GuidFileType guidFileType in DelimitedFileReader.FileTypes) {
				retVal += "|" + guidFileType.Value.GetFileDialogString();
			}
			if (includeAllFiles) {
				retVal += "|All files (*.*)|*.*";
			}
			return retVal.Substring(1);
		}

		/// <summary>
		/// Returns a string to be used in a File Open dialog.
		/// </summary>
		/// <param name="includeAllFiles"></param>
		/// <returns></returns>
		public static string GetAllFileFiltersInOne( bool includeAllFiles )
		{
			string retVal = string.Empty;
			string allFilters = GetAllFileFilters(includeAllFiles);
			foreach (GuidFileType guidFileType in DelimitedFileReader.FileTypes) {
				retVal += ";" + guidFileType.Value.GetFileExtensions();
			}
			return string.Format("All supported files ({0})|{0}|{1}", retVal.Substring(1), allFilters);
		}

		/// <summary>
		/// Returns whether the specified extension exists.
		/// </summary>
		/// <param name="extension"></param>
		/// <returns></returns>
		public static bool ExtensionExists( string extension )
		{
			throw new Exception("method not implemented");
		}

		/// <summary>
		/// Returns all supported extensions.
		/// </summary>
		/// <returns></returns>
		public static List<string> GetAllExtensions()
		{
			List<string> extensions = new List<string>();
			foreach (GuidFileType guidFileType in DelimitedFileReader.FileTypes) {
				foreach (string ext in guidFileType.Value.FileExtensions) {
					extensions.Add(ext);
				}
			}
			return extensions;
		}

		/// <summary>
		/// Returns whether the specified delimiter exists.
		/// </summary>
		/// <param name="delimiter"></param>
		/// <returns></returns>
		public static bool DelimiterExists( string delimiter )
		{
			throw new Exception("method not implemented");
		}

		/// <summary>
		/// Returns all delimiters.
		/// </summary>
		/// <returns></returns>
		public static List<string> GetAllDelimiters()
		{
			List<string> delimiters = new List<string>();
			foreach (GuidFileType guidFileType in DelimitedFileReader.FileTypes) {
				delimiters.Add(guidFileType.Value.Delimiter);
			}
			return delimiters;
		}
	}
}
