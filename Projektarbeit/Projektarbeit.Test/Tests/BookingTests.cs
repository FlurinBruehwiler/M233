using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Projektarbeit.Endpoints.AuthenticationEndpoints.Dtos;
using Projektarbeit.Models;

namespace Projektarbeit.Test.Tests;

public class BookingTests : IDisposable
{
    private readonly string _adminToken;
    private readonly string _userToken;
    private readonly HttpClient _client;
    private readonly ProjektarbeitApplication _application;
    
    public BookingTests()
    {
        _application = new ProjektarbeitApplication();
        _client = _application.CreateClient();
        
        _adminToken = Login("example4@example.com", "password");
        _userToken = Login("example1@example.com", "password");
    }
    
    [Fact]
    public async Task GetBookingsAsAdministrator()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        using var response = await _client.GetAsync("/bookings");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonRes = await ResponseAsJsonAsync(response);
        Assert.Equal(4, jsonRes.GetArrayLength()); 
    }
    
    [Fact]
    public async Task GetBookingsAsUser()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_userToken);
        using var response = await _client.GetAsync("/bookings");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonRes = await ResponseAsJsonAsync(response);
        Assert.Equal(1, jsonRes.GetArrayLength()); 
    }
    
    [Fact]
    public async Task PostBookingAsAdministrator()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        var input = new
        {
            ParticipationCount = 1,
            Date = "3.2.2500",
            Time = Time.Ganztagig,
            User = 2
        };
        using var response = await _client.PostAsJsonAsync("/bookings", input);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task PostBookingAsUser()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_userToken);
        var input = new
        {
            ParticipationCount = 1,
            Date = "3.2.2500",
            Time = Time.Ganztagig
        };
        using var response = await _client.PostAsJsonAsync("/bookings", input);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Fact]
    public async Task PatchBookingAsAdministrator()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        var input = new
        {
            ParticipationCount = 2,
            Date = "3.2.2502",
            Time = Time.Nachmittag,
            User = 3,
            Id = 1
        };
        using var response = await _client.PatchAsJsonAsync("/bookings", input);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task PatchBookingAsUser()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_userToken);
        var input = new
        {
            ParticipationCount = 2,
            Date = "3.2.2502",
            Time = Time.Nachmittag,
            Id = 1
        };
        using var response = await _client.PatchAsJsonAsync("/bookings", input);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBookingAsAdministrator()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        using var response = await _client.DeleteAsync("/bookings/2");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteBookingAsUser()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_userToken);
        using var response = await _client.DeleteAsync("/bookings/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private async Task<JsonElement> ResponseAsJsonAsync(HttpResponseMessage response)
    {
        var textRes = await response.Content.ReadAsStreamAsync();
        var json = await JsonDocument.ParseAsync(textRes);
        return json.RootElement;
    }
    
    private string Login(string email, string password)
    {
        using var res =
            _client.PostAsJsonAsync("/login", 
                new LoginRequestDto { Email = email, Password = password }).GetAwaiter().GetResult();

        var authHeader = res.Headers
            .FirstOrDefault(x => x.Key == "Authorization");
        return authHeader.Value.First();
    }

    public void Dispose()
    {
        _client.Dispose();
        _application.Dispose();
    }
}