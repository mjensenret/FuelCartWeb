using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class Transactions
    {
        [Key]
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int OrderId { get; set; }
        public int SequenceId { get; set; }
        public int OperatorId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public double IVVolume { get; set; }
        public double GVVolume { get; set; }
        public double GSTVolume { get; set; }
        public double GSVVolume { get; set; }
        public double IVTotalizer { get; set; }
        public double GVTotalizer { get; set; }
        public double GSTTotalizer { get; set; }
        public double GSVTotalizer { get; set; }
        public double AvgMeterFactor { get; set; }
        public double AvgTemperature { get; set; }
        public double AvgPressure { get; set; }
        public double AvgDensity { get; set; }
        public double CTL { get; set; }
        public double CPL { get; set; }
        public int CartId { get; set; }
        public virtual FuelCartModel FuelCart { get; set; }
    }

}
