using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFit.Web.Abstractions;
using AutoFit.Web.ViewModels.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IFileService _fileService;
        public ShopController(IFileService fileService, ILoggerFactory loggerFactory) :base(loggerFactory)
        {
            _fileService = fileService;
        }
       
        public IActionResult Index()
        {
            var model = new FilesViewModel();
            model.ContainerList = _fileService.ListContainersAsync();
            foreach (var container in model.ContainerList)
            {
                model.ContainerDetailsList.Add(
                                    new AzureContainerDetails()
                                    {
                                        ContainerMetadata = container.Metadata,
                                        ContainerName = container.Name,
                                        FileNameList = _fileService.GetBlobsFromContainer(container.Name)
                                    });


            }

            return View(model);
        }

        public IActionResult Kaufabwicklung()
        {
            return View();
        }
    }
}