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
    /// Lógica de interacción para ElementoInsumoSeleccionado.xaml
    /// </summary>
    public partial class ElementoInsumoSeleccionado : UserControl
    {
        public InsumoReceta InsumoAsignado { get; set; }
        public EventHandler BtnDesasignarInsumoClicked;

        public ElementoInsumoSeleccionado()
        {
            InitializeComponent();
        }

        public ElementoInsumoSeleccionado(InsumoReceta insumo)
        {
            InitializeComponent();
            InsumoAsignado = insumo;
            CargarDatos();
        }

        private void BtnDesasignarInsumo_Click(object sender, RoutedEventArgs e)
        {
            BtnDesasignarInsumoClicked?.Invoke(this, e);
        }

        private void CargarDatos()
        {
            string cantidadInsumoPreestablecida = "1";

            lbNombreInsumo.Content = InsumoAsignado.Nombre;
            lbUnidadMedida.Content = InsumoAsignado.UnidadMedida.Nombre;
            tbxCantidadInsumo.Text = cantidadInsumoPreestablecida;
        }

    }
}
