#region

using System;

#endregion

namespace StarMathLib
{
    public static partial class StarMath
    {
        #region 3D Coordinate Transforms

        /// <summary>
        ///   Creates a translated coordinate frame.
        /// </summary>
        /// <param name = "Tx">Amount of translation in x.</param>
        /// <param name = "Ty">Amount of translation in y.</param>
        /// <param name = "Tz">Amount of translation in z.</param>
        /// <returns>4-by-4 matrix translated by the amount specified.</returns>
        public static double[,] Translate(double Tx, double Ty, double Tz)
        {
            var T = makeIdentity(4);

            T[0, 3] = Tx;
            T[1, 3] = Ty;
            T[2, 3] = Tz;

            return T;
        }

        /// <summary>
        ///   Creates a rotation matrix about the X-axis.
        /// </summary>
        /// <param name = "xdeg">The amount of angle in degrees (positive is counter-clockwise).</param>
        /// <returns>4-by-4 matrix rotated by the amount specified.</returns>
        public static double[,] RotationX(double xdeg)
        {
            var ROTX = makeIdentity(4);
            var xrad = (Math.PI*xdeg)/180;

            ROTX[1, 1] = ROTX[2, 2] = Math.Cos(xrad);
            ROTX[1, 2] = -Math.Sin(xrad);
            ROTX[2, 1] = Math.Sin(xrad);

            return ROTX;
        }

        /// <summary>
        ///   Creates a rotation matrix about the Y-axis.
        /// </summary>
        /// <param name = "ydeg">The amount of angle in degrees (positive is counter-clockwise).</param>
        /// <returns>4-by-4 matrix rotated by the amount specified.</returns>
        public static double[,] RotationY(double ydeg)
        {
            var ROTY = makeIdentity(4);
            var yrad = (Math.PI*ydeg)/180;

            ROTY[0, 0] = ROTY[2, 2] = Math.Cos(yrad);
            ROTY[2, 0] = -Math.Sin(yrad);
            ROTY[0, 2] = Math.Sin(yrad);

            return ROTY;
        }

        /// <summary>
        ///   Creates a rotation matrix about the Z-axis.
        /// </summary>
        /// <param name = "zdeg">The amount of angle in degrees (positive is counter-clockwise).</param>
        /// <returns>4-by-4 matrix rotated by the amount specified.</returns>
        public static double[,] RotationZ(double zdeg)
        {
            var ROTZ = makeIdentity(4);
            var zrad = (Math.PI*zdeg)/180;

            ROTZ[0, 0] = ROTZ[1, 1] = Math.Cos(zrad);
            ROTZ[1, 0] = Math.Sin(zrad);
            ROTZ[0, 1] = -Math.Sin(zrad);

            return ROTZ;
        }

        #endregion

    }
}