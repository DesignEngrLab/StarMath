/*************************************************************************
 *     This file & class is part of the StarMath Project
 *     Copyright 2010 Matthew Ira Campbell, PhD.
 *
 *     StarMath is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *  
 *     StarMath is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 *  
 *     You should have received a copy of the GNU General Public License
 *     along with StarMath.  If not, see <http://www.gnu.org/licenses/>.
 *     
 *     Please find further details and contact information on GraphSynth
 *     at http://starmath.codeplex.com/.
 *************************************************************************/
using System;

namespace StarMathLib
{
    /// <summary>
    /// The one and only class in the StarMathLib. All functions are static
    /// functions located here.
    /// </summary>
    public static partial class StarMath
    {
        #region Make simple Matrices functions
        /// <summary>
        /// Makes a sqare matrix of size p by p of all zeroes.
        /// </summary>
        /// <param name="p">The size (number of both rows and columns).</param>
        /// <returns>an empty (all zeroes) square matrix (2D double).</returns>
        public static double[,] makeZero(int p)
        {
            if (p <= 0) throw new Exception("The size, p, must be a positive integer.");
            double[,] I = new double[p, p];
            I.Initialize();
            return I;
        }
        /// <summary>
        /// Makes a sqare matrix of size p by p of all zeroes.
        /// </summary>
        /// <param name="p">The size (number of both rows and columns).</param>
        /// <returns>an empty (all zeroes) square matrix (2D int).</returns>
        public static int[,] makeZeroInt(int p)
        {
            if (p <= 0) throw new Exception("The size, p, must be a positive integer.");
            int[,] I = new int[p, p];
            I.Initialize();
            return I;
        }
        /// <summary>
        /// Makes a matrix of size numRows by numCols of all zeroes.
        /// </summary>
        /// <param name="numRows">The number of rows.</param>
        /// <param name="numCols">The number of columns.</param>
        /// <returns>an empty (all zeroes) matrix.</returns>
        public static double[,] makeZero(int numRows, int numCols)
        {
            if (numRows <= 0) throw new Exception("The number of rows, numRows, must be a positive integer.");
            else if (numCols <= 0) throw new Exception("The number of columns, numCols, must be a positive integer.");
            double[,] I = new double[numRows, numCols];
            I.Initialize();
            return I;
        }
        /// <summary>
        /// Makes an identity matrix of size p by p.
        /// </summary>
        /// <param name="p">The size (number of both rows and columns).</param>
        /// <returns>the identity matrix, I.</returns>
        public static double[,] makeIdentity(int p)
        {
            if (p <= 0) throw new Exception("The size, p, must be a positive integer.");
            double[,] I = new double[p, p];
            I.Initialize();
            for (int i = 0; i != p; i++)
                I[i, i] = 1.0;
            return I;
        }
        #endregion

        #region Make Progression
        /// <summary>
        /// Makes the AP (Type double).
        /// </summary>
        /// <param name="Start">The start.</param>
        /// <param name="CommonDifference">The common difference.</param>
        /// <param name="End">The end.</param>
        /// <returns>Returns an double array with a series of numbers starting from Start till End with a distance of CommonDifference between two numbers</returns>
        public static double[] makeAP(double Start, double CommonDifference, double End)
        {

            int NumOfElements = (int)((End - Start) / CommonDifference);

            double[] AP = new double[NumOfElements];

            for (int i = 0; i != NumOfElements; i++)
                AP[i] = Start + CommonDifference * i;

            return AP;
        }
        /// <summary>
        /// Makes the AP (Variable type int).
        /// </summary>
        /// <param name="Start">The start.</param>
        /// <param name="CommonDifference">The common difference.</param>
        /// <param name="End">The end.</param>
        /// <returns>Returns an double array with a series of numbers starting from Start till End with a distance of CommonDifference between two numbers</returns>
        public static int[] makeAP(int Start, int CommonDifference, int End)
        {

            int NumOfElements = (End - Start) / CommonDifference;

            int[] AP = new int[NumOfElements];

            for (int i = 0; i != NumOfElements; i++)
                AP[i] = Start + CommonDifference * i;

            return AP;
        }
        #endregion

        #region Get/Set parts of a matrix
        /// <summary>
        /// Gets a column of matrix, A.
        /// </summary>
        /// <param name="colIndex">The column index.</param>
        /// <param name="A">The matrix, A.</param>
        /// <returns>A double array that contains the requested column</returns>
        public static double[] GetColumn(int colIndex, double[,] A)
        {
            int n = A.GetLength(1);
            if ((colIndex < 0) || (colIndex >= n))
                throw new Exception("MatrixMath Size Error: An index value of "
                    + colIndex
                    + " for getColumn is not in required range from 0 up to (but not including) "
                    + n + ".");
            double[] v = new double[n];
            for (int i = 0; i < n; i++)
                v[i] = A[i, colIndex];
            return v;
        }

        /// <summary>
        /// Gets a row of matrix, A.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="A">The matrix, A.</param>
        /// <returns>A double array that contains the requested row</returns>
        public static double[] GetRow(int rowIndex, double[,] A)
        {
            int n_rows = A.GetLength(0);
            int n_cols = A.GetLength(1);
            if ((rowIndex < 0) || (rowIndex >= n_rows))
                throw new Exception("MatrixMath Size Error: An index value of "
                    + rowIndex
                    + " for getRow is not in required range from 0 up to (but not including) "
                    + n_rows + ".");
            double[] v = new double[n_cols];
            for (int i = 0; i < n_cols; i++)
                v[i] = A[rowIndex, i];
            return v;
        }

        /// <summary>
        /// Sets/Replaces the given row of matrix A with the vector v.
        /// </summary>
        /// <param name="rowIndex">The index of the row, rowIndex.</param>
        /// <param name="A">The matrix, A.</param>
        /// <param name="v">The vector, v.</param>
        public static void SetRow(int rowIndex, double[,] A, double[] v)
        {
            int n = A.GetLength(0);
            if ((rowIndex < 0) || (rowIndex >= n))
                throw new Exception("MatrixMath Size Error: An index value of "
                    + rowIndex
                    + " for getRow is not in required range from 0 up to (but not including) "
                    + n + ".");
            for (int i = 0; i < n; i++)
                A[rowIndex, i] = v[i];
        }
        /// <summary>
        /// Sets/Replaces the given column of matrix A with the vector v.
        /// </summary>
        /// <param name="colIndex">Index of the col.</param>
        /// <param name="A">The A.</param>
        /// <param name="v">The v.</param>
        public static void SetColumn(int colIndex, double[,] A, double[] v)
        {
            int n = A.GetLength(1);
            if ((colIndex < 0) || (colIndex >= n))
                throw new Exception("MatrixMath Size Error: An index value of "
                    + colIndex
                    + " for getColumn is not in required range from 0 up to (but not including) "
                    + n + ".");
            for (int i = 0; i < n; i++)
                A[i, colIndex] = v[i];
        }

        /// <summary>
        /// Get more than one column from a given 2D double array. Invalid column indices numbers are ignored
        /// </summary>
        /// <param name="ColumnList">The column list indices.</param>
        /// <param name="A">2D double array from which columns need to be extracted</param>
        /// <returns>A single 2D double array that contains all the requested columns</returns>
        public static double[,] GetColumns(int[] ColumnList, double[,] A)
        {
            int NumberOfValidColumns = 0;
            int[] ValidColumns = new int[ColumnList.GetLength(0)];
            for (int k = 0; k < ColumnList.GetLength(0); k++)
            {
                if (ColumnList[k] >= 0 && ColumnList[k] < A.GetLength(1))
                {
                    ValidColumns[NumberOfValidColumns] = ColumnList[k];
                    NumberOfValidColumns++;
                }
            }
            double[,] Columns = new double[A.GetLength(0), NumberOfValidColumns];

            for (int k = 0; k < NumberOfValidColumns; k++)
            {
                for (int j = 0; j < A.GetLength(0); j++)
                {
                    Columns[j, k] = A[j, ValidColumns[k]];
                }
            }
            return Columns;
        }
        /// <summary>
        /// Generalization (overload) of the GetColumns function to be extended to a single row array. Invalid Column indices are ignored
        /// </summary>
        /// <param name="ColumnList">The column list indices.</param>
        /// <param name="A">1D double array from which columns (elements) need to be extracted</param>
        /// <returns>A single 1D double array that contains all the requested columns (elements)</returns>
        public static double[] GetColumns(int[] ColumnList, double[] A)
        {
            int NumberOfValidColumns = 0;
            int[] ValidColumns = new int[ColumnList.GetLength(0)];
            for (int k = 0; k < ColumnList.GetLength(0); k++)
            {
                if (ColumnList[k] >= 0 && ColumnList[k] < A.GetLength(0))
                {
                    ValidColumns[NumberOfValidColumns] = ColumnList[k];
                    NumberOfValidColumns++;
                }
            }
            double[] Columns = new double[NumberOfValidColumns];

            for (int k = 0; k < NumberOfValidColumns; k++)
            {
                Columns[k] = A[ValidColumns[k]];
            }
            return Columns;
        }
        /// <summary>
        /// Generalization (overload) of the GetColumns function to be extended to a single row array for integers
        /// </summary>
        /// <param name="ColumnList">The column list indices.</param>
        /// <param name="A">1D integer array from which columns (elements) need to be extracted</param>
        /// <returns>A single 1D integer array that contains all the requested columns (elements)</returns>
        public static int[] GetColumns(int[] ColumnList, int[] A)
        {
            int NumberOfValidColumns = 0;
            int[] ValidColumns = new int[ColumnList.GetLength(0)];
            for (int k = 0; k < ColumnList.GetLength(0); k++)
            {
                if (ColumnList[k] >= 0 && ColumnList[k] < A.GetLength(0))
                {
                    ValidColumns[NumberOfValidColumns] = ColumnList[k];
                    NumberOfValidColumns++;
                }
            }
            int[] Columns = new int[NumberOfValidColumns];

            for (int k = 0; k < NumberOfValidColumns; k++)
            {
                Columns[k] = A[ValidColumns[k]];
            }
            return Columns;
        }

        /// <summary>
        /// Get more than one row from a given 2D double array.  Invalid row indices are ignored
        /// </summary>
        /// <param name="RowList">The row list indices.</param>
        /// <param name="A">2D double array from which rows need to be extracted</param>
        /// <returns>A single 2D double array that contains all the requested rows</returns>
        public static double[,] GetRows(int[] RowList, double[,] A)
        {
            int NumberOfValidRows = 0;
            int[] ValidRows = new int[RowList.GetLength(0)];
            for (int k = 0; k < RowList.GetLength(0); k++)
            {
                if (RowList[k] >= 0 && RowList[k] < A.GetLength(0))
                {
                    ValidRows[NumberOfValidRows] = RowList[k];
                    NumberOfValidRows++;
                }
            }
            double[,] Rows = new double[NumberOfValidRows, A.GetLength(1)];

            for (int k = 0; k < NumberOfValidRows; k++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    Rows[k, j] = A[ValidRows[k], j];
                }
            }
            return Rows;
        }
        /// <summary>
        /// Jions two 2D double arrays side by side and returns the results. The given variables remain unchanged
        /// </summary>
        /// <param name="Matrix1">The Matrix that comes on the left.</param>
        /// <param name="Matrix2">Matrix that is attached to the right</param>
        /// <returns>A 2D double array that has Matrix1 and Matrix2 side by side</returns>
        public static double[,] JoinCol(double[,] Matrix1, double[,] Matrix2)
        {
            if (Matrix1.GetLength(0) != Matrix2.GetLength(0))
                throw new Exception("MatrixMath Size Error: Row dimensions do not match for matrix1 and matrix2");
            int NumRows = Matrix1.GetLength(0);
            int NumCols = Matrix1.GetLength(1) + Matrix2.GetLength(1);
            int Mat1Cols = Matrix1.GetLength(1);
            int Mat2Cols = Matrix2.GetLength(1);

            double[,] JointMatrix = new double[NumRows, NumCols];

            for (int j = 0; j < Mat1Cols; j++)
            {
                for (int k = 0; k < NumRows; k++)
                {
                    JointMatrix[k, j] = Matrix1[k, j];
                }
            }

            for (int j = 0; j < Mat2Cols; j++)
            {
                for (int k = 0; k < NumRows; k++)
                {
                    JointMatrix[k, j + Mat1Cols] = Matrix2[k, j];
                }
            }
            return JointMatrix;
        }
        /// <summary>
        /// Jions two 2D double arrays one under the other and returns the results. The given variables remain unchanged
        /// </summary>
        /// <param name="Matrix1">The Matrix that comes on the top.</param>
        /// <param name="Matrix2">Matrix that is attached to the bottom</param>
        /// <returns>A 2D double array that has Matrix1 and Matrix2 one below the other</returns>
        public static double[,] JoinRow(double[,] Matrix1, double[,] Matrix2)
        {
            if (Matrix1.GetLength(1) != Matrix2.GetLength(1))
                throw new Exception("MatrixMath Size Error: Column dimensions do not match for matrix1 and matrix2");
            int numRows = Matrix1.GetLength(0) + Matrix2.GetLength(0);
            int numCols = Matrix1.GetLength(1);
            int mat1Rows = Matrix1.GetLength(0);
            int mat2Rows = Matrix2.GetLength(0);
            double[,] JointMatrix = new double[numRows, numCols];

            for (int j = 0; j < mat1Rows; j++)
            {
                for (int k = 0; k < numCols; k++)
                {
                    JointMatrix[j, k] = Matrix1[j, k];
                }
            }

            for (int j = 0; j < mat2Rows; j++)
            {
                for (int k = 0; k < numCols; k++)
                {
                    JointMatrix[j + mat1Rows, k] = Matrix2[j, k];
                }
            }
            return JointMatrix;
        }
        /// <summary>
        /// Concatenates two 1D double arrays and returns the result. The given variables remain unchanged
        /// </summary>
        /// <param name="Array1">Array that comes to the left.</param>
        /// <param name="Array2">Array that is appended to the end of the first array</param>
        /// <returns>A double array that has Array1 and Array2 side by side</returns>
        public static double[] JoinArr(double[] Array1, double[] Array2)
        {
            int Mat1Elements = Array1.GetLength(0);
            int Mat2Elements = Array2.GetLength(0);
            int NumElements = Mat1Elements + Mat2Elements;
            double[] JointArray = new double[NumElements];

            for (int j = 0; j < Mat1Elements; j++)
                JointArray[j] = Array1[j];

            for (int j = 0; j < Mat2Elements; j++)
                JointArray[j + Mat1Elements] = Array2[j];

            return JointArray;
        }
        /// <summary>
        /// Concatenates two 1D integer arrays and returns the result. The given variables remain unchanged
        /// </summary>
        /// <param name="Array1">Array that comes to the left.</param>
        /// <param name="Array2">Array that is appended to the end of the first array</param>
        /// <returns>An integer array that has Array1 and Array2 side by side</returns>
        public static int[] JoinArr(int[] Array1, int[] Array2)
        {
            int Mat1Elements = Array1.GetLength(0);
            int Mat2Elements = Array2.GetLength(0);
            int NumElements = Mat1Elements + Mat2Elements;
            int[] JointArray = new int[NumElements];

            for (int j = 0; j < Mat1Elements; j++)
                JointArray[j] = Array1[j];

            for (int j = 0; j < Mat2Elements; j++)
                JointArray[j + Mat1Elements] = Array2[j];

            return JointArray;
        }
        #endregion
    }
}

