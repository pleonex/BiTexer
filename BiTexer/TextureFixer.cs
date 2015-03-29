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
					ChangeTextureCoordinate(ref cmd, oldSize, newSize);
					commandsList.SetCommand(cmd);
				}

				if (cmd.Id.IsVertex() && checkNextVertex) {
					checkNextVertex = false;
					ChangeVertex(ref cmd, oldSize, newSize);
					commandsList.SetCommand(cmd);
				}
			}
		}

		private void ChangeTextureCoordinate(ref Command cmd, Size oldSize, Size newSize)
		{
			double x = (cmd.Arguments[0] & 0xFFF).ToDouble(true, 11, 4);
			double y = (cmd.Arguments[0] >> 16).ToDouble(true, 11, 4);
		}

		private void ChangeVertex(ref Command cmd, Size oldSize, Size newSize)
		{
			double x, y;
			switch (cmd.Id) {
			case Commands.VTX_16:
				x = (cmd.Arguments[0] & 0xFFFF).ToDouble(true, 3, 12);
				y = (cmd.Arguments[0] >> 16).ToDouble(true, 3, 12);
				break;

			case Commands.VTX_10:
				x = (cmd.Arguments[0] & 0x3FF).ToDouble(true, 3, 6);
				y = (cmd.Arguments[0] >> 10).ToDouble(true, 3, 6);
				break;

			case Commands.VTX_XY:
				x = (cmd.Arguments[0] & 0xFFFF).ToDouble(true, 3, 12);
				y = (cmd.Arguments[0] >> 16).ToDouble(true, 3, 12);
				break;

			case Commands.VTX_XZ:
				x = (cmd.Arguments[0] & 0xFFFF).ToDouble(true, 3, 12);
				break;

			case Commands.VTX_YZ:
				y = (cmd.Arguments[0] & 0xFFFF).ToDouble(true, 3, 12);
				break;
			}
		}
	}
}

