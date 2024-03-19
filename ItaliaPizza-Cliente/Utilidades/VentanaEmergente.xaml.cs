using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ItaliaPizza_Cliente.Utilidades
{
    /// <summary>
    /// Interaction logic for VentanaEmergente.xaml
    /// </summary>
    public partial class VentanaEmergente : Window
    {
        private const int VENTANA_ERROR = 1;
        private const int VENTANA_INFORMACION = 2;
        private const int VENTANA_CONFIRMACION = 3;
        public bool AceptarAccion { get; set; }
        public VentanaEmergente(String titulo, String mensaje, Window ventaPrincipal, int tipoVentana)
        {
            InitializeComponent();
            txbTitulo.Text = titulo;
            txbDescripcion.Text = mensaje;
            AceptarAccion = false;
            MostrarTipoVentana(tipoVentana);
            MostrarVentana(ventaPrincipal, this);
        }

        public VentanaEmergente(String titulo, String mensaje, String mensajeBtn1, String mensajeBtn2, Window ventaPrincipal, int tipoVentana)
        {
            InitializeComponent();
            txbTitulo.Text = titulo;
            txbDescripcion.Text = mensaje;
            btnAceptarAccion.Content = mensajeBtn1;
            btnNegarAccion.Content = mensajeBtn2;
            AceptarAccion = false;
            MostrarTipoVentana(tipoVentana);
            MostrarVentana(ventaPrincipal, this);
        }

        private void MostrarTipoVentana(int tipoVentana)
        {
            if (tipoVentana == VENTANA_ERROR)
            {
                stp1boton.Visibility = Visibility.Visible;
                imgImagen.Source = new BitmapImage(new Uri("/Recursos/iconos/icono_error.png", UriKind.Relative));

            }
            else if (tipoVentana == VENTANA_INFORMACION)
            {
                stp1boton.Visibility = Visibility.Visible;
                imgImagen.Source = new BitmapImage(new Uri("/Recursos/iconos/icono_informacion.png", UriKind.Relative));
            }
            else if (tipoVentana == VENTANA_CONFIRMACION)
            {
                stp2boton.Visibility = Visibility.Visible;
                imgImagen.Source = new BitmapImage(new Uri("/Recursos/iconos/icono_confirmacion.png", UriKind.Relative));
            }  
        }

        private static void MostrarVentana(Window ventanPricipal, Window ventanaEmergente)
        {
            if (ventanPricipal != null)
            {
                double left = ventanPricipal.Left + (ventanPricipal.Width - ventanaEmergente.Width) / 2;
                double top = ventanPricipal.Top + (ventanPricipal.Height - ventanaEmergente.Height) / 2;
                ventanaEmergente.Left = left;
                ventanaEmergente.Top = top;
                ventanaEmergente.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        private void btnAceptarAccion_Click(object sender, RoutedEventArgs e)
        {
            AceptarAccion = true;
            this.Close();
        }

        private void btnNegarAccion_Click(object sender, RoutedEventArgs e)
        {
            AceptarAccion = false;
            this.Close();
        }

        private void btnCerrarEmergente_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
