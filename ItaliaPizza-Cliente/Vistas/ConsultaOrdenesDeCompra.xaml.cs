﻿using Aspose.Pdf.Annotations;
using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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
        private SolidColorBrush COLOR_BRUSH_AMARILLO = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD6B400"));
        private SolidColorBrush COLOR_BRUSH_GRIS = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF656565"));

        private List<ProveedorDto> _proveedores = new List<ProveedorDto>(); 
        private List<OrdenDeCompraDto> _ordenesCompra = new List<OrdenDeCompraDto>();
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

            MostrarDatosOrdenCompra(orden);
            MostrarDatosProveedor(orden.Proveedor);
            MostrarInsumos(orden.ListaElementosOrdenCompra.ToList());
            ucOrdenCompra.Visibility = Visibility.Visible;
            brdFondo.Visibility = Visibility.Visible;
        }

        private void ImgCerrar_MouseLeftButtonDown(object sender, EventArgs e)
        {
            CerrarTarjeta();
        }

        private void BtnModificarOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            ElementoConsultaOrdenCompra elementoConsultaOrdenCompra = sender as ElementoConsultaOrdenCompra;
            OrdenDeCompraDto orden = elementoConsultaOrdenCompra.OrdenDeCompraDto;
            EdicionOrdenCompra paginaEdicionOrdenCompra = new EdicionOrdenCompra(orden);
            NavigationService.Navigate(paginaEdicionOrdenCompra);
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
            try
            {
                _proveedores = RecuperarProveedores();
                CargarProveedores(_proveedores);
                _ordenesCompra = RecuperarOrdenesOrdenesCompra();
                dpkFechaBusqueda.SelectedDate = DateTime.Now;
                MostrarOrdenesCompra((_ordenesCompra.Where(o => o.Fecha.Date == _fechaSeleccionada.Date).ToList()));
                ucOrdenCompra.ClickCerrar += ImgCerrar_MouseLeftButtonDown;
            }
            catch (EndpointNotFoundException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this) );
            }
            catch (TimeoutException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (FaultException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (CommunicationException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
            catch (Exception ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
            }
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
            _fechaSeleccionada = (DateTime)dpkFechaBusqueda.SelectedDate;
            FiltrarOrdenesCompra(_proveedorSeleccionado, _estadoSeleccionado, _fechaSeleccionada);
        }

        private void BtnRegistrarOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            RegistroOrdenCompra paginaRegistrarOrdenCompra = new RegistroOrdenCompra();
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
            ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaRegistrarOrdenCompra);
        }

        private void Combo_ItemSeleccionadoChanged(object sender, SelectionChangedEventArgs e)
        {
            _proveedorSeleccionado = cbxProveedores.SelectedItem as ProveedorDto;
            FiltrarOrdenesCompra(_proveedorSeleccionado, _estadoSeleccionado, _fechaSeleccionada);
        }

        private void MostrarOrdenesCompra(List<OrdenDeCompraDto> ordenesCompra)
        {
            lblMensajeSinResultados.Visibility = Visibility.Collapsed;
            skpContenedorOrdenesCompra.Children.Clear();
            ordenesCompra?.ForEach(ordenCompra =>
            {
                var elementoConsultaOrdenCompra = new ElementoConsultaOrdenCompra
                {
                    lblNumeroOrden = { Content = ordenCompra.IdOrdenCompra },
                    lblNombreProveedor = { Content = ordenCompra.Proveedor.NombreCompleto },
                    lblCantidadInsumosSolicitados = { Content = $"{ordenCompra.ListaElementosOrdenCompra.Length} productos." },
                    lblFecha = { Content = ordenCompra.Fecha.ToShortDateString() },
                    lblTotalOrdenCompra = { Content = $"${ordenCompra.Costo:F2}" },
                    OrdenDeCompraDto = ordenCompra
                };
                CambiarColorBotonOrdenCompra(ordenCompra.IdEstadoOrdenCompra, elementoConsultaOrdenCompra.btnAccionOrdenCompra);
                AgregarListenerBotonOrdenCompra(ordenCompra.IdEstadoOrdenCompra, elementoConsultaOrdenCompra);

                skpContenedorOrdenesCompra.Children.Add(elementoConsultaOrdenCompra);
            });
        }

        private void AgregarListenerBotonOrdenCompra(int idEstadoOrdenCompra, ElementoConsultaOrdenCompra elementoConsultaOrdenCompra)
        {
            switch (idEstadoOrdenCompra)
            {
                case (int)EnumEstadosOrdenCompra.Enviada:
                    elementoConsultaOrdenCompra.btnOrdenCompraClicked += BtnRegistrarPagoOrdenCompra_Click;
                    break;

                case (int)EnumEstadosOrdenCompra.Borrador:
                    elementoConsultaOrdenCompra.btnOrdenCompraClicked += BtnModificarOrdenCompra_Click;
                    break;

                case (int)EnumEstadosOrdenCompra.Surtida:
                    elementoConsultaOrdenCompra.btnOrdenCompraClicked += BtnVerOrdenCompra_Click;
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
                    btnAccionOrdenCompra.Background = COLOR_BRUSH_AMARILLO;
                    btnAccionOrdenCompra.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case (int)EnumEstadosOrdenCompra.Borrador:
                    btnAccionOrdenCompra.Content = "Modificar";
                    btnAccionOrdenCompra.Background = new SolidColorBrush(Colors.Black);
                    btnAccionOrdenCompra.Foreground = new SolidColorBrush(Colors.White);
                    break;

                case (int)EnumEstadosOrdenCompra.Surtida:
                    btnAccionOrdenCompra.Content = "Ver";
                    btnAccionOrdenCompra.Background = COLOR_BRUSH_GRIS;
                    btnAccionOrdenCompra.Foreground = new SolidColorBrush(Colors.White);
                    break;

                default:
                    break;
            }
        }

        private void MostrarInsumos(List<ElementoOrdenCompraDto> listaElementosOrdenCompra)
        {
            listaElementosOrdenCompra?.ForEach(i =>
            {
                InsumoOrdenCompraDto insumo = i.InsumoOrdenCompraDto;
                OrdenCompraInsumoItem insumoItem = new OrdenCompraInsumoItem()
                {
                    lblNombreInsumo = { Content = insumo.Nombre },
                    lblCodigoInsumo = { Content = insumo.Codigo },
                    lblNombreUnidad = { Content = insumo.UnidadMedida },
                    lblCosto = { Content = "$" + insumo.CostoUnitario.ToString("F2") },
                    lblCantidadSolicitada = { Content = i.CantidadInsumosAdquiridos }
                };
                ucOrdenCompra.skpContenedorInsumos.Children.Add(insumoItem);
            });
        }

        private void MostrarDatosProveedor(ProveedorDto proveedor)
        {
            ucOrdenCompra.lblNombreProveedor.Content = proveedor.NombreCompleto;
            ucOrdenCompra.lblCorreoElectronicoProveedor.Content = proveedor.CorreoElectronico;
            ucOrdenCompra.lblNumeroTelefonoProveedor.Content = proveedor.NumeroTelefono;
        }

        private void MostrarDatosOrdenCompra(OrdenDeCompraDto orden)
        {
            ucOrdenCompra.lblNumeroOrden.Content = orden.IdOrdenCompra.ToString();
            ucOrdenCompra.lblFechaOrdenCompra.Content = orden.Fecha.ToShortDateString();

            double total = orden.Costo;
            double subtotal = total / 1.16;
            double iva = subtotal * 0.16;

            ucOrdenCompra.lbSubtotal.Content = subtotal.ToString("F2");
            ucOrdenCompra.lbIVA.Content = iva.ToString("F2");
            ucOrdenCompra.lbTotal.Content = total.ToString("F2");

        }

        private List<ProveedorDto> RecuperarProveedores()
        {
            List<ProveedorDto> proveedores = new List<ProveedorDto>
            {
                new ProveedorDto() { NombreCompleto = "Todos", IdProveedor = 0 }
            };
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
            } 
            else
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
                skpContenedorOrdenesCompra.Children.Clear();
                MostrarMensaje("No existen órdenes para el proveedor o estado seleccionados");
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            lblMensajeSinResultados.Content = mensaje;
            lblMensajeSinResultados.Visibility = Visibility.Visible;
        }

        private void CerrarTarjeta()
        {
            brdFondo.Visibility = Visibility.Collapsed;
            ucOrdenCompra.Visibility = Visibility.Collapsed;
        }
    }
}
