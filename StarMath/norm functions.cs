using System;

namespace StarMathLib
{
    // note this is set to public for testing purposes only - remove when done.
    public static partial class StarMath
    {
        #region Norm Functions
        public static double norm1(double[] x, double[] y)
        {

            if ((x == null) || (y == null)) return double.NaN;
            double norm = 0.0;
            double xTerm, yTerm;
            int maxlength = x.GetLength(0) > y.GetLength(0) ? x.GetLength(0) : y.GetLength(0);

            for (int i = 0; i != maxlength; i++)
            {
                if (i >= x.GetLength(0)) xTerm = 0.0;
                else xTerm = x[i];
                if (i >= y.GetLength(0)) yTerm = 0.0;
                else yTerm = y[i];
                norm += Math.Abs(xTerm - yTerm);
            }
            return norm;
        }
        public static double norm1(double[] x)
        {
            if (x == null) return double.NaN;
            double norm = 0.0;
            int xlength = x.GetLength(0);
            for (int i = 0; i != xlength; i++)
                norm += Math.Abs(x[i]);
            return norm;
        }
        public static double norm1(double[,] x)
        {
            if (x == null) return double.NaN;
            double norm = 0.0;
            int rowlength = x.GetLength(0);
            int collength = x.GetLength(0);
            for (int i = 0; i != rowlength; i++)
                for (int j = 0; j != collength; j++)
                    norm += Math.Abs(x[i, j]);
            return norm;
        }
        public static double norm2(double[] x, double[] y)
        {
            double norm = 0.0;
            int xlength = x.GetLength(0);
            if (xlength != y.GetLength(0)) return -1.0;
            else
                for (int i = 0; i != xlength; i++)
                    norm += (x[i] - y[i]) * (x[i] - y[i]);
            return Math.Sqrt(norm);
        }
        public static double norm2(double[] x)
        {
            double norm = 0.0;
            int xlength = x.GetLength(0);
            for (int i = 0; i != xlength; i++)
                norm += (x[i] * x[i]);
            return Math.Sqrt(norm);
        }
        public static double norm2(double[,] A)
        {
            double norm = 0.0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            for (int i = 0; i != maxRow; i++)
                for (int j = 0; j != maxCol; j++)
                    norm += (A[i, j] * A[i, j]);
            return Math.Sqrt(norm);
        }
        public static double norm2(int[,] A)
        {
            double norm = 0.0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            for (int i = 0; i != maxRow; i++)
                for (int j = 0; j != maxCol; j++)
                    norm += (double)(A[i, j] * A[i, j]);
            return Math.Sqrt(norm);
        }
        #endregion
    }
}

