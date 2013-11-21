//
// Copyright (C) 2008-2013 Kody Brown (kody@bricksoft.com).
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
using Bricksoft.PowerCode;

namespace cat.delimited
{
	public class tab : ICataloger
	{
		public string Name { get { return _name; } }
		private string _name = "tab";

		public string Description { get { return _description; } }
		private string _description = "Formatted tab-separated files (.tab).";

		public bool CanCat( CatOptions catOptions, string fileName )
		{
			return Path.GetExtension(fileName).Equals(".tab", StringComparison.CurrentCultureIgnoreCase);
		}

		public bool Cat( CatOptions catOptions, string fileName )
		{
			return Cat(catOptions, fileName, 0, long.MaxValue);
		}

		public bool Cat( CatOptions catOptions, string fileName, int lineStart, long linesToWrite )
		{
			csv c = new csv();
			return c.Cat(catOptions, fileName, lineStart, linesToWrite);
		}
	}
}
