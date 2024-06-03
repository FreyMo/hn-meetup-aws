namespace Aws.Meetup.WebApi;

public record ReportSettings
{
    public required string QueueUrl { get; init; }
    
    public required string BucketName { get; init; }
    
    public required string KeyPrefix { get; init; }
}