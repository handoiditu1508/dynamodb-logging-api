using Lollipop.Helpers.Extensions;
using Microsoft.Extensions.Configuration;

namespace Lollipop.Helpers
{
    public static class AppSettings
    {
        private static IConfigurationRoot _configuration;

        public static IConfigurationRoot Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var appsettingsFileName = EnvironmentVariable.AspNetCoreEnvironment.IsNullOrWhiteSpace() ?
                        "appsettings.json" :
                        $"appsettings.{EnvironmentVariable.AspNetCoreEnvironment}.json";

                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(appsettingsFileName)
                        .Build();
                }
                return _configuration;
            }
        }

        public static class MongoLogging
        {
            public static long MaxCollectionSize => long.Parse(Configuration["MongoLogging:MaxCollectionSize"]);
            public static int MaxDocuments => int.Parse(Configuration["MongoLogging:MaxDocuments"]);
        }
    }
}
