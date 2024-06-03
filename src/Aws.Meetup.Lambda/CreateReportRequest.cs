namespace Aws.Meetup.Lambda;

public record CreateReportRequest(Guid Guid, string Title, string Content);
