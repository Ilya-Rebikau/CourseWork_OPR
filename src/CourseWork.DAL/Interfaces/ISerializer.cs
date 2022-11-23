using CourseWork.Models;

namespace CourseWork.DAL.Interfaces
{
    public interface ISerializer
    {
        Matrix DeserializeCsvMatrix(string fileNameWithPath);
    }
}
