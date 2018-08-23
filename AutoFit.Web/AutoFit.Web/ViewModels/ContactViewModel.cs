using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.ViewModels
{
    public class ContactViewModel
    {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Subject { get; set; }
	    public string Message { get; set; }
	    public string Email { get; set; }
	}
}
