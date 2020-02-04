// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="add subtract multiply.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Add Vector-to-Vector and Matrix-to-Matrix

        /// <summary>
        ///     Adds arrays A and B
        /// </summary>
        /// <param name="A">1D float array 1</param>
        /// <param name="B">1D float array 2</param>
        /// <returns>1D float array that contains sum of vectros A and B</returns>
        /// <exception cref="System.ArithmeticException">Matrix sizes do not match</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] add(this IList<float> A, IList<float> B)
        {
            var length = A.Count;
            if (length != B.Count) throw new ArithmeticException("Matrix sizes do not match");
            return add(A, B, length);
        }

        /// <summary>
        ///     Adds arrays A and B
        /// </summary>
        /// <param name="A">1D int array 1</param>
        /// <param name="B">1D float array 2</param>
        /// <returns>1D float array that contains sum of vectros A and B</returns>
        /// <exception cref="System.ArithmeticException">Matrix sizes do not match</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] add(this IList<int> A, IList<float> B)
        {
            var length = A.Count;
            if (length != B.Count) throw new ArithmeticException("Matrix sizes do not match");
            return add(A, B, length);
        }



        /// <summary>
        ///     Adds arrays A and B
        /// </summary>
        /// <param name="A">2D float array 1</param>
        /// <param name="B">2D float array 2</param>
        /// <returns>2D float array that contains sum of vectros A and B</returns>
        /// <exception cref="System.ArithmeticException">
        ///     Matrix row count do not match
        ///     or
        ///     Matrix column count do not match
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] add(this float[,] A, float[,] B)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if (numRows != B.GetLength(0))
                throw new ArithmeticException("Matrix row count do not match");
            if (numCols != B.GetLength(1))
                throw new ArithmeticException("Matrix column count do not match");

            return add(A, B, numRows, numCols);
        }

        /// <summary>
        ///     Adds arrays A and B
        /// </summary>
        /// <param name="A">2D float array 1</param>
        /// <param name="B">2D int array 2</param>
        /// <returns>2D float array that contains sum of vectros A and B</returns>
        /// <exception cref="System.ArithmeticException">
        ///     Matrix row count do not match
        ///     or
        ///     Matrix column count do not match
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] add(this int[,] A, float[,] B)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if (numRows != B.GetLength(0))
                throw new ArithmeticException("Matrix row count do not match");
            if (numCols != B.GetLength(1))
                throw new ArithmeticException("Matrix column count do not match");

            return add(A, B, numRows, numCols);
        }



        /// <summary>
        ///     Adds arrays A and B
        /// </summary>
        /// <param name="A">1D float array 1</param>
        /// <param name="B">1D float array 2</param>
        /// <param name="length">The length of the array.</param>
        /// <returns>1D float array that contains sum of vectros A and B</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] add(this IList<float> A, IList<float> B, int length)
        {
            var c = new float[length];
            for (var i = 0; i != length; i++)
                c[i] = A[i] + B[i];
            return c;
        }

        /// <summary>
        ///     Adds arrays A and B
        /// </summary>
        /// <param name="A">1D int array 1</param>
        /// <param name="B">1D float array 2</param>
        /// <param name="length">The length of the array.</param>
        /// <returns>1D float array that contains sum of vectros A and B</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] add(this IList<int> A, IList<float> B, int length)
        {
            var c = new float[length];
            for (var i = 0; i != length; i++)
                c[i] = A[i] + B[i];
            return c;
        }


        /// <summary>
        ///     Adds arrays A and B
        /// </summary>
        /// <param name="A">2D float array 1</param>
        /// <param name="B">2D float array 2</param>
        /// <param name="numRows">The number of rows.</param>
        /// <param name="numCols">The number of columns.</param>
        /// <returns>2D float array that contains sum of vectros A and B</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] add(this float[,] A, float[,] B, int numRows, int numCols)
        {
            var C = new float[numRows, numCols];

            for (var i = 0; i != numRows; i++)
                for (var j = 0; j != numCols; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        /// <summary>
        ///     Adds arrays A and B
        /// </summary>
        /// <param name="A">2D float array 1</param>
        /// <param name="B">2D int array 2</param>
        /// <param name="numRows">The number of rows.</param>
        /// <param name="numCols">The number of columns.</param>
        /// <returns>2D float array that contains sum of vectros A and B</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] add(this int[,] A, float[,] B, int numRows, int numCols)
        {
            var C = new float[numRows, numCols];

            for (var i = 0; i != numRows; i++)
                for (var j = 0; j != numCols; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }



        #endregion

        #region Subtract Vector-to-Vector and Matrix-to-Matrix

        /// <summary>
        ///     Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend vector, A (1D float)</param>
        /// <param name="B">The subtrahend vector, B (1D float)</param>
        /// <returns>Returns the difference vector, C (1D float)</returns>
        /// <exception cref="System.ArithmeticException">Matrix sizes do not match</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] subtract(this IList<float> A, IList<float> B)
        {
            var length = A.Count;
            if (length != B.Count)
                throw new ArithmeticException("Matrix sizes do not match");
            return subtract(A, B, length);
        }

        /// <summary>
        ///     Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend vector, A (1D int)</param>
        /// <param name="B">The subtrahend vector, B (1D float)</param>
        /// <returns>Returns the difference vector, C (1D float)</returns>
        /// <exception cref="System.ArithmeticException">Matrix sizes do not match</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] subtract(this IList<int> A, IList<float> B)
        {
            var length = A.Count;
            if (length != B.Count)
                throw new ArithmeticException("Matrix sizes do not match");
            return subtract(A, B, length);
        }

        /// <summary>
        ///     Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend vector, A (1D float)</param>
        /// <param name="B">The subtrahend vector, B (1D int)</param>
        /// <returns>Returns the difference vector, C (1D float)</returns>
        /// <exception cref="System.ArithmeticException">Matrix sizes do not match</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] subtract(this IList<float> A, IList<int> B)
        {
            var length = A.Count;
            if (length != B.Count)
                throw new ArithmeticException("Matrix sizes do not match");
            return subtract(A, B, length);
        }

        /// <summary>
        ///     Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend matrix, A (2D float).</param>
        /// <param name="B">The subtrahend matrix, B (2D float).</param>
        /// <returns>Returns the difference matrix, C (2D float)</returns>
        /// <exception cref="System.ArithmeticException">
        ///     Matrix row count do not match
        ///     or
        ///     Matrix column count do not match
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] subtract(this float[,] A, float[,] B)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if (numRows != B.GetLength(0))
                throw new ArithmeticException("Matrix row count do not match");
            if (numCols != B.GetLength(1))
                throw new ArithmeticException("Matrix column count do not match");

            return subtract(A, B, numRows, numCols);
        }

        /// <summary>
        ///     Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend matrix, A (2D int).</param>
        /// <param name="B">The subtrahend matrix, B (2D float).</param>
        /// <returns>Returns the difference matrix, C (2D float)</returns>
        /// <exception cref="System.ArithmeticException">
        ///     Matrix row count do not match
        ///     or
        ///     Matrix column count do not match
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] subtract(this int[,] A, float[,] B)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if (numRows != B.GetLength(0))
                throw new ArithmeticException("Matrix row count do not match");
            if (numCols != B.GetLength(1))
                throw new ArithmeticException("Matrix column count do not match");

            return subtract(A, B, numRows, numCols);
        }

        /// <summary>
        ///     Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend matrix, A (2D float).</param>
        /// <param name="B">The subtrahend matrix, B (2D int).</param>
        /// <returns>Returns the difference matrix, C (2D float)</returns>
        /// <exception cref="System.ArithmeticException">
        ///     Matrix row count do not match
        ///     or
        ///     Matrix column count do not match
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] subtract(this float[,] A, int[,] B)
        {
            var numRows = A.GetLength(0);
            var numCols = A.GetLength(1);
            if (numRows != B.GetLength(0))
                throw new ArithmeticException("Matrix row count do not match");
            if (numCols != B.GetLength(1))
                throw new ArithmeticException("Matrix column count do not match");

            return subtract(A, B, numRows, numCols);
        }


        /// <summary>
        ///     Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend vector, A (1D float)</param>
        /// <param name="B">The subtrahend vector, B (1D float)</param>
        /// <param name="length">The length of the vectors.</param>
        /// <returns>Returns the difference vector, C (1D float)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] subtract(this IList<float> A, IList<float> B, int length)
        {
            var c = new float[length];
            for (var i = 0; i != length; i++)
                c[i] = A[i] - B[i];
            return c;
        }

        /// <summary>
        ///     Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend vector, A (1D int)</param>
        /// <param name="B">The subtrahend vector, B (1D float)</param>
        /// <param name="length">The length of the vectors.</param>
        /// <returns>Returns the difference vector, C (1D float)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] subtract(this IList<int> A, IList<float> B, int length)
        {
            var c = new float[length];
            for (var i = 0; i != length; i++)
                c[i] = A[i] - B[i];
            return c;
        }

        /// <summary>
        ///     Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend vector, A (1D float)</param>
        /// <param name="B">The subtrahend vector, B (1D int)</param>
        /// <param name="length">The length of the vectors.</param>
        /// <returns>Returns the difference vector, C (1D float)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] subtract(this IList<float> A, IList<int> B, int length)
        {
            var c = new float[length];
            for (var i = 0; i != length; i++)
                c[i] = A[i] - B[i];
            return c;
        }


        /// <summary>
        ///     Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend matrix, A (2D float).</param>
        /// <param name="B">The subtrahend matrix, B (2D float).</param>
        /// <param name="numRows">The number of rows.</param>
        /// <param name="numCols">The number of columns.</param>
        /// <returns>Returns the difference matrix, C (2D float)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] subtract(this float[,] A, float[,] B, int numRows, int numCols)
        {
            var C = new float[numRows, numCols];

            for (var i = 0; i != numRows; i++)
                for (var j = 0; j != numCols; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }

        /// <summary>
        ///     Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend matrix, A (2D int).</param>
        /// <param name="B">The subtrahend matrix, B (2D float).</param>
        /// <param name="numRows">The number of rows.</param>
        /// <param name="numCols">The number of columns.</param>
        /// <returns>Returns the difference matrix, C (2D float)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] subtract(this int[,] A, float[,] B, int numRows, int numCols)
        {
            var C = new float[numRows, numCols];

            for (var i = 0; i != numRows; i++)
                for (var j = 0; j != numCols; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }

        /// <summary>
        ///     Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend matrix, A (2D float).</param>
        /// <param name="B">The subtrahend matrix, B (2D int).</param>
        /// <param name="numRows">The number of rows.</param>
        /// <param name="numCols">The number of columns.</param>
        /// <returns>Returns the difference matrix, C (2D float)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] subtract(this float[,] A, int[,] B, int numRows, int numCols)
        {
            var C = new float[numRows, numCols];

            for (var i = 0; i != numRows; i++)
                for (var j = 0; j != numCols; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }
        #endregion
    }
}