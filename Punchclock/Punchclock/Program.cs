using Microsoft.EntityFrameworkCore;
using Punchclock;
using Punchclock.Models;
using Punchclock.Services;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpoint();

app.Run();

namespace Punchclock
{
    public partial class Program
    {
    }
}