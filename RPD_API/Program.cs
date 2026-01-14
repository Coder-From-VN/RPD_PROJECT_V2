using RPD_API.Models;
using Microsoft.EntityFrameworkCore;
using RPD_API.UnitOfWork;
using RPD_API.Extensions;
using RPD_API.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//Add autoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddScoped<IUnitOfWorkRepo, UnitOfWorkRepo>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<rpdDbContext>(op =>
{
    op.UseSqlServer(
        builder.Configuration.GetConnectionString("RPD_API_DB_CS"),
        sql => sql.EnableRetryOnFailure()
    );
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:7185")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
