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
        #region Add, Subtract, and Multiply functions

        #region Multiplication of Scalars, Vectors, and Matrices

        #region Scalars multiplying vectors
        /// <summary>
        /// Multiplies all elements of a 1D double array with the double value.
        /// </summary>
        /// <param name = "a">The double value to be multiplied</param>
        /// <param name = "B">The double vector to be multiplied with</param>
        /// <returns>A 1D double array that contains the product</returns>
        public static double[] multiply(double a, IList<double> B)
        {
            // scale vector B by the amount of scalar a
            return multiply(a, B, B.Count());
        }
        /// <summary>
        /// Multiplies all elements of a 1D double array with the double value.
        /// </summary>
        /// <param name="a">The double value to be multiplied</param>
        /// <param name="B">The double vector to be multiplied with</param>
        /// <param name="length">The length of the vector B. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A 1D double array that contains the product
        /// </returns>
        public static double[] multiply(double a, IList<double> B, int length)
        {
            // scale vector B by the amount of scalar a
            var c = new double[length];
            for (var i = 0; i != length; i++)
                c[i] = a * B[i];
            return c;
        }
        /// <summary>
        /// Multiplies all elements of a 1D integer array with the double value.
        /// </summary>
        /// <param name = "a">The double value to be multiplied</param>
        /// <param name = "B">The integer vector to be multiplied with</param>
        /// <returns>A 1D double array that contains the product</returns>
        public static double[] multiply(double a, IList<int> B)
        {
            // scale vector B by the amount of scalar a
            return multiply(a, B, B.Count());
        }
        /// <summary>
        /// Multiplies all elements of a 1D integer array with the double value.
        /// </summary>
        /// <param name="a">The double value to be multiplied</param>
        /// <param name="B">The integer vector to be multiplied with</param>
        /// <param name="length">The length of the vector B. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A 1D double array that contains the product
        /// </returns>
        public static double[] multiply(double a, IList<int> B, int length)
        {
            // scale vector B by the amount of scalar a
            var c = new double[length];
            for (var i = 0; i != length; i++)
                c[i] = a * B[i];
            return c;
        }

        /// <summary>
        /// Divides all elements of a 1D double array by the double value.
        /// </summary>
        /// <param name = "B">The vector to be divided</param>
        /// <param name = "a">The double value to be divided by, the divisor.</param>
        /// <returns>A 1D double array that contains the product</returns>
        public static double[] divide(IList<double> B, double a)
        {
            return multiply((1 / a), B);
        }
        /// <summary>
        /// Divides all elements of a 1D double array by the double value.
        /// </summary>
        /// <param name="B">The vector to be divided</param>
        /// <param name="a">The double value to be divided by, the divisor.</param>
        /// <param name="length">The length of the vector B. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A 1D double array that contains the product
        /// </returns>
        public static double[] divide(IList<double> B, double a, int length)
        {
            return multiply((1 / a), B, length);
        }
        /// <summary>
        /// Divides all elements of a 1D integer array by the double value.
        /// </summary>
        /// <param name = "B">The vector to be divided</param>
        /// <param name = "a">The double value to be divided by, the divisor.</param>
        /// <returns>A 1D double array that contains the product</returns>
        public static double[] divide(IList<int> B, double a)
        {
            return multiply((1 / a), B);
        }
        /// <summary>
        /// Divides all elements of a 1D integer array by the double value.
        /// </summary>
        /// <param name="B">The vector to be divided</param>
        /// <param name="a">The double value to be divided by, the divisor.</param>
        /// <param name="length">The length of the vector B. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A 1D double array that contains the product
        /// </returns>
        public static double[] divide(IList<int> B, double a, int length)
        {
            return multiply((1 / a), B, length);
        }

        /// <summary>
        /// Multiplies all elements of a 2D double array with a double value.
        /// </summary>
        /// <param name = "a">The double value to be multiplied</param>
        /// <param name = "B">The matrix to be multiplied with</param>
        /// <returns>A 2D double array that contains the product</returns>
        public static double[,] multiply(double a, double[,] B)
        {
            return multiply(a, B, B.GetLength(0), B.GetLength(1));
        }

        /// <summary>
        /// Multiplies all elements of a 2D double array with a double value.
        /// </summary>
        /// <param name="a">The double value to be multiplied</param>
        /// <param name="B">The matrix to be multiplied with</param>
        /// <param name="numRows">The number of rows. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <param name="numCols">The number of cols. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A 2D double array that contains the product
        /// </returns>
        public static double[,] multiply(double a, double[,] B, int numRows, int numCols)
        {
            var c = new double[numRows, numCols];
            for (var i = 0; i != numRows; i++)
                for (var j = 0; j != numCols; j++)
                    c[i, j] = a * B[i, j];
            return c;
        }
        /// <summary>
        /// Multiplies all elements of a 2D int array with a double value.
        /// </summary>
        /// <param name = "a">The double value to be multiplied</param>
        /// <param name = "B">The matrix to be multiplied with</param>
        /// <returns>A 2D double array that contains the product</returns>
        public static double[,] multiply(double a, int[,] B)
        {
            return multiply(a, B, B.GetLength(0), B.GetLength(1));
        }
        /// <summary>
        /// Multiplies all elements of a 2D int array with a double value.
        /// </summary>
        /// <param name="a">The double value to be multiplied</param>
        /// <param name="B">The matrix to be multiplied with</param>
        /// <param name="numRows">The number of rows. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <param name="numCols">The number of cols. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A 2D double array that contains the product
        /// </returns>
        public static double[,] multiply(double a, int[,] B, int numRows, int numCols)
        {
            var c = new double[numRows, numCols];
            for (var i = 0; i != numRows; i++)
                for (var j = 0; j != numCols; j++)
                    c[i, j] = a * B[i, j];
            return c;
        }

        /// <summary>
        /// Divides all elements of a 2D double array by the double value.
        /// </summary>
        /// <param name = "B">The matrix to be divided</param>
        /// <param name = "a">The double value to be divided by, the divisor.</param>
        /// <returns>A 2D double array that contains the product</returns>
        public static double[,] divide(double[,] B, double a)
        {
            return multiply((1 / a), B);
        }
        /// <summary>
        /// Divides all elements of a 2D double array by the double value.
        /// </summary>
        /// <param name="B">The matrix to be divided</param>
        /// <param name="a">The double value to be divided by, the divisor.</param>
        /// <param name="numRows">The number of rows. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <param name="numCols">The number of cols. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A 2D double array that contains the product
        /// </returns>
        public static double[,] divide(double[,] B, double a, int numRows, int numCols)
        {
            return multiply((1 / a), B, numRows, numCols);
        }
        /// <summary>
        /// Divides all elements of a 2D integer array by the double value.
        /// </summary>
        /// <param name = "B">The matrix to be divided</param>
        /// <param name = "a">The double value to be divided by, the divisor.</param>
        /// <returns>A 2D double array that contains the product</returns>
        public static double[,] divide(int[,] B, double a)
        {
            return multiply((1 / a), B);
        }

        /// <summary>
        /// Divides all elements of a 2D integer array by the double value.
        /// </summary>
        /// <param name="B">The matrix to be divided</param>
        /// <param name="a">The double value to be divided by, the divisor.</param>
        /// <param name="numRows">The number of rows. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <param name="numCols">The number of cols. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A 2D double array that contains the product
        /// </returns>
        public static double[,] divide(int[,] B, double a, int numRows, int numCols)
        {
            return multiply((1 / a), B, numRows, numCols);
        }
        #endregion

        #region Dot-product of vectors to vectors
        /// <summary>
        /// The dot product of the two 1D double vectors A and B
        /// </summary>
        /// <param name = "A">1D double Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns>A double value that contains the dot product</returns>
        public static double multiplyDot(IList<double> A, IList<double> B)
        {
            var length = A.Count();
            if (length != B.Count())
                throw new Exception("Matrix sizes do not match");
            return multiplyDot(A, B, length);
        }
        /// <summary>
        /// The dot product of the two 1D double vectors A and B
        /// </summary>
        /// <param name="A">1D double Array, A</param>
        /// <param name="B">1D double Array, B</param>
        /// <param name="length">The length of both vectors A and B. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A double value that contains the dot product
        /// </returns>
        public static double multiplyDot(IList<double> A, IList<double> B, int length)
        {
            var c = 0.0;
            for (var i = 0; i != length; i++)
                c += A[i] * B[i];
            return c;
        }
        /// <summary>
        /// The dot product of the one 1D int vector and one 1D double vector
        /// </summary>
        /// <param name = "A">1D int Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns>A double value that contains the dot product</returns>
        public static double multiplyDot(IList<int> A, IList<double> B)
        {
            var length = A.Count();
            if (length != B.Count())
                throw new Exception("Matrix sizes do not match");
            return multiplyDot(A, B, length);
        }
        /// <summary>
        /// The dot product of the one 1D int vector and one 1D double vector
        /// </summary>
        /// <param name="A">1D int Array, A</param>
        /// <param name="B">1D double Array, B</param>
        /// <param name="length">The length of both vectors A and B. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A double value that contains the dot product
        /// </returns>
        public static double multiplyDot(IList<int> A, IList<double> B, int length)
        {
            var c = 0.0;
            for (var i = 0; i != length; i++)
                c += A[i] * B[i];
            return c;
        }
        /// <summary>
        /// The dot product of the two 1D int vectors A and B
        /// </summary>
        /// <param name = "A">1D int Array, A</param>
        /// <param name = "B">1D int Array, B</param>
        /// <returns>A double value that contains the dot product</returns>
        public static int multiplyDot(IList<int> A, IList<int> B)
        {
            // this is B dot term_i multiplication
            var length = A.Count();
            if (length != B.Count())
                throw new Exception("Matrix sizes do not match");
            return multiplyDot(A, B, length);
        }
        /// <summary>
        /// The dot product of the two 1D int vectors A and B
        /// </summary>
        /// <param name="A">1D int Array, A</param>
        /// <param name="B">1D int Array, B</param>
        /// <param name="length">The length of both vectors A and B. This is an optional argument, but if it is already known 
        /// - there is a slight speed advantage to providing it.</param>
        /// <returns>
        /// A double value that contains the dot product
        /// </returns>
        public static int multiplyDot(IList<int> A, IList<int> B, int length)
        {
            var c = 0;
            for (var i = 0; i != length; i++)
                c += A[i] * B[i];
            return c;
        }
        #endregion

        #region Cross-product of vectors to vectors
        /// <summary>
        /// The cross product of two double-precision vectors, A and B,
        /// </summary>
        /// <param name = "A">1D double Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns>A double value that contains the dot product</returns>
        public static double[] multiplyCross(IList<double> A, IList<double> B)
        {
            if ((A.Count() == 1) && (B.Count() == 1))
                return new[] { 0.0 };
            if ((A.Count() == 2) && (B.Count() == 2))
                return new[] { 0.0, 0.0, multiplyCross2(A, B) };
            if ((A.Count() == 3) && (B.Count() == 3))
                return multiplyCross3(A, B);
            if ((A.Count() == 7) && (B.Count() == 7))
                return multiplyCross7(A, B);
            throw new Exception("Cross product is only possible for vectors of length: 1, 3, or 7");
        }

        /// <summary>
        /// The cross product of an integer vector, A, and a double vector, B.
        /// </summary>
        /// <param name = "A">1D integer Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns>A double value that contains the dot product</returns>
        public static double[] multiplyCross(IList<int> A, IList<double> B)
        {
            if ((A.Count() == 1) && (B.Count() == 1))
                return new[] { 0.0 };
            if ((A.Count() == 2) && (B.Count() == 2))
                return new[] { 0.0, 0.0, multiplyCross2(A, B) };
            if ((A.Count() == 3) && (B.Count() == 3))
                return multiplyCross3(A, B);
            if ((A.Count() == 7) && (B.Count() == 7))
                return multiplyCross7(A, B);
            throw new Exception("Cross product is only possible for vectors of length: 1, 3, or 7");
        }

        /// <summary>
        /// The cross product of two integer vectors, A and B.
        /// </summary>
        /// <param name = "A">1D integer Array, A</param>
        /// <param name = "B">1D integer Array, B</param>
        /// <returns>A double value that contains the dot product</returns>
        public static int[] multiplyCross(IList<int> A, IList<int> B)
        {
            if ((A.Count() == 1) && (B.Count() == 1))
                return new[] { 0 };
            if ((A.Count() == 2) && (B.Count() == 2))
                return new[] { 0, 0, multiplyCross2(A, B) };
            if ((A.Count() == 3) && (B.Count() == 3))
                return multiplyCross3(A, B);
            if ((A.Count() == 7) && (B.Count() == 7))
                return multiplyCross7(A, B);
            throw new Exception("Cross product is only possible for vectors of length: 1, 3, or 7");
        }

        /// <summary>
        /// The cross product of two double vectors, A and B, which are of length, 2.
        /// In actuality, there is no cross-product for 2D. This is shorthand for 2D systems 
        /// that are really simplifications of 3D. The returned scalar is actually the value in 
        /// the third (read: z) direction.
        /// </summary>
        /// <param name = "A">1D double Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns></returns>
        public static double multiplyCross2(IList<double> A, IList<double> B)
        {
            if (((A.Count() == 2) && (B.Count() == 2))
                || ((A.Count() == 3) && (B.Count() == 3) && A[2] == 0.0 && B[2] == 0.0))
                return A[0] * B[1] - B[0] * A[1];
            throw new Exception("This cross product \"shortcut\" is only used with 2D vectors to get the single value in the,"
                                + "would be, Z-direction.");
        }
        /// <summary>
        ///  The cross product of an integer vector, A, and a double vector, B, which are of length, 2.
        /// In actuality, there is no cross-product for 2D. This is shorthand for 2D systems 
        /// that are really simplifications of 3D. The returned scalar is actually the value in 
        /// the third (read: z) direction.
        /// </summary>
        /// <param name = "A">1D integer Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns></returns>
        public static double multiplyCross2(IList<int> A, IList<double> B)
        {
            if (((A.Count() == 2) && (B.Count() == 2))
                || ((A.Count() == 3) && (B.Count() == 3) && A[2] == 0.0 && B[2] == 0.0))
                return A[0] * B[1] - B[0] * A[1];
            throw new Exception("This cross product \"shortcut\" is only used with 2D vectors to get the single value in the,"
                                + "would be, Z-direction.");
        }
        /// <summary>
        /// The cross product of two integer vectors, A and B, which are of length, 2.
        /// In actuality, there is no cross-product for 2D. This is shorthand for 2D systems 
        /// that are really simplifications of 3D. The returned scalar is actually the value in 
        /// the third (read: z) direction.
        /// </summary>
        /// <param name = "A">1D integer Array, A</param>
        /// <param name = "B">1D integer Array, B</param>
        /// <returns></returns>
        public static int multiplyCross2(IList<int> A, IList<int> B)
        {
            if (((A.Count() == 2) && (B.Count() == 2))
                || ((A.Count() == 3) && (B.Count() == 3) && A[2] == 0.0 && B[2] == 0.0))
                return A[0] * B[1] - B[0] * A[1];
            throw new Exception("This cross product \"shortcut\" is only used with 2D vectors to get the single value in the,"
                                + "would be, Z-direction.");
        }

        /// <summary>
        /// The cross product of two double vectors, A and B, which are of length, 7.
        /// This is equivalent to calling multiplyCross, but a slight speed advantage
        /// may exist in skipping directly to this sub-function.
        /// </summary>
        /// <param name = "A">1D double Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns></returns>
        public static double[] multiplyCross7(IList<double> A, IList<double> B)
        {
            return new[]
                       {
                           A[1]*B[3] - A[3]*B[1] + A[2]*B[6] - A[6]*B[2] + A[4]*B[5] - A[5]*B[4],
                           A[2]*B[4] - A[4]*B[2] + A[3]*B[0] - A[0]*B[3] + A[5]*B[6] - A[6]*B[5],
                           A[3]*B[5] - A[5]*B[3] + A[4]*B[1] - A[1]*B[4] + A[6]*B[0] - A[0]*B[6],
                           A[4]*B[6] - A[6]*B[4] + A[5]*B[2] - A[2]*B[5] + A[0]*B[1] - A[1]*B[0],
                           A[5]*B[0] - A[0]*B[5] + A[6]*B[3] - A[3]*B[6] + A[1]*B[2] - A[2]*B[1],
                           A[6]*B[1] - A[1]*B[6] + A[0]*B[4] - A[4]*B[0] + A[2]*B[3] - A[3]*B[2],
                           A[0]*B[2] - A[2]*B[0] + A[1]*B[5] - A[5]*B[1] + A[3]*B[4] - A[4]*B[3]
                       };
        }
        /// <summary>
        /// The cross product of an integer vector, A, and a double vector, B, which are of length, 7.
        /// This is equivalent to calling multiplyCross, but a slight speed advantage
        /// may exist in skipping directly to this sub-function.
        /// </summary>
        /// <param name = "A">1D integer Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns></returns>
        public static double[] multiplyCross7(IList<int> A, IList<double> B)
        {
            return new[]
                       {
                           A[1]*B[3] - A[3]*B[1] + A[2]*B[6] - A[6]*B[2] + A[4]*B[5] - A[5]*B[4],
                           A[2]*B[4] - A[4]*B[2] + A[3]*B[0] - A[0]*B[3] + A[5]*B[6] - A[6]*B[5],
                           A[3]*B[5] - A[5]*B[3] + A[4]*B[1] - A[1]*B[4] + A[6]*B[0] - A[0]*B[6],
                           A[4]*B[6] - A[6]*B[4] + A[5]*B[2] - A[2]*B[5] + A[0]*B[1] - A[1]*B[0],
                           A[5]*B[0] - A[0]*B[5] + A[6]*B[3] - A[3]*B[6] + A[1]*B[2] - A[2]*B[1],
                           A[6]*B[1] - A[1]*B[6] + A[0]*B[4] - A[4]*B[0] + A[2]*B[3] - A[3]*B[2],
                           A[0]*B[2] - A[2]*B[0] + A[1]*B[5] - A[5]*B[1] + A[3]*B[4] - A[4]*B[3]
                       };
        }

        /// <summary>
        /// The cross product of two integer vectors, A and B, which are of length, 7.
        /// This is equivalent to calling multiplyCross, but a slight speed advantage
        /// may exist in skipping directly to this sub-function.
        /// </summary>
        /// <param name = "A">1D integer Array, A</param>
        /// <param name = "B">1D integer Array, B</param>
        /// <returns></returns>
        public static int[] multiplyCross7(IList<int> A, IList<int> B)
        {
            return new[]
                       {
                           A[1]*B[3] - A[3]*B[1] + A[2]*B[6] - A[6]*B[2] + A[4]*B[5] - A[5]*B[4],
                           A[2]*B[4] - A[4]*B[2] + A[3]*B[0] - A[0]*B[3] + A[5]*B[6] - A[6]*B[5],
                           A[3]*B[5] - A[5]*B[3] + A[4]*B[1] - A[1]*B[4] + A[6]*B[0] - A[0]*B[6],
                           A[4]*B[6] - A[6]*B[4] + A[5]*B[2] - A[2]*B[5] + A[0]*B[1] - A[1]*B[0],
                           A[5]*B[0] - A[0]*B[5] + A[6]*B[3] - A[3]*B[6] + A[1]*B[2] - A[2]*B[1],
                           A[6]*B[1] - A[1]*B[6] + A[0]*B[4] - A[4]*B[0] + A[2]*B[3] - A[3]*B[2],
                           A[0]*B[2] - A[2]*B[0] + A[1]*B[5] - A[5]*B[1] + A[3]*B[4] - A[4]*B[3]
                       };
        }

        /// <summary>
        /// The cross product of two double vectors, A and B, which are of length, 3.
        /// This is equivalent to calling multiplyCross, but a slight speed advantage
        /// may exist in skipping directly to this sub-function.
        /// </summary>
        /// <param name = "A">1D double Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns></returns>
        public static double[] multiplyCross3(IList<double> A, IList<double> B)
        {
            return new[]
                       {
                           A[1]*B[2] - B[1]*A[2],
                           A[2]*B[0] - B[2]*A[0],
                           A[0]*B[1] - B[0]*A[1]
                       };
        }
        /// <summary>
        /// The cross product of an integer vector, A, and a double vector, B, which are of length, 7.
        /// This is equivalent to calling multiplyCross, but a slight speed advantage
        /// may exist in skipping directly to this sub-function.
        /// </summary>
        /// <param name = "A">1D integer Array, A</param>
        /// <param name = "B">1D double Array, B</param>
        /// <returns></returns>
        public static double[] multiplyCross3(IList<int> A, IList<double> B)
        {
            return new[]
                       {
                           A[1]*B[2] - B[1]*A[2],
                           A[2]*B[0] - B[2]*A[0],
                           A[0]*B[1] - B[0]*A[1]
                       };
        }
        /// <summary>
        /// The cross product of two integer vectors, A and B, which are of length, 3.
        /// This is equivalent to calling multiplyCross, but a slight speed advantage
        /// may exist in skipping directly to this sub-function.
        /// </summary>
        /// <param name = "A">1D integer Array, A</param>
        /// <param name = "B">1D integer Array, B</param>
        /// <returns></returns>
        public static int[] multiplyCross3(IList<int> A, IList<int> B)
        {
            return new[]
                       {
                           A[1]*B[2] - B[1]*A[2],
                           A[2]*B[0] - B[2]*A[0],
                           A[0]*B[1] - B[0]*A[1]
                       };
        }
        #endregion

        #region Multiply vector by transpose of another vector.
        /// <summary>
        /// Multiply vector by transpose of another vector to create a matrix.
        /// Product of each element of array-1 (1D double) with each element of array-2 (1D double)
        /// C[i,j] = A[i] * B[j]
        /// </summary>
        /// <param name = "A">1D double array - column vector (1 element per row)</param>
        /// <param name = "B">1D double array - row vector (1 element column)</param>
        /// <returns>2D double array product matrix, value of element [i,j] = A[i] * B[j]</returns>
        public static double[,] multiplyVectorsIntoAMatrix(IList<double> A, IList<double> B)
        {
            var CRowSize = A.Count();
            var CColSize = B.Count();

            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i] * B[j];
            return C;
        }
        /// <summary>
        /// Multiply vector by transpose of another vector to create a matrix.
        /// Product of each element of array-1 (1D int) with each element of array-2 (1D double)
        /// C[i,j] = A[i] * B[j]
        /// </summary>
        /// <param name = "A">1D integer array - column vector (1 element per row)</param>
        /// <param name = "B">1D double array - row vector (1 element column)</param>
        /// <returns>2D double array product matrix, value of element [i,j] = A[i] * B[j]</returns>
        public static double[,] multiplyVectorsIntoAMatrix(IList<int> A, IList<double> B)
        {
            var CRowSize = A.Count();
            var CColSize = B.Count();

            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i] * B[j];
            return C;
        }
        /// <summary>
        /// Multiply vector by transpose of another vector to create a matrix.
        /// Product of each element of array-1 (1D int) with each element of array-2 (1D int)
        /// C[i,j] = A[i] * B[j]
        /// </summary>
        /// <param name = "A">1D integer array - column vector (1 element per row)</param>
        /// <param name = "B">1D integer array - row vector (1 element column)</param>
        /// <returns>2D double array product matrix, value of element [i,j] = A[i] * B[j]</returns>
        public static int[,] multiplyVectorsIntoAMatrix(IList<int> A, IList<int> B)
        {
            var CRowSize = A.Count();
            var CColSize = B.Count();

            var C = new int[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i] * B[j];
            return C;
        }
        #endregion

        #region Matrix(2D) to matrix(2D) multiplication
        /// <summary>
        /// Product of two matrices (2D double)
        /// </summary>
        /// <param name = "A">2D double Array, A</param>
        /// <param name = "B">2D double Array, A</param>
        /// <returns>A 2D double array that is the product of the two matrices A and B</returns>
        public static double[,] multiply(double[,] A, double[,] B)
        {
            if (A.GetLength(1) != B.GetLength(0))
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = B.GetLength(1);


            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                {
                    C[i, j] = 0.0;
                    for (var k = 0; k != A.GetLength(1); k++)
                        C[i, j] += A[i, k] * B[k, j];
                }
            return C;
        }

        /// <summary>
        /// Product of two matrices (2D double)
        /// </summary>
        /// <param name = "A">2D int Array, A</param>
        /// <param name = "B">2D double Array, A</param>
        /// <returns>A 2D double array that is the product of the two matrices A and B</returns>
        public static double[,] multiply(int[,] A, double[,] B)
        {
            if (A.GetLength(1) != B.GetLength(0))
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = B.GetLength(1);


            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                {
                    C[i, j] = 0.0;
                    for (var k = 0; k != A.GetLength(1); k++)
                        C[i, j] += A[i, k] * B[k, j];
                }
            return C;
        }
        /// <summary>
        /// Product of two matrices (2D double)
        /// </summary>
        /// <param name = "A">2D double Array, A</param>
        /// <param name = "B">2D int Array, A</param>
        /// <returns>A 2D double array that is the product of the two matrices A and B</returns>
        public static double[,] multiply(double[,] A, int[,] B)
        {
            if (A.GetLength(1) != B.GetLength(0))
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = B.GetLength(1);


            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                {
                    C[i, j] = 0.0;
                    for (var k = 0; k != A.GetLength(1); k++)
                        C[i, j] += A[i, k] * B[k, j];
                }
            return C;
        }

        /// <summary>
        /// Product of two matrices (2D double)
        /// </summary>
        /// <param name = "A">2D int Array, A</param>
        /// <param name = "B">2D int Array, A</param>
        /// <returns>A 2D int array that is the product of the two matrices A and B</returns>
        public static int[,] multiply(int[,] A, int[,] B)
        {
            if (A.GetLength(1) != B.GetLength(0))
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = B.GetLength(1);


            var C = new int[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                {
                    C[i, j] = 0;
                    for (var k = 0; k != A.GetLength(1); k++)
                        C[i, j] += A[i, k] * B[k, j];
                }
            return C;
        }
        #endregion

        #region Multiply matrix to a vector (and vice versa)
        /// <summary>
        /// Product of a matrix and a vector (2D double and 1D double)
        /// </summary>
        /// <param name = "A">2D double Array</param>
        /// <param name = "B">1D double array - column vector (1 element row)</param>
        /// <returns>A 1D double array that is the product of the two matrices A and B</returns>
        public static double[] multiply(double[,] A, IList<double> B)
        {
            // this is B dot term_i multiplication
            var ARowSize = A.GetLength(0);
            var AColSize = A.GetLength(1);
            if (AColSize != B.Count())
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new double[ARowSize];

            for (var i = 0; i != ARowSize; i++)
            {
                C[i] = 0.0;
                for (var j = 0; j != AColSize; j++)
                    C[i] += A[i, j] * B[j];
            }
            return C;
        }

        /// <summary>
        /// Product of a matrix and a vector (2D double and 1D double)
        /// </summary>
        /// <param name = "A">2D int Array</param>
        /// <param name = "B">1D double array - column vector (1 element row)</param>
        /// <returns>A 1D double array that is the product of the two matrices A and B</returns>
        public static double[] multiply(int[,] A, IList<double> B)
        {
            // this is B dot term_i multiplication
            var ARowSize = A.GetLength(0);
            var AColSize = A.GetLength(1);
            if (AColSize != B.Count())
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new double[ARowSize];

            for (var i = 0; i != ARowSize; i++)
            {
                C[i] = 0.0;
                for (var j = 0; j != AColSize; j++)
                    C[i] += A[i, j] * B[j];
            }
            return C;
        }

        /// <summary>
        /// Product of two matrices (2D double and 1D double)
        /// </summary>
        /// <param name = "A">2D double Array</param>
        /// <param name = "B">1D int array - column vector (1 element row)</param>
        /// <returns>A 1D double array that is the product of the two matrices A and B</returns>
        public static double[] multiply(double[,] A, IList<int> B)
        {
            // this is B dot term_i multiplication
            var ARowSize = A.GetLength(0);
            var AColSize = A.GetLength(1);
            if (AColSize != B.Count())
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new double[ARowSize];

            for (var i = 0; i != ARowSize; i++)
            {
                C[i] = 0.0;
                for (var j = 0; j != AColSize; j++)
                    C[i] += A[i, j] * B[j];
            }
            return C;
        }

        /// <summary>
        /// Product of two matrices (2D double and 1D double)
        /// </summary>
        /// <param name = "A">2D int Array</param>
        /// <param name = "B">1D int array - column vector (1 element row)</param>
        /// <returns>A 1D int array that is the product of the two matrices A and B</returns>
        public static int[] multiply(int[,] A, IList<int> B)
        {
            // this is B dot term_i multiplication
            var ARowSize = A.GetLength(0);
            var AColSize = A.GetLength(1);
            if (AColSize != B.Count())
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new int[ARowSize];

            for (var i = 0; i != ARowSize; i++)
            {
                C[i] = 0;
                for (var j = 0; j != AColSize; j++)
                    C[i] += A[i, j] * B[j];
            }
            return C;
        }

        /// <summary>
        /// Product of two matrices (1D double and 2D double)
        /// </summary>
        /// <param name = "A">1D double array - row vector (1 element column)</param>
        /// <param name = "B">2D double Array</param>
        /// <returns>A 1D double array that is the product of the two matrices A and B</returns>
        public static double[] multiply(IList<double> B, double[,] A)
        {
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.Count())
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new double[CColSize];

            for (var i = 0; i != CColSize; i++)
            {
                C[i] = 0.0;
                for (var j = 0; j != CRowSize; j++)
                    C[i] += B[j] * A[j, i];
            }
            return C;
        }
        /// <summary>
        /// Product of two matrices (1D double and 2D double)
        /// </summary>
        /// <param name = "A">1D double array - row vector (1 element column)</param>
        /// <param name = "B">2D int Array</param>
        /// <returns>A 1D double array that is the product of the two matrices A and B</returns>
        public static double[] multiply(IList<double> B, int[,] A)
        {
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.Count())
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new double[CColSize];

            for (var i = 0; i != CColSize; i++)
            {
                C[i] = 0.0;
                for (var j = 0; j != CRowSize; j++)
                    C[i] += B[j] * A[j, i];
            }
            return C;
        }
        /// <summary>
        /// Product of two matrices (1D double and 2D double)
        /// </summary>
        /// <param name = "A">1D int array - row vector (1 element column)</param>
        /// <param name = "B">2D double Array</param>
        /// <returns>A 1D double array that is the product of the two matrices A and B</returns>
        public static double[] multiply(IList<int> B, double[,] A)
        {
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.Count())
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new double[CColSize];

            for (var i = 0; i != CColSize; i++)
            {
                C[i] = 0.0;
                for (var j = 0; j != CRowSize; j++)
                    C[i] += B[j] * A[j, i];
            }
            return C;
        }
        /// <summary>
        /// Product of two matrices (1D double and 2D double)
        /// </summary>
        /// <param name = "A">1D int array - row vector (1 element column)</param>
        /// <param name = "B">2D int Array</param>
        /// <returns>A 1D int array that is the product of the two matrices A and B</returns>
        public static int[] multiply(IList<int> B, int[,] A)
        {
            // this is B dot term_i multiplication
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.Count())
                throw new Exception("Column count in first matrix must be equal to row count in second matrix");

            var C = new int[CColSize];

            for (var i = 0; i != CColSize; i++)
            {
                C[i] = 0;
                for (var j = 0; j != CRowSize; j++)
                    C[i] += B[j] * A[j, i];
            }
            return C;
        }

        #endregion
        #endregion

        #region Add Vector-to-Vector and Matrix-to-Matrix

        /// <summary>
        /// Adds arrays A and B
        /// </summary>
        /// <param name = "A">1D double array 1</param>
        /// <param name = "B">1D double array 2</param>
        /// <returns>1D double array that contains sum of vectros A and B</returns>
        public static double[] add(IList<double> A, IList<double> B)
        {
            // add vector A to vector B
            var size = A.Count();
            if (size != B.Count()) throw new Exception("Matrix sizes do not match");
            var c = new double[size];
            for (var i = 0; i != size; i++)
                c[i] = A[i] + B[i];
            return c;
        }
        /// <summary>
        /// Adds arrays A and B
        /// </summary>
        /// <param name = "A">1D int array 1</param>
        /// <param name = "B">1D double array 2</param>
        /// <returns>1D double array that contains sum of vectros A and B</returns>
        public static double[] add(IList<int> A, IList<double> B)
        {
            // add vector A to vector B
            var size = A.Count();
            if (size != B.Count()) throw new Exception("Matrix sizes do not match");
            var c = new double[size];
            for (var i = 0; i != size; i++)
                c[i] = A[i] + B[i];
            return c;
        }
        /// <summary>
        /// Adds arrays A and B
        /// </summary>
        /// <param name = "A">1D int array 1</param>
        /// <param name = "B">1D int array 2</param>
        /// <returns>1D integer array that contains sum of vectros A and B</returns>
        public static int[] add(IList<int> A, IList<int> B)
        {
            // add vector A to vector B
            var size = A.Count();
            if (size != B.Count()) throw new Exception("Matrix sizes do not match");
            var c = new int[size];
            for (var i = 0; i != size; i++)
                c[i] = A[i] + B[i];
            return c;
        }

        /// <summary>
        /// Adds arrays A and B
        /// </summary>
        /// <param name = "A">2D double array 1</param>
        /// <param name = "B">2D double array 2</param>
        /// <returns>2D double array that contains sum of vectros A and B</returns>
        public static double[,] add(double[,] A, double[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        /// <summary>
        /// Adds arrays A and B
        /// </summary>
        /// <param name = "A">2D double array 1</param>
        /// <param name = "B">2D int array 2</param>
        /// <returns>2D double array that contains sum of vectros A and B</returns>
        public static double[,] add(int[,] A, double[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        /// <summary>
        /// Adds arrays A and B
        /// </summary>
        /// <param name = "A">2D integer array 1</param>
        /// <param name = "B">2D integer array 2</param>
        /// <returns>2D integer array that contains sum of vectros A and B</returns>
        public static int[,] add(int[,] A, int[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new int[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }

        #endregion

        #region Subtract Vector-to-Vector and Matrix-to-Matrix

        /// <summary>
        /// Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend vector, A (1D double)</param>
        /// <param name = "B">The subtrahend vector, B (1D double)</param>
        /// <returns>Returns the difference vector, C (1D double)</returns>
        public static double[] subtract(IList<double> A, IList<double> B)
        {
            // add vector A to vector B
            var size = A.Count();
            if (size != B.Count())
                throw new Exception("Matrix sizes do not match");
            var c = new double[size];
            for (var i = 0; i != size; i++)
                c[i] = A[i] - B[i];
            return c;
        }
        /// <summary>
        /// Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend vector, A (1D int)</param>
        /// <param name = "B">The subtrahend vector, B (1D double)</param>
        /// <returns>Returns the difference vector, C (1D double)</returns>
        public static double[] subtract(IList<int> A, IList<double> B)
        {
            // add vector A to vector B
            var size = A.Count();
            if (size != B.Count())
                throw new Exception("Matrix sizes do not match");
            var c = new double[size];
            for (var i = 0; i != size; i++)
                c[i] = A[i] - B[i];
            return c;
        }
        /// <summary>
        /// Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend vector, A (1D double)</param>
        /// <param name = "B">The subtrahend vector, B (1D int)</param>
        /// <returns>Returns the difference vector, C (1D double)</returns>
        public static double[] subtract(IList<double> A, IList<int> B)
        {
            // add vector A to vector B
            var size = A.Count();
            if (size != B.Count())
                throw new Exception("Matrix sizes do not match");
            var c = new double[size];
            for (var i = 0; i != size; i++)
                c[i] = A[i] - B[i];
            return c;
        }
        /// <summary>
        /// Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend vector, A (1D int)</param>
        /// <param name = "B">The subtrahend vector, B (1D int)</param>
        /// <returns>Returns the difference vector, C (1D int)</returns>
        public static int[] subtract(IList<int> A, IList<int> B)
        {
            // add vector A to vector B
            var size = A.Count();
            if (size != B.Count())
                throw new Exception("Matrix sizes do not match");
            var c = new int[size];
            for (var i = 0; i != size; i++)
                c[i] = A[i] - B[i];
            return c;
        }

        /// <summary>
        /// Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend matrix, A (2D double).</param>
        /// <param name = "B">The subtrahend matrix, B (2D double).</param>
        /// <returns>Returns the difference matrix, C (2D double)</returns>
        public static double[,] subtract(double[,] A, double[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }
        /// <summary>
        /// Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend matrix, A (2D int).</param>
        /// <param name = "B">The subtrahend matrix, B (2D double).</param>
        /// <returns>Returns the difference matrix, C (2D double)</returns>
        public static double[,] subtract(int[,] A, double[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }
        /// <summary>
        /// Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend matrix, A (2D double).</param>
        /// <param name = "B">The subtrahend matrix, B (2D int).</param>
        /// <returns>Returns the difference matrix, C (2D double)</returns>
        public static double[,] subtract(double[,] A, int[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new double[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }

        /// <summary>
        /// Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend matrix, A (2D integer).</param>
        /// <param name = "B">The subtrahend matrix, B (2D integer).</param>
        /// <returns>Returns the difference matrix, C (2D integer)</returns>
        public static int[,] subtract(int[,] A, int[,] B)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new int[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }


        /// <summary>
        /// Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name = "A">The minuend matrix, A (2D integer).</param>
        /// <param name = "B">The subtrahend matrix, B (2D integer).</param>
        /// <returns>Returns the difference matrix, C (2D integer)</returns>
        public static int[,] subtract(int[,] A, int[,] B, int numRows, int numCols)
        {
            var CRowSize = A.GetLength(0);
            var CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0))
                throw new Exception("Matrix row count do not match");
            if (CColSize != B.GetLength(1))
                throw new Exception("Matrix column count do not match");

            var C = new int[CRowSize, CColSize];

            for (var i = 0; i != CRowSize; i++)
                for (var j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }

     

        #endregion

        #region Sum

        /// <summary>
        /// Sum up all the elements of a given matrix
        /// </summary>
        /// <param name = "B">Matrix (1D double) whose parameters need to be summed up</param>
        /// <returns>Returns the total (double) </returns>
        public static double sum(IEnumerable<double> B)
        {
            return B.Sum();
        }

        /// <summary>
        /// Sum up all the elements of a given matrix
        /// </summary>
        /// <param name = "B">Matrix (1D int) whose parameters need to be summed up</param>
        /// <returns>Returns the total (int) </returns>
        public static double sum(IEnumerable<int> B)
        {
            return B.Sum();
        }

        /// <summary>
        /// Sum up all the elements of a given matrix
        /// </summary>
        /// <param name = "B">Matrix (2D double) whose parameters need to be summed up</param>
        /// <returns>Returns the total (double) </returns>
        public static double sum(double[,] B)
        {
            return JoinMatrixColumnsIntoVector(B).Sum();
        }

        /// <summary>
        /// Sum up all the elements of a given matrix
        /// </summary>
        /// <param name = "B">Matrix (2D double) whose parameters need to be summed up</param>
        /// <returns>Returns the total (int) </returns>
        public static double sum(int[,] B)
        {
            return JoinMatrixColumnsIntoVector(B).Sum();
        }
        #endregion

        #endregion
    }
}