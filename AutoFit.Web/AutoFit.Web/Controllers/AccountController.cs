using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoFit.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AutoFit.Web.Controllers
{

 
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel inputModel)
        {

            var userNameFromAppsettings = _configuration.GetSection("UserManagement").GetSection("username").Value;
            var passwordFromAppsettings = _configuration.GetSection("UserManagement").GetSection("password").Value;


            if (inputModel.Username.Equals(userNameFromAppsettings) && inputModel.Password.Equals(passwordFromAppsettings))
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userNameFromAppsettings),
                    new Claim(ClaimTypes.Role, "Administrator" )
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("index", "home");
            }

            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(inputModel);

        }


    }
}
