using System.Text.Json;

namespace Aws.Meetup.Cdk;

public static class JsonOptions
{
    public static JsonSerializerOptions Default { get; } = new (JsonSerializerDefaults.Web);
}