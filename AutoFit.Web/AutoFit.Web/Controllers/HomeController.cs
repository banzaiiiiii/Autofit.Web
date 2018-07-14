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

namespace AutoFit.Web.Controllers
{
    public class HomeController : Controller
    {
	    private readonly HomeService _homeService;

	    public HomeController(HomeService homeService)
	    {
		    _homeService = homeService;
	    }

        public IActionResult Index()
        {
	        var buissnessView = new HomeIndexViewModel()
	                            {
		                            FirmenName = "AutoFit",
		                            Ort = "Rochlitz"
	                            };
	        return View(buissnessView);
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
