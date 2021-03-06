﻿using StarMathLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotNum = DotNumerics.LinearAlgebra;

namespace TestEXE_for_StarMath
{
    internal static class Program
    {
        private static void Main()
        {
            //  testRBF();
            // SparseFunctionTest();
            // testStackFunctions();
            // testLUfunctions();
            //testSVD();
            benchMarkMatrixSolve();
            //benchMarkMatrixInversion();
            //  compareSolvers_Inversion_to_GaussSeidel();
            //checkEigen();
            Console.WriteLine("Press any key to close.");
            Console.ReadLine();

        }

        private static void testSVD()
        {
            var watch = new Stopwatch();
            double[,] A = new double[,] { { 1, 2 }, { 4, 5 } };
            //  double[,] A = new double[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
            watch.Restart();
            double[] sigma = A.SingularValueDecomposition(out var U, out var V);
            var sigMatrix = new double[U.GetLength(0), V.GetLength(0)];
            for (int i = 0; i < sigma.Length; i++)
                sigMatrix[i, i] = sigma[i];
            watch.Stop();
            var B = U.multiply(sigMatrix.multiply(V.transpose()));
            var error = B.subtract(A).norm1() / A.norm1();
            Console.WriteLine(error);
        }

        private static void testRBF()
        {
            const int NumPoints = 3;
            var Values = new[] { 43.0, 16.4, 88.9 };// ,91.2, 23.4};
            const double EpsilonSquared = 3.0;
            var NodePositions = new[]
            {
                new[] { 1.3, 2.1, 3.0}, new[] { 1.0, 0.4, 2.9}, new[] { 7.3, 5.5, 2.8}
               // , new[] { 0.2, 6.3, 8.1}, new[] { 10.2,8.3, 2.1}
            };
            const int size = NumPoints + 3 + 1;
            var A = new double[size, size];
            /* x is comprised of the coeffs1 on the inverse quadric terms followed by the slopes
             * followed by the offset */
            var b = new double[size];
            /* the first row is that the sum of weights should equal zero. */
            for (int i = 0; i < NumPoints; i++)
                A[0, i] = 1.0;
            /* the next n rows are for the main basis functions */
            for (int i = 0; i < NumPoints; i++)
            {
                b[i + 1] = Values[i];
                for (int j = 0; j < NumPoints; j++)
                    A[i + 1, j] = 1 / (Math.Sqrt(EpsilonSquared + NodePositions[i].norm2(NodePositions[j], true)));
                for (int j = 0; j < 3; j++)
                    A[i + 1, j + NumPoints] = NodePositions[i][j];
                A[i + 1, NumPoints + 3] = 1;
            }
            /* the following three equations are needed to solve for the slope terms even though slope is not
             * included. */
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < NumPoints; j++)
                    A[i + 1 + NumPoints, j] = NodePositions[j][i];
            }
            var x = StarMath.solve(A, b);
            var Coefficients = new double[NumPoints];
            var Slope = new double[3];
            for (int i = 0; i < NumPoints; i++)
                Coefficients[i] = x[i];
            for (int i = 0; i < 3; i++)
                Slope[i] = x[i + NumPoints];
            var Offset = x[NumPoints + 3];
        }

        private static void testStackFunctions()
        {
            var A = new[,] { { 0.1, 0.2, 0.3 }, { 1, 2, 3 }, { 10, 20, 30 }, { 100, 200, 300 } };
            A.Max(out _, out _);
            Console.WriteLine(A.JoinMatrixColumnsIntoVector().MakePrintString());
        }

        private static void testLUfunctions()
        {
            const int size = 4;
            var r = new Random();

            var A = new double[size, size];
            for (var i = 0; i < size; i++)
                for (var j = 0; j < size; j++)
                    //if (i != j)
                    A[i, j] = (200 * r.NextDouble()) - 100.0;
            Console.WriteLine("A =");
            Console.WriteLine(A.MakePrintString());


            StarMath.LUDecomposition(A, out var L, out var U, out _);
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

        private static void benchMarkMatrixSolve()
        {
            var watch = new Stopwatch();
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

            var limits = new int[,] {{3,10,30,100,300,1000,2000},
            {50,50,30,30,20,10,3}};
            for (var index = 0; index < limits.GetLength(1); index++)
            {
                int size = limits[0, index];
                const int numTrials = 1;//limits[1, index];
                const double probZero = -1.0;
                var rnd = new Random();
                for (var k = 0; k <= numTrials; k++)
                {
                    var A = new double[size, size];
                    var x = new double[size];
                    for (var i = 0; i < size; i++)
                    {
                        x[i] = (200 * r.NextDouble()) - 100.0;
                        for (var j = 0; j < size; j++)
                        {
                            if (rnd.NextDouble() > probZero)
                                A[i, j] = (200 * r.NextDouble()) - 100.0;
                        }
                    }
                    var b = A.multiply(x);
                    var result = new List<string> { size.ToString(), k.ToString() };

                    #region ALGlib

                    //Console.WriteLine("\n\n\nALGlib: start invert check for matrix of size: " + size);

                    //int info;
                    //alglib.matinvreport rep;
                    //watch.Restart();
                    //var B = (double[,])A.Clone();

                    //alglib.rmatrixinverse(ref B, out info, out rep);
                    //watch.Stop();
                    //recordResults(result, A, B, watch, k);

                    //#endregion

                    //#region Dot Numerics

                    //Console.WriteLine("\n\n\nDot Numerics: start invert check for matrix of size: " + size);
                    //var A_DN = new DotNum.Matrix(A);
                    //watch.Restart();
                    //var B_DN = A_DN.Inverse();
                    //watch.Stop();
                    //recordResults(result, A, B_DN.CopyToArray(), watch, k);

                    //#endregion
                    //#region Dot Numerics

                    //Console.WriteLine("\n\n\nDot Numerics: start invert check for matrix of size: " + size);
                    //watch.Restart();
                    //A_DN = new DotNum.Matrix(A);
                    //B_DN = A_DN.Inverse();
                    //watch.Stop();
                    //recordResults(result, A, B_DN.CopyToArray(), watch, k);

                    //#endregion


                    //#region Math.Net

                    //Console.WriteLine("\n\n\nMath.Net: start invert check for matrix of size: " + size);
                    //var matrixBuilder = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build;
                    //var A_MD = matrixBuilder.DenseOfArray(A);
                    ////var A_MD = new MathDot.deMatrix<double>();
                    ////    .Create(A);
                    //watch.Restart();
                    //var B_MD = A_MD.Inverse();
                    //watch.Stop();
                    //recordResults(result, A, B_MD.ToArray(), watch, k);

                    //#endregion

                    //#region Math.Net

                    //Console.WriteLine("\n\n\nMath.Net: start invert check for matrix of size: " + size);

                    //watch.Restart();
                    //A_MD = matrixBuilder.DenseOfArray(A);
                    //B_MD = A_MD.Inverse();
                    //watch.Stop();

                    //recordResults(result, A, B_MD.ToArray(), watch, k);

                    #endregion

                    #region StarMath

                    Console.WriteLine("\n\n\nSTARMATH: start solve check for matrix of size: " + size);

                    watch.Restart();
                    var x2 = StarMath.solve(A, b);
                    watch.Stop();
                    recordResults(result, x, x2, watch, k);
                    #endregion
                    results.Add(result);
                }
            }
            SaveResultsToCSV("results.csv", results);


        }


        private static void benchMarkMatrixInversion()
        {
            var watch = new Stopwatch();
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
                const int numTrials = 1;//limits[1, index];
                const double probZero = 0.2;
                var rnd = new Random();
                for (var k = 0; k <= numTrials; k++)
                {
                    var A = new double[size, size];
                    for (var i = 0; i < size; i++)
                        for (var j = 0; j < size; j++)
                        {
                            if (rnd.NextDouble() > probZero)
                                A[i, j] = (200 * r.NextDouble()) - 100.0;
                        }

                    var result = new List<string> { size.ToString(), k.ToString() };

                    #region ALGlib

                    Console.WriteLine("\n\n\nALGlib: start invert check for matrix of size: " + size);
                    watch.Restart();
                    var B = (double[,])A.Clone();
                    alglib.rmatrixinverse(ref B, out _, out _);
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
                    var matrixBuilder = MathNet.Numerics.LinearAlgebra.Matrix<double>.Build;
                    var A_MD = matrixBuilder.DenseOfArray(A);
                    //var A_MD = new MathDot.deMatrix<double>();
                    //    .Create(A);
                    watch.Restart();
                    var B_MD = A_MD.Inverse();
                    watch.Stop();
                    recordResults(result, A, B_MD.ToArray(), watch, k);

                    #endregion

                    #region Math.Net

                    Console.WriteLine("\n\n\nMath.Net: start invert check for matrix of size: " + size);

                    watch.Restart();
                    A_MD = matrixBuilder.DenseOfArray(A);
                    B_MD = A_MD.Inverse();
                    watch.Stop();

                    recordResults(result, A, B_MD.ToArray(), watch, k);

                    #endregion

                    //#region StarMath

                    //Console.WriteLine("\n\n\nSTARMATH: start invert check for matrix of size: " + size);

                    //watch.Restart();
                    //B = A.inverse();
                    //watch.Stop();
                    //recordResults(result, A, B, watch, k);
                    //#endregion
                    //results.Add(result);
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
            var λ = A.GetEigenValuesAndVectors(out var eigenVectors);
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
            var results = new List<List<string>>();

            var r = new Random();
            var fractionDiag = new double[] { 1.0, 0.5, 0.3, 0.1 };
            var matrixSize = new int[] { 5, 180, 200, 220, 240 };
            for (var i = 0; i < matrixSize.GetLength(0); i++)
            {
                for (int j = 0; j < fractionDiag.GetLength(0); j++)
                {
                    int size = matrixSize[i];
                    const int numTrials = 10;
                    for (var k = 0; k <= numTrials; k++)
                    {
                        var A = new double[size, size];
                        var b = new double[size];
                        for (var ii = 0; ii < size; ii++)
                        {
                            b[ii] = (200 * r.NextDouble()) - 100.0;
                            for (var jj = 0; jj < size; jj++)
                                A[ii, jj] = (200 * r.NextDouble()) - 100.0;
                            if (A[ii, ii] > 0) A[ii, ii] = A.GetRow(ii).norm1();
                            else A[ii, ii] = 2 * A[ii, ii] - A.GetRow(ii).norm1();
                            A[ii, ii] *= fractionDiag[j];
                        }
                        var result = new List<string> { k.ToString(), size.ToString(), fractionDiag[j].ToString() };

                        watch.Restart();
                        //var x = StarMath.solve(A, b);
                        var x = StarMath.SolveAnalytically(A, b);
                        watch.Stop();
                        recordResults(result, A, x, b, watch);
                        watch.Restart();
                        x = StarMath.SolveIteratively(A, b);
                        watch.Stop();
                        recordResults(result, A, x, b, watch);
                        Console.WriteLine(result.Aggregate((resultString, next) =>
                      resultString + " " + next));
                        results.Add(result);
                    }
                }
            }
            SaveResultsToCSV("results.csv", results);
        }
        private static void SparseFunctionTest()
        {
            var watch = new Stopwatch();
            var results = new List<List<string>>();

            var r = new Random();
            var numberPerRow = new[] { 3, 6, 12, 24, 48 };
            var matrixSize = new[] { 4, 20, 40, 80 };
            for (var i = 0; i < matrixSize.GetLength(0); i++)
            {
                for (int j = 0; j < numberPerRow.GetLength(0); j++)
                {
                    int size = 10 * matrixSize[i];
                    const int numTrials = 0;
                    for (var k = 0; k <= numTrials; k++)
                    {
                        var A = new double[size, size];
                        //var A = new SparseMatrix(size, size);
                        var b = new double[size];
                        for (var ii = 0; ii < size; ii++)
                        {
                            b[ii] = (200 * r.NextDouble()) - 100.0;
                            var diagvalue = (200 * r.NextDouble()) - 100.0;
                            A[ii, ii] = diagvalue;
                            var formerRandomPositions = new List<int>();
                            for (var jj = 0; jj < numberPerRow[j]; jj++)
                            {
                                int randomPosition;
                                do
                                {
                                    randomPosition = r.Next(size);
                                } while (ii == randomPosition || formerRandomPositions.Contains(randomPosition));
                                // formerRandomPositions.Add(randomPosition);
                                var value = (200 * r.NextDouble()) - 100.0;
                                A[ii, randomPosition] = A[randomPosition, ii] = value;
                            }
                        }

                        var sparseA = A.ConvertDenseToSparseMatrix();
                        var result = new List<string> { k.ToString(), size.ToString(), (numberPerRow[j] / (double)size).ToString() };

                        watch.Restart();
                        var x = sparseA.SolveAnalytically(b, true).ToArray();
                        sparseA[0, 0] *= 1.0;
                        watch.Stop();
                        recordResults(result, A, x, b, watch);
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
        private static void recordResults(List<string> result, double[] x1, double[] x2, Stopwatch watch, int k)
        {
            if (k == 0) return; //it seems that the first time you call a new function there may be a delay. This is especially
            // true if the function is in another dll.
            var C = x1.subtract(x2);
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

