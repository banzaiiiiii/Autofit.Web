using AutoFit.Web.Data;
using AutoFit.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.Services
{
    public class CartService
    {
        //private readonly ISession _session;
        private readonly WebsiteDbContext _dbContext;

        public CartService(/*ISession session,*/ WebsiteDbContext dbContext)
        {
            //_session = session;

            _dbContext = dbContext;
        }


        public void AddProductToCart(string name, ISession session)
        {

            var cartList = new List<CartProduct>();

            var stringObject = session.GetString("cart");

            if (!string.IsNullOrEmpty(stringObject))
            {
                cartList = JsonConvert.DeserializeObject<List<CartProduct>>(stringObject);
            }

            if (cartList.Any(x => x.Name == name))
            {

            }
            else
            {
                cartList.Add(new CartProduct
                {
                    Name = name
                });
            }

            stringObject = JsonConvert.SerializeObject(cartList);

            session.SetString("cart", stringObject);
        }

        public IEnumerable<CartViewModel> GetCart(ISession session)
        {

            var stringObject = session.GetString("cart");
            if (string.IsNullOrEmpty(stringObject))
            {
                return new List<CartViewModel>();
            }

            var cartList = JsonConvert.DeserializeObject <List<CartProduct>>(stringObject);

            // suche anhand product name, vielleicht etwas unsicher
            var model = _dbContext.Products.Where(x => cartList.Any(y => y.Name == x.Name))
                .Select(x => new CartViewModel
            {
                Name = x.Name,
                Value = x.Value
            }).ToList();

            return model;
        }

    }
}
