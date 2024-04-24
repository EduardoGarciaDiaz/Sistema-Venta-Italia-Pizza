using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace ItaliaPizza_Cliente.Utilidades
{
    public class ManejadorExcepcion
    {
        public static void ManejarExcepcionError(Exception ex, Window mainWindow)
        {
            //_logger.Error(ex.Message + "\n" + ex.StackTrace + "\n");
            Console.WriteLine(ex.Message + "\n" + ex.StackTrace + "\n");
            InicioSesion inicioSesion = new InicioSesion();
            inicioSesion.Show();
            mainWindow.Close();
        }

        public static void ManejarExcepcionError(Exception ex, Window mainWindow, Window secondaryWindow)
        {
            //_logger.Error(ex.Message + "\n" + ex.StackTrace + "\n");
            Console.WriteLine(ex.Message + "\n" + ex.StackTrace + "\n");
            InicioSesion inicioSesion = new InicioSesion();
            inicioSesion.Show();
            secondaryWindow.Close();
            mainWindow.Close();
        }

        public static void ManejarExcepcionFatal(Exception ex, Window mainWindow)
        {
            //_logger.Fatal(ex.Message + "\n" + ex.StackTrace + "\n");
            Console.WriteLine(ex.Message + "\n" + ex.StackTrace + "\n");
            CerrarSesion();
            InicioSesion inicioSesion = new InicioSesion();
            inicioSesion.Show();
            mainWindow.Close();
        }


        private static void CerrarSesion()
        {
            try
            {
                ServicioInicioSesionClient servicioInicioSesionClient = new ServicioInicioSesionClient();
                servicioInicioSesionClient.CerrarSesion(EmpleadoSingleton.getInstance().IdUsuario);
                EmpleadoSingleton.LimpiarSingleton();
            }
            catch (EndpointNotFoundException) { }
            catch (TimeoutException) { }
            catch (FaultException<ExcepcionServidorItaliaPizza>) { }
            catch (FaultException) { }
            catch (CommunicationException) { }
            catch (Exception) { }
        }
    }
}
