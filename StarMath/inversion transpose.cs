using System;

namespace StarMathLib
{
    // note this is set to public for testing purposes only - remove when done.
    public static partial class StarMath
    {
        #region Matrix Inversion & Transpose
        public static double[,] inverseUpper(double[,] A)
        {
            if (A.GetLength(0) != A.GetLength(1))
            {
                output("Matrix cannnot be inverted. Can only invert sqare matrices.");
                return null;
            }
            int size = A.GetLength(0);
            double[,] B = new double[size, size];
            B.Initialize();
            double Bjj, v;
            double[] t = new double[size];


            for (int j = 0; j < size; j++)
            {
                //if (A[innerK, innerK] == 0)
                //    return null;
                B[j, j] = 1 / A[j, j];
                for (int i = 0; i < j; i++)
                    B[i, j] = A[i, j];
            }
            for (int j = 1; j < size; j++)
            {
                Bjj = -B[j, j];
                for (int i = 0; i < j; i++)
                    t[i] = B[i, j];
                for (int i = 0; i < j; i++)
                {
                    if (i < j - 1)
                    {
                        v = 0.0;
                        for (int jj = i + 1; jj < j; jj++)
                            v += B[i, jj] * t[jj];
                    }
                    else v = 0;
                    B[i, j] = v + B[i, i] * t[i];
                }
                for (int ii = 0; ii < j; ii++)
                    B[ii, j] = Bjj * B[ii, j];
            }
            return B;
        }

        public static double[,] inverse(double[,] A)
        {
            output("inverting matrix...", 5);
            // this code is adapted from http://users.erols.com/mdinolfo/matrix.htm
            // one constraint/caveat in this function is that the diagonal elts. cannot
            // be zero.
            // if the matrix is not square or is less than B 2x2, 
            // then this function won't work
            if (A.GetLength(0) != A.GetLength(1))
            {
                output("Matrix cannnot be inverted. Can only invert sqare matrices.");
                return null;
            }
            int size = A.GetLength(0);
            double[,] B = new double[size, size];


            if (size == 1)
            {
                B[0, 0] = 1 / A[0, 0];
                return B;
            }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    B[i, j] = A[i, j];

            // normalize row 0
            for (int i = 1; i < size; i++) B[0, i] /= B[0, 0];
            #region LU factorization
            for (int i = 1; i < size; i++)
            {
                for (int j = i; j < size; j++)
                { // do B column of L
                    double sum = 0.0;
                    for (int k = 0; k < i; k++)
                        sum += B[j, k] * B[k, i];
                    B[j, i] -= sum;
                }
                if (i == size - 1) continue;
                for (int j = i + 1; j < size; j++)
                {  // do B row of U
                    double sum = 0.0;
                    for (int k = 0; k < i; k++)
                        sum += B[i, k] * B[k, j];
                    B[i, j] =
                       (B[i, j] - sum) / B[i, i];
                }
            }
            #endregion
            #region invert L
            for (int i = 0; i < size; i++)
                for (int j = i; j < size; j++)
                {
                    double x = 1.0;
                    if (i != j)
                    {
                        x = 0.0;
                        for (int k = i; k < j; k++)
                            x -= B[j, k] * B[k, i];
                    }
                    B[j, i] = x / B[j, j];
                }
            #endregion
            #region invert U
            for (int i = 0; i < size; i++)
                for (int j = i; j < size; j++)
                {
                    if (i == j) continue;
                    double sum = 0.0;
                    for (int k = i; k < j; k++)
                        sum += B[k, j] * ((i == k) ? 1.0 : B[i, k]);
                    B[i, j] = -sum;
                }
            #endregion
            #region final inversion
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    double sum = 0.0;
                    for (int k = ((i > j) ? i : j); k < size; k++)
                        sum += ((j == k) ? 1.0 : B[j, k]) * B[k, i];
                    B[j, i] = sum;
                }
            #endregion
            return B;
        }

        public static double[,] transpose(double[,] A)
        {
            int CRowSize = A.GetLength(1);
            int CColSize = A.GetLength(0);
            //if ((CRowSize == 0) || (CColSize == 0))
            //    return null;

            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[j, i];
            return C;
        }
        #endregion
    }
}

