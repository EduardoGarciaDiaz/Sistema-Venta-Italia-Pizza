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
        public EventHandler gridInsumoRegistroReceta_Click;


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

        private void CargarDatos()
        {
            lbCodigo.Content = InsumoAsignado.Codigo;
            lbNombre.Content = InsumoAsignado.Nombre;
            lbCategoria.Content = InsumoAsignado.Categoria.Nombre;
        }

        public void GridInsumoRegistroReceta_Click(object sender, RoutedEventArgs e)
        {
            gridInsumoRegistroReceta_Click?.Invoke(this, e);
        }
    }
}
