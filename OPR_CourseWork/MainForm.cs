using Lab7;

namespace OPR_CourseWork
{
    public partial class MainForm : Form
    {
        private List<List<double?>> _startNumbers;

        public MainForm()
        {
            InitializeComponent();
            openFileDialog.Filter = "Text files(*.txt)|*.txt";
            buttonReadMatrix.Click += ButtonReadMatrixAndCalculate_Click;
            richTextBoxInfo.AppendText(
                "Данное ПО предназначено для решения задачи Коммивояжера методом ветвей и границ. " +
                "Данное решение позволит найти наиболее оптимальный маршрут для посещения всех необходимых городов.\n" +
                "Исходные данные должны быть в формате .txt и содержать матрицу расстояний между городами, где каждая новая строка матрицы " +
                "отделена новой строкой, а столбец — пробелом. Бесконечность должна обозначаться ключевым словом «inf». Пример исходного файла:\n" +
                "inf 1 2\n" +
                "3 inf 4\n" +
                "5 6 inf");
        }

        private void Calculate()
        {
            double lowerBorder = 0;
            var solver = new BranchesAndBoundariesSolver(_startNumbers);
            Matrix previousMatrix = null;
            bool solveResult = false;
            solveResult = solver.Solve(_startNumbers, lowerBorder, previousMatrix);
            while (!solveResult)
            {
                var matrixes = solver.MatrixList.Where(m => m.WasUsed is false).OrderBy(m => m.LowerBorder).ToList();
                _startNumbers = matrixes[0].Numbers;
                matrixes[0].WasUsed = true;
                solver.BranchingCoords.Clear();
                var currentMatrix = matrixes[0];
                while (currentMatrix.PreviousMatrix is not null)
                {
                    previousMatrix = currentMatrix.PreviousMatrix;
                    solver.BranchingCoords.Add(previousMatrix.BranchingCoords);
                    currentMatrix = previousMatrix;
                }

                solver.BranchingCoords.Reverse();
                lowerBorder = matrixes[0].LowerBorder;
                previousMatrix = matrixes[0].PreviousMatrix;
                solveResult = solver.Solve(_startNumbers, lowerBorder, previousMatrix);
            }

            richTextBox.AppendText($"Итоговый контур: {solver.GetCountourString()}\n");
            richTextBox.AppendText($"Длина контура {solver.ContourLength}\n");
            richTextBox.AppendText("Задача решена!\n");
        }

        private void ButtonReadMatrixAndCalculate_Click(object sender, EventArgs e)
        {
            try
            {


                if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                richTextBox.Clear();
                _startNumbers = new List<List<double?>>();
                string filename = openFileDialog.FileName;
                var fileTextLines = File.ReadLines(filename);
                for (int i = 0; i < fileTextLines.Count(); i++)
                {
                    var row = new List<double?>();
                    var numbersString = fileTextLines.ElementAt(i).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var numberString in numbersString)
                    {
                        if (numberString.ToLower() == "inf")
                        {
                            row.Add(double.PositiveInfinity);
                        }
                        else
                        {
                            row.Add(double.Parse(numberString));
                        }
                    }

                    _startNumbers.Add(row);
                }

                Calculate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}