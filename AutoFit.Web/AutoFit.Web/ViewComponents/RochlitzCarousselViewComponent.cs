using AutoFit.Web.Abstractions;
using AutoFit.Web.ViewModels.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.ViewComponents
{
    public class RochlitzCarousselViewComponent : ViewComponent
    {

        private readonly IFileService _fileService;

        public RochlitzCarousselViewComponent(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
         
            var model = new FilesViewModel();
            
            model.RochlitzCarousselContainer = _fileService.ResolveCloudBlobContainer("carousselrochlitz");

            return View("_cars", model);
        }

    }
}
