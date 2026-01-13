using AutoMapper;
using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GrowthRate, GrowthRateDTO>().ReverseMap();
            CreateMap<PostGrowthRateDTO, GrowthRate>();

            CreateMap<Types, TypesDTO>().ReverseMap();
            CreateMap<PostTypesDTO, Types>();

            CreateMap<StatType, StatTypeDTO>().ReverseMap();
            CreateMap<PostStatTypeDTO, StatType>();

            CreateMap<Abilities, AbilitiesDTO>().ReverseMap();
            CreateMap<PostAbilitiesDTO, Abilities>();

            CreateMap<EggGroup, EggGroupDTO>().ReverseMap();
            CreateMap<PostEggGroupDTO, EggGroup>();

            CreateMap<EffortValues, EffortValuesDTO>().ReverseMap();
            CreateMap<PostEffortValuesDTO, EffortValues>()
                .ForMember(dest => dest.pokeID,
                    opt => opt.Ignore())   // set in service
                .ForMember(dest => dest.Pokemons,
                    opt => opt.Ignore());
            CreateMap<PutEffortValuesDTO, EffortValues>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<GameVersion, GameVersionDTO>().ReverseMap();
            CreateMap<PostGameVersionDTO, GameVersion>();

            CreateMap<ImageLink, ImageLinkDTO>().ReverseMap();
            CreateMap<PostImageLinkDTO, ImageLink>()
                    .ForMember(d => d.Pokemons, o => o.Ignore())
                    .ForMember(d => d.pokeID, o => o.Ignore());

            CreateMap<PutImageLinkDTO, ImageLink>()
                    .ForMember(d => d.Pokemons, o => o.Ignore())
                    .ForMember(d => d.pokeID, o => o.Ignore());


            CreateMap<MoveDTO, Move>();
            CreateMap<Move, MoveDTO>()
                .ForMember(dest => dest.TypesName, opt => opt.MapFrom(src => src.Types.typesName));
            CreateMap<PostMoveDTO, Move>();
            CreateMap<PutMoveDTO, Move>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<PokemonsDTO, Pokemons>();
            CreateMap<Pokemons, PokemonsDTO>();
            CreateMap<Pokemons, PokemonDetailDTO>()
                .ForMember(dest => dest.grName, opt => opt.MapFrom(src => src.GrowthRate.grName))
                .ForMember(dest => dest.EvolutionChart, opt => opt.MapFrom(src => src.PreEvolutionChart))
                .ForMember(dest => dest.PreEvolutionChart, opt => opt.MapFrom(src => src.EvolutionChart));
            CreateMap<PostPokemonDTO, Pokemons>();
            CreateMap<PostFullPokemonsDTO, PostPokemonDTO>();
            CreateMap<PutFullPokemonsDTO, PutPokemonDTO>().ReverseMap();

            CreateMap<PokemonStats, PokemonStatsDTO>().ReverseMap();
            CreateMap<PokemonAbilities, PokemonAbilitiesDTO>().ReverseMap();
            CreateMap<PokemonGameVersion, PokemonGameVersionDTO>().ReverseMap();
            CreateMap<PokemonEggGroup, PokemonEggGroupDTO>().ReverseMap();
            CreateMap<PokemonType, PokemonTypeDTO>().ReverseMap();
            CreateMap<PokemonMove, PokemonMoveDTO>().ReverseMap();

            CreateMap<EvolutionChart, EvolutionChartDTO>()
    .ForMember(d => d.prePokemonName,
        o => o.MapFrom(s => s.PrePokemons.pokeName))
    .ForMember(d => d.prePokemonImagelink,
        o => o.MapFrom(s =>
            s.PrePokemons.ImageLink
                .Select(i => i.imgLink)
                .FirstOrDefault()))
    .ForMember(d => d.PokemonName,
        o => o.MapFrom(s => s.Pokemons.pokeName))
    .ForMember(d => d.PokemonImagelink,
        o => o.MapFrom(s =>
            s.Pokemons.ImageLink
                .Select(i => i.imgLink)
                .FirstOrDefault()));
            CreateMap<PostEvolutionChartDTO, EvolutionChart>()
                .ForMember(d => d.Pokemons, o => o.Ignore())
                .ForMember(d => d.PrePokemons, o => o.Ignore());

            CreateMap<PutEvolutionChartDTO, EvolutionChart>()
                .ForMember(d => d.Pokemons, o => o.Ignore())
                .ForMember(d => d.PrePokemons, o => o.Ignore());
        }
    }
}
