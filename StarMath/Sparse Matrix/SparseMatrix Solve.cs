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
        /// <exception cref="System.ArithmeticException">Spare Matrix must be square to solve Ax = b.
        /// or
        /// Sparse Matrix must be have the same number of rows as the vector, b.</exception>
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
                var L = Copy();
                L.CholeskyDecomposition();
                return L.solveFromCholeskyFactorization(b, NumCols);
            }
            else
            {
                var LU = Copy();
                LU.LUDecomposition();
                return LU.solveFromLUDecomposition(b, NumCols);

            }
        }

        private double[] solveFromLUDecomposition(IList<double> b, int length)
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
                x[i] = (b[i] - sumFromKnownTerms) / Diagonals[i].Value;
            }

            // backward substitution
            for (int i = length - 1; i >= 0; i--)
            {
                var sumFromKnownTerms = 0.0;
                var startCell = RowLasts[i];  // this is because it is the transposed one
                while (startCell != null && startCell.ColIndex > i)
                {
                    sumFromKnownTerms += startCell.Value * x[startCell.ColIndex];
                    startCell = startCell.Left;
                }
                x[i] -= sumFromKnownTerms;
            }
            return x;
        }

        private void LUDecomposition()
        {
            if (NumCols != NumRows)
                throw new ArithmeticException("LU Decomposition can only be determined for square matrices.");

            // normalize row 0 
            var topCell = RowFirsts[0];
            var cell = topCell.Right;
            while (cell != null)
            {
                cell.Value /= topCell.Value;
                cell = cell.Right;
            }
            for (var i = 1; i < NumRows; i++)
            {
                double sum;
                var startCellColI = ColFirsts[i];
                for (var j = i; j < NumRows; j++)
                {
                    var cellColI = startCellColI;
                    var cellRowJ = RowFirsts[j];
                    sum = 0.0;
                    while (cellColI != null && cellRowJ != null && cellColI.RowIndex < i && cellRowJ.ColIndex < i)
                    {
                        if (cellColI.RowIndex == cellRowJ.ColIndex)
                        {
                            sum += cellColI.Value * cellRowJ.Value;
                            cellColI = cellColI.Down;
                            cellRowJ = cellRowJ.Right;
                        }
                        else if (cellColI.RowIndex < cellRowJ.ColIndex)
                            cellColI = cellColI.Down;
                        else cellRowJ = cellRowJ.Right;
                    }
                    while (cellColI != null && cellColI.RowIndex <= j)
                        cellColI = cellColI.Down;
                    var alreadyExists = TrySearchRightToCell(i, ref cellRowJ);
                    if (!alreadyExists && sum.IsNegligible()) continue;
                    // what's up with this furthestDownCells?! It turns out the the AddCell function
                    // was too slow when it was done on the basis of just the [i,j] indices. This "fat"
                    // approach to encoding sparse matrices is bad for that. To avoid, this - the
                    // special function "AddCellToTheLeftOfAndBelow" was created. It has a significant
                    // improvemnt on time.
                    if (!alreadyExists && !sum.IsNegligible())
                        cellRowJ = AddCellToTheLeftOfAndAbove(cellRowJ, cellColI, j, i, 0.0);
                    cellRowJ.Value -= sum;
                }
                var startCellRowI = RowFirsts[i];
                for (var j = i + 1; j < NumRows; j++)
                {
                    var cellRowI = startCellRowI;
                    var cellColJ = ColFirsts[j];
                    sum = 0.0;
                    while (cellRowI != null && cellColJ != null && cellRowI.ColIndex < i && cellColJ.RowIndex < i)
                    {
                        if (cellRowI.ColIndex == cellColJ.RowIndex)
                        {
                            sum -= cellRowI.Value * cellColJ.Value;
                            cellRowI = cellRowI.Right;
                            cellColJ = cellColJ.Down;
                        }
                        else if (cellRowI.ColIndex < cellColJ.RowIndex)
                            cellRowI = cellRowI.Right;
                        else cellColJ = cellColJ.Down;
                    }
                    while (cellColJ != null && cellColJ.RowIndex <= i)
                        cellColJ = cellColJ.Down;
                    var alreadyExists = TrySearchRightToCell(j, ref cellRowI);
                    if (!alreadyExists && sum.IsNegligible()) continue;
                    // what's up with this furthestDownCells?! It turns out the the AddCell function
                    // was too slow when it was done on the basis of just the [i,j] indices. This "fat"
                    // approach to encoding sparse matrices is bad for that. To avoid, this - the
                    // special function "AddCellToTheLeftOfAndBelow" was created. It has a significant
                    // improvemnt on time.
                    if (!alreadyExists && !sum.IsNegligible())
                        cellRowI = AddCellToTheLeftOfAndAbove(cellRowI, cellColJ, i, j, 0.0);
                    cellRowI.Value = (sum + cellRowI.Value) / Diagonals[i].Value;
                }
            }
        }

        /// <summary>
        /// Overwrites the matrix with its Cholesky decomposition (i.e. it is destructive).
        /// This is based on: https://en.wikipedia.org/wiki/Cholesky_decomposition#LDL_decomposition_2
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
                    if (!alreadyExists && sum.IsNegligible()) continue;
                    // what's up with this furthestDownCells?! It turns out the the AddCell function
                    // was too slow when it was done on the basis of just the [i,j] indices. This "fat"
                    // approach to encoding sparse matrices is bad for that. To avoid, this - the
                    // special function "AddCellToTheLeftOfAndBelow" was created. It has a significant
                    // improvemnt on time.
                    if (!alreadyExists && !sum.IsNegligible())
                        cellRowI = AddCellToTheLeftOfAndBelow(cellRowI, furthestDownCells[j], i, j, 0.0);
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

        /// <summary>
        /// Solves from cholesky factorization.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="length">The length.</param>
        /// <returns>System.Double[].</returns>
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
        /// Adds the cell to the left of and below.
        /// </summary>
        /// <param name="cellToTheRight">The cell to the right.</param>
        /// <param name="cellToTheUp">The cell to the up.</param>
        /// <param name="rowI">The row i.</param>
        /// <param name="colI">The col i.</param>
        /// <param name="value">The value.</param>
        /// <returns>SparseCell.</returns>
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

        /// <summary>
        /// Adds the cell to the left of and below.
        /// </summary>
        /// <param name="cellToTheRight">The cell to the right.</param>
        /// <param name="cellToTheUp">The cell to the up.</param>
        /// <param name="rowI">The row i.</param>
        /// <param name="colI">The col i.</param>
        /// <param name="value">The value.</param>
        /// <returns>SparseCell.</returns>
        private SparseCell AddCellToTheLeftOfAndAbove(SparseCell cellToTheRight, SparseCell cellToTheDown, int rowI, int colI, double value)
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
                cell.Down = cellToTheDown;
                cell.Up = cellToTheDown.Up;
                cell.Up.Down = cell;
                cellToTheDown.Up = cell;

            }
            if (rowI == colI) Diagonals[rowI] = cell;
            cellsRowbyRow.Add(cell);
            NumNonZero++;

            return cell;
        }


        /// <summary>
        /// Finds the potential diagonals.
        /// </summary>
        /// <param name="potentialDiagonals">The potential diagonals.</param>
        /// <param name="length">The length.</param>
        /// <param name="minimalConsideration">The minimal consideration.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private bool findPotentialDiagonals(out List<int>[] potentialDiagonals, int length, double minimalConsideration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether [is gauss seidel appropriate] [the specified b].
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="potentialDiagonals">The potential diagonals.</param>
        /// <param name="initialGuess">The initial guess.</param>
        /// <returns><c>true</c> if [is gauss seidel appropriate] [the specified b]; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
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
        /// <exception cref="System.NotImplementedException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public double[] solveIteratively(IList<double> b, IList<double> initialGuess = null,
            List<int>[] potentialDiagonals = null)
        {
            throw new NotImplementedException();
        }
    }
}