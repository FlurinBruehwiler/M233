using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Projektarbeit.Configurations;
using Projektarbeit.Errors;
using Projektarbeit.Extensions;
using Projektarbeit.Models;
using Projektarbeit.Services;
using Microsoft.OpenApi.Models;
using Projektarbeit;
using Projektarbeit.TestData;
using Projektarbeit.Validators;
using static System.Text.Encoding;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<SaveService>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlite("Data Source=db.db");
    options.EnableSensitiveDataLogging();
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new DateOnlyConverter());
});


builder.Services.AddScoped<IValidator<RegisterRequestDto>, RequestUserDtoValidator>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header, 
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey 
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        { 
            new OpenApiSecurityScheme 
            { 
                Reference = new OpenApiReference 
                { 
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" 
                } 
            },
            new string[] { } 
        } 
    });
});
builder.Services.Configure<AuthenticationConfiguration>(builder.Configuration.GetSection("Authentication"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(UTF8.GetBytes(builder.Configuration.GetRequiredSection("Authentication:Secret")
                    .Value!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Headers.Authorization;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<TestDataManager>();
}

var app = builder.Build();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is not BadRequestException exception)
            return;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status400BadRequest;

        await context.Response.WriteAsJsonAsync(exception.Errors);
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseEndpoint();

// SaveService.Validators = typeof(Program)
//     .Assembly
//     .ExportedTypes
//     .Where(x => x.GetInterfaces()
//         .Any(i => i.IsGenericType && 
//                   i.GetGenericTypeDefinition() == typeof(ICustomValidator<>)))
//     .Select(Activator.CreateInstance)
//     .Cast<ICustomValidator<object>>()
//     .GroupBy(x => x.GetType())
//     .ToDictionary(x => x.Key
//             .GetGenericArguments()
//             .First(),
//         x => x.AsEnumerable().ToList());

app.Run();

namespace Projektarbeit
{
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program
    {
    }
}