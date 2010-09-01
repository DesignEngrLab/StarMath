#region

using System;

#endregion

namespace StarMathLib
{
    public static partial class StarMath
    {
        private const int cellWidth = 16;
        private const int numDecimals = 3;

        /// <summary>
        ///   Makes the print string.
        /// </summary>
        /// <param name = "A">The A.</param>
        public static string MakePrintString(double[,] A)
        {
            var format = "{0:F" + numDecimals + "}";
            var p = "";
            for (var i = 0; i < A.GetLength(0); i++)
            {
                p += "| ";
                for (var j = 0; j < A.GetLength(1); j++)
                {
                    var numStr = string.Format(format, A[i, j]);
                    numStr = numStr.TrimEnd('0');
                    numStr = numStr.TrimEnd('.');
                    var padAmt = ((double)(cellWidth - numStr.Length)) / 2;
                    numStr = numStr.PadLeft((int)Math.Floor(padAmt + numStr.Length));
                    numStr = numStr.PadRight(cellWidth);
                    p += numStr + " |";
                }
                p += "\n";
            }
            return p;
        }

        /// <summary>
        ///   Makes the print string.
        /// </summary>
        /// <param name = "A">The A.</param>
        public static string MakePrintString(double[] A)
        {
            var format = "{0:F" + numDecimals + "}";
            var p = "";
            for (var i = 0; i < A.GetLength(0); i++)
            {
                p += "| ";
                var numStr = string.Format(format, A[i]);
                numStr = numStr.TrimEnd('0');
                numStr = numStr.TrimEnd('.');
                var padAmt = ((double)(cellWidth - numStr.Length)) / 2;
                numStr = numStr.PadLeft((int)Math.Floor(padAmt));
                numStr = numStr.PadRight((int)Math.Ceiling(padAmt));
                p += numStr + " |";
            }
            return p;
        }
    }
}