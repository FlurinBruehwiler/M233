var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello rld!");

app.MapGet("calculator/add/{term1:int}/{term2:int}", (int term1, int term2) =>
{
    return term1 + term2;
});

app.Run();