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
    /// Lógica de interacción para ElementoConsultaOrdenCompra.xaml
    /// </summary>
    public partial class ElementoConsultaOrdenCompra : UserControl
    {
        public event RoutedEventHandler Click;
        public OrdenDeCompraDto OrdenDeCompraDto { get; set; }

        public ElementoConsultaOrdenCompra()
        {
            InitializeComponent();
        }

        private void BtnAccionOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
