using System.Net;

namespace M223ErweitertesHelloWorldTest;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        using var application = new TestApplication();

        using var client = application.CreateClient();
        using var response = await client.GetAsync("/calculator/add/3/4");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("7", await response.Content.ReadAsStringAsync());
    }
}