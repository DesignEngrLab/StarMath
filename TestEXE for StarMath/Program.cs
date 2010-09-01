using System;
using StarMathLib;

namespace TestEXE_for_StarMath
{
    class Program
    {
        static void Main()
        {
            benchMarkMatrixInversion();
            testLUfunctions();
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
            Console.WriteLine(StarMath.MakePrintString(A));

            Console.WriteLine("Combined LU = ");
            Console.WriteLine(StarMath.LUDecomposition(A));

            double[,] L, U;
            StarMath.LUDecomposition(A, out L, out U);
            Console.WriteLine(" L = ");
            Console.WriteLine(StarMath.MakePrintString(L));
            Console.WriteLine(" U = ");
            Console.WriteLine(StarMath.MakePrintString(U));

            Console.WriteLine("L * U =");
            Console.WriteLine(StarMath.MakePrintString(StarMath.multiply(L, U)));

            var E = StarMath.subtract(A, StarMath.multiply(L, U));
            var error = StarMath.norm2(E);
            Console.WriteLine("error = " + error);

            Console.ReadLine();
        }

        private static void benchMarkMatrixInversion()
        {
            const int size = 100;

            DateTime now = DateTime.Now;
            Random r = new Random();
            double[,] A = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    A[i, j] = (200 * r.NextDouble()) - 100.0;
            Console.WriteLine("start invert check");
            double[,] B = StarMath.inverse(A);
            double[,] C = StarMath.subtract(StarMath.multiply(A, B), StarMath.makeIdentity(size));
            double error = StarMath.norm2(C);
            var interval = DateTime.Now - now;
            Console.WriteLine("end invert, error = " + error);
            Console.WriteLine("time = " + interval);
            Console.ReadLine();
        }
    }
}
