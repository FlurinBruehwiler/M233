using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Projektarbeit.Models;

namespace Projektarbeit.Test.Tests;

public class UserTests : IDisposable
{
    private readonly string _adminToken;
    private readonly HttpClient _client;
    private readonly ProjektarbeitApplication _application;
    
    public UserTests()
    {
        _application = new ProjektarbeitApplication();
        _client = _application.CreateClient();
        
        _adminToken = Login("example4@example.com", "password");
    }
    
    [Fact]
    public async Task GetUsersAsAdministrator()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        using var response = await _client.GetAsync("/users");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jsonRes = await ResponseAsJsonAsync(response);
        Assert.Equal(4, jsonRes.GetArrayLength());
    }

    [Fact]
    public async Task PostUserAsAdministrator()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        var input = new CreateUserRequestDto
        {
            Email = "test@test.com",
            Firstname = "frank",
            Lastname = "herbert",
            Password = "meinpasswort",
            IsAdministrator = false
        };
        using var response = await _client.PostAsJsonAsync("/users", input);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task PatchUserAsAdministrator()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        var input = new PatchUserRequestDto
        {
            IsAdministrator = true,
            Id = 1
        };
        using var response = await _client.PatchAsJsonAsync("/users", input);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task DeleteUserAsAdministrator()
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_adminToken);
        using var response = await _client.DeleteAsync("/users/1");
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