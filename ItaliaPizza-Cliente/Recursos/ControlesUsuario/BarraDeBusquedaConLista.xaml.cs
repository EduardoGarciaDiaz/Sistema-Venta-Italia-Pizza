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
        ObservableCollection<Cliente> clientes;

        public BarraDeBusquedaConLista()
        {
            clientes = new ObservableCollection<Cliente>();
            InitializeComponent();
            this.DataContext = clientes;
        }

        public void AgregarClienteALista(Cliente cliente)
        {
            this.clientes.Add(cliente);
        }

        private void ListaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Cliente clienteSeleccionado = ListaClientes.SelectedItem as Cliente;
            ListaClientes.Visibility = Visibility.Hidden;
            MessageBox.Show(clienteSeleccionado.Nombre);
        }

        private void TxtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtBusqueda.Text.ToString()))
            {
                ListaClientes.Visibility = Visibility.Hidden;
            } else
            {
                ListaClientes.Visibility = Visibility.Visible;
            }
        }
    }

    public class Cliente
    {
        public string Nombre { get; set; }
        public string CorreoElectronico { get; set; }

        public Cliente(string nombre, string correoElectronico)
        {
            Nombre = nombre;
            CorreoElectronico = correoElectronico;
        }
    }
}
