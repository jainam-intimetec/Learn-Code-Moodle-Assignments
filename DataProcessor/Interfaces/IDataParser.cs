using DataProcessor.Models;

namespace DataProcessor.Interfaces;

public interface IDataParser
{
    Record Parse(string line);
}