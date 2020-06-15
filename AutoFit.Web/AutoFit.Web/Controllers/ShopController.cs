﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFit.Web.Abstractions;
using AutoFit.Web.Data;
using AutoFit.Web.Data.Abstractions;
using AutoFit.Web.ViewModels.Files;
using AutoFit.Web.ViewModels.Shop;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IFileService _fileService;
        //sollte besser bezahlservice heißen
        private readonly IShopService _shopService;
        private readonly IProduct _productService;
        private readonly IMapper _mapper;

        public ShopController(IFileService fileService, IProduct productService, IShopService shopService, IMapper mapper, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _fileService = fileService;
            _shopService = shopService;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProducts();
            var model = new ProductViewModel
            {
                Products = products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Value = x.Value,
                ProductImages = _fileService.GetBlobsFromContainer($"shopitem{x.Id}")
            })
            };
            return View(model);
        }

        //this is shitty as fuck 
        public IActionResult Kaufabwicklung(string productName, string productPrice)
        {
            var shoppingCart = new ShoppingCartModel
            {
                ProduktName = productName,
                Currency = "EUR",
                Value = productPrice
            };
            return View(shoppingCart);
        }


        [HttpGet]
        [Route("/api/create")]
        public async Task<IActionResult> CreatePayment(string productName, string productPrice)
        {
            _logger.LogInformation($"Creating payment against paypal api");

            // create a payment for a virtuell product, that is created in service method atm
            var shoppingCart = new ShoppingCartModel
            {
                ProduktName = productName,
                Currency = "EUR",
                Value = productPrice
            };
            var result = await _shopService.CreatePayment(shoppingCart);


            foreach (var link in result.links)
            {
                if (link.rel.Equals("approval_url"))
                {
                    _logger.LogInformation($"Found approvel url {link.href} from response");
                    return Redirect(link.href);
                }
            }
            return NotFound();
        }

        [HttpGet]
        [Route("Shop/Success")]
        public async Task<IActionResult> ExecutePayment(string paymentID, string token, string PayerID)
        {
            _logger.LogInformation($"Executing payment against paypal api");

            // create a payment for a virtuell product, that is created in service method atm
            var result = await _shopService.ExecutePayment(PayerID, paymentID);

            _logger.LogInformation($"new result from paypal api: '{result}'");

            var paymentDetails = result.ToString();
            ViewBag.testing = paymentDetails;
            return View("Success");

        }

        [HttpGet]
        [Route("cancel")]
        public IActionResult CancelPayment()
        {
            return NotFound("Ops something went wrong with your paypal payment");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(string name, string description, string value)
        {
            if (ModelState.IsValid)
            {
                await _productService.Add(name, description, value);
            }
            return View();
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            // if id is not 0, delete product
            if (!id.Equals(0))
            {
                await _productService.Delete(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}