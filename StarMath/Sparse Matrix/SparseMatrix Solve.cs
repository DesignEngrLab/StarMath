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
        public double[] SolveAnalyticallyCSRApproach(IList<double> b, bool IsASymmetric = false,
            List<int>[] potentialDiagonals = null)
        {
            if (IsASymmetric)
            {
                double[] D;
                List<double>[] L;
                List<int>[] LIndices;
                CholeskyDecompositionCSRApproach(out L, out LIndices, out D);
                return solveFromCholeskyFactorizationCSRApproach(b, L, LIndices, D, NumCols);
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

        private double[] solveFromCholeskyFactorizationCSRApproach(IList<double> b, List<double>[] L, List<int>[] LIndices, double[] D, int length)
        {
            var x = new double[length];
            // forward substitution
            for (int i = 0; i < length; i++)
            {
                var sumFromKnownTerms = 0.0;
                var ithPosition = 0;
                var ithRowColIndex = GetLIndex(LIndices, i, ithPosition);
                while (ithRowColIndex < i)
                {
                    sumFromKnownTerms += L[i][ithPosition] * x[ithRowColIndex];
                    ithRowColIndex = GetLIndex(LIndices, i, ++ithPosition);
                }
                x[i] = (b[i] - sumFromKnownTerms);
            }
            for (int i = 0; i < length; i++)
                x[i] /= D[i];

            // backward substitution
            for (int j = length - 1; j >= 0; j--)
            {
                var sumFromKnownTerms = 0.0;
                var ithRow = length - 1;
                while (ithRow > j)
                {
                    var jthPosition = LIndices[ithRow].IndexOf(j);
                    if (jthPosition >= 0)
                        sumFromKnownTerms += L[ithRow][jthPosition] * x[ithRow];
                    ithRow--;
                }
                x[j] -= sumFromKnownTerms;
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
        private void CholeskyDecompositionCSRApproach(out List<double>[] L, out List<int>[] LIndices, out double[] D)
        {
            if (NumCols != NumRows)
                throw new ArithmeticException("Cholesky Decomposition can only be determined for square matrices.");
            L = new List<double>[NumRows];
            LIndices = new List<int>[NumRows];
            D = new double[NumRows];
            for (var i = 0; i < NumRows; i++)
            {
                L[i] = new List<double>();
                LIndices[i] = new List<int>();
                double sum;
                var nextNonZeroCell = RowFirsts[i];
                int ithPosition, ithRowColIndex;
                for (var j = 0; j < i; j++)
                {
                    sum = 0.0;
                    ithPosition = 0;
                    var jthPosition = 0;
                    ithRowColIndex = GetLIndex(LIndices, i, ithPosition);
                    var jthRowColIndex = GetLIndex(LIndices, j, jthPosition);
                    while (ithRowColIndex < j && jthRowColIndex < j)
                    {
                        if (ithRowColIndex == jthRowColIndex)
                        {
                            sum += L[i][ithPosition] * L[j][jthPosition]
                                   * D[ithRowColIndex];
                            ithRowColIndex = GetLIndex(LIndices, i, ++ithPosition);
                            jthRowColIndex = GetLIndex(LIndices, j, ++jthPosition);
                        }
                        else if (ithRowColIndex < jthRowColIndex)
                            ithRowColIndex = GetLIndex(LIndices, i, ++ithPosition);
                        else jthRowColIndex = GetLIndex(LIndices, j, ++jthPosition);
                    }
                    if (j < nextNonZeroCell.ColIndex && sum != 0.0)
                    {
                        L[i].Add(-sum / D[j]);
                        LIndices[i].Add(j);
                    }
                    else if (j == nextNonZeroCell.ColIndex && !nextNonZeroCell.Value.IsPracticallySame(sum))
                    {
                        L[i].Add((nextNonZeroCell.Value - sum) / D[j]);
                        LIndices[i].Add(j);
                        nextNonZeroCell = nextNonZeroCell.Right;
                    }
                }
                sum = 0.0;
                ithPosition = 0;
                ithRowColIndex = GetLIndex(LIndices, i, ithPosition);
                while (ithRowColIndex < i)
                {
                    sum += L[i][ithPosition] * L[i][ithPosition] * D[ithRowColIndex];
                    ithRowColIndex = GetLIndex(LIndices, i, ++ithPosition);
                }
                D[i] = Diagonals[i].Value - sum;
            }
        }

        private int GetLIndex(List<int>[] LIndices, int i, int ithPosition)
        {
            if (ithPosition >= LIndices[i].Count) return int.MaxValue;
            else return LIndices[i][ithPosition];
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
                x[i] /= Diagonals[i].Value;

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
            var furthestDownCells = new SparseCell[NumCols];
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
                        furthestDownCells[cellRowI.ColIndex] = cellRowI;
                        furthestDownCells[cellRowJ.ColIndex] = cellRowJ;
                        if (cellRowI.ColIndex == cellRowJ.ColIndex)
                        {
                            sum += cellRowI.Value * cellRowJ.Value
                                * Diagonals[cellRowI.ColIndex].Value;
                            cellRowI = cellRowI.Right;
                            cellRowJ = cellRowJ.Right;
                        }
                        else if (cellRowI.ColIndex < cellRowJ.ColIndex)
                            cellRowI = cellRowI.Right;
                        else cellRowJ = cellRowJ.Right;
                    }
                    var alreadyExists = TrySearchRightToCell(j, ref cellRowI);
                    if (cellRowI != null) furthestDownCells[cellRowI.ColIndex] = cellRowI;
                    // what's up with this furthestDownCells?! It turns out the the AddCell function
                    // was too slow when it was done on the basis of just the [i,j] indices. This "fat"
                    // approach to encoding sparse matrices is bad for that. To avoid, this - the
                    // special function "AddCellToTheLeftOfAndBelow" was created. It has a significant
                    // improvemnt on time.
                    if (!alreadyExists && sum.IsNegligible()) continue;
                    //if (alreadyExists && cellRowI.Value.IsPracticallySame(sum))
                    //{
                    //    RemoveCell(cellRowI);
                    //    continue;
                    //}
                    if (!alreadyExists && !sum.IsNegligible()) cellRowI = AddCellToTheLeftOfAndBelow(cellRowI, furthestDownCells[j], i, j, 0.0);
                    cellRowI.Value = (cellRowI.Value - sum) / Diagonals[j].Value;
                }

                cellRowI = startCellRowI;
                sum = 0.0;
                while (cellRowI.ColIndex < i)
                {
                    sum += cellRowI.Value * cellRowI.Value * Diagonals[cellRowI.ColIndex].Value;
                    cellRowI = cellRowI.Right;
                    furthestDownCells[cellRowI.ColIndex] = cellRowI;
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

        private SparseCell AddCellToTheLeftOfAndBelow(SparseCell cellToTheRight, SparseCell cellToTheUp, int rowI, int colI, double value)
        {
            var cell = new SparseCell(rowI, colI, value);
            // stitch it into the rows
            if (RowFirsts[rowI].ColIndex > colI)
            {
                cell.Right = RowFirsts[rowI];
                RowFirsts[rowI].Left = cell;
                RowFirsts[rowI] = cell;
            }
            else if (RowLasts[rowI].ColIndex < colI)
            {
                cell.Left = RowLasts[rowI];
                RowLasts[rowI].Right = cell;
                RowLasts[rowI] = cell;
            }
            else
            {
                cell.Right = cellToTheRight;
                cell.Left = cellToTheRight.Left;
                cell.Left.Right = cell;
                cellToTheRight.Left = cell;
            }
            // stitch it into the colums
            if (ColFirsts[colI].RowIndex > rowI)
            {
                cell.Down = ColFirsts[colI];
                ColFirsts[colI].Up = cell;
                ColFirsts[colI] = cell;
            }
            else if (ColLasts[colI].RowIndex < rowI)
            {
                cell.Up = ColLasts[colI];
                ColLasts[colI].Down = cell;
                ColLasts[colI] = cell;
            }
            else
            {
                //if (cellToTheUp==null)
                //var startCell = ColFirsts[colI];
                //while (startCell.RowIndex < rowI)
                //    startCell = startCell.Down;
                //cell.Down = startCell;
                //cell.Up = startCell.Up;
                //cell.Up.Down = cell;
                //startCell.Up = cell;
                cell.Up = cellToTheUp;
                cell.Down = cellToTheUp.Down;
                cell.Down.Up = cell;
                cellToTheUp.Down = cell;

            }
            if (rowI == colI) Diagonals[rowI] = cell;
            cellsRowbyRow.Add(cell);
            NumNonZero++;

            return cell;
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