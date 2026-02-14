using AutoMapper;
using RPD_API.DTO;
using RPD_API.DTO.Move;
using RPD_API.DTO.Types;
using RPD_API.Models;

namespace RPD_API.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Abilities
            CreateMap<Abilities, AbilitiesDTO>();
            CreateMap<PostAbilitiesDTO, Abilities>();
            CreateMap<PutAbilitiesDTO, Abilities>();
            // PokemonAbilities
            CreateMap<PokemonAbilities, PokemonAbilitiesDTO>()
                .ForMember(d => d.abName,o => o.MapFrom(s => s.Abilities.abName));
            CreateMap<PostPokemonAbilitiesDTO, PokemonAbilities>();
            CreateMap<PutPokemonAbilitiesDTO, PokemonAbilities>();

            //EggGroup
            CreateMap<EggGroup, EggGroupDTO>();
            CreateMap<PostEggGroupDTO, EggGroup>();
            CreateMap<PutEggGroupDTO, EggGroup>(); ;
            //PokemonEggGroup
            CreateMap<PokemonEggGroup, PokemonEggGroupDTO>()
                .ForMember(d => d.egName,o => o.MapFrom(s => s.EggGroup.egName));
            CreateMap<PostPokemonEggGroupDTO, PokemonEggGroup>();
            CreateMap<PutPokemonEggGroupDTO, PokemonEggGroup>();

            //GameVersion
            CreateMap<GameVersion, GameVersionDTO>();
            CreateMap<PostGameVersionDTO, GameVersion>();
            CreateMap<PutGameVersionDTO, GameVersion>();
            //PokemonGameVersion
            CreateMap<PokemonGameVersion, PokemonGameVersionDTO>();
            CreateMap<PostPokemonGameVersionDTO, PokemonGameVersion>();
            CreateMap<PutPokemonGameVersionDTO, PokemonGameVersion>();

            //MOVE
            CreateMap<Move, MoveDTO>()
                .ForMember(dest => dest.TypesName, opt => opt.MapFrom(src => src.Types.typesName));
            CreateMap<PostMoveDTO, Move>();
            CreateMap<PutMoveDTO, Move>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            //POKEMON MOVE
            CreateMap<PokemonMove, PokemonMoveDTO>()
                .ForMember(dest => dest.Move,opt => opt.MapFrom(src => src.Move)); ;
            CreateMap<PostPokemonMoveDTO, PokemonMove>();
            CreateMap<PutPokemonMoveDTO, PokemonMove>()
                .ForAllMembers(opt =>opt.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PostPokemonMoveListItem, PokemonMove>()
                .ForMember(dest => dest.Pokemons, opt => opt.Ignore())
                .ForMember(dest => dest.Move, opt => opt.Ignore())
                .ForMember(dest => dest.pokeID, opt => opt.Ignore());

            //StatType
            CreateMap<StatType, StatTypeDTO>();
            CreateMap<PostStatTypeDTO, StatType>();
            CreateMap<PutStatTypeDTO, StatType>()
                .ForAllMembers(opts =>opts.Condition((src, dest, srcMember) => srcMember != null));
            //PokemonStats
            CreateMap<PokemonStats, PokemonStatsDTO>();
            CreateMap<PostPokemonStatsDTO, PokemonStats>()
                .ForMember(dest => dest.pokeID,opt => opt.Ignore());
            CreateMap<PutPokemonStatsDTO, PokemonStats>();

            // Types
            CreateMap<Types, TypesDTO>();
            CreateMap<PostTypesDTO, Types>();
            CreateMap<PutTypesDTO, Types>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            // PokemonType
            CreateMap<PokemonType, PokemonTypeDTO>();
            CreateMap<PostPokemonTypeDTO, PokemonType>();
            CreateMap<PutPokemonTypeDTO, PokemonType>();

            //GrowthRate
            CreateMap<GrowthRate, GrowthRateDTO>();
            CreateMap<PostGrowthRateDTO, GrowthRate>();
            CreateMap<PutGrowthRateDTO, GrowthRate>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) => srcMember != null));

            //EV
            CreateMap<EffortValues, EffortValuesDTO>().ReverseMap();
            CreateMap<PostPokemonsEffortValuesDTO, EffortValues>().ReverseMap();
            CreateMap<PutEffortValuesDTO, EffortValues>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

            //IMG
            CreateMap<ImageLink, ImageLinkDTO>().ReverseMap();
            CreateMap<PostImageLinkDTO, ImageLink>();
            CreateMap<PutImageLinkDTO, ImageLink>()
                .ForAllMembers(opt =>opt.Condition((src, dest, srcMember) => srcMember != null));

            //pokemon
            CreateMap<Pokemons, PokemonsDTO>()
            .ForMember(
                dest => dest.ImageLink,
                opt => opt.MapFrom(src =>
                    src.ImageLink != null
                        ? src.ImageLink.Select(i => i.imgLink)
                        : new List<string>()
                )
            )
            .ForMember(
                dest => dest.TypeName,
                opt => opt.MapFrom(src =>
                    src.PokemonType != null
                        ? src.PokemonType.Select(t => t.Types.typesName)
                        : new List<string>()
                )
            );

            CreateMap<Pokemons, PokemonDetailDTO>()
                .ForMember( dest => dest.grName,opt => opt
                    .MapFrom( src => src.GrowthRate != null ? src.GrowthRate.grName : null))
                .ForMember( dest => dest.EvolutionChart, opt => opt
                    .MapFrom(src => src.EvolutionChart))
                .ForMember(dest => dest.PreEvolutionChart,opt => opt
                    .MapFrom(src => src.PreEvolutionChart));

            CreateMap<PostPokemonDTO, Pokemons>();
            CreateMap<PostFullPokemonsDTO, Pokemons>();
            CreateMap<PostFullPokemonsDTO, PostPokemonDTO>();
            CreateMap<PutFullPokemonsDTO, Pokemons>();
            CreateMap<PutPokemonDTO, Pokemons>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null)
                );
            CreateMap<PutFullPokemonsDTO, PutPokemonDTO>().ReverseMap();

            //Evolution
            CreateMap<EvolutionChart, EvolutionChartDTO>()
                .ForMember(d => d.prePokemonName,
                    o => o.MapFrom(s => s.PrePokemons.pokeName))
                .ForMember(d => d.prePokemonImagelink,
                    o => o.MapFrom(s =>s.PrePokemons.ImageLink
                .Select(i => i.imgLink)
                .FirstOrDefault()))
                .ForMember(d => d.PokemonName,
                    o => o.MapFrom(s => s.Pokemons.pokeName))
                .ForMember(d => d.PokemonImagelink,
                    o => o.MapFrom(s => s.Pokemons.ImageLink
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
