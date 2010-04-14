using System;
using StarMathLib;

namespace TestEXE_for_StarMath
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = 1000;

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
