using DataProcessor.Models;

namespace DataProcessor.Interfaces;

public interface IDataTransformer
{
    void Transform(Record record);
}