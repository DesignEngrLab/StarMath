using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarMathLib
{
    public class SparseMatrix
    {
        SparseCell[] RowFirsts;
        SparseCell[] RowLasts;
        SparseCell[] ColFirsts;
        SparseCell[] ColLasts;
        public int NumNonZero;

        public double this[int rowI, int colI]
        {
            get
            {
                if (rowI >= colI)
                    return SearchDownTo(colI, RowFirsts[rowI]).Value;
                else
                    return SearchLeftTo(rowI, ColFirsts[colI]).Value;
            }
            set
            {
                if (rowI >= colI)
                    SearchDownTo(colI, RowFirsts[rowI]).Value = value;
                else SearchLeftTo(rowI, ColFirsts[colI]).Value = value;
            }
        }

        SparseCell SearchLeftTo(int colIndex, SparseCell startCell)
        {
            while (startCell.ColIndex != colIndex)
                startCell = startCell.Left;
            return startCell;
        }
        SparseCell SearchDownTo(int rowIndex, SparseCell startCell)
        {
            while (startCell.RowIndex != rowIndex)
                startCell = startCell.Down;
            return startCell;
        }
        public SparseMatrix(IList<int> rowIndices, IList<int> colIndices, IList<double> values, int numRows, int numCols)
        {
            NumNonZero = values.Count;
            RowFirsts = new SparseCell[numRows];
            RowLasts = new SparseCell[numRows];
            ColFirsts = new SparseCell[numCols];
            ColLasts = new SparseCell[numCols];

            for (int i = 0; i < NumNonZero; i++)
            {
                var rowI = rowIndices[i];
                var colI = colIndices[i];
                var cell = new SparseCell(rowI, colI, values[i]);
                if (RowFirsts[rowI] == null) RowFirsts[rowI] = cell;
                else if (RowFirsts[rowI].ColIndex > colI)
                {
                    cell.Left = RowFirsts[rowI];
                    RowFirsts[rowI].Right = cell;
                    RowFirsts[rowI] = cell;
                }
                if (RowLasts[rowI] == null) RowFirsts[rowI] = cell;
                else if (RowLasts[rowI].ColIndex < colI)
                {
                    cell.Right = RowLasts[rowI];
                    RowLasts[rowI].Left = cell;
                    RowLasts[rowI] = cell;
                }

            }
        }


    }

        internal class SparseCell
        {
            internal int RowIndex;
            internal int ColIndex;
            internal double Value;
            internal SparseCell Left;
            internal SparseCell Right;
            internal SparseCell Up;
            internal SparseCell Down;

            public SparseCell(int rowIndex, int colIndex, double value)
            {
                RowIndex = rowIndex;
                ColIndex = colIndex;
                Value = value;
            }
        }
    }

