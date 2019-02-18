using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindApiDemo.Services
{
    //inyectar dentro de las mismas controladores
    //S03c25
    public interface ICustomerRepository
    {
        //ObtenerCliente y saber si queremos ordenes o no
        IEnumerable<EFModels.Customers> GetCustomers();
        EFModels.Customers GetCustomers(string customerId, bool includeOrders);
        //obtener ordenes
        IEnumerable<EFModels.Orders> GetOrders(string customerId);
        EFModels.Orders GetOrders(string customerId, int orderId);
        //S03cC29 creamos un booleano para verificar si cliente existe
        bool CustomerExists(string customerId);
        //S03c30 crear registro BBDD
        void AddOrder(string customerId, EFModels.Orders orders);
        bool Save();
        //S03C33 EliminarRegistro
        void DeleteOrder(EFModels.Orders orders);

    }
}
