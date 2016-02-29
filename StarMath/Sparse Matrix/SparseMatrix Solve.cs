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
using System.Linq;
using StarMathLib.CSparse;

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
                return SolveIteratively(b, initialGuess, potentialDiagonals);
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
        public double[] SolveAnalytically(IList<double> b, bool IsASymmetric = false,
           List<int>[] potentialDiagonals = null)
        {
            if (IsASymmetric)
            {
                var ccs = convertToCCS(this);
                var S = Main.SymbolicAnalysisLDL(ccs);
                double[] D;
                CompressedColumnStorage L;
                Main.FactorizeLDL(ccs, S, out D, out L);
                return Main.SolveLDL(b, L, D, S.InversePermute);

                /*** old code
                var L = Copy();
                L.CholeskyDecomposition();
                return L.solveFromCholeskyFactorization(b);
                ***/
            }
            else
            {
                var ccs = convertToCCS(this);
                var columnPermutation = ApproximateMinimumDegree.Generate(ccs);
                CompressedColumnStorage L, U;
                int[] pinv;
                // Numeric LU factorization
                Main.FactorizeLU(ccs, columnPermutation, out L, out U, out pinv);
                var x = Main.ApplyInverse(pinv, b, NumCols); // x = b(p)
                Main.SolveLower(L, x); // x = L\x.
                Main.SolveUpper(U, x); // x = U\x.
                return Main.ApplyInverse(columnPermutation, x, NumCols); // b(q) = x
                /*** old code
                var LU = Copy();
                LU.LUDecomposition();
                return LU.solveFromLUDecomposition(b);
                ***/
            }
        }
        private static CompressedColumnStorage convertToCCS(SparseMatrix sparseMatrix)
        {
            var cellCounter = 0;
            var colCounter = 0;
            var ccs = new CompressedColumnStorage(sparseMatrix.NumRows, sparseMatrix.NumCols, sparseMatrix.NumNonZero);
            var columnPointers = new int[sparseMatrix.NumCols + 1];
            columnPointers[0] = cellCounter;
            var rowIndices = new int[sparseMatrix.NumNonZero];
            var values = new double[sparseMatrix.NumNonZero];

            foreach (var topcell in sparseMatrix.ColFirsts)
            {
                var cell = topcell;
                while (cell != null)
                {
                    values[cellCounter] = cell.Value;
                    rowIndices[cellCounter] = cell.RowIndex;
                    cell = cell.Down;
                    cellCounter++;
                }
                columnPointers[++colCounter] = cellCounter;
            }
            ccs.ColumnPointers = columnPointers;
            ccs.RowIndices = rowIndices;
            ccs.Values = values;
            return ccs;
        }
        #region Old Methods - someday one needs to rewrite to avoid CCS and use thesse methods
        private double[] solveFromLUDecomposition(IList<double> b)
        {
            var x = new double[NumRows];
            // forward substitution
            for (int i = 0; i < NumRows; i++)
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
            for (int i = NumRows - 1; i >= 0; i--)
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
        /// <returns>System.Double[].</returns>
        private double[] solveFromCholeskyFactorization(IList<double> b)
        {
            var x = new double[NumRows];
            // forward substitution
            for (int i = 0; i < NumRows; i++)
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
            for (int i = 0; i < NumRows; i++)
                x[i] /= Diagonals[i].Value;

            // backward substitution
            for (int i = NumRows - 1; i >= 0; i--)
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
        #endregion
        #region Gauss-Seidel or Successive Over-Relaxation

        private bool isGaussSeidelAppropriate(IList<double> b, out List<int>[] potentialDiagonals,
            ref IList<double> initialGuess)
        {
            potentialDiagonals = null;
            if (NumRows < StarMath.GaussSeidelMinimumMatrixSize) return false;
            if (initialGuess == null)
                initialGuess = makeInitialGuess(b);
            var error = StarMath.norm1(StarMath.subtract(b, multiply(initialGuess))) / b.norm1();
            if (error > StarMath.MaxErrorForUsingGaussSeidel) return false;
            return findPotentialDiagonals(out potentialDiagonals, StarMath.GaussSeidelDiagonalDominanceRatio);
        }

        private bool findPotentialDiagonals(out List<int>[] potentialDiagonals,
            double minimalConsideration)
        {
            potentialDiagonals = new List<int>[NumRows];
            for (var i = 0; i < NumRows; i++)
            {
                var cell = RowFirsts[i];
                var norm1 = 0.0;
                do
                {
                    norm1 += Math.Abs(cell.Value);
                    cell = cell.Right;
                } while (cell != null);
                var potentialIndices = new List<int>();
                cell = RowFirsts[i];
                do
                {
                    if (Math.Abs(cell.Value) / (norm1 - cell.Value) > minimalConsideration)
                        potentialIndices.Add(cell.ColIndex);
                    cell = cell.Right;
                } while (cell != null);
                if (potentialIndices.Count == 0) return false;
                potentialDiagonals[i] = potentialIndices;
            }
            return potentialDiagonals.SelectMany(x => x).Distinct().Count() == NumRows;
        }

        public double[] SolveIteratively(IList<double> b,
            IList<double> initialGuess = null, List<int>[] potentialDiagonals = null)
        {
            double[] x;
            if (initialGuess == null)
                x = makeInitialGuess(b);
            else x = initialGuess.ToArray();

            if (potentialDiagonals == null &&
                !findPotentialDiagonals(out potentialDiagonals, StarMath.GaussSeidelDiagonalDominanceRatio))
                return null;
            var order = Enumerable.Range(0, NumRows).ToArray();
            if (!order.All(rowIndex => potentialDiagonals[rowIndex].Contains(rowIndex)))
                order = reorderMatrixForDiagonalDominance(NumRows, potentialDiagonals);
            if (order == null) return null;
            var bNorm1 = StarMath.norm1(b);
            var error = StarMath.norm1(StarMath.subtract(b, multiply(x))) / bNorm1;
            var success = error <= StarMath.GaussSeidelMaxError;
            var xWentNaN = false;
            var iteration = NumRows * StarMath.GaussSeidelMaxIterationFactor;
            while (!xWentNaN && !success && iteration-- > 0)
            {
                for (var i = 0; i < NumRows; i++)
                {
                    var rowI = order[i];
                    var diagCell = Diagonals[i];
                    var adjust = b[rowI];
                    var cell = RowFirsts[rowI];
                    do
                    {
                        if (cell != diagCell)
                            adjust -= cell.Value * x[cell.ColIndex];
                        cell = cell.Right;
                    } while (cell != null);
                    x[rowI] = (1 - StarMath.GaussSeidelRelaxationOmega) * x[rowI] +
                           StarMath.GaussSeidelRelaxationOmega * adjust / this[rowI, i];
                }
                xWentNaN = x.Any(double.IsNaN);
                error = StarMath.norm1(StarMath.subtract(b, multiply(x))) / bNorm1;
                success = error <= StarMath.GaussSeidelMaxError;
            }
            if (!success) return null;

            return x;
        }

        private double[] makeInitialGuess(IList<double> b)
        {
            var initialGuess = new double[NumRows];
            var initGuessValue = StarMath.SumAllElements(b) / SumAllElements();
            for (var i = 0; i < NumRows; i++) initialGuess[i] = initGuessValue;
            return initialGuess;
        }
        private int[] reorderMatrixForDiagonalDominance(int length, List<int>[] potentialIndices)
        {
            var popularity = new int[length];
            for (var i = 0; i < length; i++)
                popularity[i] = potentialIndices.Count(r => r.Contains(i));
            var orderToAddress = StarMath.makeLinearProgression(length, 1).OrderBy(x => popularity[x]).ToList();
            var stack = new Stack<int[]>();
            var seed = new int[length];
            int[] candidate;
            var solutionFound = false;
            for (var i = 0; i < length; i++) seed[i] = -1;
            stack.Push(seed);
            do
            {
                candidate = stack.Pop();
                var numToFill = candidate.Count(x => x == -1);
                if (numToFill == 0) solutionFound = true;
                else
                {
                    var colIndex = orderToAddress[length - numToFill];
                    var possibleIndicesForRow = new List<int>();
                    for (var oldRowIndex = 0; oldRowIndex < length; oldRowIndex++)
                    {
                        if (!potentialIndices[oldRowIndex].Contains(colIndex)) continue;
                        if (candidate.Contains(oldRowIndex)) continue;
                        possibleIndicesForRow.Add(oldRowIndex);
                    }
                    if (possibleIndicesForRow.Count == 1)
                    {
                        candidate[colIndex] = possibleIndicesForRow[0];
                        stack.Push(candidate);
                    }
                    else
                    {
                        possibleIndicesForRow = possibleIndicesForRow.OrderBy(r => Math.Abs(this[r, colIndex])).ToList();
                        foreach (var i in possibleIndicesForRow)
                        {
                            var child = (int[])candidate.Clone();
                            child[colIndex] = i;
                            stack.Push(child);
                        }
                    }
                }
            } while (!solutionFound && stack.Any());
            if (solutionFound) return candidate;
            return null;
        }

        #endregion
    }
}
