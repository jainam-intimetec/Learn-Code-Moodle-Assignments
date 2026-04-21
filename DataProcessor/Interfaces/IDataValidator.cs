using DataProcessor.Models;

namespace DataProcessor.Interfaces;

public interface IDataValidator
{
    bool IsValid(Record record);
}