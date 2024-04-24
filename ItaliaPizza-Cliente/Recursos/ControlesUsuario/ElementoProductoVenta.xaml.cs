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
    /// Lógica de interacción para ElementoProductoVenta.xaml
    /// </summary>
    public partial class ElementoProductoVenta : UserControl
    {
        public event RoutedEventHandler ProdcutoVentaClicked;

        public ElementoProductoVenta()
        {
            InitializeComponent();
        }

        private void UcProductoVenta_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ProdcutoVentaClicked?.Invoke(this, new RoutedEventArgs());
        }
    }
}
