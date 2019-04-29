using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Infrastructure
{
    public interface IOpenAutomationSoftware
    {
        List<CartStatusViewModel> LoadCartStatus();
    }
}
