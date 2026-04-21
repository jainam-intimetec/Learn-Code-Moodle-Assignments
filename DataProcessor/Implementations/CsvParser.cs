using DataProcessor.Interfaces;
using DataProcessor.Models;

namespace DataProcessor.Implementations;

public class CsvParser : IDataParser
{
    public Record Parse(string line)
    {
        var parts = line.Split(',');

        if (parts.Length < 3)
        {
            throw new FormatException("Invalid line format");
        }

        return new Record
        {
            Id = parts[0].Trim(),
            Name = parts[1].Trim(),
            RawValue = parts[2].Trim(),
            RawDate = parts.Length > 3 ? parts[3].Trim() : null
        };
    }
}
