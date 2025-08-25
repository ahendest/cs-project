using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace cs_project.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<ICorrectionLogRepository, CorrectionLogRepository>();
        services.AddScoped<IPumpRepository, PumpRepository>();
        services.AddScoped<ITankRepository, TankRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPricePolicyRepository, PricePolicyRepository>();
        services.AddScoped<IShiftRepository, ShiftRepository>();
        services.AddScoped<IShiftEmployeeRepository, ShiftEmployeeRepository>();
        services.AddScoped<ISupplierRepository, SupplierRepository>();
        services.AddScoped<ISupplierInvoiceRepository, SupplierInvoiceRepository>();
        services.AddScoped<ISupplierInvoiceLineRepository, SupplierInvoiceLineRepository>();
        services.AddScoped<ISupplierPaymentRepository, SupplierPaymentRepository>();
        services.AddScoped<ISupplierPaymentApplyRepository, SupplierPaymentApplyRepository>();
        services.AddScoped<IStationFuelPriceRepository, StationFuelPriceRepository>();
        services.AddScoped<IStationRepository, StationRepository>();
        services.AddScoped<ICustomerTransactionRepository, CustomerTransactionRepository>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddScoped<ISupplierCostRepository, SupplierCostRepository>();
        return services;
    }

    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<ICorrectionLogService, CorrectionLogService>();
        services.AddScoped<IPumpService, PumpService>();
        services.AddScoped<ITankService, TankService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IPricePolicyService, PricePolicyService>();
        services.AddScoped<IPricingService, PricingService>();
        services.AddScoped<IShiftService, ShiftService>();
        services.AddScoped<IShiftEmployeeService, ShiftEmployeeService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ISupplierInvoiceService, SupplierInvoiceService>();
        services.AddScoped<ISupplierInvoiceLineService, SupplierInvoiceLineService>();
        services.AddScoped<ISupplierPaymentService, SupplierPaymentService>();
        services.AddScoped<ISupplierPaymentApplyService, SupplierPaymentApplyService>();
        services.AddScoped<IStationFuelPriceService, StationFuelPriceService>();
        services.AddScoped<ISalesService, SalesService>();
        services.AddScoped<IStationService, StationService>();
        services.AddScoped<ICustomerTransactionService, CustomerTransactionService>();
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
        return services;
    }
}

