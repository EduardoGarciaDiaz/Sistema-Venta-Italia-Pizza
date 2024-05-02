using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Usuarios.xaml
    /// </summary>
    public partial class Usuarios : Page
    {
        private List<UsuarioDto> _clientes = new List<UsuarioDto>();
        private List<EmpleadoDto> _empleados = new List<EmpleadoDto>();
        private List<TipoEmpleadoDto> _tiposEmpleado = new List<TipoEmpleadoDto>();
        private List<ElementoUsuario> _usuariosActuales = new List<ElementoUsuario>();
        private int _tipoUsuarioActual = 0;

        public Usuarios()
        {
            InitializeComponent();
            this.Loaded += PrepararVentana;
        }

        private void PrepararVentana(object sender, RoutedEventArgs e)
        {
            ObtenerUusuarios();
            MostrarUsuarios(_clientes, _empleados);
            CargarTiposEmpleados(_tiposEmpleado);
            barraBusquedaUsuario.Background = new SolidColorBrush(Colors.White);
            barraBusquedaUsuario.ImgBuscarClicked += ImgBuscar_Click;
            barraBusquedaUsuario.plhrInstruccion.Text = "Busca un Usuario por nombre, direccion o telefono...";
            ResaltarFiltroSeleccionado(brdTodos);
        }

        private void ObtenerUusuarios()
        {
            try
            {
                ObtenerEmpleados();
                ObtenerClientes();
                ObtenerTiposEmpleados();
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

        private void BtnModificarUsuario_Click(object sender, EventArgs e)
        {
            ElementoUsuario elementoUsuario = sender as ElementoUsuario;
            EdicionUsuario paginaEdicionUsuario;

            if (elementoUsuario.Empleado != null)
            {
                EmpleadoDto empleado = elementoUsuario.Empleado;
                paginaEdicionUsuario = new EdicionUsuario(empleado);
            }
            else
            {
                UsuarioDto usuario = elementoUsuario.Usuario;
                paginaEdicionUsuario = new EdicionUsuario(usuario);
            }
            NavigationService.Navigate(paginaEdicionUsuario);
        }

        private void BtnDesactivarActivar_Click(Object sender, EventArgs e)
        {
            ElementoUsuario elementoUsuario = sender as ElementoUsuario;
            try
            {
                if (elementoUsuario.Empleado != null)
                {
                    var empleado = elementoUsuario.Empleado;
                    if (empleado.Usuario.EsActivo)
                    {
                        elementoUsuario.Empleado.Usuario.EsActivo = DesactivarActivarUsuario(empleado.IdUsuario, true, true, elementoUsuario);
                    }
                    else
                    {
                        elementoUsuario.Empleado.Usuario.EsActivo = DesactivarActivarUsuario(empleado.IdUsuario, true, false, elementoUsuario);
                    }
                }
                else if (elementoUsuario.Usuario != null)
                {
                    var cliente = elementoUsuario.Usuario;
                    if (cliente.EsActivo)
                    {
                        elementoUsuario.Usuario.EsActivo = DesactivarActivarUsuario(cliente.IdUsuario, false, true, elementoUsuario);
                    }
                    else
                    {
                        elementoUsuario.Usuario.EsActivo = DesactivarActivarUsuario(cliente.IdUsuario, false, false, elementoUsuario);
                    }
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

        private void BtnFiltrosEmpleados_Click(object sender, MouseButtonEventArgs e)
        {
            if (lbxListaTiposEmpleados.IsVisible)
            {
                lbxListaTiposEmpleados.Visibility = Visibility.Hidden;
            }
            else
            {
                lbxListaTiposEmpleados.Visibility = Visibility.Visible;
            }
        }

        private void BtnTodos_Click(object sender, RoutedEventArgs e)
        {
            RemoverFiltrosTipoEmpleados();
            btnFiltros.IsEnabled = false;
            lbxListaTiposEmpleados.Visibility = Visibility.Hidden;
            _tipoUsuarioActual = 0;
            ResaltarFiltroSeleccionado(brdTodos);
            MostrarCoincidencias(_usuariosActuales);
        }

        private void BtnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            btnFiltros.IsEnabled = true;
            _tipoUsuarioActual = 1;
            ResaltarFiltroSeleccionado(brdEmpleados);
            List<ElementoUsuario> usuariosFiltrados = _usuariosActuales.Where(usuario => usuario.Empleado != null).ToList();
            MostrarCoincidencias(usuariosFiltrados);
        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            RemoverFiltrosTipoEmpleados();
            btnFiltros.IsEnabled = false;
            lbxListaTiposEmpleados.Visibility = Visibility.Hidden;
            _tipoUsuarioActual = 2;
            ResaltarFiltroSeleccionado(brdClientes);
            List<ElementoUsuario> usuariosFiltrados = _usuariosActuales.Where(usuario => usuario.Empleado == null).ToList();
            MostrarCoincidencias(usuariosFiltrados);
        }

        private void BtnQuitarFiltroEmpleado_Click(object sender, MouseButtonEventArgs e)
        {
            Image imgBorrarTipoEmpleado = sender as Image;
            StackPanel spnTipoEmpleado = imgBorrarTipoEmpleado.Parent as StackPanel;
            Border brdTipoEmpleado = spnTipoEmpleado.Parent as Border;
            String nombreTipoEmpleado = ObtenerLabelTipoEmpleado(brdTipoEmpleado).Content.ToString();
            int columnaDeReferencia = Grid.GetColumn(brdTipoEmpleado);
            LimpiarFiltrosTipoEmpleado(columnaDeReferencia);
            FiltrarEmpleados(_usuariosActuales.Where(usuario => usuario.Empleado != null).ToList());
            ListBoxItem lbiTipoEmpleado = new ListBoxItem();
            lbiTipoEmpleado.Name = "_" + _tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(nombreTipoEmpleado)).IdTipoEmpleado;
            lbiTipoEmpleado.Content = _tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(nombreTipoEmpleado)).Nombre;
            lbiTipoEmpleado.Style = (Style)FindResource("ListItem");
            lbxListaTiposEmpleados.Items.Add(lbiTipoEmpleado);
        }

        private void ImgBuscar_Click(object sender, EventArgs e)
        {
            FiltrarUsuariosPorBusqueda();
        }

        private void BtnRegistrarUsuario_Click(object sender, RoutedEventArgs e)
        {
            RegistroUsuario paginaRegistroUsuario = new RegistroUsuario();
            MainWindow ventanaPrincipal = (MainWindow)Window.GetWindow(this);
            ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaRegistroUsuario);
        }

        private void TiposEmpleados_Selection(object sender, SelectionChangedEventArgs e)
        {
            SeleccionarFiltroTipoEmpleado();
        }


        private void ObtenerEmpleados()
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            _empleados = servicioUsuariosClient.RecuperarEmpleados().ToList();
        }

        private void ObtenerClientes()
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            _clientes = servicioUsuariosClient.RecuperarClientes().ToList();
        }

        private void ObtenerTiposEmpleados()
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            _tiposEmpleado = servicioUsuariosClient.RecuperarTiposEmpleado().ToList();
        }

        private void MostrarUsuarios(List<UsuarioDto> listaClientes, List<EmpleadoDto> listaEmpleados)
        {
            wrpUsuariosLista.Children.Clear();
            foreach (var item in listaEmpleados)
            {
                ElementoUsuario elementoUsuario = new ElementoUsuario(item);
                elementoUsuario.BtnModificarUsuarioClicked += BtnModificarUsuario_Click;
                elementoUsuario.BtnDesactivarActivarUsuarioClicked += BtnDesactivarActivar_Click;
                wrpUsuariosLista.Children.Add(elementoUsuario);
            }
            foreach (var item in listaClientes)
            {
                ElementoUsuario elementoUsuario = new ElementoUsuario(item);

                elementoUsuario.BtnModificarUsuarioClicked += BtnModificarUsuario_Click;
                elementoUsuario.BtnDesactivarActivarUsuarioClicked += BtnDesactivarActivar_Click;
                wrpUsuariosLista.Children.Add(elementoUsuario);
            }
            _usuariosActuales = ObtenerUsuariosVisibles();
        }

        private void CargarTiposEmpleados(List<TipoEmpleadoDto> puestos)
        {
            foreach (var item in puestos)
            {
                ListBoxItem lbxiTipoEmpleado = new ListBoxItem();
                lbxiTipoEmpleado.Name = "_"+item.IdTipoEmpleado.ToString();
                lbxiTipoEmpleado.Content = item.Nombre;
                lbxiTipoEmpleado.Style = (Style)FindResource("ListItem");                
                lbxListaTiposEmpleados.Items.Add(lbxiTipoEmpleado);
            }
        }

      
        private bool DesactivarActivarUsuario(int idUsuario, bool esEmpelado, bool desactivar, ElementoUsuario usuario)
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            bool exitoAccion = servicioUsuariosClient.Activar_DesactivarUsuario(idUsuario, esEmpelado, desactivar);
            if (exitoAccion && desactivar)
            {
                usuario.brdActivoBackGorund.Background = new SolidColorBrush(Colors.Red);
                usuario.btnEsActivo.HorizontalAlignment = HorizontalAlignment.Left;
                usuario.lblModificarEstado.Content = "Activar";
                usuario.EsActivo = false;
                VentanaEmergente ventanaEmergente = new VentanaEmergente("¡Éxito!", "Se desactivó correctamente al Usuario", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                return false;
            }
            else if(esEmpelado  && desactivar)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("¡Error!", "No se pudo desactivar al Empleado, revise si el Usuario no está actualemte activo o verifque su conexión ", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                return true;
            }
            else if(desactivar)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("¡Error!", "No se pudo desactivar al cliente, revise si tiene pedidos pendientes o verifique su conexión ", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                return true;
            }
            else if (exitoAccion && !desactivar)
            {
                usuario.EsActivo = true;
                usuario.brdActivoBackGorund.Background = new SolidColorBrush(Colors.Black);
                usuario.btnEsActivo.HorizontalAlignment = HorizontalAlignment.Right;
                usuario.lblModificarEstado.Content = "Desactivar";
                VentanaEmergente ventanaEmergente = new VentanaEmergente("¡Exito!", "Se activó correctamente al Usuario", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                return true;
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("¡Error!", "Hubo un probelma al activar al Usuario, revise su conexion e inténtelo más tarde", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                return false;
            }
        }

        private void ResaltarFiltroSeleccionado(Border borderSeleccionado)
        {
            brdClientes.Background  = new SolidColorBrush(Colors.Transparent);
            brdEmpleados.Background = new SolidColorBrush(Colors.Transparent);
            brdTodos.Background = new SolidColorBrush(Colors.Transparent);
            borderSeleccionado.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8D72A"));            
        }       

        private void SeleccionarFiltroTipoEmpleado()
        {
            ListBoxItem itemSeleccionado = lbxListaTiposEmpleados.SelectedItem as ListBoxItem;
            if (itemSeleccionado != null)
            {
                lbxListaTiposEmpleados.Items.Remove(itemSeleccionado);
                AgregarFiltroDeTipoEmpleado(itemSeleccionado);
                FiltrarEmpleados(_usuariosActuales.Where(usuario => usuario.Empleado != null).ToList());
                lbxListaTiposEmpleados.Visibility = Visibility.Hidden;
            }
        }            

        private void LimpiarFiltrosTipoEmpleado(int columnaDeReferencia)
        {
            for (int i = columnaDeReferencia; i < 9; i++)
            {
                if (i < 8)
                {
                    Border borderDetino = grdFiltros.Children[i] as Border;
                    Border borderSigueinte = grdFiltros.Children[i + 1] as Border;
                    Label labelDestino = ObtenerLabelTipoEmpleado(borderDetino);
                    if (borderSigueinte.Visibility == Visibility.Visible)
                    {
                        Label labelSiguiente = ObtenerLabelTipoEmpleado(borderSigueinte);
                        labelDestino.Content = labelSiguiente.Content;
                    }
                    else
                    {
                        labelDestino.Content = String.Empty;
                        borderDetino.Visibility = Visibility.Collapsed;
                        break;
                    }
                }
                else
                {
                    Border borderDetino = grdFiltros.Children[i] as Border;
                    Label labelDestino = ObtenerLabelTipoEmpleado(borderDetino);
                    labelDestino.Content = String.Empty;
                    grdFiltros.Children[i].Visibility = Visibility.Collapsed;
                }
            }
        }

        private void AgregarFiltroDeTipoEmpleado(ListBoxItem itemSeleciconado)
        {
            if (brdFiltro1.Visibility == Visibility.Collapsed)
            {
                brdFiltro1.Visibility = Visibility.Visible;
                Label lblFiltro1 = ObtenerLabelTipoEmpleado(brdFiltro1);
                lblFiltro1.Content = itemSeleciconado.Content;
            }
            else if (brdFiltro2.Visibility == Visibility.Collapsed)
            {
                brdFiltro2.Visibility = Visibility.Visible;
                Label lblFiltro2 = ObtenerLabelTipoEmpleado(brdFiltro2);
                lblFiltro2.Content = itemSeleciconado.Content;
            }
            else if (brdFiltro3.Visibility == Visibility.Collapsed)
            {
                brdFiltro3.Visibility = Visibility.Visible;
                Label labelFiltro3 = ObtenerLabelTipoEmpleado(brdFiltro3);
                labelFiltro3.Content = itemSeleciconado.Content;

            }
            else if (brdFiltro4.Visibility == Visibility.Collapsed)
            {
                brdFiltro4.Visibility = Visibility.Visible;
                Label lblFiltro4 = ObtenerLabelTipoEmpleado(brdFiltro4);
                lblFiltro4.Content = itemSeleciconado.Content;
            }
        }

        private void FiltrarEmpleados(List<ElementoUsuario> empleados)
        {
            List<ElementoUsuario> empleadosFiltrados = new List<ElementoUsuario>();
            List<ElementoUsuario> puesto1;
            List<ElementoUsuario> puesto2;
            List<ElementoUsuario> puesto3;
            List<ElementoUsuario> puesto4;
            if (brdFiltro1.Visibility != Visibility.Collapsed)
            {
                Label lblFiltro1 = ObtenerLabelTipoEmpleado(brdFiltro1);
                puesto1 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(lblFiltro1.Content)).ToList();
                empleadosFiltrados.AddRange(puesto1);

                if (brdFiltro2.Visibility != Visibility.Collapsed)
                {

                    Label lblFiltro2 = ObtenerLabelTipoEmpleado(brdFiltro2);
                    puesto2 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(lblFiltro2.Content)).ToList();
                    empleadosFiltrados.AddRange(puesto2);
                    if (brdFiltro3.Visibility != Visibility.Collapsed)
                    {

                        Label labelFiltro3 = ObtenerLabelTipoEmpleado(brdFiltro3);
                        puesto3 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(labelFiltro3.Content)).ToList();
                        empleadosFiltrados.AddRange(puesto3);
                        if (brdFiltro4.Visibility != Visibility.Collapsed)
                        {

                            Label lblFiltro4 = ObtenerLabelTipoEmpleado(brdFiltro4);
                            puesto4 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(lblFiltro4.Content)).ToList();
                            empleadosFiltrados.AddRange(puesto4);
                        }
                    }                   
                }
            }
            else
            {
                empleadosFiltrados = empleados.ToList();
            }
            empleados.Clear();
            MostrarCoincidencias(empleadosFiltrados);
        }

        private void RemoverFiltrosTipoEmpleados()
        {
            List<Border> borders = new List<Border>() { brdFiltro1, brdFiltro2, brdFiltro3, brdFiltro4};
            foreach (var item in borders)
            {
                if (item.Visibility == Visibility.Visible)
                { 
                    Label label = ObtenerLabelTipoEmpleado(item);
                    ListBoxItem lbiTipoEmpleado = new ListBoxItem();
                    lbiTipoEmpleado.Name = "_" + _tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(label.Content)).IdTipoEmpleado;
                    lbiTipoEmpleado.Content = _tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(label.Content)).Nombre;
                    lbiTipoEmpleado.Style = (Style)FindResource("ListItem");
                    lbxListaTiposEmpleados.Items.Add(lbiTipoEmpleado);
                    label.Content = String.Empty;
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }


        private void FiltrarUsuariosPorBusqueda()
        {
            string criterioBusqueda = barraBusquedaUsuario.tbxBusqueda.Text.Trim().ToLower();
            List<ElementoUsuario> usuariosFiltrados = _usuariosActuales.Where(usuario => usuario.lblNombre.Text.ToLower().Contains(criterioBusqueda) ||
                                                                                      usuario.lblDireccion.Text.ToLower().Contains(criterioBusqueda) ||
                                                                                      usuario.lblTelefono.Text.ToLower().Contains(criterioBusqueda)).ToList(); 
            switch (_tipoUsuarioActual)
            {
                case 0:
                    MostrarCoincidencias(usuariosFiltrados);
                    break;
                case 1:
                    usuariosFiltrados = usuariosFiltrados.Where(usuario => usuario.Empleado != null).ToList();
                    MostrarCoincidencias(usuariosFiltrados);
                    break;
                case 2:
                    usuariosFiltrados = usuariosFiltrados.Where(usuario => usuario.Empleado == null).ToList();
                    MostrarCoincidencias(usuariosFiltrados);
                    break;
            }
        }

        private List<ElementoUsuario> ObtenerUsuariosVisibles() 
        {
            List<ElementoUsuario> usuariosAVisibles = new List<ElementoUsuario>();
            foreach (UIElement elemento in wrpUsuariosLista.Children)
            {
                if (elemento is ElementoUsuario usuario)
                {
                    usuariosAVisibles.Add(usuario);
                }
            }
            return usuariosAVisibles;
        }

        private void MostrarCoincidencias(List<ElementoUsuario> usuariosQueMostrar)
        {
            wrpUsuariosLista.Children.Clear();
            List<ElementoUsuario> usuarios = usuariosQueMostrar.ToList(); 
            foreach (var item in usuarios)
            {
                wrpUsuariosLista.Children.Add(item);
            }
        }

        private Label ObtenerLabelTipoEmpleado(Border border)
        {
            StackPanel children = (StackPanel)border.Child;
            Label label = (Label)children.Children[0];
            return label;
        }

    }
}
