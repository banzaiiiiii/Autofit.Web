using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;
using AutoFit.Web.Data;
using AutoFit.Web.Data.Abstractions;
using AutoFit.Web.Services;
using AutoFit.Web.ViewModels;

using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class NaturkinderController : BaseController
    {

	    private readonly IContact _contactService;
	    private readonly IMail _mailService;

		public NaturkinderController(IContact contactService, IMail mailService, ILoggerFactory loggerFactory)
			: base(loggerFactory)
		{
			_contactService = contactService;
			_mailService = mailService;
		}

	    [HttpGet]
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

			    _contactService.Add(newContact);
			    try
			    {
				    await _contactService.SaveAsync();

			    await _mailService.SendEmailAsync(newContact.Subject, emailBody);

			    return View("SuccessView");
			    }
			    catch (Exception ex)
			    {
					_logger.LogError(ex, "Error saving or sending Contact.");
			    }
			    
		    }

		    return View();

	    }

	    [HttpGet]
	    public IActionResult Contact()
	    {
		    return View();
	    }

		[HttpGet]
	    public IActionResult BlogPost()
	    {
		    return View();
	    }

	    [HttpGet]
	    public IActionResult Verein()
	    {
		    return View();
	    }

	    [HttpGet]
	    public IActionResult Jobs()
	    {
		    return View();
	    }

	    public IActionResult Impressum()
	    {
		    return View("Impressum");
	    }

	    public IActionResult Datenschutz()
	    {
		    return View("Datenschutz");
	    }

	}
}