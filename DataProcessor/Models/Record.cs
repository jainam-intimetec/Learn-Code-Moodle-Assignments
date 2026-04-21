namespace DataProcessor.Models;

public class Record
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RawValue { get; set; } = string.Empty;
    public string? RawDate { get; set; }
    public double Value { get; set; }
    public DateTime? Date { get; set; }
    public double DoubledValue { get; set; }
    public double SquaredValue { get; set; }
}
