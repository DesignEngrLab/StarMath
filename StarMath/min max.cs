using System;

namespace StarMathLib
{
    // note this is set to public for testing purposes only - remove when done.
    public static partial class StarMath
    {
        #region Min max functions etc..
        //Mukund's domain
        public static double[] makeAP(double Start, double CommonDifference, double End)
        {

            int NumOfElements = (int)((double)(End - Start) / CommonDifference);

            double[] AP = new double[NumOfElements];

            for (int i = 0; i != NumOfElements; i++)
                AP[i] = Start + CommonDifference * (double)i;

            return AP;
        }
        public static int[] makeAP(int Start, int CommonDifference, int End)
        {

            int NumOfElements = (int)((End - Start) / CommonDifference);

            int[] AP = new int[NumOfElements];

            for (int i = 0; i != NumOfElements; i++)
                AP[i] = Start + CommonDifference * i;

            return AP;
        }
        public static double[,] multiply(double a, int[,] B)
        {
            double[,] c = new double[B.GetLength(0), B.GetLength(1)];
            for (int i = 0; i != B.GetLength(0); i++)
                for (int j = 0; j != B.GetLength(1); j++)
                    c[i, j] = a * (double)B[i, j];
            return c;
        }
        public static double Min(double[,] A)
        {
            double Min = 0.0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            Min = A[0, 0];
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
        public static double Max(double[,] A)
        {
            double Max = 0.0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            Max = A[0, 0];
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
        public static double[] MinMax(double[,] A)
        {
            double Max = 0.0;
            double Min = 0.0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            Max = A[0, 0];
            Min = A[0, 0];
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
        public static int Min(int[,] A)
        {
            int Min = 0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            Min = A[0, 0];
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
        public static int Max(int[,] A)
        {
            int Max = 0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            Max = A[0, 0];
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
        public static int Max(int[] A)
        {
            int Max = 0;
            int maxRow = A.GetLength(0);
            Max = A[0];
            for (int i = 0; i != maxRow; i++)
            {
                if (A[i] > Max)
                    Max = A[i];
            }
            return Max;
        }
        public static int[] MinMax(int[,] A)
        {
            int Max = 0;
            int Min = 0;
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            Max = A[0, 0];
            Min = A[0, 0];
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
        public static double[] MinRC(double[,] A)
        {
            double Min = 0.0;
            int MinDim1 = 0; //Dim1 = rows
            int MinDim2 = 0; //Dim2 = columns
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            Min = A[0, 0];
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
        public static double[] MaxRC(double[,] A)
        {
            double Max = 0.0;
            int MaxDim1 = 0; //Dim1 = rows
            int MaxDim2 = 0; //Dim2 = columns
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            Max = A[0, 0];
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
        public static double[] MinMaxRC(double[,] A)
        {
            double Max = 0.0;
            double Min = 0.0;
            int MinDim1 = 0; //Dim1 = rows
            int MinDim2 = 0; //Dim2 = columns
            int MaxDim1 = 0; //Dim1 = rows
            int MaxDim2 = 0; //Dim2 = columns
            int maxRow = A.GetLength(0);
            int maxCol = A.GetLength(1);
            Max = A[0, 0];
            Min = A[0, 0];
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
        public static double[] MinR(double[] A)
        {
            double Min = 0.0;
            int MinDim = 0; //Dim1 = rows
            int maxRow = A.GetLength(0);
            Min = A[0];
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
        public static double[,] GetColumns(int[] ColumnList, double[,] A)
        {
            int NumberOfValidColumns = 0;
            int[] ValidColumns = new int[ColumnList.GetLength(0)];
            for (int k = 0; k < ColumnList.GetLength(0); k++)
            {
                if (ColumnList[k] >= 0 && ColumnList[k] < A.GetLength(1))
                {
                    ValidColumns[NumberOfValidColumns] = ColumnList[k];
                    NumberOfValidColumns++;
                }
            }
            double[,] Columns = new double[A.GetLength(0), NumberOfValidColumns];

            for (int k = 0; k < NumberOfValidColumns; k++)
            {
                for (int j = 0; j < A.GetLength(0); j++)
                {
                    Columns[j, k] = A[j, ValidColumns[k]];
                }
            }
            return Columns;
        }
        public static double[] GetColumns(int[] ColumnList, double[] A)
        {
            int NumberOfValidColumns = 0;
            int[] ValidColumns = new int[ColumnList.GetLength(0)];
            for (int k = 0; k < ColumnList.GetLength(0); k++)
            {
                if (ColumnList[k] >= 0 && ColumnList[k] < A.GetLength(0))
                {
                    ValidColumns[NumberOfValidColumns] = ColumnList[k];
                    NumberOfValidColumns++;
                }
            }
            double[] Columns = new double[NumberOfValidColumns];

            for (int k = 0; k < NumberOfValidColumns; k++)
            {
                Columns[k] = A[ValidColumns[k]];
            }
            return Columns;
        }
        public static int[] GetColumns(int[] ColumnList, int[] A)
        {
            int NumberOfValidColumns = 0;
            int[] ValidColumns = new int[ColumnList.GetLength(0)];
            for (int k = 0; k < ColumnList.GetLength(0); k++)
            {
                if (ColumnList[k] >= 0 && ColumnList[k] < A.GetLength(0))
                {
                    ValidColumns[NumberOfValidColumns] = ColumnList[k];
                    NumberOfValidColumns++;
                }
            }
            int[] Columns = new int[NumberOfValidColumns];

            for (int k = 0; k < NumberOfValidColumns; k++)
            {
                Columns[k] = A[ValidColumns[k]];
            }
            return Columns;
        }
        public static double[,] GetRows(int[] RowList, double[,] A)
        {
            int NumberOfValidRows = 0;
            int[] ValidRows = new int[RowList.GetLength(0)];
            for (int k = 0; k < RowList.GetLength(0); k++)
            {
                if (RowList[k] >= 0 && RowList[k] < A.GetLength(0))
                {
                    ValidRows[NumberOfValidRows] = RowList[k];
                    NumberOfValidRows++;
                }
            }
            double[,] Rows = new double[NumberOfValidRows, A.GetLength(1)];

            for (int k = 0; k < NumberOfValidRows; k++)
            {
                for (int j = 0; j < A.GetLength(1); j++)
                {
                    Rows[k, j] = A[ValidRows[k], j];
                }
            }
            return Rows;
        }
        public static double[,] JoinCol(double[,] Matrix1, double[,] Matrix2)
        {

            int NumRows = 0;
            int NumCols = 0;
            int Mat1Cols = 0;
            int Mat2Cols = 0;
            double[,] JointMatrix;
            if (Matrix1.GetLength(0) != Matrix2.GetLength(0))
                throw new Exception("MatrixMath Size Error: Row dimensions do not match for matrix1 and matrix2");
            else
            {
                NumRows = Matrix1.GetLength(0);
                NumCols = Matrix1.GetLength(1) + Matrix2.GetLength(1);
                Mat1Cols = Matrix1.GetLength(1);
                Mat2Cols = Matrix2.GetLength(1);

                JointMatrix = new double[NumRows, NumCols];

                for (int j = 0; j < Mat1Cols; j++)
                {
                    for (int k = 0; k < NumRows; k++)
                    {
                        JointMatrix[k, j] = Matrix1[k, j];
                    }
                }

                for (int j = 0; j < Mat2Cols; j++)
                {
                    for (int k = 0; k < NumRows; k++)
                    {
                        JointMatrix[k, j + Mat1Cols] = Matrix2[k, j];
                    }
                }
                return JointMatrix;
            }
        }
        public static double[,] JoinRow(double[,] Matrix1, double[,] Matrix2)
        {

            int NumRows = 0;
            int NumCols = 0;
            int Mat1Rows = 0;
            int Mat2Rows = 0;
            double[,] JointMatrix;
            if (Matrix1.GetLength(1) != Matrix2.GetLength(1))
                throw new Exception("MatrixMath Size Error: Column dimensions do not match for matrix1 and matrix2");
            else
            {
                NumRows = Matrix1.GetLength(0) + Matrix2.GetLength(0);
                NumCols = Matrix1.GetLength(1);
                Mat1Rows = Matrix1.GetLength(0);
                Mat2Rows = Matrix2.GetLength(0);
                JointMatrix = new double[NumRows, NumCols];

                for (int j = 0; j < Mat1Rows; j++)
                {
                    for (int k = 0; k < NumCols; k++)
                    {
                        JointMatrix[j, k] = Matrix1[j, k];
                    }
                }

                for (int j = 0; j < Mat2Rows; j++)
                {
                    for (int k = 0; k < NumCols; k++)
                    {
                        JointMatrix[j + Mat1Rows, k] = Matrix2[j, k];
                    }
                }
            }
            return JointMatrix;

        }
        public static double[] JoinArr(double[] Array1, double[] Array2)
        {
            int Mat1Elements = Array1.GetLength(0);
            int Mat2Elements = Array2.GetLength(0);
            int NumElements = Mat1Elements + Mat2Elements;
            double[] JointArray = new double[NumElements];

            for (int j = 0; j < Mat1Elements; j++)
                JointArray[j] = Array1[j];

            for (int j = 0; j < Mat2Elements; j++)
                JointArray[j + Mat1Elements] = Array2[j];

            return JointArray;
        }
        public static int[] JoinArr(int[] Array1, int[] Array2)
        {
            int Mat1Elements = Array1.GetLength(0);
            int Mat2Elements = Array2.GetLength(0);
            int NumElements = Mat1Elements + Mat2Elements;
            int[] JointArray = new int[NumElements];

            for (int j = 0; j < Mat1Elements; j++)
                JointArray[j] = Array1[j];

            for (int j = 0; j < Mat2Elements; j++)
                JointArray[j + Mat1Elements] = Array2[j];

            return JointArray;
        }
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

