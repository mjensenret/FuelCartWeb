using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Infrastructure;

namespace WebApp.Hubs
{
    public class FuelCartHub : Hub
    {
        private IOpenAutomationSoftware _oas;

        public FuelCartHub()
        {
            
        }
    }
}
