//
//  Options.cs
//
//  Author:
//       Benito Palacios Sánchez <benito356@gmail.com>
//
//  Copyright (c) 2015 Benito Palacios Sánchez
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using CommandLine;
using CommandLine.Text;

namespace BiTexer
{
	public class Options
	{
		[Option('i', "inputFile", Required = true, HelpText = "Input file to read.")]
		public string InputFile { get; set; }

		[Option('o', "outputFile", Required = true, HelpText = "Output file to write.")]
		public string OutputFile { get; set; }

		[Option('m', "model", Required = true, HelpText = "Number of the model to process.")]
		public int Model { get; set; }

		[Option('p', "polygon", Required = true, HelpText = "Number of the polygon to process.")]
		public int Polygon { get; set; }

		[OptionArray('s', "originalSize", Required = true, HelpText = "Size of the original texture.")]
		public int[] OriginalSize { get; set; }

		public Size GetOriginalSize()
		{
			return new Size(OriginalSize[0], OriginalSize[1]);
		}

		[Option('S', "newSize", Required = true, HelpText = "Size of the nwe texture.")]
		public int[] NewSize { get; set; }

		public Size GetNewSize()
		{
			return new Size(NewSize[0], NewSize[1]);
		}
	}
}

