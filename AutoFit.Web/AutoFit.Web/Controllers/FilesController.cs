using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;
using AutoFit.Web.ViewModels.Files;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class FilesController : BaseController
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var containerList = await _fileService.ListContainersAsync();

            var model = new FilesViewModel();
            foreach (var container in containerList)
            {
                model.Container.Add(
                                    new AzureContainerDetails()
                                    {
                                        ContainerName = container.Name,
                                        FileNameList =  _fileService.GetBlobsFromContainer(container.Name)

                                    });

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, string containerName)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var fileName = file.FileName;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                var byteArray = stream.ToArray();
                await _fileService.UploadFileAsync(byteArray, fileName, containerName);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files, string containerName)
        {
            if (files == null || files.Count == 0)
                return Content("files not selected");

            foreach (var file in files)
            {
                var fileName = file.FileName;

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    var byteArray = stream.ToArray();
                    await _fileService.UploadFileAsync(byteArray, fileName, containerName);
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Download(string filename, string containerName)
        {
            if (filename == null)
                return Content("filename not found");

            var fileStream = await _fileService.DownloadToStream(filename, containerName);

            var memory = new MemoryStream();

            await fileStream.CopyToAsync(memory); memory.Position = 0;
            return File(memory, "application/octet-stream", filename);


        }
        public IActionResult AddMetaData(string filename, string containerName, string itemName, string preis)
        {
            try
            {
                _fileService.SetMetaBlobMetaData(filename, containerName, itemName, preis);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "wrong format for metadata");
                return RedirectToAction("Index");
            }
           
        }

        public async Task<IActionResult> CreateContainer(string containerName)
        {
            await _fileService.CreateFolder(containerName);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteContainer(string containerName)
        {
            await _fileService.DeleteContainerAsync(containerName);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFile(string containerName, string fileName)
        {
            await _fileService.DeleteAsync(containerName, fileName);

            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> GetUrls(string containerName)
        //{
        // var list = await _fileService.GetBlobsFromContainer(containerName);
        // foreach (var item in list)
        // {
        //  item.Uri
        // }
        //}
    }
}