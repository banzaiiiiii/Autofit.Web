using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;
using AutoFit.Web.ViewModels.Files;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace AutoFit.Web.Controllers
{
    public class FilesController : Controller
    {
	    private readonly IFileService _fileService;

		public FilesController(IFileService fileService)
		{
			_fileService = fileService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UploadFile(IFormFile file)
		{
			if (file == null || file.Length == 0)
				return Content("file not selected");

			var fileName = file.FileName;

			using (var stream = new MemoryStream())
			{
				await file.CopyToAsync(stream);
				var byteArray = stream.ToArray();
				await _fileService.UploadFileAsync(byteArray, fileName, "bild");
			}

			return RedirectToAction("Files");
		}

		[HttpPost]
		public async Task<IActionResult> UploadFiles(List<IFormFile> files)
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
					await _fileService.UploadFileAsync(byteArray, fileName, "bild");
				}
			}

			return RedirectToAction("Files");
		}

		public async Task<IActionResult> Files()
		{
			var containerList = await _fileService.ListContainersAsync();
			var blobList = await _fileService.GetBlobsFromContainer("bild");
			
			var model = new FilesViewModel();
			foreach (var item in blobList)
			{
				
				var blobName = Path.GetFileName(item.Uri.ToString());
				var blobSize = _fileService.ResolveCloudBlockBlob("bilder", blobName).Properties.Length;
				
				model.Files.Add(
					new FileDetails
					{
						Name = blobName,
						Size = blobSize
					});
			}
			foreach (var item in containerList)
			{
				model.Container.Add(
				                new AzureContainerDetails() { Name = item.Name });
			}
			return View(model);
		}

		public async Task<IActionResult> Download(string filename)
		{
			if (filename == null)
				return Content("filename not found");

			var fileStream = await _fileService.DownloadToStream(filename, "bild");

			using (var memory = new MemoryStream())
			{
				await fileStream.CopyToAsync(memory);memory.Position = 0;
				return File(memory, "application/octet-stream", filename);
			}

		}

	}
}