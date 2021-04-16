using System;
using System.Diagnostics;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;
using AutoFit.Web.Data;
using AutoFit.Web.Data.Abstractions;

using Microsoft.AspNetCore.Mvc;
using AutoFit.Web.Models;
using AutoFit.Web.Services;
using AutoFit.Web.ViewModels;
using AutoFit.Web.ViewModels.Files;

using AutoMapper;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace AutoFit.Web.Controllers
{
    public class HomeController : BaseController
    {

        
        private readonly IMail _mailService;
        private readonly IFileService _fileService;

        public HomeController(IFileService fileService, IMail mailService, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
        }
        [Authorize]
        public IActionResult Index()
        {
            return View("Index");
        }
        public IActionResult UnderDev()
        {
            // return View("Index");
            return View("UnderDevelopment");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                var newContact = contactViewModel;
                newContact.TimeStamp = DateTime.Now;


                string emailBody = "Neue Email von: " + newContact.FirstName + newContact.LastName + " mit der Adresse: " + newContact.Email
                                 + " die hinterlassene Nachricht lautet: " + newContact.Message;

                try
                {
                    //_contactService.Add(newContact);
                    //await _contactService.SaveAsync();
                    await _mailService.SendEmailAsync(newContact, emailBody);

                    return View("MessageSendSuccess");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed sending or saving contact");
                }
            }

            return View("MessageSendFailed");

        }
        [Authorize]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public IActionResult Datenschutz()
        {
            return View("DatenschutzV2");
        }
        [Authorize]
        public IActionResult Impressum()
        {
            return View();
        }
        [Authorize]
        public IActionResult AutofitRochlitz()
        {
            return View("_news_rochlitz");
        }
        [Authorize]
        public IActionResult AutofitBurgstädt()
        {
            return View("_news_burgstädt");
        }
        [Authorize]
        public IActionResult Jobs()
        {
            return View("_jobs");
        }
        [Authorize]
        public IActionResult Fahrzeuge()
        {
            return View("Fahrzeuge");
        }

    }
}

