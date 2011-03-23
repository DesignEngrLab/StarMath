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
 *     Please find further details and contact information on StarMath
 *     at http://starmath.codeplex.com/.
 *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Norm Functions
        #region 1-norm (Manhattan Distance)
        /// <summary>
        /// Returns to 1-norm (sum of absolute values of all terms)
        /// of the difference between x and y.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <param name = "y">The vector, y.</param>
        /// <returns>Scalar value of 1-norm.</returns>
        public static double norm1(IList<double> x, IList<double> y)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            if (y == null) throw new Exception("The vector, y, is null.");
            if (x.Count() != y.Count()) throw new Exception("The vectors are not the same size.");
            var norm = 0.0;
            var maxlength = x.Count();

            for (var i = 0; i != maxlength; i++)
                norm += Math.Abs(x[i] - y[i]);

            return norm;
        }

        /// <summary>
        /// Returns to 1-norm (sum of absolute values of all terms)
        /// of the difference between x and y.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <param name = "y">The vector, y.</param>
        /// <returns>Scalar value of 1-norm.</returns>
        public static int norm1(IList<int> x, IList<int> y)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            if (y == null) throw new Exception("The vector, y, is null.");
            if (x.Count() != y.Count()) throw new Exception("The vectors are not the same size.");
            var norm = 0;
            var maxlength = x.Count();

            for (var i = 0; i != maxlength; i++)
                norm += Math.Abs(x[i] - y[i]);

            return norm;
        }

        /// <summary>
        /// Returns to 1-norm (sum of absolute values of all terms)
        /// of the vector, x.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <returns>Scalar value of 1-norm.</returns>
        public static double norm1(IEnumerable<double> x)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            return x.Sum(a => Math.Abs(a));
        }
        /// <summary>
        /// Returns to 1-norm (sum of absolute values of all terms)
        /// of the vector, x.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <returns>Scalar value of 1-norm.</returns>
        public static int norm1(IEnumerable<int> x)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            return x.Sum(a => Math.Abs(a));
        }

        /// <summary>
        /// Returns to 1-norm (sum of absolute values of all terms)
        /// of the matrix, A.
        /// </summary>
        /// <param name = "A">The matrix, A.</param>
        /// <returns>Scalar value of 1-norm.</returns>
        public static double norm1(double[,] A)
        {
            if (A == null) throw new Exception("The matrix, A, is null.");
            var norm = 0.0;
            var rowlength = A.GetLength(0);
            var collength = A.GetLength(0);
            for (var i = 0; i != rowlength; i++)
                for (var j = 0; j != collength; j++)
                    norm += Math.Abs(A[i, j]);
            return norm;
        }


        /// <summary>
        /// Returns to 1-norm (sum of absolute values of all terms)
        /// of the matrix, A.
        /// </summary>
        /// <param name = "A">The matrix, A.</param>
        /// <returns>Scalar value of 1-norm.</returns>
        public static int norm1(int[,] A)
        {
            if (A == null) throw new Exception("The matrix, A, is null.");
            var norm = 0;
            var rowlength = A.GetLength(0);
            var collength = A.GetLength(0);
            for (var i = 0; i != rowlength; i++)
                for (var j = 0; j != collength; j++)
                    norm += Math.Abs(A[i, j]);
            return norm;
        }

        #endregion

        #region 2-norm (Euclidian Distance)
        /// <summary>
        /// Returns to 2-norm (square root of the sum of squares of all terms)
        /// of the difference between x and y.
        /// </summary>
        /// <param name="x">The vector, x.</param>
        /// <param name="y">The vector, y.</param>
        /// <param name="dontDoSqrt">if set to <c>true</c> [don't take the square root].</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(IList<double> x, IList<double> y, Boolean dontDoSqrt = false)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            if (y == null) throw new Exception("The vector, y, is null.");
            if (x.Count() != y.Count()) throw new Exception("The vectors are not the same size.");
            var norm = 0.0;
            var maxlength = x.Count();

            for (var i = 0; i != maxlength; i++)
                norm += (x[i] - y[i]) * (x[i] - y[i]);
            return dontDoSqrt ? norm : Math.Sqrt(norm);
        }


        /// <summary>
        /// Returns to 2-norm (square root of the sum of squares of all terms)
        /// of the difference between x and y.
        /// </summary>
        /// <param name="x">The vector, x.</param>
        /// <param name="y">The vector, y.</param>
        /// <param name="dontDoSqrt">if set to <c>true</c> [don't take the square root].</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(IList<int> x, IList<int> y, Boolean dontDoSqrt = false)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            if (y == null) throw new Exception("The vector, y, is null.");
            if (x.Count() != y.Count()) throw new Exception("The vectors are not the same size.");
            var norm = 0.0;
            var maxlength = x.Count();

            for (var i = 0; i != maxlength; i++)
                norm += (x[i] - y[i]) * (x[i] - y[i]);
            return dontDoSqrt ? norm : Math.Sqrt(norm);
        }

        /// <summary>
        /// Returns to 2-norm (square root of the sum of squares of all terms)
        /// of the vector, x.
        /// </summary>
        /// <param name="x">The vector, x.</param>
        /// <param name="dontDoSqrt">if set to <c>true</c> [don't take the square root].</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(IEnumerable<double> x, Boolean dontDoSqrt = false)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            return dontDoSqrt ? x.Sum(a => a * a) : Math.Sqrt(x.Sum(a => a * a));
        }

        /// <summary>
        /// Returns to 2-norm (square root of the sum of squares of all terms)
        /// of the vector, x.
        /// </summary>
        /// <param name="x">The vector, x.</param>
        /// <param name="dontDoSqrt">if set to <c>true</c> [don't take the square root].</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(IEnumerable<int> x, Boolean dontDoSqrt = false)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            return dontDoSqrt ? x.Sum(a => a * a) : Math.Sqrt(x.Sum(a => a * a));
        }


        /// <summary>
        /// Returns to 2-norm (square root of the sum of squares of all terms)
        /// of the matrix, A.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="dontDoSqrt">if set to <c>true</c> [don't take the square root].</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(double[,] A, Boolean dontDoSqrt = false)
        {
            if (A == null) throw new Exception("The matrix, A, is null.");
            var norm = 0.0;
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            for (var i = 0; i != maxRow; i++)
                for (var j = 0; j != maxCol; j++)
                    norm += (A[i, j] * A[i, j]);
            return dontDoSqrt ? norm : Math.Sqrt(norm);
        }

        /// <summary>
        /// Returns to 2-norm (square root of the sum of squares of all terms)
        /// of the matrix, A.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="dontDoSqrt">if set to <c>true</c> [don't take the square root].</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double norm2(int[,] A, Boolean dontDoSqrt = false)
        {
            if (A == null) throw new Exception("The matrix, A, is null.");
            var norm = 0.0;
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            for (var i = 0; i != maxRow; i++)
                for (var j = 0; j != maxCol; j++)
                    norm += (A[i, j] * A[i, j]);
            return dontDoSqrt ? norm : Math.Sqrt(norm);
        }
        #endregion

        #region P-norm
        /// <summary>
        /// Returns to p-norm (p-root of the sum of each term raised to the p power)
        /// </summary>
        /// <param name="x">The vector, x.</param>
        /// <param name="p">The power, p.</param>
        /// <param name="dontDoPRoot">if set to <c>true</c> [don't take the P-root].</param>
        /// <returns>Scalar value of P-norm.</returns>
        public static double normP(IEnumerable<double> x, double p, Boolean dontDoPRoot = false)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            return dontDoPRoot ? x.Sum(a => Math.Pow(a, p)) : Math.Pow(x.Sum(a => a * a), 1 / p);
        }

        /// <summary>
        /// Returns to p-norm (p-root of the sum of each term raised to the p power)
        /// </summary>
        /// <param name="x">The vector, x.</param>
        /// <param name="p">The power, p.</param>
        /// <param name="dontDoPRoot">if set to <c>true</c> [don't take the P-root].</param>
        /// <returns>Scalar value of P-norm.</returns>
        public static double normP(IEnumerable<int> x, double p, Boolean dontDoPRoot = false)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            return dontDoPRoot ? x.Sum(a => Math.Pow(a, p)) : Math.Pow(x.Sum(a => a * a), 1 / p);
        }


        /// <summary>
        /// Returns to p-norm (p-root of the sum of each term raised to the p power)
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="p">The power, p.</param>
        /// <param name="dontDoPRoot">if set to <c>true</c> [don't take the P-root].</param>
        /// <returns>Scalar value of P-norm.</returns>
        public static double normP(double[,] A, double p, Boolean dontDoPRoot = false)
        {
            if (A == null) throw new Exception("The matrix, A, is null.");
            var norm = 0.0;
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            for (var i = 0; i != maxRow; i++)
                for (var j = 0; j != maxCol; j++)
                    norm += Math.Pow(A[i, j], p);
            return dontDoPRoot ? norm : Math.Pow(norm, 1 / p);
        }

        /// <summary>
        /// Returns to 2-norm (square root of the sum of squares of all terms)
        /// of the matrix, A.
        /// </summary>
        /// <param name="A">The matrix, A.</param>
        /// <param name="p">The power, p.</param>
        /// <param name="dontDoPRoot">if set to <c>true</c> [don't take the P-root].</param>
        /// <returns>Scalar value of 2-norm.</returns>
        public static double normP(int[,] A, double p, Boolean dontDoPRoot = false)
        {
            if (A == null) throw new Exception("The matrix, A, is null.");
            var norm = 0.0;
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            for (var i = 0; i != maxRow; i++)
                for (var j = 0; j != maxCol; j++)
                    norm += Math.Pow(A[i, j], p);
            return dontDoPRoot ? norm : Math.Pow(norm, 1 / p);
        }
        #endregion

        #endregion

        #region Normalize

        /// <summary>
        /// Returns to normalized vector (has lenght or 2-norm of 1))
        /// of the vector, x.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <returns>unit vector.</returns>
        public static double[] normalize(double[] x)
        {
            return divide(x, norm2(x));
        }

        #endregion
    }
}