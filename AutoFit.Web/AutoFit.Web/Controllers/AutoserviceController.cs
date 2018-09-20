using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class AutoServiceController : BaseController
    {
	    public AutoServiceController(ILoggerFactory loggerFactory)
		    : base(loggerFactory)
		{

	    }

		public IActionResult Index()
        {
            return View();
        }
    }
}