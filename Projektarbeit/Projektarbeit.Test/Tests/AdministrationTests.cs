using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Projektarbeit.Models;

namespace Projektarbeit.Test.Tests;

public class AdministrationTests : IDisposable
{
        private readonly string _adminToken;
    private readonly HttpClient _client;
    private readonly ProjektarbeitApplication _application;
    
    public AdministrationTests()
    {
        _application = new ProjektarbeitApplication();
        _client = _application.CreateClient();
        
        _adminToken = Login("example4@example.com", "password");
    }
    
    [Fact]
    public async Task GetWorkplaceAmount()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        using var response = await _client.GetAsync("/workplace");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var res = await response.Content.ReadAsStringAsync();
        Assert.Equal("\"20\"", res); 
    }
    
    [Fact]
    public async Task SetWorkplaceAmount()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        using var response = await _client.PutAsync("/workplace/30", new StringContent(""));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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