using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using WebApp.Hubs;
using WebApp.Infrastructure;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //private IHubContext<FuelCartHub> _fcHubContext;
        private IOpenAutomationSoftware _oasInterface;
        private FuelCartDBContext _context;
        readonly IConfiguration _configuration;

        public HomeController(IHubContext<FuelCartHub> hubContext, IOpenAutomationSoftware oasInterface, FuelCartDBContext context, IConfiguration configuration)
        {
            //_fcHubContext = hubContext;
            _oasInterface = oasInterface;
            _context = context;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //var fcGroups = _context.FuelCarts.Select(x => x.OASGroupName).ToList();
            //var networkNode = _configuration.GetValue<string>("OASServer");

            return View();
        }

        [AllowAnonymous]
        [HttpPost("LoadCartStatus", Name ="LoadCartStatus")]
        [ProducesResponseType(typeof(CartStatus), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult LoadCartStatus()
        {
            //var cartList = _context.FuelCarts.ToList();

            //var cartModel = cartList
            //    .Select(x => new
            //    {
            //        x.CartId,
            //        x.CartName,
            //        x.OASGroupName
            //    })
            //    .ToList()
            //    .Select(y => new CartStatus()
            //    {
            //        CartId = y.CartId,
            //        CartName = y.CartName,
            //        OASGroup = y.OASGroupName,
            //        Status = "Initializing..."
            //    })
            //    .ToList();

            //if (cartModel == null)
            //    return NotFound();
            _oasInterface.LoadCartStatus();
            return RedirectToAction("Index");
        }

        public void LoadCarts()
        {
            _oasInterface.LoadCartStatus();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
