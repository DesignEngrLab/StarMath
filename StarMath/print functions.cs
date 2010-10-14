#region

using System;
using System.Collections.Generic;

#endregion

namespace StarMathLib
{
    public static partial class StarMath
    {
        private const int cellWidth = 10;
        private const int numDecimals = 3;

        /// <summary>
        ///   Makes the print string.
        /// </summary>
        /// <param name = "A">The A.</param>
        public static string MakePrintString(double[,] A)
        {
            if (A == null) return "<null>";
            var format = "{0:F" + numDecimals + "}";
            var p = "";
            for (var i = 0; i < A.GetLength(0); i++)
            {
                p += "| ";
                for (var j = 0; j < A.GetLength(1); j++)
                    p += formatCell(format, A[i, j]) + ",";
                p = p.Remove(p.Length - 1);
                p += " |\n";
            }
            p = p.Remove(p.Length - 1);
            return p;
        }

        private static object formatCell(string format, double p)
        {
            var numStr = string.Format(format, p);
            numStr = numStr.TrimEnd('0');
            numStr = numStr.TrimEnd('.');
            var padAmt = ((double)(cellWidth - numStr.Length)) / 2;
            numStr = numStr.PadLeft((int)Math.Floor(padAmt + numStr.Length));
            numStr = numStr.PadRight(cellWidth);
            return numStr;
        }

        /// <summary>
        ///   Makes the print string.
        /// </summary>
        /// <param name = "A">The A.</param>
        public static string MakePrintString(IList<double> A)
        {
            if (A == null) return "<null>";
            var format = "{0:F" + numDecimals + "}";
            var p = "{ ";
            for (var i = 0; i < A.Count; i++)
                p += formatCell(format, A[i]) + ",";
            p = p.Remove(p.Length - 1);
            p += " }";
            return p;
        }

        /// <summary>
        ///   Makes the print string.
        /// </summary>
        /// <param name = "A">The A.</param>
        public static string MakePrintString(int[,] A)
        {
            if (A == null) return "<null>";
            const string format = "{0}";
            var p = "";
            for (var i = 0; i < A.GetLength(0); i++)
            {
                p += "| ";
                for (var j = 0; j < A.GetLength(1); j++)
                    p += formatCell(format, A[i, j]) + ",";
                p = p.Remove(p.Length - 1);
                p += " |\n";
            }
            p = p.Remove(p.Length - 1);
            return p;
        }

        /// <summary>
        ///   Makes the print string.
        /// </summary>
        /// <param name = "A">The A.</param>
        public static string MakePrintString(IList<int> A)
        {
            if (A == null) return "<null>";
            const string format = "{0}";
            var p = "{ ";
            for (var i = 0; i < A.Count; i++)
                p += formatCell(format, A[i]) + ",";
            p = p.Remove(p.Length - 1);
            p += " }";
            return p;
        }
    }
}