using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projektarbeit.Models;
using Projektarbeit.TestData;

namespace Projektarbeit.Test;

public class ProjektarbeitApplication : WebApplicationFactory<Program>
{
    private readonly string _environment;
    private readonly string _name;
    private TestDataManager _testDataManager = null!;

    public ProjektarbeitApplication(string environment = "Production")
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
                => new DbContextOptionsBuilder<DatabaseContext>()
                    .UseInMemoryDatabase(_name)
                    .UseApplicationServiceProvider(sp)
                    .Options);

            services.AddHostedService(x => new TestDataManager(x)
            {
                ShouldWork = false
            });
            
            _testDataManager = new TestDataManager(services.BuildServiceProvider());
            _testDataManager.StartAsync(new CancellationToken()).GetAwaiter().GetResult();
        });
        
        return base.CreateHost(builder);
    }

    public new void Dispose()
    {
        _testDataManager.StopAsync(new CancellationToken()).GetAwaiter().GetResult();
        
        base.Dispose();
    }
}