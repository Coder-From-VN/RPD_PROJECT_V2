using RPD_API.Models;
using Microsoft.EntityFrameworkCore;
using RPD_API.Repo.IRepo;
using RPD_API.Repo;
using System.Text.Json.Serialization;
using RPD_API.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//dependeci injection

//ignorCycles
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//automappper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//repo
builder.Services.AddScoped<IGrowthRateRepo, GrowthRateRepo>();
builder.Services.AddScoped<ITypesRepo, TypesRepo>();
builder.Services.AddScoped<IStatTypeRepo, StatTypeRepo>();
builder.Services.AddScoped<IAbilitiesRepo, AbilitiesRepo>();
builder.Services.AddScoped<IEggGroupRepo, EggGroupRepo>();
builder.Services.AddScoped<IEffortValuesRepo, EffortValuesRepo>();
builder.Services.AddScoped<IGameVersionRepo, GameVersionRepo>();
builder.Services.AddScoped<IImageLinkRepo, ImageLinkRepo>();
builder.Services.AddScoped<IMoveRepo, MoveRepo>();
builder.Services.AddScoped<IPokemonsRepo, PokemonsRepo>();
builder.Services.AddScoped<IPokemonAbilitiesRepo, PokemonAbilitiesRepo>();
builder.Services.AddScoped<IPokemonTypeRepo, PokemonTypeRepo>();
builder.Services.AddScoped<IPokemonStatsRepo, PokemonStatsRepo>();
builder.Services.AddScoped<IPokemonEggGroupRepo, PokemonEggGroupRepo>();
builder.Services.AddScoped<IPokemonGameVersionRepo, PokemonGameVersionRepo>();
builder.Services.AddScoped<IPokemonMoveRepo, PokemonMoveRepo>();
builder.Services.AddScoped<IEvolutionChartRepo, EvolutionChartRepo>();
builder.Services.AddScoped<ITrainnerRepo, TrainnerRepo>();
builder.Services.AddScoped<PokemonService>();
//dependeci injection

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Identity User
builder.Services.AddIdentity<Trainner, IdentityRole>()
    .AddEntityFrameworkStores<rpdDbContext>().AddDefaultTokenProviders();

builder.Services.AddDbContext<rpdDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("RPD_API_DB_CS"));
});

//add authen token
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

//Add lock
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "RPD API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
