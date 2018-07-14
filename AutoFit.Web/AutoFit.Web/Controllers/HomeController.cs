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
        //private readonly HomeService _homeService;

        public HomeController(ILogger<HomeController> logger)
	        :base(logger)
        {
			Exception e = new NullReferenceException();
            HandleError(" hey im an error", e);
			

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
