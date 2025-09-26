using AutoMapper;
using RPD_API.DTO;
using RPD_API.DTO.Abilities;
using RPD_API.DTO.EffortValues;
using RPD_API.DTO.EggGroup;
using RPD_API.DTO.GameVersion;
using RPD_API.DTO.GrowthRate;
using RPD_API.DTO.Move;
using RPD_API.DTO.StatType;
using RPD_API.DTO.Types;
using RPD_API.Models;

namespace RPD_API.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GrowthRate, GrowthRateDTO>();
            CreateMap<GrowthRateDTO, GrowthRate>();
            CreateMap<PostGrowthRateDTO, GrowthRate>();

            CreateMap<Types, TypesDTO>();
            CreateMap<TypesDTO, Types>();
            CreateMap<PostTypesDTO, Types>();

            CreateMap<StatType, StatTypeDTO>();
            CreateMap<StatTypeDTO, StatType>();
            CreateMap<PostStatTypeDTO, StatType>();

            CreateMap<Abilities, AbilitiesDTO>();
            CreateMap<AbilitiesDTO, Abilities>();
            CreateMap<PostAbilitiesDTO, Abilities>();

            CreateMap<EggGroup, EggGroupDTO>();
            CreateMap<EggGroupDTO, EggGroup>();
            CreateMap<PostEggGroupDTO, EggGroup>();

            CreateMap<EffortValues, EffortValuesDTO>();
            CreateMap<EffortValuesDTO, EffortValues>();
            CreateMap<PostEffortValuesDTO, EffortValues>();

            CreateMap<GameVersion, GameVersionDTO>();
            CreateMap<GameVersionDTO, GameVersion>();
            CreateMap<PostGameVersionDTO, GameVersion>();

            CreateMap<ImageLink, ImageLinkDTO>();
            CreateMap<ImageLinkDTO, ImageLink>();

            CreateMap<MoveDTO, Move>();
            CreateMap<Move, MoveDTO>()
                .ForMember(dest => dest.TypesName, opt => opt.MapFrom(src => src.Types.typesName));
            CreateMap<PostMoveDTO, Move>();
        }
    }
}
