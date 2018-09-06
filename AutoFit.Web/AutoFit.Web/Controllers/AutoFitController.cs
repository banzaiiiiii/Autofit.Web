﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class AutoFitController : BaseController
    {
	    public AutoFitController(ILogger<AutoFitController> logger)
		    : base(logger)
	    {

	    }

        public IActionResult Index()
        {
            return View();
        }
    }
}