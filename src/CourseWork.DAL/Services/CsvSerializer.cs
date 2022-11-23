using CourseWork.DAL.Interfaces;
using CourseWork.Models;

namespace CourseWork.DAL.Services
{
    internal class CsvSerializer : ISerializer
    {
        public Matrix DeserializeCsvMatrix(string fileNameWithPath)
        {
            var elements = new List<List<float?>>();
            var lines = File.ReadAllLines(fileNameWithPath);
            for (int i = 0; i < lines.Length; i++)
            {
                var row = new List<float?>();
                var elementsInLine = lines[i].Split(';', StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < elementsInLine.Length; j++)
                {
                    
                    if (elementsInLine[j] == "infinity")
                    {
                        row.Add(float.PositiveInfinity);
                    }
                    else
                    {
                        row.Add(float.Parse(elementsInLine[j]));
                    }
                }

                elements.Add(row);
            }

            return new Matrix(elements);
        }
    }
}
