using System.Diagnostics;
using System.Text;

namespace Lab7
{
    public class BranchesAndBoundariesSolver
    {
        private static List<List<double?>> _startNumbers;

        public BranchesAndBoundariesSolver(List<List<double?>> startNumbers)
        {
            _startNumbers = startNumbers;
        }

        public List<Matrix> MatrixList { get; set; } = new List<Matrix>();

        public List<List<int>> BranchingCoords { get; set; } = new List<List<int>>();

        public double ContourLength { get; set; }

        //public List<int> BranchingCoordsToRemove { get; set; } = new List<int>();

        public bool Solve(List<List<double?>> startNumbers, double lowerBorder = 0, Matrix previousMatrix = null)
        {
            var matrix = new Matrix(startNumbers);
            MatrixList.Add(matrix);
            //Debug.WriteLine($"Исходная матрица:\n {matrix}\n______________________________________________________");
            matrix.SubstractMinInRows();
            if (MatrixList.Count == 1)
            {
                matrix.InitializeCastConst();
            }
            else
            {
                matrix.CastConst = lowerBorder;
                matrix.PreviousMatrix = previousMatrix;
            }
            
            matrix.SubstractMinInColumns();
            matrix.InitializeBranchingCoords();
            BranchingCoords.Add(matrix.BranchingCoords);
            var matrixWithArc = matrix.GetMatrixWithArc();
            var matrixWithoutArc = matrix.GetMatrixWithoutArc();
            matrix.WasUsed = true;
            MatrixList.Add(matrixWithoutArc);
            MatrixList.Add(matrixWithArc);
            matrixWithArc.InitializetH();
            matrixWithoutArc.InitializetH();
            if (matrixWithArc.LowerBorder < matrixWithoutArc.LowerBorder)
            {
                matrix = new Matrix(matrixWithArc.Numbers, matrixWithArc.LowerBorder, matrix);
                matrixWithArc.WasUsed = true;
            }
            else
            {
                matrix = new Matrix(matrixWithoutArc.Numbers, matrixWithoutArc.LowerBorder, matrix);
                matrixWithoutArc.WasUsed = true;
            }

            while (!matrix.CheckFor2x2())
            {
                var previousMatrixLowerBorder = matrix.LowerBorder;
                //Debug.WriteLine($"Новая матрица:\n {matrix}\n______________________________________________________");
                AddRestrictions(matrix);
                matrix.SubstractMinInRows();
                matrix.SubstractMinInColumns();
                //Debug.WriteLine($"Матрица после приведения:\n {matrix}\n______________________________________________________");
                matrix.InitializeBranchingCoords();
                BranchingCoords.Add(matrix.BranchingCoords);
                matrixWithoutArc = matrix.GetMatrixWithoutArc();
                matrixWithArc = matrix.GetMatrixWithArc();
                matrix.WasUsed = true;
                MatrixList.Add(matrixWithoutArc);
                MatrixList.Add(matrixWithArc);
                matrixWithArc.InitializetH();
                //Debug.WriteLine($"Матрица с дугой:\n {matrixWithArc}");
                //Debug.WriteLine($"Border = {matrixWithArc.LowerBorder}\n______________________________________________________");
                matrixWithoutArc.InitializetH();
                //Debug.WriteLine($"Матрица без дуги:\n {matrixWithoutArc}");
                //Debug.WriteLine($"Border = {matrixWithoutArc.LowerBorder}\n______________________________________________________");
                var minLowerBorder = MatrixList.Where(m => m.WasUsed is false).Min(m => m.LowerBorder);
                if (matrixWithArc.LowerBorder <= matrixWithoutArc.LowerBorder)
                {
                    matrix = new Matrix(matrixWithArc.Numbers, matrixWithArc.LowerBorder, matrix);
                    matrixWithArc.WasUsed = true;
                }
                else
                {
                    matrix = new Matrix(matrixWithoutArc.Numbers, matrixWithoutArc.LowerBorder, matrix);
                    matrixWithoutArc.WasUsed = true;
                }
                //if (previousMatrixLowerBorder < matrix.LowerBorder)
                //{
                //    BranchingCoordsToRemove.Add(BranchingCoords.Last()[0]);
                //    BranchingCoordsToRemove.Add(BranchingCoords.Last()[1]);
                //    return false;
                //}
            }

            AddRestrictions(matrix);
            for (int i = 0; i < matrix.Numbers.Count; i++)
            {
                for (int j = 0; j < matrix.Numbers[i].Count; j++)
                {
                    if (matrix.Numbers[i][j] == 0)
                    {
                        BranchingCoords.Add(new List<int> { i, j });
                    }
                }
            }

            //if (BranchingCoordsToRemove.Count > 0)
            //{
            //    for (int i = 0; i < BranchingCoords.Count; i++)
            //    {
            //        if (BranchingCoords[i][0] == BranchingCoordsToRemove[0] && BranchingCoords[i][1] == BranchingCoordsToRemove[1])
            //        {
            //            BranchingCoords.RemoveAt(i);
            //        }
            //    }

            //    BranchingCoordsToRemove.Clear();
            //}

            ContourLength = 0;
            for (int i = 0; i < BranchingCoords.Count; i++)
            {
                var number = _startNumbers[BranchingCoords[i][0]][BranchingCoords[i][1]];
                if (number is not null)
                {
                    ContourLength += (double)number;
                }
            }

            if (ContourLength >= matrix.LowerBorder)
            {
                return true;
            }

            return false;
        }

        public string GetCountourString()
        {
            var resultSb = new StringBuilder();
            int currentPoint = 0;
            for (int i = 0; i < BranchingCoords.Count; i++)
            {
                if (BranchingCoords[i][0] == currentPoint)
                {
                    currentPoint = BranchingCoords[i][1];
                    resultSb.Append($"{BranchingCoords[i][0] + 1} → {BranchingCoords[i][1] + 1}");
                    break;
                }
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (currentPoint != 0)
            {
                for (int i = 0; i < BranchingCoords.Count; i++)
                {
                    if (currentPoint == BranchingCoords[i][0])
                    {
                        currentPoint = BranchingCoords[i][1];
                        resultSb.Append($" → {currentPoint + 1}");
                    }
                }
                
                if (stopwatch.ElapsedMilliseconds > 10000)
                {
                    throw new TimeoutException("Задача не решаема!");
                }
            }

            return resultSb.ToString();
        }

        private static void AddRestrictions(Matrix matrix)
        {
            var pairs = new List<int>();
            for (int i = 0; i < matrix.Numbers.Count; i++)
            {
                var row = matrix.Numbers[i];
                for (int j = 0; j < matrix.Numbers[i].Count; j++)
                {
                    var column = matrix.Columns[j];
                    if (!row.All(x => x is null))
                    {
                        if (!column.All(x => x is null))
                        {
                            if (!row.Contains(double.PositiveInfinity) && !column.Contains(double.PositiveInfinity))
                            {
                                pairs.Add(i);
                                pairs.Add(j);
                            }
                        }
                    }

                }
            }
            for (int i = 0; i < pairs.Count; i += 2)
            {
                matrix.Numbers[pairs[i]][pairs[i + 1]] = double.PositiveInfinity;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < MatrixList.Count; i++)
            {
                sb.AppendLine(MatrixList[i].ToString());
                sb.AppendLine(MatrixList[i].WasUsed.ToString());
                sb.AppendLine("_____________________________");
            }

            return sb.ToString();
        }
    }
}
