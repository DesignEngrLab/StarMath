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
using System.Collections.Generic;

namespace StarMathLib
{
    public static partial class StarMath
    {
        /// <summary>
        /// Solves the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static double[] solve(double[,] A, IList<double> b)
        {
            if (A.GetLength(0) != A.GetLength(1))
                throw new Exception("Matrix, A, must be square.");
            if (A.GetLength(0) != b.Count)
                throw new Exception("Matrix, A, must be have the same number of rows as the vector, b.");

            /****** need code to determine when to switch between *****
             ****** this analytical approach and the SOR approach *****/
            return multiply(inverse(A), b);
        }
        /// <summary>
        /// Solves the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static double[] solve(int[,] A, IList<double> b)
        {
            if (A.GetLength(0) != A.GetLength(1))
                throw new Exception("Matrix, A, must be square.");
            if (A.GetLength(0) != b.Count)
                throw new Exception("Matrix, A, must be have the same number of rows as the vector, b.");

            /****** need code to determine when to switch between *****
             ****** this analytical approach and the SOR approach *****/
            return multiply(inverse(A), b);
        }
        /// <summary>
        /// Solves the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static double[] solve(double[,] A, IList<int> b)
        {
            if (A.GetLength(0) != A.GetLength(1))
                throw new Exception("Matrix, A, must be square.");
            if (A.GetLength(0) != b.Count)
                throw new Exception("Matrix, A, must be have the same number of rows as the vector, b.");

            /****** need code to determine when to switch between *****
             ****** this analytical approach and the SOR approach *****/
            return multiply(inverse(A), b);
        }
        /// <summary>
        /// Solves the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static double[] solve(int[,] A, IList<int> b)
        {
            if (A.GetLength(0) != A.GetLength(1))
                throw new Exception("Matrix, A, must be square.");
            if (A.GetLength(0) != b.Count)
                throw new Exception("Matrix, A, must be have the same number of rows as the vector, b.");

            /****** need code to determine when to switch between *****
             ****** this analytical approach and the SOR approach *****/
            return multiply(inverse(A), b);
        }
    }
}