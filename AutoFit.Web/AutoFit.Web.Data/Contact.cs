using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AutoFit.Web.Data
{
	public class Contact
	{
		public int Id { get; set; }
		[MaxLength(20)]
		public string FirstName { get; set; }
		[MaxLength(20)]
		public string LastName { get; set; }
		[MaxLength(40)]
		public string Email { get; set; }
		[MaxLength(50)]
		public string Subject { get; set; }
		[MaxLength(200)]
		public string Message { get; set; }
		public DateTime? TimeSpamp { get; set; }

	}
}
