// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="inversion transpose.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Matrix Inversion

        /// <summary>
        /// Inverses the matrix A only if the matrix has already been
        /// "triangularized" - meaning there are no elements in the bottom
        /// triangle - A[i,j]=0.0 where j>i
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>The inverted matrix, A^-1.</returns>
        /// <exception cref="System.ArithmeticException">Matrix cannnot be inverted. Can only invert sqare matrices.</exception>
        public static double[,] inverseUpper(double[,] A)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("Matrix cannnot be inverted. Can only invert sqare matrices.");

            var B = new double[length, length];
            var t = new double[length];

            for (var j = 0; j < length; j++)
            {
                B[j, j] = 1 / A[j, j];
                for (var i = 0; i < j; i++)
                    B[i, j] = A[i, j];
            }
            for (var j = 1; j < length; j++)
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
                            v += B[i, jj] * t[jj];
                    }
                    else v = 0;
                    B[i, j] = v + B[i, i] * t[i];
                }
                for (var ii = 0; ii < j; ii++)
                    B[ii, j] = Bjj * B[ii, j];
            }
            return B;
        }

        /// <summary>
        /// Inverses the matrix A only if the matrix has already been
        /// "triangularized" - meaning there are no elements in the bottom
        /// triangle - A[i,j]=0.0 where j&gt;i
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>The inverted matrix, A^-1.</returns>
        /// <exception cref="System.ArithmeticException">Matrix cannnot be inverted. Can only invert sqare matrices.</exception>
        public static double[,] inverseUpper(int[,] A)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("Matrix cannnot be inverted. Can only invert sqare matrices.");
            var B = new double[length, length];
            var t = new double[length];

            for (var j = 0; j < length; j++)
            {
                B[j, j] = 1 / (double)A[j, j];
                for (var i = 0; i < j; i++)
                    B[i, j] = A[i, j];
            }
            for (var j = 1; j < length; j++)
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
                            v += B[i, jj] * t[jj];
                    }
                    else v = 0;
                    B[i, j] = v + B[i, i] * t[i];
                }
                for (var ii = 0; ii < j; ii++)
                    B[ii, j] = Bjj * B[ii, j];
            }
            return B;
        }

        /// <summary>
        /// Inverses the matrix A only if the diagonal is all non-zero.
        /// A[i,i] != 0.0
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>The inverted matrix, A^-1.</returns>
        /// <exception cref="System.ArithmeticException">Matrix cannnot be inverted. Can only invert sqare matrices.</exception>
        public static double[,] inverse(this double[,] A)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("Matrix cannnot be inverted. Can only invert sqare matrices.");
            if (length == 1) return new[,] { { 1 / A[0, 0] } };
            return inverseWithLUResult(LUDecomposition(A, length), length);
        }

        /// <summary>
        /// Inverses the matrix A only if the diagonal is all non-zero.
        /// A[i,i] != 0.0
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>The inverted matrix, A^-1.</returns>
        /// <exception cref="System.ArithmeticException">Matrix cannnot be inverted. Can only invert sqare matrices.</exception>
        public static double[,] inverse(this int[,] A)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("Matrix cannnot be inverted. Can only invert sqare matrices.");
            if (length == 1) return new[,] { { 1 / (double)A[0, 0] } };
            return inverseWithLUResult(LUDecomposition(A, length), length);
        }

        /// <summary>
        /// Inverses the with lu result.
        /// </summary>
        /// <param name="B">The b.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.Double[].</returns>
        private static double[,] inverseWithLUResult(double[,] B, int length)
        {
            // one constraint/caveat in this function is that the diagonal elts. cannot
            // be zero.
            // if the matrix is not square or is less than B 2x2, 
            // then this function won't work

            #region invert L

            for (var i = 0; i < length; i++)
            {
                B[i, i] = 1.0 / B[i, i];
                for (var j = i + 1; j < length; j++)
                {
                    var sum = 0.0;
                    for (var k = i; k < j; k++)
                        sum -= B[j, k] * B[k, i];
                    B[j, i] = (sum == 0) ? 0.0 : sum / B[j, j];
                }
            }

            #endregion

            #region invert U

            for (var i = 0; i < length; i++)
                for (var j = i + 1; j < length; j++)
                {
                    var sum = -B[i, j];
                    for (var k = i + 1; k < j; k++)
                        sum -= B[k, j] * B[i, k];
                    B[i, j] = sum;
                }

            #endregion

            #region final inversion

            for (var i = 0; i < length; i++)
                for (var j = 0; j < length; j++)
                {
                    if (j == i)
                    {
                        var sum = B[i, i];
                        for (var k = i + 1; k < length; k++)
                            sum += B[i, k] * B[k, i];
                        B[i, i] = sum;
                    }
                    else if (j < i)
                    {
                        var sum = 0.0;
                        for (var k = i; k < length; k++)
                            sum += B[j, k] * B[k, i];
                        B[j, i] = sum;
                    }
                    else // then i<j
                    {
                        var sum = B[j, i];
                        for (var k = j + 1; k < length; k++)
                            sum += B[j, k] * B[k, i];
                        B[j, i] = sum;
                    }
                }

            #endregion

            return B;
        }

        #endregion

        #region LU Decomposition

        /// <summary>
        /// Returns the LU decomposition of A in a new matrix.
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <param name="L">The L matrix is output where the diagonal elements are included and not (necessarily) equal to one.</param>
        /// <param name="U">The U matrix is output where the diagonal elements are all equal to one.</param>
        /// <exception cref="ArithmeticException">LU Decomposition can only be determined for square matrices.</exception>
        /// <exception cref="System.ArithmeticException">Matrix cannot be inverted. Can only invert sqyare matrices.</exception>
        public static void LUDecomposition(double[,] A, out double[,] L, out double[,] U)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("LU Decomposition can only be determined for square matrices.");
            L = LUDecomposition(A, length);
            U = new double[length, length];
            for (var i = 0; i < length; i++)
            {
                U[i, i] = 1.0;
                for (var j = i + 1; j < length; j++)
                {
                    U[i, j] = L[i, j];
                    L[i, j] = 0.0;
                }
            }
        }

        /// <summary>
        /// Returns the LU decomposition of A in a new matrix.
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <param name="length">The length/order/number of rows of matrix, A.</param>
        /// <returns>A matrix of equal size to A that combines the L and U. Here the diagonals belongs to L and the U's diagonal
        /// elements are all 1.</returns>
        /// <exception cref="ArithmeticException">LU Decomposition can only be determined for square matrices.</exception>
        /// <exception cref="System.ArithmeticException">LU Decomposition can only be determined for square matrices.</exception>
        private static double[,] LUDecomposition(double[,] A, int length = -1)
        {
            if (length == -1) length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("LU Decomposition can only be determined for square matrices.");
            var B = (double[,])A.Clone();
            // normalize row 0
            for (var i = 1; i < length; i++) B[0, i] /= B[0, 0];

            for (var i = 1; i < length; i++)
            {
                for (var j = i; j < length; j++)
                {
                    // do a column of L
                    for (var k = 0; k < i; k++)
                        B[j, i] -= B[j, k] * B[k, i];
                }
                if (i == length - 1) continue;
                for (var j = i + 1; j < length; j++)
                {
                    // do a row of U
                    var sum = B[i, j];
                    for (var k = 0; k < i; k++)
                        sum -= B[i, k] * B[k, j];
                    B[i, j] = (sum == 0.0) ? 0.0 : sum / B[i, i];
                }
            }
            return B;
        }

        /// <summary>
        /// Returns the LU decomposition of A in a new matrix.
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <param name="L">The L matrix is output where the diagonal elements are included and not (necessarily) equal to one.</param>
        /// <param name="U">The U matrix is output where the diagonal elements are all equal to one.</param>
        /// <exception cref="ArithmeticException">LU Decomposition can only be determined for square matrices.</exception>
        /// <exception cref="System.ArithmeticException">LU Decomposition can only be determined for square matrices.</exception>
        public static void LUDecomposition(int[,] A, out double[,] L, out double[,] U)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("LU Decomposition can only be determined for square matrices.");
            L = LUDecomposition(A, length);
            U = new double[length, length];
            for (var i = 0; i < length; i++)
            {
                U[i, i] = 1.0;
                for (var j = i + 1; j < length; j++)
                {
                    U[i, j] = L[i, j];
                    L[i, j] = 0.0;
                }
            }
        }

        /// <summary>
        /// Returns the LU decomposition of A in a new matrix.
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <param name="length">The length.</param>
        /// <returns>A matrix of equal size to A that combines the L and U. Here the diagonals belongs to L and the U's diagonal
        /// elements are all 1.</returns>
        /// <exception cref="ArithmeticException">LU Decomposition can only be determined for square matrices.</exception>
        private static double[,] LUDecomposition(int[,] A, int length = -1)
        {
            if (length == -1) length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("LU Decomposition can only be determined for square matrices.");
            var B = new double[length, length];
            B[0, 0] = A[0, 0];
            // normalize row 0
            for (var i = 1; i < length; i++) B[0, i] = A[0, i] / B[0, 0];

            for (var i = 1; i < length; i++)
                for (var j = 0; j < length; j++)
                    B[i, j] = A[i, j];

            for (var i = 1; i < length; i++)
            {
                for (var j = i; j < length; j++)
                {
                    // do a column of L
                    for (var k = 0; k < i; k++)
                        B[j, i] -= B[j, k] * B[k, i];
                }
                if (i == length - 1) continue;
                for (var j = i + 1; j < length; j++)
                {
                    // do a row of U
                    var sum = B[i, j];
                    for (var k = 0; k < i; k++)
                        sum -= B[i, k] * B[k, j];
                    B[i, j] = sum / B[i, i];
                }
            }
            return B;
        }

        #endregion

        #region Cholesky Decomposition
        // this is intended only for symmetric positive definite matrices
        /// <summary>
        /// Returns the Cholesky decomposition of A in a new matrix.
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="ArithmeticException">
        /// Cholesky Decomposition can only be determined for square matrices.
        /// or
        /// Matrix is not positive definite. Cannot complete Cholesky decomposition.
        /// </exception>
        /// <exception cref="System.ArithmeticException">Matrix cannot be inverted. Can only invert square matrices.</exception>
        public static double[,] CholeskyDecomposition(double[,] A)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("Cholesky Decomposition can only be determined for square matrices.");
            var L = (double[,])A.Clone();

            for (var i = 0; i < length; i++)
            {
                double sum;
                for (var j = 0; j < i; j++)
                {
                    sum = 0.0;
                    for (int k = 0; k < j; k++)
                        sum += L[i, k] * L[j, k];
                    L[i, j] = (L[i, j] - sum) / L[j, j];
                }
                sum = 0.0;
                for (int k = 0; k < i; k++)
                    sum += L[i, k] * L[i, k];
                sum = L[i, i] - sum;
                if (sum < 0) throw new ArithmeticException("Matrix is not positive definite. Cannot complete Cholesky decomposition.");
                L[i, i] = Math.Sqrt(sum);
                for (int j = i + 1; j < length; j++)
                    L[i, j] = 0.0;
            }
            return L;
        }
        // this is intended only for symmetric positive definite matrices
        /// <summary>
        /// Returns the Cholesky decomposition of A in a new matrix.
        /// </summary>
        /// <param name="A">The matrix to invert. This matrix is unchanged by this function.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="ArithmeticException">Cholesky Decomposition can only be determined for square matrices.
        /// or
        /// Matrix is not positive definite. Cannot complete Cholesky decomposition.</exception>
        /// <exception cref="System.ArithmeticException">Cholesky Decomposition can only be determined for square matrices.
        /// or
        /// Matrix is not positive definite. Cannot complete Cholesky decomposition.</exception>
        public static double[,] CholeskyDecomposition(int[,] A)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("Cholesky Decomposition can only be determined for square matrices.");
            var L = new double[length, length];

            for (var i = 0; i < length; i++)
            {
                double sum;
                for (var j = 0; j < i; j++)
                {
                    sum = 0.0;
                    for (int k = 0; k < j; k++)
                        sum += L[i, k] * L[j, k];
                    L[i, j] = (A[i, j] - sum) / L[j, j];
                }
                sum = 0.0;
                for (int k = 0; k < i; k++)
                    sum += L[i, k] * L[i, k];
                sum = A[i, i] - sum;
                if (sum < 0) throw new ArithmeticException("Matrix is not positive definite. Cannot complete Cholesky decomposition.");
                L[i, i] = Math.Sqrt(sum);
            }
            return L;
        }

        #endregion

        #region Transpose

        /// <summary>
        /// Transposes the matrix, A.
        /// </summary>
        /// <param name="A">The matrix to transpose. This matrix is unchanged by this function.</param>
        /// <returns>The transpose of A.</returns>
        /// <exception cref="ArithmeticException">The matrix, A, is null.</exception>
        /// <exception cref="System.ArithmeticException">The matrix, A, is null.</exception>
        public static double[,] transpose(this double[,] A)
        {
            if (A == null) throw new ArithmeticException("The matrix, A, is null.");
            var numRows = A.GetLength(1);
            var numCols = A.GetLength(0);
            var C = new double[numRows, numCols];

            for (var i = 0; i != numRows; i++)
                for (var j = 0; j != numCols; j++)
                    C[i, j] = A[j, i];
            return C;
        }

        /// <summary>
        /// Transposes the matrix, A.
        /// </summary>
        /// <param name="A">The matrix to transpose. This matrix is unchanged by this function.</param>
        /// <returns>The transpose of A.</returns>
        /// <exception cref="ArithmeticException">The matrix, A, is null.</exception>
        /// <exception cref="System.ArithmeticException">The matrix, A, is null.</exception>
        public static int[,] transpose(this int[,] A)
        {
            if (A == null) throw new ArithmeticException("The matrix, A, is null.");
            var numRows = A.GetLength(1);
            var numCols = A.GetLength(0);

            var C = new int[numRows, numCols];

            for (var i = 0; i != numRows; i++)
                for (var j = 0; j != numCols; j++)
                    C[i, j] = A[j, i];
            return C;
        }

        #endregion

        #region Determinant

        /// <summary>
        /// Returns the determinant of matrix, A.
        /// </summary>
        /// <param name="A">The input argument matrix. This matrix is unchanged by this function.</param>
        /// <returns>a single value representing the matrix's determinant.</returns>
        /// <exception cref="System.ArithmeticException">The matrix, A, is null.
        /// or
        /// The determinant is only possible for square matrices.</exception>
        /// <exception cref="ArithmeticException">The matrix, A, is null.
        /// or
        /// The determinant is only possible for square matrices.</exception>
        public static double determinant(this double[,] A)
        {
            if (A == null) throw new ArithmeticException("The matrix, A, is null.");
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("The determinant is only possible for square matrices.");
            if (length == 0) return 0.0;
            if (length == 1) return A[0, 0];
            if (length == 2) return (A[0, 0] * A[1, 1]) - (A[0, 1] * A[1, 0]);
            if (length == 3)
                return (A[0, 0] * A[1, 1] * A[2, 2])
                       + (A[0, 1] * A[1, 2] * A[2, 0])
                       + (A[0, 2] * A[1, 0] * A[2, 1])
                       - (A[0, 0] * A[1, 2] * A[2, 1])
                       - (A[0, 1] * A[1, 0] * A[2, 2])
                       - (A[0, 2] * A[1, 1] * A[2, 0]);
            return determinantBig(A, length);
        }

        /// <summary>
        /// Determinants the big.
        /// </summary>
        /// <param name="A">a.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.Double.</returns>
        private static double determinantBig(double[,] A, int length)
        {
            var L = LUDecomposition(A, length);
            var result = 1.0;
            for (var i = 0; i < length; i++)
                if (double.IsNaN(L[i, i]))
                    return 0;
                else result *= L[i, i];
            return result;
        }

        /// <summary>
        /// Returns the determinant of matrix, A.
        /// </summary>
        /// <param name="A">The input argument matrix. This matrix is unchanged by this function.</param>
        /// <returns>a single value representing the matrix's determinant.</returns>
        /// <exception cref="System.ArithmeticException">The matrix, A, is null.
        /// or
        /// The determinant is only possible for square matrices.</exception>
        /// <exception cref="ArithmeticException">The matrix, A, is null.
        /// or
        /// The determinant is only possible for square matrices.</exception>
        public static int determinant(this int[,] A)
        {
            if (A == null) throw new ArithmeticException("The matrix, A, is null.");
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new ArithmeticException("The determinant is only possible for square matrices.");
            if (length == 0) return 0;
            if (length == 1) return A[0, 0];
            if (length == 2) return (A[0, 0] * A[1, 1]) - (A[0, 1] * A[1, 0]);
            if (length == 3)
                return (A[0, 0] * A[1, 1] * A[2, 2])
                       + (A[0, 1] * A[1, 2] * A[2, 0])
                       + (A[0, 2] * A[1, 0] * A[2, 1])
                       - (A[0, 0] * A[1, 2] * A[2, 1])
                       - (A[0, 1] * A[1, 0] * A[2, 2])
                       - (A[0, 2] * A[1, 1] * A[2, 0]);
            return determinantBig(A, length);
        }

        /// <summary>
        /// Returns the determinant of matrix, A. Only used internally for matrices larger than 3.
        /// </summary>
        /// <param name="A">The input argument matrix. This matrix is unchanged by this function.</param>
        /// <param name="length">The length of the side of the square matrix.</param>
        /// <returns>a single value representing the matrix's determinant.</returns>
        private static int determinantBig(int[,] A, int length)
        {
            var L = LUDecomposition(A, length);
            var result = 1.0;
            for (var i = 0; i < length; i++)
                if (double.IsNaN(L[i, i]))
                    return 0;
                else result *= L[i, i];
            return (int)result;
        }

        #endregion
    }
}