//
//  TextureFixer.cs
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

namespace BiTexer
{
	public class TextureFixer
	{
		public TextureFixer(ModelStream model)
		{
			Model = model;
		}

		public ModelStream Model {
			get;
			private set;
		}

		public void Fix(int model, int polygon, Size oldSize, Size newSize)
		{
			var commandsList = Model.GetCommandsList(model, polygon);
			if (commandsList == null)
				return;

			bool checkNextVertex = false;
			while (commandsList.Next()) {
				Command cmd = commandsList.GetCommand();

				if (cmd.Id == Commands.TEXCOORD) {
					checkNextVertex = true;
					cmd = ChangeTextureCoordinate(cmd, oldSize, newSize);
					commandsList.SetCommand(cmd);
				}

				if (cmd.Id.IsVertex() && checkNextVertex) {
					checkNextVertex = false;
					cmd = ChangeVertex(cmd, oldSize, newSize);
					commandsList.SetCommand(cmd);
				}
			}
		}

		private static Command ChangeTextureCoordinate(Command cmd, Size oldSize, Size newSize)
		{
			uint arg = 0;
			arg |= UpdateValue(cmd.Arguments[0], oldSize.Width, newSize.Width, 0, 11, 4);
			arg |= UpdateValue(cmd.Arguments[0], oldSize.Height, newSize.Height, 1, 11, 4);

			return new Command(cmd.Id, new uint[] { arg });
		}

		private static Command ChangeVertex(Command cmd, Size oldSize, Size newSize)
		{
			uint[] args = new uint[1];
			switch (cmd.Id) {
			case Commands.VTX_16:
				args = new uint[2];
				args[0]  = UpdateValue(cmd.Arguments[0], oldSize.Width,  newSize.Width,  0, 3, 12);
				args[0] |= UpdateValue(cmd.Arguments[0], oldSize.Height, newSize.Height, 1, 3, 12);
				args[1]  = cmd.Arguments[1];	// No need to update coordinate Z
				break;

			case Commands.VTX_10:
				args[0]  = UpdateValue(cmd.Arguments[0], oldSize.Width,  newSize.Width,  0, 3, 6);
				args[0] |= UpdateValue(cmd.Arguments[0], oldSize.Height, newSize.Height, 1, 3, 6);
				args[0] |= UpdateValue(cmd.Arguments[0], 1, 1, 2, 3, 6);
				break;

			case Commands.VTX_XY:
				args[0]  = UpdateValue(cmd.Arguments[0], oldSize.Width,  newSize.Width,  0, 3, 12);
				args[0] |= UpdateValue(cmd.Arguments[0], oldSize.Height, newSize.Height, 1, 3, 12);
				break;

			case Commands.VTX_XZ:
				args[0] = UpdateValue(cmd.Arguments[0], oldSize.Width, newSize.Width, 0, 3, 12);
				args[0] |= UpdateValue(cmd.Arguments[0], 1, 1, 1, 3, 12);
				break;

			case Commands.VTX_YZ:
				args[0] = UpdateValue(cmd.Arguments[0], oldSize.Height, newSize.Height, 0, 3, 12);
				args[0] |= UpdateValue(cmd.Arguments[0], 1, 1, 1, 3, 12);
				break;
			}

			return new Command(cmd.Id, args);
		}

		private static uint UpdateValue(uint value, int refOriginal, int refNew, int i,
			int integerBits, int fractionalBits)
		{
			int bitsPerValue = integerBits + fractionalBits + 1; // sign bit too
			value >>= i * bitsPerValue;
			value &= (1u << bitsPerValue) - 1;

			float factor = (float)refNew / refOriginal;
			float coordinate = value.ToDouble(integerBits, fractionalBits);
			coordinate *= factor;

			uint newValue = coordinate.ToUInt32(integerBits, fractionalBits);
			newValue &= (1u << bitsPerValue) - 1;
			newValue <<= i * bitsPerValue;

			return newValue;
		}
	}
}

