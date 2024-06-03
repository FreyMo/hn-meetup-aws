using System.Text.Json;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Aws.Meetup.WebApi;

public class ReportService(IAmazonSQS sqsClient, IAmazonS3 s3Client, IOptions<ReportSettings> options) : IReportService
{
    public async Task<Guid> TriggerReportGeneration(Report report)
    {
        var guid = Guid.NewGuid();
        
        await sqsClient.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = options.Value.QueueUrl,
            MessageBody = JsonSerializer.Serialize(new CreateReportRequest(guid, report.Title, report.Content))
        });

        return guid;
    }

    public async Task<Stream?> DownloadReport(Guid guid)
    {
        try
        {
            var response = await s3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = options.Value.BucketName,
                Key = $"{options.Value.KeyPrefix}/{guid}"
            });

            return response!.ResponseStream;
        }
        catch (AmazonS3Exception)
        {
            return null;
        }
    }
}