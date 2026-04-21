using DataProcessor.Interfaces;
using DataProcessor.Models;

namespace DataProcessor.Implementations;

public class RecordTransformer : IDataTransformer
{
    public void Transform(Record record)
    {
        record.Name = record.Name.ToUpperInvariant();

        if (double.TryParse(record.RawValue, out var value))
        {
            record.Value = value;
            record.DoubledValue = value * 2;
            record.SquaredValue = value * value;
        }

        if (!string.IsNullOrWhiteSpace(record.RawDate) &&
            DateTime.TryParse(record.RawDate, out var date))
        {
            record.Date = date;
        }
    }
}
