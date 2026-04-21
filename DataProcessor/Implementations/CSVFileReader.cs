using DataProcessor.Interfaces;

namespace DataProcessor.Implementations;

public class CsvFileReader : IDataReader
{
    public IEnumerable<string> Read(string path)
    {
        return File.ReadAllLines(path);
    }
}