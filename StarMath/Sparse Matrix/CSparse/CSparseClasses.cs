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
        internal int[] cp; // column pointers for Cholesky, row counts for QR
        internal int[] leftmost; // leftmost[i] = min(find(A(i,:))), for QR
        internal int lnz; // # entries in L for LU or Cholesky; in V for QR
        internal int m2; // # of rows for QR, after adding fictitious rows
        internal int[] parent; // elimination tree for Cholesky and QR
        internal int[] pinv; // inverse row perm. for QR, fill red. perm for Chol
        internal int[] q; // fill-reducing column permutation for LU and QR
        internal int unz; // # entries in U for LU; in R for QR
    }

    internal class CompressedColumnStorage
    {
        internal readonly int ncols;

        internal readonly int NonZerosCount;

        internal readonly int nrows;

        /// <summary>
        ///     Row pointers with last entry equal number of non-zeros (size = RowCount + 1)
        /// </summary>
        internal int[] ColumnPointers;

        /// <summary>
        ///     Column indices (size >= NonZerosCount)
        /// </summary>
        internal int[] RowIndices;

        /// <summary>
        ///     Numerical values (size >= NonZerosCount)
        /// </summary>
        internal double[] Values;

        /// <inheritdoc />
        internal CompressedColumnStorage(int rowCount, int columnCount, int valueCount)
        {
            ColumnPointers = new int[columnCount + 1];
            RowIndices = new int[valueCount];
            nrows = rowCount;
            ncols = columnCount;
            NonZerosCount = valueCount;
            if (valueCount > 0)
            {
                Values = new double[valueCount];
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
        internal readonly int ncols;
        internal readonly int nrows;

        /// <summary>
        ///     Column pointers with last entry equal number of non-zeros (size = ColumnCount + 1)
        /// </summary>
        internal int[] ColumnPointers;

        /// <summary>
        ///     Row indices (size = NonZerosCount)
        /// </summary>
        internal int[] RowIndices;


        internal SymbolicColumnStorage(CompressedColumnStorage mat)
        {
            nrows = mat.nrows;
            ncols = mat.ncols;
            ColumnPointers = mat.ColumnPointers;
            RowIndices = mat.RowIndices;
        }

        internal SymbolicColumnStorage(int rowCount, int columnCount, int valueCount)
        {
            // Explicitly allow m or n = 0 (may occur in Dulmage-Mendelsohn decomposition).
            if (rowCount < 0 || columnCount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            nrows = rowCount;
            ncols = columnCount;

            if (valueCount > 0)
            {
                ColumnPointers = new int[columnCount + 1];
                RowIndices = new int[valueCount];
            }
        }


        /// <summary>
        ///     Gets the number of non-zero entries.
        /// </summary>
        internal int NonZerosCount
        {
            get { return ColumnPointers[ncols]; }
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
            int j, k, p;
            var result = new SymbolicColumnStorage(ncols, nrows, 0);

            var ci = new int[nrows + 1];
            var cj = new int[RowIndices.Length];

            var w = new int[nrows];

            for (p = 0; p < ColumnPointers[ncols]; p++)
            {
                // Row counts.
                w[RowIndices[p]]++;
            }

            // Column pointers.
            CumulativeSum(ci, w, nrows);

            for (j = 0; j < ncols; j++)
            {
                for (p = ColumnPointers[j]; p < ColumnPointers[j + 1]; p++)
                {
                    k = w[RowIndices[p]]++;

                    // Place A(i,j) as entry C(j,i)
                    cj[k] = j;
                }
            }

            result.ColumnPointers = ci;
            result.RowIndices = cj;

            return result;
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
        internal SymbolicColumnStorage Add(SymbolicColumnStorage other)
        {
            int j, nz = 0;

            // check inputs
            if (nrows != other.nrows || ncols != other.ncols)
            {
                throw new ArgumentException();
            }

            var m = nrows;
            var n = other.ncols;

            var bi = other.ColumnPointers;

            var anz = ColumnPointers[ncols];
            var bnz = bi[n];

            // Workspace
            var w = new int[m];

            // Allocate result: (anz + bnz) is an upper bound
            var cp = new int[bi.Length];
            var ci = new int[anz + bnz];

            for (j = 0; j < n; j++)
            {
                // Column j of result starts here
                cp[j] = nz;
                nz = Scatter(j, w, j + 1, ci, nz); // A(:,j)
                nz = other.Scatter(j, w, j + 1, ci, nz); // B(:,j)
            }

            // Finalize the last column
            cp[n] = nz;

            // Remove extra space
            Array.Resize(ref ci, nz);

            var result = new SymbolicColumnStorage(m, n, 0);

            result.ColumnPointers = cp;
            result.RowIndices = ci;

            return result;
        }


        /// <summary>
        ///     Drops entries from a sparse matrix
        /// </summary>
        /// <param name="func">Drop element a_{i,j} if func(i, j) is false.</param>
        /// <returns>New number of entries in A.</returns>
        internal int Keep()
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
            int i, p;

            for (p = ColumnPointers[j]; p < ColumnPointers[j + 1]; p++)
            {
                i = RowIndices[p]; // A(i,j) is nonzero
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