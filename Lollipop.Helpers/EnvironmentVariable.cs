namespace Lollipop.Helpers
{
    public static class EnvironmentVariable
    {
        public static string AspNetCoreEnvironment => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        public static string ConnectionString => Environment.GetEnvironmentVariable("Log_ConnectionString");
        public static string Database => Environment.GetEnvironmentVariable("Log_Database");
        public static string ApiKeyValue => Environment.GetEnvironmentVariable("ApiKeyValue");
    }
}
