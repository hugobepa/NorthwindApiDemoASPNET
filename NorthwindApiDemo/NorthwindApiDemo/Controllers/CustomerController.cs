using Microsoft.AspNetCore.Mvc;
using NorthwindApiDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApiDemo.Controllers
{
    //como rutaDefault
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        //S03C29 crear variable parallma

        //S03C26 
        //using NorthwindApiDemo.Services;
        private ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

    
        //[HttpGet("prueba")]
        //Ruta especifica
        //[HttpGet("api/customers")]
        //como rutaDefault
        [HttpGet()]
        public JsonResult GetCustomers()
        {

            #region ApartirS03C26orUsode ServiciosRepositorios
            //metodo guarro o resultado directo
            var customers = _customerRepository.GetCustomers();
            //return new JsonResult(customers);
            //metodo elegante

            #endregion

            //S03C26 amadir directamente a BBDD

            #region S03C26 amadir directamente a BBDD
            /*
                 * var results = new List<Models.CustomerWithoutOrdersDTO>();
                foreach (var customer in customers)
                {
                    results.Add(new Models.CustomerWithoutOrdersDTO()
                    {
                    
                         CustomerID  = customer.CustomerId,
                         CompanyName  = customer.CompanyName,
                         ContactName  = customer.ContactName,
                         ContactTitle  = customer.ContactTitle,
                         Address  = customer.Address,
                         City  = customer.City,
                         Region  = customer.Region,
                         PostalCode  = customer.PostalCode,
                         Country  = customer.Country,
                         Phone  = customer.Phone,
                         Fax  = customer.Fax             
                    });
                }
             */



            #endregion
            //S03C27AUTOMAPEO
            var results = AutoMapper.Mapper.Map<IEnumerable<Models.CustomerWithoutOrdersDTO>>(customers);


            #region previously S03c26

            //C14 llamamos a una lista creada en un repositorio Repository mediante Hydrator
            //Descomentar linea  inferior solamente sino se hace servir Servicios de repositorio hasta C03S26
            //return new JsonResult(Repository.Instance.Customers);

            //ejemplo sencillo de lista C12 C13
            /*
            return new JsonResult(new List<object>()
            { 
                new {CustomerID = 1, ContactName = "Anderson"},
                new {CustomerID = 2, ContactName = "Solaris"}
             });
             */

            #endregion
            return new JsonResult(results);
        }


        #region IActionResult GetCustomer >C28
        /*
        //C15 creamos un elemento para llamar a un valor especifico 
        //IActionResult cambiamos por JsonResult porque es mas generico y hereda de el JsonResult
        //evitandonos problemas como el NotFound
        //sino encontramos nadassugue este metodo
        //[HttpGet("{id})" el obetenido sera el que valor url (http://localhost/api/customers/3)
        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {

            var result = Repository.Instance.Customers.FirstOrDefault(c => c.ID == id);
           

            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        */
        #endregion
        #region IActionResult GetCustomer =>C28 con BBDD


        //S03C28 trabajando desde BBDD, verificamos si quiere orden de trabajo
        [HttpGet("{id}")]
        public IActionResult GetCustomer(string id,bool includeOrders = false)
        {


            var customer = _customerRepository.GetCustomers(id, includeOrders);


            if (customer == null)
            {
                return NotFound();
            }
            //S03C28 Pasar cliente con ordenes directamente BBDD
            if (includeOrders)
            {
                var customerResult = AutoMapper.Mapper.Map<Models.CustomersDTO>(customer);
                return Ok(customerResult);
            }

            //S03C28 Pasar cliente sin ordenes  BBDD
            var customerResultOnly = AutoMapper.Mapper.Map<Models.CustomerWithoutOrdersDTO>(customer);
            return Ok(customerResultOnly);
           
        }



        #endregion


    }
}
