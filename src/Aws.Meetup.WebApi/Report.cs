using System.ComponentModel.DataAnnotations;

namespace Aws.Meetup.WebApi;

public record Report
{
    [MaxLength(256)]
    public required string Title { get; init; }
    
    [MaxLength(1024)]
    public required string Content { get; init; }
}