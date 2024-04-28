using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
    /// Interaction logic for RegistroOrdenCompra.xaml
    /// </summary>
    public partial class RegistroOrdenCompra : Page
    {
        private List<ProveedorDto> _proveedores = new List<ProveedorDto>();
        private List<InsumoOrdenCompraDto> _insumosDisponibles = new List<InsumoOrdenCompraDto>();
        private readonly List<InsumoOrdenCompraDto> _insumosSeleccionado = new List<InsumoOrdenCompraDto>();
        private readonly List<ElementoInsumoOrdenCompraSeleccionado> _listaElementosEnOrdenCompra = new List<ElementoInsumoOrdenCompraSeleccionado>();
        private readonly List<ElementoInsumoOrdenCompra> _listaElementoEnListaInsumos = new List<ElementoInsumoOrdenCompra>();
       
        public RegistroOrdenCompra()
        {
            InitializeComponent();
            this.Loaded += PrepararWindow;
        }

        private void PrepararWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                ObtenerInformacion();
                CargarProveedores(_proveedores);
                MostrarInsumosdDisponibles(_insumosDisponibles);
                barraBusquedaInsumo.ImgBuscarClicked += ImgBuscar_Click;
                barraBusquedaInsumo.plhrInstruccion.Text = "Buscar insumo por nombre o código...";
            }
            catch (EndpointNotFoundException ex)
            {
                ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
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

        private void BtnInsumoMenos_Click(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden = sender as ElementoInsumoOrdenCompraSeleccionado;
            DisminuirCantidad(elementoInsumoOrden);
        }

        private void BtnInsumoMas_Click(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden = sender as ElementoInsumoOrdenCompraSeleccionado;
            AumentarCantidad(elementoInsumoOrden);
        }

        private void BtnAgregarAOrden_Click(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompra elementoInsumo = sender as ElementoInsumoOrdenCompra;
            AgregarInsumoAOrdenDeCompra(elementoInsumo.Insumo);
            QuitarInsumosDeListaInsumos(elementoInsumo);
            CalcularCostoOrdenCompra(_listaElementosEnOrdenCompra);
        }

        private void BtnEliminarInsumoOrden_Click(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden = sender as ElementoInsumoOrdenCompraSeleccionado;
            AgregarInsumoAlistaInsumosDisponibles(elementoInsumoOrden.Insumo);
            QuitarInsumoDeOrdenDeCompra(elementoInsumoOrden);
            CalcularCostoOrdenCompra(_listaElementosEnOrdenCompra);
        }

        private void ImgBuscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = barraBusquedaInsumo.txbBusqueda.Text;
            skpListaInsumos.Children.Clear();
            List<ElementoInsumoOrdenCompra> insumosFiltrados = _listaElementoEnListaInsumos.Where(insumo => insumo.Insumo.Codigo.Contains(criterioBusqueda) ||
                                                                                                  insumo.Insumo.Nombre.Contains(criterioBusqueda)).ToList();
            MostrarInsumosFiltrados(insumosFiltrados);
        }

        private void BtnRegistrarProveedor_Click(object sender, RoutedEventArgs e)
        {
            RegistroProveedor paginaRegistroProveedor = new RegistroProveedor(false);
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
            ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaRegistroProveedor);
        }

        private void BtnLimpiarOrden_Click(object sender, MouseButtonEventArgs e)
        {
            var listaCopia = _listaElementosEnOrdenCompra.ToList();
            foreach (var item in listaCopia)
            {
                AgregarInsumoAlistaInsumosDisponibles(item.Insumo);
                QuitarInsumoDeOrdenDeCompra(item);
            }
            skpInsumosOrdenCompra.Children.Clear();
            CalcularCostoOrdenCompra(_listaElementosEnOrdenCompra);
        }

        private void LbxProveedores_Selected(object sender, SelectionChangedEventArgs e)
        {
            ProveedorDto proveedorSeleccionado = (ProveedorDto)lbxProveedores.SelectedItem;
            if (proveedorSeleccionado != null)
            {
                MostrarInformacionProveedor(proveedorSeleccionado);
            }
        }

        private void BtnEnviarOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCantidades(_listaElementosEnOrdenCompra))
            {
                if (ValidarProveedorSeleccionado())
                {
                    if (!ValidarOrdenCompra(_listaElementosEnOrdenCompra))
                    {
                        try
                        {
                            int idOrdenCompra = GuardarOrdenDeCompra(_listaElementosEnOrdenCompra);
                            if (idOrdenCompra != 0)
                            {

                                if (EnviarOrdenDeCompra(idOrdenCompra))
                                {
                                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Exito!!", "La orden de compra se envio correctamente", Window.GetWindow(this), 2);
                                    ventanaEmergente.ShowDialog();
                                }
                                else
                                {
                                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Upss!!", "La orden de compra se guardo correctamente pero hubo un problema al enviarla, intentelos mas tarde", Window.GetWindow(this), 1);
                                    ventanaEmergente.ShowDialog();
                                }
                                SalirAPantallaInicio();
                            }
                            else
                            {
                                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "Ocurrio un error al guardar la orden de compra, intentelo mas tarde y verifique su conexión", Window.GetWindow(this), 1);
                                ventanaEmergente.ShowDialog();
                            }
                        }
                        catch (EndpointNotFoundException ex)
                        {
                            ManejadorVentanasEmergentes.MostrarVentanaErrorConexionFallida();
                            ManejadorExcepcion.ManejarExcepcionError(ex, Window.GetWindow(this));
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
                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "Debes Seleccionar un proveedor para guardar la orden de compra", Window.GetWindow(this), 1);
                    ventanaEmergente.ShowDialog();
                }
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "No puede haber cantidades vacias", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }
        }

        private void BtnGuardarOrden_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidarProveedorSeleccionado())
                {
                    int idOrdenCompra = GuardarOrdenDeCompra(_listaElementosEnOrdenCompra);
                    if (idOrdenCompra != 0)
                    {
                        VentanaEmergente ventanaEmergente = new VentanaEmergente("Exito!!", "La orden de compra se guardo correctamente.", Window.GetWindow(this), 2);
                        ventanaEmergente.ShowDialog();
                        SalirAPantallaInicio();
                    }
                    else
                    {
                        VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "Ocurrio un error al guardar la orden de compra, intentelo mas tarde y verifique su conexión", Window.GetWindow(this), 1);
                        ventanaEmergente.ShowDialog();
                    }

                }
                else
                {
                    VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "Debes Seleccionar un proveedor para guardar la orden de compra", Window.GetWindow(this), 1);
                    ventanaEmergente.ShowDialog();
                }
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

        private void TxbCantidad_TextChanged(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden = sender as ElementoInsumoOrdenCompraSeleccionado;
            if (!String.IsNullOrEmpty(elementoInsumoOrden.tbxCantidad.Text))
            {
                int cantidad = int.Parse(elementoInsumoOrden.tbxCantidad.Text);
                if (cantidad > 0)
                {
                    float costo = elementoInsumoOrden.Insumo.CostoUnitario;
                    float subtotoal = cantidad * costo;
                    elementoInsumoOrden.lblSubtotal.Content = subtotoal.ToString();
                }
                else
                {
                    AgregarInsumoAlistaInsumosDisponibles(elementoInsumoOrden.Insumo);
                    QuitarInsumoDeOrdenDeCompra(elementoInsumoOrden);
                }
                CalcularCostoOrdenCompra(_listaElementosEnOrdenCompra);
            }
        }

        private void BtnCancelarOrden_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensajeConfirmacion();
        }

        private void MostrarInsumosFiltrados(List<ElementoInsumoOrdenCompra> insumosFiltrados)
        {
            foreach (var item in insumosFiltrados)
            {
                skpListaInsumos.Children.Add(item);
            }
        }

        private void ObtenerInformacion()
        {
            ObtenerProveedoresActivos();
            ObtenerInsumoActivos();
        }

        private void ObtenerProveedoresActivos()
        {
            ServicioProveedoresClient servicioProveedoresClient = new ServicioProveedoresClient();
            _proveedores = servicioProveedoresClient.RecuperarProveedoresActivos().ToList();
        }

        private void ObtenerInsumoActivos()
        {
            ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
            _insumosDisponibles = servicioProductosClient.RecuperarInsumosActivos().ToList();
        }

        private void CargarProveedores(List<ProveedorDto> proveedoresACargar)
        {
            if(proveedoresACargar.Count > 0)
            {
                lbxProveedores.ItemsSource = proveedoresACargar;
                lbxProveedores.DisplayMemberPath = "NombreCompleto";
            }
        }

        private void MostrarInformacionProveedor(ProveedorDto proveedorSeleccionado)
        {
            lblNombreProveedor.Content = proveedorSeleccionado.NombreCompleto;
            lblRFCProveedor.Content = proveedorSeleccionado.RFC;
            lblTelefonoProveedor.Content = proveedorSeleccionado.NumeroTelefono;
            lblCorreo.Content = proveedorSeleccionado.CorreoElectronico;
            String direccion = proveedorSeleccionado.Direccion.Ciudad + ", Col. " + proveedorSeleccionado.Direccion.Colonia.ToString() + " " + proveedorSeleccionado.Direccion.CodigoPostal + ", Calle " + proveedorSeleccionado.Direccion.Calle + " #" + proveedorSeleccionado.Direccion.Numero.ToString();
            lblDireccion.Content = direccion;
        }

        private void MostrarInsumosdDisponibles(List<InsumoOrdenCompraDto> insumosDisponibles)
        {
            skpListaInsumos.Children.Clear();
            foreach (var item in insumosDisponibles)
            {
                ElementoInsumoOrdenCompra elementoInsumoOrden = new ElementoInsumoOrdenCompra(item);
                elementoInsumoOrden.BtnAgregarAOrdenClicked += BtnAgregarAOrden_Click;
                skpListaInsumos.Children.Add(elementoInsumoOrden);
                _listaElementoEnListaInsumos.Add(elementoInsumoOrden);
            }
        }

        
        private void AgregarInsumoAOrdenDeCompra(InsumoOrdenCompraDto insumo)
        {
            if (insumo != null)
            {
                ElementoInsumoOrdenCompraSeleccionado elementoInsumo = new ElementoInsumoOrdenCompraSeleccionado(insumo);
                elementoInsumo.BtnInsumoMasClicked += BtnInsumoMas_Click;
                elementoInsumo.BtnInsumoMenosClicked += BtnInsumoMenos_Click;
                elementoInsumo.BtnEliminarInsumoClicked += BtnEliminarInsumoOrden_Click;
                elementoInsumo.TxbCantidaTextChanged += TxbCantidad_TextChanged;
                skpInsumosOrdenCompra.Children.Add(elementoInsumo);
                _insumosSeleccionado.Add(elementoInsumo.Insumo);
                _listaElementosEnOrdenCompra.Add(elementoInsumo);
            }
        }

        private void QuitarInsumosDeListaInsumos(ElementoInsumoOrdenCompra elementoInsumo)
        {
            if (elementoInsumo != null)
            {
                skpListaInsumos.Children.Remove(elementoInsumo);
                _insumosDisponibles.Remove(elementoInsumo.Insumo);
                _listaElementoEnListaInsumos.Remove(elementoInsumo);
            }
        }      
        
        private void DisminuirCantidad(ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden)
        {
            int cantidad = int.Parse(elementoInsumoOrden.tbxCantidad.Text);
            cantidad--;
            elementoInsumoOrden.tbxCantidad.Text = cantidad.ToString();
        }


        private void AumentarCantidad(ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden)
        {
            int cantidad = int.Parse(elementoInsumoOrden.tbxCantidad.Text);
            cantidad++;
            elementoInsumoOrden.tbxCantidad.Text = cantidad.ToString();
        }

        private void AgregarInsumoAlistaInsumosDisponibles(InsumoOrdenCompraDto elementoInsumoOrden)
        {
            if (elementoInsumoOrden != null)
            {
                ElementoInsumoOrdenCompra elementoInsumo = new ElementoInsumoOrdenCompra(elementoInsumoOrden);
                elementoInsumo.BtnAgregarAOrdenClicked += BtnAgregarAOrden_Click;
                skpListaInsumos.Children.Add(elementoInsumo);
                _insumosDisponibles.Add(elementoInsumo.Insumo);
                _listaElementoEnListaInsumos.Add(elementoInsumo);
            }
        }

        private void QuitarInsumoDeOrdenDeCompra(ElementoInsumoOrdenCompraSeleccionado elementoInsumo)
        {
            skpInsumosOrdenCompra.Children.Remove(elementoInsumo);
            _insumosSeleccionado.Remove(elementoInsumo.Insumo);
            _listaElementosEnOrdenCompra.Remove(elementoInsumo);
        }

        private void CalcularCostoOrdenCompra(List<ElementoInsumoOrdenCompraSeleccionado> insumosEnOrdenDeCompra)
        {
            if (insumosEnOrdenDeCompra != null)
            {
                float total = 0;
                foreach (var item in insumosEnOrdenDeCompra)
                {
                    int cantidad = int.Parse(item.tbxCantidad.Text);
                    float costo = item.Insumo.CostoUnitario;
                    float subtotoal = cantidad * costo;
                    total += subtotoal;
                }
                lblTotalProducto.Content = _insumosSeleccionado.Count();
                lblSubtotal.Content = total.ToString("0.##");
                lblIva.Content = (total * 0.16).ToString("0.##");
                lblTotalCosto.Content = (total * 1.16).ToString("0.##");
            }
        } 

        private bool ValidarProveedorSeleccionado()
        {
            return lbxProveedores.SelectedItem != null;
        }

        private bool ValidarOrdenCompra(List<ElementoInsumoOrdenCompraSeleccionado> itemsDeOrdenCompra)
        {
            bool ordenVacia = true;
            if (itemsDeOrdenCompra.Count > 0)
            {
                ordenVacia = false;
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "No se puede enviar una orden de compra vacía, agregue al menos un producto", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }
            return ordenVacia;
        }

        private int GuardarOrdenDeCompra(List<ElementoInsumoOrdenCompraSeleccionado> itemsDeOrdenCompra)
        {
            int idOrdenCompra = 0;
            List<ElementoOrdenCompraDto> listaInsumosOrden = new List<ElementoOrdenCompraDto>();
            foreach (var item in itemsDeOrdenCompra)
            {
                ElementoOrdenCompraDto elementoOrdenCompraDto = new ElementoOrdenCompraDto()
                {
                    IdElementoOrdenCompra = 0,
                    InsumoOrdenCompraDto = item.Insumo,
                    CantidadInsumosAdquiridos = int.Parse(item.tbxCantidad.Text)
                };
                listaInsumosOrden.Add(elementoOrdenCompraDto);
            }
            OrdenDeCompraDto ordenDeCompraDto = new OrdenDeCompraDto()
            {
                IdOrdenCompra = 0,
                IdEstadoOrdenCompra = 0,
                Fecha = DateTime.Now,                
                Costo = float.Parse(lblTotalCosto.Content.ToString()),
                Proveedor = _proveedores.FirstOrDefault(pro => pro.RFC.Equals(lblRFCProveedor.Content)),
                IdProveedor = _proveedores.FirstOrDefault(pro => pro.RFC.Equals(lblRFCProveedor.Content)).IdProveedor,
                ListaElementosOrdenCompra = listaInsumosOrden.ToArray()
            };
            ServicioOrdenesCompraClient servicioOrdenesCompraClient = new ServicioOrdenesCompraClient();
            idOrdenCompra = servicioOrdenesCompraClient.GuardarOrdenDeCompraNueva(ordenDeCompraDto);
            return idOrdenCompra;
        }

        private bool ValidarCantidades(List<ElementoInsumoOrdenCompraSeleccionado> itemsDeOrdenCompra)
        {
            foreach (var item in itemsDeOrdenCompra)
            {
                if (String.IsNullOrEmpty(item.tbxCantidad.Text))
                {
                    return false;
                }
            }
            return true;
        }

        private bool EnviarOrdenDeCompra(int idOrdenCompra)
        {
            ServicioOrdenesCompraClient servicioOrdenesCompraClient = new ServicioOrdenesCompraClient();
            return servicioOrdenesCompraClient.EnviarOrdenDeCompra(idOrdenCompra);
        }

        private void MostrarMensajeConfirmacion()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("Cuidado!!!", "¿Seguro que desea cancelar el registro?, se perderán los datos que no se hayan guardado", "Si, Cancelar Registro", "No, Cancelar Accion", Window.GetWindow(this), 3);
            ventanaEmergente.ShowDialog();
            if (ventanaEmergente.AceptarAccion)
            {
                skpInsumosOrdenCompra.Children.Clear();
                skpListaInsumos.Children.Clear();
                _listaElementosEnOrdenCompra.Clear();
                SalirAPantallaInicio();
            }
        }       

        private void SalirAPantallaInicio()
        {
            ConsultaOrdenesDeCompra paginaOrdenesCompras = new ConsultaOrdenesDeCompra();
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
            ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaOrdenesCompras);
        }

    }
}
