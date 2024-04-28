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
    /// Lógica de interacción para ElementoInsumoRegistroReceta.xaml
    /// </summary>
    public partial class ElementoInsumoRegistroReceta : UserControl
    {
        public bool EsSeleccionado { get; set; }
        public InsumoRegistroReceta InsumoAsignado { get; set; }
        public EventHandler GrdInsumoRegistroRecetaClicked;

        public ElementoInsumoRegistroReceta()
        {
            InitializeComponent();
        }

        public ElementoInsumoRegistroReceta(InsumoRegistroReceta insumo)
        {
            InitializeComponent();
            InsumoAsignado = insumo;
            CargarDatos();
        }

        public void GrdInsumoRegistroReceta_Click(object sender, RoutedEventArgs e)
        {
            GrdInsumoRegistroRecetaClicked?.Invoke(this, e);
        }       

        private void CargarDatos()
        {
            lblCodigo.Content = InsumoAsignado.Codigo;
            lblNombre.Content = InsumoAsignado.Nombre;
            lblCategoria.Content = InsumoAsignado.Categoria.Nombre;
        }

    }
}
