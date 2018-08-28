using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoFit.Web.Data.Abstractions
{
   public interface IMail
   {
	   Task SendEmailAsync(string subject, string emailBody);
   }
}
