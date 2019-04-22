using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class DbInitializer
    {
        public static void Initialize(FuelCartDBContext context)
        {
            context.Database.EnsureCreated();

            if(context.RegisterHead.Any())
            {
                return;
            }

            var registerHeads = new RegisterHead[]
            {
                new RegisterHead{Type = "MicroLoad"},
                new RegisterHead{Type = "AccuLoad" },
                new RegisterHead{Type = "TopTech"}
            };
            foreach (RegisterHead r in registerHeads)
            {
                context.RegisterHead.Add(r);
            }
            context.SaveChanges();            
        }
    }
}
