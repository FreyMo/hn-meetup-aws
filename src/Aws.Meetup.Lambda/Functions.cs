using System.Text.Json;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Aws.Meetup.Lambda;

public class Functions(IReportGenerator reportGenerator)
{
    [LambdaFunction]
    public async Task PdfReportGenerator(SQSEvent sqsEvent, ILambdaContext _)
    {
        foreach (var message in sqsEvent.Records)
        {
            var request = JsonSerializer.Deserialize<CreateReportRequest>(message.Body);

            if (request is null)
            {
                throw new InvalidOperationException(
                    $"Cannot handle message {message.MessageId}. Content: {message.Body}");
            }

            await reportGenerator.GenerateAndUploadReport(request);
        }
    }
}