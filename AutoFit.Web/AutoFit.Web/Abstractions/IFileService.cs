using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;


namespace AutoFit.Web.Abstractions
{
    public interface IFileService
    {
        Task UploadFileAsync(Byte[] byteArray, string userEmail, string containerName);
        Task<Stream> DownloadToStream(string fileName, string containerName);
        Task DeleteAsync(string containerName, string fileName);
        Task DeleteContainerAsync(string containerName);
        IEnumerable<CloudBlockBlob> GetBlobsFromContainer(string containername);

        CloudBlockBlob GetThumbnailFromContainer(string containername);

        IEnumerable<CloudBlobContainer> ListContainersAsync();
        Task CreateFolder(string containername);
        void SetMetaBlobMetaData(string fileName, string containerName, string itemName, string preis);
        void SetContainerMetaData(string containerName, string discription, string price);

        CloudBlobContainer ResolveCloudBlobContainer(string containerName);
    }
}
