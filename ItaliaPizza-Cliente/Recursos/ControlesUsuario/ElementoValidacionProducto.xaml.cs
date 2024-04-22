using ItaliaPizza_Cliente.ServicioItaliaPizza;
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
    /// Lógica de interacción para ElementoValidacionProducto.xaml
    /// </summary>
    public partial class ElementoValidacionProducto : UserControl
    {
        private const string SIMBOLO_MONEDA = "$";
        public EventHandler tbxCantidadFisicaEnter_Pressed;

        public Producto ProductoAsignado { get; set; }

        public ElementoValidacionProducto()
        {
            InitializeComponent();
        }

        public ElementoValidacionProducto(Producto producto)
        {
            InitializeComponent();
            ProductoAsignado = producto;
            CrearElementoValidacion();
        }

        private void CrearElementoValidacion()
        {
            lblCodigo.Content = ProductoAsignado.Codigo;
            lblNombre.Content = ProductoAsignado.Nombre;
            lblCategoria.Content = ProductoAsignado.Insumo.Categoria.Nombre;
            if (ProductoAsignado.ProductoVenta != null)
            {
                lblCategoria.Content = ProductoAsignado.ProductoVenta.Categoria.Nombre; 
            }
            string cantidad = ProductoAsignado.Insumo.Cantidad.ToString();
            string unidadMedida = ProductoAsignado.Insumo.UnidadMedida.Nombre;
            lblCantidad.Content = cantidad + unidadMedida;
            lblCosto.Content = SIMBOLO_MONEDA + ProductoAsignado.Insumo.CostoUnitario;
            lblUnidadMedida.Content = unidadMedida;
        }

        private void TbxCantidadFisicaEnter_Pressed(object sender, KeyEventArgs e)
        {
            tbxCantidadFisicaEnter_Pressed?.Invoke(this, e);
        }
    }
}
