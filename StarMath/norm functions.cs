#region

using System;
using System.Collections.Generic;
using System.Linq;
#endregion

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
            if (y == null) throw new Exception("The vector, y, is null.");
            if (x.GetLength(0) != y.GetLength(0)) throw new Exception("The vectors are not the same size.");
            var norm = 0.0;
            var maxlength = x.GetLength(0);

            for (var i = 0; i != maxlength; i++)
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
        public static int norm1(IEnumerable<int> x)
        {
            if (x == null) throw new Exception("The vector, x, is null.");
            return x.Sum(a => Math.Abs(a));
            //var norm = 0;
            //var xlength = x.GetLength(0);
            //for (var i = 0; i != xlength; i++)
            //    norm += Math.Abs(x[i]);
            //return norm;
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
            var norm = 0.0;
            var xlength = x.GetLength(0);
            for (var i = 0; i != xlength; i++)
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
            var norm = 0.0;
            var rowlength = A.GetLength(0);
            var collength = A.GetLength(0);
            for (var i = 0; i != rowlength; i++)
                for (var j = 0; j != collength; j++)
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
            if (x == null) throw new Exception("The vector, y, is null.");
            if (x.GetLength(0) != y.GetLength(0)) throw new Exception("The vectors are not the same size.");
            var norm = 0.0;
            var xlength = x.GetLength(0);
            if (xlength != y.GetLength(0)) return -1.0;
            for (var i = 0; i != xlength; i++)
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
            var norm = 0.0;
            var xlength = x.GetLength(0);
            for (var i = 0; i != xlength; i++)
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
            var norm = 0.0;
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            for (var i = 0; i != maxRow; i++)
                for (var j = 0; j != maxCol; j++)
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
            var norm = 0.0;
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            for (var i = 0; i != maxRow; i++)
                for (var j = 0; j != maxCol; j++)
                    norm += (A[i, j] * A[i, j]);
            return Math.Sqrt(norm);
        }

        #endregion

        #region Normalize

        /// <summary>
        ///   Returns to normalized vector (has lenght or 2-norm of 1))
        ///   of the vector, x.
        /// </summary>
        /// <param name = "x">The vector, x.</param>
        /// <returns>unit vector.</returns>
        public static double[] normalize(double[] x)
        {
            return multiply((1 / norm2(x)), x);
        }

        #endregion
    }
}