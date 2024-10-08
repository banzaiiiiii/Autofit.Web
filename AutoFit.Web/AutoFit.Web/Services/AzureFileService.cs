﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AutoFit.Web.Abstractions;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


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
            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
        }

        public IEnumerable<CloudBlockBlob> GetBlobsFromContainer(string containername)
        {
            BlobContinuationToken blobContinuationToken = null;

            var containerName = ResolveCloudBlobContainer(containername);
            BlobResultSegment response = null;
            
                do
                {
                    try
                    {
                         response = containerName.ListBlobsSegmented(blobContinuationToken);
                    }
                    catch (Exception ex)
                    {

                    _logger.LogDebug(ex, "no container found");
                    yield break;
                    }
                    
                    
                    blobContinuationToken = response.ContinuationToken;
                    foreach (var blob in response.Results.OfType<CloudBlockBlob>())
                    {
                        blob.FetchAttributes();
                        yield return blob;
                    }
                } while (blobContinuationToken != null);
            
        }

        public CloudBlockBlob GetThumbnailFromContainer (string containername)
        {
            var containerName = ResolveCloudBlobContainer(containername);


            var blob = containerName.GetBlockBlobReference("thumbnail.jpg");
            return blob;
        }
        public IEnumerable<CloudBlobContainer> ListContainersAsync()
        {
            BlobContinuationToken continuationToken = null;

            do
            {
                var storageAccount = GetCloudStorageAccount();
                var blobClient = storageAccount.CreateCloudBlobClient();

                var response = blobClient.ListContainersSegmented(continuationToken);
                continuationToken = response.ContinuationToken;
                //results.AddRange(response.Results);
                foreach (var container in response.Results.OfType<CloudBlobContainer>())
                {
                    container.FetchAttributes();
                    yield return container;
                }
            }
            while (continuationToken != null);

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
                //blockBlob.Metadata.Add("Name", "null");
                //blockBlob.Metadata.Add("Preis", "null");
                //blockBlob.SetMetadata();
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

        public void SetMetaBlobMetaData(string fileName, string containerName, string itemName, string preis)
        {
            var cloudBlockBlob = ResolveCloudBlockBlob(containerName, fileName);

            cloudBlockBlob.Metadata["LastUpdated"] = DateTime.Now.ToString();
            if (fileName != null)
            {
                cloudBlockBlob.Metadata.Remove("Name");
                cloudBlockBlob.Metadata.Add("Name", itemName);
            }
            if (preis != null)
            {
                cloudBlockBlob.Metadata.Remove("Preis");
                cloudBlockBlob.Metadata.Add("Preis", preis);

            }

            cloudBlockBlob.SetMetadata();
        }

        public void SetContainerMetaData(string containerName, string discription, string price)
        {
            var container = ResolveCloudBlobContainer(containerName);

            container.Metadata["LastUpdated"] = DateTime.Now.ToString();
            if (containerName != null)
            {
                container.Metadata.Remove("Name");
                container.Metadata.Add("Name", discription);
            }
            if (price != null)
            {
                container.Metadata.Remove("Preis");
                container.Metadata.Add("Preis", price);

            }

            container.SetMetadata();
        }



    }
}
