namespace Aws.Meetup.Cdk.Configuration;

public record LambdaConfig
{
    public required string Handler { get; init; }
    
    public required string CodePath { get; init; }
}