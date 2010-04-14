using System;
using StarMatLib;

namespace TestEXE_for_StarMat
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
            double[,] B = StarMat.inverse(A);
            double[,] C = StarMat.subtract(StarMat.multiply(A, B), StarMat.makeIdentity(size));
            double error = StarMat.norm2(C);
            TimeSpan interval = DateTime.Now - now;
            Console.WriteLine("end invert, error = " + error);
            Console.WriteLine("time = " + interval);
            Console.ReadLine();
        }
    }
}
