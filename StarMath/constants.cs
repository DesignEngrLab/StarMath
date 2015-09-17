// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="constants.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
namespace StarMathLib
{
    /// <summary>
    /// The one and only class in the StarMathLib. All functions are static
    /// functions located here.
    /// </summary>
    public static partial class StarMath
    {
        /// <summary>
        ///     The cell width
        /// </summary>
        private const int cellWidth = 10;

        /// <summary>
        ///     The number decimals
        /// </summary>
        private const int numDecimals = 3;

        /// <summary>
        ///     The maximum error for using gauss seidel
        /// </summary>
        private const double MaxErrorForUsingGaussSeidel = 3.0;

        /// <summary>
        ///     The gauss seidel maximum error
        /// </summary>
        private const double GaussSeidelMaxError = 1e-12;

        /// <summary>
        ///     The gauss seidel diagonal dominance ratio
        /// </summary>
        private const double GaussSeidelDiagonalDominanceRatio = 0.3;

        /* in some simple studies, GaussSeidel failed when ALL diagonals were lower than 0.3 (and higher than -0.5)
         * so it may seem imprudent to set the diagonal dominance ratio so high. But this is only to throw out
         * cases in which ANY of the diagonals are lower than this value. */

        /// <summary>
        ///     The gauss seidel minimum matrix size
        /// </summary>
        private const int GaussSeidelMinimumMatrixSize = 200;

        /// <summary>
        ///     The gauss seidel maximum iteration factor
        /// </summary>
        private const int GaussSeidelMaxIterationFactor = 1;

        /// <summary>
        ///     The gauss seidel relaxation omega
        /// </summary>
        private const double GaussSeidelRelaxationOmega = 1.2;

        /// <summary>
        ///     The maximum sv diter
        /// </summary>
        private const int maxSVDiter = 1000;

        /// <summary>
        ///     The starting carol seed
        /// </summary>
        private const int StartingCarolSeed = 3;

        /// <summary>
        ///     The random
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        ///     The _equality tolerance
        /// </summary>
        private static double _equalityTolerance = 1e-15;

        /// <summary>
        ///     Gets or sets the  tolerance for the equality functions: IsPracticallySame, IsNegligible, IsGreaterThanNonNegligible
        ///     IsLessThanNonNegligible.
        /// </summary>
        /// <value>The equality tolerance.</value>
        public static double EqualityTolerance
        {
            get { return _equalityTolerance; }
            set { _equalityTolerance = value; }
        }

        /// <summary>
        ///     Determines whether [is practically same] [the specified x].
        ///     the norm is within 1e-15
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns><c>true</c> if [is practically same] [the specified x]; otherwise, <c>false</c>.</returns>
        public static bool IsPracticallySame(this double x, double y)
        {
            return IsNegligible(x - y);
        }

        /// <summary>
        ///     Determines whether [is practically same] [the specified x].
        ///     the norm is within 1e-15
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns><c>true</c> if [is practically same] [the specified x]; otherwise, <c>false</c>.</returns>
        public static bool IsPracticallySame(this double[] x, double[] y)
        {
            var n = x.GetLength(0);
            if (n != y.GetLength(0)) return false;
            return IsNegligible(x.subtract(y));
        }

        /// <summary>
        ///     Determines whether the specified x is negligible (|x| lte 1e-15).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if the specified x is negligible; otherwise, <c>false</c>.</returns>
        public static bool IsNegligible(this double[] x)
        {
            return (x.norm2(true) <= EqualityTolerance);
        }

        /// <summary>
        ///     Determines whether the specified x is negligible (|x| lte 1e-15).
        /// </summary>
        /// <param name="x">The x.</param>
        /// <returns><c>true</c> if the specified x is negligible; otherwise, <c>false</c>.</returns>
        public static bool IsNegligible(this double x)
        {
            return (Math.Abs(x) <= EqualityTolerance);
        }

        /// <summary>
        ///     Determines whether [is greater than] [the specified y] and not practically the same.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns><c>true</c> if [is greater than non negligible] [the specified y]; otherwise, <c>false</c>.</returns>
        public static bool IsGreaterThanNonNegligible(this double x, double y = 0)
        {
            return (x > y && !IsPracticallySame(x, y));
        }

        /// <summary>
        ///     Determines whether [is less than] [the specified y] and not practically the same.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns><c>true</c> if [is less than non negligible] [the specified y]; otherwise, <c>false</c>.</returns>
        public static bool IsLessThanNonNegligible(this double x, double y = 0)
        {
            return (x < y && !IsPracticallySame(x, y));
        }
    }
}