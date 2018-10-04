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

		public async Task CreateFolder(string containername)
		{
			var storageAccount = GetCloudStorageAccount();
			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(containername);
			await container.CreateAsync();
		}

		

		public async Task<List<IListBlobItem>> GetBlobsFromContainer(string containername)
		{

			List<IListBlobItem> blobs = new List<IListBlobItem>();

			BlobContinuationToken blobContinuationToken = null;

			var containerName = ResolveCloudBlobContainer(containername);

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

		public async Task DeleteAsync(string containerName, string fileName)
		{
			var blockBlob = ResolveCloudBlockBlob(containerName, fileName);
			await blockBlob.DeleteAsync();
		}

		public async Task DeleteContainerAsync(string containerName)
		{
			var container = ResolveCloudBlobContainer(containerName);
			await container.DeleteAsync();
		}

		public async Task<Stream> DownloadToStream(string fileName, string containerName)
		{
			var cloudBlockBlob = ResolveCloudBlockBlob(containerName, fileName);
			var stream = await cloudBlockBlob.OpenReadAsync();

			return stream;
		}

		public async Task UploadFileAsync(Byte[] byteArray, string fileName, string containerName)
		{
			try
			{
				var blockBlob = ResolveCloudBlockBlob(containerName, fileName);

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

		
	}
}
