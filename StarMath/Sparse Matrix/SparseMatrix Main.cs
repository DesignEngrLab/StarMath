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

namespace StarMathLib
{
    /// <summary>
    /// Class SparseMatrix.
    /// </summary>
    public partial class SparseMatrix
    {
        private readonly SparseCell[] cellsRowbyRow;

        /// <summary>
        /// The first non-zero cell in each row.
        /// </summary>
        internal SparseCell[] RowFirsts { get; private set; }

        /// <summary>
        /// The last non-zero cell in each row.
        /// </summary>
        internal SparseCell[] RowLasts { get; private set; }

        /// <summary>
        /// The first non-zero cell in each column.
        /// </summary>
        internal SparseCell[] ColFirsts { get; private set; }

        /// <summary>
        /// The last non-zero cell in each column.
        /// </summary>
        internal SparseCell[] ColLasts { get; private set; }

        /// <summary>
        /// The number non zero
        /// </summary>
        public int NumNonZero { get; private set; }
        /// <summary>
        /// The number cols
        /// </summary>
        public int NumCols { get; private set; }
        /// <summary>
        /// The number rows
        /// </summary>
        public int NumRows { get; private set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix" /> class.
        /// </summary>
        /// <param name="rowIndices">The row indices.</param>
        /// <param name="colIndices">The col indices.</param>
        /// <param name="values">The values.</param>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        public SparseMatrix(IList<int> rowIndices, IList<int> colIndices, IList<double> values, int numRows, int numCols) : this(numRows, numCols, values.Count)
        {
            IOrderedEnumerable<int> indices = Enumerable.Range(0, NumNonZero).OrderBy(i => rowIndices[i] * numCols + colIndices[i]);
            var rowByRowIndices = indices.Select(i => rowIndices[i] * numCols + colIndices[i]).ToArray();
            var rowByRowValues = indices.Select(i => values[i]).ToArray();
            FillInSparseMatrix(this, rowByRowIndices, rowByRowValues);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        /// <param name="cellDictionary">The cell dictionary with keys as [i,j] pairs.</param>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        public SparseMatrix(Dictionary<int[], double> cellDictionary, int numRows, int numCols) : this(numRows, numCols, cellDictionary.Count)
        {
            var orderedCellDictionary = cellDictionary.OrderBy(x => x.Key[0] * numCols + x.Key[1]);
            var rowByRowIndices = orderedCellDictionary.Select(x => x.Key[0] * numCols + x.Key[1]).ToArray();
            var rowByRowValues = orderedCellDictionary.Select(x => x.Value).ToArray();
            FillInSparseMatrix(this, rowByRowIndices, rowByRowValues);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        /// <param name="cellDictionary">The cell dictionary with keys as single row-by-row indices.</param>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        public SparseMatrix(Dictionary<int, double> cellDictionary, int numRows, int numCols) : this(numRows, numCols, cellDictionary.Count)
        {
            var orderedCellDictionary = cellDictionary.OrderBy(x => x.Key);
            var rowByRowIndices = orderedCellDictionary.Select(x => x.Key).ToArray();
            var rowByRowValues = orderedCellDictionary.Select(x => x.Value).ToArray();
            FillInSparseMatrix(this, rowByRowIndices, rowByRowValues);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        /// <param name="numValues">The number values.</param>
        public SparseMatrix(int numRows, int numCols, int numValues) : this(numRows, numCols)
        {
            NumNonZero = numValues;
            cellsRowbyRow = new SparseCell[NumNonZero];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        public SparseMatrix(int numRows, int numCols)
        {
            NumRows = numRows;
            NumCols = numCols;
            RowFirsts = new SparseCell[numRows];
            RowLasts = new SparseCell[numRows];
            ColFirsts = new SparseCell[numCols];
            ColLasts = new SparseCell[numCols];
        }
        /// <summary>
        /// Fills the in sparse matrix.
        /// </summary>
        /// <param name="newMatrix">The new matrix.</param>
        /// <param name="rowByRowIndices">The row by row indices.</param>
        /// <param name="rowByRowValues">The row by row values.</param>
        private static void FillInSparseMatrix(SparseMatrix newMatrix, IList<int> rowByRowIndices, IList<double> rowByRowValues)
        {
            var rowI = 0;
            var rowLowerLimit = 0;
            var rowUpperLimit = newMatrix.NumCols;
            var newRow = true;
            for (var i = 0; i < newMatrix.NumNonZero; i++)
            {
                var index = rowByRowIndices[i];
                var value = rowByRowValues[i];
                while (i < newMatrix.NumNonZero - 1 && rowByRowIndices[i] == rowByRowIndices[i + 1])
                {
                    i++;
                    value += rowByRowValues[i];
                }
                while (index >= rowUpperLimit)
                {
                    newRow = true;
                    rowI++;
                    rowLowerLimit += newMatrix.NumCols;
                    rowUpperLimit += newMatrix.NumCols;
                }
                var colI = index - rowLowerLimit;
                var cell = new SparseCell(rowI, colI, value);
                newMatrix.cellsRowbyRow[i] = cell;
                if (newRow)
                {
                    newMatrix.RowFirsts[rowI] = cell;
                    newMatrix.RowLasts[rowI] = cell;
                    newRow = false;
                }
                else
                {
                    cell.Left = newMatrix.RowLasts[rowI];
                    newMatrix.RowLasts[rowI].Right = cell;
                    newMatrix.RowLasts[rowI] = cell;
                }
                if (newMatrix.ColFirsts[colI] == null)
                {
                    newMatrix.ColFirsts[colI] = cell;
                    newMatrix.ColLasts[colI] = cell;
                }
                else
                {
                    cell.Up = newMatrix.ColLasts[colI];
                    newMatrix.ColLasts[colI].Down = cell;
                    newMatrix.ColLasts[colI] = cell;
                }
            }

        }
        #endregion

        /// <summary>
        /// Gets or sets the <see cref="System.Double" /> with the specified row i.
        /// </summary>
        /// <param name="rowI">The row i.</param>
        /// <param name="colI">The col i.</param>
        /// <returns>System.Double.</returns>
        public double this[int rowI, int colI]
        {
            get
            {
                var c = CellAt(rowI, colI);
                if (c == null) return 0.0;
                else return c.Value;
            }
            set
            {
                var c = CellAt(rowI, colI);
                if (c == null) AddCell(rowI, colI, value);
                else c.Value = value;
            }
        }

        private SparseCell CellAt(int rowI, int colI)
        {
            if (rowI >= colI) return SearchRightToCell(colI, RowFirsts[rowI]);
            return SearchDownToCell(rowI, ColFirsts[colI]);
        }

        /// <summary>
        /// Searches the left to.
        /// </summary>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="startCell">The start cell.</param>
        /// <returns>SparseCell.</returns>
        private SparseCell SearchRightToCell(int colIndex, SparseCell startCell)
        {
            do
            {
                if (startCell == null || startCell.ColIndex > colIndex)
                    return null;
                if (startCell.ColIndex == colIndex) return startCell;
                startCell = startCell.Right;
            } while (true);
        }
        /// <summary>
        /// Searches down to.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="startCell">The start cell.</param>
        /// <returns>SparseCell.</returns>
        /// <exception cref="Exception">No non-zero sparse matrix cell found at the location.</exception>
        private SparseCell SearchDownToCell(int rowIndex, SparseCell startCell)
        {
            do
            {
                if (startCell == null || startCell.RowIndex > rowIndex)
                    return null;
                if (startCell.ColIndex == rowIndex) return startCell;
                startCell = startCell.Down;
            } while (true);
        }

        private SparseMatrix Copy()
        {
            var copy = new SparseMatrix(NumRows, NumCols, NumNonZero);
            FillInSparseMatrix(copy,
                cellsRowbyRow.Select(x => x.RowIndex * NumCols + x.ColIndex).ToArray(),
                cellsRowbyRow.Select(c => c.Value).ToArray());
            return copy;
        }

        private void RemoveCell(SparseCell cell)
        {
            NumNonZero--;
            if (cell.Left == null)
                RowFirsts[cell.RowIndex] = cell.Right;
            else cell.Left.Right = cell.Right;
            if (cell.Right == null)
                RowLasts[cell.RowIndex] = cell.Left;
            else cell.Right.Left = cell.Left;

            if (cell.Up == null)
                ColFirsts[cell.ColIndex] = cell.Down;
            else cell.Up.Down = cell.Down;
            if (cell.Down == null)
                ColLasts[cell.ColIndex] = cell.Up;
            else cell.Down.Up = cell.Up;
        }

        private SparseCell AddCell(int rowI, int colI, double value = Double.NaN)
        {
            NumNonZero++;
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
                var startCell = RowFirsts[rowI];
                while (startCell.ColIndex < colI)
                    startCell = startCell.Right;
                cell.Right = startCell;
                cell.Left = startCell.Left;
                cell.Left.Right = cell;
                startCell.Left = cell;
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
                var startCell = ColFirsts[colI];
                while (startCell.RowIndex < rowI)
                    startCell = startCell.Down;
                cell.Down = startCell;
                cell.Up = startCell.Up;
                cell.Up.Down = cell;
                startCell.Up = cell;
            }

            return cell;
        }
    }

    /// <summary>
    /// Class SparseCell.
    /// </summary>
    internal class SparseCell
    {
        /// <summary>
        /// The col index
        /// </summary>
        internal int ColIndex;

        /// <summary>
        /// Down
        /// </summary>
        internal SparseCell Down;

        /// <summary>
        /// The left
        /// </summary>
        internal SparseCell Left;

        /// <summary>
        /// The right
        /// </summary>
        internal SparseCell Right;

        /// <summary>
        /// The row index
        /// </summary>
        internal int RowIndex;

        /// <summary>
        /// Up
        /// </summary>
        internal SparseCell Up;

        /// <summary>
        /// The value
        /// </summary>
        internal double Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseCell" /> class.
        /// </summary>
        /// <param name="rowIndex">Index of the row.</param>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="value">The value.</param>
        public SparseCell(int rowIndex, int colIndex, double value)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;
            Value = value;
        }
    }
}