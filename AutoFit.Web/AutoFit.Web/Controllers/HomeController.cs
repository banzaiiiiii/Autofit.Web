using System;
using System.Diagnostics;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;
using AutoFit.Web.Data;
using AutoFit.Web.Data.Abstractions;

using Microsoft.AspNetCore.Mvc;
using AutoFit.Web.Models;
using AutoFit.Web.Services;
using AutoFit.Web.ViewModels;
using AutoFit.Web.ViewModels.Files;

using AutoMapper;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace AutoFit.Web.Controllers
{
    public class HomeController : BaseController
    {

	    private readonly IContact _contactService;
	    private readonly IMail _mailService;
	    private readonly IFileService _fileService;

        public HomeController(IFileService fileService, IContact contactService, IMail mailService, ILoggerFactory loggerFactory)
	        : base(loggerFactory)
        {
	        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
	        _contactService = contactService ?? throw new ArgumentNullException(nameof(contactService));
	        _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }

        public IActionResult Index()
        {
			RecordInSession("home");
	        return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel contactViewModel)
        {
			RecordInSession("contact");
	        if (ModelState.IsValid)
	        {
		         var newContact = Mapper.Map<Contact>(contactViewModel);
	             newContact.TimeStamp = DateTime.Now;
						

	        string emailBody = "Neue Email von: " + newContact.FirstName + newContact.LastName + " mit der Adresse: " + newContact.Email
	                         + " die hinterlassene Nachricht lautet: " + newContact.Message;

			 
		        try
		        {
			        //_contactService.Add(newContact);
			        //await _contactService.SaveAsync();
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
			RecordInSession("Error");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

	    public IActionResult Jobs()
	    {
			RecordInSession("Jobs");
			return View();
	    }

		public IActionResult Datenschutz()
		{
			RecordInSession("Datenschutz");
			return View();
		}

		public IActionResult Impressum()
		{
			RecordInSession("Impressum");
			return View();
		}

		private void RecordInSession(string action)
		{
			var paths = HttpContext.Session.GetString("actions") ?? string.Empty;
			HttpContext.Session.SetString("actions", paths + ";" + action);
		}
	}
}

