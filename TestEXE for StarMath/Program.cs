using System;
using StarMathLib;

namespace TestEXE_for_StarMath
{
    internal class Program
    {
        private static void Main()
        {
            testStackFunctions();
            testLUfunctions();
            benchMarkMatrixInversion();
            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }

        private static void testStackFunctions()
        {
            var A = new[,] { { 0.1, 0.2, 0.3 }, { 1, 2, 3 }, { 10, 20, 30 }, { 100, 200, 300 } };
            int i, j;
            StarMath.Max(A, out i, out j);
            Console.WriteLine(StarMath.MakePrintString(StarMath.JoinMatrixColumnsIntoVector(A)));
        }

        private static void testLUfunctions()
        {
            const int size = 14;
            var r = new Random();

            var A = new double[size, size];
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
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
        }

        private static void benchMarkMatrixInversion()
        {
            const int size = 500;
            var now = DateTime.Now;
            var r = new Random();
            var A = new double[size, size];
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    A[i, j] = (200 * r.NextDouble()) - 100.0;
            Console.WriteLine("start invert check for matrix of size: " + size);
            var B = StarMath.inverse(A);
            var C = StarMath.subtract(StarMath.multiply(A, B), StarMath.makeIdentity(size));
            var error = StarMath.norm2(C);
            var interval = DateTime.Now - now;
            Console.WriteLine("end invert, error = " + error);
            Console.WriteLine("time = " + interval);
        }
    }
}