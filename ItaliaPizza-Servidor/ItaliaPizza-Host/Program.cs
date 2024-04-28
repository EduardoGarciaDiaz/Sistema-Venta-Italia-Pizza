using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Host
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(ItaliaPizza_Servicios.ServicioItaliaPizza)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("Server is running");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Server is NOT running");
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
