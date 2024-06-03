namespace Aws.Meetup.WebApi;

public record CreateReportRequest(Guid Guid, string Title, string Content);
