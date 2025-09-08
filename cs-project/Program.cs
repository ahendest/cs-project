using Azure.Identity;
using cs_project.Core.Interfaces;
using cs_project.Infrastructure;
using cs_project.Infrastructure.Auth;
using cs_project.Infrastructure.Data;
using cs_project.Infrastructure.Data.Auditing;
using cs_project.Infrastructure.Mapping;
using cs_project.Options;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connection string 'DefaultConnection' is not configured.");
}

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

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
builder.Services.AddScoped<AuditInterceptor>();


builder.Services.AddDbContext<AppDbContext>((serviceProvider, opts) =>
{
 
    opts.UseSqlServer(connectionString, sql =>
    {
        sql.EnableRetryOnFailure();
        sql.MigrationsAssembly("cs_project.Infrastructure"); 
    });
    opts.AddInterceptors(serviceProvider.GetRequiredService<AuditInterceptor>());
});


builder.Services.AddHostedService<AuditWriterService>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer();

builder.Services.AddOptions<JwtOptions>()
    .BindConfiguration("Jwt")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<IOptions<JwtOptions>, IHostEnvironment, ILogger<Program>>((options, jwtOptions, env, logger) =>
    {
        options.IncludeErrorDetails = env.IsDevelopment();
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                if (env.IsDevelopment())
                {
                    logger.LogError(ctx.Exception, "JWT authentication failure");
                }
                else
                {
                    logger.LogError("JWT authentication failure");
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Value.Issuer,
            ValidAudience = jwtOptions.Value.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key))
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ViewAuditLogs", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ViewCorrectionLogs", policy => policy.RequireRole("Admin"));
});

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

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});
builder.Services.AddValidatorsFromAssemblyContaining<AuditLogCreateDTOValidator>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddRepositories();
builder.Services.AddBusinessServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();

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


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); 

if (Environment.GetEnvironmentVariable("DISABLE_HTTPS_REDIRECT") != "1")
{
    app.UseHttpsRedirection();
}

app.UseRouting();

app.UseMiddleware<cs_project.Middleware.ErrorHandlingMiddleware>();

app.UseCors("FrontendPolicy");

app.UseAuthentication();

app.UseAuthorization();

var healthCheckOptions = new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        if (report.Status != HealthStatus.Healthy)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError("Health check failure: {Status}", report.Status);
        }

        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                exception = e.Value.Exception?.Message
            })
        });
        await context.Response.WriteAsync(result);
    }
};

app.MapHealthChecks("/health", healthCheckOptions);

app.MapControllers();


app.Run();
