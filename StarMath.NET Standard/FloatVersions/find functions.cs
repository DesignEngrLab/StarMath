// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="find functions.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Max matrix functions.

        /// <summary>
        /// Finds the maximum value in the given 2D float array
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <returns>A float value that is the maximum of A</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this float[,] A)
        {
            var max = float.NegativeInfinity;
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            for (var i = 0; i < numRows; i++)
                for (var j = 0; j < numCols; j++)
                    if (max < A[i, j]) max = A[i, j];
            return max;
        }

        /// <summary>
        /// Finds the maximum value in the given 2D float array and returns the row and column indices along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <returns>the maximum value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this float[,] A, out int rowIndex, out int colIndex)
        {
            var max = float.NegativeInfinity;
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            rowIndex = colIndex = -1;
            for (var i = 0; i < numRows; i++)
                for (var j = 0; j < numCols; j++)
                    if (max < A[i, j])
                    {
                        max = A[i, j];
                        rowIndex = i;
                        colIndex = j;
                    }
            return max;
        }
        #endregion

        #region Min matrix functions.

        /// <summary>
        /// Finds the minimum value in the given 2D float array
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <returns>A float value that is the minimum of A</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this float[,] A)
        {
            var min = float.PositiveInfinity;
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            for (var i = 0; i < numRows; i++)
                for (var j = 0; j < numCols; j++)
                    if (min > A[i, j]) min = A[i, j];
            return min;
        }

        /// <summary>
        /// Finds the minimum value in the given 2D float array and returns the row and column indices along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <returns>the minimum value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this float[,] A, out int rowIndex, out int colIndex)
        {
            var min = float.PositiveInfinity;
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            rowIndex = colIndex = -1;
            for (var i = 0; i < numRows; i++)
                for (var j = 0; j < numCols; j++)
                    if (min > A[i, j])
                    {
                        min = A[i, j];
                        rowIndex = i;
                        colIndex = j;
                    }
            return min;
        }
        #endregion

        #region Min and max vector functions.

        /// <summary>
        /// Finds the maximum value in the given 1D float array
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <returns>An float value that is the maximum of A</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this IList<float> A)
        {
            var max = float.NegativeInfinity;
            var numElts = A.Count;
            for (var i = 0; i < numElts; i++)
                if (max < A[i]) max = A[i];
            return max;
        }

        /// <summary>
        /// Finds the minimum value in the given 1D float array
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <returns>An float value that is the minimum of A</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this IList<float> A)
        {
            var min = float.PositiveInfinity;
            var numElts = A.Count;
            for (var i = 0; i < numElts; i++)
                if (min > A[i]) min = A[i];
            return min;
        }

        /// <summary>
        /// Finds the minimum value in the given 1D float array and returns its index along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="index">The index.</param>
        /// <returns>the minimum value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(this IList<float> A, out int index)
        {
            index = -1;
            var min = float.PositiveInfinity;
            var numElts = A.Count;
            for (var i = 0; i < numElts; i++)
                if (min > A[i])
                {
                    min = A[i];
                    index = i;
                }
            return min;
        }

        /// <summary>
        /// Finds the minimum value in the given 1D float array and returns its index along with it.
        /// </summary>
        /// <param name="A">The array to be searched for</param>
        /// <param name="index">The index.</param>
        /// <returns>the maximum value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(this IList<float> A, out int index)
        {
            index = -1;
            var max = float.NegativeInfinity;
            var numElts = A.Count;
            for (var i = 0; i < numElts; i++)
                if (max < A[i])
                {
                    max = A[i];
                    index = i;
                }
            return max;
        }

        #endregion

        #region Find where a particular value is in a vector or matrix.

        /// <summary>
        /// Finds all the indices for the specified find value.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="FindVal">The find value.</param>
        /// <returns>IList&lt;System.Int32&gt;.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<int> find(this IList<float> A, float FindVal)
        {
            return find(FindVal, A);
        }

        /// <summary>
        /// Finds all the indices for the specified find value.
        /// </summary>
        /// <param name="FindVal">The find value.</param>
        /// <param name="A">The A.</param>
        /// <returns>IList&lt;System.Int32&gt;.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<int> find(float FindVal, IList<float> A)
        {
            return A.Select((value, index) => new { Item = value, Position = index })
                .Where(x => x.Item == FindVal).Select(a => a.Position).ToList();
        }


        /// <summary>
        /// Finds the [rowIndex, colIndex] for the specified find value.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="FindVal">The find value.</param>
        /// <returns>System.Int32[].</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] find(this float[,] A, float FindVal)
        {
            return find(FindVal, A);
        }

        /// <summary>
        /// Finds the [rowIndex, colIndex] for the specified find value.
        /// </summary>
        /// <param name="FindVal">The find value.</param>
        /// <param name="A">The A.</param>
        /// <returns>System.Int32[].</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] find(float FindVal, float[,] A)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            for (var i = 0; i < numRows; i++)
                for (var j = 0; j < numCols; j++)
                    if (FindVal == A[i, j])
                        return new[] { i, j };
            return null;
        }

        #endregion
    }
}