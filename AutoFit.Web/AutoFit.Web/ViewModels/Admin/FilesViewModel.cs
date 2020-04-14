using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Azure.Storage.Blob;

namespace AutoFit.Web.ViewModels.Files
{
	public class FileDetails
	{
		public string Name { get; set; }
		public long Size { get; set; }
	}

	public class AzureContainerDetails
	{
        public string ContainerName { get; set; }
		public IDictionary<string, string> ContainerMetadata { get; set; }
		public IEnumerable<CloudBlockBlob> FileNameList { get; set; } = new List<CloudBlockBlob>();
    }

	public class FilesViewModel
	{
        public IEnumerable<CloudBlobContainer> ContainerList { get; set; } = new List<CloudBlobContainer>();

        public CloudBlobContainer RochlitzCarousselContainer { get; set; }
        public CloudBlobContainer BurgstädtCarousselContainer { get; set; }


        public List<FileDetails> Files { get; set; }
			= new List<FileDetails>();

		public List<AzureContainerDetails> ContainerDetailsList { get; set; }
			= new List<AzureContainerDetails>();
	}

   
   
}
