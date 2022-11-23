using System.Text;

namespace CourseWork.Models
{
    public class Matrix
    {
        public Matrix(List<List<float?>> numbers)
        {
            StartNumbers = CopyNumbers(numbers);
            Numbers = CopyNumbers(numbers);
            GetColumns();
        }

        public Matrix(List<List<float?>> numbers, float lowerBorder, Matrix previousMatrix)
        {
            CastConst = lowerBorder;
            StartNumbers = CopyNumbers(numbers);
            Numbers = CopyNumbers(numbers);
            GetColumns();
            PreviousMatrix = previousMatrix;
        }

        public Matrix PreviousMatrix { get; set; }

        public bool WasUsed = false;

        public float LowerBorder => CastConst + H;

        public List<List<float?>> StartNumbers { get; }

        public float CastConst { get; set; }

        public List<List<float?>> Numbers { get; set; }

        public List<List<float?>> Columns { get; set; }

        public List<int> BranchingCoords { get; set; }

        public float H { get; set; }

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

        public Matrix GetMatrixWithoutArc()
        {
            var newNumbers = CopyNumbers(Numbers);
            if (newNumbers[BranchingCoords[0]][BranchingCoords[1]] is not null)
            {
                newNumbers[BranchingCoords[0]][BranchingCoords[1]] = float.PositiveInfinity;
            }

            return new Matrix(newNumbers, LowerBorder, this);
        }

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

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var row in Numbers)
            {
                foreach (var number in row)
                {
                    if (number is null)
                    {
                        sb.Append("X ");
                    }

                    sb.Append(number.ToString() + " ");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

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
