namespace Aws.Meetup.WebApi;

public interface IReportService
{
    Task<Guid> TriggerReportGeneration(Report report);
    
    Task<Stream?> DownloadReport(Guid guid);
}