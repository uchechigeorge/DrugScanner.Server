using Microsoft.Extensions.Options;
using DrugScanner.Server.Models;

namespace DrugScanner.Server.Services;

public partial class MailSenderService : IMailSenderService
{

  private readonly MailSettings mMailSettings;

  public MailSenderService(IOptions<MailSettings> mailSettings)
  {
    mMailSettings = mailSettings.Value;
  }

}