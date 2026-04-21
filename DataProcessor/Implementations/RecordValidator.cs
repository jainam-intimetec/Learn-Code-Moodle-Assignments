using DataProcessor.Interfaces;
using DataProcessor.Models;

namespace DataProcessor.Implementations;

public class RecordValidator : IDataValidator
{
    public bool IsValid(Record record)
    {
        return !string.IsNullOrWhiteSpace(record.Id)
            && !string.IsNullOrWhiteSpace(record.Name)
            && !string.IsNullOrWhiteSpace(record.RawValue)
            && double.TryParse(record.RawValue, out _);
    }
}
