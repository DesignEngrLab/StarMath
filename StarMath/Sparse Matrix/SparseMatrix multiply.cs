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
        public static double[,] multiply(this double[,] A, SparseMatrix B)
        {
            throw new NotImplementedException();
        }

        public static SparseMatrix multiplyToSparse(this double[,] A, SparseMatrix B)
        {
            throw new NotImplementedException();
        }
    }

    public partial class SparseMatrix
    {
        #region Scalars multiplying matrices

        /// <summary>
        /// Multiplies all elements of a 2D double array with a double value.
        /// </summary>
        /// <param name="B">The matrix to be multiplied with</param>
        /// <param name="a">The double value to be multiplied</param>
        /// <returns>A 2D double array that contains the product</returns>
        public void Multiply(double a)
        {
            foreach (var sparseCell in cellsRowbyRow)
                sparseCell.Value *= a;
        }

        public void Divide(double a)
        {
             Multiply(1/a);
        }

        #endregion

        #region Matrix(2D) to matrix(2D) multiplication

        public void multiplyInPlace(double[,] A)
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

        public double[,] multiply(double[,] A)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Multiply matrix to a vector (and vice versa)

        #endregion
    }
}
