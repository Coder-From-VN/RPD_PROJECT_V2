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

            CreateMap<PokemonsDTO, Pokemons>();
            CreateMap<Pokemons, PokemonsDTO>()
                .ForMember(dest => dest.grName, opt => opt.MapFrom(src => src.GrowthRate.grName))
                .ForMember(dest => dest.EvolutionChart, opt => opt.MapFrom(src => src.EvolutionChart))
                .ForMember(dest => dest.PreEvolutionChart, opt => opt.MapFrom(src => src.PreEvolutionChart));
            CreateMap<PostPokemonDTO, Pokemons>();
            CreateMap<PostFullPokemonsDTO, PostPokemonDTO>();
            CreateMap<PutFullPokemonsDTO, PutPokemonDTO>().ReverseMap();

            CreateMap<ImageLink, ImageLinkDTO>().ReverseMap();
            CreateMap<EffortValues, EffortValuesDTO>().ReverseMap();
            CreateMap<PokemonStats, PokemonStatsDTO>().ReverseMap();
            CreateMap<PokemonAbilities, PokemonAbilitiesDTO>().ReverseMap();
            CreateMap<PokemonGameVersion, PokemonGameVersionDTO>().ReverseMap();
            CreateMap<PokemonEggGroup, PokemonEggGroupDTO>().ReverseMap();
            CreateMap<PokemonType, PokemonTypeDTO>().ReverseMap();
            CreateMap<PokemonMove, PokemonMoveDTO>().ReverseMap();

            CreateMap<EvolutionChart, EvolutionChartDTO>()
    .ForMember(dest => dest.prePokemonName, opt => opt.MapFrom(src => src.PrePokemons.pokeName))
    .ForMember(dest => dest.prePokemonImagelink, opt => opt.MapFrom(src => src.PrePokemons.ImageLink.FirstOrDefault().imgLink))
    .ForMember(dest => dest.PokemonName, opt => opt.MapFrom(src => src.Pokemons.pokeName))
    .ForMember(dest => dest.PokemonImagelink, opt => opt.MapFrom(src => src.Pokemons.ImageLink.FirstOrDefault().imgLink));
            CreateMap<PostEvolutionChartDTO, EvolutionChart>();
            CreateMap<PutEvolutionChartDTO, EvolutionChart>();


        }
    }
}
