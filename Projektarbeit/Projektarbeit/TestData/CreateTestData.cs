using Microsoft.EntityFrameworkCore;
using Projektarbeit.Models;
using Projektarbeit.Services;

namespace Projektarbeit.TestData;

public class TestDataManager : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<User> _users;
    private readonly List<Booking> _bookings;
    public bool ShouldWork { get; set; }

    public TestDataManager(IServiceProvider serviceProvider)
    {
        ShouldWork = true;
        _serviceProvider = serviceProvider;
        AuthService.CreatePasswordHash("password", out var hash, out var salt);
        
        _users = new List<User>
        {
            new()
            {
                Firstname = "user1",
                LastName = "Test",
                IsAdministrator = false,
                Email = "example1@example.com",
                PasswordHash = hash,
                PasswordSalt = salt
            },
            new()
            {
                Firstname = "user2",
                LastName = "Test",
                IsAdministrator = false,
                Email = "example2@example.com",
                PasswordHash = hash,
                PasswordSalt = salt
            },
            new()
            {
                Firstname = "user3",
                LastName = "Test",
                IsAdministrator = false,
                Email = "example3@example.com",
                PasswordHash = hash,
                PasswordSalt = salt
            },
            new()
            {
                Firstname = "user4",
                LastName = "Test",
                IsAdministrator = true,
                Email = "example4@example.com",
                PasswordHash = hash,
                PasswordSalt = salt
            }
        };
        _bookings = new List<Booking>
        {
            new()
            {
                Date = new DateOnly(3000, 1, 5),
                Status = Status.Abgelehnt,
                Time = Time.Ganztagig,
                ParticipationCount = 3,
                User = _users[0]
            },
            new()
            {
                Date = new DateOnly(2100, 2, 4),
                Status = Status.Angenommen,
                Time = Time.Ganztagig,
                ParticipationCount = 5,
                User = _users[1]
            },
            new()
            {
                Date = new DateOnly(2050, 3, 3),
                Status = Status.Beantragt,
                Time = Time.Ganztagig,
                ParticipationCount = 2,
                User = _users[2]
            },
            new()
            {
                Date = new DateOnly(3000, 4, 2),
                Status = Status.Beantragt,
                Time = Time.Ganztagig,
                ParticipationCount = 1,
                User = _users[3]
            }
        };
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!ShouldWork)
            return;
        
        using var scope = _serviceProvider.CreateScope();
        var databaseContext =
            scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        await databaseContext.Bookings.AddRangeAsync(_bookings, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (!ShouldWork)
            return;
        
        using var scope = _serviceProvider.CreateScope();
        var databaseContext =
            scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        foreach (var booking in _bookings)
        {
            if(await databaseContext.Bookings.ContainsAsync(booking, cancellationToken: cancellationToken))
                databaseContext.Bookings.Remove(booking);
        }

        foreach (var user in _users)
        {
            if(await databaseContext.Users.ContainsAsync(user, cancellationToken: cancellationToken))
                databaseContext.Users.Remove(user);
        }

        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}