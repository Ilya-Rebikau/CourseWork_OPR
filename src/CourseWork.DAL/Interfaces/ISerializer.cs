using CourseWork.Models;

namespace CourseWork.DAL.Interfaces
{
    /// <summary>
    /// Интерфейс для сериализации и десериализации.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Десериализует матрицу из файла.
        /// </summary>
        /// <param name="fileNameWithPath">Путь к матрице.</param>
        /// <returns>Матрица.</returns>
        Matrix DeserializeMatrix(string fileNameWithPath);
    }
}
