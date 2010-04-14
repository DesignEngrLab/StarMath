using System;

namespace StarMatLib
{
    // note this is set to public for testing purposes only - remove when done.
    public static partial class StarMat
    {
        #region Add, Subtract, and Multiply functions
        #region Multiplication of Scalars, Vectors, and Matrices
        public static double[] multiply(double a, double[] B)
        {
            // scale vector B by the amount of scalar B
            int size = B.GetLength(0);
            double[] c = new double[size];
            for (int i = 0; i != size; i++)
                c[i] = a * B[i];
            return c;
        }
        public static double[,] multiply(double a, double[,] B)
        {
            double[,] c = new double[B.GetLength(0), B.GetLength(1)];
            for (int i = 0; i != B.GetLength(0); i++)
                for (int j = 0; j != B.GetLength(1); j++)
                    c[i, j] = a * B[i, j];
            return c;
        }
        public static double multiplyDot(double[] A, double[] B)
        {
            // this is B dot term_i multiplication
            int size = A.GetLength(0);
            if (size != B.GetLength(0)) return double.NaN;
            double c = 0.0;
            for (int i = 0; i != size; i++)
                c += A[i] * B[i];
            return c;
        }
        public static double[] multiplyCross(double[] A, double[] B)
        {
            throw new NotImplementedException();
        }
        public static double[,] multiplyVectorsIntoAMatrix(double[] A, double[] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = B.GetLength(0);

            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i] * B[j];
            return C;
        }
        public static double[,] multiply(double[,] A, double[,] B)
        {
            if (A.GetLength(1) != B.GetLength(0))
                return null;
            // this is B dot term_i multiplication
            int CRowSize = A.GetLength(0);
            int CColSize = B.GetLength(1);


            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                {
                    C[i, j] = 0.0;
                    for (int k = 0; k != A.GetLength(1); k++)
                        C[i, j] += A[i, k] * B[k, j];
                }
            return C;
        }
        public static double[] multiply(double[,] A, double[] B)
        {
            // this is B dot term_i multiplication
            int ARowSize = A.GetLength(0);
            int AColSize = A.GetLength(1);
            if (AColSize != B.GetLength(0)) return null;

            double[] C = new double[ARowSize];

            for (int i = 0; i != ARowSize; i++)
            {
                C[i] = 0.0;
                for (int j = 0; j != AColSize; j++)
                    C[i] += A[i, j] * B[j];
            }
            return C;
        }
        public static double[] multiply(double[] B, double[,] A)
        {
            // this is B dot term_i multiplication
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if (CRowSize != B.GetLength(0)) return null;

            double[] C = new double[CColSize];

            for (int i = 0; i != CColSize; i++)
            {
                C[i] = 0.0;
                for (int j = 0; j != CRowSize; j++)
                    C[i] += B[j] * A[j, i];
            }
            return C;
        }
        #endregion

        #region Add Vector-to-Vector and Matrix-to-Matrix
        public static double[] add(double[] A, double[] B)
        {
            // add vector A to vector B
            int size = A.GetLength(0);
            if (size != B.GetLength(0)) return null;
            double[] c = new double[size];
            for (int i = 0; i != size; i++)
                c[i] = A[i] + B[i];
            return c;
        }
        public static double[,] add(double[,] A, double[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }
        public static double[,] add(double[,] A, int[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + (double)B[i, j];
            return C;
        }
        public static int[,] add(int[,] A, int[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            int[,] C = new int[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] + B[i, j];
            return C;
        }
        #endregion

        #region Subtract Vector-to-Vector and Matrix-to-Matrix
        public static double[] subtract(double[] A, double[] B)
        {
            // add vector A to vector B
            int size = A.GetLength(0);
            if (size != B.GetLength(0)) return null;
            double[] c = new double[size];
            for (int i = 0; i != size; i++)
                c[i] = A[i] - B[i];
            return c;
        }
        public static double[,] subtract(double[,] A, double[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            double[,] C = new double[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }
        public static int[,] subtract(int[,] A, int[,] B)
        {
            int CRowSize = A.GetLength(0);
            int CColSize = A.GetLength(1);
            if ((CRowSize != B.GetLength(0)) || (CColSize != B.GetLength(1)))
                return null;

            int[,] C = new int[CRowSize, CColSize];

            for (int i = 0; i != CRowSize; i++)
                for (int j = 0; j != CColSize; j++)
                    C[i, j] = A[i, j] - B[i, j];
            return C;
        }
        #endregion
        #endregion
    }
}

