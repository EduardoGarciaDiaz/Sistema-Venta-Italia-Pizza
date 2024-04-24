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
    /// Interaction logic for ElementoInsumoOrdenCompra.xaml
    /// </summary>
    public partial class ElementoInsumoOrdenCompra : UserControl
    {
        public EventHandler BtnAgregarAOrdenClicked;
        public InsumoOrdenCompraDto Insumo { get; set; }

        public ElementoInsumoOrdenCompra(InsumoOrdenCompraDto insumo)
        {
            InitializeComponent();
            InstanciarInsumo(insumo);
            this.Insumo = insumo;
        }
      
        private void BtnAgregarAOrden_Click(object sender, RoutedEventArgs e)
        {
            BtnAgregarAOrdenClicked?.Invoke(this, e);
        }

        private void InstanciarInsumo(InsumoOrdenCompraDto insumo)
        {
            if (insumo != null)
            {
                lblNombreInsumo.Content = insumo.Nombre;
                lblCodigoInsumo.Content = insumo.Codigo;
                lblCosto.Content = insumo.CostoUnitario;
                lblExistencia.Content = insumo.Existencia;
                lblUnidadMedida.Content = insumo.UnidadMedida;
            }
        }
    }
}
