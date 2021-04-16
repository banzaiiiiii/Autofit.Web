using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    [Authorize]
    public class LeistungenController : BaseController
    {
        public LeistungenController(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            
        }

        public IActionResult Gebrauchtwagen()
        {
            return View();
        }

        public IActionResult Autopflege()
        {
            return View();
        }

        public IActionResult Reparaturen()
        {
            return View();
        }
        public IActionResult Autoglas()
        {
            return View();
        }

        public IActionResult Fehlerdiagnose()
        {
            return View();
        }

        public IActionResult HU_AU()
        {
            return View();
        }

        public IActionResult Inspektionen()
        {
            return View();
        }

        public IActionResult Reifenservice()
        {
            return View();
        }

        public IActionResult Klimaservice()
        {
            return View();
        }

        public IActionResult Ölwechsel()
        {
            return View("Ölservice");
        }
    }
}