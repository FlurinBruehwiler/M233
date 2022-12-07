using Projektarbeit.Endpoints;

namespace Projektarbeit.Extensions;

public static class WebApplicationExentions
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