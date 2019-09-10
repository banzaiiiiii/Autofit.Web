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
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

	    public IActionResult Jobs()
	    {
		    return View();
	    }

	    public async Task<IActionResult> Cars()
	    {
			var containerList = await _fileService.ListContainersAsync();

		    var model = new FilesViewModel();
		    foreach (var container in containerList)
		    {
			    model.Container.Add(
			                        new AzureContainerDetails()
			                        {
				                        ContainerName = container.Name,
				                        FileNameList = _fileService.GetBlobsFromContainer(container.Name)
			                        });


		    }
		    return View(model);
		}

	}
}
