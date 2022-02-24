using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using TripleSix.Core.Helpers;

namespace Sample.Common.Helpers
{
    public class MailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendMail(string[] toAddresses, string subject, string body)
        {
            var client = new SmtpClient()
            {
                Host = _configuration.GetValue<string>("Mail:Host"),
                Port = _configuration.GetValue<int>("Mail:Port"),
                Credentials = new NetworkCredential(_configuration.GetValue<string>("Mail:Username"), _configuration.GetValue<string>("Mail:Password")),
                EnableSsl = _configuration.GetValue<bool>("Mail:EnableSsl"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000,
            };

            var message = new MailMessage()
            {
                From = new MailAddress(_configuration.GetValue<string>("Mail:SenderAddress"), _configuration.GetValue<string>("Mail:SenderName")),
                Subject = subject,
                Body = body,
                SubjectEncoding = Encoding.UTF8,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
            };

            foreach (var toAddress in toAddresses)
                message.To.Add(toAddress);

            client.Send(message);
        }

        public void SendMail(string toAddresses, string subject, string body)
        {
            SendMail(new[] { toAddresses }, subject, body);
        }
    }
}
