using AutoFit.Web.Data;
using AutoFit.Web.ViewModels.Shop;
using AutoMapper;
using System.Collections.Generic;


namespace AutoFit.Web.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<IEnumerable<Product>, ProductViewModel>();
               
        }
    }
}
