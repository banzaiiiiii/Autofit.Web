using System.ComponentModel.DataAnnotations;

namespace AutoFit.Web.ViewModels
{
    public class ContactViewModel
    {
		[Required]
		[MaxLength(30)]
		public string FirstName { get; set; }
	    [Required]
	    [MaxLength(30)]
		public string LastName { get; set; }
	    [Required]
	    [MaxLength(50)]
		public string Subject { get; set; }
	    [MaxLength(200)]
		public string Message { get; set; }
	    [Required]
	    [MaxLength(40)]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
		[MaxLength(12)]
		[DataType(DataType.PhoneNumber)]
		public string TelefonNummer { get; set; }
		[Required]
		public bool IsChecked { get; set; }
	}
}
