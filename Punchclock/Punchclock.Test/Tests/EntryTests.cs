using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Punchclock.Models.Dto;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Punchclock.Test.Tests;

public class EntryTests
{
    private DemoEntry GetDemoEntry()
    {
        return new DemoEntry{ CheckIn = DateTime.Parse("13.09.204"), CheckOut = DateTime.Now };
    }
    
    [Fact]
    public async Task CreateEntry()
    {
        await using var application = new PunchclockApplication();
        var inputEntry = GetDemoEntry();
        
        using var client = application.CreateClient();
        using var response = await client.PostAsJsonAsync("/entries", inputEntry);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var textRes = await response.Content.ReadAsStringAsync();
        var entryRes = JsonSerializer.Deserialize<EntryDto>(textRes, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
        Assert.Equivalent(inputEntry, entryRes);
    }

    [Fact]
    public async Task GetAllEntries()
    {
        await using var application = new PunchclockApplication();
        
        using var client = application.CreateClient();

        var inputEntry = GetDemoEntry();
        using var _ = await client.PostAsJsonAsync("/entries", inputEntry);
        
        using var response = await client.GetAsync("/entries");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var textRes = await response.Content.ReadAsStringAsync();
        var entryRes = JsonSerializer.Deserialize<List<EntryDto>>(textRes, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
        Assert.Equal(1, entryRes?.Count);
    }
    
    [Fact]
    public async Task DeleteEntry()
    {
        await using var application = new PunchclockApplication();
        
        using var client = application.CreateClient();
        var inputEntry = GetDemoEntry();
        using var _ = await client.PostAsJsonAsync("/entries", inputEntry);
        
        using var response = await client.DeleteAsync("/entries/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task PutEntry()
    {
        await using var application = new PunchclockApplication();
        var initialEntry = GetDemoEntry();
        var inputEntry = new { CheckIn = DateTime.Parse("14.09.204"), CheckOut = DateTime.Now, Id = 1L };
        
        using var client = application.CreateClient();
        using var _ = await client.PostAsJsonAsync("/entries", initialEntry);
        using var response = await client.PutAsJsonAsync("/entries", inputEntry);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var textRes = await response.Content.ReadAsStringAsync();
        var entryRes = JsonSerializer.Deserialize<EntryDto>(textRes, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});
        Assert.Equivalent(inputEntry, entryRes);
    }
}