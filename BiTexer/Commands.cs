//
//  Commands.cs
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
	public enum Commands : byte
	{
		Unknown,
		NOP = 0x00,
		MTX_MODE = 0x10,
		MTX_PUSH = 0x11,
		MTX_POP = 0x12,
		MTX_STORE = 0x13,
		MTX_RESTORE = 0x14,
		MTX_IDENTITY = 0x15,
		MTX_LOAD_4x4 = 0x16,
		MTX_LOAD_4x3 = 0x17,
		MTX_MULT_4x4 = 0x18,
		MTX_MULT_4x3 = 0x19,
		MTX_MULT_3x3 = 0x1A,
		MTX_SCALE = 0x1B,
		MTX_TRANS = 0x1C,
		COLOR = 0x20,
		NORMAL = 0x21,
		TEXCOORD = 0x22,
		VTX_16 = 0x23,
		VTX_10 = 0x24,
		VTX_XY = 0x25,
		VTX_XZ = 0x26,
		VTX_YZ = 0x27,
		VTX_DIFF = 0x28,
		POLYGON_ATTR = 0x29,
		TEXIMAGE_PARAM = 0x2A,
		PLTT_BASE = 0x2B,
		DIF_AMB = 0x30,
		SPE_EMI = 0x31,
		LIGHT_VECTOR = 0x32,
		LIGHT_COLOR = 0x33,
		SHININESS = 0x34,
		BEGIN_VTXS = 0x40,
		END_VTXS = 0x41,
		SWAP_BUFFERS = 0x50,
		VIEWPORT = 0x60,
		BOX_TEST = 0x70,
		POS_TEST = 0x71,
		VEC_TEST = 0x72,
	}

	public static class CommandsExtension
	{
		public static bool IsVertex(this Commands cmd)
		{
			switch (cmd) {
			case Commands.VTX_16:
			case Commands.VTX_10:
			case Commands.VTX_XY:
			case Commands.VTX_XZ:
			case Commands.VTX_YZ:
			case Commands.VTX_DIFF:
				return true;

			default:
				return false;
			}
		}

		public static int ArgumentsSize(this Commands cmd)
		{
			switch (cmd) {
			case Commands.NOP:          	return 0;
			case Commands.MTX_MODE:     	return 1;
			case Commands.MTX_PUSH:     	return 0;
			case Commands.MTX_POP:      	return 1;
			case Commands.MTX_STORE:    	return 1;
			case Commands.MTX_RESTORE:  	return 1;
			case Commands.MTX_IDENTITY: 	return 0;
			case Commands.MTX_LOAD_4x4: 	return 16;
			case Commands.MTX_LOAD_4x3: 	return 12;
			case Commands.MTX_MULT_4x4: 	return 16;
			case Commands.MTX_MULT_4x3: 	return 12;
			case Commands.MTX_MULT_3x3: 	return 9;
			case Commands.MTX_SCALE:    	return 3;
			case Commands.MTX_TRANS:    	return 3;
			case Commands.COLOR:        	return 1;
			case Commands.NORMAL:       	return 1;
			case Commands.TEXCOORD:     	return 1;
			case Commands.VTX_16: 			return 2;
			case Commands.VTX_10: 			return 1;
			case Commands.VTX_XY: 			return 1;
			case Commands.VTX_XZ: 			return 1;
			case Commands.VTX_YZ: 			return 1;
			case Commands.VTX_DIFF: 		return 1;
			case Commands.POLYGON_ATTR:   	return 1;
			case Commands.TEXIMAGE_PARAM: 	return 1;
			case Commands.PLTT_BASE: 		return 1;
			case Commands.DIF_AMB: 			return 1;
			case Commands.SPE_EMI: 			return 1;
			case Commands.LIGHT_VECTOR: 	return 1;
			case Commands.LIGHT_COLOR: 		return 1;
			case Commands.SHININESS: 		return 32;
			case Commands.BEGIN_VTXS: 		return 1;
			case Commands.END_VTXS: 		return 0;
			case Commands.SWAP_BUFFERS: 	return 1;
			case Commands.VIEWPORT: 		return 1;
			case Commands.BOX_TEST: 		return 3;
			case Commands.POS_TEST:		 	return 2;
			case Commands.VEC_TEST:			return 1;
			default:   						return 0;
			}
		}
	}
}

