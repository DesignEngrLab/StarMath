// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="3D transforms.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Runtime.CompilerServices;

namespace StarMathLib
{
    public static partial class StarMath
    {
        #region 3D Coordinate Transforms

        /// <summary>
        ///     Creates a translated coordinate frame.
        /// </summary>
        /// <param name="tx">Amount of translation in x.</param>
        /// <param name="ty">Amount of translation in y.</param>
        /// <param name="tz">Amount of translation in z.</param>
        /// <returns>4-by-4 matrix translated by the amount specified.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public static float[,] TranslateFloat(float tx, float ty, float tz)
        {
            var T = makeIdentityFloat(4);

            T[0, 3] = tx;
            T[1, 3] = ty;
            T[2, 3] = tz;

            return T;
        }

        /// <summary>
        ///     Creates a rotation matrix about the X-axis.
        /// </summary>
        /// <param name="angle">The amount of angle in degrees (positive is counter-clockwise).</param>
        /// <param name="inRadians">if set to <c>true</c> [in radians].</param>
        /// <returns>4-by-4 matrix rotated by the amount specified.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[,] RotationXFloat(float angle, bool inRadians = false)
        {
            var rotx = makeIdentityFloat(4);
            if (!inRadians)
                angle = MathF.PI * angle / 180f;

            rotx[1, 1] = rotx[2, 2] = MathF.Cos(angle);
            rotx[2, 1] = MathF.Sin(angle);
            rotx[1, 2] = -rotx[2, 1];

            return rotx;
        }

        /// <summary>
        ///     Creates a rotation matrix about the Y-axis.
        /// </summary>
        /// <param name="angle">The amount of angle in degrees (positive is counter-clockwise).</param>
        /// <param name="inRadians">if set to <c>true</c> [in radians].</param>
        /// <returns>4-by-4 matrix rotated by the amount specified.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public static float[,] RotationYFloat(float angle, bool inRadians = false)
        {
            var roty = makeIdentityFloat(4);
            if (!inRadians)
                angle = MathF.PI * angle / 180f;

            roty[0, 0] = roty[2, 2] = MathF.Cos(angle);
            roty[0, 2] = MathF.Sin(angle);
            roty[2, 0] = -roty[0, 2];

            return roty;
        }

        /// <summary>
        ///     Creates a rotation matrix about the Z-axis.
        /// </summary>
        /// <param name="angle">The amount of angle in degrees (positive is counter-clockwise).</param>
        /// <param name="inRadians">if set to <c>true</c> [in radians].</param>
        /// <returns>4-by-4 matrix rotated by the amount specified.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public static float[,] RotationZFloat(float angle, bool inRadians = false)
        {
            var rotz = makeIdentityFloat(4);
            if (!inRadians)
                angle = MathF.PI * angle / 180f;

            rotz[0, 0] = rotz[1, 1] = MathF.Cos(angle);
            rotz[1, 0] = MathF.Sin(angle);
            rotz[0, 1] = -rotz[1, 0];

            return rotz;
        }

        #endregion
    }
}