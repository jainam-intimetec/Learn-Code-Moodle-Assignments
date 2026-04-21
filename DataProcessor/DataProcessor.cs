using DataProcessor.Implementations;
using DataProcessor.Services;

namespace DataProcessor;

public class DataProcessor : DataProcessingService
{
    public DataProcessor(string inputFile, string outputFile)
        : base(
            new CsvFileReader(),
            new CsvParser(),
            new RecordValidator(),
            new RecordTransformer(),
            new FileLogger())
    {
        InputFilePath = inputFile;
        OutputFilePath = outputFile;
    }

    public static new void GenerateSampleData(string filePath, int recordCount)
    {
        DataProcessingService.GenerateSampleData(filePath, recordCount);
    }
}
