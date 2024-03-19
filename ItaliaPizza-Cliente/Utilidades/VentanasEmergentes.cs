using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ItaliaPizza_Cliente.Utilidades
{
    public static class VentanasEmergentes
    {
        private const int VENTANA_ERROR = 1;

        public static void MostrarVentanaErrorConexionFallida()
        {
            string tituloVentanaEmergente = "Conexión fallida";
            string mensajeVentanaEmergente = "No se pudo conectar con el servidor, por favor inténtalo más tarde.";
            Window ventanaActual = Application.Current.MainWindow;

            VentanaEmergente ventanaEmergente = new VentanaEmergente(
                tituloVentanaEmergente,
                mensajeVentanaEmergente,
                ventanaActual,
                VENTANA_ERROR);
        }

        public static void MostrarVentanaErrorTiempoEspera()
        {
            string tituloVentanaEmergente = "Tiempo de espera excedido";
            string mensajeVentanaEmergente = "La operación ha excedido el tiempo de espera, por favor inténtelo más tarde.";
            Window ventanaActual = Application.Current.MainWindow;

            VentanaEmergente ventanaEmergente = new VentanaEmergente(
                tituloVentanaEmergente,
                mensajeVentanaEmergente,
                ventanaActual,
                VENTANA_ERROR);
        }

        public static void MostrarVentanaErrorBaseDatos()
        {
            string tituloVentanaEmergente = "Error en la base de datos";
            string mensajeVentanaEmergente = "Ocurrió un error en la base de datos, por favor inténtelo más tarde.";
            Window ventanaActual = Application.Current.MainWindow;

            VentanaEmergente ventanaEmergente = new VentanaEmergente(
                tituloVentanaEmergente,
                mensajeVentanaEmergente,
                ventanaActual,
                VENTANA_ERROR);
        }

        public static void MostrarVentanaErrorServidor()
        {
            string tituloVentanaEmergente = "Error en el servidor";
            string mensajeVentanaEmergente = "Ocurrió un error en el servidor, por favor inténtelo más tarde.";
            Window ventanaActual = Application.Current.MainWindow;

            VentanaEmergente ventanaEmergente = new VentanaEmergente(
                tituloVentanaEmergente,
                mensajeVentanaEmergente,
                ventanaActual,
                VENTANA_ERROR);
        }

        public static void MostrarVentanaErrorInesperado()
        {
            string tituloVentanaEmergente = "Error inesperado";
            string mensajeVentanaEmergente = "Ocurrió un error inesperado, por favor inténtelo más tarde.";
            Window ventanaActual = Application.Current.MainWindow;

            VentanaEmergente ventanaEmergente = new VentanaEmergente(
                tituloVentanaEmergente,
                mensajeVentanaEmergente,
                ventanaActual,
                VENTANA_ERROR);
        }
    }
}
