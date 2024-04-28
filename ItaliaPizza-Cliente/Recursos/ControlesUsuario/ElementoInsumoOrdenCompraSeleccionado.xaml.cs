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
        public EventHandler BtnInsumoMenosClicked;
        public EventHandler BtnInsumoMasClicked;
        public EventHandler BtnEliminarInsumoClicked;
        public EventHandler TxbCantidaTextChanged;
        public InsumoOrdenCompraDto Insumo { get; set; }

        public ElementoInsumoOrdenCompraSeleccionado(InsumoOrdenCompraDto insumo)
        {
            InitializeComponent();
            InstanciarInsumo(insumo);
            this.Insumo = insumo;
        }

        private void BtnInsumoMenos(object sender, MouseButtonEventArgs e)
        {
            BtnInsumoMenosClicked?.Invoke(this, e);
        }

        private void BtnInsumoMas(object sender, MouseButtonEventArgs e)
        {
            BtnInsumoMasClicked?.Invoke(this, e);
        }

        private void BtnEliminarInsumoOrden(object sender, MouseButtonEventArgs e)
        {
            BtnEliminarInsumoClicked?.Invoke(this, e);
        }

        private void TxbCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            TxbCantidaTextChanged?.Invoke(this,e);
        }

        private void TxbCantidad_Input(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private void InstanciarInsumo(InsumoOrdenCompraDto insumo)
        {
            if (insumo != null)
            {
                lblNombreInsumo.Content = insumo.Nombre;
                lblCodigoInsumo.Content = insumo.Codigo;
                lblCosto.Content = insumo.CostoUnitario.ToString();
                lblUnidadMedida.Content = insumo.UnidadMedida;
                lblSubtotal.Content = insumo.CostoUnitario.ToString();
                tbxCantidad.Text = "1";
            }
        }

    }
}
