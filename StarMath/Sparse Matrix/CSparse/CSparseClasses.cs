// ***********************************************************************
// Assembly         : StarMath
// Author           : Matthew I. Campbell
// Created          : 02-28-2016
// Modified from    : Timothy A. Davis, 2006-2014
// Last Modified By : Matt
// Last Modified On : 02-28-2016
// ***********************************************************************
// <copyright file="CSparseClasses.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace StarMathLib.CSparse
{
    internal class SymbolicFactorization
    {
        internal int[] ColumnPointers; // column pointers for Cholesky
        internal int NumNonZeroInLower; // # entries in L for LU or Cholesky
        internal int[] ParentIndices; // elimination tree for Cholesky 
        internal int[] InversePermute; // inverse row perm. for Chol
        internal int[] ColumnPermutation; // fill-reducing column permutation for LU 
        internal int NumNonZeroInUpper; // # entries in U for LU
    }

    internal class CompressedColumnStorage
    {
        internal readonly int ncols;
        internal readonly int nrows;
        internal int[] ColumnPointers;
        internal int[] RowIndices;
        internal double[] Values;
        
        internal CompressedColumnStorage(int rowCount, int columnCount, int numNonZero)
        {
            ColumnPointers = new int[columnCount + 1];
            RowIndices = new int[numNonZero];
            nrows = rowCount;
            ncols = columnCount;
            if (numNonZero > 0)
            {
                Values = new double[numNonZero];
            }
        }

        /// <summary>
        ///     Change the max # of entries sparse matrix
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal bool Resize(int size)
        {
            if (size <= 0)
            {
                size = ColumnPointers[ncols];
            }
            Array.Resize(ref RowIndices, size);
            Array.Resize(ref Values, size);

            return true;
        }
    }

    /// <summary>
    ///     Represents the nonzero pattern of a column-compressed matrix.
    /// </summary>
    /// <remarks>
    ///     Used for ordering and symbolic factorization.
    /// </remarks>
    internal class SymbolicColumnStorage
    {
        private readonly int ncols;
        private readonly int nrows;
        internal int[] ColumnPointers;
        internal int[] RowIndices;

        private SymbolicColumnStorage(CompressedColumnStorage mat)
        {
            nrows = mat.nrows;
            ncols = mat.ncols;
            ColumnPointers = mat.ColumnPointers;
            RowIndices = mat.RowIndices;
        }
        
        public SymbolicColumnStorage(int ncols, int nrows, int[] colPointers, int[] rowIndices)
        {
            this.ncols = ncols;
            this.nrows = nrows;
            ColumnPointers = colPointers;
            RowIndices = rowIndices;
        }

        /// <summary>
        ///     Change the max # of entries sparse matrix
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal bool Resize(int size)
        {
            if (size <= 0)
            {
                size = ColumnPointers[ncols];
            }

            Array.Resize(ref RowIndices, size);

            return true;
        }

        /// <summary>
        ///     Computes the transpose of a sparse matrix, C = A';
        /// </summary>
        /// <returns>Transposed matrix, C = A'</returns>
        internal SymbolicColumnStorage Transpose()
        {
            var colPointers = new int[nrows + 1];
            var rowIndices = new int[RowIndices.Length];
            var workspace = new int[nrows];

            for (int p = 0; p < ColumnPointers[ncols]; p++)
            {
                // Row counts.
                workspace[RowIndices[p]]++;
            }
            // Column pointers.
            CumulativeSum(colPointers, workspace, nrows);

            for (int j = 0; j < ncols; j++)
            {
                for (int p = ColumnPointers[j]; p < ColumnPointers[j + 1]; p++)
                {
                  int  k = workspace[RowIndices[p]]++;
                    // Place A(i,j) as entry C(j,i)
                    rowIndices[k] = j;
                }
            }
            return new SymbolicColumnStorage(ncols, nrows, colPointers, rowIndices);
        }

        /// <summary>
        ///     Cumulative sum of given array.
        /// </summary>
        /// <param name="sum">Output: cumulative sum of counts</param>
        /// <param name="counts">input array, overwritten with sum</param>
        /// <param name="size">length of counts</param>
        /// <returns>sum[size] (non-zeros)</returns>
        private static int CumulativeSum(int[] sum, int[] counts, int size)
        {
            int i, nz = 0;

            for (i = 0; i < size; i++)
            {
                sum[i] = nz;
                nz += counts[i];
                counts[i] = sum[i]; // also copy p[0..n-1] back into c[0..n-1]
            }

            sum[size] = nz;

            return nz;
        }

        #region for AMD Generate (ConstructMatrix uses subsequent 3 functions)

        internal static SymbolicColumnStorage ConstructMatrix(CompressedColumnStorage a)
        {
            var A = new SymbolicColumnStorage(a);
            // Compute A'
            var AT = A.Transpose();
            // Return A+A'
            var result = A.Add(AT);
            // Drop diagonal entries.
            result.Keep();

            return result;
        }

        /// <summary>
        ///     Symbolic sum C = A + B
        /// </summary>
        /// <param name="other">column-compressed matrix</param>
        /// <returns>Sum C = A + B</returns>
        private SymbolicColumnStorage Add(SymbolicColumnStorage other)
        {
            var workspace = new int[this.nrows];
            // Allocate result: (anz + bnz) is an upper bound
            var columnPointers = new int[other.ColumnPointers.Length];
            var rowIndices = new int[ColumnPointers[ncols] + other.ColumnPointers[other.ncols]];
            int nz = 0;
            for (int j = 0; j < other.ncols; j++)
            {
                // Column j of result starts here
                columnPointers[j] = nz;
                nz = Scatter(j, workspace, j + 1, rowIndices, nz); // A(:,j)
                nz = other.Scatter(j, workspace, j + 1, rowIndices, nz); // B(:,j)
            }
            // Finalize the last column
            columnPointers[other.ncols] = nz;

            // Remove extra space
            Array.Resize(ref rowIndices, nz);

            return new SymbolicColumnStorage(this.nrows, other.ncols, columnPointers,
                rowIndices);
        }


        private int Keep()
        {
            int i, j, nz = 0;

            for (j = 0; j < ncols; j++)
            {
                i = ColumnPointers[j];

                // Record new location of col j.
                ColumnPointers[j] = nz;

                for (; i < ColumnPointers[j + 1]; i++)
                {
                    if (RowIndices[i] != j) //keep off diag
                    {
                        // Keep A(i,j).
                        RowIndices[nz] = RowIndices[i];
                        nz++;
                    }
                }
            }

            // Record new nonzero count.
            ColumnPointers[ncols] = nz;

            // Remove extra space.
            Array.Resize(ref RowIndices, nz);

            return nz;
        }

        /// <summary>
        ///     Scatters and sums a sparse vector A(:,j) into a dense vector, x = x + beta * A(:,j).
        /// </summary>
        /// <param name="j">the column of A to use</param>
        /// <param name="work">size m, node i is marked if w[i] = mark</param>
        /// <param name="mark">mark value of w</param>
        /// <param name="ci">pattern of x accumulated in ci</param>
        /// <param name="nz">pattern of x placed in C starting at C.i[nz]</param>
        /// <returns>new value of nz</returns>
        private int Scatter(int j, int[] work, int mark, int[] ci, int nz)
        {
             for (int p = ColumnPointers[j]; p < ColumnPointers[j + 1]; p++)
            {
              int  i = RowIndices[p]; // A(i,j) is nonzero
                if (work[i] < mark)
                {
                    work[i] = mark; // i is new entry in column j
                    ci[nz++] = i; // add i to pattern of C(:,j)
                }
            }

            return nz;
        }

        #endregion
    }
}