namespace Shared.Helpers;

public static class StoragePathHelper
{
    public static string GetDataFilePath(string fileName)
    {
        var workingDirectoryCandidate = FindDataDirectory(new DirectoryInfo(Environment.CurrentDirectory));
        if (workingDirectoryCandidate is not null)
            return Path.Combine(workingDirectoryCandidate.FullName, fileName);

        var baseDirectoryCandidate = FindDataDirectory(new DirectoryInfo(AppContext.BaseDirectory));
        if (baseDirectoryCandidate is not null)
            return Path.Combine(baseDirectoryCandidate.FullName, fileName);

        return Path.Combine(Environment.CurrentDirectory, "Data", fileName);
    }

    private static DirectoryInfo? FindDataDirectory(DirectoryInfo? startDirectory)
    {
        var currentDirectory = startDirectory;

        while (currentDirectory is not null)
        {
            var dataDirectoryPath = Path.Combine(currentDirectory.FullName, "Data");
            if (Directory.Exists(dataDirectoryPath))
                return new DirectoryInfo(dataDirectoryPath);

            currentDirectory = currentDirectory.Parent;
        }

        return null;
    }
}
