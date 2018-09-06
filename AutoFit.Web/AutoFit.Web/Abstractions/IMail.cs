using System.Threading.Tasks;

namespace AutoFit.Web.Abstractions
{
   public interface IMail
   {
	   Task SendEmailAsync(string subject, string emailBody);
   }
}
