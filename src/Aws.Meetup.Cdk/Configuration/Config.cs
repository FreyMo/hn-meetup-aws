using System.Text.Json;
using Amazon.CDK;

namespace Aws.Meetup.Cdk.Configuration;

public record Config
{
    public static Config ParseFrom(App app, string environment)
    {
        var context = app.Node.TryGetContext(environment);
        var json = JsonSerializer.Serialize(context);
    
        return JsonSerializer.Deserialize<Config>(json, JsonOptions.Default)!;
    }
    
    public required string Account { get; init; }
    
    public required string Region { get; init; }
    
    public required string Prefix { get; init; }
    
    public required WebApiConfig WebApi { get; init; }
    
    public required LambdaConfig Lambda { get; init; }
}