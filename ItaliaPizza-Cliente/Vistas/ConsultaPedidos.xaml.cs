using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
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

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para ConsultaPedidos.xaml
    /// </summary>
    public partial class ConsultaPedidos : Page
    {
        private List<PedidoConsultaDTO> _pedidos;
        public ConsultaPedidos()
        {
            InitializeComponent();
            this.BqdClientes.Placeholder.Text = "Ingresa nombre de cliente...";
            this.DpkFechaBusqueda.SelectedDate = DateTime.Now;

            ServicioPedidosClient servicioPedidosClient = new ServicioPedidosClient();
            _pedidos = servicioPedidosClient.RecuperarPedidos().ToList();
            MostrarPedidos(_pedidos);
        }

        private void MostrarPedidos(List<PedidoConsultaDTO> pedidos)
        {
            if (pedidos != null)
            {
                foreach (var pedido in pedidos)
                {
                    ElementoConsultaPedido elementoConsultaPedido = new ElementoConsultaPedido();
                    elementoConsultaPedido.LblNumeroPedido.Content = pedido.NumeroPedido;
                    elementoConsultaPedido.LblNombreCliente.Content = pedido.NombreCliente;
                    elementoConsultaPedido.LblCantidadProductos.Content = pedido.CantidadProductos + " productos.";
                    elementoConsultaPedido.LblFecha.Content = pedido.Fecha.ToShortDateString();
                    elementoConsultaPedido.LblTotalPedido.Content = "$" + pedido.Total.ToString("F2");
                    elementoConsultaPedido.LblEstadoPedido.Content = pedido.estadoPedido.Nombre;
                    elementoConsultaPedido.Click += ElementoConsultaPedidoClick;

                    SkpContenedorPedidos.Children.Add(elementoConsultaPedido);
                }
            }
        }

        private void ElementoConsultaPedidoClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
