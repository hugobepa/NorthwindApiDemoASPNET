using NorthwindApiDemo.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApiDemo.Services
{
    public interface ICustomerRepository
    {
        IEnumerable<Customers> GetCustomers();
        Customers GetCustomer(string customerId, bool includeOrders);
        IEnumerable<Orders> GetOrders(string customerId);
        Orders GetOrder(string customerId, int orderId);
        bool CustomerExists(string customerId);
        void AddOrder(string customerId, Orders order);
        bool Save();
        void DeleteOrder(Orders order);
    }
}
