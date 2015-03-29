//
//  CommandsList.cs
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
	public class CommandsList
	{
		const int EntriesInPack = 4;
		const int PackIdSize = 4;

		BinaryReader reader;
		private long endPosition;

		private Command command;
		private int entryIndex;
		private long idPosition;
		private long argumentsPosition;

		public CommandsList(Stream stream, int size)
		{
			BaseStream = stream;
			reader = new BinaryReader(stream);

			endPosition = stream.Position + size;
			argumentsPosition = stream.Position;

			StartReadNewPack();
		}

		private Stream BaseStream {
			get;
			set;
		}

		public bool Next()
		{
			if (BaseStream.Position == endPosition)
				return false;
				
			ReadCommand();

			if (entryIndex == EntriesInPack)
				StartReadNewPack();

			return true;
		}

		public Command GetCommand()
		{
			return command;
		}

		public void SetCommand(Command cmd)
		{
			command = cmd;

			// TODO: Write into the stream
		}

		private void StartReadNewPack()
		{
			entryIndex = 0;
			idPosition = argumentsPosition;
			argumentsPosition += PackIdSize;
		}

		private void ReadCommand()
		{
			BaseStream.Position = idPosition++;
			Commands id = (Commands)reader.ReadByte();

			BaseStream.Position = argumentsPosition;
			int argsSize = id.ArgumentsSize();
			argumentsPosition += argsSize * 4;

			uint[] args = new uint[argsSize];
			for (int i = 0; i < argsSize; i++)
				args[i] = reader.ReadUInt32();

			command = new Command(id, args);
			entryIndex++;
		}
	}
}

