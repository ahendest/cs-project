using Microsoft.EntityFrameworkCore;
using cs_project.Core.Entities;

namespace cs_project.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pump> Pumps => Set<Pump>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<FuelPrice> FuelPrices => Set<FuelPrice>();
    }
}
