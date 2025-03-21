using System;

public class Logger
{

    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }


    public void LogInfo(string message, params object[] variables)
    {
        string formattedMessage = string.Format(message, variables);
        Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {formattedMessage}");
    }


    public void LogError(string errorMessage, Exception ex = null)
    {
        if (ex != null)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {errorMessage}. Exception: {ex.Message}");
        }
        else
        {
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {errorMessage}");
        }
    }
}