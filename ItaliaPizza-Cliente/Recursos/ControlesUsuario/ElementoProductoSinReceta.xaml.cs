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
    /// Lógica de interacción para ElementoProductoSinReceta.xaml
    /// </summary>
    public partial class ElementoProductoSinReceta : UserControl
    {
        public ProductoSinReceta ProductoAsignado { get; set; }
        public bool EsSeleccionado { get; set; }
        public EventHandler GrdProductoSinRecetaClicked;

        public ElementoProductoSinReceta()
        {
            InitializeComponent();
        }

        public ElementoProductoSinReceta(ProductoSinReceta producto)
        {
            InitializeComponent();
            ProductoAsignado = producto;
            EsSeleccionado = false;
            CargarDatos();
        }

        public void GrdProductoSinReceta_Click(object sender, RoutedEventArgs e)
        {
            GrdProductoSinRecetaClicked?.Invoke(this, e);
        }

        private void CargarDatos()
        {
            lblCodigo.Content = ProductoAsignado.Codigo;
            tbkNombre.Text = ProductoAsignado.Nombre;

            if (ProductoAsignado.Foto != null)
            {
                BitmapImage foto = ConvertidorBytes.ConvertirBytesABitmapImage(ProductoAsignado.Foto);
                imgFoto.Source = foto;
            }
        }

       

    }
}
