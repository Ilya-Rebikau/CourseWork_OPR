using CourseWork.Models;

namespace CourseWork.BLL.Interfaces
{
    /// <summary>
    /// Интерфейс решателя.
    /// </summary>
    public interface ISolver
    {
        /// <summary>
        /// Длина контура.
        /// </summary>
        float ContourLength { get; set; }

        /// <summary>
        /// Координаты ветвления.
        /// </summary>
        List<List<int>> BranchingCoords { get; set; }

        /// <summary>
        /// Список всех матриц.
        /// </summary>
        List<Matrix> MatrixList { get; set; }

        /// <summary>
        /// Решить задачу и найти минимальный контур.
        /// </summary>
        /// <param name="startNumbers"></param>
        /// <param name="lowerBorder"></param>
        /// <param name="previousMatrix"></param>
        /// <param name="fileName"></param>
        /// <returns>Возвращает true если задача решена и false если нет.</returns>
        bool Solve(List<List<float?>> startNumbers, float lowerBorder, Matrix previousMatrix, string fileName);

        /// <summary>
        /// Вывести контур в виде строки из замкнутой последовательности координат.
        /// </summary>
        /// <returns></returns>
        string GetCountourString();

        /// <summary>
        /// Очистить данные.
        /// </summary>
        void Clear();
    }
}
