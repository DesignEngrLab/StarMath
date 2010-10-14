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
 *     Please find further details and contact information on StarMath
 *     at http://starmath.codeplex.com/.
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarMath
{
    public static partial class StarMath
    {
        #region Min max functions.
        /// <summary>
        ///   Finds the maximum value in the given 2D double array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A double value that is the maximum of A</returns>
        public static double Max(double[,] A)
        {
            return JoinMatrixColumnsIntoVector(A).Max();
        }
        /// <summary>
        /// Finds the maximum value in the given 2D double array and returns the row and column indices along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <returns>
        /// the maximum value 
        /// </returns>
        public static double Max(double[,] A, out int rowIndex, out int colIndex)
        {
            var max = Max(A);
            rowIndex = A.GetLength(0);
            colIndex = -1;
            while (--rowIndex >= 0)
                if (GetRow(rowIndex, A).Contains(max))
                {
                    colIndex = Array.IndexOf(GetRow(rowIndex, A), max);
                    return max;
                }
            return double.NegativeInfinity;
        }
        /// <summary>
        ///   Finds the maximum value in the given 2D integer array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An integer value that is the maximum of A</returns>
        public static int Max(int[,] A)
        {
            return JoinMatrixColumnsIntoVector(A).Max();
        }
        /// <summary>
        /// Finds the maximum value in the given 2D double array and returns the row and column indices along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <returns>
        /// the maximum value 
        /// </returns>
        public static int Max(int[,] A, out int rowIndex, out int colIndex)
        {
            var max = Max(A);
            rowIndex = A.GetLength(0);
            colIndex = -1;
            while (--rowIndex >= 0)
                if (GetRow(rowIndex, A).Contains(max))
                {
                    colIndex = Array.IndexOf(GetRow(rowIndex, A), max);
                    return max;
                }
            return int.MinValue;
        }
        /// <summary>
        ///   Finds the minimum value in the given 2D double array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A double value that is the minimum of A</returns>
        public static double Min(double[,] A)
        {
            return JoinMatrixColumnsIntoVector(A).Min();
        }
        /// <summary>
        /// Finds the minimum value in the given 2D double array and returns the row and column indices along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <returns>
        /// the minimum value.
        /// </returns>
        public static double Min(double[,] A, out int rowIndex, out int colIndex)
        {
            var min = Min(A);
            rowIndex = A.GetLength(0);
            colIndex = -1;
            while (--rowIndex >= 0)
                if (GetRow(rowIndex, A).Contains(min))
                {
                    colIndex = Array.IndexOf(GetRow(rowIndex, A), min);
                    return min;
                }
            return double.PositiveInfinity;
        }
        /// <summary>
        ///   Finds the minimum value in the given 2D integer array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An integer value that is the minimum of A</returns>
        public static int Min(int[,] A)
        {
            return JoinMatrixColumnsIntoVector(A).Min();
        }
        /// <summary>
        /// Finds the minimum value in the given 2D double array and returns the row and column indices along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <returns>
        /// the minimum value.
        /// </returns>
        public static int Min(int[,] A, out int rowIndex, out int colIndex)
        {
            var min = Min(A);
            rowIndex = A.GetLength(0);
            colIndex = -1;
            while (--rowIndex >= 0)
                if (GetRow(rowIndex, A).Contains(min))
                {
                    colIndex = Array.IndexOf(GetRow(rowIndex, A), min);
                    return min;
                }
            return int.MaxValue;
        }

        
        /// <summary>
        ///   Finds the maximum value in the given 1D integer array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An integer value that is the maximum of A</returns>
        public static int Max(IList<int> A)
        {
            return A.Max();
        }
        /// <summary>
        ///   Finds the minimum value in the given 1D integer array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An integer value that is the minimum of A</returns>
        public static int Min(IList<int> A)
        {
            return A.Min();
        }
        /// <summary>
        ///   Finds the maximum value in the given 1D double array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An double value that is the maximum of A</returns>
        public static double Max(IList<double> A)
        {
            return A.Max();
        }
        /// <summary>
        ///   Finds the minimum value in the given 1D double array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An double value that is the minimum of A</returns>
        public static double Min(IList<double> A)
        {
            return A.Min();
        }

        /// <summary>
        /// Finds the minimum value in the given 1D double array and returns its index along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="index">The index.</param>
        /// <returns>
        /// the minimum value 
        /// </returns>
        public static double Min(IList<double> A, out int index)
        {
            var min = A.Min();
            index = A.IndexOf(min);
            return min;
        }
        /// <summary>
        /// Finds the minimum value in the given 1D double array and returns its index along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="index">The index.</param>
        /// <returns>
        /// the maximum value
        /// </returns>
        public static double Max(IList<double> A, out int index)
        {
            var max = A.Max();
            index = A.IndexOf(max);
            return max;
        }
        /// <summary>
        /// Finds the minimum value in the given 1D double array and returns its index along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="index">The index.</param>
        /// <returns>
        /// the minimum value 
        /// </returns>
        public static int Min(IList<int> A, out int index)
        {
            var min = A.Min();
            index = A.IndexOf(min);
            return min;
        }
        /// <summary>
        /// Finds the minimum value in the given 1D double array and returns its index along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="index">The index.</param>
        /// <returns>
        /// the maximum value
        /// </returns>
        public static int Max(IList<int> A, out int index)
        {
            var max = A.Max();
            index = A.IndexOf(max);
            return max;
        }

        /// <summary>
        ///   Finds all the indices for the specified find value.
        /// </summary>
        /// <param name = "FindVal">The find value.</param>
        /// <param name = "A">The A.</param>
        /// <returns></returns>
        public static IList<int> find(double FindVal, IList<double> A)
        {
            return A.Select((value, index) => new { Item = value, Position = index })
                .Where(x => x.Item == FindVal).Select(a => a.Position).ToList();
        }

        /// <summary>
        ///   Finds all the indices for the specified find value.
        /// </summary>
        /// <param name = "FindVal">The find value.</param>
        /// <param name = "A">The A.</param>
        /// <returns></returns>
        public static IList<int> find(int FindVal, IList<int> A)
        {
            return A.Select((value, index) => new { Item = value, Position = index })
                .Where(x => x.Item == FindVal).Select(a => a.Position).ToList();
        }

        /// <summary>
        /// Finds the [rowIndex, colIndex] for the specified find value.
        /// </summary>
        /// <param name="FindVal">The find value.</param>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static int[] find(double FindVal, double[,] A)
        {
            var rowIndex = A.GetLength(0);
            while (--rowIndex >= 0)
                if (GetRow(rowIndex, A).Contains(FindVal))
                {
                    var colIndex = Array.IndexOf(GetRow(rowIndex, A), FindVal);
                    return new[] { rowIndex, colIndex };
                }
            return null;
        }

        /// <summary>
        /// Finds the [rowIndex, colIndex] for the specified find value.
        /// </summary>
        /// <param name="FindVal">The find value.</param>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static int[] find(int FindVal, int[,] A)
        {
            var rowIndex = A.GetLength(0);
            while (--rowIndex >= 0)
                if (GetRow(rowIndex, A).Contains(FindVal))
                {
                    var colIndex = Array.IndexOf(GetRow(rowIndex, A), FindVal);
                    return new[] { rowIndex, colIndex };
                }
            return null;
        }

        /// <summary>
        /// Calculates the standard deviation assuming the whole population is provided (not sample st. dev.).
        /// </summary>
        /// <param name="A">An vector of integers, A.</param>
        /// <returns></returns>
        public static double standardDeviation(IList<int> A)
        {
            var mean = A.Average();
            var variance = A.Sum(a => (a - mean) * (a - mean));
            return Math.Sqrt(variance / A.Count);
        }
        /// <summary>
        /// Calculates the standard deviation assuming the whole population is provided (not sample st. dev.).
        /// </summary>
        /// <param name="A">An vector of doubles, A.</param>
        /// <returns></returns>
        public static double standardDeviation(IList<double> A)
        {
            var mean = A.Average();
            var variance = A.Sum(a => (a - mean) * (a - mean));
            return Math.Sqrt(variance / A.Count);
        }

        /// <summary>
        /// Calculates the standard deviation assuming the whole population is provided (not sample st. dev.).
        /// </summary>
        /// <param name="A">A matrix in integers, A.</param>
        /// <returns></returns>
        public static double standardDeviation(int[,] A)
        {
            var vectA = JoinMatrixColumnsIntoVector(A);
            var mean = vectA.Average();
            var variance = vectA.Sum(a => (a - mean) * (a - mean));
            return Math.Sqrt(variance / vectA.Count());
        }
        /// <summary>
        /// Calculates the standard deviation assuming the whole population is provided (not sample st. dev.).
        /// </summary>
        /// <param name="A">A matrix in doubles, A.</param>
        /// <returns></returns>
        public static double standardDeviation(double[,] A)
        {
            var vectA = JoinMatrixColumnsIntoVector(A);
            var mean = vectA.Average();
            var variance = vectA.Sum(a => (a - mean) * (a - mean));
            return Math.Sqrt(variance / vectA.Count());
        }

        #endregion
    }
}