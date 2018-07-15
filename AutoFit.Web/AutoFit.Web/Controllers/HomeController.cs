using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public HomeController(HomeService homeService, ILogger<HomeController> logger)
	        :base(logger)
        {
	        _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
	        var testViewUebergabe = await _homeService.LoadAllBuisnesses();

	        return View(testViewUebergabe);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
