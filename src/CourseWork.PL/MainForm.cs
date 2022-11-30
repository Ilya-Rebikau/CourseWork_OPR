using CourseWork.BLL.Interfaces;
using CourseWork.Models;

namespace CourseWork.PL
{
    public partial class MainForm : Form
    {
        private List<List<float?>> _startNumbers;
        private readonly ISolver _solver;
        public MainForm(ISolver solver)
        {
            InitializeComponent();
            _solver = solver;
            openFileDialog.Filter = "Csv файлы(*.csv)|*.csv";
            buttonReadMatrix.Click += ButtonReadMatrixAndCalculate_Click;
        }

        private void Calculate(string fileName)
        {
            float lowerBorder = 0;
            Matrix previousMatrix = null;
            bool solveResult = _solver.Solve(_startNumbers, lowerBorder, previousMatrix, fileName);
            while (!solveResult)
            {
                var matrixes = _solver.MatrixList.Where(m => m.WasUsed is false).OrderBy(m => m.LowerBorder).ToList();
                var currentMatrix = matrixes[0];
                _startNumbers = currentMatrix.Numbers;
                currentMatrix.WasUsed = true;
                _solver.BranchingCoords.Clear();
                while (currentMatrix.PreviousMatrix is not null)
                {
                    previousMatrix = currentMatrix.PreviousMatrix;
                    _solver.BranchingCoords.Add(previousMatrix.BranchingCoords);
                    currentMatrix = previousMatrix;
                }

                _solver.BranchingCoords.Reverse();
                lowerBorder = currentMatrix.LowerBorder;
                previousMatrix = currentMatrix.PreviousMatrix;
                solveResult = _solver.Solve(_startNumbers, lowerBorder, previousMatrix.PreviousMatrix, null);
            }

            richTextBox.AppendText($"Итоговый контур: {_solver.GetCountourString()}\n");
            richTextBox.AppendText($"Длина контура {_solver.ContourLength}\n");
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
                string fileName = openFileDialog.FileName;
                Calculate(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _solver.Clear();
            }
        }
    }
}