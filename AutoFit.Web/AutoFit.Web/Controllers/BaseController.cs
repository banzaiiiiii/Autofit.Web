using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class BaseController : Controller
    {
		protected ILoggerFactory _loggerFactory { get; }
	    protected ILogger _logger { get; }


		protected BaseController(ILoggerFactory loggerFactory)
		{
			_loggerFactory = loggerFactory;
			_logger = _loggerFactory.CreateLogger(GetType());
		}

	}
}