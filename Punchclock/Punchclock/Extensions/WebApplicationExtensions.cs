using Punchclock.Endpoints;

namespace Punchclock.Extensions;

public static class WebApplicationExtensions
{
    public static void UseEndpoint(this WebApplication app)
    {
        var endpointDefinitions = typeof(IEndpoints).Assembly.ExportedTypes
            .Where(x => typeof(IEndpoints).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
            .Select(Activator.CreateInstance).Cast<IEndpoints>().ToList();
            
        foreach (var endpointDefinition in endpointDefinitions)
        {
            endpointDefinition.DefineEndpoints(app);
        }
    }
}