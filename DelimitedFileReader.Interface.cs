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

namespace Bricksoft.PowerCode
{
	/// <summary>
	/// Interface for file reader classes.
	/// </summary>
	public interface IDelimitedFileReader : IDisposable
	{
		/// <summary>
		/// Gets or sets the file name.
		/// </summary>
		string FileName { get; set; }

		/// <summary>
		/// Gets or sets the file type.
		/// </summary>
		FileType FileType { get; set; }

		/// <summary>
		/// Gets whether the end of the file has been reached.
		/// </summary>
		bool EndOfStream { get; }

		/// <summary>
		/// Gets whether the file reader is open.
		/// </summary>
		bool IsOpen { get; }

		/// <summary>
		/// Opens the file specified.
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		FileType OpenFile( string fileName );

		/// <summary>
		/// Opens the file specified.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fileType"></param>
		/// <returns></returns>
		FileType OpenFile( string fileName, FileType fileType );

		/// <summary>
		/// Closes the file reader.
		/// </summary>
		void Close();

		/// <summary>
		/// Reads a line from the file.
		/// </summary>
		/// <returns></returns>
		string[] ReadLine();

		/// <summary>
		/// Parses a line from the file (based on its delimiter) into an array.
		/// </summary>
		/// <param name="line"></param>
		/// <returns></returns>
		string[] ParseLine( string line );
	}
}
