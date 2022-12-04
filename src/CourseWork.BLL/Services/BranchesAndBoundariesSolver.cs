using CourseWork.BLL.Interfaces;
using CourseWork.DAL.Interfaces;
using CourseWork.Models;
using System.Text;

namespace CourseWork.BLL.Services
{
    /// <summary>
    /// Решатель для нахождения контура и его длины методом ветвей и границ.
    /// </summary>
    public class BranchesAndBoundariesSolver : ISolver
    {
        /// <summary>
        /// Объект ISerializer для десериализации матрицы.
        /// </summary>
        private readonly ISerializer _serializer;

        /// <summary>
        /// Инициализирует новый объект класса <see cref="BranchesAndBoundariesSolver"/>.
        /// </summary>
        /// <param name="serializer">Объект ISerializer для десериализации матрицы.</param>
        public BranchesAndBoundariesSolver(ISerializer serializer)
        {
            _serializer = serializer;
        }

        /// <inheritdoc/>
        public List<Matrix> MatrixList { get; set; } = new List<Matrix>();

        /// <inheritdoc/>
        public List<List<int>> BranchingCoords { get; set; } = new List<List<int>>();

        /// <inheritdoc/>
        public float ContourLength { get; set; }

        /// <inheritdoc/>
        public bool Solve(List<List<float?>> startNumbers = null, float lowerBorder = 0, Matrix previousMatrix = null, string fileName = null)
        {
            Matrix matrix;
            if (fileName is not null)
            {
                matrix = _serializer.DeserializeMatrix(fileName);
            }
            else
            {
                matrix = new Matrix(startNumbers);
            }

            var startNumbersClone = DeepCloneListOfLists(matrix.Numbers);
            
            MatrixList.Add(matrix);
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
                AddRestrictions(matrix);
                matrix.SubstractMinInRows();
                matrix.SubstractMinInColumns();
                matrix.InitializeBranchingCoords();
                BranchingCoords.Add(matrix.BranchingCoords);
                matrixWithoutArc = matrix.GetMatrixWithoutArc();
                matrixWithArc = matrix.GetMatrixWithArc();
                matrix.WasUsed = true;
                MatrixList.Add(matrixWithoutArc);
                MatrixList.Add(matrixWithArc);
                matrixWithArc.InitializetH();
                matrixWithoutArc.InitializetH();
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

            ContourLength = 0;
            for (int i = 0; i < BranchingCoords.Count; i++)
            {
                ContourLength += (float)startNumbersClone[BranchingCoords[i][0]][BranchingCoords[i][1]];
            }

            if (ContourLength >= matrix.LowerBorder)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public string GetCountourString()
        {
            var resultSb = new StringBuilder();
            int currentPoint = 0;
            BranchingCoords = BranchingCoords.OrderBy(bc => bc[0]).ToList();
            for (int i = 0; i < BranchingCoords.Count; i++)
            {
                if (BranchingCoords[i][0] == currentPoint)
                {
                    currentPoint = BranchingCoords[i][1];
                    resultSb.Append($"{BranchingCoords[i][0] + 1} → {BranchingCoords[i][1] + 1}");
                    break;
                }
            }

            int cycleCount = 0;
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

                cycleCount++;
                if (cycleCount > Math.Pow(BranchingCoords.Count, 2) + 1)
                {
                    throw new InvalidOperationException("Задача не решаема!");
                }
            }

            return resultSb.ToString();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            MatrixList = new List<Matrix>();
            BranchingCoords = new List<List<int>>();
            ContourLength = 0;
        }

        /// <summary>
        /// Добавляет ограничения на матрицу если в строке и столбце нет ни одной бесконечности,
        /// то на их пересечении ставит бесконечность.
        /// </summary>
        /// <param name="matrix">Матрица.</param>
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
                            if (!row.Contains(float.PositiveInfinity) && !column.Contains(float.PositiveInfinity))
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
                matrix.Numbers[pairs[i]][pairs[i + 1]] = float.PositiveInfinity;
            }
        }

        /// <summary>
        /// Глубокое копирование листа.
        /// </summary>
        /// <param name="oldList">Старый лист.</param>
        /// <returns>Новый скопированный лист.</returns>
        private static List<List<float?>> DeepCloneListOfLists(List<List<float?>> oldList)
        {
            var newList = new List<List<float?>>();
            for (int i = 0; i < oldList.Count; i++)
            {
                var row = new List<float?>();
                for (int j = 0; j < oldList[i].Count; j++)
                {
                    row.Add(oldList[i][j]);
                }

                newList.Add(row);
            }

            return newList;
        }
    }
}
