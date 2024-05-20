using DrugScanner.Server.Models;

namespace DrugScanner.Server.Services;

public class ServiceModule
{
  public static void RegisterServices(IServiceCollection services)
  {
    services.AddScoped<INewsFeedsService, NewsFeedsService>();
    services.AddScoped<IDrugsService, DrugsService>();
    services.AddScoped<IReportsService, ReportsService>();

    services.AddScoped<IMailSenderService, MailSenderService>();
    services.Configure<MailSettings>(IoCContainer.Configuration!.GetSection("MailSettings"));

  }
}