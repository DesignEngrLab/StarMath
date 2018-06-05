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

//using StarMathLib.CSparse;

//using StarMathLib.CSparseClasses;

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
        public IList<double> solve(IList<double> b, IList<double> initialGuess = null,
            bool IsASymmetric = false)
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
            return SolveAnalytically(b, IsASymmetric);
        }

        public double[] solve(IList<double> bValues, IList<int> bIndices, IList<double> initialGuess = null,
    bool IsASymmetric = false)
        {
            if (NumRows != NumCols)
                throw new ArithmeticException("Spare Matrix must be square to solve Ax = b.");
            List<int>[] potentialDiagonals;
            if (isGaussSeidelAppropriate(bValues, bIndices, out potentialDiagonals, ref initialGuess))
                return SolveIteratively(bValues, bIndices, initialGuess, potentialDiagonals);
            /****** need code to determine when to switch between *****
             ****** this analytical approach and the SOR approach *****/
            return SolveAnalytically(bValues, bIndices, IsASymmetric);
        }

        private double[] SolveIteratively(IList<double> bValues, IList<int> bIndices, IList<double> initialGuess, List<int>[] potentialDiagonals)
        {
            throw new NotImplementedException();
        }

        private bool isGaussSeidelAppropriate(IList<double> bValues, IList<int> bIndices, out List<int>[] potentialDiagonals, ref IList<double> initialGuess)
        {
            throw new NotImplementedException();
        }

        private double[] SolveAnalytically(IList<double> bValues, IList<int> bIndices, bool isASymmetric)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Solves the system of equations analytically.
        /// </summary>
        /// <param name="b">The b.</param>
        /// <param name="IsASymmetric">if set to <c>true</c> [a is symmetric].</param>
        /// <param name="potentialDiagonals">The potential diagonals.</param>
        /// <returns>System.Double[].</returns>
        public IList<double> SolveAnalytically(IList<double> b, bool IsASymmetric = false)
        {
            if (IsASymmetric)
            {
                var ccs = convertToCCS(this);
                var S = CSparse.SymbolicAnalysisLDL(ccs);
                double[] D;
                CompressedColumnStorage L;
                CSparse.FactorizeLDL(ccs, S, out D, out L);
                return CSparse.SolveLDL(b, L, D, S.InversePermute);
            }
            else
            {
                var ccs = convertToCCS(this);
                var columnPermutation = ApproximateMinimumDegree.Generate(
                    new SymbolicColumnStorage(ccs), NumCols);
                CompressedColumnStorage L, U;
                int[] pinv;
                // Numeric LU factorization
                CSparse.FactorizeLU(ccs, columnPermutation, out L, out U, out pinv);
                var x = CSparse.ApplyInverse(pinv, b, NumCols); // x = b(p)
                CSparse.SolveLower(L, x); // x = L\x.
                CSparse.SolveUpper(U, x); // x = U\x.
                return CSparse.ApplyInverse(columnPermutation, x, NumCols); // b(q) = x
                /*** old code
                var LU = Copy();
                LU.LUDecomposition();
                return LU.solveFromLUDecomposition(b);
                ***/
            }
        }
        public IList<double> SolveAnalytically2(IList<double> b, bool IsASymmetric = false)
        {
            if (IsASymmetric)
            {
                if (TopologyChanged)
                {
                    var ccs = convertToCCS(this);
                    var S = CSparse.SymbolicAnalysisLDL(ccs);
                    invPermutationVector = S.InversePermute;
                    //invPermutationVector = Enumerable.Range(0, NumCols).ToArray();
                    permutationVector = InvertPermutation(invPermutationVector, NumCols);
                    CreateLDLFactorization(permutationVector, invPermutationVector);
                    SolveFactorization();
                    ValuesChanged = false;
                    TopologyChanged = false;
                }
                if (ValuesChanged)
                {
                    SolveFactorization();
                    ValuesChanged = false;
                }
                var x = ApplyPermutation(b, permutationVector, NumCols);
                x = FactorizationMatrix.SolveLowerTriangularMatrix(x, true, true);
                x = FactorizationMatrix.SolveUpperTriangularMatrix(x, true, false);
                return ApplyPermutation(x, invPermutationVector, NumCols);
            }
            else
            {
                var ccs = convertToCCS(this);
                var columnPermutation = ApproximateMinimumDegree.Generate(new SymbolicColumnStorage(ccs), NumCols);
                CompressedColumnStorage L, U;
                int[] pinv;
                // Numeric LU factorization
                CSparse.FactorizeLU(ccs, columnPermutation, out L, out U, out pinv);
                var x = CSparse.ApplyInverse(pinv, b, NumCols); // x = b(p)
                CSparse.SolveLower(L, x); // x = L\x.
                CSparse.SolveUpper(U, x); // x = U\x.
                return CSparse.ApplyInverse(columnPermutation, x, NumCols); // b(q) = x
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
        private IList<double> solveFromLUDecomposition(IList<double> b, bool CanOverWriteB)
        {
            var x = SolveLowerTriangularMatrix(b, CanOverWriteB, true);
            return SolveUpperTriangularMatrix(x, CanOverWriteB, false);
        }

        private IList<double> SolveLowerTriangularMatrix(IList<double> b, bool CanOverWriteB, bool IncludeDiagonal)
        {
            var x = CanOverWriteB ? b : b.ToArray();
            var i = 0;
            while (x[i] == 0.0) i++;
            for (; i < NumRows; i++)
            {
                if (x[i] == 0.0) continue;
                var diag = Diagonals[i];
                var cell = ColLasts[i];
                while (cell != diag)
                {
                    x[cell.RowIndex] -= cell.Value * x[cell.ColIndex];
                    cell = cell.Up;
                }
            }
            if (IncludeDiagonal)
                for (i = 0; i < NumRows; i++)
                {
                    if (x[i] == 0.0) continue;
                    x[i] /= Diagonals[i].Value;
                }
            return x;
        }

        private IList<double> SolveUpperTriangularMatrix(IList<double> b, bool CanOverWriteB, bool IncludeDiagonal)
        {
            var x = CanOverWriteB ? b : b.ToArray();
            var i = NumRows - 1;
            while (x[i] == 0.0) i--;
            for (; i >= 0; i--)
            {
                if (x[i] == 0.0) continue;
                var diag = Diagonals[i];
                var cell = RowFirsts[i];
                while (cell != diag)
                {
                    x[cell.ColIndex] -= cell.Value * x[cell.RowIndex];
                    cell = cell.Right;
                }
            }
            if (IncludeDiagonal)
                for (i = 0; i < NumRows; i++)
                {
                    if (x[i] == 0.0) continue;
                    x[i] /= Diagonals[i].Value;
                }
            return x;
        }

        private void LUDecomposition()
        {
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
        /// Returns the L+D Sparse Matrix via Cholesky decomposition (i.e. it is destructive).
        /// This is based on: https://en.wikipedia.org/wiki/Cholesky_decomposition#LDL_decomposition_2
        /// </summary>
        /// <returns>SparseMatrix.</returns>
        /// <exception cref="System.ArithmeticException">Cholesky Decomposition can only be determined for square matrices.</exception>
        /// <exception cref="ArithmeticException">Cholesky Decomposition can only be determined for square matrices.</exception>
        public void CreateLDLFactorization(IList<int> permutationVector, IList<int> invPermutationVector)
        {
            var n = NumRows;
            FactorizationMatrix = new SparseMatrix(n, n);
            for (int rowIndex = 0; rowIndex < n; rowIndex++)
            {
                var cellA = RowFirsts[permutationVector[rowIndex]];
                //clork var sortedCells = new SortedCellList();
                var sortedCells = new SortedDictionary<int, SparseCell>();
                var cellHash = new HashSet<int>();
                CholeskyDCell diagCell = null;
                while (cellA != null)
                {
                    // this just collects cells of A that should be mirrored in L
                    var colIndex = invPermutationVector[cellA.ColIndex];
                    if (colIndex == rowIndex)
                        diagCell = new CholeskyDCell(rowIndex, rowIndex, cellA);
                    else if (colIndex < rowIndex)
                    {
                        cellHash.Add(colIndex);
                        sortedCells.Add(colIndex, new CholeskyLCell(rowIndex, colIndex, cellA, FactorizationMatrix.Diagonals[colIndex]));
                    }
                    cellA = cellA.Right;
                }
                while (sortedCells.Any())
                {
                    //clork var lowestEntry = sortedCells.Pop();
                    var lowestEntry = sortedCellsPop(sortedCells);
                    var colIndex = lowestEntry.Key;
                    var anchorCell = lowestEntry.Value;
                    cellHash.Remove(colIndex);
                    AddCellRightAndBelow(FactorizationMatrix, anchorCell, rowIndex, colIndex);
                    var anchorsDiagonal = FactorizationMatrix.Diagonals[colIndex];
                    diagCell.AddDoubletTerm(anchorCell, anchorsDiagonal);
                    var cell = anchorCell.Up;
                    while (cell.RowIndex > colIndex)
                    {
                        var newColIndex = cell.RowIndex;
                        CholeskyLCell newCell;
                        int position;
                        // clork if (sortedCells. PositionIfExists(newColIndex, out position))
                        if (cellHash.Contains(newColIndex))
                            //newCell = (CholeskyLCell)sortedCells[position];
                            newCell = (CholeskyLCell)sortedCells[newColIndex];
                        else
                        {
                            newCell = new CholeskyLCell(rowIndex, newColIndex, null, FactorizationMatrix.Diagonals[newColIndex]);
                            //clork sortedCells.Insert(position, newColIndex, newCell);
                            //sortedCells.Add(position, newCell);
                            cellHash.Add(newColIndex);
                            sortedCells.Add(newColIndex, newCell);
                        }
                        newCell.AddTripletTerm(anchorCell, cell, anchorsDiagonal);
                        cell = cell.Up;
                    }
                }
                AddCellRightAndBelow(FactorizationMatrix, diagCell, rowIndex, rowIndex);
            }
        }

        private KeyValuePair<int, SparseCell> sortedCellsPop(SortedDictionary<int, SparseCell> sortedCells)
        {

            var top = sortedCells.First();
            sortedCells.Remove(top.Key);
            return top;
        }

        private void AddCellRightAndBelow(SparseMatrix matrix, SparseCell cell, int rowIndex, int colIndex)
        {
            matrix.cellsRowbyRow.Add(cell);
            if (rowIndex == colIndex) matrix.Diagonals[colIndex] = cell;
            if (matrix.RowLasts[rowIndex] != null)
            {
                cell.Left = matrix.RowLasts[rowIndex];
                matrix.RowLasts[rowIndex].Right = cell;
                matrix.RowLasts[rowIndex] = cell;
            }
            else matrix.RowFirsts[rowIndex] = matrix.RowLasts[rowIndex] = cell;
            if (matrix.ColLasts[colIndex] != null)
            {
                cell.Up = matrix.ColLasts[colIndex];
                matrix.ColLasts[colIndex].Down = cell;
                matrix.ColLasts[colIndex] = cell;
            }
            else matrix.ColFirsts[colIndex] = matrix.ColLasts[colIndex] = cell;

            matrix.NumNonZero++;
        }
        private void SolveFactorization()
        {
            foreach (var sparseCell in FactorizationMatrix.cellsRowbyRow)
            {
                sparseCell.Evaluate();
            }
        }

        private static IList<double> ApplyPermutation(IList<double> b, int[] permutationVector, int length)
        {
            var x = new double[length];
            for (var i = 0; i < length; i++)
                x[i] = b[permutationVector[i]];
            return x;
        }
        private int[] InvertPermutation(int[] p, int n)
        {
            var pinv = new int[n];
            // Invert the permutation.
            for (int i = 0; i < n; i++)
                pinv[p[i]] = i;

            return pinv;
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
