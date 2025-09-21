using RPD_API.Models;
using Microsoft.EntityFrameworkCore;
using RPD_API.Repo.IRepo;
using RPD_API.Repo;
using System.Text.Json.Serialization;

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
builder.Services.AddScoped<ITypeRepo, TypeRepo>();
builder.Services.AddScoped<IStatTypeRepo, StatTypeRepo>();
builder.Services.AddScoped<IAbilitiesRepo, AbilitiesRepo>();
builder.Services.AddScoped<IEggGroupRepo, EggGroupRepo>();
//dependeci injection

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<rpdDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("RPD_API_DB_CS"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
