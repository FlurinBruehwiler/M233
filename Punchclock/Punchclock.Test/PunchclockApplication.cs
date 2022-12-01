using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Punchclock.Models;

namespace Punchclock.Test;

public class PunchclockApplication : WebApplicationFactory<Program>
{
    private readonly string _environment;
    private readonly string _name;

    public PunchclockApplication(string environment = "Development")
    {
        _environment = environment;
        var rand = new Random();
        _name = rand.Next().ToString();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
            services.AddScoped(sp 
                => new DbContextOptionsBuilder<PunchclockDbContext>()
                .UseInMemoryDatabase(_name)
                .UseApplicationServiceProvider(sp)
                .Options);
        });

        return base.CreateHost(builder);
    }
}