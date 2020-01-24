using AutoFit.Web.Abstractions;
using AutoFit.Web.ViewModels.Files;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AutoFit.Web.ViewComponents
{

    public class BurgstädtCarousselViewComponent : ViewComponent
    {

        private readonly IFileService _fileService;

        public BurgstädtCarousselViewComponent(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {

            var model = new FilesViewModel();

            model.RochlitzCarousselContainer = _fileService.ResolveCloudBlobContainer("carousselburgstaedt");
            model.ContainerDetailsList.Add(new AzureContainerDetails()
            {
                ContainerName = model.RochlitzCarousselContainer.Name,
                FileNameList = _fileService.GetBlobsFromContainer(model.RochlitzCarousselContainer.Name)
            });
            return View("_burgstädtCaroussel", model);
        }

    }

}
