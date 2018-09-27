using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AutoFit.Web.Services
{

	public class AzureFileService : BaseService, IFileService
	{
		private readonly IConfiguration _configuration;

		public AzureFileService(IConfiguration configuration, ILoggerFactory loggerFactory) : base(loggerFactory)
		{
			_configuration = configuration;
		}


		public async Task<List<IListBlobItem>> GetBlobsFromContainer(string documentType)
		{

			List<IListBlobItem> blobs = new List<IListBlobItem>();

			BlobContinuationToken blobContinuationToken = null;

			var containerName = ResolveCloudBlobContainer(SetContainerName(documentType));

			do
			{
				var response = await containerName.ListBlobsSegmentedAsync(blobContinuationToken);
				blobContinuationToken = response.ContinuationToken;
				blobs.AddRange(response.Results);

			} while (blobContinuationToken != null);

			return blobs;

		}

		public async Task<List<CloudBlobContainer>> ListContainersAsync()
		{
			BlobContinuationToken continuationToken = null;
			List<CloudBlobContainer> results = new List<CloudBlobContainer>();
			do
			{
				var storageAccount = GetCloudStorageAccount();
				var blobClient = storageAccount.CreateCloudBlobClient();

				var response = await blobClient.ListContainersSegmentedAsync(continuationToken);
				continuationToken = response.ContinuationToken;
				results.AddRange(response.Results);
			}
			while (continuationToken != null);
			return results;
		}

		public async Task DeleteAsync(string documentType, string fileName)
		{
			var blockBlob = ResolveCloudBlockBlob(SetContainerName(documentType), fileName);
			await blockBlob.DeleteAsync();
		}


		public async Task<Stream> DownloadToStream(string fileName, string documentType)
		{
			var cloudBlockBlob = ResolveCloudBlockBlob(SetContainerName(documentType), fileName);
			var stream = await cloudBlockBlob.OpenReadAsync();

			return stream;
		}

		public async Task UploadFileAsync(Byte[] byteArray, string fileName, string contentType)
		{
			try
			{
				var blockBlob = ResolveCloudBlockBlob(SetContainerName(contentType), fileName);

				await blockBlob.UploadFromByteArrayAsync(byteArray, 0, byteArray.Length);
			}
			catch (Exception e)
			{
				_logger.LogError("Exception while uploading from Azure Service: ", e);
			}
		}

		public CloudBlockBlob ResolveCloudBlockBlob(string containerName, string fileName)
		{
			var container = ResolveCloudBlobContainer(containerName);
			var blockBlob = container.GetBlockBlobReference(fileName);
			blockBlob.FetchAttributesAsync();
			return blockBlob;
		}

		public CloudBlobContainer ResolveCloudBlobContainer(string containerName)
		{
			var storageAccount = GetCloudStorageAccount();
			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(containerName);
			return container;
		}

		private CloudStorageAccount GetCloudStorageAccount()
		{
			return CloudStorageAccount.Parse(ResolveAzureStorageConnectionString());
		}

		public string ResolveAzureStorageConnectionString()
		{
			var storageConnectionString = _configuration.GetSection("FileManagement").GetSection("StorageAccount").Value;

			return storageConnectionString;
		}

		public string SetContainerName(string documentType)
		{
			string containerName = null;
			switch (documentType)
			{
				case "bild":
					containerName = "bilder";
					break;
				case "fuehrerschein":
					containerName = "fuehrerscheine";
					break;
				default:
					_logger.LogError($"Dokumententyp {documentType} not found");
					throw new Exception($"{documentType} not found");
			}

			return containerName;
		}
	}
}
