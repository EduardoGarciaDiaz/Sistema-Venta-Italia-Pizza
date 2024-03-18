using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
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
    /// Interaction logic for Usuarios.xaml
    /// </summary>
    public partial class Usuarios : Page
    {
        private List<UsuarioDto> clientes;
        private List<EmpleadoDto> empleados;

        public Usuarios()
        {
            InitializeComponent();
            PrepararVentana();
        }

        private void PrepararVentana()
        {
            RecuperarUusuarios();
            MostrarUsuarios(clientes, empleados);
        }

        private void RecuperarUusuarios()
        {
            ServicioUsuariosClient servicioUsuariosClient = new ServicioUsuariosClient();
            clientes =  servicioUsuariosClient.RecuperarClientes().ToList();
            empleados = servicioUsuariosClient.RecuperarEmpleados().ToList();
        }

        private void MostrarUsuarios(List<UsuarioDto> listaClientes, List<EmpleadoDto> listaEmpleados)
        {
            stpUsuariosLista.Children.Clear();
            foreach (var item in listaEmpleados)
            {
                ElementoUsuario elementoUsuario = new ElementoUsuario(item);
                elementoUsuario.btnModificarUusuario_Click += BtnModificarUsuario_Click;
                elementoUsuario.btnDesactivarActivarUsuario_Click += BtnDesactivarActivar_Click;
                stpUsuariosLista.Children.Add(elementoUsuario);
            }
            foreach (var item in listaClientes)
            {
                ElementoUsuario elementoUsuario = new ElementoUsuario(item);
                stpUsuariosLista.Children.Add(elementoUsuario);
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


    }
}
