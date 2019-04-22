using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Infrastructure;
using WebApp.Models;

namespace WebApp.Components
{
    public class CartStatusGridView : ViewComponent
    {
        private readonly FuelCartDBContext _context;
        private readonly IOpenAutomationSoftware _oas;

        public CartStatusGridView(FuelCartDBContext context, IOpenAutomationSoftware oas)
        {
            _context = context;
            _oas = oas;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //_oas.LoadCartStatus();
            return await Task.FromResult((IViewComponentResult)View("Default"));
        }
    }
}
