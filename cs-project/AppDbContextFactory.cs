using cs_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        Console.WriteLine($"[Factory] Environment: {environment}");
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddUserSecrets<Program>()
            .Build();

        var connStr = config.GetConnectionString("DefaultConnection");

        Console.WriteLine($"[Factory] ConnectionString: {connStr}");
        if (string.IsNullOrEmpty(connStr))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is missing or empty.");

        foreach (var provider in config.Providers)
        {
            Console.WriteLine($"Provider: {provider.GetType().Name}");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connStr, b => b.MigrationsAssembly("cs-project.Infrastructure"));

        return new AppDbContext(optionsBuilder.Options);
    }
}
