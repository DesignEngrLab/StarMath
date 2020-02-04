// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="make extract.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Make simple Matrices functions       
        /// <summary>
        /// Makes a square matrix of size p by p of all zeros.
        /// </summary>
        /// <param name="p">The size (number of both rows and columns).</param>
        /// <returns>an empty (all zeros) square matrix (2D float).</returns>
        /// <exception cref="System.ArithmeticException">The size, p, must be a positive integer.</exception>
        public static float[,] makeZeroFloat(int p)
        {
            if (p <= 0) throw new ArithmeticException("The size, p, must be a positive integer.");
            return new float[p, p];
        }

        /// <summary>
        /// Makes the zero vector.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">The size, p, must be a positive integer.</exception>
        public static float[] makeZeroVectorFloat(int p)
        {
            if (p <= 0) throw new ArithmeticException("The size, p, must be a positive integer.");
            return new float[p];
        }


        /// <summary>
        /// Makes a matrix of size numRows by numCols of all zeros.
        /// </summary>
        /// <param name="numRows">The number of rows.</param>
        /// <param name="numCols">The number of columns.</param>
        /// <returns>an empty (all zeros) matrix.</returns>
        /// <exception cref="System.ArithmeticException">The number of rows, numRows, must be a positive integer.
        /// or
        /// The number of columns, numCols, must be a positive integer.</exception>
        public static float[,] makeZeroFloat(int numRows, int numCols)
        {
            if (numRows <= 0) throw new ArithmeticException("The number of rows, numRows, must be a positive integer.");
            if (numCols <= 0) throw new ArithmeticException("The number of columns, numCols, must be a positive integer.");
            return new float[numRows, numCols];
        }


        /// <summary>
        /// Makes an identity matrix of size p by p.
        /// </summary>
        /// <param name="p">The size (number of both rows and columns).</param>
        /// <returns>the identity matrix, I.</returns>
        /// <exception cref="System.ArithmeticException">The size, p, must be a positive integer.</exception>
        public static float[,] makeIdentityFloat(int p)
        {
            if (p <= 0) throw new ArithmeticException("The size, p, must be a positive integer.");
            var I = new float[p, p];
            for (var i = 0; i < p; i++)
                I[i, i] = 1.0f;
            return I;
        }

        #endregion

        #region Make Progression

        /// <summary>
        /// Makes a linear progression from start to, but not including, the end.
        /// </summary>
        /// <param name="end">The end value (which will not be reached).</param>
        /// <param name="interval">The interval amount between values.</param>
        /// <param name="start">The starting value (the value of the first element).</param>
        /// <returns>Returns a float array with a series of numbers starting from start until the end
        /// with a distance of the interval between any pair of numbers.</returns>
        public static float[] makeLinearProgression(float end, float interval, float start = 0.0f)
        {
            var NumOfElements = (int)((end - start) / interval);

            var prog = new float[NumOfElements];

            for (var i = 0; i < NumOfElements; i++)
                prog[i] = start + interval * i;

            return prog;
        }

        /// <summary>
        /// Makes a linear progression from start to, but not including, the end.
        /// </summary>
        /// <param name="end">The end value (which will not be reached).</param>
        /// <param name="numElements">The number of elements.</param>
        /// <param name="start">The starting value (the value of the first element).</param>
        /// <returns>Returns a float array with a series of numbers starting from start until the end
        /// with a distance of the interval between any pair of numbers.</returns>
        public static float[] makeLinearProgression(float end, int numElements, float start = 0.0f)
        {
            var prog = new float[numElements];
            var interval = (end - start) / numElements;
            for (var i = 0; i < numElements; i++)
                prog[i] = start + interval * i;
            return prog;
        }

        #endregion


        /// <summary>
        /// Converts the 2D float array to a Sparse matrix.
        /// </summary>
        /// <param name="A">a.</param>
        /// <param name="optionalTolerance">An optional tolerance.</param>
        /// <returns>SparseMatrix.</returns>
        public static SparseMatrix ConvertDenseToSparseMatrix(this float[,] A, double optionalTolerance = DefaultEqualityTolerance)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            List<int> rowIndices = new List<int>();
            List<int> colIndices = new List<int>();
            List<double> values = new List<double>();
            for (int i = 0; i < numRows; i++)
                for (int j = 0; j < numCols; j++)
                {
                    if (!A[i, j].IsNegligible(optionalTolerance))
                    {
                        rowIndices.Add(i);
                        colIndices.Add(j);
                        values.Add(A[i, j]);
                    }
                }
            return new SparseMatrix(rowIndices, colIndices, values, numRows, numCols);
        }

        #region Get/Set/Remove parts of a matrix

        #region Get

        /// <summary>
        /// Gets the column of matrix, A.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="colIndex">The column index.</param>
        /// <returns>A float array that contains the requested column</returns>
        public static float[] GetColumn(this float[,] A, int colIndex)
        {
            return GetColumn(colIndex, A);
        }


        /// <summary>
        /// Gets a column of matrix, A.
        /// </summary>
        /// <param name="colIndex">The column index.</param>
        /// <param name="A">The matrix, A.</param>
        /// <returns>A float array that contains the requested column</returns>
        /// <exception cref="System.ArithmeticException">StarMath Size Error: An index value of 
        ///                                     + colIndex
        ///                                     +  for getColumn is not in required range from 0 up to (but not including) 
        ///                                     + numRows + .</exception>
        public static float[] GetColumn(int colIndex, float[,] A)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if ((colIndex < 0) || (colIndex >= numCols))
                throw new ArithmeticException("StarMath Size Error: An index value of "
                                    + colIndex
                                    + " for getColumn is not in required range from 0 up to (but not including) "
                                    + numRows + ".");
            var v = new float[numRows];
            for (var i = 0; i < numRows; i++)
                v[i] = A[i, colIndex];
            return v;
        }

        /// <summary>
        /// Get more than one column from a given 2D float array.
        /// </summary>
        /// <param name="A">2D float array from which columns need to be extracted</param>
        /// <param name="ColumnList">The column list indices.</param>
        /// <returns>A  2D float array that contains all the requested columns</returns>
        public static float[,] GetColumns(this float[,] A, IList<int> ColumnList)
        {
            return GetColumns(ColumnList, A);
        }

        /// <summary>
        /// Get more than one column from a given 2D float array.
        /// </summary>
        /// <param name="ColumnList">The column list indices.</param>
        /// <param name="A">2D float array from which columns need to be extracted</param>
        /// <returns>A  2D float array that contains all the requested columns</returns>
        public static float[,] GetColumns(IList<int> ColumnList, float[,] A)
        {
            var columns = new float[A.GetLength(0), ColumnList.Count];
            var k = 0;
            foreach (var i in ColumnList)
                SetColumn(k++, columns, GetColumn(i, A));
            return columns;
        }

        /// <summary>
        /// Gets a row of matrix, A.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="rowIndex">The row index.</param>
        /// <returns>A float array that contains the requested row</returns>
        public static float[] GetRow(this float[,] A, int rowIndex)
        {
            return GetRow(rowIndex, A);
        }

        /// <summary>
        /// Gets a row of matrix, A.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="A">The matrix, A.</param>
        /// <returns>A float array that contains the requested row</returns>
        /// <exception cref="System.ArithmeticException">StarMath Size Error: An index value of 
        ///                                     + rowIndex
        ///                                     +  for getRow is not in required range from 0 up to (but not including) 
        ///                                     + numRows + .</exception>
        public static float[] GetRow(int rowIndex, float[,] A)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if ((rowIndex < 0) || (rowIndex >= numRows))
                throw new ArithmeticException("StarMath Size Error: An index value of "
                                    + rowIndex
                                    + " for getRow is not in required range from 0 up to (but not including) "
                                    + numRows + ".");
            var v = new float[numCols];
            for (var i = 0; i < numCols; i++)
                v[i] = A[rowIndex, i];
            return v;
        }


        /// <summary>
        /// Get more than one row from a given 2D float array.
        /// </summary>
        /// <param name="A">2D float array from which rows need to be extracted</param>
        /// <param name="RowList">The row list indices.</param>
        /// <returns>A  2D float array that contains all the requested rows</returns>
        public static float[,] GetRows(this float[,] A, IList<int> RowList)
        {
            return GetRows(RowList, A);
        }


        /// <summary>
        /// Get more than one row from a given 2D float array.
        /// </summary>
        /// <param name="RowList">The row list indices.</param>
        /// <param name="A">2D float array from which rows need to be extracted</param>
        /// <returns>A  2D float array that contains all the requested rows</returns>
        public static float[,] GetRows(IList<int> RowList, float[,] A)
        {
            var Rows = new float[RowList.Count, A.GetLength(1)];
            var k = 0;
            foreach (var i in RowList)
                SetRow(k++, Rows, GetRow(i, A));
            return Rows;
        }

        #endregion

        #region Set

        /// <summary>
        /// Sets/Replaces the given row of matrix A with the vector v.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="rowIndex">The index of the row, rowIndex.</param>
        /// <param name="v">The vector, v.</param>
        public static void SetRow(this float[,] A, int rowIndex, IList<float> v)
        {
            SetRow(rowIndex, A, v);
        }

        /// <summary>
        /// Sets/Replaces the given row of matrix A with the vector v.
        /// </summary>
        /// <param name="rowIndex">The index of the row, rowIndex.</param>
        /// <param name="A">The matrix, A.</param>
        /// <param name="v">The vector, v.</param>
        /// <exception cref="System.ArithmeticException">StarMath Size Error: An index value of 
        ///                                     + rowIndex
        ///                                     +  for getRow is not in required range from 0 up to (but not including) 
        ///                                     + numRows + .</exception>
        public static void SetRow(int rowIndex, float[,] A, IList<float> v)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if ((rowIndex < 0) || (rowIndex >= numRows))
                throw new ArithmeticException("StarMath Size Error: An index value of "
                                    + rowIndex
                                    + " for getRow is not in required range from 0 up to (but not including) "
                                    + numRows + ".");
            for (var i = 0; i < numCols; i++)
                A[rowIndex, i] = v[i];
        }

        /// <summary>
        /// Sets/Replaces the given column of matrix A with the vector v.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="colIndex">The index of the column, rowIndex.</param>
        /// <param name="v">The vector, v.</param>
        public static void SetColumn(this float[,] A, int colIndex, IList<float> v)
        {
            SetColumn(colIndex, A, v);
        }

        /// <summary>
        /// Sets/Replaces the given column of matrix A with the vector v.
        /// </summary>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="A">The A.</param>
        /// <param name="v">The v.</param>
        /// <exception cref="System.ArithmeticException">StarMath Size Error: An index value of 
        ///                                     + colIndex
        ///                                     +  for getColumn is not in required range from 0 up to (but not including) 
        ///                                     + numCols + .</exception>
        public static void SetColumn(int colIndex, float[,] A, IList<float> v)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if ((colIndex < 0) || (colIndex >= numCols))
                throw new ArithmeticException("StarMath Size Error: An index value of "
                                    + colIndex
                                    + " for getColumn is not in required range from 0 up to (but not including) "
                                    + numCols + ".");
            for (var i = 0; i < numRows; i++)
                A[i, colIndex] = v[i];
        }
        #endregion

        #region Remove

        /// <summary>
        /// Removes the given row of matrix A with the vector v.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="rowIndex">The index of the row, rowIndex.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">StarMath Size Error: An index value of 
        ///                                     + rowIndex
        ///                                     +  for RemoveRow is not in required range from 0 up to (but not including) 
        ///                                     + numRows + .</exception>
        public static float[,] RemoveRow(this float[,] A, int rowIndex)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if ((rowIndex < 0) || (rowIndex >= numRows))
                throw new ArithmeticException("StarMath Size Error: An index value of "
                                    + rowIndex
                                    + " for RemoveRow is not in required range from 0 up to (but not including) "
                                    + numRows + ".");
            var B = new float[numRows - 1, numCols];
            for (var i = 0; i < rowIndex; i++)
                B.SetRow(i, A.GetRow(i));
            for (var i = rowIndex + 1; i < numRows; i++)
                B.SetRow(i - 1, A.GetRow(i));
            return B;
        }

        /// <summary>
        /// Removes the given column of matrix A with the vector v.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="colIndex">The index of the column, rowIndex.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">StarMath Size Error: An index value of 
        ///                                     + colIndex
        ///                                     +  for RemoveColumn is not in required range from 0 up to (but not including) 
        ///                                     + numCols + .</exception>
        public static float[,] RemoveColumn(this float[,] A, int colIndex)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if ((colIndex < 0) || (colIndex >= numRows))
                throw new ArithmeticException("StarMath Size Error: An index value of "
                                    + colIndex
                                    + " for RemoveColumn is not in required range from 0 up to (but not including) "
                                    + numCols + ".");
            var B = new float[numRows, numCols - 1];
            for (var i = 0; i < colIndex; i++)
                B.SetColumn(i, A.GetColumn(i));
            for (var i = colIndex + 1; i < numCols; i++)
                B.SetColumn(i - 1, A.GetColumn(i));
            return B;
        }

        /// <summary>
        /// Removes the given row of matrix A with the vector v.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="rowIndices">The row indices.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">
        /// StarMath Size Error: A row index, with value  + rowIndices.Max() +
        ///                                     , in the provided rowIndices for RemoveRows exceeds the number of rows (number of rows =  +
        ///                                     numRows +
        ///                                     ) in the provided matrix.
        /// or
        /// StarMath Size Error: An there are more rows to remove (rowIndices.Count =  +
        ///                                     rowIndices.Count +
        ///                                     ) than there are rows in the matrix provided to RemoveRows (number of rows =  +
        ///                                     numRows + ).
        /// </exception>
        public static float[,] RemoveRows(this float[,] A, IList<int> rowIndices)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            var numToRemove = rowIndices.Count;
            if (rowIndices.Max() >= numRows)
                throw new ArithmeticException("StarMath Size Error: A row index, with value " + rowIndices.Max() +
                                    ", in the provided rowIndices for RemoveRows exceeds the number of rows (number of rows = " +
                                    numRows +
                                    ") in the provided matrix.");
            if (rowIndices.Count >= numRows)
                throw new ArithmeticException("StarMath Size Error: An there are more rows to remove (rowIndices.Count = " +
                                    rowIndices.Count +
                                    ") than there are rows in the matrix provided to RemoveRows (number of rows = " +
                                    numRows + ").");
            var sortedRowIndices = rowIndices.OrderBy(x => x).ToArray();
            var B = new float[numRows - numToRemove, numCols];

            var k = 0; //rowIndices position
            for (var i = 0; i < numRows; i++)
                if (k < numToRemove && sortedRowIndices[k] == i) k++;
                else B.SetRow(i - k, A.GetRow(i));
            return B;
        }

        /// <summary>
        /// Removes the given columns of matrix A.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="colIndices">The col indices.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">
        /// StarMath Size Error: A row index, with value  + colIndices.Max() +
        ///                                     , in the provided rowIndices for RemoveColumns exceeds the number of rows (number of rows =  +
        ///                                     numCols +
        ///                                     ) in the provided matrix.
        /// or
        /// StarMath Size Error: An there are more rows to remove (rowIndices.Count = 
        ///                                     + colIndices.Count +
        ///                                     ) than there are rows in the matrix provided to RemoveColumns (number of rows =  +
        ///                                     numCols + ).
        /// </exception>
        /// <exception cref="ArithmeticException">StarMath Size Error: A row index, with value  + colIndices.Max() +
        /// , in the provided rowIndices for RemoveColumns exceeds the number of rows (number of rows =  + numCols +
        /// ) in the provided matrix.
        /// or
        /// StarMath Size Error: An there are more rows to remove (rowIndices.Count =
        /// + colIndices.Count +
        /// ) than there are rows in the matrix provided to RemoveColumns (number of rows =  + numCols + ).</exception>
        public static float[,] RemoveColumns(this float[,] A, IList<int> colIndices)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            var numToRemove = colIndices.Count;
            if (colIndices.Max() >= numCols)
                throw new ArithmeticException("StarMath Size Error: A row index, with value " + colIndices.Max() +
                                    ", in the provided rowIndices for RemoveColumns exceeds the number of rows (number of rows = " +
                                    numCols +
                                    ") in the provided matrix.");
            if (colIndices.Count >= numCols)
                throw new ArithmeticException("StarMath Size Error: An there are more rows to remove (rowIndices.Count = "
                                    + colIndices.Count +
                                    ") than there are rows in the matrix provided to RemoveColumns (number of rows = " +
                                    numCols + ").");
            var B = new float[numRows, numCols - colIndices.Count];
         var sortedColIndices = colIndices.OrderBy(x => x).ToArray();
           var k = 0; //colIndices position
            for (var i = 0; i < numCols; i++)
                if (k < numToRemove && sortedColIndices[k] == i) k++;
                else B.SetColumn(i - k, A.GetColumn(i));
            return B;
        }

        #endregion

        /// <summary>
        /// Get some portion of a vector and put in a new vector.
        /// </summary>
        /// <param name="A">1D float array from which elements need to be extracted</param>
        /// <param name="indexList">The indices of the elements.</param>
        /// <returns>A single 1D float array that contains all the requested elements.</returns>
        public static float[] GetPartialVector(this IList<float> A, IList<int> indexList)
        {
            var result = new float[indexList.Count];
            for (var i = 0; i < indexList.Count; i++)
                result[i] = A[indexList[i]];
            return result;
        }

        /// <summary>
        /// Removes the given entry of vector A.
        /// </summary>
        /// <param name="A">The vector, A.</param>
        /// <param name="index">The index to remove.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">StarMath Size Error: An index value of 
        ///                                     + index
        ///                                     +  for RemoveVectorCell is not in required range from 0 up to (but not including) 
        ///                                     + length + .</exception>
        /// <exception cref="ArithmeticException">StarMath Size Error: An index value of
        /// + index
        /// +  for RemoveVectorCell is not in required range from 0 up to (but not including)
        /// + length + .</exception>
        public static float[] RemoveVectorCell(this IList<float> A, int index)
        {
            var length = A.Count;
            if ((index < 0) || (index >= length))
                throw new ArithmeticException("StarMath Size Error: An index value of "
                                    + index
                                    + " for RemoveVectorCell is not in required range from 0 up to (but not including) "
                                    + length + ".");
            var B = new List<float>(A);
            B.RemoveAt(index);
            return B.ToArray();
        }
        /// <summary>
        /// Removes the given entry of vector A.
        /// </summary>
        /// <param name="A">The vector, A.</param>
        /// <param name="indices">The indices.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">
        /// StarMath Size Error: A cell index, with value  + indices.Max() +
        ///                                     , in the provided indices for RemoveVectorCells exceeds the number of entries (size =  +
        ///                                     length +
        ///                                     ) in the provided vector.
        /// or
        /// StarMath Size Error: There are more cells to remove (Count =  + indices.Count +
        ///                                     ) than there are cells in the vector provided to RemoveVectorCells (Count =  +
        ///                                     length + ).
        /// </exception>
        /// <exception cref="ArithmeticException">StarMath Size Error: An index value of
        /// + index
        /// +  for RemoveVectorCell is not in required range from 0 up to (but not including)
        /// + length + .</exception>
        public static float[] RemoveVectorCells(this IList<float> A, IList<int> indices)
        {
            var length = A.Count;
            var numToRemove = indices.Count;
            if (indices.Max() >= length)
                throw new ArithmeticException("StarMath Size Error: A cell index, with value " + indices.Max() +
                                    ", in the provided indices for RemoveVectorCells exceeds the number of entries (size = " +
                                    length +
                                    ") in the provided vector.");
            if (indices.Count >= length)
                throw new ArithmeticException("StarMath Size Error: There are more cells to remove (Count = " + indices.Count +
                                    ") than there are cells in the vector provided to RemoveVectorCells (Count = " +
                                    length + ").");
            var B = new float[length - numToRemove];
            var sortedRowIndices = indices.OrderBy(x => x).ToArray();
            var k = 0; //rowIndices position
            for (var i = 0; i < length; i++)
                if (k < numToRemove && sortedRowIndices[k] == i) k++;
                else B[i - k] = A[i];
            return B;
        }


        #endregion

        #region Join Matrices into taller/fatter matrix

        /// <summary>
        /// Jions two 2D float arrays side by side and returns the results. The given variables remain unchanged
        /// </summary>
        /// <param name="Matrix1">The Matrix that comes on the left.</param>
        /// <param name="Matrix2">Matrix that is attached to the right</param>
        /// <returns>A 2D float array that has Matrix1 and Matrix2 side by side</returns>
        /// <exception cref="System.ArithmeticException">StarMath Size Error: Row dimensions do not match for matrix1 and matrix2</exception>
        public static float[,] JoinCol(this float[,] Matrix1, float[,] Matrix2)
        {
            if (Matrix1.GetLength(0) != Matrix2.GetLength(0))
                throw new ArithmeticException("StarMath Size Error: Row dimensions do not match for matrix1 and matrix2");
            var NumRows = Matrix1.GetLength(0);
            var NumCols = Matrix1.GetLength(1) + Matrix2.GetLength(1);
            var Mat1Cols = Matrix1.GetLength(1);
            var Mat2Cols = Matrix2.GetLength(1);

            var JointMatrix = new float[NumRows, NumCols];

            for (var j = 0; j < Mat1Cols; j++)
                for (var k = 0; k < NumRows; k++)
                    JointMatrix[k, j] = Matrix1[k, j];


            for (var j = 0; j < Mat2Cols; j++)
                for (var k = 0; k < NumRows; k++)
                    JointMatrix[k, j + Mat1Cols] = Matrix2[k, j];

            return JointMatrix;
        }

        /// <summary>
        /// Joins two 2D float arrays one under the other and returns the results. The given variables remain unchanged
        /// </summary>
        /// <param name="Matrix1">The Matrix that comes on the top.</param>
        /// <param name="Matrix2">Matrix that is attached to the bottom</param>
        /// <returns>A 2D float array that has Matrix1 and Matrix2 one below the other</returns>
        /// <exception cref="System.ArithmeticException">StarMath Size Error: Column dimensions do not match for matrix1 and matrix2</exception>
        public static float[,] JoinRow(this float[,] Matrix1, float[,] Matrix2)
        {
            if (Matrix1.GetLength(1) != Matrix2.GetLength(1))
                throw new ArithmeticException("StarMath Size Error: Column dimensions do not match for matrix1 and matrix2");
            var numRows = Matrix1.GetLength(0) + Matrix2.GetLength(0);
            var numCols = Matrix1.GetLength(1);
            var mat1Rows = Matrix1.GetLength(0);
            var mat2Rows = Matrix2.GetLength(0);
            var JointMatrix = new float[numRows, numCols];

            for (var j = 0; j < mat1Rows; j++)
                for (var k = 0; k < numCols; k++)
                    JointMatrix[j, k] = Matrix1[j, k];

            for (var j = 0; j < mat2Rows; j++)
                for (var k = 0; k < numCols; k++)
                    JointMatrix[j + mat1Rows, k] = Matrix2[j, k];

            return JointMatrix;
        }
        #endregion

        #region Join Vectors into one long Vector

        /// <summary>
        /// Concatenates two 1D float arrays and returns the result. The given variables remain unchanged
        /// </summary>
        /// <param name="Array1">Array that comes first.</param>
        /// <param name="Array2">Array that is appended to the end of the first array</param>
        /// <returns>An float array that has Array1 and Array2 side by side</returns>
        public static float[] JoinVectors(this IList<float> Array1, IList<float> Array2)
        {
            var Array1Length = Array1.Count;
            var Array2Length = Array2.Count;
            var JointArray = new float[Array1Length + Array2Length];

            for (var j = 0; j < Array1Length; j++)
                JointArray[j] = Array1[j];

            for (var j = 0; j < Array2Length; j++)
                JointArray[j + Array1Length] = Array2[j];
            return JointArray;
        }
        #endregion

        #region Join/Flatten Matrix into one long vector

        /// <summary>
        /// Joins the matrix columns into vector.
        /// </summary>
        /// <param name="A">The matrix of doubles, A.</param>
        /// <returns>System.Double[].</returns>
        public static float[] JoinMatrixColumnsIntoVector(this float[,] A)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            var B = new float[numRows * numCols];
            for (var i = 0; i < numCols; i++)
                GetColumn(i, A).CopyTo(B, i * numRows);
            return B;
        }

        /// <summary>
        /// Joins the matrix rows into vector.
        /// </summary>
        /// <param name="A">The matrix of doubles, A.</param>
        /// <returns>System.Double[].</returns>
        public static float[] JoinMatrixRowsIntoVector(this float[,] A)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            var B = new float[numRows * numCols];
            for (var i = 0; i < numRows; i++)
                GetRow(i, A).CopyTo(B, i * numCols);
            return B;
        }
        #endregion

        #region Distinct

        /// <summary>
        /// Removes any duplicates in the List of vectors and returns just the distinct cases.
        /// The order is preserved with duplicates removed.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns>List&lt;System.Double[]&gt;.</returns>
        public static List<float[]> DistinctVectors(this List<float[]> list)
        {
            var distinctList = new List<float[]>(list);
            var m = list.Count;
            var n = list[0].GetLength(0);
            var CarolNumbers = new float[n];
            var CarolSeed = StartingCarolSeed;
            for (var i = 0; i < n; i++)
            {
                var carolNumber = MathF.Pow(2, CarolSeed) - 1;
                carolNumber *= carolNumber;
                carolNumber -= 2;
                CarolNumbers[i] = carolNumber;
                CarolSeed += 3;
            }
            var indices = makeLinearProgression(m);
            indices = indices.OrderBy(index => list[index].dotProduct(CarolNumbers)).ToArray();
            for (var i = m - 1; i > 0; i--)
            {
                if (IsPracticallySame(list[indices[i]], list[indices[i - 1]]))
                    distinctList[indices[i]] = null;
            }
            distinctList.RemoveAll(v => v == null);
            return distinctList;
        }

        #endregion
    }
}