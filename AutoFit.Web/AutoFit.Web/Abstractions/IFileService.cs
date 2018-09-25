using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage.Blob;

namespace AutoFit.Web.Abstractions
{
	public interface IFileService
	{
		Task UploadFileAsync(FileStream fileStream, string userEmail, string documentType);
		Task<Stream> DownloadToStream(string fileName, string documentType);
		Task DeleteAsync(string documentType, string fileName);
		Task<List<IListBlobItem>> GetBlobsFromContainer(string documentType);
		Task<List<CloudBlobContainer>> ListContainersAsync();
		CloudBlockBlob ResolveCloudBlockBlob(string containerName, string fileName);
	}
}
