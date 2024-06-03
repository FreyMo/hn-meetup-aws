using System.Net.Mime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Aws.Meetup.Lambda;

public class ReportGenerator(IAmazonS3 s3Client, IOptions<ReportSettings> options) : IReportGenerator
{
    public async Task GenerateAndUploadReport(CreateReportRequest request)
    {
        using var stream = new MemoryStream();
        
        Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
        
                    page.Header()
                        .Text(request.Title)
                        .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);
        
                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(x =>
                        {
                            x.Spacing(20);
                
                            x.Item().Text(request.Content);
                            x.Item().Image(Placeholders.Image(200, 100));
                        });
        
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            })
            .GeneratePdf(stream);

        await s3Client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = options.Value.BucketName,
            Key = $"{options.Value.KeyPrefix}/{request.Guid}",
            InputStream = stream,
            ContentType = MediaTypeNames.Application.Pdf
        });
    }
}
