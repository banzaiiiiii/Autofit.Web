using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AutoFit.Web.Controllers
{
    public class AuthController : Controller
    {
	    [Route("signout")]
		[HttpPost]
		public async Task SignOut()
	    {
		    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

		    var scheme = User.FindFirst("tfp").Value;
		    await HttpContext.SignOutAsync(scheme);
	    }

	    [Route("signin")]
	    public IActionResult SignIn()
	    {
		    return Challenge(new AuthenticationProperties {RedirectUri = "/"}, "B2C_1_sign_in");
	    }
	}
}
