using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class CartStatus
    {
        public int CartId { get; set; }
        public string CartName { get; set; }
        public string OASGroup { get; set; }
        public string Status { get; set; }
        public decimal NetVolume { get; set; }
        public decimal GrossVolume { get; set; }
    }
}
