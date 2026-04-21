using System.Text;
using DataProcessor.Interfaces;
using DataProcessor.Implementations;
using DataProcessor.Models;

namespace DataProcessor.Services;

public class DataProcessingService
{
    private readonly IDataReader _reader;
    private readonly IDataParser _parser;
    private readonly IDataValidator _validator;
    private readonly IDataTransformer _transformer;
    private readonly ILogger _logger;

    private readonly List<string> _rawData = new();
    private List<Record> _parsedRecords = new();
    private readonly StringBuilder _logBuffer = new();

    public string InputFilePath { get; set; } = string.Empty;
    public string OutputFilePath { get; set; } = string.Empty;
    public int RecordsProcessed { get; private set; }
    public int ErrorCount { get; private set; }
    public List<string> ErrorMessages { get; } = new();
    public bool ValidateData { get; set; } = true;
    public bool TransformData { get; set; } = true;
    public Dictionary<string, int> Statistics { get; } = new();
    public string LogFilePath { get; set; } = "processing.log";
    public string DateFormat { get; set; } = "yyyy-MM-dd";
    public int BatchSize { get; set; } = 100;

    public DataProcessingService(
        IDataReader reader,
        IDataParser parser,
        IDataValidator validator,
        IDataTransformer transformer,
        ILogger logger)
    {
        _reader = reader;
        _parser = parser;
        _validator = validator;
        _transformer = transformer;
        _logger = logger;
    }

    public List<Record> Process(string filePath)
    {
        InputFilePath = filePath;
        ResetState();

        try
        {
            LoadAndProcessRecords(filePath);
            CalculateStatistics();
        }
        catch (Exception ex)
        {
            RecordFatalError(ex);
        }

        return _parsedRecords.ToList();
    }

    public void ProcessData()
    {
        if (string.IsNullOrWhiteSpace(InputFilePath))
        {
            throw new InvalidOperationException("InputFilePath must be set before calling ProcessData().");
        }

        if (string.IsNullOrWhiteSpace(OutputFilePath))
        {
            throw new InvalidOperationException("OutputFilePath must be set before calling ProcessData().");
        }

        ResetState();

        try
        {
            Log("Starting data processing");
            LoadAndProcessRecords(InputFilePath);
            CalculateStatistics();
            WriteCsvOutput(OutputFilePath);
            WriteLogFile();

            Console.WriteLine("Processing complete!");
            Console.WriteLine($"Records processed: {RecordsProcessed}");
            Console.WriteLine($"Errors: {ErrorCount}");
        }
        catch (Exception ex)
        {
            RecordFatalError(ex);
            WriteLogFile();
            Console.WriteLine($"Processing failed: {ex.Message}");
        }
    }

    public void DisplayStatistics()
    {
        Console.WriteLine("\n=== Processing Statistics ===");
        foreach (var stat in Statistics)
        {
            Console.WriteLine($"{stat.Key}: {stat.Value}");
        }

        if (ErrorMessages.Count > 0)
        {
            Console.WriteLine("\n=== Errors ===");
            foreach (var error in ErrorMessages)
            {
                Console.WriteLine($"- {error}");
            }
        }
    }

    public void ExportToJson(string jsonFilePath)
    {
        Log($"Exporting to JSON: {jsonFilePath}");

        var exporter = new JsonExporter();
        exporter.Export(jsonFilePath, _parsedRecords);

        Log("JSON export complete");
    }

    public void ExportToXml(string xmlFilePath)
    {
        Log($"Exporting to XML: {xmlFilePath}");

        var exporter = new XmlExporter();
        exporter.Export(xmlFilePath, _parsedRecords);

        Log("XML export complete");
    }

    public void ExportByFormat(string filePath, string format)
    {
        switch (format.ToLowerInvariant())
        {
            case "json":
                ExportToJson(filePath);
                break;
            case "xml":
                ExportToXml(filePath);
                break;
            case "csv":
                WriteCsvOutput(filePath);
                break;
            default:
                throw new ArgumentException($"Unsupported format: {format}");
        }
    }

    public List<Record> FilterByValue(double minValue)
    {
        var filtered = new List<Record>();

        foreach (var record in _parsedRecords)
        {
            if (record.Value >= minValue)
            {
                filtered.Add(record);
            }
        }

        Log($"Filtered {filtered.Count} records with value >= {minValue}");
        return filtered;
    }

    public void UpdateConfiguration(string dateFormatNew, int batchSizeNew, bool validate, bool transform)
    {
        DateFormat = dateFormatNew;
        BatchSize = batchSizeNew;
        ValidateData = validate;
        TransformData = transform;

        Log($"Configuration updated: dateFormat={DateFormat}, batchSize={BatchSize}");
    }

    public static void GenerateSampleData(string filePath, int recordCount)
    {
        var lines = new List<string>();
        var random = new Random();

        for (int i = 1; i <= recordCount; i++)
        {
            string id = $"ID{i:D4}";
            string name = $"Item{i}";
            double value = random.Next(10, 1000);
            DateTime date = DateTime.Now.AddDays(-random.Next(0, 365));

            lines.Add($"{id},{name},{value},{date:yyyy-MM-dd}");
        }

        File.WriteAllLines(filePath, lines);
        Console.WriteLine($"Generated {recordCount} sample records in {filePath}");
    }

    private void LoadAndProcessRecords(string filePath)
    {
        Log($"Reading input file: {filePath}");
        _rawData.Clear();
        _rawData.AddRange(_reader.Read(filePath));
        Log($"Read {_rawData.Count} lines");

        Log("Parsing data...");
        foreach (var line in _rawData)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                var record = _parser.Parse(line);

                if (ValidateData && !_validator.IsValid(record))
                {
                    ErrorCount++;
                    AddValidationErrors(record);
                    continue;
                }

                if (TransformData)
                {
                    _transformer.Transform(record);
                }
                else
                {
                    SyncDerivedValues(record);
                }

                _parsedRecords.Add(record);
            }
            catch (FormatException)
            {
                ErrorCount++;
                ErrorMessages.Add($"Invalid line format: {line}");
                Log($"ERROR: Invalid line format: {line}");
            }
            catch (Exception ex)
            {
                ErrorCount++;
                ErrorMessages.Add($"Error processing line '{line}': {ex.Message}");
                Log($"ERROR: {ex.Message}");
            }
        }

        Log($"Parsed {_parsedRecords.Count} records");
    }

    private void CalculateStatistics()
    {
        Log("Calculating statistics...");
        Statistics["total_records"] = _parsedRecords.Count;
        Statistics["error_count"] = ErrorCount;

        double totalValue = 0;
        foreach (var record in _parsedRecords)
        {
            totalValue += record.Value;
        }

        Statistics["total_value"] = (int)totalValue;
        Statistics["average_value"] = _parsedRecords.Count > 0
            ? (int)(totalValue / _parsedRecords.Count)
            : 0;

        RecordsProcessed = _parsedRecords.Count;
        Log($"Statistics calculated: {Statistics.Count} metrics");
    }

    private void WriteCsvOutput(string filePath)
    {
        Log($"Writing output to: {filePath}");

        var outputLines = new List<string>
        {
            "ID,NAME,VALUE,DATE,DOUBLED_VALUE,SQUARED_VALUE"
        };

        foreach (var record in _parsedRecords)
        {
            var dateValue = record.Date.HasValue
                ? record.Date.Value.ToString(DateFormat)
                : record.RawDate ?? string.Empty;

            var line =
                $"{record.Id}," +
                $"{record.Name}," +
                $"{GetValueText(record)}," +
                $"{dateValue}," +
                $"{record.DoubledValue}," +
                $"{record.SquaredValue}";

            outputLines.Add(line);
        }

        File.WriteAllLines(filePath, outputLines);
        OutputFilePath = filePath;
        RecordsProcessed = _parsedRecords.Count;

        Log($"Output written. {RecordsProcessed} records processed");
    }

    private static string GetValueText(Record record)
    {
        return record.RawValue ?? record.Value.ToString();
    }

    private void SyncDerivedValues(Record record)
    {
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

    private void AddValidationErrors(Record record)
    {
        if (string.IsNullOrWhiteSpace(record.Id))
        {
            ErrorMessages.Add("Record missing ID");
        }

        if (string.IsNullOrWhiteSpace(record.Name))
        {
            ErrorMessages.Add($"Record {record.Id} missing name");
        }

        if (string.IsNullOrWhiteSpace(record.RawValue) ||
            !double.TryParse(record.RawValue, out _))
        {
            ErrorMessages.Add($"Record {record.Id} has invalid value");
        }
    }

    private void ResetState()
    {
        RecordsProcessed = 0;
        ErrorCount = 0;
        ErrorMessages.Clear();
        Statistics.Clear();
        _rawData.Clear();
        _parsedRecords = new List<Record>();
        _logBuffer.Clear();
    }

    private void RecordFatalError(Exception ex)
    {
        ErrorCount++;
        ErrorMessages.Add($"Fatal error: {ex.Message}");
        Log($"FATAL ERROR: {ex.Message}");
    }

    private void WriteLogFile()
    {
        File.WriteAllText(LogFilePath, _logBuffer.ToString());
    }

    private void Log(string message)
    {
        _logger.Log(message);

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        _logBuffer.AppendLine($"[{timestamp}] {message}");
    }
}
