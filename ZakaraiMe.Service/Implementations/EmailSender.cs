namespace ZakaraiMe.Service.Implementations
{
    using Contracts;
    using Microsoft.Extensions.Configuration;
    using System.Net;
    using System.Net.Mail;
    using System.Threading.Tasks;

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            using (SmtpClient client = new SmtpClient())
            {
                NetworkCredential credential = new NetworkCredential
                {
                    UserName = configuration["Mailing:Address"],
                    Password = configuration["Mailing:Password"]
                };

                client.Credentials = credential;
                client.Host = configuration["Mailing:ServerName"];
                client.Port = int.Parse(configuration["Mailing:Port"]);
                client.EnableSsl = true;
                client.Timeout = 10000;

                using (MailMessage emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(email));
                    emailMessage.From = new MailAddress(configuration["Mailing:Address"]);
                    emailMessage.Subject = subject;
                    emailMessage.Body = message;
                    client.Send(emailMessage);
                }
            }
            await Task.CompletedTask;
        }
    }
}
