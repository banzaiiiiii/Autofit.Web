using System;
using System.Diagnostics;
using System.Threading.Tasks;

using AutoFit.Web.Data;

using Microsoft.AspNetCore.Mvc;
using AutoFit.Web.Models;
using AutoFit.Web.Services;
using AutoFit.Web.ViewModels;

using AutoMapper;

using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class HomeController : BaseController
    {

	    private readonly ContactService _contactService;
	    private readonly MailService _mailService;

        public HomeController(ContactService contactService, MailService mailService, ILoggerFactory loggerFactory)
	        : base(loggerFactory)
		{
	        _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
	        _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }

        public IActionResult Index()
        {
	        return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel contactViewModel)
        {
	        if (ModelState.IsValid)
	        {
		         var newContact = Mapper.Map<Contact>(contactViewModel);
	             newContact.TimeStamp = DateTime.Now;
						

	        string emailBody = "Neue Email von: " + newContact.FirstName + newContact.LastName + " mit der Adresse: " + newContact.Email
	                         + " die hinterlassene Nachricht lautet: " + newContact.Message;

			 
		        try
		        {
			        _contactService.Add(newContact);
			        await _contactService.SaveAsync();
			        await _mailService.SendEmailAsync(newContact.Subject, emailBody);

		    return View("SuccessView");
		        }
		        catch (Exception ex)
		        {
					_logger.LogError(ex, "Failed sending or saving contact");
		        }
	        
			}

	        return View();

        }

		[HttpGet]
	    public IActionResult Contact()
	    {
		    return View();
	    }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

	    public IActionResult Jobs()
	    {
		    return View();
	    }

	    public IActionResult Cars()
	    {
		    return View();
	    }



	}
}
