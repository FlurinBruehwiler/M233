using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Projektarbeit.Configurations;
using Projektarbeit.Endpoints.AuthenticationEndpoints;
using Projektarbeit.Errors;
using Projektarbeit.Extensions;
using Projektarbeit.Models;
using Projektarbeit.Models.Dto;
using Projektarbeit.Services;
using Projektarbeit.Validators;
using static System.Text.Encoding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddScoped<IValidator<BookingDto>, BookingDtoValidator>();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlite("Data Source=db.db");
    options.EnableSensitiveDataLogging();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

        await context.Response.WriteAsJsonAsync(exception.Error);
    });
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

app.UseEndpoint();

app.Run();

namespace Projektarbeit
{
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program
    {
    }
}