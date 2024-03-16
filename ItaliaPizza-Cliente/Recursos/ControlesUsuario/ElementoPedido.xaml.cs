using ItaliaPizza_Cliente.ServicioItaliaPizza;
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

namespace ItaliaPizza_Cliente.Recursos.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para ElementoPedido.xaml
    /// </summary>
    public partial class ElementoPedido : UserControl
    {
        private static readonly Regex _regex = new Regex("[^0-9]+");
        public EventHandler TbxCantidadProductoTextChanged;
        public EventHandler BtnSumarClicked;
        public EventHandler BtnRestarClicked;
        public EventHandler TbxLostFocusTbxCantidad;

        public ProductoVentaPedidos ProductoVentaPedidos { get; set; }

        public ElementoPedido()
        {
            InitializeComponent();
        }

        private void TxtCantidadPagaCliente_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !EsTextoPermitido(TbxCantidadProducto.Text+e.Text);
        }

        private bool EsTextoPermitido(string texto)
        {
            return !_regex.IsMatch(texto) && texto.Length <= 4;
        }

        private void TbxCantidadProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            TbxCantidadProductoTextChanged?.Invoke(this, e);
        }

        private void BtnSumarCantidadProducto_Click(object sender, RoutedEventArgs e)
        {
            BtnSumarClicked?.Invoke(this, e);
        }

        private void BtnRestarCantidadProducto_Click(object sender, RoutedEventArgs e)
        {
            BtnRestarClicked?.Invoke(this, e);
        }

        private void TbxCantidadProducto_LostFocus(object sender, RoutedEventArgs e)
        {
            TbxLostFocusTbxCantidad.Invoke(this, e);
        }
    }
}
