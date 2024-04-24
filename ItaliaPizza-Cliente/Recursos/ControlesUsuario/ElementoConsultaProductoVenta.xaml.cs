using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza_Cliente.Recursos.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para ElementoConsultaProductoVenta.xaml
    /// </summary>
    public partial class ElementoConsultaProductoVenta : UserControl
    {
        public Producto ProductoAsignado { get; set; }
        public EventHandler GridProductoVentaClicked;
        public EventHandler ImgModificarProductoVentaClicked;
        public EventHandler BtnDesactivarActivarProductoClicked;
        private const string SIMBOLO_MONEDA = "$";

        public ElementoConsultaProductoVenta()
        {
            InitializeComponent();
        }

        public ElementoConsultaProductoVenta(Producto productoVenta)
        {
            InitializeComponent();
            ProductoAsignado = productoVenta;
            CrearElementoProductoVenta();
        }

        private void CrearElementoProductoVenta()
        {
            ProductoVenta productoVenta = ProductoAsignado.ProductoVenta;
            lblCodigo.Content = ProductoAsignado.Codigo;
            lblNombre.Content = ProductoAsignado.Nombre;
            lblPrecio.Content = SIMBOLO_MONEDA + productoVenta.Precio;
            if (productoVenta.Foto != null)
            {
                BitmapImage foto = ConvertidorBytes.ConvertirBytesABitmapImage(productoVenta.Foto);
                if (foto != null)
                {
                    imgFotoProducto.Source = foto;
                }
            }
            ActualizarEstadoActivo(ProductoAsignado.EsActivo);
        }

        public void ActualizarEstadoActivo(bool esActivo)
        {
            if (!esActivo)
            {
                brdActivoBackground.Background = new SolidColorBrush(Colors.White);
                brdActivoBackground.BorderBrush = new SolidColorBrush(Colors.Gray);
                btnEsActivo.HorizontalAlignment = HorizontalAlignment.Left;
                btnEsActivo.Fill = new SolidColorBrush(Colors.Gray);
                lblModificarEstado.Content = "Activar";
            }
            else
            {
                brdActivoBackground.Background = new SolidColorBrush(Colors.Black);
                brdActivoBackground.BorderBrush = new SolidColorBrush(Colors.Black);
                btnEsActivo.HorizontalAlignment = HorizontalAlignment.Right;
                btnEsActivo.Fill = new SolidColorBrush(Colors.Yellow);
                lblModificarEstado.Content = "Desactivar";
            }
        }

        private void GridProductoVenta_Click(object sender, RoutedEventArgs e)
        {
            GridProductoVentaClicked?.Invoke(this, e);
        }

        private void ImgModificarProductoVenta_Click(object sender, RoutedEventArgs e)
        {
            ImgModificarProductoVentaClicked?.Invoke(this, e);
        }

        private void BtnDesactivarActivar_Click(object sender, MouseButtonEventArgs e)
        {
            BtnDesactivarActivarProductoClicked?.Invoke(this, e);
        }
    }
}
