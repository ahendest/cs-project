using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace cs_project.Tests.HealthChecks;

public class HealthCheckTests
{
    [Fact]
    public async Task DatabaseHealthCheck_ReturnsHealthy()
    {
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>(opts => opts.UseInMemoryDatabase("hc"));
        services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
        services.AddLogging();

        await using var provider = services.BuildServiceProvider();
        var hc = provider.GetRequiredService<HealthCheckService>();

        var result = await hc.CheckHealthAsync();

        Assert.Equal(HealthStatus.Healthy, result.Status);
    }
}
