using System;
using StarMathLib;

namespace TestEXE_for_StarMath
{
    class Program
    {
        static void Main(string[] args)
        {
            //benchMarkMatrixInversion();
           testLUfunctions();
        }

        private static void testLUfunctions()
        {
            int size = 4;
            Random r = new Random();

            double[,] A = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    A[i, j] = (200 * r.NextDouble()) - 100.0;
            Console.WriteLine("A =");
            StarMath.ConsoleWrite(A);

            Console.WriteLine("Combined LU = ");
            StarMath.ConsoleWrite(StarMath.LUDecomposition(A));

            double[,] L, U;
            StarMath.LUDecomposition(A, out L, out U);
            Console.WriteLine(" L = ");
            StarMath.ConsoleWrite(L);
            Console.WriteLine(" U = ");
            StarMath.ConsoleWrite(U);

            Console.WriteLine("L * U =");
            StarMath.ConsoleWrite(StarMath.multiply(L, U));
            
            var E = StarMath.subtract(A, StarMath.multiply(L, U));
            var error = StarMath.norm2(E);
            Console.WriteLine("error = " + error.ToString());

            Console.ReadLine();
        }

        private static void benchMarkMatrixInversion()
        {
            int size = 100;

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
            TimeSpan interval = DateTime.Now - now;
            Console.WriteLine("end invert, error = " + error);
            Console.WriteLine("time = " + interval);
            Console.ReadLine();
        }
    }
}
