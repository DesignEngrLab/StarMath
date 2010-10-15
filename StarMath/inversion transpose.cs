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

namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Matrix Inversion & Transpose

        /// <summary>
        ///   Inverses the matrix A only if the matrix has already been
        ///   "triangularized" - meaning there are no elements in the bottom
        ///   triangle - A[i,j]=0.0 where j>i
        /// </summary>
        /// <param name = "A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>The inverted matrix, A^-1.</returns>
        public static double[,] inverseUpper(double[,] A)
        {
            if (A.GetLength(0) != A.GetLength(1))
                throw new Exception("Matrix cannnot be inverted. Can only invert sqare matrices.");

            var size = A.GetLength(0);
            var B = new double[size,size];
            B.Initialize();
            var t = new double[size];


            for (var j = 0; j < size; j++)
            {
                B[j, j] = 1/A[j, j];
                for (var i = 0; i < j; i++)
                    B[i, j] = A[i, j];
            }
            for (var j = 1; j < size; j++)
            {
                var Bjj = -B[j, j];
                for (var i = 0; i < j; i++)
                    t[i] = B[i, j];
                for (var i = 0; i < j; i++)
                {
                    double v;
                    if (i < j - 1)
                    {
                        v = 0.0;
                        for (var jj = i + 1; jj < j; jj++)
                            v += B[i, jj]*t[jj];
                    }
                    else v = 0;
                    B[i, j] = v + B[i, i]*t[i];
                }
                for (var ii = 0; ii < j; ii++)
                    B[ii, j] = Bjj*B[ii, j];
            }
            return B;
        }
        /// <summary>
        ///   Inverses the matrix A only if the matrix has already been
        ///   "triangularized" - meaning there are no elements in the bottom
        ///   triangle - A[i,j]=0.0 where j>i
        /// </summary>
        /// <param name = "A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>The inverted matrix, A^-1.</returns>
        public static double[,] inverseUpper(int[,] A)
        {
            return inverseUpper(castIntArrayToDouble(A));
        }

        /// <summary>
        ///   Inverses the matrix A only if the diagonal is all non-zero.
        ///   A[i,i] != 0.0
        /// </summary>
        /// <param name = "A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>The inverted matrix, A^-1.</returns>
        public static double[,] inverse(double[,] A)
        {
            // this code is adapted from http://users.erols.com/mdinolfo/matrix.htm
            // one constraint/caveat in this function is that the diagonal elts. cannot
            // be zero.
            // if the matrix is not square or is less than B 2x2, 
            // then this function won't work
            if (A.GetLength(0) != A.GetLength(1))
                throw new Exception("Matrix cannnot be inverted. Can only invert sqare matrices.");
            var size = A.GetLength(0);
            if (size == 1) return new[,] {{1/A[0, 0]}};

            var B = LUDecomposition(A);

            #region invert L

            for (var i = 0; i < size; i++)
                for (var j = i; j < size; j++)
                {
                    var x = 1.0;
                    if (i != j)
                    {
                        x = 0.0;
                        for (var k = i; k < j; k++)
                            x -= B[j, k]*B[k, i];
                    }
                    B[j, i] = x/B[j, j];
                }

            #endregion

            #region invert U

            for (var i = 0; i < size; i++)
                for (var j = i; j < size; j++)
                {
                    if (i == j) continue;
                    var sum = 0.0;
                    for (var k = i; k < j; k++)
                        sum += B[k, j]*((i == k) ? 1.0 : B[i, k]);
                    B[i, j] = -sum;
                }

            #endregion

            #region final inversion

            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                {
                    var sum = 0.0;
                    for (var k = ((i > j) ? i : j); k < size; k++)
                        sum += ((j == k) ? 1.0 : B[j, k])*B[k, i];
                    B[j, i] = sum;
                }

            #endregion

            return B;
        }
        /// <summary>
        ///   Inverses the matrix A only if the diagonal is all non-zero.
        ///   A[i,i] != 0.0
        /// </summary>
        /// <param name = "A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>The inverted matrix, A^-1.</returns>
        public static double[,] inverse(int[,] A)
        {
            return inverse(castIntArrayToDouble(A));
        }


        /// <summary>
        ///   Returns the LU decomposition of A in a new matrix.
        /// </summary>
        /// <param name = "A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>A matrix of equal size to A that combines the L and U. Here the diagonals belongs to L and the U's diagonal elements are all 1.</returns>
        public static double[,] LUDecomposition(double[,] A)
        {
            var size = A.GetLength(0);
            if (size != A.GetLength(1))
                throw new Exception("LU Decomposition can only be determined for square matrices.");
            var B = (double[,]) A.Clone();
            // normalize row 0
            for (var i = 1; i < size; i++) B[0, i] /= B[0, 0];

            for (var i = 1; i < size; i++)
            {
                for (var j = i; j < size; j++)
                {
                    // do a column of L
                    var sum = 0.0;
                    for (var k = 0; k < i; k++)
                        sum += B[j, k]*B[k, i];
                    B[j, i] -= sum;
                }
                if (i == size - 1) continue;
                for (var j = i + 1; j < size; j++)
                {
                    // do a row of U
                    var sum = 0.0;
                    for (var k = 0; k < i; k++)
                        sum += B[i, k]*B[k, j];
                    B[i, j] =
                        (B[i, j] - sum)/B[i, i];
                }
            }
            return B;
        }
        /// <summary>
        ///   Returns the LU decomposition of A in a new matrix.
        /// </summary>
        /// <param name = "A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>A matrix of equal size to A that combines the L and U. Here the diagonals belongs to L and the U's diagonal elements are all 1.</returns>
        public static double[,] LUDecomposition(int[,] A)
        {
            return LUDecomposition(castIntArrayToDouble(A));
        }

        /// <summary>
        ///   Returns the LU decomposition of A in a new matrix.
        /// </summary>
        /// <param name = "A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <param name = "L">The L matrix is output where the diagonal elements are included and not (necessarily) equal to one.</param>
        /// <param name = "U">The U matrix is output where the diagonal elements are all equal to one.</param>
        public static void LUDecomposition(double[,] A, out double[,] L, out double[,] U)
        {
            var size = A.GetLength(0);
            L = LUDecomposition(A);
            U = new double[size,size];
            for (var i = 0; i < size; i++)
            {
                U[i, i] = 1.0;
                for (var j = i + 1; j < size; j++)
                {
                    U[i, j] = L[i, j];
                    L[i, j] = 0.0;
                }
            }
        }
        /// <summary>
        ///   Returns the LU decomposition of A in a new matrix.
        /// </summary>
        /// <param name = "A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <param name = "L">The L matrix is output where the diagonal elements are included and not (necessarily) equal to one.</param>
        /// <param name = "U">The U matrix is output where the diagonal elements are all equal to one.</param>
        public static void LUDecomposition(int[,] A, out double[,] L, out double[,] U)
        {
            LUDecomposition(castIntArrayToDouble(A), out L, out U);
        }

        /// <summary>
        ///   Transposes the matrix, A.
        /// </summary>
        /// <param name = "A">The matrix to transpose. This matrix is unchanged by this function.</param>
        /// <returns>The transponse of A.</returns>
        public static double[,] transpose(double[,] A)
        {
            var CRowSize = A.GetLength(1);
            var CColSize = A.GetLength(0);

            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[j, i];
            return C;
        }
        /// <summary>
        ///   Transposes the matrix, A.
        /// </summary>
        /// <param name = "A">The matrix to transpose. This matrix is unchanged by this function.</param>
        /// <returns>The transponse of A.</returns>
        public static int[,] transpose(int[,] A)
        {
            var CRowSize = A.GetLength(1);
            var CColSize = A.GetLength(0);

            var C = new int[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[j, i];
            return C;
        }

        /// <summary>
        ///   Returns the determinant of matrix, A.
        /// </summary>
        /// <param name = "A">The input argument matrix. This matrix is unchanged by this function.</param>
        /// <exception cref = "Exception"></exception>
        /// <returns>a single value representing the matrix's determinant.</returns>
        public static double determinant(double[,] A)
        {
            if (A.GetLength(0) != A.GetLength(1))
                throw new Exception("The determinant is only possible for square matrices.");
            if ((A.GetLength(0) == 0) && (A.GetLength(1) == 0))
                return 0.0;
            if ((A.GetLength(0) == 1) && (A.GetLength(1) == 1))
                return A[0, 0];
            if ((A.GetLength(0) == 2) && (A.GetLength(1) == 2))
                return (A[0, 0]*A[1, 1]) - (A[0, 1]*A[1, 0]);
            if ((A.GetLength(0) == 3) && (A.GetLength(0) == 3))
                return (A[0, 0]*A[1, 1]*A[2, 2])
                       + (A[0, 1]*A[1, 2]*A[2, 0])
                       + (A[0, 2]*A[1, 0]*A[2, 1])
                       - (A[0, 0]*A[1, 2]*A[2, 1])
                       - (A[0, 1]*A[1, 0]*A[2, 2])
                       - (A[0, 2]*A[1, 1]*A[2, 0]);
            return determinantsMoreThan3(A);
        }
        /// <summary>
        ///   Returns the determinant of matrix, A.
        /// </summary>
        /// <param name = "A">The input argument matrix. This matrix is unchanged by this function.</param>
        /// <exception cref = "Exception"></exception>
        /// <returns>a single value representing the matrix's determinant.</returns>
        public static double determinant(int[,] A)
        {
            return determinant(castIntArrayToDouble(A));
        }

        /// <summary>
        ///   Returns the determinant of matrix, A. Only used internally for matrices larger than 3.
        /// </summary>
        /// <param name = "A">The input argument matrix. This matrix is unchanged by this function.</param>
        /// <returns>a single value representing the matrix's determinant.</returns>
        private static double determinantsMoreThan3(double[,] A)
        {
            var size = A.GetLength(0);
            double[,] L, U;
            LUDecomposition(A, out L, out U);
            var result = 1.0;
            for (var i = 0; i < size; i++)
                result *= L[i, i];
            return result;
        }

        #endregion
    }
}