using System;

namespace TestEXE_for_StarMath
{
    class Program
    {
        static void Main()
        {
            //benchMarkMatrixInversion();
            testStackFunctions();
           // testLUfunctions();
            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }

        private static void testStackFunctions()
        {
            var A = new[,] {{0.1, 0.2, 0.3}, {1, 2, 3}, {10, 20, 30}, {100, 200, 300}};
            int i, j;
            StarMath.StarMath.Max(A, out i, out j);
            Console.WriteLine(StarMath.StarMath.MakePrintString(StarMath.StarMath.JoinMatrixColumnsIntoVector(A)));
        }

        private static void testLUfunctions()
        {
            const int size = 14;
            Random r = new Random();

            double[,] A = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    A[i, j] = (200 * r.NextDouble()) - 100.0;
            Console.WriteLine("A =");
            Console.WriteLine(StarMath.StarMath.MakePrintString(A));

            Console.WriteLine("Combined LU = ");
            Console.WriteLine(StarMath.StarMath.LUDecomposition(A));

            double[,] L, U;
            StarMath.StarMath.LUDecomposition(A, out L, out U);
            Console.WriteLine(" L = ");
            Console.WriteLine(StarMath.StarMath.MakePrintString(L));
            Console.WriteLine(" U = ");
            Console.WriteLine(StarMath.StarMath.MakePrintString(U));

            Console.WriteLine("L * U =");
            Console.WriteLine(StarMath.StarMath.MakePrintString(StarMath.StarMath.multiply(L, U)));

            var E = StarMath.StarMath.subtract(A, StarMath.StarMath.multiply(L, U));
            var error = StarMath.StarMath.norm2(E);
            Console.WriteLine("error = " + error);

        }

        private static void benchMarkMatrixInversion()
        {
            const int size = 500;

            DateTime now = DateTime.Now;
            Random r = new Random();
            double[,] A = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    A[i, j] = (200 * r.NextDouble()) - 100.0;
            Console.WriteLine("start invert check for matrix of size: " + size);
            double[,] B = StarMath.StarMath.inverse(A);
            double[,] C = StarMath.StarMath.subtract(StarMath.StarMath.multiply(A, B), StarMath.StarMath.makeIdentity(size));
            double error = StarMath.StarMath.norm2(C);
            var interval = DateTime.Now - now;
            Console.WriteLine("end invert, error = " + error);
            Console.WriteLine("time = " + interval);
        }
    }
}
