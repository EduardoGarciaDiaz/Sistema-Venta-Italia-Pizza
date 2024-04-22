﻿using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
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
    /// Interaction logic for ConsultaOrdenesDeCompra.xaml
    /// </summary>
    public partial class ConsultaOrdenesDeCompra : Page
    {
        private List<ProveedorDto> _proveedores = new List<ProveedorDto>(); 
        private List<OrdenDeCompraDto> _ordenesCompra = new List<OrdenDeCompraDto>();

        private SolidColorBrush _colorBrushAmarillo = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6B400"));
        private SolidColorBrush _colorBrushNegro = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00000000"));
        private SolidColorBrush _colorBrushGris = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF656565"));
        private int _estadoSeleccionado = 0;
        private ProveedorDto _proveedorSeleccionado;
        private DateTime _fechaSeleccionada = DateTime.Now;

        public ConsultaOrdenesDeCompra()
        {
            InitializeComponent();
            this.Loaded += PrepararDatos_Loaded;
        }

        private void BtnVerOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            ElementoConsultaOrdenCompra elementoConsultaOrdenCompra = sender as ElementoConsultaOrdenCompra;
            OrdenDeCompraDto orden = elementoConsultaOrdenCompra.OrdenDeCompraDto;
            //Accion
        }

        private void BtnModificarOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            ElementoConsultaOrdenCompra elementoConsultaOrdenCompra = sender as ElementoConsultaOrdenCompra;
            OrdenDeCompraDto orden = elementoConsultaOrdenCompra.OrdenDeCompraDto;
            //Accion
        }

        private void BtnRegistrarPagoOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            ElementoConsultaOrdenCompra elementoConsultaOrdenCompra = sender as ElementoConsultaOrdenCompra;
            OrdenDeCompraDto orden = elementoConsultaOrdenCompra.OrdenDeCompraDto;
            RegistroPagoOrdenCompra registroPagoOrdenCompra = new RegistroPagoOrdenCompra(orden);
            NavigationService.Navigate(registroPagoOrdenCompra);
        }

        private void PrepararDatos_Loaded(object sender, RoutedEventArgs e)
        {
            _proveedores = RecuperarProveedores();
            CargarProveedores(_proveedores);
            _ordenesCompra = RecuperarOrdenesOrdenesCompra();
            DpkFechaBusqueda.SelectedDate = DateTime.Now;
            MostrarOrdenesCompra((_ordenesCompra.Where(o => o.Fecha.Date == _fechaSeleccionada.Date).ToList()));
        }

        private void MostrarOrdenesCompra(List<OrdenDeCompraDto> ordenesCompra)
        {
            lblMensajeSinResultados.Visibility = Visibility.Collapsed;
            SkpContenedorOrdenesCompra.Children.Clear();
            ordenesCompra?.ForEach(ordenCompra =>
            {
                var elementoConsultaOrdenCompra = new ElementoConsultaOrdenCompra
                {
                    lblNumeroOrden = { Content = ordenCompra.IdOrdenCompra },
                    lblNombreProveedor = { Content = ordenCompra.Proveedor.NombreCompleto },
                    lblCantidadInsumosSolicitados = { Content = $"{ordenCompra.listaElementosOrdenCompra.Length} productos." },
                    LblFecha = { Content = ordenCompra.Fecha.ToShortDateString() },
                    LblTotalOrdenCompra = { Content = $"${ordenCompra.listaElementosOrdenCompra.Sum(p => (p.CantidadInsumosAdquiridos * p.InsumoOrdenCompraDto.CostoUnitario)):F2}" },
                    OrdenDeCompraDto = ordenCompra
                };
                CambiarColorBotonOrdenCompra(ordenCompra.IdEstadoOrdenCompra, elementoConsultaOrdenCompra.btnAccionOrdenCompra);
                AgregarListenerBotonOrdenCompra(ordenCompra.IdEstadoOrdenCompra, elementoConsultaOrdenCompra);

                SkpContenedorOrdenesCompra.Children.Add(elementoConsultaOrdenCompra);
            });
        }

        private void AgregarListenerBotonOrdenCompra(int idEstadoOrdenCompra, ElementoConsultaOrdenCompra elementoConsultaOrdenCompra)
        {
            switch (idEstadoOrdenCompra)
            {
                case (int)EnumEstadosOrdenCompra.Enviada:
                    elementoConsultaOrdenCompra.Click += BtnRegistrarPagoOrdenCompra_Click;
                    break;

                case (int)EnumEstadosOrdenCompra.Borrador:
                    elementoConsultaOrdenCompra.Click += BtnModificarOrdenCompra_Click;
                    break;

                case (int)EnumEstadosOrdenCompra.Surtida:
                    elementoConsultaOrdenCompra.Click += BtnVerOrdenCompra_Click;
                    break;

                default:
                    break;
            }
        }

        private void CambiarColorBotonOrdenCompra(int idEstadoOrdenCompra, Button btnAccionOrdenCompra)
        {
            switch (idEstadoOrdenCompra)
            {
                case (int)EnumEstadosOrdenCompra.Enviada:
                    btnAccionOrdenCompra.Content = "Registrar pago";
                    btnAccionOrdenCompra.Background = _colorBrushAmarillo;
                    btnAccionOrdenCompra.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case (int)EnumEstadosOrdenCompra.Borrador:
                    btnAccionOrdenCompra.Content = "Modificar";
                    btnAccionOrdenCompra.Background = new SolidColorBrush(Colors.Black);
                    btnAccionOrdenCompra.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case (int)EnumEstadosOrdenCompra.Surtida:
                    btnAccionOrdenCompra.Content = "Ver";
                    btnAccionOrdenCompra.Background = _colorBrushGris;
                    btnAccionOrdenCompra.Foreground = new SolidColorBrush(Colors.White);
                    break;

                default:
                    break;
            }
        }

        private void BtnRegistrarOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            RegistroOrdenCompra paginaRegistrarOrdenCompra = new RegistroOrdenCompra();
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
            ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaRegistrarOrdenCompra);
        }

        private List<ProveedorDto> RecuperarProveedores()
        {
            List<ProveedorDto> proveedores = new List<ProveedorDto>();
            proveedores.Add(new ProveedorDto() { NombreCompleto = "Todos", IdProveedor = 0});
            ServicioProveedoresClient servicioProveedoresCliente = new ServicioProveedoresClient();
            proveedores.AddRange(servicioProveedoresCliente.RecuperarProveedores().ToList());
            return proveedores;
        }

        private List<OrdenDeCompraDto> RecuperarOrdenesOrdenesCompra()
        {
            ServicioOrdenesCompraClient servicioOrdenesCompraCliente = new ServicioOrdenesCompraClient();
            return servicioOrdenesCompraCliente.RecuperarOrdenesDeCompra().ToList();
        }

        private void CargarProveedores(List<ProveedorDto> proveedores)
        {
            cbxProveedores.ItemsSource = proveedores;
            cbxProveedores.SelectedItem = proveedores.FirstOrDefault();
            _proveedorSeleccionado = cbxProveedores.SelectedItem as ProveedorDto;
        }

        private void Combo_ItemSeleccionadoChanged(object sender, SelectionChangedEventArgs e)
        {
            _proveedorSeleccionado = cbxProveedores.SelectedItem as ProveedorDto;
            FiltrarOrdenesCompra(_proveedorSeleccionado, _estadoSeleccionado, _fechaSeleccionada);
        }

        private void FiltrarOrdenesCompra(ProveedorDto proveedor, int estadoSeleccionado, DateTime fecha)
        {
            List<OrdenDeCompraDto> ordenDeCompraDtos = new List<OrdenDeCompraDto>();
            if (proveedor.IdProveedor == 0 && estadoSeleccionado == 0)
            {
                ordenDeCompraDtos = _ordenesCompra.Where(o => o.Fecha.Date == fecha.Date).ToList();
            } else if (proveedor.IdProveedor == 0)
            {
                ordenDeCompraDtos = _ordenesCompra.Where(o =>
                    o.IdEstadoOrdenCompra == estadoSeleccionado
                    && o.Fecha.Date == _fechaSeleccionada.Date)
                    .ToList();
            } else if (estadoSeleccionado == 0)
            {
                ordenDeCompraDtos = _ordenesCompra.Where(o => 
                    o.IdProveedor == proveedor.IdProveedor
                    && o.Fecha.Date == _fechaSeleccionada.Date)
                    .ToList();
            } else
            {
                ordenDeCompraDtos = _ordenesCompra.Where(o =>
                o.IdProveedor == proveedor.IdProveedor
                && o.IdEstadoOrdenCompra == estadoSeleccionado
                && o.Fecha.Date == _fechaSeleccionada.Date).ToList();
            }
            if (ordenDeCompraDtos.Count > 0)
            {
                MostrarOrdenesCompra(ordenDeCompraDtos);
            } 
            else
            {
                SkpContenedorOrdenesCompra.Children.Clear();
                MostrarMensaje("No existen ordenes para el proveedor o estado seleccionados");
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            lblMensajeSinResultados.Content = mensaje;
            lblMensajeSinResultados.Visibility = Visibility.Visible;
        }

        private void LblTodasOrdenesCompra_Click(object sender, MouseButtonEventArgs e)
        {
            _estadoSeleccionado = 0;
            FiltrarOrdenesCompra(_proveedorSeleccionado, _estadoSeleccionado, _fechaSeleccionada);
        }

        private void LblOrdenesCompraEnviadas_Click(object sender, MouseButtonEventArgs e)
        {
            _estadoSeleccionado = (int)EnumEstadosOrdenCompra.Enviada;
            FiltrarOrdenesCompra(_proveedorSeleccionado, _estadoSeleccionado, _fechaSeleccionada);
        }

        private void LblOrdenesCompraBorradores_Click(object sender, MouseButtonEventArgs e)
        {
            _estadoSeleccionado = (int)EnumEstadosOrdenCompra.Borrador;
            FiltrarOrdenesCompra(_proveedorSeleccionado, _estadoSeleccionado, _fechaSeleccionada);
        }

        private void LblOrdenesCompraSurtidas_Click(object sender, MouseButtonEventArgs e)
        {
            _estadoSeleccionado = (int)EnumEstadosOrdenCompra.Surtida;
            FiltrarOrdenesCompra(_proveedorSeleccionado, _estadoSeleccionado, _fechaSeleccionada);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _fechaSeleccionada = (DateTime) DpkFechaBusqueda.SelectedDate;
            FiltrarOrdenesCompra(_proveedorSeleccionado, _estadoSeleccionado, _fechaSeleccionada);
        }
    }
}
