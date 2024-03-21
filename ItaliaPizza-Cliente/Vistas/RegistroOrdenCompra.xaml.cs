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
    /// Interaction logic for RegistroOrdenCompra.xaml
    /// </summary>
    public partial class RegistroOrdenCompra : Page
    {
        List<ProveedorDto> proveedores = new List<ProveedorDto>();
        List<InsumoOrdenCompraDto> insumosDisponibles = new List<InsumoOrdenCompraDto>();
        List<InsumoOrdenCompraDto> insumosSeleccionado = new List<InsumoOrdenCompraDto>();
        List<ElementoInsumoOrdenCompraSeleccionado> listaElementosEnOrdenCompra = new List<ElementoInsumoOrdenCompraSeleccionado>();
        List<ElementoInsumoOrdenCompra> listaElementoEnListaInsumos = new List<ElementoInsumoOrdenCompra>();
        public RegistroOrdenCompra()
        {
            InitializeComponent();
            PrepararWindow();
        }

        private void PrepararWindow()
        {
            ObtenerInformacion();
            CargarProveedores(proveedores);
            MostrarInsumosdDisponibles(insumosDisponibles);
            barraBusquedaInsumo.ImgBuscarClicked += ImgBuscar_Click;
        }

        private void ImgBuscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = barraBusquedaInsumo.TxtBusqueda.Text;
            stpListaInsumos.Children.Clear();
            List<ElementoInsumoOrdenCompra> insumosFiltrados = listaElementoEnListaInsumos.Where(insumo => insumo.insumoDto.Codigo.Contains(criterioBusqueda) ||
                                                                                                 insumo.insumoDto.Nombre.Contains(criterioBusqueda)).ToList();
            MostrarInsumosFiltrados(insumosFiltrados);
        }

        private void MostrarInsumosFiltrados(List<ElementoInsumoOrdenCompra> insumosFiltrados)
        {
            foreach (var item in insumosFiltrados)
            {
                stpListaInsumos.Children.Add(item);
            }
        }

        private void ObtenerInformacion()
        {
            try
            {
                ObtenerProveedoresAtivos();
                ObtenerIsnsumoActivos();
            }
            catch (EndpointNotFoundException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (TimeoutException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException<ExcepcionServidorItaliaPizza> ex)
            {
                VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (FaultException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (CommunicationException ex)
            {
                VentanasEmergentes.MostrarVentanaErrorServidor();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
            catch (Exception ex)
            {
                VentanasEmergentes.MostrarVentanaErrorInesperado();
                ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
            }
        }

        private void ObtenerProveedoresAtivos()
        {
            ServicioProveedoresClient servicioProveedoresClient = new ServicioProveedoresClient();
            proveedores = servicioProveedoresClient.RecuperarProveedores().ToList();
        }

        private void ObtenerIsnsumoActivos()
        {
            ServicioProductosClient servicioProductosClient = new ServicioProductosClient();
            insumosDisponibles = servicioProductosClient.RecuperarInsumosActivos().ToList();
        }

        private void CargarProveedores(List<ProveedorDto> proveedoresACargar)
        {
            lstProveedores.ItemsSource = proveedoresACargar;
            lstProveedores.DisplayMemberPath = "NombreCompleto";
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
            stpListaInsumos.Children.Clear();
            foreach (var item in insumosDisponibles)
            {
                ElementoInsumoOrdenCompra elementoInsumoOrden = new ElementoInsumoOrdenCompra(item);
                elementoInsumoOrden.btnAgregarAOrden_Click += BtnAgregarAOrden;
                stpListaInsumos.Children.Add(elementoInsumoOrden);
                listaElementoEnListaInsumos.Add(elementoInsumoOrden);
            }
        }

        private void BtnAgregarAOrden(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompra elementoInsumo = sender as ElementoInsumoOrdenCompra;
            AgregarInsumoAOrdenDeCompra(elementoInsumo.insumoDto);
            QuitarInsumosDeListaInsumos(elementoInsumo);
            CalcularCostoOrdenCompra(listaElementosEnOrdenCompra);
        }
        private void AgregarInsumoAOrdenDeCompra(InsumoOrdenCompraDto insumo)
        {
            if (insumo != null)
            {
                ElementoInsumoOrdenCompraSeleccionado elementoInsumo = new ElementoInsumoOrdenCompraSeleccionado(insumo);
                elementoInsumo.btnInsumoMas_Click += BtnInsumoMas;
                elementoInsumo.btnInsumoMenos_Click += BtnInsumoMenos;
                elementoInsumo.btnEliminarInsumo_Click += BtnEliminarInsumoOrden;
                elementoInsumo.txbCantida_TextChanged += TxbCantidad_TextChanged;
                stpInsumosOrdenCompra.Children.Add(elementoInsumo);
                insumosSeleccionado.Add(elementoInsumo.insumoDto);
                listaElementosEnOrdenCompra.Add(elementoInsumo);
            }
        }

        private void QuitarInsumosDeListaInsumos(ElementoInsumoOrdenCompra elementoInsumo)
        {
            if (elementoInsumo != null)
            {
                stpListaInsumos.Children.Remove(elementoInsumo);
                insumosDisponibles.Remove(elementoInsumo.insumoDto);
                listaElementoEnListaInsumos.Remove(elementoInsumo);
            }
        }      

        private void BtnInsumoMenos(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden = sender as ElementoInsumoOrdenCompraSeleccionado;
            DisminuirCantidad(elementoInsumoOrden);
        }

        private void DisminuirCantidad(ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden)
        {
            int cantidad = int.Parse(elementoInsumoOrden.txbCantidad.Text);
            cantidad--;
            elementoInsumoOrden.txbCantidad.Text = cantidad.ToString();
        }

        private void BtnInsumoMas(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden = sender as ElementoInsumoOrdenCompraSeleccionado;
            AumentarCantidad(elementoInsumoOrden);
        }

        private void AumentarCantidad(ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden)
        {
            int cantidad = int.Parse(elementoInsumoOrden.txbCantidad.Text);
            cantidad++;
            elementoInsumoOrden.txbCantidad.Text = cantidad.ToString();
        }

        private void BtnEliminarInsumoOrden(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden = sender as ElementoInsumoOrdenCompraSeleccionado;
            AgregarInsumoAlistaInsumosDisponibles(elementoInsumoOrden.insumoDto);
            QuitarInsumoDeOrdenDeCompra(elementoInsumoOrden);
            CalcularCostoOrdenCompra(listaElementosEnOrdenCompra);
        }

        private void AgregarInsumoAlistaInsumosDisponibles(InsumoOrdenCompraDto elementoInsumoOrden)
        {
            if (elementoInsumoOrden != null)
            {
                ElementoInsumoOrdenCompra elementoInsumo = new ElementoInsumoOrdenCompra(elementoInsumoOrden);
                elementoInsumo.btnAgregarAOrden_Click += BtnAgregarAOrden;
                stpListaInsumos.Children.Add(elementoInsumo);
                insumosDisponibles.Add(elementoInsumo.insumoDto);
                listaElementoEnListaInsumos.Add(elementoInsumo);
            }
        }

        private void QuitarInsumoDeOrdenDeCompra(ElementoInsumoOrdenCompraSeleccionado elementoInsumo)
        {
            stpInsumosOrdenCompra.Children.Remove(elementoInsumo);
            insumosSeleccionado.Remove(elementoInsumo.insumoDto);
            listaElementosEnOrdenCompra.Remove(elementoInsumo);
        }

        private void TxbCantidad_TextChanged(object sender, EventArgs e)
        {
            ElementoInsumoOrdenCompraSeleccionado elementoInsumoOrden = sender as ElementoInsumoOrdenCompraSeleccionado;
            if (!String.IsNullOrEmpty(elementoInsumoOrden.txbCantidad.Text))
            {
                int cantidad = int.Parse(elementoInsumoOrden.txbCantidad.Text);
                if (cantidad > 0)
                {
                    float costo = elementoInsumoOrden.insumoDto.CostoUnitario;
                    float subtotoal = cantidad * costo;
                    elementoInsumoOrden.lblSubtotal.Content = subtotoal.ToString();
                    // int index = listaElementosEnOrdenCompra.IndexOf(elementoInsumoOrden);
                    // listaElementosEnOrdenCompra[index].txbCantidad.Text = cantidad.ToString();
                }
                else
                {
                    AgregarInsumoAlistaInsumosDisponibles(elementoInsumoOrden.insumoDto);
                    QuitarInsumoDeOrdenDeCompra(elementoInsumoOrden);
                }
                CalcularCostoOrdenCompra(listaElementosEnOrdenCompra);
            }
        }

        private void CalcularCostoOrdenCompra(List<ElementoInsumoOrdenCompraSeleccionado> insumosEnOrdenDeCompra)
        {
            if (insumosEnOrdenDeCompra != null)
            {
                float total = 0;
                foreach (var item in insumosEnOrdenDeCompra)
                {
                    int cantidad = int.Parse(item.txbCantidad.Text);
                    float costo = item.insumoDto.CostoUnitario;
                    float subtotoal = cantidad * costo;
                    total += subtotoal;
                }
                lblTotalProducto.Content = insumosSeleccionado.Count();
                lblTotalCosto.Content = total.ToString();
            }
        }

        private void BtnEnviarOrdenCompra_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCantidades(listaElementosEnOrdenCompra))
            {                
                if (ValidarProveedorSeleccionado())
                {
                    try
                    {
                        int idOrdenCompra = GuardarOrdenDeCompra(listaElementosEnOrdenCompra);
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
                        VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (TimeoutException ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (FaultException<ExcepcionServidorItaliaPizza> ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (FaultException ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorServidor();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (CommunicationException ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorServidor();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (Exception ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorInesperado();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
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
            if (ValidarCantidades(listaElementosEnOrdenCompra))
            {


                if (ValidarProveedorSeleccionado())
                {
                    try
                    {
                        int idOrdenCompra = GuardarOrdenDeCompra(listaElementosEnOrdenCompra);
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
                    catch (EndpointNotFoundException ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorConexionFallida();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (TimeoutException ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorTiempoEspera();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (FaultException<ExcepcionServidorItaliaPizza> ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorBaseDatos();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (FaultException ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorServidor();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (CommunicationException ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorServidor();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
                    }
                    catch (Exception ex)
                    {
                        VentanasEmergentes.MostrarVentanaErrorInesperado();
                        ManejadorExcepcion.ManejarExcepcionError(ex, NavigationService);
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

        private bool ValidarProveedorSeleccionado()
        {
            return lstProveedores.SelectedItem != null;
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
                    InsumoOrdenCompraDto = item.insumoDto,
                    CantidadInsumosAdquiridos = int.Parse(item.txbCantidad.Text)
                };
                listaInsumosOrden.Add(elementoOrdenCompraDto);
            }          
            if(listaInsumosOrden.Count > 0)
            {
                OrdenDeCompraDto ordenDeCompraDto = new OrdenDeCompraDto()
                {
                    IdOrdenCompra = 0,
                    IdEstadoOrdenCompra = 0,
                    Fecha = DateTime.Now,
                    Proveedor = proveedores.FirstOrDefault(pro => pro.RFC.Equals(lblRFCProveedor.Content)),
                    IdProveedor = proveedores.FirstOrDefault(pro => pro.RFC.Equals(lblRFCProveedor.Content)).IdProveedor,
                    listaElementosOrdenCompra = listaInsumosOrden.ToArray()
                };
                ServicioOrdenesCompraClient servicioOrdenesCompraClient = new ServicioOrdenesCompraClient();
                idOrdenCompra = servicioOrdenesCompraClient.GuardarOrdenDeCompraNueva(ordenDeCompraDto);
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error", "No se puede enviar una orden de compra vacía, agregue al menos un producto", Window.GetWindow(this), 1);
                ventanaEmergente.ShowDialog();
            }
            return idOrdenCompra;
        }

        private bool ValidarCantidades(List<ElementoInsumoOrdenCompraSeleccionado> itemsDeOrdenCompra)
        {
            foreach (var item in itemsDeOrdenCompra)
            {
                if (String.IsNullOrEmpty(item.txbCantidad.Text))
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

        private void BtnCancelarOrden_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensajeConfirmacion();
        }
        private void MostrarMensajeConfirmacion()
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("Cuidado!!!", "¿Seguro que desea cancelar el registro?, se perderán los datos que no se hayan guardado", "Si, Cancelar Registro", "No, Cancelar Accion", Window.GetWindow(this), 3);
            ventanaEmergente.ShowDialog();
            if (ventanaEmergente.AceptarAccion)
            {
                stpInsumosOrdenCompra.Children.Clear();
                stpListaInsumos.Children.Clear();
                listaElementosEnOrdenCompra.Clear();
                SalirAPantallaInicio();
            }
        }

        private void BtnRegistrarProveedor_Click(object sender, RoutedEventArgs e)
        {
            VentanaEmergente ventanaEmergente = new VentanaEmergente("AVISO!!!", "Esta funcionalidad se implementara proximamente", Window.GetWindow(this), 2);
            ventanaEmergente.ShowDialog();
        }

        private void BtnLimpiarOrden_Click(object sender, MouseButtonEventArgs e)
        {
            var listaCopia = listaElementosEnOrdenCompra.ToList();
            foreach (var item in listaCopia)
            {
                AgregarInsumoAlistaInsumosDisponibles(item.insumoDto);
                QuitarInsumoDeOrdenDeCompra(item);
            }
            stpInsumosOrdenCompra.Children.Clear();
            CalcularCostoOrdenCompra(listaElementosEnOrdenCompra);
        }

        private void lstProveedores_Selected(object sender, SelectionChangedEventArgs e)
        {
            ProveedorDto proveedorSeleccionado = (ProveedorDto) lstProveedores.SelectedItem;
            if (proveedorSeleccionado != null)
            {
                MostrarInformacionProveedor(proveedorSeleccionado);
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
