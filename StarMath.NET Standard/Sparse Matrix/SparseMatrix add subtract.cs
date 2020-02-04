// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-16-2015
// ***********************************************************************
// <copyright file="add subtract multiply.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Linq;

namespace StarMathLib
{
    public static partial class StarMath
    {
        /// <summary>
        /// Adds the specified SparseMatrix, B, to this 2D double array to create a new
        /// 2D double array.
        /// </summary>
        /// <param name="A">a.</param>
        /// <param name="B">The b.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static double[,] add(this double[,] A, SparseMatrix B)
        {
            var numRows = A.GetLength(0);
            if (B.NumRows != numRows) throw new ArithmeticException("Cannot add matrices of different sizes.");
            var numCols = A.GetLength(1);
            if (B.NumCols != numCols) throw new ArithmeticException("Cannot add matrices of different sizes.");

            var C = (double[,])A.Clone();

            for (var i = 0; i < numRows; i++)
            {
                var cell = B.RowFirsts[i];
                while (cell != null)
                {
                    C[i, cell.ColIndex] += cell.Value;
                    cell = cell.Right;
                }
            }
            return C;
        }
        /// <summary>
        /// Subtracts the specified SparseMatrix, B, to this 2D double array to create a new
        /// 2D double array.
        /// </summary>
        /// <param name="A">a.</param>
        /// <param name="B">The b.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static double[,] subtract(this double[,] A, SparseMatrix B)
        {
            var numRows = A.GetLength(0);
            if (B.NumRows != numRows) throw new ArithmeticException("Cannot add matrices of different sizes.");
            var numCols = A.GetLength(1);
            if (B.NumCols != numCols) throw new ArithmeticException("Cannot add matrices of different sizes.");

            var C = (double[,])A.Clone();

            for (var i = 0; i < numRows; i++)
            {
                var cell = B.RowFirsts[i];
                while (cell != null)
                {
                    C[i, cell.ColIndex] -= cell.Value;
                    cell = cell.Right;
                }
            }
            return C;
        }
    }
}
