using DataProcessor.Interfaces;
using DataProcessor.Models;
using System.Xml.Serialization;

namespace DataProcessor.Implementations;

public class XmlExporter : IDataExporter
{
    public void Export(string path, IEnumerable<Record> records)
    {
        var serializer = new XmlSerializer(typeof(List<Record>));

        using var writer = new StreamWriter(path);
        serializer.Serialize(writer, records.ToList());
    }
}