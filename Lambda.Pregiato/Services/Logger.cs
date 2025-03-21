using System;

public class Logger
{
    // Método para logar mensagens genéricas
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }

    // Método para logar informações com variáveis
    public void LogInfo(string message, params object[] variables)
    {
        string formattedMessage = string.Format(message, variables);
        Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {formattedMessage}");
    }

    // Método para logar erros
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