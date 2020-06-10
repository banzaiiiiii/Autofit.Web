using AutoFit.Web.ViewModels.Shop;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.Abstractions
{
    public interface IShopService
    {
        Task<Payment> CreatePayment(ShoppingCartModel shoppingCart);
        Task<Payment> ExecutePayment(string payerID, string paymentID);
    }
}
