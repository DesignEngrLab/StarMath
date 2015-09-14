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
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }
    }

    public partial class SparseMatrix
    {
        /// <summary>
        /// Adds the specified 2D double array, A to this sparse matrix and writes over
        /// this sparse matrix with the result.
        /// </summary>
        /// <param name="A">a.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void addInPlace(double[,] A)
        {
            throw new NotImplementedException();
            //var C = new double[numRows, numCols];

            //for (var i = 0; i  NumRows; i++)
            //    for (var j = 0; j != numCols; j++)
            //    {
            //        C[i, j] = 0.0;
            //        for (var k = 0; k != A.GetLength(1); k++)
            //            C[i, j] += A[i, k] * B[k, j];
            //    }
            //return C;
        }

        /// <summary>
        /// Adds the specified 2D double array, A to this sparse matrix to create a new
        /// 2D double array.
        /// </summary>
        /// <param name="A">a.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public double[,] add(double[,] A)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the specified 2D double array, A to this sparse matrix and writes over
        /// this sparse matrix with the result.
        /// </summary>
        /// <param name="A">a.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void addInPlace(SparseMatrix A)
        {
            if (NumRows != A.NumRows || NumCols != A.NumCols)
                throw new ArithmeticException(
                    "Adding Sparse Matrices can only be accomplished if both are the same size.");

            for (var i = 0; i < NumRows; i++)
            {
                var thisCell = RowFirsts[i];
                var ACell = A.RowFirsts[i];
                while (thisCell != null || ACell != null)
                {
                    if (thisCell == null || (ACell != null && ACell.ColIndex < thisCell.ColIndex))
                    {
                        AddCell(i, ACell.ColIndex, ACell.Value);
                        ACell = ACell.Right;
                    }
                    else if (ACell == null || thisCell.ColIndex < ACell.ColIndex)
                    {
                        thisCell = thisCell.Right;
                    }
                    else //then the two values must be at the same cell
                    {
                        thisCell.Value += ACell.Value;
                        thisCell = thisCell.Right;
                        ACell = ACell.Right;
                    }
                }
            }
        }


        /// <summary>
        /// Adds the specified 2D double array, A to this sparse matrix to create a new
        /// 2D double array.
        /// </summary>
        /// <param name="A">a.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public SparseMatrix add(SparseMatrix A)
        {
            if (NumRows != A.NumRows || NumCols != A.NumCols)
                throw new ArithmeticException("Adding Sparse Matrices can only be accomplished if both are the same size.");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Sums the rows.
        /// </summary>
        /// <returns>System.Double[].</returns>
        public double[] SumTheRows()
        {
            var rowSums = new double[NumRows];
            for (int i = 0; i < NumRows; i++)
            {
                var sum = 0.0;
                var cell = RowFirsts[i];
                while (cell != null)
                {
                    sum += cell.Value;
                    cell = cell.Right;
                }
                rowSums[i] = sum;
            }
            return rowSums;
        }
        /// <summary>
        /// Sums the columns.
        /// </summary>
        /// <returns>System.Double[].</returns>
        public double[] SumTheColumns()
        {
            var colSums = new double[NumCols];
            for (int i = 0; i < NumCols; i++)
            {
                var sum = 0.0;
                var cell = ColFirsts[i];
                while (cell != null)
                {
                    sum += cell.Value;
                    cell = cell.Down;
                }
                colSums[i] = sum;
            }
            return colSums;
        }

    }
}
