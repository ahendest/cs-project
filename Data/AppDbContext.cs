using cs_project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace cs_project.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pump> Pumps => Set<Pump>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<FuelPrice> FuelPrices => Set<FuelPrice>();
    }
}
