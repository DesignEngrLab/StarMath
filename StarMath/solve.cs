/*************************************************************************
 *     This file & class is part of the StarMath Project
 *     Copyright 2010, 2011 Matthew Ira Campbell, PhD.
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
        /// <summary>
        /// Solves the specified A matrix.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="b">The b.</param>
        /// <param name="initialGuess">The initial guess.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Matrix, A, must be square.
        /// or
        /// Matrix, A, must be have the same number of rows as the vector, b.</exception>
        public static double[] solve(double[,] A, IList<double> b, IList<double> initialGuess = null)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new Exception("Matrix, A, must be square.");
            if (length != b.Count)
                throw new Exception("Matrix, A, must be have the same number of rows as the vector, b.");
            List<int>[] potentialIndices;
            if (isGaussSeidelAppropriate(A, out potentialIndices, length))
                return solveGaussSeidel(A, b, initialGuess, length, potentialIndices);
            /****** need code to determine when to switch between *****
             ****** this analytical approach and the SOR approach *****/
            return solveByInverse(A, b);
        }

        /// <summary>
        /// Solves the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="b">The b.</param>
        /// <param name="initialGuess">The initial guess.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Matrix, A, must be square.
        /// or
        /// Matrix, A, must be have the same number of rows as the vector, b.
        /// </exception>
        public static double[] solve(int[,] A, IList<double> b, IList<double> initialGuess = null)
        {
            var length = A.GetLength(0);
            if (length != A.GetLength(1))
                throw new Exception("Matrix, A, must be square.");
            if (length != b.Count)
                throw new Exception("Matrix, A, must be have the same number of rows as the vector, b.");

            var B = new double[length, length];
            for (int i = 0; i < length; i++)
                for (int j = 0; j < length; j++)
                    B[i, j] = A[i, j];
            return solve(B, b, initialGuess);
        }
        /// <summary>
        /// Solves the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="b">The b.</param>
        /// <param name="initialGuess">The initial guess.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Matrix, A, must be square.
        /// or
        /// Matrix, A, must be have the same number of rows as the vector, b.
        /// </exception>
        public static double[] solve(double[,] A, IList<int> b, IList<double> initialGuess = null)
        {
            return solve(A, b.Select(Convert.ToDouble).ToArray(), initialGuess);
        }
        /// <summary>
        /// Solves the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <param name="b">The b.</param>
        /// <param name="initialGuess">The initial guess.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Matrix, A, must be square.
        /// or
        /// Matrix, A, must be have the same number of rows as the vector, b.
        /// </exception>
        public static double[] solve(int[,] A, IList<int> b, IList<double> initialGuess = null)
        {
            return solve(A, b.Select(Convert.ToDouble).ToArray(), initialGuess);
        }

        public static double[] solveByInverse(double[,] A, IList<double> b)
        {
            return multiply(inverse(A), b);
        }

        // Gauss-Seidel is not working. It only seems to diverge. I can't figure out the problem. todo bug
        #region Gauss-Seidel or Successive Over-Relaxation

        private static bool isGaussSeidelAppropriate(double[,] A, IList<double> b, IList<double> initialGuess, int length)
        {
            //return false; /* Gauss-Seidel is never appropriate! At least not until we fix it. Only seems to diverge. */

            ifInitialGuessIsNull(ref initialGuess, A, b, length);
            var old_b = multiply(A, initialGuess);
            var error = 0.0;
            for (int i = 0; i < length; i++)
                error += Math.Abs(old_b[i] - b[i]) / (Math.Abs(old_b[i]) + Math.Abs(b[i]) + 1e-6);
            if (error > MaxErrorForUsingGaussSeidel) return false;
            var numZeros = 0;
            for (int i = 0; i < length; i++)
                for (int j = 0; j < length; j++)
                    if (A[i, j] == 0.0) numZeros++;
            var nonZeroFraction = 1 - (numZeros / (length * length));
            if (nonZeroFraction > MaxFractionOfZeroesForGaussSeidel) return false;
            return true;
        }
        private static bool isGaussSeidelAppropriate(double[,] A, out List<int>[] potentialRows, int length)
        {
            potentialRows = new List<int>[length];
            if (length < GaussSeidelMinimumMatrixSize) return false;
            for (int i = 0; i < length; i++)
            {
                var rowNorm1 = A.GetRow(i).norm1();
                var potentialIndices = new List<int>();
                for (int j = 0; j < length; j++)
                    if (Math.Abs(A[i, j]) / (rowNorm1 - Math.Abs(A[i, j])) < GaussSeidelDiagonalDominanceRatio)
                        potentialIndices.Add(j);
                if (potentialIndices.Count == 0) return false;
                potentialRows[i] = potentialIndices;
            }
            return potentialRows.SelectMany(x => x).Distinct().Count() == length;
        }

        public static double[] solveGaussSeidel(double[,] A, IList<double> b,
            IList<double> initialGuess = null, int length = 0, List<int>[] potentialIndices = null)
        {
            if (length <= 0) length = b.Count;
            ifInitialGuessIsNull(ref initialGuess, A, b, length);
            if (potentialIndices == null) potentialIndices = reorderMatrixForDiagonalDominance(A, length);
            // todo: need to rewrite this reorder function so that it works like it does in PMKS
            var C = A;
            double[] d = b.ToArray();
            var x = new double[length];
            List<int> order = null;
            var reorderMatrix = needToReorder(A, length);
            if (reorderMatrix)
            {
                order = reorderMatrixForDiagonalDominance(A, length);
                for (int i = 0; i < length; i++)
                {
                    d[i] = b[order[i]];
                    SetRow(i, C, GetRow(order[i], A));
                }
            }
            for (int i = 0; i < length; i++)
                x[i] = initialGuess[i];

            var cNorm1 = norm1(d);
            var error = norm1(subtract(d, multiply(C, x, length, length), length)) / cNorm1;
            var success = error <= GaussSeidelMaxError;
            var xWentNaN = false;
            var iteration = length * length * GaussSeidelMaxIterationFactor;
            while (!xWentNaN && !success && iteration-- > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    var adjust = d[i];
                    for (int j = 0; j < length; j++)
                        if (i != j)
                            adjust -= C[i, j] * x[j];
                    x[i] = (1 - GaussSeidelRelaxationOmega) * x[i] +
                           GaussSeidelRelaxationOmega * adjust / C[i, i];
                }
                xWentNaN = x.Any(double.IsNaN);
                error = norm1(subtract(d, multiply(C, x, length, length), length)) / cNorm1;
                success = error <= GaussSeidelMaxError;
            }
            if (!success) return null;

            return x;
        }

        private static void ifInitialGuessIsNull(ref IList<double> initialGuess, double[,] A, IList<double> b, int length)
        {
            if (initialGuess == null)
            {
                initialGuess = new double[length];
                var initGuessValue = sum(b) / sum(A);
                for (int i = 0; i < length; i++) initialGuess[i] = initGuessValue;
            }
        }

        private static List<int> reorderMatrixForDiagonalDominance(double[,] A, int length)
        {
            var order = new List<int>(length);

            for (int i = 0; i < length; i++)
            {
                var maxValue = double.NegativeInfinity;
                var maxIndex = -1;
                for (int j = 0; j < length; j++)
                {
                    if (!order.Contains(j) && Math.Abs(A[j, i]) > maxValue)
                    {
                        maxValue = A[j, i];
                        maxIndex = j;
                    }
                }
                order.Add(maxIndex);
            }
            return order;
        }

        private static bool needToReorder(double[,] A, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (A[i, i] == 0.0) return true;
                var rowSum = 0.0;
                for (int j = 0; j < length; j++)
                    if (i != j)
                        rowSum += Math.Abs(A[i, j]);
                if (Math.Abs(A[i, i] / rowSum) < GaussSeidelDiagonalDominanceRatio)
                    return true;
            }
            return false;
        }
        #endregion
    }
}