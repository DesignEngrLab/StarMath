using System;

namespace StarMatLib
{
    // note this is set to public for testing purposes only - remove when done.
    public static partial class StarMat
    {
        #region Make and Extract functions

        public static int[,] makeZero(int p)
        {
            int[,] I = new int[p, p];
            for (int i = 0; i != p; i++)
                for (int j = 0; j != p; j++)
                    I[i, j] = 0;
            return I;
        }
        /// <summary>
        /// Makes an identity matrix of size p by p.
        /// </summary>
        /// <param name="p">The size, p.</param>
        /// <returns>the identity matrix, I.</returns>
        public static double[,] makeIdentity(int p)
        {
            double[,] I = new double[p, p];
            for (int i = 0; i != p; i++)
                for (int j = 0; j != p; j++)
                    if (i == j) I[i, j] = 1.0;
                    else I[i, j] = 0.0;
            return I;
        }
        /// <summary>
        /// Gets a column of matrix, A.
        /// </summary>
        /// <param name="colindex">The column index.</param>
        /// <param name="A">The matrix, A.</param>
        /// <returns>the column vector, v.</returns>
        public static double[] GetColumn(int colIndex, double[,] A)
        {
            int n = A.GetLength(1);
            if ((colIndex < 0) || (colIndex >= n))
                throw new Exception("MatrixMath Size Error: An index value of "
                    + colIndex.ToString()
                    + " for getColumn is not in required range from 0 up to (but not including) "
                    + n.ToString() + ".");
            else
            {
                double[] v = new double[n];
                for (int i = 0; i < n; i++)
                    v[i] = A[i, colIndex];
                return v;
            }
        }

        /// <summary>
        /// Gets a row of matrix, A.
        /// </summary>
        /// <param name="colindex">The row index.</param>
        /// <param name="A">The matrix, A.</param>
        /// <returns>The index of the row, rowIndex.</returns>
        public static double[] GetRow(int rowIndex, double[,] A)
        {
            int n_rows = A.GetLength(0);
            int n_cols = A.GetLength(1);
            if ((rowIndex < 0) || (rowIndex >= n_rows))
                throw new Exception("MatrixMath Size Error: An index value of "
                    + rowIndex.ToString()
                    + " for getRow is not in required range from 0 up to (but not including) "
                    + n_rows.ToString() + ".");
            else
            {
                double[] v = new double[n_cols];
                for (int i = 0; i < n_cols; i++)
                    v[i] = A[rowIndex, i];
                return v;
            }
        }

        /// <summary>
        /// Sets/Replaces the row of matrix A with the vector v.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="v">The vecvtor, v.</param>
        /// <param name="p">The index of the row, rowIndex.</param>
        public static void SetRow(int rowIndex, double[,] A, double[] v)
        {
            int n = A.GetLength(0);
            if ((rowIndex < 0) || (rowIndex >= n))
                throw new Exception("MatrixMath Size Error: An index value of "
                    + rowIndex.ToString()
                    + " for getRow is not in required range from 0 up to (but not including) "
                    + n.ToString() + ".");
            else
            {
                for (int i = 0; i < n; i++)
                    A[rowIndex, i] = v[i];
            }
        }
        public static void SetColumn(int colIndex, double[,] A, double[] v)
        {
            int n = A.GetLength(1);
            if ((colIndex < 0) || (colIndex >= n))
                throw new Exception("MatrixMath Size Error: An index value of "
                    + colIndex.ToString()
                    + " for getColumn is not in required range from 0 up to (but not including) "
                    + n.ToString() + ".");
            else
            {
                for (int i = 0; i < n; i++)
                    A[i, colIndex] = v[i];
            }
        }
        #endregion
    }
}

