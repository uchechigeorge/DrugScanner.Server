namespace DrugScanner.Server.Models;

public class MailSenderOptions
{
  public List<string>? To { get; set; }
  public string? Subject { get; set; }
  public string? Body { get; set; }
  public List<IFormFile>? Attachments { get; set; }

}

public class MailSettings
{
  public string? Address { get; set; }
  public string? From { get; set; }
  public string? DisplayName { get; set; }
  public string? Password { get; set; }
  public string? Host { get; set; }
  public int Port { get; set; }
}