using System.Text.Json.Serialization;

namespace Aws.Meetup.Cdk.Configuration;

public record WebApiConfig
{
    [JsonPropertyName("env")]
    public required IDictionary<string, string> EnvironmentVariables { get; init; }
}