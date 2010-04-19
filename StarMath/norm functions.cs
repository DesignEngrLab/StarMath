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
    public static partial class StarMath
    {
        #region Norm Functions

        /// <summary>
        ///   Returns to 1-norm (sum of absolute values of all terms)
        ///   of the difference between x and y.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <param name = "y">The vector, y.</param>
        /// <returns>Scalar value of 1-norm.</returns>
        public static double norm1(double[] x, double[] y)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            else if (x == null) throw new Exception("The vector, y, is null.");
            else if (x.GetLength(0) != y.GetLength(0)) throw new Exception("The vectors are not the same size.");
            double norm = 0.0;
            int maxlength = x.GetLength(0);

            for (int i = 0; i != maxlength; i++)
            {
                double xTerm;
                if (i >= x.GetLength(0)) xTerm = 0.0;
                else xTerm = x[i];
                double yTerm;
                if (i >= y.GetLength(0)) yTerm = 0.0;
                else yTerm = y[i];
                norm += Math.Abs(xTerm - yTerm);
            }
            return norm;
        }

        /// <summary>
        ///   Returns to 1-norm (sum of absolute values of all terms)
        ///   of the vector, x.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <returns>Scalar value of 1-norm.</returns>
        public static double norm1(double[] x)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            double norm = 0.0;
            int xlength = x.GetLength(0);
            for (int i = 0; i != xlength; i++)
                norm += Math.Abs(x[i]);
            return norm;
        }

        /// <summary>
        ///   Returns to 1-norm (sum of absolute values of all terms)
        ///   of the matrix, A.
        /// </summary>
        /// <param name = "A">The matrix, A.</param>
        /// <returns>Scalar value of 1-norm.</returns>
        public static double norm1(double[,] A)
        {
            if (A == null) throw new Exception("The matrix, A, is null.");
            double norm = 0.0;
            int rowlength = A.GetLength(0);
            int collength = A.GetLength(0);
            for (int i = 0; i != rowlength; i++)
                for (int j = 0; j != collength; j++)
                    norm += Math.Abs(A[i, j]);
            return norm;
        }

        /// <summary>
        ///   Returns to 2-norm (square root of the sum of squares of all terms)
        ///   of the difference between x and y.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <param name = "y">The vector, y.</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(double[] x, double[] y)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            else if (x == null) throw new Exception("The vector, y, is null.");
            else if (x.GetLength(0) != y.GetLength(0)) throw new Exception("The vectors are not the same size.");
            double norm = 0.0;
            int xlength = x.GetLength(0);
            if (xlength != y.GetLength(0)) return -1.0;
            for (int i = 0; i != xlength; i++)
                norm += (x[i] - y[i]) * (x[i] - y[i]);
            return Math.Sqrt(norm);
        }

        /// <summary>
        ///   Returns to 2-norm (square root of the sum of squares of all terms)
        ///   of the vector, x.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(double[] x)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            double norm = 0.0;
            int xlength = x.GetLength(0);
            for (int i = 0; i != xlength; i++)
                norm += (x[i] * x[i]);
            return Math.Sqrt(norm);
        }

        /// <summary>
        ///   Returns to 2-norm (square root of the sum of squares of all terms)
        ///   of the matrix, A.
        /// </summary>
        /// <param name = "A">The matrix, A.</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(double[,] A)
        {
            if (A == null) throw new Exception("The matrix, A, is null.");
            double norm = 0.0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            for (int i = 0; i != maxRow; i++)
                for (int j = 0; j != maxCol; j++)
                    norm += (A[i, j] * A[i, j]);
            return Math.Sqrt(norm);
        }

        /// <summary>
        ///   Returns to 2-norm (square root of the sum of squares of all terms)
        ///   of the matrix, A.
        /// </summary>
        /// <param name = "A">The matrix, A.</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(int[,] A)
        {
            if (A == null) throw new Exception("The matrix, A, is null.");
            double norm = 0.0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            for (int i = 0; i != maxRow; i++)
                for (int j = 0; j != maxCol; j++)
                    norm += (A[i, j] * A[i, j]);
            return Math.Sqrt(norm);
        }
        #endregion
    }
}