using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace AutoFit.Web.ViewModels.Files
{ 
	    public class FileInputModel
	    {
		    public IFormFile FileToUpload { get; set; }
	    }
}
