using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpreadsheetEvaluator.Domain.Configuration;
using SpreadsheetEvaluator.Domain.Interfaces;
using SpreadsheetEvaluator.Domain.Services;
using SpreadsheetEvaluator.Domain.Utilities;
using System;
using System.IO;
using System.Net.Http;

namespace SpreadsheetEvaluator.Domain
{
    public static class Startup
    {
        public static IServiceProvider InitServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            
            ConfigureServices(serviceCollection);
            ConfigureDefaultApplicationSettings(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IHubService, HubService>();
            serviceCollection.AddScoped<ISpreadsheetReferenceService, SpreadsheetReferenceService>();
            serviceCollection.AddScoped<ISpreadsheetCreationService, SpreadsheetCreationService>();
            serviceCollection.AddScoped<IComputeCellServiceInterface, ComputeCellService>();
            serviceCollection.AddSingleton<HttpClient>();
            serviceCollection.AddSingleton<JobsPostRequestHelper>();
            serviceCollection.AddSingleton<HttpHelper>();
        }

        private static void ConfigureDefaultApplicationSettings(IServiceCollection serviceCollection)
        {
            var configuration = InitializeConfigurationSettings();
            var applicationSettings = configuration.GetSection("ApplicationSettings");

            serviceCollection.Configure<ApplicationSettings>(applicationSettings);
        }

        private static IConfigurationRoot InitializeConfigurationSettings()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}
