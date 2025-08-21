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

            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Employee, EmployeeCreateDTO>().ReverseMap();

            CreateMap<Supplier, SupplierDTO>().ReverseMap();
            CreateMap<Supplier, SupplierCreateDTO>().ReverseMap();

            CreateMap<Shift, ShiftDTO>().ReverseMap();
            CreateMap<Shift, ShiftCreateDTO>().ReverseMap();
        }
    }
}
