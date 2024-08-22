using Microsoft.Extensions.Configuration;
using System;
using System.IO;


namespace TestAutomation.Utilities
{
    public static class AppConfigReader
    {

        private static IConfigurationRoot configuration;

         static AppConfigReader()
        {

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("C:\\Users\\jeffe\\source\\repos\\TestAutomation\\TestAutomation\\Resources\\configs\\AppConfigs.json",
                optional: false, reloadOnChange: true);

            configuration = builder.Build();

        }

        public static string GetConfig(string key) { return configuration[key]; }


    }
}
