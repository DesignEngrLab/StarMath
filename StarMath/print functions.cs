/*************************************************************************
 *     This file & class is part of the StarMath Project
 *     Copyright 2010 Matthew Ira Campbell, PhD.
 *
 *     StarMath is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *  
 *     StarMath is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 *  
 *     You should have received a copy of the GNU General Public License
 *     along with StarMath.  If not, see <http://www.gnu.org/licenses/>.
 *     
 *     Please find further details and contact information on GraphSynth
 *     at http://starmath.codeplex.com/.
 *************************************************************************/
using System;

namespace StarMathLib
{
    public static partial class StarMath
    {
        static int cellWidth = 16;
        static int numDecimals = 3;
        public static void ConsoleWrite(double[,] A)
        {
            string format = "{0:F" + numDecimals.ToString() + "}";
            for (int i = 0; i < A.GetLength(0); i++)
            {
                string p = "| ";
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    string numStr = string.Format(format, A[i, j]);
                    numStr = numStr.TrimEnd('0');
                    numStr = numStr.TrimEnd('.');
                    double padAmt = ((double)(cellWidth - numStr.Length)) / 2;
                    numStr = numStr.PadLeft((int)Math.Floor(padAmt + numStr.Length));
                    numStr = numStr.PadRight(cellWidth);
                    p += numStr + " |";
                }
                Console.WriteLine(p);
            }
        }
        public static void ConsoleWrite(double[] A)
        {
            string format = "{0:F" + numDecimals.ToString() + "}";
            for (int i = 0; i < A.GetLength(0); i++)
            {
                string p = "| ";
                var numStr = string.Format(format, A[i]);
                numStr = numStr.TrimEnd('0');
                numStr = numStr.TrimEnd('.');
                double padAmt = (cellWidth - numStr.Length) / 2;
                numStr = numStr.PadLeft((int)Math.Floor(padAmt));
                numStr = numStr.PadRight((int)Math.Ceiling(padAmt));
                p += numStr + " |";
                Console.WriteLine(p);
            }
        }
    }
}