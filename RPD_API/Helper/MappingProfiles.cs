using AutoMapper;
using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GrowthRate, GrowthRateDTO>();
            CreateMap<GrowthRateDTO, GrowthRate>();

            CreateMap<Models.Type, TypeDTO>();
            CreateMap<TypeDTO, Models.Type>();

        }
    }
}
