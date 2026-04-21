using DataProcessor.Implementations;
using DataProcessor.Interfaces;
using DataProcessor.Services;

namespace DataProcessor;

internal static class Program
{
    private const string InputFilePath = "input.csv";
    private const string OutputFilePath = "output.csv";
    private const string JsonOutputPath = "output.json";
    private const string XmlOutputPath = "output.xml";
    private const string JsonTestOutputPath = "output_test.json";
    private const string XmlTestOutputPath = "output_test.xml";
    private const int SampleRecordCount = 50;
    private const int FilterThreshold = 100;

    public static void Main(string[] args)
    {
        PrepareSampleInput();

        var processor = CreateProcessor();
        ConfigureProcessor(processor);

        processor.ProcessData();
        processor.DisplayStatistics();
        ExportResults(processor);
        ShowSummary(processor);
    }

    private static DataProcessingService CreateProcessor()
    {
        IDataReader reader = new CsvFileReader();
        IDataParser parser = new CsvParser();
        IDataValidator validator = new RecordValidator();
        IDataTransformer transformer = new RecordTransformer();
        ILogger logger = new FileLogger();

        return new DataProcessingService(reader, parser, validator, transformer, logger)
        {
            InputFilePath = InputFilePath,
            OutputFilePath = OutputFilePath
        };
    }

    private static void ConfigureProcessor(DataProcessingService processor)
    {
        processor.ValidateData = true;
        processor.TransformData = true;
        processor.DateFormat = "MM/dd/yyyy";
        processor.BatchSize = 50;
    }

    private static void PrepareSampleInput()
    {
        DataProcessingService.GenerateSampleData(InputFilePath, SampleRecordCount);
    }

    private static void ExportResults(DataProcessingService processor)
    {
        processor.ExportToJson(JsonOutputPath);
        processor.ExportToXml(XmlOutputPath);

        try
        {
            processor.ExportByFormat(JsonTestOutputPath, "json");
            processor.ExportByFormat(XmlTestOutputPath, "xml");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Export error: {ex.Message}");
        }
    }

    private static void ShowSummary(DataProcessingService processor)
    {
        var filteredRecords = processor.FilterByValue(FilterThreshold);

        Console.WriteLine($"\nFiltered records: {filteredRecords.Count}");
        Console.WriteLine($"\nRecords processed: {processor.RecordsProcessed}");
        Console.WriteLine($"Errors: {processor.ErrorCount}");
    }
}
