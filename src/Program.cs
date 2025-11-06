using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TipoCambio.Data;
using TipoCambio.Services;

var host = new HostBuilder()

    .ConfigureAppConfiguration(configBuilder =>
    {
        configBuilder.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
        configBuilder.AddEnvironmentVariables();
    })
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(
    services => {

        services.AddDbContext<TipoCambioDbContext>(options =>
        options.UseInMemoryDatabase("CasaCambioDB"));
        services.AddSingleton<JwtService>();

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

TipoCambioSeeder.Seed(host.Services);

host.Run();