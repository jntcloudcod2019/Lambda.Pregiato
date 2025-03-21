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

    public void LogAction(string action, string user, DateTime timestamp)
    {
        Console.WriteLine($"[ACTION] {timestamp:yyyy-MM-dd HH:mm:ss} - Usuário '{user}' realizou a ação: {action}");
    }

    public void LogVariables(params (string Name, object Value)[] variables)
    {
        Console.WriteLine($"[VARIABLES] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - Variáveis:");
        foreach (var variable in variables)
        {
            Console.WriteLine($"  {variable.Name}: {variable.Value}");
        }
    }
}