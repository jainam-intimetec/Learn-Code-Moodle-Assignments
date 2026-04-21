using DataProcessor.Interfaces;

namespace DataProcessor.Implementations;

public class FileLogger : ILogger
{
    private const string LogFile = "processing.log";

    public void Log(string message)
    {
        var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        File.AppendAllText(LogFile, line + Environment.NewLine);
    }
}