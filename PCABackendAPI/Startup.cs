using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PCABackendBL.BLServices;
using PCABackendBL.BLServices.Interfaces;
using PCABackendDA;
using PCABackendDA.DataRepository;
using PCABackendDA.DataRepository.Interfaces;
using System.IO;


[assembly: FunctionsStartup(typeof(PCABackendAPI.Startup))]
namespace PCABackendAPI
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddHttpClient();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddScoped<IDataConnection>((s) =>
            {
                return new DataConnection(config);
            });

            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
            builder.Services.AddScoped<IPowerConsumptionRepository, PowerConsumptionRepository>();
            builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<IPowerConsumptionService, PowerConsumptionService>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();


            builder.Services.AddVersionedApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });
            builder.Services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

        }
       
    }
}
