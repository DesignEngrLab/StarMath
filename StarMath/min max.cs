/*************************************************************************
 *     This file & class is part of the StarMath Project
 *     Copyright 2010 Matthew Ira Campbell, PhD.
 *
 *     StarMath is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *  
 *     StarMath is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 *  
 *     You should have received a copy of the GNU General Public License
 *     along with StarMath.  If not, see <http://www.gnu.org/licenses/>.
 *     
 *     Please find further details and contact information on GraphSynth
 *     at http://starmath.codeplex.com/.
 *************************************************************************/
using System;

namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Min max functions etc..

        /// <summary>
        /// Mins the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static double Min(double[,] A)
        {
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            double Min = double.MaxValue;
            for (int i = 0; i != maxRow; i++)
            {
                for (int j = 0; j != maxCol; j++)
                {
                    if (A[i, j] < Min)
                        Min = A[i, j];
                }
            }
            return Min;
        }
        /// <summary>
        /// Maxes the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static double Max(double[,] A)
        {
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            double Max = double.MinValue;
            for (int i = 0; i != maxRow; i++)
            {
                for (int j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                        Max = A[i, j];
                }
            }
            return Max;
        }
        /// <summary>
        /// Mins the max.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static double[] MinMax(double[,] A)
        {
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            double Max = double.MinValue;
            double Min = double.MaxValue;
            for (int i = 0; i != maxRow; i++)
            {
                for (int j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                        Max = A[i, j];
                    if (A[i, j] < Min)
                        Min = A[i, j];
                }
            }
            return (new double[] { Min, Max });
        }
        /// <summary>
        /// Mins the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static int Min(int[,] A)
        {
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            int Min = int.MaxValue;
            for (int i = 0; i != maxRow; i++)
            {
                for (int j = 0; j != maxCol; j++)
                {
                    if (A[i, j] < Min)
                        Min = A[i, j];
                }
            }
            return Min;
        }
        /// <summary>
        /// Maxes the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static int Max(int[,] A)
        {
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            int Max = int.MinValue;
            for (int i = 0; i != maxRow; i++)
            {
                for (int j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                        Max = A[i, j];
                }
            }
            return Max;
        }
        /// <summary>
        /// Maxes the specified A.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static int Max(int[] A)
        {
            int maxRow = A.GetLength(0);
            int Max = int.MinValue;
            for (int i = 0; i != maxRow; i++)
            {
                if (A[i] > Max)
                    Max = A[i];
            }
            return Max;
        }
        /// <summary>
        /// Mins the max.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static int[] MinMax(int[,] A)
        {
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            int Max = int.MinValue;
            int Min = int.MaxValue;
            for (int i = 0; i != maxRow; i++)
            {
                for (int j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                        Max = A[i, j];
                    if (A[i, j] < Min)
                        Min = A[i, j];
                }
            }
            return (new int[] { Min, Max });
        }
        /// <summary>
        /// Mins the RC.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static double[] MinRC(double[,] A)
        {
            int MinDim1 = 0; //Dim1 = rows
            int MinDim2 = 0; //Dim2 = columns
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            double Min = double.MaxValue;
            for (int i = 0; i != maxRow; i++)
            {
                for (int j = 0; j != maxCol; j++)
                {
                    if (A[i, j] < Min)
                    {
                        Min = A[i, j];
                        MinDim1 = i;
                        MinDim2 = j;
                    }
                }
            }
            return new double[] { Min, MinDim1, MinDim2 };
        }
        /// <summary>
        /// Maxes the RC.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static double[] MaxRC(double[,] A)
        {
            int MaxDim1 = 0; //Dim1 = rows
            int MaxDim2 = 0; //Dim2 = columns
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            double Max = double.MinValue;
            for (int i = 0; i != maxRow; i++)
            {
                for (int j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                    {
                        Max = A[i, j];
                        MaxDim1 = i;
                        MaxDim2 = j;
                    }
                }
            }
            return new double[] { Max, MaxDim1, MaxDim2 };
        }
        /// <summary>
        /// Mins the max RC.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static double[] MinMaxRC(double[,] A)
        {
            int MinDim1 = 0; //Dim1 = rows
            int MinDim2 = 0; //Dim2 = columns
            int MaxDim1 = 0; //Dim1 = rows
            int MaxDim2 = 0; //Dim2 = columns
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            double Max = double.MinValue;
            double Min = double.MaxValue;
            for (int i = 0; i != maxRow; i++)
            {
                for (int j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                    {
                        MaxDim1 = i;
                        MaxDim2 = j;
                        Max = A[i, j];
                    }
                    if (A[i, j] < Min)
                    {
                        MinDim1 = i;
                        MinDim2 = j;
                        Min = A[i, j];
                    }
                }
            }
            return (new double[] { Min, MinDim1, MinDim2, Max, MaxDim1, MaxDim2 });
        }
        /// <summary>
        /// Mins the R.
        /// </summary>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static double[] MinR(double[] A)
        {
            int MinDim = 0; //Dim1 = rows
            int maxRow = A.GetLength(0);
            double Min = double.MaxValue;
            for (int i = 0; i < maxRow; i++)
            {
                if (A[i] < Min)
                {
                    Min = A[i];
                    MinDim = i;
                }
            }
            return new double[] { Min, MinDim };
        }
        /// <summary>
        /// Finds the specified find val.
        /// </summary>
        /// <param name="FindVal">The find val.</param>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static int[] find(double FindVal, double[] A)
        {
            int Size_of_A = A.GetLength(0);
            int k = 0;
            int[] SearchResults = new int[Size_of_A];

            for (int i = 0; i < Size_of_A; i++)
            {
                if (A[i] == FindVal)
                {
                    SearchResults[k] = i;
                    k++;
                }
            }
            return GetColumns(makeAP(0, 1, k), SearchResults);
        }
        /// <summary>
        /// Finds the specified find val.
        /// </summary>
        /// <param name="FindVal">The find val.</param>
        /// <param name="A">The A.</param>
        /// <returns></returns>
        public static int[] find(int FindVal, int[] A)
        {
            int Size_of_A = A.GetLength(0);
            int k = 0;
            int[] SearchResults = new int[Size_of_A];

            for (int i = 0; i < Size_of_A; i++)
            {
                if (A[i] == FindVal)
                {
                    SearchResults[k] = i;
                    k++;
                }
            }
            return GetColumns(makeAP(0, 1, k), SearchResults);
        }
        #endregion
    }
}

