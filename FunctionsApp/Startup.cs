using FunctionsApp.Repository;
using FunctionsApp.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;


[assembly: FunctionsStartup(typeof(FunctionsApp.Startup))]

namespace FunctionsApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var mongoClient = new MongoClient(config.GetValue<string>("MongoDBConnectionString"));
            builder.Services.AddSingleton<IMongoDatabase>(_ => mongoClient.GetDatabase(config.GetValue<string>("DBName")));

            builder.Services.AddTransient<IRocketMessageService, RocketMessageService>();
            builder.Services.AddTransient<IUpdateRocketStateService, UpdateRocketStateService>();
            builder.Services.AddTransient<IGetRocketStateService, GetRocketStateService>();
            builder.Services.AddTransient<IGetRocketListService, GetRocketListService>();

            builder.Services.AddTransient<IRocketStateRepository, RocketStateRepository>();
            builder.Services.AddTransient<IRocketMessageRepository, RocketMessageRepository>(); 
        }
    }
}
