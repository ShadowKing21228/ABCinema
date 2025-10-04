namespace ABCinema_WPF.Logging;

public static class AppLogger
{
    public static void LogInfo(string log)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("[INFO] " + log);
        Console.ResetColor();
    }
    
    public static void LogError(string log)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[INFO] " + log);
        Console.ResetColor();
    }
    
    public static void LogWarning(string log)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[INFO] " + log);
        Console.ResetColor();
    }
}