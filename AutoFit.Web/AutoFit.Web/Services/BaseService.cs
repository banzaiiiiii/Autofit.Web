using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Services
{
	public class BaseService 
    {
		protected ILoggerFactory _LoggerFactory { get; }
	    protected ILogger _logger { get; }

	    protected BaseService(ILoggerFactory loggerFactory)
	    {
		    _LoggerFactory = loggerFactory;
		    _logger = _LoggerFactory.CreateLogger(GetType());
	    }

	}
}
