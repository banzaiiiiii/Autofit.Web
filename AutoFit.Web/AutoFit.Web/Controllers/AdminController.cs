using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;
using AutoFit.Web.ViewModels.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly IFileService _fileService;

        public AdminController(IFileService fileService, ILoggerFactory loggerFactory) : base(loggerFactory)
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
                                        ContainerName = container.Name,
                                        FileNameList = _fileService.GetBlobsFromContainer(container.Name)

                                    }); ;

            }
            if (TempData["errorMessage"] != null)
            {
                ViewBag.Message = TempData["errorMessage"].ToString();
                TempData.Remove("errorMessage");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadThumbnail(IFormFile file, string containerName)
        {

            if (file == null || file.Length == 0)
                return Content("file not selected");

            await CreateContainer(containerName);
            var fileName = "thumbnail.jpg";
                //file.FileName;

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
                _fileService.SetContainerMetaData(containerName, itemName, preis);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "wrong format for metadata");
                TempData["errorMessage"] = "Beschreibung konnte nicht hinzugefügt werden. Falsches Format!";
                return RedirectToAction("Index");
            }

        }

        public async Task<IActionResult> CreateContainer(string containerName)
        {
            try
            {
                await _fileService.CreateFolder(containerName);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "could not create folder");
                TempData["errorMessage"] = "!!nur Kleinbuchstaben und Zahlen erlaubt!!";
                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteContainer(string containerName)
        {
            await _fileService.DeleteContainerAsync(containerName);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteFile(string containerName, string fileName)
        {
            try
            {
                await _fileService.DeleteAsync(containerName, fileName);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "file does not exist");
            }
           

            return RedirectToAction("Index");
        }

        public IActionResult Shop()
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

        public async Task<IActionResult> UploadShopImages(List<IFormFile> files, string containerName)
        {

            await CreateContainer(containerName);
            await UploadFiles(files, containerName);

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