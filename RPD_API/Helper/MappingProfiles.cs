using AutoMapper;
using RPD_API.DTO;
using RPD_API.DTO.Move;
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

            CreateMap<StatType, StatTypeDTO>();
            CreateMap<StatTypeDTO, StatType>();

            CreateMap<Abilities, AbilitiesDTO>();
            CreateMap<AbilitiesDTO, Abilities>();

            CreateMap<EggGroup, EggGroupDTO>();
            CreateMap<EggGroupDTO, EggGroup>();

            CreateMap<EffortValues, EffortValuesDTO>();
            CreateMap<EffortValuesDTO, EffortValues>();

            CreateMap<GameVersion, GameVersionDTO>();
            CreateMap<GameVersionDTO, GameVersion>();

            CreateMap<ImageLink, ImageLinkDTO>();
            CreateMap<ImageLinkDTO, ImageLink>();

            CreateMap<MoveDTO, Move>();
            CreateMap<Move, MoveDTO>()
            .ForMember(dest => dest.TypeName,
                       opt => opt.MapFrom(src => src.Type.typeName));

        }
    }
}
