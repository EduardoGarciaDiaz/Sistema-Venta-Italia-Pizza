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
    /// Lógica de interacción para TarjetaOrdenCompra.xaml
    /// </summary>
    public partial class TarjetaOrdenCompra : UserControl
    {
        public event EventHandler ClickCerrar;

        public TarjetaOrdenCompra()
        {
            InitializeComponent();
        }

        private void ImgCerrar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClickCerrar?.Invoke(this, e);
        }
    }
}
