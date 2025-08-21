using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;
using cs_project.Core.Entities.Audit;
using cs_project.Core.Entities.Pricing;

namespace cs_project.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pump, PumpDTO>().ReverseMap();
            CreateMap<Pump, PumpCreateDTO>().ReverseMap();

            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Employee, EmployeeCreateDTO>().ReverseMap();

            CreateMap<Supplier, SupplierDTO>().ReverseMap();
            CreateMap<Supplier, SupplierCreateDTO>().ReverseMap();

            CreateMap<Shift, ShiftDTO>().ReverseMap();
            CreateMap<Shift, ShiftCreateDTO>().ReverseMap();

            CreateMap<AuditLog, AuditLogDTO>().ReverseMap();
            CreateMap<AuditLog, AuditLogCreateDTO>().ReverseMap();

            CreateMap<CorrectionLog, CorrectionLogDTO>().ReverseMap();
            CreateMap<CorrectionLog, CorrectionLogCreateDTO>().ReverseMap();

            CreateMap<CustomerTransaction, CustomerTransactionDTO>().ReverseMap();
            CreateMap<CustomerTransaction, CustomerTransactionCreateDTO>().ReverseMap();

            CreateMap<CustomerPayment, CustomerPaymentDTO>().ReverseMap();
            CreateMap<CustomerPayment, CustomerPaymentCreateDTO>().ReverseMap();

            CreateMap<Station, StationDTO>().ReverseMap();
            CreateMap<Station, StationCreateDTO>().ReverseMap();

            CreateMap<Tank, TankDTO>().ReverseMap();
            CreateMap<Tank, TankCreateDTO>().ReverseMap();

            CreateMap<ShiftEmployee, ShiftEmployeeDTO>().ReverseMap();
            CreateMap<ShiftEmployee, ShiftEmployeeCreateDTO>().ReverseMap();

            CreateMap<SupplierInvoice, SupplierInvoiceDTO>().ReverseMap();
            CreateMap<SupplierInvoice, SupplierInvoiceCreateDTO>().ReverseMap();

            CreateMap<SupplierInvoiceLine, SupplierInvoiceLineDTO>().ReverseMap();
            CreateMap<SupplierInvoiceLine, SupplierInvoiceLineCreateDTO>().ReverseMap();

            CreateMap<SupplierPayment, SupplierPaymentDTO>().ReverseMap();
            CreateMap<SupplierPayment, SupplierPaymentCreateDTO>().ReverseMap();

            CreateMap<SupplierPaymentApply, SupplierPaymentApplyDTO>().ReverseMap();
            CreateMap<SupplierPaymentApply, SupplierPaymentApplyCreateDTO>().ReverseMap();

            CreateMap<ExchangeRate, ExchangeRateDTO>().ReverseMap();
            CreateMap<ExchangeRate, ExchangeRateCreateDTO>().ReverseMap();

            CreateMap<PricePolicy, PricePolicyDTO>().ReverseMap();
            CreateMap<PricePolicy, PricePolicyCreateDTO>().ReverseMap();

            CreateMap<StationFuelPrice, StationFuelPriceDTO>().ReverseMap();
            CreateMap<StationFuelPrice, StationFuelPriceCreateDTO>().ReverseMap();
        }
    }
}
