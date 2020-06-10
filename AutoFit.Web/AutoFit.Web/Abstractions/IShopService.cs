using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.Abstractions
{
    public interface IShopService
    {
        Task<Payment> CreatePayment();
        Task<Payment> ExecutePayment(string payerID, string paymentID);
    }
}
