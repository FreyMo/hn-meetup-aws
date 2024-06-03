namespace Aws.Meetup.Lambda;

public record ReportSettings
{
    public required string BucketName { get; init; }
    
    public required string KeyPrefix { get; init; }
}