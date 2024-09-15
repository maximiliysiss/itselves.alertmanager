using System.IO;
using Itselves.AlertManager.Prometheus.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus.Client.DependencyInjection;

namespace Itselves.AlertManager.Prometheus.IntegrationTests.Shared.Fixture;

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
            .AddMetricFactory();

        services
            .AddPrometheusAlertManager();
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
