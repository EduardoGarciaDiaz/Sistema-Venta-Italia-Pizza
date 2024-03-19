﻿using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Usuarios()
        {
            InitializeComponent();
            PrepararVentana();
        }

        private void PrepararVentana()
        {
            RecuperarUusuarios();
            MostrarUsuarios(clientes, empleados);
            CargarTiposEmpleados(tiposEmpleado);
            barraBusquedaUsuario.TxtBusqueda.Text = "Busaca un usuario";
            barraBusquedaUsuario.Background = new SolidColorBrush(Colors.White);
            barraBusquedaUsuario.ImgBuscarClicked += ImgBuscar_Click;
        }

        private void RecuperarUusuarios()
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            clientes =  servicioUsuariosClient.RecuperarClientes().ToList();
            empleados = servicioUsuariosClient.RecuperarEmpleados().ToList();
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
            ElementoUsuario elementoUsuario = sender as ElementoUsuario;
            UsuarioDto usuario =  elementoUsuario.usuario;
            VentanaEmergente ventanaEmergente = new VentanaEmergente("AVISO!!", "La funcionalidad modificar sera proximamemnte impelemtada", Window.GetWindow(this), 2);
            ventanaEmergente.ShowDialog();
        }

        private void BtnDesactivarActivar_Click(Object sender, EventArgs e)
        {
            ElementoUsuario elementoUsuario = sender as ElementoUsuario;
            if (elementoUsuario.empleado != null)
            {
                var empleado = elementoUsuario.empleado;
                if (empleado.Usuario.EsActivo)
                {
                    DesactivarActivarUsuario(empleado.IdUsuario, true, true, elementoUsuario);
                }
                else
                {
                    DesactivarActivarUsuario(empleado.IdUsuario, true, false, elementoUsuario);
                }
            }
            else if(elementoUsuario.usuario != null)
            {
                var cliente = elementoUsuario.usuario;
                if (cliente.EsActivo)
                {
                    DesactivarActivarUsuario(cliente.IdUsuario, false, true, elementoUsuario);
                }
                else
                {
                    DesactivarActivarUsuario(cliente.IdUsuario, false, false, elementoUsuario);
                }
            }

        }

        private void DesactivarActivarUsuario(int idUsuario, bool esEmpelado, bool desactivar, ElementoUsuario usuario)
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            bool exitoAccion = servicioUsuariosClient.Activar_DesactivarUsuario(idUsuario, esEmpelado, desactivar);
            if (exitoAccion && desactivar)
            {
                usuario.brdActivoBackGorund.Background = new SolidColorBrush(Colors.Red);
                usuario.btnEsActivo.HorizontalAlignment = HorizontalAlignment.Left;
                usuario.esActivo = false;
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Información!!", "Se desactivo correctamente al usuario", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
            }
            else if(esEmpelado  && desactivar)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error!!", "No se pudo desactivar al empleado, revise si el usuario no esta actualemte activo, o verifque su conexión ", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
            }
            else if(desactivar)
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error!!", "No se pudo desactivar al cliente, revise si tiene pedidos pendientes, o verifique su conexión ", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
            }
            else if (exitoAccion && !desactivar)
            {
                usuario.esActivo = true;
                usuario.brdActivoBackGorund.Background = new SolidColorBrush(Colors.Black);
                usuario.btnEsActivo.HorizontalAlignment = HorizontalAlignment.Right;
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Exito!!", "Se activo correctamente al usuario", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
            }
            else
            {
                VentanaEmergente ventanaEmergente = new VentanaEmergente("Error!!", "Hubo un probelma al activar al usuario, revise su conexion e intentelo mas tarde", Window.GetWindow(this), 2);
                ventanaEmergente.ShowDialog();
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
            ResaltarFiltroSeleccionado(brdTodos);
            SettearElementosUsuario(usuariosActuales);
        }

        private void BtnEmpleados_Click(object sender, RoutedEventArgs e)
        {
            btnFiltros.IsEnabled = true;
            ResaltarFiltroSeleccionado(brdEmpleados);
            List<ElementoUsuario> usuariosFiltrados = usuariosActuales.Where(usuario => usuario.empleado != null).ToList();
            SettearElementosUsuario(usuariosFiltrados);
        }

        private void BtnClientes_Click(object sender, RoutedEventArgs e)
        {
            RemoverFiltrosTipoEmpleados();
            btnFiltros.IsEnabled = false;
            ltbListaTiposEmpleados.Visibility = Visibility.Hidden;
            ResaltarFiltroSeleccionado(brdClientes);
            List<ElementoUsuario> usuariosFiltrados = usuariosActuales.Where(usuario => usuario.empleado == null).ToList();
            SettearElementosUsuario(usuariosFiltrados);
        }

        private void TiposEmpleados_Selection(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem itemSeleccionado = ltbListaTiposEmpleados.SelectedItem as ListBoxItem;
            if(itemSeleccionado != null)
            {
                ltbListaTiposEmpleados.Items.Remove(itemSeleccionado);
                AgregarFiltroDeTipoEmpleado(itemSeleccionado);
                FiltrarEmpleados();
                ltbListaTiposEmpleados.Visibility = Visibility.Hidden;
            }
        }

        private void BtnQuitarFiltroEmpleado_Click(object sender, MouseButtonEventArgs e)
        {
            Image imgBorrarTipoEmpleado = sender as Image;
            StackPanel spnTipoEmpleado = imgBorrarTipoEmpleado.Parent as StackPanel;
            Border brdTipoEmpleado = spnTipoEmpleado.Parent as Border;
            Label lblTipoEmpleado = ObtenerLabeTipoEMpleado(brdTipoEmpleado);
            int columnaDeReferencia = Grid.GetColumn(brdTipoEmpleado);
            for (int i = columnaDeReferencia; i < 9; i++)
            {
                if (i < 8)
                {
                    Border borderDetino = grdFiltros.Children[i] as Border;
                    Border borderSigueinte = grdFiltros.Children[i + 1] as Border;
                    if(borderSigueinte.Visibility == Visibility.Visible)
                    {
                        Label labelDestino = ObtenerLabeTipoEMpleado(borderDetino);
                        Label labelSiguiente = ObtenerLabeTipoEMpleado(borderSigueinte);
                        labelDestino.Content = labelSiguiente.Content;
                        labelSiguiente.Content = String.Empty;
                        borderSigueinte.Visibility = Visibility.Collapsed;
                    }
                    else
                    {

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
            FiltrarEmpleados();
            ListBoxItem lbiTipoEmpleado = new ListBoxItem();
            lbiTipoEmpleado.Name = "_" + tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(lblTipoEmpleado.Content)).IdTipoEmpleado;
            lbiTipoEmpleado.Content = tiposEmpleado.FirstOrDefault(tipo => tipo.Nombre.Equals(lblTipoEmpleado.Content)).Nombre;
            lbiTipoEmpleado.Style = (Style)FindResource("ListItem");
            ltbListaTiposEmpleados.Items.Add(lbiTipoEmpleado);
        }

        private void AgregarFiltroDeTipoEmpleado(ListBoxItem itemSeleciconado)
        {
            if(brdFiltro1.Visibility == Visibility.Collapsed)
            { 
                brdFiltro1.Visibility = Visibility.Visible;
                Label label = ObtenerLabeTipoEMpleado(brdFiltro1);
                label.Content = itemSeleciconado.Content;
            }
            else if(brdFiltro2.Visibility == Visibility.Collapsed)
            {
                brdFiltro2.Visibility = Visibility.Visible;
                Label label = ObtenerLabeTipoEMpleado(brdFiltro2);
                label.Content = itemSeleciconado.Content;
            }
            else if (brdFiltro3.Visibility == Visibility.Collapsed)
            {
                brdFiltro3.Visibility = Visibility.Visible;
                Label label = ObtenerLabeTipoEMpleado(brdFiltro3);
                label.Content = itemSeleciconado.Content;
            }
            else if(brdFiltro4.Visibility == Visibility.Collapsed)
            {
                brdFiltro4.Visibility = Visibility.Visible;
                Label label = ObtenerLabeTipoEMpleado(brdFiltro4);
                label.Content = itemSeleciconado.Content;
            }
        }

        private void FiltrarEmpleados()
        {
            List<ElementoUsuario> empleados = ObtenerUsuariosVisibles().Where(usuario => usuario.empleado != null).ToList();
            List<ElementoUsuario> empleadosFiltrados = new List<ElementoUsuario>();
            List<ElementoUsuario> puesto1;
            List<ElementoUsuario> puesto2;
            List<ElementoUsuario> puesto3;
            List<ElementoUsuario> puesto4;
            if (brdFiltro1.Visibility != Visibility.Collapsed)
            {
                Label label = ObtenerLabeTipoEMpleado(brdFiltro1);
                puesto1 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(label.Content)).ToList();
                empleadosFiltrados.AddRange(puesto1);
            }
            if (brdFiltro2.Visibility != Visibility.Collapsed)
            {
                Label label = ObtenerLabeTipoEMpleado(brdFiltro2);
                puesto2 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(label.Content)).ToList();
                empleadosFiltrados.AddRange(puesto2);
            }
            if (brdFiltro3.Visibility != Visibility.Collapsed)
            {
                Label label = ObtenerLabeTipoEMpleado(brdFiltro1);
                puesto3 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(label.Content)).ToList();
                empleadosFiltrados.AddRange(puesto3);
            }
            if (brdFiltro4.Visibility != Visibility.Collapsed)
            {
                Label label = ObtenerLabeTipoEMpleado(brdFiltro1);
                puesto4 = empleados.Where(usuario => usuario.lblTipoEmpleado.Text.Equals(label.Content)).ToList();
                empleadosFiltrados.AddRange(puesto4);
            }
            empleados.Clear();
            SettearElementosUsuario(empleadosFiltrados);
        }

        private void RemoverFiltrosTipoEmpleados()
        {
            List<Border> borders = new List<Border>() { brdFiltro1, brdFiltro2, brdFiltro3, brdFiltro4};
            foreach (var item in borders)
            {
                if (item.Visibility == Visibility.Visible)
                {
                    Label label = ObtenerLabeTipoEMpleado(item);
                    label.Content = String.Empty;
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void ImgBuscar_Click(object sender, EventArgs e)
        {
            string criterioBusqueda = barraBusquedaUsuario.TxtBusqueda.Text.Trim().ToLower();           
            List<ElementoUsuario> usuariosFiltrados = ObtenerUsuariosVisibles().Where(usuario => usuario.lblNombre.Text.ToLower().Contains(criterioBusqueda) ||
                                                                                      usuario.lblDireccion.Text.ToLower().Contains(criterioBusqueda) ||
                                                                                      usuario.lblTelefono.Text.ToLower().Contains(criterioBusqueda)).ToList();
            SettearElementosUsuario(usuariosFiltrados);
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

        private void SettearElementosUsuario(List<ElementoUsuario> usuariosQueMostrar)
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




    }
}
