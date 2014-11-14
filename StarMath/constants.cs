﻿/*************************************************************************
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
        private const double MaxErrorForUsingGaussSeidel = 0.5;
        private const double MaxFractionOfZeroesForGaussSeidel = 0.7;
        private const double GaussSeidelMaxError = 1e-10;
        private const double GaussSeidelDiagonalDominanceRatio = 0.5;
        private const int GaussSeidelMaxIterationFactor = 1;
        private const double GaussSeidelRelaxationOmega = 1.0;

        const int maxSVDiter = 1000;

    }
}