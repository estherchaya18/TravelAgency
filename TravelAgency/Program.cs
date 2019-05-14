using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TravelAgency
{
    public class Program
    {
        public static void Main(string[] args)
        {
               var host = BuildWebHost(args).Run();
           // var BuildWebHost(args).Run();
            // host.Run();
           // using (var scope = host.Services.CreateScope())
            //{
              //  var services = scope.ServiceProvider;

                //DatabaseContext context = services.GetRequiredService<DatabaseContext>();

                //if(context.Phone.Count() == 0 )
                //{
                  //  (context.Add(new Phone() {Id = "aaa" , Name = "Iphone"}));
                    //context.SaveChanges();
                //}
               */ 

            }


          
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
