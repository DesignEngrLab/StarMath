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


namespace StarMathLib
{
    public static partial class StarMath
    {
        private const int cellWidth = 10;
        private const int numDecimals = 3;

        private const double EqualityTolerance = 1e-15;
        private const double MaxErrorForUsingGaussSeidel = 3.0;
        private const double GaussSeidelMaxError = 1e-12;
        private const double GaussSeidelDiagonalDominanceRatio = 0.3;
        /* in some simple studies, GaussSeidel failed when ALL diagonals were lower than 0.3 (and higher than -0.5)
         * so it may seem imprudent to set the diagonal dominance ratio so high. But this is only to throw out
         * cases in which ANY of the diagonals are lower than this value. */
        private const int GaussSeidelMinimumMatrixSize = 55;
        private const int GaussSeidelMaxIterationFactor = 1;
        private const double GaussSeidelRelaxationOmega = 1.2;

        const int maxSVDiter = 1000;

        private static readonly Random random = new Random();
        private const int StartingCarolSeed = 3;



        /// <summary>
        /// Determines whether [is practically same] [the specified x].     
        /// the norm is within 1e-15
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool IsPracticallySame(double x, double y)
        {
            return IsNegligible(x - y);
        }

        /// <summary>
        /// Determines whether [is practically same] [the specified x].
        /// the norm is within 1e-15
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool IsPracticallySame(double[] x, double[] y)
        {
            var n = x.GetLength(0);
            if (n != y.GetLength(0)) return false;
            return IsNegligible(x.subtract(y));
        }

        /// <summary>
        /// Determines whether the specified x is negligible (|x| lte 1e-15).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static bool IsNegligible(double[] x)
        {
            return (x.norm2(true) <= EqualityTolerance);
        }

        /// <summary>
        /// Determines whether the specified x is negligible (|x| lte 1e-15).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns></returns>
        public static bool IsNegligible(double x)
        {
            return (Math.Abs(x) <= EqualityTolerance);
        }

    }
}