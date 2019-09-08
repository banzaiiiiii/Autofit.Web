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
		public List<IListBlobItem> FileNameList { get; set; } = new List<IListBlobItem>();

	}

	public class FilesViewModel
	{
		public List<FileDetails> Files { get; set; }
			= new List<FileDetails>();

		public List<AzureContainerDetails> Container { get; set; }
			= new List<AzureContainerDetails>();
	}
}
