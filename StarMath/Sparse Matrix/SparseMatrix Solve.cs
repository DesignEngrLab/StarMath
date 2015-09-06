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
        /// <param name="AIsSymmetricPositiveDefinite">if set to <c>true</c> [a is symmetric].</param>
        /// <param name="potentialDiagonals">The potential diagonals.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public double[] SolveAnalytically(IList<double> b, bool AIsSymmetricPositiveDefinite = false,
            List<int>[] potentialDiagonals = null)
        {
            var length = b.Count;
            if (AIsSymmetricPositiveDefinite)
            {
                var L = this.Copy();
                L.CholeskyDecomposition();
                return L.solveFromCholeskyFactorization(b, NumCols);

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

        private double[] solveFromCholeskyFactorization(IList<double> b, int length)
        {
            var x = new double[length];
            // forward substitution
            for (int i = 0; i < length; i++)
            {
                var sumFromKnownTerms = 0.0;
                var startCell = RowFirsts[i];
                while (startCell != null && startCell.ColIndex < i)
                {
                    sumFromKnownTerms += startCell.Value * x[startCell.ColIndex];
                    startCell = startCell.Right;
                }
                x[i] = (b[i] - sumFromKnownTerms);
            }
            for (int i = 0; i < length; i++)
                x[i] /= this[i, i];

            // backward substitution
            for (int i = length - 1; i >= 0; i--)
            {
                var sumFromKnownTerms = 0.0;
                var startCell = ColLasts[i];  // this is because it is the transposed one
                while (startCell != null && startCell.RowIndex > i)
                {
                    sumFromKnownTerms += startCell.Value * x[startCell.RowIndex];
                    startCell = startCell.Up;
                }
                x[i] -= sumFromKnownTerms;
            }
            return x;
        }

        /// <summary>
        /// Overwrites the matrix with its Cholesky decomposition (i.e. it is destructive).
        /// </summary>
        /// <returns>SparseMatrix.</returns>
        /// <exception cref="System.ArithmeticException">Cholesky Decomposition can only be determined for square matrices.</exception>
        /// <exception cref="ArithmeticException">Cholesky Decomposition can only be determined for square matrices.</exception>
        public void CholeskyDecomposition()
        {
            if (NumCols != NumRows)
                throw new ArithmeticException("Cholesky Decomposition can only be determined for square matrices.");

            for (var i = 0; i < NumRows; i++)
            {
                double sum;
                var startCellRowI = RowFirsts[i];
                SparseCell cellRowI;
                for (var j = 0; j < i; j++)
                {
                    cellRowI = startCellRowI;
                    var cellRowJ = RowFirsts[j];
                    sum = 0.0;
                    while (cellRowI.ColIndex < j && cellRowJ.ColIndex < j)
                    {
                        if (cellRowI.ColIndex == cellRowJ.ColIndex)
                        {
                            sum += cellRowI.Value * cellRowJ.Value
                                * this[cellRowI.ColIndex, cellRowI.ColIndex];
                            cellRowI = cellRowI.Right;
                            cellRowJ = cellRowJ.Right;
                        }
                        else if (cellRowI.ColIndex < cellRowJ.ColIndex)
                            cellRowI = cellRowI.Right;
                        else cellRowJ = cellRowJ.Right;
                    }
                    cellRowI = SearchRightToCell(j, cellRowI);
                    if (cellRowI == null && sum.IsNegligible()) continue;
                    if (cellRowI != null && cellRowI.Value.IsPracticallySame(sum))
                    {
                        RemoveCell(cellRowI);
                        continue;
                    }
                    if (cellRowI == null && !sum.IsNegligible()) cellRowI = AddCell(i, j, 0.0);
                    cellRowI.Value = (cellRowI.Value - sum) / this[j, j];
                }

                cellRowI = startCellRowI;
                sum = 0.0;
                while (cellRowI.ColIndex < i)
                {
                    sum += cellRowI.Value * cellRowI.Value * this[cellRowI.ColIndex, cellRowI.ColIndex];
                    cellRowI = cellRowI.Right;
                }
                cellRowI.Value -= sum;
                // delete the rest of the entries on the row
                RowLasts[i] = cellRowI;
                while (cellRowI.Right != null)
                {
                    cellRowI = cellRowI.Right;
                    cellRowI.Left.Right = null;
                    ColFirsts[cellRowI.ColIndex] = cellRowI.Down;
                    cellRowI.Down.Up = null;
                }
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