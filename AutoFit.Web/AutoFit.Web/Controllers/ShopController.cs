using System.Threading.Tasks;
using AutoFit.Web.Abstractions;
using AutoFit.Web.ViewModels.Files;
using AutoFit.Web.ViewModels.Shop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFit.Web.Controllers
{
    public class ShopController : BaseController
    {
        private readonly IFileService _fileService;
        private readonly IShopService _shopService;
        public ShopController(IFileService fileService, IShopService shopService, ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            _fileService = fileService;
            _shopService = shopService;
        }

        public IActionResult Index()
        {
            var model = new FilesViewModel();
            model.ContainerList = _fileService.ListContainersAsync();
            foreach (var container in model.ContainerList)
            {
                model.ContainerDetailsList.Add(
                                    new AzureContainerDetails()
                                    {
                                        ContainerMetadata = container.Metadata,
                                        ContainerName = container.Name,
                                        FileNameList = _fileService.GetBlobsFromContainer(container.Name)
                                    });
            }

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
        public async Task<IActionResult> CancelPayment()
        {
            return NotFound("Ops something went wrong with your paypal payment");
        }
    }
}