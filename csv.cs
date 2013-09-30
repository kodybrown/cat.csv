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

namespace cat.csv
{
	public class csv : ICataloger
	{
		public bool CanCat( CatOptions catOptions, string fileName )
		{
			return Path.GetExtension(fileName).Equals(".csv", StringComparison.CurrentCultureIgnoreCase);
		}

		public bool Cat( CatOptions catOptions, string fileName )
		{
			return Cat(catOptions, fileName, 0, long.MaxValue);
		}

		public bool Cat( CatOptions catOptions, string fileName, int lineStart, long linesToWrite )
		{
			string[] lines;
			Dictionary<int, int> maxLen = new Dictionary<int, int>();
			List<string[]> lineColumns = new List<string[]>();
			int lineNumber;
			int padLen;
			int winWidth = Console.WindowWidth - 1;
			string colPad;

			lines = File.ReadAllLines(fileName);
			lineStart = Math.Max(lineStart, 0);
			lineNumber = 0;
			padLen = catOptions.showLineNumbers ? 3 : 0;
			if (linesToWrite < 0) {
				linesToWrite = long.MaxValue;
			}
			colPad = string.Empty;

			DelimitedFileReader f = new DelimitedFileReader();
			f.OpenFile(fileName, new FileType("csv", ",", new string[] { ".csv" }, "csv"));

			// Get the maxlength of each column.
			for (int i = Math.Min(lines.Length, Math.Max(0, lineStart)); i < Math.Min(lineStart + linesToWrite, lines.Length); i++) {
				string[] ls = f.ParseLine(lines[i]);
				lineColumns.Add(ls);
				for (int c = 0; c < ls.Length; c++) {
					if (maxLen.ContainsKey(c)) {
						maxLen[c] = Math.Max(maxLen[c], ls[c].Length + 1);
					} else {
						maxLen.Add(c, ls[c].Length + 1);
					}
				}
			}

			StringBuilder res = new StringBuilder();

			foreach (string[] ls in lineColumns) {
				lineNumber++;

				if (catOptions.showLineNumbers) {
					Console.BackgroundColor = catOptions.lineNumBackColor;
					Console.ForegroundColor = catOptions.lineNumForeColor;
					Console.Write("{0," + padLen + "}", lineNumber);
					Console.BackgroundColor = catOptions.defaultBackColor;
					Console.ForegroundColor = catOptions.defaultForeColor;
				}

				res.Clear();
				for (int c = 0; c < ls.Length; c++) {
					//res.AppendFormat("{0,-" + maxLen[c] + "}, ", ls[c]);
					//res.AppendFormat("{0,-" + maxLen[c] + "} ", ls[c] + ",");
					res.AppendFormat("{0,-" + maxLen[c] + "}, ", ls[c]);
				}
				Console.WriteLine(res.ToString().TrimEnd(' ', ','));
			}

			return true;
		}
	}
}
