using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Sample.Common;
using Sample.Middle.Services;

namespace Sample.Middle.Helpers
{
    public class MailHelper
    {
        public ISettingService SettingService { get; set; }

        public async Task SendMail(string[] toAddresses, string subject, string body)
        {
            var identity = new Identity();
            var host = await SettingService.GetValue<string>(identity, "mail.host");
            var port = await SettingService.GetValue<int>(identity, "mail.port");
            var username = await SettingService.GetValue<string>(identity, "mail.username");
            var password = await SettingService.GetValue<string>(identity, "mail.password");
            var enableSsl = new[] { "1", "true" }.Contains(await SettingService.GetValue<string>(identity, "mail.enable-ssl"));
            var senderAddress = await SettingService.GetValue<string>(identity, "mail.sender-address");
            var senderName = await SettingService.GetValue<string>(identity, "mail.sender-name");

            var client = new SmtpClient()
            {
                Host = host,
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000,
            };

            var message = new MailMessage()
            {
                From = new MailAddress(senderAddress, senderName),
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

        public async Task SendMail(string toAddresses, string subject, string body)
        {
            await SendMail(new[] { toAddresses }, subject, body);
        }
    }
}
