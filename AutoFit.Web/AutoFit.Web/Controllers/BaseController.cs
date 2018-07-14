using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class BaseController : Controller
    {
	    private readonly ILogger _logger;

		public BaseController() { }

	    protected BaseController(ILogger logger)
	    {
		    _logger = logger;
	    }

	    protected void HandleError(string errormessage, Exception e)
	    {
		    _logger.LogError(errormessage, e);

		    ModelState.AddModelError("", errormessage);
	    }

	    protected void HandleWarning(string errorMessage)
	    {
		    _logger.LogWarning(errorMessage);

		    ModelState.AddModelError("", errorMessage);
	    }

	    protected JsonResult BuildJsonResult(bool succeeded, bool includeModelStateErrors = true)
	    {
		    if (!includeModelStateErrors) return new JsonResult(new { status = succeeded });

		    var result = new JsonResult(new
		                                {
			                                status = succeeded,
			                                validationErrors = ModelState.Keys
			                                                             .SelectMany(key => ModelState[key].Errors)
			                                                             .Select(e => $"{e.ErrorMessage}</br>")
			                                                             .ToArray()
		                                });
		    return result;
	    }
    }
}