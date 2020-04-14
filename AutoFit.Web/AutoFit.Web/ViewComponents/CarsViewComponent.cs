using AutoFit.Web.Abstractions;
using AutoFit.Web.ViewModels.Files;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.ViewComponents
{
    public class CarsViewComponent : ViewComponent
    {
        private readonly IFileService _fileService;

        public CarsViewComponent(IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
           

            var model = new FilesViewModel();
            model.ContainerList =  _fileService.ListContainersAsync();
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
            return View("_cars", model);
        }
    }
}
