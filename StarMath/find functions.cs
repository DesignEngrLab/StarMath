using System;
using System.Collections.Generic;
using System.Linq;

namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Min max functions etc..

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
        ///   Finds the minimum value in the given 2D integer array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An integer value that is the minimum of A</returns>
        public static int Min(int[,] A)
        {
            return JoinMatrixColumnsIntoVector(A).Min();
        }

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
        ///   Finds the maximum value in the given 2D integer array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An integer value that is the maximum of A</returns>
        public static int Max(int[,] A)
        {
            return JoinMatrixColumnsIntoVector(A).Max();
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
        /// Finds the minimum and maximum value in the given 2D double array
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="max">The max.</param>
        /// <param name="min">The min.</param>
        public static void MinMax(double[,] A, out double max, out double min)
        {
            max = Max(A);
            min = Min(A);
        }


        /// <summary>
        /// Finds the minimum and maximum value in the given 2D double array
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="max">The max.</param>
        /// <param name="min">The min.</param>
        public static void MinMax(int[,] A, out int max, out int min)
        {
            max = Max(A);
            min = Min(A);
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
        ///   Finds the minimum and maximum value in the given 2D double array and returns the row and column indices
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A 1 x 6 double array that contains the min value and its row and col index and the max value and its row and col index in A</returns>
        public static double[] MinMaxRC(double[,] A)
        {
            var MinDim1 = 0; //Dim1 = rows
            var MinDim2 = 0; //Dim2 = columns
            var MaxDim1 = 0; //Dim1 = rows
            var MaxDim2 = 0; //Dim2 = columns
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Max = double.MinValue;
            var Min = double.MaxValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                    {
                        MaxDim1 = i;
                        MaxDim2 = j;
                        Max = A[i, j];
                    }
                    if (A[i, j] >= Min) continue;
                    MinDim1 = i;
                    MinDim2 = j;
                    Min = A[i, j];
                }
            }
            return (new[] { Min, MinDim1, MinDim2, Max, MaxDim1, MaxDim2 });
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