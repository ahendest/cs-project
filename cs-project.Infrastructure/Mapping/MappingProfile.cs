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
        }
    }
}
