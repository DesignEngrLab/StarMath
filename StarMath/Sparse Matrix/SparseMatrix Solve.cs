// ***********************************************************************
// Assembly         : StarMath
// Author           : Matthew I. Campbell
// Created          : 09-03-2015
//
// Last Modified By : Matt
// Last Modified On : 09-03-2015
// ***********************************************************************
// <copyright file="SparseMatrix.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace StarMathLib
{
    /// <summary>
    /// Class SparseMatrix.
    /// </summary>
    public partial class SparseMatrix
    {
        /// <summary>
        /// Solves the system of equations where this Sparse Matrix is 'A' in Ax = b.
        /// The resulting x is returned.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="initialGuess">The initial guess.</param>
        /// <param name="IsASymmetric">The is a symmetric.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">
        /// Spare Matrix must be square to solve Ax = b.
        /// or
        /// Sparse Matrix must be have the same number of rows as the vector, b.
        /// </exception>
        /// <exception cref="ArithmeticException">Spare Matrix must be square to solve Ax = b.
        /// or
        /// Sparse Matrix must be have the same number of rows as the vector, b.</exception>
        public double[] solve(IList<double> b, IList<double> initialGuess = null,
            Boolean IsASymmetric = false)
        {
            if (NumRows != NumCols)
                throw new ArithmeticException("Spare Matrix must be square to solve Ax = b.");
            if (NumRows != b.Count)
                throw new ArithmeticException("Sparse Matrix must be have the same number of rows as the vector, b.");
            List<int>[] potentialDiagonals;
            if (isGaussSeidelAppropriate(b, out potentialDiagonals, ref initialGuess))
                return solveIteratively(b, initialGuess, potentialDiagonals);
            /****** need code to determine when to switch between *****
             ****** this analytical approach and the SOR approach *****/
            return SolveAnalytically(b, IsASymmetric, potentialDiagonals);
        }

        /// <summary>
        /// Solves the system of equations analytically.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="IsASymmetric">if set to <c>true</c> [a is symmetric].</param>
        /// <param name="potentialDiagonals">The potential diagonals.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public double[] SolveAnalytically(IList<double> b, bool IsASymmetric = false,
            List<int>[] potentialDiagonals = null)
        {
            if (IsASymmetric)
            {
                double[] L, D;
                CholeskyDecomposition(out L, out D);
                return solveFromCholeskyFactorization(b, L, D, NumCols);
            }
            else
            {
                throw new NotImplementedException();
                //double[,] C;
                //double[] d;
                //if (needToReorder(A, length, 0.0))
                //{
                //    if (potentialDiagonals == null &&
                //        !findPotentialDiagonals(A, out potentialDiagonals, length, 0.0))
                //        return null;
                //    var order = reorderMatrixForDiagonalDominance(A, length, potentialDiagonals);
                //    if (order == null) return null;
                //    C = new double[length, length];
                //    d = new double[length];
                //    for (var i = 0; i < length; i++)
                //    {
                //        d[i] = b[order[i]];
                //        SetRow(i, C, GetRow(order[i], A));
                //    }
                //}
                //else
                //{
                //    C = (double[,]) A.Clone();
                //    d = b.ToArray();
                //}
                //var LU = LUDecomposition(C, length);
                //var x = new double[length];
                //// forward substitution
                //for (int i = 0; i < length; i++)
                //{
                //    var sumFromKnownTerms = 0.0;
                //    for (int j = 0; j < i; j++)
                //        sumFromKnownTerms += LU[i, j]*x[j];
                //    x[i] = (d[i] - sumFromKnownTerms)/LU[i, i];
                //}
                //// backward substitution
                //for (int i = length - 1; i >= 0; i--)
                //{
                //    var sumFromKnownTerms = 0.0;
                //    for (int j = i + 1; j < length; j++)
                //        sumFromKnownTerms += LU[i, j]*x[j];
                //    x[i] -= sumFromKnownTerms;
                //}
                //return x;
            }
        }

        private double[] solveFromCholeskyFactorization(IList<double> b, double[] L, double[] D, int length)
        {
            var x = new double[length];
            var ithRowStartIndex = 0;
            // forward substitution
            for (int i = 0; i < length; i++)
            {
                var sumFromKnownTerms = 0.0;
                for (int j = 0; j < i; j++)
                    sumFromKnownTerms += L[ithRowStartIndex + j] * x[j];
                x[i] = (b[i] - sumFromKnownTerms);
                ithRowStartIndex += i;
            }
            for (int i = 0; i < length; i++)
                x[i] /= D[i];

            ithRowStartIndex = (length*(length + 1)/2) - 1;
            // backward substitution
            for (int i = length - 1; i >= 0; i--)
            {
                var sumFromKnownTerms = 0.0;
                var jthColStartIndex = 0;
                for (int j = i + 1; j < length; j++)
                {
                    sumFromKnownTerms += L[ithRowStartIndex + jthColStartIndex] * x[j];
                    jthColStartIndex += j;
                }
                x[i] -= sumFromKnownTerms;
                ithRowStartIndex -= i+1;
            }
            return x;
        }
 
        /// <summary>
        /// Overwrites the matrix with its Cholesky decomposition (i.e. it is destructive).
        /// This is based on: https://en.wikipedia.org/wiki/Cholesky_decomposition#LDL_decomposition_2
        /// </summary>
        /// <param name="L">The Lower matrix as a 1D array.</param>
        /// <param name="D">The Diagonals as a 1D array.</param>
        /// <returns>SparseMatrix.</returns>
        /// <exception cref="System.ArithmeticException">Cholesky Decomposition can only be determined for square matrices.</exception>
        /// <exception cref="ArithmeticException">Cholesky Decomposition can only be determined for square matrices.</exception>
        private void CholeskyDecomposition(out double[] L, out double[] D)
        {
            if (NumCols != NumRows)
                throw new ArithmeticException("Cholesky Decomposition can only be determined for square matrices.");
            L = new double[NumRows * (NumRows - 1) / 2];
            D = new double[NumRows];
            var ithRowStartIndex = 0;
            for (var i = 0; i < NumRows; i++)
            {
                var jthRowStartIndex = 0;
                var sum = 0.0;
                var nextNonZeroCell = RowFirsts[i];
                for (var j = 0; j < i; j++)
                {
                    sum = 0.0;
                    for (int k = 0; k < j; k++)
                    {
                        sum += L[ithRowStartIndex + k] * L[jthRowStartIndex + k] * D[k];
                    }
                    if (j < nextNonZeroCell.ColIndex)
                        L[ithRowStartIndex + j] = -sum / D[j];
                    else
                    {
                        L[ithRowStartIndex + j] = (nextNonZeroCell.Value - sum) / D[j];
                        nextNonZeroCell = nextNonZeroCell.Right;
                    }
                    jthRowStartIndex += j;
                }
                sum = 0.0;
                for (int k = 0; k < i; k++)
                    sum += L[ithRowStartIndex + k] * L[ithRowStartIndex + k] * D[k];
                D[i] = Diagonals[i].Value - sum;
                ithRowStartIndex += i;
            }
        }

        private bool findPotentialDiagonals(out List<int>[] potentialDiagonals, int length, double minimalConsideration)
        {
            throw new NotImplementedException();
        }

        private bool isGaussSeidelAppropriate(IList<double> b, out List<int>[] potentialDiagonals,
            ref IList<double> initialGuess)
        {
            potentialDiagonals = null;
            return false;
            throw new NotImplementedException();
            //potentialDiagonals = null;
            //if (length < StarMath.GaussSeidelMinimumMatrixSize) return false;
            //if (initialGuess == null)
            //    initialGuess = makeInitialGuess(A, b, length);
            //var error = StarMath.norm1(StarMath.subtract(b, StarMath.multiply(A, initialGuess, length, length), length)) / StarMath.norm1(b);
            //if (error > StarMath.MaxErrorForUsingGaussSeidel) return false;
            //return findPotentialDiagonals(out potentialDiagonals, length, StarMath.GaussSeidelDiagonalDominanceRatio);
        }

        /// <summary>
        /// Solves the system of equations iteratively.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="initialGuess">The initial guess.</param>
        /// <param name="potentialDiagonals">The potential diagonals.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public double[] solveIteratively(IList<double> b, IList<double> initialGuess = null,
            List<int>[] potentialDiagonals = null)
        {
            throw new NotImplementedException();
        }
    }
}