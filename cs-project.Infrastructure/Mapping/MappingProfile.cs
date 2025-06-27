using AutoMapper;
using cs_project.Core.DTOs;
using cs_project.Core.Entities;

namespace cs_project.Infrastructure.Mapping

{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pump, PumpDTO>().ReverseMap();
            CreateMap<Pump, PumpCreateDTO>().ReverseMap();

            CreateMap<Transaction, TransactionsDTO>().ReverseMap();
            CreateMap<Transaction, TransactionsCreateDTO>().ReverseMap();

            CreateMap<FuelPrice, FuelPriceDTO>().ReverseMap();
            CreateMap<FuelPrice, FuelPriceCreateDTO>().ReverseMap();

        }
    }
}
