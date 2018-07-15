using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

	    protected void ThrowIfNull(params object[] parameters)
	    {
		    for (var i = 0; i < parameters.Length; i++)
		    {
			    if (parameters[i] == null)
			    {
					throw new NullReferenceException();
			    }
		    }
	    }
    }
}
