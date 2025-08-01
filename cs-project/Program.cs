using Azure.Identity;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure.Auth;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Data.Auditing;
using cs_project.Infrastructure.Mapping;
using cs_project.Infrastructure.Repositories;
using cs_project.Infrastructure.Services;
using cs_project.Options;
using cs_project.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

if (!builder.Environment.IsDevelopment())
{
    var keyVaultName = builder.Configuration["KeyVaultName"];
    if (!string.IsNullOrEmpty(keyVaultName))
    {
        var vaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
        builder.Configuration
            .AddAzureKeyVault(vaultUri, new DefaultAzureCredential());
    }
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0))
    ));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 

})
.AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true;
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = ctx =>
        {
            Console.WriteLine("JWT failure: " + ctx.Exception.ToString());
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});


builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});
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

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
builder.Services.AddSingleton<SaveChangesInterceptor, AuditInterceptor>();

builder.Services.AddHostedService<AuditWriterService>();
builder.Services.AddSingleton<DbConnectionInterceptor, MySqlAuditConnectionInterceptor>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "cs-project API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");
}

builder.Services.AddHealthChecks()
    .AddMySql(
        connectionString,
        name: "mysql",
        timeout: TimeSpan.FromSeconds(5));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); 

app.UseHttpsRedirection();

app.UseRouting();

app.UseMiddleware<cs_project.Middleware.ErrorHandlingMiddleware>();

app.UseCors("FrontendPolicy");

app.UseAuthentication();  

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");  
Console.WriteLine("JWT VALIDATION SETTINGS:");
Console.WriteLine("Issuer: " + builder.Configuration["Jwt:Issuer"]);
Console.WriteLine("Audience: " + builder.Configuration["Jwt:Audience"]);
Console.WriteLine("Key length: " + builder.Configuration["Jwt:Key"]?.Length);

app.Run();
