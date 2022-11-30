using Microsoft.EntityFrameworkCore;
using Punchclock;
using Punchclock.Models;
using Punchclock.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<EntryValidator>();
builder.Services.AddScoped<EntryService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddDbContext<PunchclockDbContext>(options =>
{
    options.UseSqlite("db.sql");
    options.EnableSensitiveDataLogging();
});

var app = builder.Build();

app.UseEndpoint();

app.Run();

public partial class Program
{
}