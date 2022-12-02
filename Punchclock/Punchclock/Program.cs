using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Punchclock.Configurations;
using Punchclock.Errors;
using Punchclock.Extensions;
using Punchclock.Models.Db;
using Punchclock.Models.Dto;
using Punchclock.Repositories;
using Punchclock.Services;
using Punchclock.Validators;
using static System.Text.Encoding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<EntryService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IValidator<CategoryDto>, CategoryValidators>();
builder.Services.AddScoped<IValidator<EntryDto>, EntryValidators>();
builder.Services.AddScoped<IValidator<TagDto>, TagValidators>();

builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<TagRepository>();
builder.Services.AddScoped<EntryRepository>();

builder.Services.AddDbContext<PunchclockDbContext>(options =>
{
    options.UseSqlite("Data Source=db.db");
    options.EnableSensitiveDataLogging();
});

builder.Services.AddHttpContextAccessor();

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
                context.Token = context.Request.Cookies["dasToken"];
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

namespace Punchclock
{
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Program
    {
    }
}