using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NorthwindApiDemo.EFModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace NorthwindApiDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// <summary>
        ///  Para llamar los servicios utilizaremos este metodo para llamar al metodo MVC
        /// </summary>
        /// <param name="services"></param>
        /// services.AddMvc();  llamamos los ercicios amc
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // sin tocar configuracion en EFModels\NorthwindContext.cs
            //services.AddDbContext<NorthwindContext>(options =>
            services.AddDbContext<NorthwindContext>(options =>
            {
                options.UseSqlServer("Server =.\\SQLEXPRESS;DataBase= Northwind;Trusted_Connection=True");
            });

            //s03c25
            //addTransient (crear servicios cada vez solicitados)
            //addScope //crear servicios 1 vez por solicitud
            //addSingleton //crear servicio 1 solicitado
            services.AddScoped<Services.ICustomerRepository, Services.CustomerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configuramos entorno y como sale
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env">Aqui llamanos al entorno con el qual trabajamos</param>
        ///  if (env.IsDevelopment()) aqui chequeamos si el entorno es de desarollo
        ///  else{ app.UseExceptionHandler(); }si el evento del entono no esta declarado
        ///  app.UseStatusCodePages();  middleware (tuberia codigo pagina) muy funiconal para errores
        ///  app.UseMvc();  utilizar el midware MVC
        ///  
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseStatusCodePages();

            //S02C27 AUTOMAPEO

            #region mapeo

            //mappeo odea linko Customer con su modelo
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<Customers, Models.CustomerWithoutOrdersDTO>();
                //S03C28 Mapeo de cliente con o sin ordenes
                config.CreateMap<Customers, Models.CustomersDTO>();
                config.CreateMap<Orders, Models.OrdersDTO>();
                //S03C30 Crear registro
                config.CreateMap<Models.OrdersForCreationDTO,Orders>();
                //S03C1 uPDATE REGISTRO
                config.CreateMap<Models.OrdersForUpdateDTO, Orders>();

            });
            
           

            #endregion





            app.UseMvc();




            /*
            app.UseMvc(config =>
                {
                config.MapRoute(
                    name: "Default",
                    template: "{contoller}/{action}/{id?}",
                    defaults: new
                    {
                        controller = "Home",
                        action ="Index"
                    });

                });
                */
            ///Aqui es donde se configurala accion que sucedera:
            ///lanzamos excepcion personalizada          
            //app.Run(async (context) =>
            // {
            //     throw new Exception("testeando excepciones");
            //});

            ///Aqui es donde se configurala accion que sucedera:
            ///en este caso sino hay valor devuelve esta respuesta escrita      
            /*
             app.Run(async (context) =>
             {
                 await context.Response.WriteAsync("Hello World!");
             });
             */
        }
    }
}
