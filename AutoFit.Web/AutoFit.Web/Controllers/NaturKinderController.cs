using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class NaturkinderController : BaseController
    {
		public NaturkinderController(ILogger<NaturkinderController> logger)
			: base(logger)
		{

		}

		public IActionResult Index()
        {
            return View();
        }

	    public IActionResult Intern()
	    {

		    return View();
	    }

	}
}