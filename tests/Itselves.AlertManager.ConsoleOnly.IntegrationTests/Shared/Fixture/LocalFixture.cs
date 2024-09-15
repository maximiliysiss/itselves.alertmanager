using System.IO;
using Itselves.AlertManager.ConsoleOnly.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Itselves.AlertManager.ConsoleOnly.IntegrationTests.Shared.Fixture;

public sealed class LocalFixture : WebApplicationFactory<LocalFixture.Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder.UseContentRoot(Directory.GetCurrentDirectory());

    protected override IHostBuilder CreateHostBuilder() => Host.CreateDefaultBuilder()
        .ConfigureServices(ConfigureServices)
        .ConfigureWebHostDefaults(a => a.UseStartup<Startup>());

    private void ConfigureServices(IServiceCollection services)
    {
        services
            .AddConsoleOnlyAlertManager();
    }

    public sealed class Startup
    {
        public void ConfigureServices()
        {
        }

        public void Configure()
        {
        }
    }
}
