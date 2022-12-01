using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Punchclock;
using Punchclock.Models;
using Punchclock.Services;
using static System.Text.Encoding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<EntryService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddDbContext<PunchclockDbContext>(options =>
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
            IssuerSigningKey = new SymmetricSecurityKey(UTF8.GetBytes(builder.Configuration.GetRequiredSection("Authentication:Secret").Value!)),
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

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

app.UseEndpoint();

app.Run();

namespace Punchclock
{
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class Program
    {
    }
}