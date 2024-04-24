using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para BarraDeBusquedaConLista.xaml
    /// </summary>
    public partial class BarraDeBusquedaConLista : UserControl
    {

        public EventHandler TxtBusquedaTextChanged;
        public EventHandler ImgBuscarClicked;
        public EventHandler ListaSelectionChanged;

        public BarraDeBusquedaConLista()
        {
            InitializeComponent();
        }

        private void ListaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListaSelectionChanged?.Invoke(this, e);
        }

        private void ImgBuscar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ImgBuscarClicked?.Invoke(this, e);
        }

        private void TxtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            TxtBusquedaTextChanged?.Invoke(this, e);
        }
    }

}
