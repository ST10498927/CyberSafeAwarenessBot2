
using System;

namespace CyberSafeAwarenessBot2.Data
{
    public static class DatabaseConfig
    {
        public static string ConnectionString
        {
            get
            {
                string? password =
                    Environment.GetEnvironmentVariable("CYBERSAFE_DB_PASSWORD");

                if (string.IsNullOrWhiteSpace(password))
                {
                    throw new InvalidOperationException(
                        "Database password not configured. Set CYBERSAFE_DB_PASSWORD locally.");
                }

                return "Server=localhost;Port=3306;" +
                       "Database=CyberSecurityBotDB;" +
                       "Uid=root;Pwd=" + password + ";";
            }
        }
    }
}
