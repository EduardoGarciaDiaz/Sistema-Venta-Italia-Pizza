using ItaliaPizza_Cliente.ServicioItaliaPizza;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Lógica de interacción para ElementoInsumoRegistroPagoOrden.xaml
    /// </summary>
    public partial class ElementoInsumoRegistroPagoOrden : UserControl
    {
        public event EventHandler TextChanged;
        public InsumoOrdenCompraDto Insumo { get; set; }

        public ElementoInsumoRegistroPagoOrden()
        {
            InitializeComponent();
        }

        private void TbxCantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, e);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            double result;
            TextBox textBox = sender as TextBox;
            string fullText = textBox.Text.Insert(textBox.SelectionStart, e.Text);

            bool isDecimal = Double.TryParse(fullText, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
            if (!isDecimal || fullText.Equals(""))
            {
                e.Handled = true;
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbxCantidadInsumo.Text))
            {
                tbxCantidadInsumo.Text = "0";
            }
        }
    }
}
