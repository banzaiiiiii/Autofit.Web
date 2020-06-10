using AutoFit.Web.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoFit.Web.Services
{
    public class PayPalService : BaseService, IShopService
    {
        private readonly Dictionary<string, string> _payPalConfig;

        public PayPalService(IConfiguration configuration, ILoggerFactory loggerFactory) : base(loggerFactory)
        {

            _payPalConfig = new Dictionary<string, string>()
    {
        { "clientId" , configuration.GetSection("paypal:settings:clientId").Value },
        { "clientSecret", configuration.GetSection("paypal:settings:clientSecret").Value },
        { "mode", configuration.GetSection("paypal:settings:mode").Value },
        { "business", configuration.GetSection("paypal:settings:business").Value },
        { "merchantId", configuration.GetSection("paypal:settings:merchantId").Value },
    };

        }

        public async Task<Payment> CreatePayment()
        {
            var createdPayment = new Payment();

            var apiContext = new APIContext(new OAuthTokenCredential(_payPalConfig).GetAccessToken())
            { 
                Config = _payPalConfig
            };

            try
            {
                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    // is list because buyer can buy more than one product
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            amount = new Amount
                            {
                                currency = "EUR",
                                total = "100"
                            },
                            description = "Test product"
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        cancel_url = "http://localhost:5001/Shop/Cancel",
                        return_url = "http://localhost:5001/Shop/Success"
                    }
                };
                 createdPayment = await Task.Run(() => payment.Create(apiContext));
                _logger.LogInformation($"payment created successfully '{createdPayment}'");
            }
            catch (Exception exception)
            {

                _logger.LogError($"an error ocuccured creating an invoice '{createdPayment}'", exception);
            }
            return createdPayment;
        }

        public async Task<Payment> ExecutePayment(string payerID, string paymentID)
        {
            var apiContext = new APIContext(new OAuthTokenCredential(_payPalConfig).GetAccessToken())
            {
                Config = _payPalConfig
            };

            var paymentExecution = new PaymentExecution() { payer_id = payerID };
            var payment = new Payment() { id = paymentID };
            var executedPayment = await Task.Run(() => payment.Execute(apiContext, paymentExecution));

            return executedPayment;
        }


    }
}
