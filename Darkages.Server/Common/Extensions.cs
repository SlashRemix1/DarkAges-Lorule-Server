﻿///************************************************************************
//Project Lorule: A Dark Ages Server (http://darkages.creatorlink.net/index/)
//Copyright(C) 2018 TrippyInc Pty Ltd
//
//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.
//
//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.
//
//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
//*************************************************************************/
using Mono.CSharp;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Darkages.Common
{

    public static class Extensions
    {
        public static void Times(this int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }

        public static T Eval<T>(this string code) where T : class
        {
            return Evaluator.Evaluate(code) as T;
        }

        public static void Run(this string code, bool repQuotes = false)
        {
            var run = repQuotes ?
                        code.Replace("'", "\"") : code;
            Evaluator.Run(run);
        }

        private static readonly Encoding encoding = Encoding.GetEncoding(949);

        public static byte[] ToByteArray(this string str)
        {
            return encoding.GetBytes(str);
        }

        public static string ToHexString(this byte[] Bytes)
        {
            return string.Concat(Bytes.Select(b => "0x" + b.ToString("X2") + " ")).TrimEnd(new char[] { ' ' });
        }

        public static bool IsWithin(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }

        public static int Clamp(this int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;

            return value;
        }
    }
}
