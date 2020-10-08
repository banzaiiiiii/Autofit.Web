using AutoFit.Web.ViewModels;
using System.Threading.Tasks;

namespace AutoFit.Web.Abstractions
{
   public interface IMail
   {
	   Task SendEmailAsync(ContactViewModel contact, string emailBody);
   }
}
