using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ItaliaPizza_Servicios.ItaliaPizzaServicio)))
            {
                host.Open();
                Console.WriteLine("Server is running");
                foreach (TiposEmpleado tip in Class1.ProbarBD())
                {
                    Console.WriteLine(tip.Nombre);
                }
                Console.ReadLine();
            }
        }
    }
}
