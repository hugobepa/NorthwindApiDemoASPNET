using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NorthwindApiDemo.Models;
using NorthwindApiDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApiDemo.Controllers
{
    [Route("api/customers")]
    public class CustomerController : Controller
    {
        private ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        [HttpGet()]
        public IActionResult GetCustomers()
        {
            var customers =
                _customerRepository
                .GetCustomers();
            var results = Mapper
                .Map<IEnumerable<CustomerWithoutOrders>>(customers);
            return new JsonResult(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomer(string id, bool includeOrders = false)
        {            
            var customer =
                _customerRepository.GetCustomer(id, includeOrders);
            if(customer == null)
            {
                return NotFound();
            }
         if(includeOrders)
            {
                var customerResult =
                    Mapper.Map<CustomerDTO>(customer);
                return Ok(customerResult);
            }
            var customerResultOnly =
                   Mapper.Map<CustomerWithoutOrders>(customer);
            return Ok(customerResultOnly);            
        }
    }
}
