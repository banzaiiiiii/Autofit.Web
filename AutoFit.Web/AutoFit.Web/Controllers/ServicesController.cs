using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoFit.Web.Controllers
{
    [Authorize]
    public class ServicesController : Controller
    {
        public IActionResult Autoglas()
        {
            return View("Autoglas");
        }

        public IActionResult Index()
        {
            return View("_leistungen");
        }
    }
}