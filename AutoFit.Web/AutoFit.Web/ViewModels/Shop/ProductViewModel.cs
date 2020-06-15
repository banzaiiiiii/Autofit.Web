using AutoFit.Web.Data;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.ViewModels.Shop
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public IEnumerable<CloudBlockBlob> ProductImages { get; set; }
    }
   
}
