using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using DrugScanner.Server.Models;

namespace DrugScanner.Server.Services;

public partial class MailSenderService : IMailSenderService
{

  #region Methods

  public async Task<bool> TrySendEmailAsync(MailSenderOptions options)
  {
    try
    {
      var email = new MimeMessage();
      options.To?.ForEach(t =>
      {
        email.To.Add(MailboxAddress.Parse(t));
      });

      email.From.Add(new MailboxAddress(mMailSettings.DisplayName, mMailSettings.Address));
      email.Subject = options.Subject;

      var builder = new BodyBuilder();
      if (options.Attachments != null)
      {
        byte[] fileBytes;
        foreach (var file in options.Attachments)
        {
          if (file.Length > 0)
          {
            using (var ms = new MemoryStream())
            {
              file.CopyTo(ms);
              fileBytes = ms.ToArray();
            }
            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
          }
        }
      }

      builder.HtmlBody = options.Body;
      email.Body = builder.ToMessageBody();
      using var smtp = new SmtpClient();
      smtp.Connect(mMailSettings.Host, mMailSettings.Port, SecureSocketOptions.StartTls);
      smtp.Authenticate(mMailSettings.Address, mMailSettings.Password);
      await smtp.SendAsync(email);
      smtp.Disconnect(true);
      return true;
    }
    catch (Exception ex)
    {
      if (IoCContainer.Environment!.IsDevelopment())
      {
        Console.WriteLine(ex.ToString());
      }

      return false;
    }
  }

  #endregion

}