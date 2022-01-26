using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseToolkit
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            serviceProvider.GetService<App>().Run();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // add options
            services.AddOptions();
            services.Configure<ApplicationOptions>(Configuration.GetSection("Settings"));

            // add app and services
            services.AddTransient<App>();
            services.AddTransient<ISQLServerToolkit, SQLServerToolkit>();
            services.AddTransient<IMySQLToolkit, MySQLToolkit>();
            services.AddTransient<IPostgreSQLToolkit, PostgreSQLToolkit>();
            services.AddTransient<IMongoDBToolkit, MongoDBToolkit>();
            services.AddTransient<ISQLiteToolkit, SQLiteToolkit>();
        }
    }
}