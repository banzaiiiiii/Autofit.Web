using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Data;
using AutoFit.Web.Data.Abstractions;

using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Services
{
    public class BaseService 
    {
		protected ILogger Logger { get; set; }
	  
	    protected BaseService()
	    {
	    }

	    protected BaseService(ILogger logger)
	    {
		    Logger = logger;
	    }

    }
}
