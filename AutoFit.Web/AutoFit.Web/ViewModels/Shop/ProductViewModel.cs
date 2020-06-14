using AutoFit.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.ViewModels.Shop
{
    public class ProductViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
