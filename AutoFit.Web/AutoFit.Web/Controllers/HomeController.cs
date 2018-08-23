using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Data;

using Microsoft.AspNetCore.Mvc;
using AutoFit.Web.Models;
using AutoFit.Web.Services;
using AutoFit.Web.ViewModels;

using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly HomeService _homeService;
	    private readonly ContactService _contactService;

        public HomeController(HomeService homeService, ContactService contactService, ILogger<HomeController> logger)
	        :base(logger)
        {
	        _homeService = homeService;
	        _contactService = contactService;
        }

        public IActionResult Index()
        {
	        
	        return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

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

	        await _contactService.AddAsync(contact);
	        return View("Contact", contact);
        }	

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
