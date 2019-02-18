using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NorthwindApiDemo.EFModels;

namespace NorthwindApiDemo.Services
{
    public class CustomerRepository : ICustomerRepository
    {

        private EFModels.NorthwindContext _context;

        public CustomerRepository(NorthwindContext context)
        {
            this._context = context;
        }
        //s03c30
        //obtener cliente y guradar orden
        public void AddOrder(string customerId, Orders orders)
        {
            var customer = GetCustomers(customerId, false);
            customer.Orders.Add(orders);
            //throw new NotImplementedException();
        }


        //S03C029 extraer ordenes por metodo
        //si el cliente existe o no
        public bool CustomerExists(string customerId)
        {

            return _context.Customers.Any(c => c.CustomerId == customerId);
            //throw new NotImplementedException();
        }

        //S03C33 EliminarRegistro
        public void DeleteOrder(Orders order)
        {

            _context.Orders.Remove(order);
            //throw new NotImplementedException();
        }

        public IEnumerable<Customers> GetCustomers()
        {

            //retornar lista ordenada por compañias
            return _context.Customers.OrderBy(c => c.CompanyName).ToList();

            //throw new NotImplementedException();
        }

        public Customers GetCustomers(string customerId, bool includeOrders)
        {
            //si tiene ordenes idCliente comoordernes
            if (includeOrders)
            {
                //using Microsoft.EntityFrameworkCore;   Include
                return _context.Customers.Include(c => c.Orders).Where(c =>  c.CustomerId == customerId).FirstOrDefault();
            }

            //Solo cliente
            return _context.Customers.Where(c => c.CustomerId == customerId).FirstOrDefault();
            //throw new NotImplementedException();
        }

        public IEnumerable<Orders> GetOrders(string customerId)
        {
           
            return _context.Orders.Where(c => c.CustomerId == customerId).ToList();
            //throw new NotImplementedException();
        }

        public Orders GetOrders(string customerId, int orderId)
        {

            return _context.Orders.Where(c => c.CustomerId == customerId && c.OrderId == orderId).FirstOrDefault();
            //throw new NotImplementedException();
        }

        //c03s30 hacemo persistencia para guardar ordenes
        //si hacemos la validacion sobre uno o mas registros
        public bool Save()
        {

            return (_context.SaveChanges() >= 0);
            //throw new NotImplementedException();
        }
    }
}
