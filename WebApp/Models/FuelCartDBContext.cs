using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class FuelCartDBContext : DbContext
    {
        public FuelCartDBContext(DbContextOptions<FuelCartDBContext> options) : base(options)
        {

        }

        public DbSet<FuelCartModel> FuelCarts { get; set;}
        public DbSet<RegisterHead> RegisterHead { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}
