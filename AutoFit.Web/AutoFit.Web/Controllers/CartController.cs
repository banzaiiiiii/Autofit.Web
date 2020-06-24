using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFit.Web.Data;
using AutoFit.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AutoFit.Web.Controllers
{
    public class CartController : Controller
    {
        
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        { 
           var model = _cartService.GetCart(HttpContext.Session);
            return View("cart", model);
        }

        public IActionResult AddToCart(string productName)
        {
            
            _cartService.AddProductToCart(productName, HttpContext.Session);

            return RedirectToAction(nameof(Index));
        }
    }
}