namespace Module.Identity.Persistence.Email;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string token);

}