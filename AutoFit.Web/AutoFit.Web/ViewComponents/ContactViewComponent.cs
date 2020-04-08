using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.ViewComponents
{
    public class ContactViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {


            
            return View("_contact");
        }
    }
}
