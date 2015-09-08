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
        #region Fields and Properties
        private readonly HashSet<SparseCell> cellsRowbyRow;

        /// <summary>
        /// The first non-zero cell in each row.
        /// </summary>
        internal SparseCell[] Diagonals { get; private set; }

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
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix" /> class.
        /// </summary>
        /// <param name="rowIndices">The row indices.</param>
        /// <param name="colIndices">The col indices.</param>
        /// <param name="values">The values.</param>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        public SparseMatrix(IList<int> rowIndices, IList<int> colIndices, IList<double> values, int numRows, int numCols) : this(numRows, numCols)
        {
            var indices = Enumerable.Range(0, values.Count).OrderBy(i => rowIndices[i] * numCols + colIndices[i]).ToList();
            var orederedRowByRowIndices = indices.Select(i => rowIndices[i] * numCols + colIndices[i]).ToArray();
            var orderedRowByRowValues = indices.Select(i => values[i]).ToArray();
            FillInSparseMatrix(this, orederedRowByRowIndices, orderedRowByRowValues);
        }

        public SparseMatrix(IList<int> rowByRowIndices, IList<double> values, int numRows, int numCols) : this(numRows, numCols)
        {
            var indices = Enumerable.Range(0, values.Count).OrderBy(i => rowByRowIndices[i]).ToList();
            var orderedRowByRowIndices = indices.Select(i => rowByRowIndices[i]).ToArray();
            var orderedRowByRowValues = indices.Select(i => values[i]).ToArray();
            FillInSparseMatrix(this, orderedRowByRowIndices, orderedRowByRowValues);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        /// <param name="cellDictionary">The cell dictionary with keys as [i,j] pairs.</param>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        public SparseMatrix(Dictionary<int[], double> cellDictionary, int numRows, int numCols) : this(numRows, numCols)
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
        public SparseMatrix(Dictionary<int, double> cellDictionary, int numRows, int numCols) : this(numRows, numCols)
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
        public SparseMatrix(int numRows, int numCols)
        {
            cellsRowbyRow = new HashSet<SparseCell>();
            NumRows = numRows;
            NumCols = numCols;
            Diagonals = new SparseCell[numRows];
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
            for (var i = 0; i < rowByRowValues.Count; i++)
            {
                var index = rowByRowIndices[i];
                var value = rowByRowValues[i];
                while (i < rowByRowValues.Count - 1 && rowByRowIndices[i] == rowByRowIndices[i + 1])
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
                if (rowI == colI) newMatrix.Diagonals[rowI] = cell;
                newMatrix.cellsRowbyRow.Add(cell);
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
            newMatrix.NumNonZero = newMatrix.cellsRowbyRow.Count;
        }
        #endregion

        #region Finding Cell(s) Methods
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
            if (rowI == colI) return Diagonals[rowI];
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
        /// <exception cref="ArithmeticException">No non-zero sparse matrix cell found at the location.</exception>
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
        #endregion

        private SparseMatrix Copy()
        {
            return new SparseMatrix(cellsRowbyRow.Select(x => x.RowIndex * NumCols + x.ColIndex).ToArray(),
                cellsRowbyRow.Select(c => c.Value).ToArray(),NumRows,NumCols);
        }

        private void RemoveCell(SparseCell cell)
        {
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
            cellsRowbyRow.Remove(cell);
            NumNonZero--;
        }

        private SparseCell AddCell(int rowI, int colI, double value = Double.NaN)
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
            if (rowI == colI) Diagonals[rowI] = cell;
            cellsRowbyRow.Add(cell);
            NumNonZero++;

            return cell;
        }

        /// <summary>
        /// Finds the index of the insertion within the cellsRowbyRow. Of course, there are built-in functions to do this,
        /// but the idea with writing our own is that we can make a faster search given information that is known about
        /// this .
        /// </summary>
        /// <param name="cell">The cell.</param>
        /// <returns>System.Int32.</returns>
        //private int FindInsertionIndex(SparseCell cell)
        //{
           //int i= cellsRowbyRow.IndexOf(cell);
           // if (i >= 0) return i;
            //var averageCellPerRow = NumNonZero / NumRows;
            //var index = Math.Min(averageCellPerRow * cell.RowIndex + cell.ColIndex, NumNonZero - 1);
            //int step = averageCellPerRow;
            //do
            //{
            //    if (cell.RowIndex < cellsRowbyRow[index].RowIndex
            //        || (cell.RowIndex == cellsRowbyRow[index].RowIndex
            //        && cell.ColIndex < cellsRowbyRow[index].ColIndex))
            //    {
            //        if (index == 0 || step == 1) step = 0;
            //        else if (step > 0) step = -step / 2;
            //    }
            //    else if (cell.RowIndex > cellsRowbyRow[index].RowIndex
            //        || (cell.RowIndex == cellsRowbyRow[index].RowIndex
            //        && cell.ColIndex > cellsRowbyRow[index].ColIndex))
            //    {
            //        if (index == NumNonZero - 1 || step == -1) step = 0;
            //        else if (step < 0) step = -step / 2;
            //    }
            //    else step = 0;
            //    index += step;
            //    if (index < 0)
            //    {
            //        step -= index;
            //        index = 0;
            //    }
            //    else if (index >= NumNonZero)
            //    {
            //        step = index - (NumNonZero - 1);
            //        index = NumNonZero - 1;
            //    }
            //} while (step != 0);
            //return index;
        //}


        /// <summary>
        /// Removes the row.
        /// </summary>
        /// <param name="rowIndexToRemove">The row index to remove.</param>
        public void RemoveRow(int rowIndexToRemove)
        {
            var thisCell = RowFirsts[rowIndexToRemove];
            while (thisCell != null)
            {
                var nextCell = thisCell.Right;
                RemoveCell(thisCell);
                thisCell = nextCell;
            }

            NumRows--;
            var newRowFirsts = new SparseCell[NumRows];
            var newRowLasts = new SparseCell[NumRows];

            for (int i = 0; i < rowIndexToRemove; i++)
            {
                newRowFirsts[i] = RowFirsts[i];
                newRowLasts[i] = RowLasts[i];
            }
            for (int i = rowIndexToRemove; i < NumRows; i++)
            {
                newRowFirsts[i] = RowFirsts[i + 1];
                newRowLasts[i] = RowLasts[i + 1];
                var cell = RowFirsts[i + 1];
                while (cell != null)
                {
                    cell.RowIndex = i;
                    if (cell.ColIndex == i) Diagonals[i] = cell;
                    cell = cell.Right;
                }
            }
            RowFirsts = newRowFirsts;
            RowLasts = newRowLasts;
        }
        /// <summary>
        /// Removes the column.
        /// </summary>
        /// <param name="colIndexToRemove">The col index to remove.</param>
        public void RemoveColumn(int colIndexToRemove)
        {
            var thisCell = ColFirsts[colIndexToRemove];
            while (thisCell != null)
            {
                var nextCell = thisCell.Down;
                RemoveCell(thisCell);
                thisCell = nextCell;
            }

            NumCols--;
            var newColFirsts = new SparseCell[NumCols];
            var newColLasts = new SparseCell[NumCols];

            for (int i = 0; i < colIndexToRemove; i++)
            {
                newColFirsts[i] = ColFirsts[i];
                newColLasts[i] = ColLasts[i];
            }
            for (int i = colIndexToRemove; i < NumCols; i++)
            {
                newColFirsts[i] = ColFirsts[i + 1];
                newColLasts[i] = ColLasts[i + 1];
                var cell = ColFirsts[i + 1];
                while (cell != null)
                {
                    cell.ColIndex = i;
                    if (cell.RowIndex == i) Diagonals[i] = cell;
                    cell = cell.Down;
                }
            }
            ColFirsts = newColFirsts;
            ColLasts = newColLasts;
        }
        /// <summary>
        /// Removes the rows.
        /// </summary>
        /// <param name="rowIndicesToRemove">The row indices to remove.</param>
        public void RemoveRows(IList<int> rowIndicesToRemove)
        {
            var numToRemove = rowIndicesToRemove.Count;
            var removeIndices = rowIndicesToRemove.OrderBy(i => i).ToArray();
            for (int i = 0; i < numToRemove; i++)
            {
                var cell = RowFirsts[removeIndices[i]];
                while (cell != null)
                {
                    var nextCell = cell.Right;
                    RemoveCell(cell);
                    cell = nextCell;
                }
            }
            NumRows -= numToRemove;
            var newRowFirsts = new SparseCell[NumRows];
            var newRowLasts = new SparseCell[NumRows];
            var offset = 0;
            for (int i = 0; i < NumRows; i++)
            {
                while (offset < numToRemove && (i + offset) == removeIndices[offset])
                    offset++;
                newRowFirsts[i] = RowFirsts[i + offset];
                newRowLasts[i] = RowLasts[i + offset];
                var cell = RowFirsts[i + offset];
                while (cell != null)
                {
                    cell.RowIndex = i;
                    if (cell.ColIndex == i) Diagonals[i] = cell;
                    cell = cell.Right;
                }
            }
            RowFirsts = newRowFirsts;
            RowLasts = newRowLasts;
        }
        /// <summary>
        /// Removes the columns.
        /// </summary>
        /// <param name="colIndicesToRemove">The col indices to remove.</param>
        public void RemoveColumns(IList<int> colIndicesToRemove)
        {
            var numToRemove = colIndicesToRemove.Count;
            var removeIndices = colIndicesToRemove.OrderBy(i => i).ToArray();
            for (int i = 0; i < numToRemove; i++)
            {
                var cell = ColFirsts[removeIndices[i]];
                while (cell != null)
                {
                    var nextCell = cell.Down;
                    RemoveCell(cell);
                    cell = nextCell;
                }
            }
            NumCols -= numToRemove;
            var newColFirsts = new SparseCell[NumCols];
            var newColLasts = new SparseCell[NumCols];
            var offset = 0;
            for (int i = 0; i < NumCols; i++)
            {
                while (offset < numToRemove && (i + offset) == removeIndices[offset])
                    offset++;
                newColFirsts[i] = ColFirsts[i + offset];
                newColLasts[i] = ColLasts[i + offset];
                var cell = ColFirsts[i + offset];
                while (cell != null)
                {
                    cell.ColIndex = i;
                    if (cell.RowIndex == i) Diagonals[i] = cell;
                    cell = cell.Down;
                }
            }
            ColFirsts = newColFirsts;
            ColLasts = newColLasts;
        }

        public void Transpose()
        {
            var tempArray = RowFirsts;
            RowFirsts = ColFirsts;
            ColFirsts = tempArray;
            tempArray = RowLasts;
            RowLasts = ColLasts;
            ColLasts = tempArray;
            foreach (var sparseCell in cellsRowbyRow)
            {
                var tempCell = sparseCell.Right;
                sparseCell.Right = sparseCell.Down;
                sparseCell.Down = tempCell;
                tempCell = sparseCell.Left;
                sparseCell.Left = sparseCell.Up;
                sparseCell.Up = tempCell;
            }
            var tempLimit = NumRows;
            NumRows = NumCols;
            NumCols = tempLimit;
            //cellsRowbyRow.Clear();
            //for (int i = 0; i < NumRows; i++)
            //{
            //    var cell = RowFirsts[i];
            //    while (cell != null)
            //    {
            //        cellsRowbyRow.Add(cell);
            //        cell = cell.Right;
            //    }
            //}
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