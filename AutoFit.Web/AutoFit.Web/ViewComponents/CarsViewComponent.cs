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
            var containerList = await _fileService.ListContainersAsync();

            var model = new FilesViewModel();
            foreach (var container in containerList)
            {
                model.Container.Add(
                                    new AzureContainerDetails()
                                    {
                                        ContainerName = container.Name,
                                        FileNameList = _fileService.GetBlobsFromContainer(container.Name)
                                    });


            }
            return View("_cars", model);
        }
    }
}
