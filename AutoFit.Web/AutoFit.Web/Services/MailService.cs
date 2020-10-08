using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;
using AutoFit.Web.ViewModels;
using Microsoft.Extensions.Configuration;

namespace AutoFit.Web.Services
{
    public class MailService : IMail
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(ContactViewModel contact, string emailBody)
        {

            var senderEmail = _configuration.GetSection("Email").GetSection("SenderEmail").Value;
            var senderPassword = _configuration.GetSection("Email").GetSection("SenderPasswort").Value;
            var autofitRochlitz = _configuration.GetSection("Email").GetSection("AutoFitRochlitz").Value;
            var autofitBurgstädt = _configuration.GetSection("Email").GetSection("AutoFitBurgstädt").Value;
            var receivingMail = ""; 

            if (string.Equals(contact.MailReceiver, "Autofit Rochlitz"))
            {
                 receivingMail = autofitRochlitz;
            }
            if (string.Equals(contact.MailReceiver, "Autofit Burgstädt"))
            {
                 receivingMail = autofitBurgstädt;
            }

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                var mailMessage = new MailMessage(senderEmail, receivingMail, contact.Subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;

                client.Send(mailMessage);
            }


            await Task.FromResult(0);
        }
    }
}
