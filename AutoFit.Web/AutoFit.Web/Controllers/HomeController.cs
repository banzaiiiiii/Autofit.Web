﻿using System;
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

        public IActionResult Index()
        {
            return View("Index");
           //return View("UnderDevelopment");
        }

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

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Datenschutz()
        {
            return View();
        }

        public IActionResult Impressum()
        {
            return View();
        }


    }
}

