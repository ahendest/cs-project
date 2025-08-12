using cs_project.Core.Entities;
using cs_project.Core.Entities.Audit;
using cs_project.Core.History;
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
        public DbSet<FuelPrice> FuelPrices => Set<FuelPrice>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Shift> Shifts => Set<Shift>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<FuelDelivery> FuelDeliveries => Set<FuelDelivery>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<CustomerPayment> CustomerPayments => Set<CustomerPayment>();
        public DbSet<SupplierPayment> SupplierPayments => Set<SupplierPayment>();
        
        public DbSet<CorrectionLog> CorrectionLogs => Set<CorrectionLog>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
        
        public DbSet<TransactionHistory> TransactionHistories => Set<TransactionHistory>();
        public DbSet<CustomerPaymentHistory> CustomerPaymentHistories => Set<CustomerPaymentHistory>();
        public DbSet<FuelDeliveryHistory> FuelDeliveryHistories => Set<FuelDeliveryHistory>();
        public DbSet<FuelPriceHistory> FuelPriceHistories => Set<FuelPriceHistory>();
        public DbSet<SupplierPaymentHistory> SupplierPaymentHistories => Set<SupplierPaymentHistory>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Station -> Tanks, Pumps, Shifts, FuelDeliveries
            modelBuilder.Entity<Station>()
                .HasMany(s => s.Tanks)
                .WithOne(t => t.Station)
                .HasForeignKey(t => t.StationId);
            
            modelBuilder.Entity<Station>()
                .HasMany(s => s.Pumps)
                .WithOne(p => p.Station)
                .HasForeignKey(p => p.StationId);

            modelBuilder.Entity<Station>()
                .HasMany(s => s.Shifts)
                .WithOne(sh => sh.Station)
                .HasForeignKey(sh => sh.StationId);

            modelBuilder.Entity<Station>()
                .HasMany(s => s.FuelDeliveries)
                .WithOne(fd => fd.Station)
                .HasForeignKey(fd => fd.StationId);

            // Tank → Pumps, FuelDeliveries
            modelBuilder.Entity<Tank>()
                .HasMany(t => t.Pumps)
                .WithOne(p => p.Tank)
                .HasForeignKey(p => p.TankId);

            modelBuilder.Entity<Tank>()
                .HasMany(t => t.FuelDeliveries)
                .WithOne(fd => fd.Tank)
                .HasForeignKey(fd => fd.TankId);

            // Pump → Transactions
            modelBuilder.Entity<Pump>()
                .HasMany(p => p.Transactions)
                .WithOne(tr => tr.Pump)
                .HasForeignKey(tr => tr.PumpId);


            // Employee → Shifts, Transactions
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Shifts)
                .WithOne(s => s.Employee)
                .HasForeignKey(s => s.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Transactions)
                .WithOne(tr => tr.Employee)
                .HasForeignKey(tr => tr.EmployeeId);

            // Shift → Transactions (if you need, or skip if no direct nav)
            modelBuilder.Entity<Shift>()
                .HasMany<Transaction>()
                .WithOne(tr => tr.Shift)
                .HasForeignKey(tr => tr.ShiftId);

            // Customer → Transactions
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Transactions)
                .WithOne(tr => tr.Customer)
                .HasForeignKey(tr => tr.CustomerId);

            // Supplier → FuelDeliveries
            modelBuilder.Entity<Supplier>()
                .HasMany(su => su.FuelDeliveries)
                .WithOne(fd => fd.Supplier)
                .HasForeignKey(fd => fd.SupplierId);

            // FuelDelivery → SupplierPayment (one-to-one)
            modelBuilder.Entity<FuelDelivery>()
                .HasOne(fd => fd.SupplierPayment)
                .WithOne(sp => sp.FuelDelivery)
                .HasForeignKey<SupplierPayment>(sp => sp.FuelDeliveryId);

            // Transaction → CustomerPayment (one-to-one)
            modelBuilder.Entity<Transaction>()
                .HasOne(tr => tr.CustomerPayment)
                .WithOne(cp => cp.Transaction)
                .HasForeignKey<CustomerPayment>(cp => cp.TransactionId);

            // CorrectionLog: Employee navigation for RequestedBy and ApprovedBy
            modelBuilder.Entity<CorrectionLog>()
                .HasOne(cl => cl.RequestedBy)
                .WithMany()
                .HasForeignKey(cl => cl.RequestedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CorrectionLog>()
                .HasOne(cl => cl.ApprovedBy)
                .WithMany()
                .HasForeignKey(cl => cl.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);


            //

            modelBuilder.Entity<Pump>()
                .Property(p => p.FuelType)
                .HasConversion<string>();

            modelBuilder.Entity<FuelPrice>()
                .Property(p => p.FuelType)
                .HasConversion<string>();

            modelBuilder.Entity<Tank>()
                .Property(t => t.FuelType)
                .HasConversion<string>();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Role)
                .HasConversion<string>();

            modelBuilder.Entity<CustomerPayment>()
                .Property(p => p.Method)
                .HasConversion<string>();

            modelBuilder.Entity<SupplierPayment>()
                .Property(p => p.Method)
                .HasConversion<string>();

            modelBuilder.Entity<CorrectionLog>()
                .Property(c => c.Type)
                .HasConversion<string>();


            var auditableEntities = new[]
            {
                typeof(Station), typeof(Tank), typeof(Pump), typeof(FuelPrice),
                typeof(Employee), typeof(Shift), typeof(Customer), typeof(Supplier),
                typeof(FuelDelivery), typeof(Transaction), typeof(CustomerPayment), typeof(SupplierPayment)
            };

            //foreach (var type in auditableEntities)
            //{
            //    Console.WriteLine($"now we are here : {type.Name} ");
            //    try
            //    {
            //        var entity = modelBuilder.Model.FindEntityType(type);
            //        if (entity != null)
            //        {
            //            modelBuilder.Entity(type)
            //                .Property<byte[]>("RowVersion")
            //                .IsRowVersion()
            //                .IsConcurrencyToken()
            //                .HasColumnType("timestamp")
            //                .ValueGeneratedOnAddOrUpdate();
            //        }
            //        else
            //        {
            //            Console.WriteLine($"Warning: {type.Name} is not a registered entity.");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Error configuring RowVersion for {type.Name}: {ex.Message}");
            //    }
            //}
        }
    }
}
