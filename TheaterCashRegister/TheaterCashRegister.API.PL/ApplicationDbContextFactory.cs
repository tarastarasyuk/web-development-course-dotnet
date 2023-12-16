using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TheaterCashRegister.DAL.Data;

namespace TheaterCashRegister.SSR.PL;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var options = GetConfiguredOptions();
        return new ApplicationDbContext(options);
    }

    public DbContextOptions<ApplicationDbContext> GetConfiguredOptions()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        var connectionString = configuration
            .GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlite(connectionString);

        return optionsBuilder.Options;
    }
}