namespace ToDoEntityModels.DataContexts;

public static class ContextLogger
{
    public static void LoggContext(string message)
    {
        string logsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Loggs");
        if (!Directory.Exists(logsFolderPath)) Directory.CreateDirectory(logsFolderPath);
        string logsDateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string logsFile = Path.Combine(logsFolderPath, $"todo_loggs-{logsDateTime}.txt");

        using StreamWriter textWriter = File.AppendText(logsFile);
        textWriter.WriteLine(message);
        textWriter.Close();
    }
}
