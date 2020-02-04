// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-16-2015
// ***********************************************************************
// <copyright file="add subtract multiply.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace StarMathLib
{
    public static partial class StarMath
    {
        /// <summary>
        /// Product of two matrices (2D double and a SparseMatrix)
        /// </summary>
        /// <param name="A">a.</param>
        /// <param name="B">The b.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static double[,] multiply(this double[,] A, SparseMatrix B)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Product of two matrices (2D double and a SparseMatrix), which results in a 
        /// new sparse matrix.
        /// </summary>
        /// <param name="A">a.</param>
        /// <param name="B">The b.</param>
        /// <returns>SparseMatrix.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static SparseMatrix multiplyToSparse(this double[,] A, SparseMatrix B)
        {
            throw new NotImplementedException();
        }
    }
}
