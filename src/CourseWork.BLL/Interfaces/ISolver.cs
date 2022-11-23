using CourseWork.Models;

namespace CourseWork.BLL.Interfaces
{
    public interface ISolver
    {
        float ContourLength { get; set; }
        List<List<int>> BranchingCoords { get; set; }
        List<Matrix> MatrixList { get; set; }
        bool Solve(List<List<float?>> startNumbers, float lowerBorder, Matrix previousMatrix, string fileName);
        string GetCountourString();
        void Clear();
    }
}
