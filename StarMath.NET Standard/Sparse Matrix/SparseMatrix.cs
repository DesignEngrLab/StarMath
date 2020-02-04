// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-16-2015
// ***********************************************************************
// <copyright file="add subtract multiply.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarMathLib
{
    public class SparseMatrix
    {
        #region Fields and Properties

        public bool TopologyChanged { get; private set; } = true;
        public bool ValuesChanged { get; private set; } = true;
        private readonly List<SparseCell> cellsRowbyRow;

        /// <summary>
        /// The first non-zero cell in each row.
        /// </summary>
        internal SparseCell[] Diagonals { get; }

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
        public SparseMatrix(IList<int> rowIndices, IList<int> colIndices, IList<double> values, int numRows, int numCols)
            : this(numRows, numCols)
        {
            var count = values.Count;
            for (int i = 0; i < count; i++)
                this[rowIndices[i], colIndices[i]] += values[i];
            NumNonZero = cellsRowbyRow.Count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        /// <param name="indices">The row by row indices.</param>
        /// <param name="values">The values.</param>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        /// <param name="InRowOrder">The in row order.</param>
        public SparseMatrix(IList<int> indices, IList<double> values, int numRows, int numCols,
            bool InRowOrder = true) : this(numRows, numCols)
        {
            if (InRowOrder)
            {
                #region Fill-in In Order

                /* this is an elaborate method to speed up the stitching together of new cells */
                var rowI = 0;
                var rowLowerLimit = 0;
                var rowUpperLimit = NumCols;
                var newRow = true;
                for (var i = 0; i < values.Count; i++)
                {
                    var index = indices[i];
                    var value = values[i];
                    while (i < values.Count - 1 && indices[i] == indices[i + 1])
                    {
                        i++;
                        value += values[i];
                    }
                    while (index >= rowUpperLimit)
                    {
                        newRow = true;
                        rowI++;
                        rowLowerLimit += NumCols;
                        rowUpperLimit += NumCols;
                    }
                    var colI = index - rowLowerLimit;
                    var cell = new SparseCell(rowI, colI, value);
                    if (rowI == colI) Diagonals[rowI] = cell;
                    cellsRowbyRow.Add(cell);
                    if (newRow)
                    {
                        RowFirsts[rowI] = cell;
                        RowLasts[rowI] = cell;
                        newRow = false;
                    }
                    else
                    {
                        cell.Left = RowLasts[rowI];
                        RowLasts[rowI].Right = cell;
                        RowLasts[rowI] = cell;
                    }
                    if (ColFirsts[colI] == null)
                    {
                        ColFirsts[colI] = cell;
                        ColLasts[colI] = cell;
                    }
                    else
                    {
                        cell.Up = ColLasts[colI];
                        ColLasts[colI].Down = cell;
                        ColLasts[colI] = cell;
                    }
                }

                #endregion
            }
            else
            {
                var count = values.Count;
                for (int i = 0; i < count; i++)
                {
                    var index = indices[i];
                    var rowI = index / NumCols;
                    var colI = index % NumCols;
                    this[rowI, colI] += values[i];
                }
            }
            NumNonZero = cellsRowbyRow.Count;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        /// <param name="cellDictionary">The cell dictionary with keys as [i,j] pairs.</param>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        public SparseMatrix(Dictionary<int[], double> cellDictionary, int numRows, int numCols) : this(numRows, numCols)
        {
            foreach (var keyValuePair in cellDictionary)
                this[keyValuePair.Key[0], keyValuePair.Key[1]] += keyValuePair.Value;
            NumNonZero = cellsRowbyRow.Count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix"/> class.
        /// </summary>
        /// <param name="numRows">The number rows.</param>
        /// <param name="numCols">The number cols.</param>
        public SparseMatrix(int numRows, int numCols)
        {
            cellsRowbyRow = new List<SparseCell>();
            NumRows = numRows;
            NumCols = numCols;
            Diagonals = new SparseCell[numRows];
            RowFirsts = new SparseCell[numRows];
            RowLasts = new SparseCell[numRows];
            ColFirsts = new SparseCell[numCols];
            ColLasts = new SparseCell[numCols];
        }

        /// <summary>
        /// Updates the values.
        /// </summary>
        /// <param name="rowIndices">The row indices.</param>
        /// <param name="colIndices">The col indices.</param>
        /// <param name="values">The values.</param>
        public void UpdateValues(IList<int> rowIndices, IList<int> colIndices, IList<double> values)
        {
            ValuesChanged = true;
            foreach (var sparseCell in cellsRowbyRow)
                sparseCell.Value = 0;
            var count = values.Count;
            for (int i = 0; i < count; i++)
                this[rowIndices[i], colIndices[i]] += values[i];
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="SparseMatrix" /> class.
        /// </summary>
        /// <param name="rowByRowIndices">The row by row indices.</param>
        /// <param name="values">The values.</param>
        /// <param name="InRowOrder">The in row order.</param>
        public void UpdateValues(IList<int> rowByRowIndices, IList<double> values, bool InRowOrder)
        {
            ValuesChanged = true;
            foreach (var sparseCell in cellsRowbyRow)
                sparseCell.Value = 0;
            var count = values.Count;
            if (InRowOrder)
            {
                var i = 0;
                foreach (var sparseCell in cellsRowbyRow)
                {
                    var cellIndex = rowByRowIndices[i];
                    do
                    {
                        sparseCell.Value += values[i++];
                    } while (i < count && rowByRowIndices[i] == cellIndex);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    var index = rowByRowIndices[i];
                    var rowI = index / NumCols;
                    var colI = index % NumCols;
                    this[rowI, colI] += values[i];
                }
            }
        }


        #endregion

        /// <summary>
        /// Converts the sparse matrix to a dense matrix.
        /// </summary>
        /// <returns>System.Double[].</returns>
        public double[,] ConvertSparseToDenseMatrix()
        {
            var A = new double[NumRows, NumCols];
            for (int i = 0; i < NumRows; i++)
            {
                var cell = RowFirsts[i];
                while (cell != null)
                {
                    A[i, cell.ColIndex] = cell.Value;
                    cell = cell.Right;
                }
            }
            return A;
        }

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
                ValuesChanged = true;
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
        private Boolean TrySearchRightToCell(int colIndex, ref SparseCell startCell)
        {
            while (true)
            {
                if (startCell == null || startCell.ColIndex > colIndex)
                    return false;
                if (startCell.ColIndex == colIndex) return true;
                startCell = startCell.Right;
            }
        }

        /// <summary>
        /// Searches the left to.
        /// </summary>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="startCell">The start cell.</param>
        /// <returns>SparseCell.</returns>
        private SparseCell SearchRightToCell(int colIndex, SparseCell startCell)
        {
            while (true)
            {
                if (startCell == null || startCell.ColIndex > colIndex)
                    return null;
                if (startCell.ColIndex == colIndex) return startCell;
                startCell = startCell.Right;
            }
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
            while (true)
            {
                if (startCell == null || startCell.RowIndex > rowIndex)
                    return null;
                if (startCell.RowIndex == rowIndex) return startCell;
                startCell = startCell.Down;
            }
        }

        #endregion

        public SparseMatrix Copy()
        {
            return new SparseMatrix(cellsRowbyRow.Select(x => x.RowIndex * NumCols + x.ColIndex).ToArray(),
                cellsRowbyRow.Select(c => c.Value).ToArray(), NumRows, NumCols);
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
            TopologyChanged = true;
        }

        private SparseCell AddCell(int rowI, int colI, double value = Double.NaN)
        {
            var cell = new SparseCell(rowI, colI, value);
            // stitch it into the rows
            if (RowFirsts[rowI] == null && RowLasts[rowI] == null)
                RowFirsts[rowI] = RowLasts[rowI] = cell;
            else if (RowFirsts[rowI] == null || RowFirsts[rowI].ColIndex > colI)
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
            if (ColFirsts[colI] == null && ColLasts[colI] == null)
                ColFirsts[colI] = ColLasts[colI] = cell;
            else if (ColFirsts[colI].RowIndex > rowI)
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
            TopologyChanged = true;

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
            TopologyChanged = true;
            var thisCell = RowFirsts[rowIndexToRemove];
            while (thisCell != null)
            {
                var nextCell = thisCell.Right;
                RemoveCell(thisCell);
                if (thisCell.ColIndex == rowIndexToRemove) Diagonals[rowIndexToRemove] = null;
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
            TopologyChanged = true;
            var thisCell = ColFirsts[colIndexToRemove];
            while (thisCell != null)
            {
                var nextCell = thisCell.Down;
                RemoveCell(thisCell);
                if (thisCell.RowIndex == colIndexToRemove) Diagonals[colIndexToRemove] = null;
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
            TopologyChanged = true;
            var numToRemove = rowIndicesToRemove.Count;
            var removeIndices = rowIndicesToRemove.OrderBy(i => i).ToArray();
            for (int i = 0; i < numToRemove; i++)
            {
                var cell = RowFirsts[removeIndices[i]];
                while (cell != null)
                {
                    var nextCell = cell.Right;
                    RemoveCell(cell);
                    if (cell.ColIndex == cell.RowIndex) Diagonals[cell.RowIndex] = null;
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
            TopologyChanged = true;
            var numToRemove = colIndicesToRemove.Count;
            var removeIndices = colIndicesToRemove.OrderBy(i => i).ToArray();
            for (int i = 0; i < numToRemove; i++)
            {
                var cell = ColFirsts[removeIndices[i]];
                while (cell != null)
                {
                    var nextCell = cell.Down;
                    RemoveCell(cell);
                    if (cell.ColIndex == cell.RowIndex) Diagonals[cell.RowIndex] = null;
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
            TopologyChanged = true;
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
                var tempIndex = sparseCell.RowIndex;
                sparseCell.RowIndex = sparseCell.ColIndex;
                sparseCell.ColIndex = tempIndex;
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

        #region Scalars multiplying matrices

        /// <summary>
        /// Multiplies all elements of this sparse matrix with a double value.
        /// </summary>
        /// <param name="a">The double value to be multiplied</param>
        /// <returns>A 2D double array that contains the product</returns>
        public void multiply(double a)
        {
            foreach (var sparseCell in cellsRowbyRow)
                sparseCell.Value *= a;
        }

        /// <summary>
        /// Divides all elements of this sparse matrix with a double value.
        /// </summary>
        /// <param name="a">The double value to be divided by.</param>
        /// <returns>A 2D double array that contains the product</returns>
        public void Divide(double a)
        {
            multiply(1 / a);
        }

        #endregion

        #region Matrix(2D) to matrix(2D) multiplication

        /// <summary>
        /// Multiplies this sparse matrix by a 2D double array. This sparse matrix is
        /// altered to reflect the result.
        /// </summary>
        /// <param name="A">a.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void multiplyInPlace(double[,] A)
        {
            throw new NotImplementedException();
            //var C = new double[numRows, numCols];

            //for (var i = 0; i  NumRows; i++)
            //    for (var j = 0; j != numCols; j++)
            //    {
            //        C[i, j] = 0.0;
            //        for (var k = 0; k != A.GetLength(1); k++)
            //            C[i, j] += A[i, k] * B[k, j];
            //    }
            //return C;
        }

        /// <summary>
        /// Multiplies this sparse matrix by a 2D double array, and returns a new double array.
        /// </summary>
        /// <param name="A">a.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public double[,] multiply(double[,] A)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Multiply matrix to a vector (and vice versa)

        /// <summary>
        /// Multiplies the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="ArithmeticException">Matrix number of columns does not match length of vector.</exception>
        public double[] multiply(IList<double> x)
        {
            var length = x.Count;
            if (length != NumCols)
                throw new ArithmeticException("Matrix number of columns does not match length of vector.");
            var b = new double[length];
            for (int i = 0; i < NumRows; i++)
            {
                var sum = 0.0;
                var cell = RowFirsts[i];
                while (cell != null)
                {
                    sum += cell.Value * x[cell.ColIndex];
                    cell = cell.Right;
                }
                b[i] = sum;
            }
            return b;
        }
        #endregion


        /// <summary>
        /// Adds the specified 2D double array, A to this sparse matrix to create a new
        /// 2D double array.
        /// </summary>
        /// <param name="A">a.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public double[,] add(double[,] A)
        {
            var numRows = A.GetLength(0);
            if (NumRows != numRows) throw new ArithmeticException("Cannot add matrices of different sizes.");
            var numCols = A.GetLength(1);
            if (NumCols != numCols) throw new ArithmeticException("Cannot add matrices of different sizes.");

            var C = (double[,])A.Clone();

            for (var i = 0; i < numRows; i++)
            {
                var cell = RowFirsts[i];
                while (cell != null)
                {
                    C[i, cell.ColIndex] += cell.Value;
                    cell = cell.Right;
                }
            }
            return C;
        }

        /// <summary>
        /// Subtracts the specified 2D double array, A to this sparse matrix to create a new
        /// 2D double array.
        /// </summary>
        /// <param name="A">a.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">
        /// Cannot subtract matrices of different sizes.
        ///  </exception>
        public double[,] subtract(double[,] A)
        {
            var numRows = A.GetLength(0);
            if (NumRows != numRows) throw new ArithmeticException("Cannot subtract matrices of different sizes.");
            var numCols = A.GetLength(1);
            if (NumCols != numCols) throw new ArithmeticException("Cannot subtract matrices of different sizes.");

            var C = (double[,])A.Clone();

            for (var i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                    C[i, j] = -C[i, j];
                var cell = RowFirsts[i];
                while (cell != null)
                {
                    C[i, cell.ColIndex] += cell.Value;
                    cell = cell.Right;
                }
            }
            return C;
        }

        /// <summary>
        /// Adds the specified 2D double array, A to this sparse matrix and writes over
        /// this sparse matrix with the result.
        /// </summary>
        /// <param name="A">a.</param>
        /// <exception cref="System.ArithmeticException">Adding Sparse Matrices can only be accomplished if both are the same size.</exception>
        public void addInPlace(SparseMatrix A)
        {
            if (NumRows != A.NumRows || NumCols != A.NumCols)
                throw new ArithmeticException(
                    "Adding Sparse Matrices can only be accomplished if both are the same size.");

            for (var i = 0; i < NumRows; i++)
            {
                var thisCell = RowFirsts[i];
                var ACell = A.RowFirsts[i];
                while (thisCell != null || ACell != null)
                {
                    if (thisCell == null || (ACell != null && ACell.ColIndex < thisCell.ColIndex))
                    {
                        AddCell(i, ACell.ColIndex, ACell.Value);
                        ACell = ACell.Right;
                    }
                    else if (ACell == null || thisCell.ColIndex < ACell.ColIndex)
                    {
                        thisCell = thisCell.Right;
                    }
                    else //then the two values must be at the same cell
                    {
                        thisCell.Value += ACell.Value;
                        thisCell = thisCell.Right;
                        ACell = ACell.Right;
                    }
                }
            }
        }
        /// <summary>
        /// Subtracts the specified 2D double array, A from this sparse matrix and writes over
        /// this sparse matrix with the result.
        /// </summary>
        /// <param name="A">a.</param>
        /// <exception cref="System.ArithmeticException">Adding Sparse Matrices can only be accomplished if both are the same size.</exception>
        public void subtractInPlace(SparseMatrix A)
        {
            if (NumRows != A.NumRows || NumCols != A.NumCols)
                throw new ArithmeticException(
                    "Adding Sparse Matrices can only be accomplished if both are the same size.");

            for (var i = 0; i < NumRows; i++)
            {
                var thisCell = RowFirsts[i];
                var ACell = A.RowFirsts[i];
                while (thisCell != null || ACell != null)
                {
                    if (thisCell == null || (ACell != null && ACell.ColIndex < thisCell.ColIndex))
                    {
                        AddCell(i, ACell.ColIndex, -ACell.Value);
                        ACell = ACell.Right;
                    }
                    else if (ACell == null || thisCell.ColIndex < ACell.ColIndex)
                    {
                        thisCell = thisCell.Right;
                    }
                    else //then the two values must be at the same cell
                    {
                        thisCell.Value -= ACell.Value;
                        thisCell = thisCell.Right;
                        ACell = ACell.Right;
                    }
                }
            }
        }


        /// <summary>
        /// Adds the specified 2D double array, A to this sparse matrix to create a new
        /// 2D double array.
        /// </summary>
        /// <param name="A">a.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public SparseMatrix add(SparseMatrix A)
        {
            var copy = this.Copy();
            copy.addInPlace(A);
            return copy;
        }

        /// <summary>
        /// Subtracts the specified 2D double array, A from this sparse matrix to create a new
        /// 2D double array.
        /// </summary>
        /// <param name="A">a.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public SparseMatrix subtract(SparseMatrix A)
        {
            var copy = this.Copy();
            copy.subtractInPlace(A);
            return copy;
        }

        /// <summary>
        /// Sums all elements.
        /// </summary>
        /// <returns>System.Double.</returns>
        public double SumAllElements()
        {
            return SumAllRows().Sum();
        }
        /// <summary>
        /// Sums all of the rows.
        /// </summary>
        /// <returns>System.Double[].</returns>
        public double[] SumAllRows()
        {
            var rowSums = new double[NumRows];
            for (int i = 0; i < NumRows; i++)
                rowSums[i] = SumRow(i);
            return rowSums;
        }
        /// <summary>
        /// Sums the values of a specified row.
        /// </summary>
        /// <returns>System.Double[].</returns>
        public double SumRow(int index)
        {
            var sum = 0.0;
            var cell = RowFirsts[index];
            while (cell != null)
            {
                sum += cell.Value;
                cell = cell.Right;
            }
            return sum;
        }
        /// <summary>
        /// Sums the columns.
        /// </summary>
        /// <returns>System.Double[].</returns>
        public double[] SumAllColumns()
        {
            var colSums = new double[NumCols];
            for (int i = 0; i < NumCols; i++)
                colSums[i] = SumColumn(i);
            return colSums;
        }
        /// <summary>
        /// Sums the values of a specified column.
        /// </summary>
        /// <returns>System.Double[].</returns>
        public double SumColumn(int index)
        {
            var sum = 0.0;
            var cell = ColFirsts[index];
            while (cell != null)
            {
                sum += cell.Value;
                cell = cell.Down;
            }
            return sum;
        }


        private SymbolicFactorization symbolicFactorizationMat;
        private double[] D;
        private CompressedColumnStorage FactorizationMatrix;
        private CompressedColumnStorage MatrixInCCS;



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
            if (isGaussSeidelAppropriate(b, out var potentialDiagonals, ref initialGuess))
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
            if (isGaussSeidelAppropriate(bValues, bIndices, out var potentialDiagonals, ref initialGuess))
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
                if (ValuesChanged || TopologyChanged)
                    MatrixInCCS = convertToCCS(this);
                if (TopologyChanged)
                    symbolicFactorizationMat = CSparse.SymbolicAnalysisLDL(MatrixInCCS);
                if (ValuesChanged || TopologyChanged)
                    CSparse.FactorizeLDL(MatrixInCCS, symbolicFactorizationMat, out D, out FactorizationMatrix);
                TopologyChanged = ValuesChanged = false;
                return CSparse.SolveLDL(b, FactorizationMatrix, D, symbolicFactorizationMat.InversePermute);
            }
            else
            {
                var ccs = convertToCCS(this);
                var columnPermutation = ApproximateMinimumDegree.Generate(
                    new SymbolicColumnStorage(ccs), NumCols);
                // Numeric LU factorization
                CSparse.FactorizeLU(ccs, columnPermutation, out var L, out var U, out var pinv);
                var x = CSparse.ApplyInverse(pinv, b, NumCols); // x = b(p)
                CSparse.SolveLower(L, x); // x = L\x.
                CSparse.SolveUpper(U, x); // x = U\x.
                return CSparse.ApplyInverse(columnPermutation, x, NumCols); // b(q) = x
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
            } while (!solutionFound && stack.Count > 0);
            if (solutionFound) return candidate;
            return null;
        }

        #endregion

    }
}
