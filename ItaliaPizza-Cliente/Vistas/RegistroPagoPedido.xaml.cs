using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para RegistroPagoPedido.xaml
    /// </summary>
    public partial class RegistroPagoPedido : Page
    {

        private static readonly Regex _regex = new Regex("[^0-9.-]+");

        public RegistroPagoPedido()
        {
            InitializeComponent();
        }

        private void TxtCantidadPagaCliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsTextoPermitido(e.Text);
        }

        private bool EsTextoPermitido(string texto)
        {
            return !_regex.IsMatch(texto);
        }
    }
}
