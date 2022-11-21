using CalendarSynchronizerWeb.Managers;
using CalendarSynchronizerWeb.Managers.Interfaces;
using Microsoft.Extensions.Options;

namespace CalendarSynchronizerWeb.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureWritable<T>(
                 this IServiceCollection services,
                 IConfigurationSection section,
                 string file = "appsettings.json") where T : class, new()
        {
            services.Configure<T>(section);
            services.AddTransient<IConfigurationManager<T>>(provider =>
            {
                var configuration = (IConfigurationRoot)provider.GetService<IConfiguration>();
                var environment = provider.GetService<IWebHostEnvironment>();
                var options = provider.GetService<IOptionsMonitor<T>>();
                return new ConfigurationManager<T>(environment, options, configuration, section.Key, file);
            });
        }
    }
}
