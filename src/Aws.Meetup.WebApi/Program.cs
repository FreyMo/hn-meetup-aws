using System.Net.Mime;
using Amazon.S3;
using Amazon.SQS;
using Aws.Meetup.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IAmazonSQS>(_ => new AmazonSQSClient());
builder.Services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client());
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddOptions<ReportSettings>().BindConfiguration(nameof(ReportSettings));

// Enable this for the full Controller style
// builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Enable this for the full Controller style
// app.MapControllers();

app.MapPost("/reports", async (IReportService reportService, Report report) =>
    {
        var guid = await reportService.TriggerReportGeneration(report);

        return Results.Created($"/reports/{guid}", null);
    })
    .WithName("CreateReport")
    .WithOpenApi();

app.MapGet("/reports/{guid:guid}", async (IReportService reportService, Guid guid) =>
    {
        var stream = await reportService.DownloadReport(guid);

        return stream is null
            ? Results.NotFound()
            : Results.File(stream, MediaTypeNames.Application.Pdf, $"{guid}.pdf");
    })
    .WithName("DownloadReport")
    .WithOpenApi();

app.Run();