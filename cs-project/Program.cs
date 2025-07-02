using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using cs_project.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();


builder.Services.AddValidatorsFromAssemblyContaining<PumpCreateDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TransactionCreateDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<FuelPriceCreateDTOValidator>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IPumpRepository, PumpRepository>();
builder.Services.AddScoped<IPumpService, PumpService>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IFuelPriceRepository, FuelPriceRepository>();
builder.Services.AddScoped<IFuelPriceService, FuelPriceService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0)) 
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<cs_project.Middleware.ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
