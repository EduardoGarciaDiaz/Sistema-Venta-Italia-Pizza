using ItaliaPizza_Cliente.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace ItaliaPizza_Cliente.Utilidades
{
    public class ManejadorExcepcion
    {
        public static void ManejarExcepcionError(Exception ex, NavigationService navigationService)
        {
            //_logger.Error(ex.Message + "\n" + ex.StackTrace + "\n");
            Console.WriteLine(ex.Message + "\n" + ex.StackTrace + "\n");
            navigationService?.Navigate(new InicioSesion());
        }

        public static void ManejarExcepcionFatal(Exception ex, NavigationService navigationService)
        {
            //_logger.Fatal(ex.Message + "\n" + ex.StackTrace + "\n");
            Console.WriteLine(ex.Message + "\n" + ex.StackTrace + "\n");
            navigationService?.Navigate(new InicioSesion());
        }

    }
}
