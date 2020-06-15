using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.ViewModels.Shop
{
    public class AdminProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public IEnumerable<AdminProductViewModel> Products { get; set; }
    }
}
