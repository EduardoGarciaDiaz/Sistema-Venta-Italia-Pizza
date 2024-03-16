using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
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

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Lógica de interacción para RegistroPedido.xaml
    /// </summary>
    public partial class RegistroPedido : Page
    {

        private ObservableCollection<ClienteBusqueda> _clientes;
        private string MENSAJE_CAMPO_VACIO = "Por favor, ingresa algo en la barra para realizar la busqueda.";
        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_CLIENTE = "El nombre ingresado no corresponde a ningun cliente.";
        private string MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO = "El nombre ingresado no corresponde a ningun cliente.";
        private string MENSAJE_BUSQUEDA_OTRO_CLIENTE = "Cambiar cliente...";
        private TipoServicio _tipoServicioSeleccionado = new TipoServicio();
        private List<TipoServicio> _tiposServicio;
        SolidColorBrush _colorBrushAmarillo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6B400"));
        SolidColorBrush _colorBrushGris = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDDDDDD"));
        SolidColorBrush _colorBrushWhite = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
        SolidColorBrush _colorBrushGrisTexto = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF918C8C"));
        List<ProductoVentaPedidos> _productosVenta;
        Dictionary<string, int> _productosEnPedido = new Dictionary<string, int>();
        private double _total = 0;
        private double _subtotal = 0;
        private double _iva = 0;

        public RegistroPedido()
        {
            _clientes = new ObservableCollection<ClienteBusqueda>();
            InitializeComponent();

            this.BarraBusquedaClientes.DataContext = _clientes;
            this.BarraBusquedaClientes.ImgBuscar_EventHandler += ImgBuscarClienteClicked;
            this.BarraBusquedaClientes.TxtBusqueda_EventHandler += TxtBusquedaClienteChanged;
            this.BarraBusquedaClientes.Lista_SelectionChanged_EventHandler += ListaSelectionChanged;
            this.BarraBusquedaProductos.ImgBuscarClicked += ImgBuscarProductosClicked;

            List<Categoria> categoriasProductoVenta = new ServicioProductosClient().RecuperarCategoriasProductoVenta().ToList();
            MostrarCategoriasProductoVenta(categoriasProductoVenta);
            _productosVenta = new ServicioProductosClient().RecuperarProductosVenta().ToList();
            MostrarProductosVenta(_productosVenta);
            MostrarCantidades();

            _tiposServicio = new ServicioPedidosClient().RecuperarTiposServicio().ToList();

    }

        public void MostrarCategoriasProductoVenta(List<Categoria> categorias)
        {
            foreach (Categoria categoria in categorias)
            {
                Label label = new Label();
                label.Content = categoria.Nombre.ToString();
                label.FontSize = 18;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.HorizontalAlignment = HorizontalAlignment.Center;
                label.Padding = new Thickness(10);
                SkpCategoriasProductoVenta.Children.Add(label);
            }
        }

        public void MostrarProductosVenta(List<ProductoVentaPedidos> productosVenta)
        {
            List<ProductoVentaPedidos>.Enumerator iterator = productosVenta.GetEnumerator();
            iterator.MoveNext();
            for (int i = 0; i <= productosVenta.Count/4;  i++)
            {
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                for (int j = 0; j < 4; j++)
                {
                    ProductoVentaPedidos productoVenta = iterator.Current;
                    ElementoProductoVenta elementoProductoVenta = new ElementoProductoVenta();
                    elementoProductoVenta.LblNombreProducto.Content = productoVenta.Nombre;
                    elementoProductoVenta.LblCodigo.Content = productoVenta.Codigo;
                    elementoProductoVenta.LblDescripcionProducto.Text = productoVenta.Descripcion;
                    elementoProductoVenta.LblPrecioProducto.Content = "$" + productoVenta.Precio.ToString();
                    elementoProductoVenta.Click += ElementoProductoVenta_Click;

                    stackPanel.Children.Add(elementoProductoVenta);

                    iterator.MoveNext();
                    if (iterator.Current == null)
                    {
                        break;
                    }
                }
                this.SkpContenedorProductos.Children.Add(stackPanel);
            }
            iterator.Dispose();
        }

        private void ElementoProductoVenta_Click(object sender, RoutedEventArgs e)
        {
            bool productoDisponibleParaVenta;
            ElementoProductoVenta elementoProductoVenta = (ElementoProductoVenta)sender;
            string codigoProducto = elementoProductoVenta.LblCodigo.Content.ToString();
            if (_productosEnPedido.ContainsKey(codigoProducto))
            {
                int cantidadActualProductos = _productosEnPedido[codigoProducto];
                int cantidadNuevaRequeridaProductos = cantidadActualProductos++;
                productoDisponibleParaVenta = new ServicioProductosClient()
                    .ValidarDisponibilidadDeProducto(codigoProducto, cantidadNuevaRequeridaProductos);
                if (productoDisponibleParaVenta)
                {
                    _productosEnPedido[codigoProducto]++;
                    ActualizarEnInterfazCantidadRequeridaProductos(codigoProducto);
                    CalcularCantidades();
                    MostrarCantidades();
                }
            }
            else
            {
                productoDisponibleParaVenta = new ServicioProductosClient().ValidarDisponibilidadDeProducto(codigoProducto, 1);
                if (productoDisponibleParaVenta)
                {
                    ProductoVentaPedidos productoSeleccionado = _productosVenta.Where(producto => producto.Codigo == codigoProducto).FirstOrDefault();
                    _productosEnPedido.Add(productoSeleccionado.Codigo, 1);
                    MostrarDatosProductoSeleccionado(productoSeleccionado);
                    CalcularCantidades();
                    MostrarCantidades();
                }
            }
        }

        private void MostrarDatosProductoSeleccionado(ProductoVentaPedidos productoSeleccionado)
        {

            ElementoPedido elementoPedido = new ElementoPedido();
            elementoPedido.LblNombreProducto.Content = productoSeleccionado.Nombre;
            elementoPedido.LblDescripcionProducto.Content = productoSeleccionado.Descripcion;
            elementoPedido.LblPrecioProducto.Content = "$" + productoSeleccionado.Precio;
            elementoPedido.ProductoVentaPedidos = productoSeleccionado;
            elementoPedido.TbxCantidadProducto.Text = _productosEnPedido[productoSeleccionado.Codigo].ToString();

            SkpContenedorProductosPedido.Children.Add(elementoPedido);
        }

        private void ActualizarEnInterfazCantidadRequeridaProductos(string codigoProducto)
        {
            ElementoPedido elementoPedido = SkpContenedorProductosPedido.Children
                                                        .OfType<ElementoPedido>()
                                                        .FirstOrDefault(ep => ep.ProductoVentaPedidos.Codigo == codigoProducto);
            if (elementoPedido != null)
            {
                elementoPedido.TbxCantidadProducto.Text = _productosEnPedido[codigoProducto].ToString();
            }
        }

        private void CalcularCantidades()
        {
            _total = ObtenerTotal();
            _subtotal = (_total / 1.16);
            _iva = _total - _subtotal;
        }

        private void MostrarCantidades()
        {
            LblTotal.Content = _total.ToString("F2");
            LblSubtotal.Content = _subtotal.ToString("F2");
            LblIva.Content = _iva.ToString("F2");
        }

        private double ObtenerTotal()
        {   
            double total = 0;
            List<ProductoVentaPedidos> productoVentaPedidos = _productosVenta.Where(pv => _productosEnPedido.Keys.ToList().Any(k => k == pv.Codigo)).ToList();
            total = productoVentaPedidos.Sum(pv => (pv.Precio * _productosEnPedido[pv.Codigo]));
            return total;
        }

        public void ImgBuscarClienteClicked(object sender, EventArgs e)
        {
            string textoIngresado = this.BarraBusquedaClientes.TxtBusqueda.Text;
            if (!string.IsNullOrWhiteSpace(textoIngresado))
            {
                _clientes.Clear();
                List<ClienteBusqueda> clientesBusqueda = new ServicioItaliaPizza.ServicioUsuariosClient().BuscarCliente(this.BarraBusquedaClientes.TxtBusqueda.Text).ToList();
                if (!(clientesBusqueda.Count == 0))
                {
                    foreach (ClienteBusqueda clienteBusqueda in clientesBusqueda)
                    {
                        _clientes.Add(clienteBusqueda);
                    }
                    this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Visible;
                } 
                else
                {
                    LblMensajeAdvertenciaCliente.Content = MENSAJE_SIN_RESULTADOS_BUSQUEDA_CLIENTE;
                }
            } else
            {
                LblMensajeAdvertenciaCliente.Content = MENSAJE_CAMPO_VACIO;
            }
        }

        private void TxtBusquedaClienteChanged(object sender, EventArgs e)
        {
            this.LblMensajeAdvertenciaCliente.Content = "";
            this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Collapsed;
        }

        private void ListaSelectionChanged(Object sender, EventArgs e)
        {
            ClienteBusqueda cliente = BarraBusquedaClientes.ListaClientes.SelectedItem as ClienteBusqueda;
            if (cliente != null)
            {
                LblNombreCliente.Content = cliente.Nombre;
                LblCorreoElectronicoCliente.Content = cliente.Correo;
                this.BarraBusquedaClientes.ListaClientes.Visibility = Visibility.Collapsed;
                this.BarraBusquedaClientes.TxtBusqueda.Text = "";
                this.BarraBusquedaClientes.Placeholder.Text = MENSAJE_BUSQUEDA_OTRO_CLIENTE;
            }
        }

        private void ImgBuscarProductosClicked(object sender, EventArgs e)
        {
            string valorBusqueda = BarraBusquedaProductos.TxtBusqueda.Text.ToString();
            if (!string.IsNullOrWhiteSpace(valorBusqueda))
            {
                List<ProductoVentaPedidos> resultadoBusquedaProductos = new List<ProductoVentaPedidos>();
                resultadoBusquedaProductos = _productosVenta.Where(producto =>
                    producto.Nombre.ToLower().Contains(valorBusqueda.ToLower())
                    || producto.Codigo.ToLower().Contains(valorBusqueda.ToLower())
                ).ToList();
                if (resultadoBusquedaProductos.Count != 0)
                {
                    SkpContenedorProductos.Children.Clear();
                    MostrarProductosVenta(resultadoBusquedaProductos);
                } 
                else
                {
                    LblMensajeAdvertenciaProducto.Content = MENSAJE_SIN_RESULTADOS_BUSQUEDA_PRODUCTO;
                }
            } 
            else
            {
                LblMensajeAdvertenciaProducto.Content = MENSAJE_CAMPO_VACIO;
            }
        }

        private void BtnEntregaDomicilio_Click(object sender, RoutedEventArgs e)
        {
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EntregaDomicilio);
            BtnEntregaDomicilio.Background = _colorBrushAmarillo;
            BtnEntregaDomicilio.Foreground = _colorBrushWhite;
            BtnComerEstablecimiento.Background = _colorBrushGris;
            BtnComerEstablecimiento.Foreground = _colorBrushGrisTexto;
        }

        private void BtnComerEstablecimiento_Click(object sender, RoutedEventArgs e)
        {
            _tipoServicioSeleccionado = _tiposServicio.ElementAt((int)EnumTiposServicio.EnEstablecimiento);
            BtnEntregaDomicilio.Background = _colorBrushGris;
            BtnEntregaDomicilio.Foreground= _colorBrushGrisTexto;
            BtnComerEstablecimiento.Background = _colorBrushAmarillo;
            BtnComerEstablecimiento.Foreground= _colorBrushWhite;
        }
    }
}
