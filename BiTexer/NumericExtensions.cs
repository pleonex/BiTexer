//
//  UIntExtension.cs
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
	public static class NumericExtensions
	{
		public static float ToDouble(this uint value, int integer, int fractional)
		{
			int integerMask = 0;
			float point = 0;

			if (value >> (integer + fractional) == 1) {
				integerMask = (int)Math.Pow(2, integer + 1) - 1;
				long intPart = (value >> fractional) & integerMask;
				point = intPart - (long)Math.Pow(2, integer + 1);
			} else {
				integerMask = (int)Math.Pow(2, integer) - 1;
				point = ((value >> fractional) & integerMask);
			}

			// Fractional part
			int fractionalMask = (int)Math.Pow(2, fractional) - 1;
			point += (float)(value & fractionalMask) / (fractionalMask + 1);
			return point;
		}

		public static uint ToUInt32(this float value, int integer, int fractional)
		{
			long point = 0;
			int intPart = (int)Math.Floor(value);

			if (value < 0) {
				point = intPart + (1 << (integer + 1));
				point <<= fractional;

				point |= 1u << (integer + fractional);
			} else {
				point = intPart << fractional;
			}

			uint fractionalStep = 1u << fractional;
			uint fraction = (uint)((value - intPart) * fractionalStep);
			fraction &= (1u << fractional) - 1;
			point |= fraction;

			uint mask = (1u << (integer + fractional + 1)) - 1;
			return (uint)(point & mask);
		}
	}
}

