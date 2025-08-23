using cs_project.Core.Entities;
using cs_project.Core.Entities.Audit;
using cs_project.Core.Entities.Pricing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace cs_project.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        public DbSet<Station> Stations => Set<Station>();
        public DbSet<Tank> Tanks => Set<Tank>();
        public DbSet<Pump> Pumps => Set<Pump>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Shift> Shifts => Set<Shift>();
        public DbSet<ShiftEmployee> ShiftEmployees => Set<ShiftEmployee>();
        public DbSet<CustomerTransaction> CustomerTransactions => Set<CustomerTransaction>();
        public DbSet<CustomerPayment> CustomerPayments => Set<CustomerPayment>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<SupplierInvoice> SupplierInvoices => Set<SupplierInvoice>();
        public DbSet<SupplierInvoiceLine> SupplierInvoiceLines => Set<SupplierInvoiceLine>();
        public DbSet<SupplierPayment> SupplierPayments => Set<SupplierPayment>();
        public DbSet<SupplierPaymentApply> SupplierPaymentApplies => Set<SupplierPaymentApply>();
        public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();
        public DbSet<PricePolicy> PricePolicies => Set<PricePolicy>();
        public DbSet<StationFuelPrice> StationFuelPrices => Set<StationFuelPrice>();
        
        public DbSet<CorrectionLog> CorrectionLogs => Set<CorrectionLog>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public override int SaveChanges()
        {
            StampAuditTimes();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            StampAuditTimes();
            return base.SaveChangesAsync(ct);
        }

        private void StampAuditTimes()
        {
            var now = DateTime.UtcNow;
            foreach (var e in ChangeTracker.Entries<BaseEntity>())
            {
                if (e.State == EntityState.Added)
                {
                    e.Property(x => x.CreatedAtUtc).CurrentValue = now;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            // ---------- BaseEntity ----------
            foreach (var et in model.Model.GetEntityTypes()
                         .Where(t => typeof(BaseEntity).IsAssignableFrom(t.ClrType)))
            {
                // rowversion (SQL Server) for optimistic concurrency
                model.Entity(et.ClrType).Property<byte[]>("RowVersion")
                    .IsRowVersion().IsConcurrencyToken().HasColumnName("RowVersion");

                // indexes oftenly use

                model.Entity(et.ClrType).HasIndex("CreatedAtUtc");
                model.Entity(et.ClrType).HasIndex("IsActive");
            }

            // ---------- Station ----------
            model.Entity<Station>(e =>
            {
                e.Property(p => p.Name).HasMaxLength(150).IsRequired();
                e.Property(p => p.Address).HasMaxLength(250).IsRequired();
            });

            // ---------- Tank ----------
            model.Entity<Tank>(e => {
                e.HasOne(t => t.Station).WithMany(s => s.Tanks).HasForeignKey(t => t.StationId).OnDelete(DeleteBehavior.NoAction);
                e.Property(t => t.FuelType).HasConversion<int>();
                e.Property(t => t.CapacityLiters).HasColumnType("decimal(18,3)");
                e.Property(t => t.CurrentVolumeLiters).HasColumnType("decimal(18,3)");
            });
            // ---------- Pump ----------
            model.Entity<Pump>(e =>
            {
                e.HasOne(p => p.Tank).WithMany(t => t.Pumps).HasForeignKey(p => p.TankId);
                e.Property(p => p.Status).HasConversion<int>();
                e.Property(p => p.CurrentVolume).HasColumnType("decimal(18,3)");
                // A Pump has many CustomerTransactions 
                e.HasMany(p => p.CustomerTransactions)
                 .WithOne(ct => ct.Pump)
                 .HasForeignKey(ct => ct.PumpId);
            });

            // ---------- Employee ----------
            model.Entity<Employee>(e =>
            {
                e.HasOne(emp => emp.Station).WithMany(st => st.Employees).HasForeignKey(emp => emp.StationId);
                e.Property(emp => emp.Role).HasConversion<int>();
            });

            // ---------- Shift / ShiftEmployee (bridge) ----------
            model.Entity<Shift>(e =>
            {
                e.Property(s => s.TotalSalesAmount).HasColumnType("decimal(18,2)");
            });

            model.Entity<ShiftEmployee>(e =>
            {
                e.HasOne(se => se.Shift).WithMany(s => s.ShiftEmployees).HasForeignKey(se => se.ShiftId);
                e.HasOne(se => se.Employee).WithMany(emp => emp.ShiftEmployees).HasForeignKey(se => se.EmployeeId);
                e.HasIndex(se => new { se.ShiftId, se.EmployeeId }).IsUnique(); // prevent duplicates
            });

            // ---------- CustomerTransaction (TEMPORAL) ----------
            model.Entity<CustomerTransaction>(e =>
            {
                e.Property(x => x.PricePerLiter).HasColumnType("decimal(18,4)");         // price per liter
                e.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");        // money
                e.Property(x => x.Liters).HasColumnType("decimal(18,3)");            // quantity
                e.Property(x => x.TimestampUtc).HasColumnName("TimestampUtc");

                e.HasOne(ct => ct.StationFuelPrice)
                    .WithMany()
                    .HasForeignKey(ct => ct.StationFuelPriceId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.Property(x => x.PricePerLiter).HasColumnType("decimal(18,4)");
                e.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
                e.Property(x => x.Liters).HasColumnType("decimal(18,3)");

                e.ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.HasPeriodStart("SysStartTime").HasColumnName("SysStartTime");
                    ttb.HasPeriodEnd("SysEndTime").HasColumnName("SysEndTime");
                    ttb.UseHistoryTable("CustomerTransactionHistory");
                }));
            });

            // ---------- CustomerPayment (TEMPORAL) ----------
            model.Entity<CustomerPayment>(e =>
            {
                e.HasOne(cp => cp.CustomerTransaction)
                 .WithMany(ct => ct.CustomerPayments)
                 .HasForeignKey(cp => cp.CustomerTransactionId);

                e.Property(cp => cp.Method).HasConversion<int>();
                e.Property(cp => cp.Amount).HasColumnType("decimal(18,2)");

                e.ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.HasPeriodStart("SysStartTime").HasColumnName("SysStartTime");
                    ttb.HasPeriodEnd("SysEndTime").HasColumnName("SysEndTime");
                    ttb.UseHistoryTable("CustomerPaymentHistory");
                }));
            });

            // ---------- Supplier ----------
            model.Entity<Supplier>(e =>
            {
                e.Property(s => s.CompanyName).HasMaxLength(200).IsRequired();
            });

            // ---------- SupplierInvoice (TEMPORAL) ----------
            model.Entity<SupplierInvoice>(e =>
            {
                e.HasOne(si => si.Supplier).WithMany(s => s.Invoices) 
                  .HasForeignKey("SupplierId").IsRequired();

                e.HasOne<Station>().WithMany().HasForeignKey(si => si.StationId).IsRequired();

                e.ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.HasPeriodStart("SysStartTime").HasColumnName("SysStartTime");
                    ttb.HasPeriodEnd("SysEndTime").HasColumnName("SysEndTime");
                    ttb.UseHistoryTable("SupplierInvoiceHistory");
                }));
            });

            // ---------- SupplierInvoiceLine (TEMPORAL) ----------
            model.Entity<SupplierInvoiceLine>(e =>
            {
                e.HasOne(l => l.SupplierInvoice).WithMany(si => si.Lines)
                  .HasForeignKey(l => l.SupplierInvoiceId).IsRequired()
                  .OnDelete(DeleteBehavior.NoAction).IsRequired();
                e.HasOne(l => l.Tank).WithMany().HasForeignKey(l => l.TankId)
                  .OnDelete(DeleteBehavior.NoAction).IsRequired();

                e.Property(l => l.FuelType).HasConversion<int>();
                e.Property(l => l.QuantityLiters).HasColumnType("decimal(18,3)");
                e.Property(l => l.UnitPrice).HasColumnType("decimal(18,4)");

                e.ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.HasPeriodStart("SysStartTime").HasColumnName("SysStartTime");
                    ttb.HasPeriodEnd("SysEndTime").HasColumnName("SysEndTime");
                    ttb.UseHistoryTable("SupplierInvoiceLineHistory");
                }));
            });

            // ---------- SupplierPayment (TEMPORAL) ----------
            model.Entity<SupplierPayment>(e =>
            {
                e.HasOne(sp => sp.Supplier).WithMany(s => s.SupplierPayments)
                  .HasForeignKey(sp => sp.SupplierId).IsRequired();

                e.Property(sp => sp.Method).HasConversion<int>();
                e.Property(sp => sp.Amount).HasColumnType("decimal(18,2)");

                e.ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.HasPeriodStart("SysStartTime").HasColumnName("SysStartTime");
                    ttb.HasPeriodEnd("SysEndTime").HasColumnName("SysEndTime");
                    ttb.UseHistoryTable("SupplierPaymentHistory");
                }));
            });

            // ---------- SupplierPaymentApply (TEMPORAL) ----------
            model.Entity<SupplierPaymentApply>(e =>
            {
                e.HasOne(a => a.SupplierPayment).WithMany(p => p.Applies)
                  .HasForeignKey(a => a.SupplierPaymentId).IsRequired()
                  .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(a => a.SupplierInvoice).WithMany(i => i.Applies)
                  .HasForeignKey(a => a.SupplierInvoiceId).IsRequired();
                    
                e.Property(a => a.AppliedAmount).HasColumnType("decimal(18,2)");

                e.ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.HasPeriodStart("SysStartTime").HasColumnName("SysStartTime");
                    ttb.HasPeriodEnd("SysEndTime").HasColumnName("SysEndTime");
                    ttb.UseHistoryTable("SupplierPaymentApplyHistory");
                }));
            });

            // ---------- ExchangeRate ----------
            model.Entity<ExchangeRate>(e =>
            {
                e.Property(x => x.Rate).HasColumnType("decimal(18,6)");
                e.Property(x => x.BaseCurrency).HasMaxLength(3).IsRequired();
                e.Property(x => x.QuoteCurrency).HasMaxLength(3).IsRequired();
                e.HasIndex(x => new { x.BaseCurrency, x.QuoteCurrency, x.RetrievedAtUtc });
            });

            // ---------- PricePolicy ----------
            model.Entity<PricePolicy>(e =>
            {
                e.HasOne(p => p.Station).WithMany().HasForeignKey(p => p.StationId);
                e.Property(p => p.FuelType).HasConversion<int>();
                e.Property(p => p.Method).HasConversion<int>();
                e.Property(p => p.BaseUsdPrice).HasColumnType("decimal(18,4)");
                e.Property(p => p.MarginPct).HasColumnType("decimal(5,4)");
                e.Property(p => p.MarginRon).HasColumnType("decimal(18,3)");
                e.Property(p => p.RoundingIncrement).HasColumnType("decimal(18,3)");
                e.Property(p => p.RoundingMode).HasConversion<int>();
                e.HasIndex(p => new { p.StationId, p.FuelType, p.EffectiveFromUtc, p.EffectiveToUtc, p.IsActive, p.Priority });
            });

            // ---------- StationFuelPrice (TEMPORAL) ----------
            model.Entity<StationFuelPrice>(e =>
            {
                e.HasOne(sfp => sfp.Station).WithMany().HasForeignKey(sfp => sfp.StationId);
                e.HasOne(sfp => sfp.DerivedFromPolicy).WithMany().HasForeignKey(sfp => sfp.DerivedFromPolicyId);
                e.Property(sfp => sfp.FuelType).HasConversion<int>();
                e.Property(sfp => sfp.PriceRon).HasColumnType("decimal(18,3)");
                e.Property(sfp => sfp.FxRateUsed).HasColumnType("decimal(18,6)");
                e.Property(sfp => sfp.CostRonUsed).HasColumnType("decimal(18,4)");

                e.ToTable(tb => tb.IsTemporal(ttb =>
                {
                    ttb.HasPeriodStart("SysStartTime").HasColumnName("SysStartTime");
                    ttb.HasPeriodEnd("SysEndTime").HasColumnName("SysEndTime");
                    ttb.UseHistoryTable("StationFuelPriceHistory");
                }));
            });


            // ---------- CorrectionLog (business log, not temporal) ----------
            model.Entity<CorrectionLog>(e =>
            {
                e.HasOne(c => c.RequestedBy).WithMany().HasForeignKey(c => c.RequestedById)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(c => c.ApprovedBy).WithMany().HasForeignKey(c => c.ApprovedById)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasIndex(c => new { c.TargetTable, c.TargetId });
            });

            // ---------- AuditLog (app-level log) ----------
            model.Entity<AuditLog>(e =>
            {
                e.HasIndex(a => a.ModifiedAt);
                e.HasIndex(a => a.CorrelationId);
            });
        }
    }
}
