using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Vistas;
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
    /// Lógica de interacción para ElementoInsumo.xaml
    /// </summary>
    public partial class ElementoConsultaInsumo : UserControl
    {
        public Producto ProductoAsignado { get; set; }
        public EventHandler gridInsumo_Click;
        public EventHandler imgModificarInsumo_Click;
        public EventHandler btnDesactivarActivarProducto_Click;
        private const string SIMBOLO_MONEDA = "$";

        public ElementoConsultaInsumo()
        {
            InitializeComponent();
        }

        public ElementoConsultaInsumo(Producto insumo)
        {
            InitializeComponent();
            ProductoAsignado = insumo;
            CrearElementoInsumo();
        }

        private void CrearElementoInsumo()
        {
            lblCodigo.Content = ProductoAsignado.Codigo;
            lblNombre.Content = ProductoAsignado.Nombre;
            lblCategoria.Content = ProductoAsignado.Insumo.Categoria.Nombre;
            lblCosto.Content = SIMBOLO_MONEDA + ProductoAsignado.Insumo.CostoUnitario;
            string cantidad = ProductoAsignado.Insumo.Cantidad.ToString();
            string unidadMedida = ProductoAsignado.Insumo.UnidadMedida.Nombre;
            lblCantidad.Content = cantidad + unidadMedida;
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

        private void BtnDesactivarActivar_Click(object sender, MouseButtonEventArgs e)
        {
            btnDesactivarActivarProducto_Click?.Invoke(this, e);
        }

        private void ImgModificarInsumo_Click(object sender, RoutedEventArgs e)
        {
            imgModificarInsumo_Click?.Invoke(this, e);
        }

        public void GridInsumo_Click(object sender, RoutedEventArgs e)
        {
            gridInsumo_Click?.Invoke(this, e);
        }
    }
}
