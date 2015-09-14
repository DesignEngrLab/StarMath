using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using DotNumerics.LinearAlgebra.CSLapack;
using StarMathLib;
using DotNum = DotNumerics.LinearAlgebra;
using MathDot = MathNet.Numerics.LinearAlgebra;

namespace TestEXE_for_StarMath
{
    internal class Program
    {
        private static void Main()
        {
            // testStackFunctions();
            //testLUfunctions();
            //benchMarkMatrixInversion();
            compareSolvers_Inversion_to_GaussSeidel();
            //checkEigen();
            Console.WriteLine("Press any key to close.");
          //  Console.ReadLine();
        }

        private static void testStackFunctions()
        {
            var A = new[,] { { 0.1, 0.2, 0.3 }, { 1, 2, 3 }, { 10, 20, 30 }, { 100, 200, 300 } };
            int i, j;
            A.Max(out i, out j);
            Console.WriteLine(A.JoinMatrixColumnsIntoVector().MakePrintString());
        }

        private static void testLUfunctions()
        {
            const int size = 9;
            var r = new Random();

            var A = new double[size, size];
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    A[i, j] = (200 * r.NextDouble()) - 100.0;
            Console.WriteLine("A =");
            Console.WriteLine(A.MakePrintString());


            double[,] L, U;
            StarMath.LUDecomposition(A, out L, out U);
            Console.WriteLine(" L = ");
            Console.WriteLine(L.MakePrintString());
            Console.WriteLine(" U = ");
            Console.WriteLine(U.MakePrintString());

            Console.WriteLine("L * U =");
            Console.WriteLine(L.multiply(U).MakePrintString());

            var E = A.subtract(L.multiply(U));
            var error = E.norm2();
            Console.WriteLine("error = " + error);
        }

        private static void benchMarkMatrixInversion()
        {
            var watch = new Stopwatch();
            double error;
            var results = new List<List<string>>
            {
                new List<string>
                {
                    "",
                    "",
                    "ALGlib Err",
                    "ALGlib Time",
                    "Dot Numerics Err",
                    "Dot Numerics Time",
                    "Dot NumericsClass Err",
                    "Dot NumericsClass Time",
                    "Math.Net Err",
                    "Math.Net Numerics Time",
                    "Math.NetClass Err",
                    "Math.NetClass Time",
                    "StArMath Err",
                    "StArMath Time"
                }
            };
            var r = new Random();

            var limits = new int[,] {{3,10,30,100,300,1000,3000},
            {50,50,30,30,20,10,3}};
            for (var index = 0; index < limits.GetLength(1); index++)
            {
                int size = limits[0, index];
                int numTrials = limits[1, index];

                for (var k = 0; k <= numTrials; k++)
                {
                    var A = new double[size, size];
                    for (var i = 0; i < size; i++)
                        for (var j = 0; j < size; j++)
                            A[i, j] = (200 * r.NextDouble()) - 100.0;
                    var result = new List<string> { size.ToString(), k.ToString() };

                    #region ALGlib

                    Console.WriteLine("\n\n\nALGlib: start invert check for matrix of size: " + size);

                    int info;
                    alglib.matinvreport rep;
                    watch.Restart();
                    var B = (double[,])A.Clone();
                    alglib.rmatrixinverse(ref B, out info, out rep);
                    watch.Stop();
                    recordResults(result, A, B, watch, k);

                    #endregion

                    #region Dot Numerics

                    Console.WriteLine("\n\n\nDot Numerics: start invert check for matrix of size: " + size);

                    var A_DN = new DotNum.Matrix(A);
                    watch.Restart();
                    var B_DN = A_DN.Inverse();
                    watch.Stop();
                    recordResults(result, A, B_DN.CopyToArray(), watch, k);

                    #endregion
                    #region Dot Numerics

                    Console.WriteLine("\n\n\nDot Numerics: start invert check for matrix of size: " + size);
                    watch.Restart();
                    A_DN = new DotNum.Matrix(A);
                    B_DN = A_DN.Inverse();
                    watch.Stop();
                    recordResults(result, A, B_DN.CopyToArray(), watch, k);

                    #endregion


                    #region Math.Net

                    Console.WriteLine("\n\n\nMath.Net: start invert check for matrix of size: " + size);

                    var A_MD = MathDot.Matrix.Create(A);
                    watch.Restart();
                    var B_MD = A_MD.Inverse();
                    watch.Stop();
                    recordResults(result, A, B_MD.CopyToArray(), watch, k);

                    #endregion

                    #region Math.Net

                    Console.WriteLine("\n\n\nMath.Net: start invert check for matrix of size: " + size);

                    watch.Restart();
                    A_MD = MathDot.Matrix.Create(A);
                    B_MD = A_MD.Inverse();
                    watch.Stop();

                    recordResults(result, A, B_MD.CopyToArray(), watch, k);

                    #endregion

                    #region StarMath

                    Console.WriteLine("\n\n\nSTARMATH: start invert check for matrix of size: " + size);

                    watch.Restart();
                    B = A.inverse();
                    watch.Stop();
                    recordResults(result, A, B, watch, k);
                    #endregion
                    results.Add(result);
                }
            }
            SaveResultsToCSV("results.csv", results);


        }


        private static void checkEigen()
        {
            var r = new Random();
            const int size = 44;
            var A = new double[size, size];
            for (var i = 0; i < size; i++)
                for (var j = i; j < size; j++)
                    A[i, j] = A[j, i] = (200 * r.NextDouble()) - 100.0;
            var eigenVectors = new double[size][];
            var λ = A.GetEigenValuesAndVectors(out eigenVectors);
            //Console.WriteLine(StarMath.MakePrintString(ans[0]));
            for (int i = 0; i < size; i++)
            {
                var lhs = A.multiply(eigenVectors[i]);
                var rhs = StarMath.multiply(λ[0][i], eigenVectors[i]);
                Console.WriteLine(lhs.subtract(rhs).norm1());
            }
        }
        private static void compareSolvers_Inversion_to_GaussSeidel()
        {
            var watch = new Stopwatch();
            double error;
            var results = new List<List<string>>();

            var r = new Random();
            var numberPerRow = new[] { 3, 6, 12, 24, 48 };
            var matrixSize = new[] { 10, 20,  40,  80 };
            for (var i = 0; i < matrixSize.GetLength(0); i++)
            {
                for (int j = 0; j < numberPerRow.GetLength(0); j++)
                {
                    int size = 20 * matrixSize[i];
                    const int numTrials = 0;
                    for (var k = 0; k <= numTrials; k++)
                    {
                        //  var A = new double[size, size];
                        var AValues = new List<double>();
                        var rows = new List<int>();
                        var cols = new List<int>();
                        var b = new double[size];
                        for (var ii = 0; ii < size; ii++)
                        {
                            b[ii] = (200 * r.NextDouble()) - 100.0;
                            //for (int jj = ii; jj < size; jj++)
                            //{
                            //    var value = (200 * r.NextDouble()) - 100.0;
                            //    AValues.Add(value);
                            //    rows.Add(ii);
                            //    cols.Add(jj);
                            //    AValues.Add(value);
                            //    rows.Add(jj);
                            //    cols.Add(ii);
                            //}
                            var diagvalue = (200 * r.NextDouble()) - 100.0;
                            AValues.Add(diagvalue);
                            rows.Add(ii);
                            cols.Add(ii);
                            var formerRandomPositions = new List<int>();
                            for (var jj = 0; jj < numberPerRow[j]; jj++)
                            {
                                int randomPosition;
                                do
                                {
                                    randomPosition = r.Next(size);
                                } while (ii == randomPosition || formerRandomPositions.Contains(randomPosition));
                                formerRandomPositions.Add(randomPosition);
                                var value = (200 * r.NextDouble()) - 100.0;
                                // A[ii, jj] = value;
                                AValues.Add(value); 
                                rows.Add(ii);
                                cols.Add(randomPosition);
                            }
                        }

                        var result = new List<string> { k.ToString(), size.ToString(), (numberPerRow[j] / (double)size).ToString() };

                        //watch.Restart();
                        //var x = StarMath.SolveAnalytically(A, b, true);
                        //watch.Stop();
                        //recordResults(result, A, x, b, watch);
                        //var SparseA = A.ConvertDenseToSparseMatrix();
                        var SparseA = new SparseMatrix(rows, cols, AValues, size, size);
                        var ATranspose = SparseA.Copy();
                        ATranspose.Transpose();
                        SparseA.addInPlace(ATranspose);
                        watch.Restart();
                        var x = SparseA.SolveAnalytically(b, true);
                        watch.Stop();
                        recordResults(result, SparseA, x, b, watch);
                        watch.Restart();
                        x = SparseA.SolveAnalyticallyCSRApproach(b, true);
                        watch.Stop();
                        recordResults(result, SparseA, x, b, watch);
                        Console.WriteLine(result.Aggregate((resultString, next) =>
                      resultString + " " + next));
                        results.Add(result);

                    }
                }
            }
            SaveResultsToCSV("results.csv", results);
        }

        private static void recordResults(List<string> result, SparseMatrix A, double[] x, double[] b, Stopwatch watch)
        {
            double error;
            try
            {
                error = b.subtract(A.multiply(x)).norm1() / b.norm1();
            }
            catch
            {
                error = double.NaN;
            }
            result.Add(error.ToString());
            result.Add(watch.Elapsed.TotalMilliseconds.ToString());
        }

        private static void recordResults(List<string> result, double[,] A, double[] x, double[] b, Stopwatch watch)
        {
            double error;
            try
            {
                error = b.subtract(A.multiply(x)).norm1() / b.norm1();
            }
            catch
            {
                error = double.NaN;
            }
            result.Add(error.ToString());
            result.Add(watch.Elapsed.TotalMilliseconds.ToString());
        }

        private static void recordResults(List<string> result, double[,] A, double[,] invA, Stopwatch watch, int k)
        {
            if (k == 0) return; //it seems that the first time you call a new function there may be a delay. This is especially
            // true if the function is in another dll.
            var C = A.multiply(invA).subtract(StarMath.makeIdentity(A.GetLength(0)));
            var error = C.norm2();
            Console.WriteLine("end invert, error = " + error);
            Console.WriteLine("time = " + watch.Elapsed);
            result.Add(error.ToString());
            result.Add(watch.Elapsed.TotalMilliseconds.ToString());
        }

        private static void SaveResultsToCSV(string path, IEnumerable<List<string>> results)
        {
            var fs = new FileStream(path, FileMode.Create);
            var r = new StreamWriter(fs);
            foreach (var list in results)
            {
                string line = "";
                foreach (var s in list)
                    line += s + ",";
                line.Trim(',');
                r.WriteLine(line);
            }
            r.Close();
            fs.Close();
        }

    }
}

