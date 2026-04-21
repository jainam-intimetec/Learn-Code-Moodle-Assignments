using DataProcessor.Models;

namespace DataProcessor.Interfaces;

public interface IDataExporter
{
    void Export(string path, IEnumerable<Record> records);
}