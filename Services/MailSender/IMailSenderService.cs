using DrugScanner.Server.Models;

namespace DrugScanner.Server.Services
{
  public interface IMailSenderService
  {
    /// <summary>
    /// Tries to send a mail message
    /// </summary>
    /// <param name="options">The mail options</param>
    /// <returns></returns>
    Task<bool> TrySendEmailAsync(MailSenderOptions options);
  }
}