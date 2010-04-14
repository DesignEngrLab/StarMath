using System;

namespace StarMathLib
{
    // note this is set to public for testing purposes only - remove when done.
    public static partial class StarMath
    {
        #region 3D Coordinate Transforms

        public static double[,] translate(double Tx, double Ty, double Tz)
        {
            double[,] T = makeIdentity(4);

            T[0, 3] = Tx;
            T[1, 3] = Ty;
            T[2, 3] = Tz;

            return T;
        }

        public static double[,] rotationX(double xdeg)
        {
            double[,] ROTX = makeIdentity(4);
            double xrad;
            xrad = (Math.PI * xdeg) / 180;

            ROTX[1, 1] = ROTX[2, 2] = Math.Cos(xrad);
            ROTX[1, 2] = -Math.Sin(xrad);
            ROTX[2, 1] = Math.Sin(xrad);

            return ROTX;
        }

        public static double[,] rotationY(double ydeg)
        {
            double[,] ROTY = makeIdentity(4);
            double yrad;
            yrad = (Math.PI * ydeg) / 180;

            ROTY[0, 0] = ROTY[2, 2] = Math.Cos(yrad);
            ROTY[2, 0] = -Math.Sin(yrad);
            ROTY[0, 2] = Math.Sin(yrad);

            return ROTY;
        }

        public static double[,] rotationZ(double zdeg)
        {
            double[,] ROTZ = makeIdentity(4);
            double zrad;
            zrad = (Math.PI * zdeg) / 180;

            ROTZ[0, 0] = ROTZ[1, 1] = Math.Cos(zrad);
            ROTZ[1, 0] = Math.Sin(zrad);
            ROTZ[0, 1] = -Math.Sin(zrad);

            return ROTZ;
        }
        #endregion
    }
}

