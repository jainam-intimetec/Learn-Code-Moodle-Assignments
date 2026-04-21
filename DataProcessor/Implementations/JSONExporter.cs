using DataProcessor.Interfaces;
using DataProcessor.Models;
using System.Text.Json;

namespace DataProcessor.Implementations;

public class JsonExporter : IDataExporter
{
    public void Export(string path, IEnumerable<Record> records)
    {
        var json = JsonSerializer.Serialize(records, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(path, json);
    }
}