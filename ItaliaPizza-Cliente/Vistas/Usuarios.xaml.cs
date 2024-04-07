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
        private List<UsuarioDto> clientes;
        private List<EmpleadoDto> empleados;
        private List<TipoEmpleadoDto> tiposEmpleado;
        private List<ElementoUsuario> usuariosActuales;
        private int tipoUsuarioActual = 0;

        public Usuarios()
        {
            InitializeComponent();
            this.Loaded += PrepararVentana;
        }

        private void PrepararVentana(object sender, RoutedEventArgs e)
        {
            ObtenerUusuarios();
            MostrarUsuarios(clientes, empleados);
            CargarTiposEmpleados(tiposEmpleado);
            barraBusquedaUsuario.Background = new SolidColorBrush(Colors.White);
            barraBusquedaUsuario.ImgBuscarClicked += ImgBuscar_Click;
            barraBusquedaUsuario.Placeholder.Text = "Busca un usuario por nombre, direccion o telefono...";
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

        private void ObtenerEmpleados()
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            empleados = servicioUsuariosClient.RecuperarEmpleados().ToList();
        }

        private void ObtenerClientes()
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            clientes = servicioUsuariosClient.RecuperarClientes().ToList();
        }

        private void ObtenerTiposEmpleados()
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            tiposEmpleado = servicioUsuariosClient.RecuperarTiposEmpleado().ToList();
        }

        private void MostrarUsuarios(List<UsuarioDto> listaClientes, List<EmpleadoDto> listaEmpleados)
        {
            wrpUsuariosLista.Children.Clear();
            foreach (var item in listaEmpleados)
            {
                ElementoUsuario elementoUsuario = new ElementoUsuario(item);
                elementoUsuario.btnModificarUusuario_Click += BtnModificarUsuario_Click;
                elementoUsuario.btnDesactivarActivarUsuario_Click += BtnDesactivarActivar_Click;
                wrpUsuariosLista.Children.Add(elementoUsuario);
            }
            foreach (var item in listaClientes)
            {
                ElementoUsuario elementoUsuario = new ElementoUsuario(item);

                elementoUsuario.btnModificarUusuario_Click += BtnModificarUsuario_Click;
                elementoUsuario.btnDesactivarActivarUsuario_Click += BtnDesactivarActivar_Click;
                wrpUsuariosLista.Children.Add(elementoUsuario);
            }
            usuariosActuales = ObtenerUsuariosVisibles();
        }

        private void CargarTiposEmpleados(List<TipoEmpleadoDto> puestos)
        {
            foreach (var item in puestos)
            {
                ListBoxItem lbiTipoEmpleado = new ListBoxItem();
                lbiTipoEmpleado.Name = "_"+item.IdTipoEmpleado.ToString();
                lbiTipoEmpleado.Content = item.Nombre;
                lbiTipoEmpleado.Style = (Style)FindResource("ListItem");                
                ltbListaTiposEmpleados.Items.Add(lbiTipoEmpleado);
            }
        }

        private void BtnModificarUsuario_Click(object sender, EventArgs e)
        {
           // ElementoUsuario elementoUsuario = sender as ElementoUsuario;
            //UsuarioDto usuario =  elementoUsuario.usuario;
            VentanaEmergente ventanaEmergente = new VentanaEmergente("AVISO!!", "La funcionalidad modificar sera proximamemnte impelemtada", Window.GetWindow(this), 2);
            ventanaEmergente.ShowDialog();
        }

        private void BtnDesactivarActivar_Click(Object sender, EventArgs e)
        {
            ElementoUsuario elementoUsuario = sender as ElementoUsuario;
            try
            {
                if (elementoUsuario.empleado != null)
                {
                    var empleado = elementoUsuario.empleado;
                    if (empleado.Usuario.EsActivo)
                    {
                        elementoUsuario.empleado.Usuario.EsActivo = DesactivarActivarUsuario(empleado.IdUsuario, true, true, elementoUsuario);
                    }
                    else
                    {
                        elementoUsuario.empleado.Usuario.EsActivo = DesactivarActivarUsuario(empleado.IdUsuario, true, false, elementoUsuario);
                    }
                }
                else if (elementoUsuario.usuario != null)
                {
                    var cliente = elementoUsuario.usuario;
                    if (cliente.EsActivo)
                    {
                        elementoUsuario.usuario.EsActivo = DesactivarActivarUsuario(cliente.IdUsuario, false, true, elementoUsuario);
                    }
                    else
                    {
                        elementoUsuario.usuario.EsActivo = DesactivarActivarUsuario(cliente.IdUsuario, false, false, elementoUsuario);
                    }
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

        private bool DesactivarActivarUsuario(int idUsuario, bool esEmpelado, bool desactivar, ElementoUsuario usuario)
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            bool exitoAccion = servicioUsuariosClient.Activar_DesactivarUsuario(idUsuario, esEmpelado, desactivar);
            if (exitoAccion && desactivar)
            {
                usuario.brdActivoBackGorund.Background = new SolidColorBrush(Colors.Red);
                usuario.btnEsActivo.HorizontalAlignment = HorizontalAlignment.Left;
                usuario.lblModificarEstado.Content = "Activar";
                usuario.esActivo = false;
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Información!!", "Se desactivo correctamente al usuario", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                return false;
            }
            else if(esEmpelado  && desactivar)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error!!", "No se pudo desactivar al empleado, revise si el usuario no esta actualemte activo, o verifque su conexión ", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                return true;
            }
            else if(desactivar)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error!!", "No se pudo desactivar al cliente, revise si tiene pedidos pendientes, o verifique su conexión ", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                return true;
            }
            else if (exitoAccion && !desactivar)
            {
                usuario.esActivo = true;
                usuario.brdActivoBackGorund.Background = new SolidColorBrush(Colors.Black);
                usuario.btnEsActivo.HorizontalAlignment = HorizontalAlignment.Right;
                usuario.lblModificarEstado.Content = "Desactivar";
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Exito!!", "Se activo correctamente al usuario", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
                return true;
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error!!", "Hubo un probelma al activar al usuario, revise su conexion e intentelo mas tarde", Window.GetWindow(this), 2);
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

       

        private void BtnFiltrosEmpleados_Click(object sender, MouseButtonEventArgs e)
        {
            if (ltbListaTiposEmpleados.IsVisible)
            {
                ltbListaTiposEmpleados.Visibility = Visibility.Hidden;
            }
            else
            {
                ltbListaTiposEmpleados.Visibility = Visibility.Visible;
            }
        }

        private void BtnTodos_Click(object sender, RoutedEventArgs e)
        {
            RemoverFiltrosTipoEmpleados();
            btnFiltros.IsEnabled = false;
            ltbListaTiposEmpleados.Visibility = Visibility.Hidden;
            tipoUsuarioActual = 0;
            ResaltarFiltroSeleccionado(brdTodos);
            MostrarCoincidencias(usuariosActuales);
        }

        private void BtnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            btnFiltros.IsEnabled = true;
            tipoUsuarioActual = 1;
            ResaltarFiltroSeleccionado(brdEmpleados);
            List<ElementoUsuario> usuariosFiltrados = usuariosActuales.Where(usuario => usuario.empleado != null).ToList();
            MostrarCoincidencias(usuariosFiltrados);
        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            RemoverFiltrosTipoEmpleados();
            btnFiltros.IsEnabled = false;
            ltbListaTiposEmpleados.Visibility = Visibility.Hidden;
            tipoUsuarioActual = 2;
            ResaltarFiltroSeleccionado(brdClientes);
            List<ElementoUsuario> usuariosFiltrados = usuariosActuales.Where(usuario => usuario.empleado == null).ToList();
            MostrarCoincidencias(usuariosFiltrados);
        }

        private void TiposEmpleados_Selection(object sender, SelectionChangedEventArgs e)
        {
            SeleccionarFiltroTipoEmpleado();
        }

        private void SeleccionarFiltroTipoEmpleado()
        {
            ListBoxItem itemSeleccionado = ltbListaTiposEmpleados.SelectedItem as ListBoxItem;
            if (itemSeleccionado != null)
            {
                ltbListaTiposEmpleados.Items.Remove(itemSeleccionado);
                AgregarFiltroDeTipoEmpleado(itemSeleccionado);
                FiltrarEmpleados(usuariosActuales.Where(usuario => usuario.empleado != null).ToList());
                ltbListaTiposEmpleados.Visibility = Visibility.Hidden;
            }
        }

        private void BtnQuitarFiltroEmpleado_Click(object sender, MouseButtonEventArgs e)
        {
            Image imgBorrarTipoEmpleado = sender as Image;
            StackPanel spnTipoEmpleado = imgBorrarTipoEmpleado.Parent as StackPanel;
            Border brdTipoEmpleado = spnTipoEmpleado.Parent as Border;
            String nombreTipoEmpleado = ObtenerLabeTipoEMpleado(brdTipoEmpleado).Content.ToString();
            int columnaDeReferencia = Grid.GetColumn(brdTipoEmpleado);
            LimpiarFiltrosTipoEmpleado(columnaDeReferencia);
            FiltrarEmpleados(usuariosActuales.Where(usuario => usuario.empleado != null).ToList());
            ListBoxItem lbiTipoEmpleado = new ListBoxItem();
            lbiTipoEmpleado.Name = "_" + tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(nombreTipoEmpleado)).IdTipoEmpleado;
            lbiTipoEmpleado.Content = tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(nombreTipoEmpleado)).Nombre;
            lbiTipoEmpleado.Style = (Style)FindResource("ListItem");
            ltbListaTiposEmpleados.Items.Add(lbiTipoEmpleado);
        }

        private void LimpiarFiltrosTipoEmpleado(int columnaDeReferencia)
        {
            for (int i = columnaDeReferencia; i < 9; i++)
            {
                if (i < 8)
                {
                    Border borderDetino = grdFiltros.Children[i] as Border;
                    Border borderSigueinte = grdFiltros.Children[i + 1] as Border;
                    Label labelDestino = ObtenerLabeTipoEMpleado(borderDetino);
                    if (borderSigueinte.Visibility == Visibility.Visible)
                    {
                        Label labelSiguiente = ObtenerLabeTipoEMpleado(borderSigueinte);
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
                    Label labelDestino = ObtenerLabeTipoEMpleado(borderDetino);
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
                Label lblFiltro1 = ObtenerLabeTipoEMpleado(brdFiltro1);
                lblFiltro1.Content = itemSeleciconado.Content;
            }
            else if (brdFiltro2.Visibility == Visibility.Collapsed)
            {
                brdFiltro2.Visibility = Visibility.Visible;
                Label lblFiltro2 = ObtenerLabeTipoEMpleado(brdFiltro2);
                lblFiltro2.Content = itemSeleciconado.Content;
            }
            else if (brdFiltro3.Visibility == Visibility.Collapsed)
            {
                brdFiltro3.Visibility = Visibility.Visible;
                Label labelFiltro3 = ObtenerLabeTipoEMpleado(brdFiltro3);
                labelFiltro3.Content = itemSeleciconado.Content;

            }
            else if (brdFiltro4.Visibility == Visibility.Collapsed)
            {
                brdFiltro4.Visibility = Visibility.Visible;
                Label lblFiltro4 = ObtenerLabeTipoEMpleado(brdFiltro4);
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
                Label lblFiltro1 = ObtenerLabeTipoEMpleado(brdFiltro1);
                puesto1 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(lblFiltro1.Content)).ToList();
                empleadosFiltrados.AddRange(puesto1);

                if (brdFiltro2.Visibility != Visibility.Collapsed)
                {

                    Label lblFiltro2 = ObtenerLabeTipoEMpleado(brdFiltro2);
                    puesto2 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(lblFiltro2.Content)).ToList();
                    empleadosFiltrados.AddRange(puesto2);
                    if (brdFiltro3.Visibility != Visibility.Collapsed)
                    {

                        Label labelFiltro3 = ObtenerLabeTipoEMpleado(brdFiltro3);
                        puesto3 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(labelFiltro3.Content)).ToList();
                        empleadosFiltrados.AddRange(puesto3);
                        if (brdFiltro4.Visibility != Visibility.Collapsed)
                        {

                            Label lblFiltro4 = ObtenerLabeTipoEMpleado(brdFiltro4);
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
                    Label label = ObtenerLabeTipoEMpleado(item);
                    ListBoxItem lbiTipoEmpleado = new ListBoxItem();
                    lbiTipoEmpleado.Name = "_" + tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(label.Content)).IdTipoEmpleado;
                    lbiTipoEmpleado.Content = tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(label.Content)).Nombre;
                    lbiTipoEmpleado.Style = (Style)FindResource("ListItem");
                    ltbListaTiposEmpleados.Items.Add(lbiTipoEmpleado);
                    label.Content = String.Empty;
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ImgBuscar_Click(object sender, EventArgs e)
        {
            FiltrarUsuariosPorBusqueda();
        }

        private void FiltrarUsuariosPorBusqueda()
        {
            string criterioBusqueda = barraBusquedaUsuario.TxtBusqueda.Text.Trim().ToLower();
            List<ElementoUsuario> usuariosFiltrados = usuariosActuales.Where(usuario => usuario.lblNombre.Text.ToLower().Contains(criterioBusqueda) ||
                                                                                      usuario.lblDireccion.Text.ToLower().Contains(criterioBusqueda) ||
                                                                                      usuario.lblTelefono.Text.ToLower().Contains(criterioBusqueda)).ToList(); 
            switch (tipoUsuarioActual)
            {
                case 0:
                    MostrarCoincidencias(usuariosFiltrados);
                    break;
                case 1:
                    usuariosFiltrados = usuariosFiltrados.Where(usuario => usuario.empleado != null).ToList();
                    MostrarCoincidencias(usuariosFiltrados);
                    break;
                case 2:
                    usuariosFiltrados = usuariosFiltrados.Where(usuario => usuario.empleado == null).ToList();
                    MostrarCoincidencias(usuariosFiltrados);
                    break;
            }
        }

        private List<ElementoUsuario> ObtenerUsuariosVisibles() 
        {
            List<ElementoUsuario> usuariosAVisibles = new List<ElementoUsuario>();
            foreach (UIElement elemento in wrpUsuariosLista.Children)
            {
                if (elemento is ElementoUsuario)
                {
                    usuariosAVisibles.Add((ElementoUsuario)elemento);
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

        private Label ObtenerLabeTipoEMpleado(Border border)
        {
            StackPanel children = (StackPanel)border.Child;
            Label label = (Label)children.Children[0];
            return label;
        }

        private void BtnRegistrarUsuario_Click(object sender, RoutedEventArgs e)
        {
            RegistroUsuario paginaRegistroUsuario = new RegistroUsuario();
            MainWindow ventanaPrincipal = (MainWindow) Window.GetWindow(this);
            ventanaPrincipal.FrameNavigator.NavigationService.Navigate(paginaRegistroUsuario);
        }
    }
}
