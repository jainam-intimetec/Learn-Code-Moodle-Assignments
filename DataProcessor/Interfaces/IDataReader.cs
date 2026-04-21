namespace DataProcessor.Interfaces;

public interface IDataReader
{
    IEnumerable<string> Read(string path);
}