using System.Diagnostics;
using System.Threading.Tasks;

using AutoFit.Web.Data;

using Microsoft.AspNetCore.Mvc;
using AutoFit.Web.Models;
using AutoFit.Web.Services;
using AutoFit.Web.ViewModels;

using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class HomeController : BaseController
    {

	    private readonly ContactService _contactService;
	    private readonly MailService _mailService;

        public HomeController(ContactService contactService, MailService mailService, ILogger<HomeController> logger)
	        :base(logger)
        {
	        _contactService = contactService;
	        _mailService = mailService;
        }

        public IActionResult Index()
        {
	        
	        return View();
        }

		//[HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel contactViewModel)
        {
	        if (!ModelState.IsValid)
	        {
		        return View();
	        }

	        var contact = new Contact()
	                      {
		                      FirstName = contactViewModel.FirstName
		                      , LastName = contactViewModel.LastName
		                      , Email = contactViewModel.Email
		                      , Message = contactViewModel.Message
		                      , Subject = contactViewModel.Subject
						};

	        string emailBody = "Neue Email von: " + contact.FirstName + contact.LastName + " mit der Adresse: " + contact.Email
	                         + " die hinterlassene Nachricht lautet: " + contact.Message;
			
            await _contactService.AddAsync(contact);
	        await _mailService.SendEmailAsync(contact.Subject, emailBody);

            return View("Contact", contact);
        }	

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

	    

		
    }
}
