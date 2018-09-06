using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;

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

        public async Task SendEmailAsync(string subject, string emailBody)
        {

	        var senderEmail = _configuration.GetSection("Email").GetSection("SenderEmail").Value;
	        var senderPassword = _configuration.GetSection("Email").GetSection("SenderPasswort").Value;
	        var toEmail = _configuration.GetSection("Email").GetSection("ReceivingEmail").Value;

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                MailMessage mailMessage = new MailMessage(senderEmail, toEmail, subject, emailBody);
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;

               client.Send(mailMessage);
            }
              

		    await Task.FromResult(0);
	    }
    }
}
