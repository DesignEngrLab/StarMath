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
        #region Add, Subtract, and Multiply functions
        #region Multiplication of Scalars, Vectors, and Matrices
        /// <summary>
        /// Multiplies the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[] multiply(double a, double[] B)
        {
            // scale vector B by the amount of scalar B
            int size = B.GetLength(0);
            double[] c = new double[size];
            for (int i = 0; i != size; i++)
                c[i] = a * B[i];
            return c;
        }
        /// <summary>
        /// Multiplies the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[,] multiply(double a, double[,] B)
        {
            double[,] c = new double[B.GetLength(0), B.GetLength(1)];
            for (int i = 0; i != B.GetLength(0); i++)
                for (int j = 0; j != B.GetLength(1); j++)
                    c[i, j] = a * B[i, j];
            return c;
        }
        /// <summary>
        /// Multiplies the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[,] multiply(double a, int[,] B)
        {
            double[,] c = new double[B.GetLength(0), B.GetLength(1)];
            for (int i = 0; i != B.GetLength(0); i++)
                for (int j = 0; j != B.GetLength(1); j++)
                    c[i, j] = a * B[i, j];
            return c;
        }
        /// <summary>
        /// Multiplies the dot.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double multiplyDot(double[] A, double[] B)
        {
            // this is B dot term_i multiplication
            int size = A.GetLength(0);
            if (size != B.GetLength(0)) return double.NaN;
            double c = 0.0;
            for (int i = 0; i != size; i++)
                c += A[i] * B[i];
            return c;
        }
        /// <summary>
        /// Multiplies the cross.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[] multiplyCross(double[] A, double[] B)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Multiplies the vectors into A matrix.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[,] multiplyVectorsIntoAMatrix(double[] A, double[] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = B.GetLength(0);

            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i] * B[j];
            return C;
        }
        /// <summary>
        /// Multiplies the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[,] multiply(double[,] A, double[,] B)
        {
            if (A.GetLength(1) != B.GetLength(0))
                return null;
            // this is B dot term_i multiplication
            int CRowSize = A.GetLength(0);
            int CColSize = B.GetLength(1);


            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                {
                    C[i, j] = 0.0;
                    for (int k = 0; k != A.GetLength(1); k++)
                        C[i, j] += A[i, k] * B[k, j];
                }
            return C;
        }
        /// <summary>
        /// Multiplies the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[] multiply(double[,] A, double[] B)
        {
            // this is B dot term_i multiplication
            int ARowSize = A.GetLength(0);
            int AColSize = A.GetLength(1);
            if (AColSize != B.GetLength(0)) return null;

            double[] C = new double[ARowSize];

            for (int i = 0; i != ARowSize; i++)
            {
                C[i] = 0.0;
                for (int j = 0; j != AColSize; j++)
                    C[i] += A[i, j] * B[j];
            }
            return C;
        }
        /// <summary>
        /// Multiplies the specified B.
        /// </summary>
        /// <param name="B">The B.</param>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static double[] multiply(double[] B, double[,] A)
        {
            // this is B dot term_i multiplication
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0)) return null;

            double[] C = new double[CColSize];

            for (int i = 0; i != CColSize; i++)
            {
                C[i] = 0.0;
                for (int j = 0; j != CRowSize; j++)
                    C[i] += B[j] * A[j, i];
            }
            return C;
        }
        #endregion

        #region Add Vector-to-Vector and Matrix-to-Matrix
        /// <summary>
        /// Adds the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[] add(double[] A, double[] B)
        {
            // add vector A to vector B
            int size = A.GetLength(0);
            if (size != B.GetLength(0)) return null;
            double[] c = new double[size];
            for (int i = 0; i != size; i++)
                c[i] = A[i] + B[i];
            return c;
        }
        /// <summary>
        /// Adds the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[,] add(double[,] A, double[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }
        /// <summary>
        /// Adds the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static double[,] add(double[,] A, int[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }
        /// <summary>
        /// Adds the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="B">The B.</param>
        /// <returns></returns>
        public static int[,] add(int[,] A, int[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            int[,] C = new int[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }
        #endregion

        #region Subtract Vector-to-Vector and Matrix-to-Matrix
        /// <summary>
        /// Subtracts one vector (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend vector, A.</param>
        /// <param name="B">The subtrahend vector, B.</param>
        /// <returns>Returns the difference vector, C</returns>
        public static double[] subtract(double[] A, double[] B)
        {
            // add vector A to vector B
            int size = A.GetLength(0);
            if (size != B.GetLength(0)) return null;
            double[] c = new double[size];
            for (int i = 0; i != size; i++)
                c[i] = A[i] - B[i];
            return c;
        }
        /// <summary>
        /// Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend matrix, A.</param>
        /// <param name="B">The subtrahend matrix, B.</param>
        /// <returns>Returns the difference matrix, C</returns>
        public static double[,] subtract(double[,] A, double[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }
        /// <summary>
        /// Subtracts one matrix (B) from the other (A). C = A - B.
        /// </summary>
        /// <param name="A">The minuend matrix, A.</param>
        /// <param name="B">The subtrahend matrix, B.</param>
        /// <returns>Returns the difference matrix, C</returns>
        public static int[,] subtract(int[,] A, int[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            int[,] C = new int[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }
        #endregion
        #endregion
    }
}

