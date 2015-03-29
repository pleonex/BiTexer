//
//  CommandIterator.cs
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
using System.IO;

namespace BiTexer
{
	public class ModelStream
	{
		public ModelStream(Stream stream)
		{
			BaseStream = stream;
		}

		public Stream BaseStream {
			get;
			private set;
		}

		public int MoveToCommandList(int model, int polygon)
		{
			var reader = new BinaryReader(BaseStream);
			long basePosition = BaseStream.Position;

			// MDL0 section offset
			BaseStream.Position = basePosition + 0x10;
			long mdl0Position   = basePosition + reader.ReadUInt32();

			// Number of models
			BaseStream.Position = mdl0Position + 0x09;
			int numModels = reader.ReadByte();
			if (model >= numModels) {
				Console.WriteLine("Error: Invalid number of model");
				return -1;
			}

			// Model offset
			BaseStream.Position = mdl0Position + 0x14 + numModels * 4 + 4 + model * 4;
			long modelPosition  = mdl0Position + reader.ReadUInt32();

			// Polygon offset
			BaseStream.Position  = modelPosition + 0x0C;
			long polygonPosition = modelPosition + reader.ReadUInt32();

			// Number of polygons
			BaseStream.Position = polygonPosition + 0x01;
			int numPolygons = reader.ReadByte();
			if (polygon >= numPolygons) {
				Console.WriteLine("Error: Invalid number of polygons");
				return -1;
			}

			// Polygon header
			BaseStream.Position = polygonPosition + 0x0C + numPolygons * 4 + 4 + polygon * 4;
			long polygonHeaderPosition = polygonPosition + reader.ReadUInt32();

			// Commands list offset
			BaseStream.Position = polygonHeaderPosition + 0x08;
			long commandsListPosition = polygonHeaderPosition + reader.ReadUInt32();
			int commandsListSize = reader.ReadInt32();

			// Move to commands list
			BaseStream.Position = commandsListPosition;
			return commandsListSize;
		}
	}
}

