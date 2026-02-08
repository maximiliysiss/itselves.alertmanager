using System.IO;
using Itselves.AlertManager.Abstraction.Extensions;
using Itselves.AlertManager.Abstraction.IntegrationTests.Cases;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Itselves.AlertManager.Abstraction.IntegrationTests.Shared.Fixture;

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
            .AddAlertManager<SeveralManagersTests.FirstAlertManager>()
            .AddAlertManager<SeveralManagersTests.SecondAlertManager>();
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
