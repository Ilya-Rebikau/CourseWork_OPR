using System.Text;

namespace CourseWork.Models
{
    /// <summary>
    /// Представляет классы матрицы.
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Инициализирует объект <see cref="Matrix"/>.
        /// </summary>
        /// <param name="numbers">Числа матрицы.</param>
        public Matrix(List<List<float?>> numbers)
        {
            StartNumbers = CopyNumbers(numbers);
            Numbers = CopyNumbers(numbers);
            GetColumns();
        }

        /// <summary>
        /// Инициализирует объект <see cref="Matrix"/>.
        /// </summary>
        /// <param name="numbers">Числа матрицы.</param>
        /// <param name="lowerBorder">Нижняя граница прошлой матрицы.</param>
        /// <param name="previousMatrix">Прошлая матрица.</param>
        public Matrix(List<List<float?>> numbers, float lowerBorder, Matrix previousMatrix)
        {
            CastConst = lowerBorder;
            StartNumbers = CopyNumbers(numbers);
            Numbers = CopyNumbers(numbers);
            GetColumns();
            PreviousMatrix = previousMatrix;
        }

        /// <summary>
        /// Предыдущая матрица.
        /// </summary>
        public Matrix PreviousMatrix { get; set; }

        /// <summary>
        /// Использовалась ли эта матрица в методе ветвей и границ или нет.
        /// </summary>
        public bool WasUsed = false;

        /// <summary>
        /// Нижняя граница матрицы.
        /// </summary>
        public float LowerBorder => CastConst + H;

        /// <summary>
        /// Начальные числа матрицы при её создании.
        /// </summary>
        public List<List<float?>> StartNumbers { get; }

        /// <summary>
        /// Константа приведения предыдущей матрицы или самой первой матрицы.
        /// </summary>
        public float CastConst { get; set; }

        /// <summary>
        /// Текущие числа матрицы.
        /// </summary>
        public List<List<float?>> Numbers { get; set; }

        /// <summary>
        /// Текущие столбцы матрицы.
        /// </summary>
        public List<List<float?>> Columns { get; set; }

        /// <summary>
        /// Координаты ветвления для метода ветвей и границ.
        /// </summary>
        public List<int> BranchingCoords { get; set; }

        /// <summary>
        /// Константа приведения текущей матрицы.
        /// </summary>
        public float H { get; set; }

        /// <summary>
        /// Проверка на то, что матрица размеров 2 на 2.
        /// </summary>
        /// <returns>true если матрицы размерностью 2 и false если нет.</returns>
        public bool CheckFor2x2()
        {
            var elements = 0;
            foreach (var row in Numbers)
            {
                foreach (var number in row)
                {
                    if (number is not null)
                    {
                        elements++;
                    }
                }
            }

            if (elements == 4)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Инициализирует текущую константу приведения.
        /// </summary>
        public void InitializetH()
        {
            H = 0;
            foreach (var row in Numbers)
            {
                if (row is not null)
                {
                    var min = row.Where(n => n is not null).Min();
                    if (min is not null)
                    {
                        H += (float)min;
                    }
                }
            }

            GetColumns();
            foreach (var column in Columns)
            {
                if (column is not null)
                {
                    var min = column.Where(n => n is not null).Min();
                    if (min is not null)
                    {
                        H += (float)min;
                    }
                }
            }
        }

        /// <summary>
        /// Получает матрицу без дуги.
        /// </summary>
        /// <returns>Матрица без дуги.</returns>
        public Matrix GetMatrixWithoutArc()
        {
            var newNumbers = CopyNumbers(Numbers);
            if (newNumbers[BranchingCoords[0]][BranchingCoords[1]] is not null)
            {
                newNumbers[BranchingCoords[0]][BranchingCoords[1]] = float.PositiveInfinity;
            }

            return new Matrix(newNumbers, LowerBorder, this);
        }

        /// <summary>
        /// Получает матрицу с дугой.
        /// </summary>
        /// <returns>Матрица с дугой.</returns>
        public Matrix GetMatrixWithArc()
        {
            var newNumbers = CopyNumbers(Numbers);
            for (int i = 0; i < newNumbers.Count; i++)
            {
                for (int j = 0; j < newNumbers[i].Count; j++)
                {
                    if (i == BranchingCoords[0] || j == BranchingCoords[1])
                    {
                        newNumbers[i][j] = null;
                    }
                }
            }

            if (newNumbers[BranchingCoords[1]][BranchingCoords[0]] is not null)
            {
                newNumbers[BranchingCoords[1]][BranchingCoords[0]] = float.PositiveInfinity;
            }

            return new Matrix(newNumbers, LowerBorder, this);
        }

        /// <summary>
        /// Инициализирует координаты ветвления.
        /// </summary>
        public void InitializeBranchingCoords()
        {
            var zeroDegrees = new Dictionary<List<int>, float?>();
            for (int i = 0; i < Numbers.Count; i++)
            {
                for (int j = 0; j < Numbers[i].Count; j++)
                {
                    if (Numbers[i][j] == 0)
                    {
                        var row = Numbers[i].ToList();
                        row.RemoveAt(j);
                        var minInRow = row.Where(n => n is not null).Min();
                        var column = Columns[j].ToList();
                        column.RemoveAt(i);
                        var minInColumn = column.Where(n => n is not null).Min();
                        zeroDegrees.Add(new List<int> { i, j }, minInRow + minInColumn);
                    }
                }
            }

            var maxDegree = zeroDegrees.Values.Max();
            BranchingCoords = zeroDegrees.FirstOrDefault(x => x.Value == maxDegree).Key;
        }

        /// <summary>
        /// Вычитает минимальные значения из строк.
        /// </summary>
        public void SubstractMinInRows()
        {
            foreach (var row in Numbers)
            {
                var minValueInRow = row.Where(n => n is not null).Min();
                for (int i = 0; i < row.Count; i++)
                {
                    row[i] = row[i] - minValueInRow;
                }
            }

            GetColumns();
        }

        /// <summary>
        /// Вычитает минимальные значения из столбцов.
        /// </summary>
        public void SubstractMinInColumns()
        {
            foreach (var column in Columns)
            {
                var minValueInColumn = column.Where(n => n is not null).Min();
                for (int i = 0; i < column.Count; i++)
                {
                    column[i] = column[i] - minValueInColumn;
                }
            }

            GetNumbers();
        }

        /// <summary>
        /// Инициализирует константу приведения для самой первой матрицы.
        /// </summary>
        public void InitializeCastConst()
        {
            var rowsMins = new List<float?>();
            foreach (var row in StartNumbers)
            {
                rowsMins.Add(row.Min());
            }

            var columnsMins = new List<float?>();
            foreach (var column in Columns)
            {
                columnsMins.Add(column.Min());
            }

            CastConst = (float)(rowsMins.Sum() + columnsMins.Sum());
        }

        /// <summary>
        /// Получает столцбы матрицы.
        /// </summary>
        private void GetColumns()
        {
            Columns = new List<List<float?>>();
            for (int i = 0; i < Numbers.Count; i++)
            {
                var column = new List<float?>();
                for (int j = 0; j < Numbers[i].Count; j++)
                {
                    column.Add(Numbers[j][i]);
                }

                Columns.Add(column);
            }
        }

        /// <summary>
        /// Получает строки матрицы.
        /// </summary>
        private void GetNumbers()
        {
            Numbers = new List<List<float?>>();
            for (int i = 0; i < Columns.Count; i++)
            {
                var row = new List<float?>();
                for (int j = 0; j < Columns[i].Count; j++)
                {
                    row.Add(Columns[j][i]);
                }

                Numbers.Add(row);
            }
        }

        /// <summary>
        /// Глубокое копирование чисел матрицы.
        /// </summary>
        /// <param name="numbers">Старый лист чисел матрицы.</param>
        /// <returns>Новый лист чисел матрицы.</returns>
        private static List<List<float?>> CopyNumbers(List<List<float?>> numbers)
        {
            var copiedNumbers = new List<List<float?>>();
            for (int i = 0; i < numbers.Count; i++)
            {
                var row = new List<float?>();
                for (int j = 0; j < numbers[i].Count; j++)
                {
                    row.Add(numbers[i][j]);
                }

                copiedNumbers.Add(row);
            }

            return copiedNumbers;
        }
    }
}
