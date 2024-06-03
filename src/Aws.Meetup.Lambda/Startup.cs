using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace Aws.Meetup.Lambda;

[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true);
        var configuration = builder.Build();
        
        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client());
        services.AddSingleton<IReportGenerator, ReportGenerator>();
        services.AddOptions<ReportSettings>().BindConfiguration(nameof(ReportSettings));
        
        services.AddSingleton<IConfiguration>(configuration);
        
        QuestPDF.Settings.License = LicenseType.Community;
    }
}
