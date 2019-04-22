using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class FuelCartModel
    {
        [Key]
        public int CartId { get; set; }
        public string CartName { get; set; }
        public string OASGroupName { get; set; }
        public virtual RegisterHead HeadType { get; set; }
        public int HeadTypeId { get; set; }
        public ICollection<Transactions> Transactions { get; set;}
    }

    public class RegisterHead
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
