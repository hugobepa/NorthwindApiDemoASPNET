using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using NorthwindApiDemo.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApiDemo.Controllers
{
    //api/customers/23/orders/1
    //Esta controladora controlara el tema de las ordenes 
    //Creamos la ruta inicial
    [Route("api/customers")]
    public class OrdersController : Controller
    {

        

        //S03C29 crear variable  y constructor
        //using NorthwindApiDemo.Services;
        private Services.ICustomerRepository _customerRepository;
        public OrdersController(Services.ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }


        //la id del cliente y las ordenes
        [HttpGet("{customerId}/orders")]
        //>S03C29 
        //public IActionResult GetOrders(int customerId)
        public IActionResult GetOrders(string customerId)
        {
            //S03C29 Creamos variable para extraerOrdenes
            //Verificamos si hay un cliente anterior
            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var orders = _customerRepository.GetOrders(customerId);

            //S03C29 Creamos una variable para crear una lista  obtenida de una variable de datos
            var ordersResult = AutoMapper.Mapper.Map<IEnumerable<Models.OrdersDTO>>(orders);

            return Ok(ordersResult);


            #region >S03c29

            /*
             *   var customer = Repository.Instance.Customers.FirstOrDefault(c => c.ID == customerId);

            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer.Orders);
             * 
             */
            #endregion

        }


        //creamos la lista de orders
        //Name ="GetOrder" es para llamar al metodo desde la misma funcion
        //> 03C29 public IActionResult GetOrder(int customerId, int id)
        [HttpGet("{customerId}/orders/{id}",Name ="GetOrder")]
        public IActionResult GetOrder(string customerId, int id)
        {

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var order = _customerRepository.GetOrders(customerId, id);

            if(order == null)
            {
                return NotFound();
            }
            #region >S03C29

            /*
             *  var customer = Repository.Instance.Customers.FirstOrDefault(c => c.ID == customerId);

            if (customer == null)
            {
                return NotFound();
            }
            //podemos ver una orden especifica de un cliente
            var order = customer.Orders.FirstOrDefault(o => o.OrderId == id);
            //return Ok(customer.Orders);
            //if (customer == null)
            if (order == null)
            {
                return NotFound();
            }
             */

            #endregion


            var orderResult = AutoMapper.Mapper.Map<Models.OrdersDTO>(order);

            return Ok(orderResult);
        }

        #region >S03c30 IActionResult CreaterOrder

        /*
         * 
         *  //S02c17CreacionNuevoregistro
        //[FromBody] este dato se cogera de dentro de la solicitud
        //[HttpPost("{customerId/orders}")] al coger el dato de la se pone post
        [HttpPost("{customerId}/orders")]
        public IActionResult CreaterOrder(int customerId, [FromBody] Models.OrdersForCreationDTO order)
        {
            //Si el campos es erroneo o vacio damos este error
            if (order == null)
            {
                return BadRequest();
            }
            //sino valida se escribe el mensaje de la classe mediante ModelState
           if( !ModelState.IsValid) { return BadRequest(ModelState); }

            var customer = Repository.Instance.Customers.FirstOrDefault(c => c.ID == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            //extraer el valor  maximo de la ID
            var maxOrderId = Repository.Instance.Customers.SelectMany(c => c.Orders).Max(o => o.OrderId);

            //crear un a Orden nuevo con un autoSuma de OrderId
            var finalOrder = new Models.OrdersDTO()
            {
                OrderId = maxOrderId++,
                CustomerId = order.CustomerId,
                EmployeeId = order.EmployeeId,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                ShipVia = order.ShipVia,
                Freight = order.Freight,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipRegion = order.ShipRegion,
                ShipPostalCode = order.ShipPostalCode,
                ShipCountry = order.ShipCountry
            };

            //CreatedAtRoute(string routeName, object routeValues, object value)
            return CreatedAtRoute("GetOrder", new { customerId = customerId, id = finalOrder.OrderId},finalOrder);

        }
         * 
         */





        #endregion

        //S03c30CreacionNuevoregistro
        //[FromBody] este dato se cogera de dentro de la solicitud
        //[HttpPost("{customerId/orders}")] al coger el dato de la se pone post
        [HttpPost("{customerId}/orders")]
        public IActionResult CreaterOrder(string customerId, [FromBody] Models.OrdersForCreationDTO order)
        {
            //Si el campos es erroneo o vacio damos este error
            if (order == null)
            {
                return BadRequest();
            }
            //sino valida se escribe el mensaje de la classe mediante ModelState
           if( !ModelState.IsValid) { return BadRequest(ModelState); }


            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            //crear un a Orden nuevo con un autoSuma de OrderId
            var finalOrder = AutoMapper.Mapper.Map<Orders>(order);

            //S03c30
            _customerRepository.AddOrder(customerId, finalOrder);

            if(!_customerRepository.Save())
            {
                return StatusCode(500, "Please verify your data");
            }

            var customerCreated = AutoMapper.Mapper.Map<Models.OrdersDTO>(finalOrder);
            
         

            //CreatedAtRoute(string routeName, object routeValues, object value)
            return CreatedAtRoute("GetOrder", new { customerId = customerId, id = customerCreated.OrderId}, customerCreated);

        }

        #region >S03C031 UpdateOrder  IActionResult UpdateOrder

        /*
         * 
         *   //S02c19ActualizarRegistro
        //[HttpPut("{customerId}/orders/{id}")] actualizar
        //[FromBody] este dato se cogera de dentro de la solicitud
        // return NoContent()  Noactualizar la url de este registro
        [HttpPut("{customerId}/orders/{id}")]
        public IActionResult UpdateOrder(int customerId,int id,[FromBody] Models.OrdersForUpdateDTO order)
        {
            //Si el campos es erroneo o vacio damos este error
            if (order == null)
            {
                return BadRequest();
            }
            //sino valida se escribe el mensaje de la classe mediante ModelState
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var customer = Repository.Instance.Customers.FirstOrDefault(c => c.ID == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            var orderFromRepository = customer.Orders.FirstOrDefault(o => o.OrderId == id);

            if (orderFromRepository == null)
            {
                return NotFound();
            }

            orderFromRepository.CustomerId = order.CustomerId;
            orderFromRepository.EmployeeId = order.EmployeeId ;
            orderFromRepository.OrderDate = order.OrderDate;
            orderFromRepository.RequiredDate = order.RequiredDate;
            orderFromRepository.ShippedDate = order.ShippedDate;
            orderFromRepository.ShipVia = order.ShipVia;
            orderFromRepository.Freight = order.Freight;
            orderFromRepository.ShipName = order.ShipName;
            orderFromRepository.ShipAddress = order.ShipAddress;
            orderFromRepository.ShipCity = order.ShipCity;
            orderFromRepository.ShipRegion = order.ShipRegion;
            orderFromRepository.ShipPostalCode = order.ShipPostalCode;
            orderFromRepository.ShipCountry = order.ShipCountry;

            return NoContent();
        }

        //S02c20ActualizarRegistroParcial
        //[HttpPut("{customerId}/orders/{id}")] actualizar
        //[FromBody] este dato se cogera de dentro de la solicitud
        // return NoContent()  Noactualizar la url de este registro
        //ejecutar la instancia patchDocument.ApplyTo(orderToUpdate);
        [HttpPatch("{customerId}/orders/{id}")]
        public IActionResult UpdateOrder(int customerId, int id, [FromBody] JsonPatchDocument<Models.OrdersForUpdateDTO> patchDocument)
        {
            //Si el campos es erroneo o vacio damos este error
            if (patchDocument == null)
            {
                return BadRequest();
            }
            //sino valida se escribe el mensaje de la classe mediante ModelState
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var customer = Repository.Instance.Customers.FirstOrDefault(c => c.ID == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            var orderFromRepository = customer.Orders.FirstOrDefault(o => o.OrderId == id);

            if (orderFromRepository == null)
            {
                return NotFound();
            }
            var orderToUpdate = new Models.OrdersForUpdateDTO()
            {
                   CustomerId = orderFromRepository.CustomerId,
                  EmployeeId = orderFromRepository.EmployeeId,
                OrderDate = orderFromRepository.OrderDate,
                RequiredDate = orderFromRepository.RequiredDate,
                ShippedDate = orderFromRepository.ShippedDate,
                ShipVia = orderFromRepository.ShipVia,
                Freight = orderFromRepository.Freight,
                ShipName = orderFromRepository.ShipName,
                ShipAddress = orderFromRepository.ShipAddress,
                ShipCity = orderFromRepository.ShipCity,
                ShipRegion  = orderFromRepository.ShipRegion,
                ShipPostalCode = orderFromRepository.ShipPostalCode,
                ShipCountry = orderFromRepository.ShipCountry,

            };

            patchDocument.ApplyTo(orderToUpdate, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            orderFromRepository.CustomerId = orderToUpdate.CustomerId;
            orderFromRepository.EmployeeId = orderToUpdate.EmployeeId;
            orderFromRepository.OrderDate =orderToUpdate.OrderDate;
            orderFromRepository.RequiredDate =orderToUpdate.RequiredDate;
            orderFromRepository.ShippedDate =orderToUpdate.ShippedDate;
            orderFromRepository.ShipVia =orderToUpdate.ShipVia;
            orderFromRepository.Freight =orderToUpdate.Freight;
            orderFromRepository.ShipName =orderToUpdate.ShipName;
            orderFromRepository.ShipAddress =orderToUpdate.ShipAddress;
            orderFromRepository.ShipCity =orderToUpdate.ShipCity;
            orderFromRepository.ShipRegion =orderToUpdate.ShipRegion;
            orderFromRepository.ShipPostalCode =orderToUpdate.ShipPostalCode;
            orderFromRepository.ShipCountry =orderToUpdate.ShipCountry;

            return NoContent();

        }
         * 
         */




        #endregion

        #region  IActionResult UpdateOrder <03C031 && >S03C32
        /*
         *    //S03c31ActualizarRegistro
    //[HttpPut("{customerId}/orders/{id}")] actualizar
    //[FromBody] este dato se cogera de dentro de la solicitud
    // return NoContent()  Noactualizar la url de este registro
    [HttpPut("{customerId}/orders/{id}")]
    public IActionResult UpdateOrder(string customerId,int id,[FromBody] Models.OrdersForUpdateDTO order)
    {
        //Si el campos es erroneo o vacio damos este error
        if (order == null)
        {
            return BadRequest();
        }
        //sino valida se escribe el mensaje de la classe mediante ModelState
        if (!ModelState.IsValid) { return BadRequest(ModelState); }

        //S03C30 actulizar registro
        if (!_customerRepository.CustomerExists(customerId))
        {
            return NotFound();
        }

        var existingOrder = _customerRepository.GetOrders(customerId,id);
        if(existingOrder ==null)
        {
            return NotFound();
        }
        //sobrecargamos metodo para acutilizar orden existente
        AutoMapper.Mapper.Map(order, existingOrder);

        if (!_customerRepository.Save())
        {
            return StatusCode(500, "Please verify your data");
        }

        return NoContent();
    }
         */

        #endregion


        //S03c32ActualizarRegistroParcial
        //[HttpPut("{customerId}/orders/{id}")] actualizar
        //[FromBody] este dato se cogera de dentro de la solicitud
        // return NoContent()  Noactualizar la url de este registro
        [HttpPatch("{customerId}/orders/{id}")]
        public IActionResult UpdateOrder(string customerId,int id,[FromBody] JsonPatchDocument<Models.OrdersForUpdateDTO> patchDocument)
        {
            //Si el campos es erroneo o vacio damos este error
            if (patchDocument == null)
            {
                return BadRequest();
            }
            //sino valida se escribe el mensaje de la classe mediante ModelState
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var existingOrder = _customerRepository.GetOrders(customerId, id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            var orderToUpdate = AutoMapper.Mapper.Map<Models.OrdersForUpdateDTO>(existingOrder);

            patchDocument .ApplyTo(orderToUpdate, ModelState);


            //S03C32UpdatePartialMappeo
            //para hacer correciones y validaciones metodo especificado
            TryValidateModel(orderToUpdate);


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //S03C32 UpdatePartialMappeo
            //mapeados los datos cambiados
            AutoMapper.Mapper.Map(orderToUpdate, existingOrder);

            if (!_customerRepository.Save())
            {
                return StatusCode(500, "Please verify your data");
            }

            return NoContent();
            
        }
        /*
        //S02c20ActualizarRegistroParcial
        //[HttpPut("{customerId}/orders/{id}")] actualizar
        //[FromBody] este dato se cogera de dentro de la solicitud
        // return NoContent()  Noactualizar la url de este registro
        //ejecutar la instancia patchDocument.ApplyTo(orderToUpdate);
        [HttpPatch("{customerId}/orders/{id}")]
        public IActionResult UpdateOrder(int customerId, int id, [FromBody] JsonPatchDocument<Models.OrdersForUpdateDTO> patchDocument)
        {
            //Si el campos es erroneo o vacio damos este error
            if (patchDocument == null)
            {
                return BadRequest();
            }
            //sino valida se escribe el mensaje de la classe mediante ModelState
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var customer = Repository.Instance.Customers.FirstOrDefault(c => c.ID == customerId);

            if (customer == null)
            {
                return NotFound();
            }

            var orderFromRepository = customer.Orders.FirstOrDefault(o => o.OrderId == id);

            if (orderFromRepository == null)
            {
                return NotFound();
            }
            var orderToUpdate = new Models.OrdersForUpdateDTO()
            {
                   CustomerId = orderFromRepository.CustomerId,
                  EmployeeId = orderFromRepository.EmployeeId,
                OrderDate = orderFromRepository.OrderDate,
                RequiredDate = orderFromRepository.RequiredDate,
                ShippedDate = orderFromRepository.ShippedDate,
                ShipVia = orderFromRepository.ShipVia,
                Freight = orderFromRepository.Freight,
                ShipName = orderFromRepository.ShipName,
                ShipAddress = orderFromRepository.ShipAddress,
                ShipCity = orderFromRepository.ShipCity,
                ShipRegion  = orderFromRepository.ShipRegion,
                ShipPostalCode = orderFromRepository.ShipPostalCode,
                ShipCountry = orderFromRepository.ShipCountry,

            };

            patchDocument.ApplyTo(orderToUpdate, ModelState);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            orderFromRepository.CustomerId = orderToUpdate.CustomerId;
            orderFromRepository.EmployeeId = orderToUpdate.EmployeeId;
            orderFromRepository.OrderDate =orderToUpdate.OrderDate;
            orderFromRepository.RequiredDate =orderToUpdate.RequiredDate;
            orderFromRepository.ShippedDate =orderToUpdate.ShippedDate;
            orderFromRepository.ShipVia =orderToUpdate.ShipVia;
            orderFromRepository.Freight =orderToUpdate.Freight;
            orderFromRepository.ShipName =orderToUpdate.ShipName;
            orderFromRepository.ShipAddress =orderToUpdate.ShipAddress;
            orderFromRepository.ShipCity =orderToUpdate.ShipCity;
            orderFromRepository.ShipRegion =orderToUpdate.ShipRegion;
            orderFromRepository.ShipPostalCode =orderToUpdate.ShipPostalCode;
            orderFromRepository.ShipCountry =orderToUpdate.ShipCountry;

            return NoContent();

        }
        */


        #region MyRegion  >S03C33IActionResult DeleteOrder 
        /*
         *  //S02c20EliminaRegistro
            //[HttpDelete("{customerId}/orders/{id}")] actualizar
            //[FromBody] este dato se cogera de dentro de la solicitud
            // return NoContent()  Noactualizar la url de este registro      
            [HttpDelete("{customerId}/orders/{id}")]
            public IActionResult DeleteOrder(int customerId, int id)
            {
                var customer = Repository.Instance.Customers.FirstOrDefault(c => c.ID == customerId);

                if (customer == null)
                {
                    return NotFound();
                }
                //podemos ver una orden especifica de un cliente
                var order = customer.Orders.FirstOrDefault(o => o.OrderId == id);
                //return Ok(customer.Orders);
                if (order == null)
                {
                    return NotFound();
                }

                customer.Orders.Remove(order);
                return NoContent();
            }  
         */
        #endregion




        //S03C33EliminaRegistroMapeo
        //[HttpDelete("{customerId}/orders/{id}")] actualizar
        //[FromBody] este dato se cogera de dentro de la solicitud
        // return NoContent()  Noactualizar la url de este registro      
        [HttpDelete("{customerId}/orders/{id}")]
        public IActionResult DeleteOrder(string customerId, int id)
        {
            //sino valida se escribe el mensaje de la classe mediante ModelState
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            if (!_customerRepository.CustomerExists(customerId))
            {
                return NotFound();
            }

            var existingOrder = _customerRepository.GetOrders(customerId, id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            /*
            //podemos ver una orden especifica de un cliente
            var order = customer.Orders.FirstOrDefault(o => o.OrderId == id);
            //return Ok(customer.Orders);
            if (order == null)
            {
                return NotFound();
            }
            */
            _customerRepository.DeleteOrder(existingOrder);

            if (!_customerRepository.Save())
            {
                return StatusCode(500, "Please verify your data");
            }

            return NoContent();
        }  
            
            
    }
}
