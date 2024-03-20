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
    /// Interaction logic for ElementoInsumoOrdenCompraSeleccionado.xaml
    /// </summary>
    public partial class ElementoInsumoOrdenCompraSeleccionado : UserControl
    {
        public EventHandler btnInsumoMenos_Click;
        public EventHandler btnInsumoMas_Click;
        public EventHandler btnEliminarInsumo_Click;
        public EventHandler txbCantida_TextChanged;
        public InsumoOrdenCompraDto insumoDto;

        public ElementoInsumoOrdenCompraSeleccionado(InsumoOrdenCompraDto insumo)
        {
            InitializeComponent();
            InstanciarInsumo(insumo);
            this.insumoDto = insumo;
        }

        private void InstanciarInsumo(InsumoOrdenCompraDto insumo)
        {
            if (insumo != null)
            {
                lblNameInsumo.Content = insumo.Nombre;
                lblCodigoInsumo.Content = insumo.Codigo;
                lblCosto.Content = insumo.CostoUnitario.ToString();
                lblUnidadMedida.Content = insumo.UnidadMedida;
                lblSubtotal.Content = insumo.CostoUnitario.ToString();
                txbCantidad.Text = "1";
            }
        }

        private void BtnInsumoMenos(object sender, MouseButtonEventArgs e)
        {
            btnInsumoMenos_Click?.Invoke(this, e);
        }

        private void BtnInsumoMas(object sender, MouseButtonEventArgs e)
        {
            btnInsumoMas_Click?.Invoke(this, e);
        }

        private void BtnEliminarInsumoOrden(object sender, MouseButtonEventArgs e)
        {
            btnEliminarInsumo_Click?.Invoke(this, e);
        }

        private void TxbCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            txbCantida_TextChanged?.Invoke(this,e);
        }

        private void TxbCantidad_Input(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }
    }
}
