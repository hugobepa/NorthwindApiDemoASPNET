using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation.ObjectHydrator;

namespace NorthwindApiDemo
{
    public class Repository
    {
        //crearemos una propiedad que se Inicializara al inicio de proyecto
        //por ese motivo lo ponemos static
        public static Repository Instance { get; } = new Repository();
        //Llamaremos  al modelo Customers  mediante una lista
        public IList<Models.CustomersDTO> Customers { get; set; }
        //crear contructor
        //raiz Proyecto , BD, paquete Nuguet buscar: "hydrator" , foundation.ObjectHydrator:core (instalar)
        public Repository()
        {

            Hydrator<Models.CustomersDTO> hydrator = new Hydrator<Models.CustomersDTO>();
            //creamos el conjunto de 5 resultados
            Customers = hydrator.GetList(5);

            //C16 crear unas ordenes aleatorias a cada cliente

            Random random = new Random();
            Hydrator<Models.OrdersDTO> ordersHydrator = new Hydrator<Models.OrdersDTO>();
            foreach (var customer in Customers)
            {
                customer.Orders = ordersHydrator.GetList(random.Next(1, 10));
            }



        }
       
     

    }
}
