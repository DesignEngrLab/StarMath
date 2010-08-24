namespace StarMathLib
{
    public static partial class StarMath
    {
        #region Min max functions etc..

        /// <summary>
        ///   Finds the minimum value in the given 2D double array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A double value that is the minimum of A</returns>
        public static double Min(double[,] A)
        {
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Min = double.MaxValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] < Min)
                        Min = A[i, j];
                }
            }
            return Min;
        }

        /// <summary>
        ///   Finds the maximum value in the given 2D double array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A double value that is the maximum of A</returns>
        public static double Max(double[,] A)
        {
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Max = double.MinValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                        Max = A[i, j];
                }
            }
            return Max;
        }

        /// <summary>
        ///   Finds the minimum and maximum value in the given 2D double array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A 1 x 2 double array that contains the minimum and maximum value in A</returns>
        public static double[] MinMax(double[,] A)
        {
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Max = double.MinValue;
            var Min = double.MaxValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                        Max = A[i, j];
                    if (A[i, j] < Min)
                        Min = A[i, j];
                }
            }
            return (new[] {Min, Max});
        }

        /// <summary>
        ///   Finds the minimum value in the given 2D integer array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An integer value that is the minimum of A</returns>
        public static int Min(int[,] A)
        {
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Min = int.MaxValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] < Min)
                        Min = A[i, j];
                }
            }
            return Min;
        }

        /// <summary>
        ///   Finds the maximum value in the given 2D integer array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An integer value that is the maximum of A</returns>
        public static int Max(int[,] A)
        {
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Max = int.MinValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                        Max = A[i, j];
                }
            }
            return Max;
        }

        /// <summary>
        ///   Finds the maximum value in the given 1D integer array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>An integer value that is the maximum of A</returns>
        public static int Max(int[] A)
        {
            var maxRow = A.GetLength(0);
            var Max = int.MinValue;
            for (var i = 0; i != maxRow; i++)
            {
                if (A[i] > Max)
                    Max = A[i];
            }
            return Max;
        }

        /// <summary>
        ///   Finds the minimum and maximum value in the given 2D double array
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A 1 x 2 double array that contains the minimum and maximum value in A</returns>
        public static int[] MinMax(int[,] A)
        {
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Max = int.MinValue;
            var Min = int.MaxValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                        Max = A[i, j];
                    if (A[i, j] < Min)
                        Min = A[i, j];
                }
            }
            return (new[] {Min, Max});
        }

        /// <summary>
        ///   Finds the minimum value in the given 2D double array and returns the row and column indices along with it.
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A 1 x 3 double array that contains the minimum value and its row and column index in A</returns>
        public static double[] MinRC(double[,] A)
        {
            var MinDim1 = 0; //Dim1 = rows
            var MinDim2 = 0; //Dim2 = columns
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Min = double.MaxValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] >= Min) continue;
                    Min = A[i, j];
                    MinDim1 = i;
                    MinDim2 = j;
                }
            }
            return new[] {Min, MinDim1, MinDim2};
        }

        /// <summary>
        ///   Finds the maximum value in the given 2D double array and returns the row and column indices along with it.
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A 1 x 3 double array that contains the maximum value and its row and column index in A</returns>
        public static double[] MaxRC(double[,] A)
        {
            var MaxDim1 = 0; //Dim1 = rows
            var MaxDim2 = 0; //Dim2 = columns
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Max = double.MinValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] <= Max) continue;
                    Max = A[i, j];
                    MaxDim1 = i;
                    MaxDim2 = j;
                }
            }
            return new[] {Max, MaxDim1, MaxDim2};
        }

        /// <summary>
        ///   Finds the minimum and maximum value in the given 2D double array and returns the row and column indices
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A 1 x 6 double array that contains the min value and its row and col index and the max value and its row and col index in A</returns>
        public static double[] MinMaxRC(double[,] A)
        {
            var MinDim1 = 0; //Dim1 = rows
            var MinDim2 = 0; //Dim2 = columns
            var MaxDim1 = 0; //Dim1 = rows
            var MaxDim2 = 0; //Dim2 = columns
            var maxRow = A.GetLength(0);
            var maxCol = A.GetLength(1);
            var Max = double.MinValue;
            var Min = double.MaxValue;
            for (var i = 0; i != maxRow; i++)
            {
                for (var j = 0; j != maxCol; j++)
                {
                    if (A[i, j] > Max)
                    {
                        MaxDim1 = i;
                        MaxDim2 = j;
                        Max = A[i, j];
                    }
                    if (A[i, j] >= Min) continue;
                    MinDim1 = i;
                    MinDim2 = j;
                    Min = A[i, j];
                }
            }
            return (new[] {Min, MinDim1, MinDim2, Max, MaxDim1, MaxDim2});
        }

        /// <summary>
        ///   Finds the minimum value in the given 1D double array and returns its index along with it.
        /// </summary>
        /// <param name = "A">The array to be searched for</param>
        /// <returns>A 1 x 2 double array that contains the minimum value and its index in A</returns>
        public static double[] MinR(double[] A)
        {
            var MinDim = 0; //Dim1 = rows
            var maxRow = A.GetLength(0);
            var Min = double.MaxValue;
            for (var i = 0; i < maxRow; i++)
            {
                if (A[i] >= Min) continue;
                Min = A[i];
                MinDim = i;
            }
            return new[] {Min, MinDim};
        }

        /// <summary>
        ///   Finds the specified find val.
        /// </summary>
        /// <param name = "FindVal">The find val.</param>
        /// <param name = "A">The A.</param>
        /// <returns></returns>
        public static int[] find(double FindVal, double[] A)
        {
            var Size_of_A = A.GetLength(0);
            var k = 0;
            var SearchResults = new int[Size_of_A];

            for (var i = 0; i < Size_of_A; i++)
            {
                if (A[i] != FindVal) continue;
                SearchResults[k] = i;
                k++;
            }
            return GetColumns(makeLinearProgression(0, 1, k), SearchResults);
        }

        /// <summary>
        ///   Finds the specified find val.
        /// </summary>
        /// <param name = "FindVal">The find val.</param>
        /// <param name = "A">The A.</param>
        /// <returns></returns>
        public static int[] find(int FindVal, int[] A)
        {
            var Size_of_A = A.GetLength(0);
            var k = 0;
            var SearchResults = new int[Size_of_A];

            for (var i = 0; i < Size_of_A; i++)
            {
                if (A[i] != FindVal) continue;
                SearchResults[k] = i;
                k++;
            }
            return GetColumns(makeLinearProgression(0, 1, k), SearchResults);
        }

        #endregion
    }
}