using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace NorthwindApiDemo
{
    public class Program
    {

        /// Build() :  contruir app
        /// Run() : correr app
        public static void Main(string[] args)
        {
           
            CreateWebHostBuilder(args).Build().Run();
        }

        ///WebHost.xxx  podemos configurarlo como queramos. Este es el default que hay de base
        ///Startup classe de configuracion de diferentes servicios que crearemos se  llama asi por defecto
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
