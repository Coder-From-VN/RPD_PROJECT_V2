using Microsoft.EntityFrameworkCore;

namespace RPD_API.Models
{
    public class rpdDbContext : DbContext
    {
        public rpdDbContext(
            DbContextOptions<rpdDbContext> options) : base(options)
        {

        }
        //khai baos dbset
        public DbSet<Abilities> Abilities { get; set; }
        public DbSet<EffortValues> EffortValues { get; set; }
        public DbSet<EggGroup> EggGroup { get; set; }
        public DbSet<EvolutionChart> EvolutionChart { get; set; }
        public DbSet<GameVersion> GameVersion { get; set; }
        public DbSet<GrowthRate> GrowthRate { get; set; }
        public DbSet<ImageLink> ImageLink { get; set; }
        public DbSet<Move> Move { get; set; }
        public DbSet<PokemonAbilities> PokemonAbilities { get; set; }
        public DbSet<PokemonEggGroup> PokemonEggGroup { get; set; }
        public DbSet<PokemonGameVersion> PokemonGameVersion { get; set; }
        public DbSet<PokemonMove> PokemonMove { get; set; }
        public DbSet<Pokemons> Pokemons { get; set; }
        public DbSet<PokemonStats> PokemonStats { get; set; }
        public DbSet<PokemonType> PokemonType { get; set; }
        public DbSet<StatType> StatType { get; set; }
        public DbSet<Type> Type { get; set; }

        //khai bao lien ket data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Type 1 to Many Move
            modelBuilder.Entity<Move>()
                .HasOne(m => m.Type)
                .WithMany(t => t.Move)
                .HasForeignKey(m => m.typeID);
            //GrowthRate 1 to Many Pokemons
            modelBuilder.Entity<Pokemons>()
                .HasOne(p => p.GrowthRate)
                .WithMany(gr => gr.Pokemons)
                .HasForeignKey(gr => gr.growthRateID);
            //ImageLink Many to 1 Pokemon
            modelBuilder.Entity<ImageLink>()
                .HasOne(img => img.Pokemons)
                .WithMany(p => p.ImageLink)
                .HasForeignKey(p => p.pokeID);
            //Pokemons 1 to Many EffortValues
            modelBuilder.Entity<EffortValues>()
                .HasOne(ev => ev.Pokemons)
                .WithMany(p => p.EffortValues)
                .HasForeignKey(p => p.pokeID);
            //One Pokémon can evolve into many others
            modelBuilder.Entity<EvolutionChart>()
                .HasKey(ec => new { ec.pokeID, ec.prePokeID });

            modelBuilder.Entity<EvolutionChart>()
                .HasOne(ec => ec.Pokemons)
                .WithMany(p => p.EvolutionChart)
                .HasForeignKey(ec => ec.pokeID)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<EvolutionChart>()
                .HasOne(ec => ec.PrePokemons)
                .WithMany(p => p.PreEvolutionChart)
                .HasForeignKey(ec => ec.prePokeID)
                .OnDelete(DeleteBehavior.NoAction);
            //Pokemon many to many Abilities join table PokemonAbilities
            // set FK
            modelBuilder.Entity<PokemonAbilities>()
                .HasKey(pa => new { pa.pokeID, pa.abID });
            //Pokemon many to many PokemonAbilities
            modelBuilder.Entity<PokemonAbilities>()
                .HasOne(pa => pa.Pokemons)
                .WithMany(p => p.PokemonAbilities)
                .HasForeignKey(pa => pa.pokeID);
            //Abilities many to many PokemonAbilities
            modelBuilder.Entity<PokemonAbilities>()
                .HasOne(pa => pa.Abilities)
                .WithMany(a => a.PokemonAbilities)
                .HasForeignKey(pa => pa.abID);
            //Pokemon many to many EggGroup
            modelBuilder.Entity<PokemonEggGroup>()
                .HasKey(pe => new { pe.pokeID, pe.egID });
            modelBuilder.Entity<PokemonEggGroup>()
                .HasOne(pe => pe.Pokemons)
                .WithMany(p => p.PokemonEggGroup)
                .HasForeignKey(p => p.pokeID);
            modelBuilder.Entity<PokemonEggGroup>()
                .HasOne(pe => pe.EggGroup)
                .WithMany(eg => eg.PokemonEggGroup)
                .HasForeignKey(pe => pe.egID);
            //Pokemon many to many Stats
            modelBuilder.Entity<PokemonStats>()
                .HasKey(ps => new { ps.pokeID, ps.stID });
            modelBuilder.Entity<PokemonStats>()
                .HasOne(pe => pe.Pokemons)
                .WithMany(p => p.PokemonStats)
                .HasForeignKey(ps => ps.pokeID);
            modelBuilder.Entity<PokemonStats>()
                .HasOne(pe => pe.StatType)
                .WithMany(st => st.PokemonStats)
                .HasForeignKey(ps => ps.stID);
            //Pokemon many to many Gameversion
            modelBuilder.Entity<PokemonGameVersion>()
                .HasKey(pgv => new { pgv.pokeID, pgv.gvID });
            modelBuilder.Entity<PokemonGameVersion>()
                .HasOne(pgv => pgv.Pokemons)
                .WithMany(p => p.PokemonGameVersion)
                .HasForeignKey(pgv => pgv.pokeID);
            modelBuilder.Entity<PokemonGameVersion>()
                .HasOne(pgv => pgv.GameVersion)
                .WithMany(gv => gv.PokemonGameVersion)
                .HasForeignKey(pgv => pgv.gvID);
            //Pokemon many to many Type
            modelBuilder.Entity<PokemonType>()
                .HasKey(pt => new { pt.pokeID, pt.typeID });
            modelBuilder.Entity<PokemonType>()
                .HasOne(pt => pt.Pokemons)
                .WithMany(p => p.PokemonType)
                .HasForeignKey(pt => pt.pokeID);
            modelBuilder.Entity<PokemonType>()
                .HasOne(pt => pt.Type)
                .WithMany(t => t.PokemonType)
                .HasForeignKey(pt => pt.typeID);
            //Pokemon many to many Move
            modelBuilder.Entity<PokemonMove>()
                .HasKey(pm => new { pm.pokeID, pm.moveID });
            modelBuilder.Entity<PokemonMove>()
                .HasOne(pm => pm.Pokemons)
                .WithMany(p => p.PokemonMove)
                .HasForeignKey(pm => pm.pokeID);
            modelBuilder.Entity<PokemonMove>()
                .HasOne(pm => pm.Move)
                .WithMany(m => m.PokemonMove)
                .HasForeignKey(pm => pm.moveID);

        }

    }
}
