namespace CyberSafeAwarenessBot2.Data
{
    public static class DatabaseConfig
    {
        public static string ConnectionString =
            "Server=localhost;Port=3306;" +
            "Database=CyberSecurityBotDB;" +
            "Uid=root;Pwd=" +
            Environment.GetEnvironmentVariable("CYBERSAFE_DB_PASSWORD");
    }
}