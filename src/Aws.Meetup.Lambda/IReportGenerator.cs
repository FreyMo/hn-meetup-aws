namespace Aws.Meetup.Lambda;

public interface IReportGenerator
{
    Task GenerateAndUploadReport(CreateReportRequest request);
}
