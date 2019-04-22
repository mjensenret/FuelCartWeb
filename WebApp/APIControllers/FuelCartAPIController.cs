using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.APIControllers
{
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class FuelCartAPIController : Controller
    {
        private readonly FuelCartDBContext _context;
        private readonly OASConfig.Config oasConfig = new OASConfig.Config();

        public FuelCartAPIController(FuelCartDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public object GetFuelCarts(DataSourceLoadOptions loadOptions)
        {
            var cartList = _context.FuelCarts;
            return DataSourceLoader.Load(cartList,loadOptions);
        }

        [HttpGet]
        public object GetHeadTypes(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_context.RegisterHead.ToList(), loadOptions);
        }

        [HttpPost]
        public IActionResult AddCart(string values)
        {

            var newCart = new FuelCartModel();
            JsonConvert.PopulateObject(values, newCart);

            if (!TryValidateModel(newCart))
                return BadRequest(ModelState.GetFullErrorMessage());

            _context.FuelCarts.Add(newCart);
            _context.SaveChanges();
            return Ok(newCart);
        }

        [HttpPut]
        public IActionResult UpdateCart(int key, string values)
        {
            var cart = _context.FuelCarts.First(x => x.CartId == key);
            JsonConvert.PopulateObject(values, cart);

            if (!TryValidateModel(cart))
                return BadRequest(ModelState.GetFullErrorMessage());
            _context.SaveChanges();

            return Ok(cart);
        }

        [HttpDelete]
        public void DeleteCart(int key)
        {
            var cart = _context.FuelCarts.First(x => x.CartId == key);
            _context.FuelCarts.Remove(cart);
            _context.SaveChanges();
        }
    }
}