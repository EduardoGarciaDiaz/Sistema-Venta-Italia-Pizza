using ItaliaPizza_Cliente.ServicioItaliaPizza;
using ItaliaPizza_Cliente.Vistas;
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

namespace ItaliaPizza_Cliente.Recursos.ControlesUsuario
{
    /// <summary>
    /// Interaction logic for ElementoUsuario.xaml
    /// </summary>
    public partial class ElementoUsuario : UserControl
    {

        public EventHandler btnModificarUusuario_Click;
        public EventHandler btnDesactivarActivarUsuario_Click;
        public int Id;
        public EmpleadoDto empleado { get; }
        public UsuarioDto usuario {  get; }
        


        public ElementoUsuario(EmpleadoDto empleado)
        {
            InitializeComponent();
            this.empleado = empleado;
            InsatanciarEmpleado();
            MostrarDatosUsuario(empleado.Usuario);
            lblTipoEmpleado.Text = empleado.TipoEmpleado;
            MostrarSiEstaActivo(empleado.Usuario.EsActivo);
        }

        public ElementoUsuario(UsuarioDto cliente)
        {
            InitializeComponent();
            this.usuario = cliente;
            InstanciaCliente();
            MostrarDatosUsuario(cliente);
            MostrarSiEstaActivo(cliente.EsActivo);
        }

        private void InsatanciarEmpleado()
        {
            imgTipoUsuario.Source = new BitmapImage(new Uri("/Recursos/iconos/icono_empleado.png", UriKind.Relative));
            lblTipoUsuario.Content = "Empleado";
            lblTipoEmpleadoTag.Visibility = Visibility.Visible;
            lblTipoEmpleado.Visibility = Visibility.Visible;
        }

        private void InstanciaCliente()
        {
            imgTipoUsuario.Source = new BitmapImage(new Uri("/Recursos/iconos/icono_usuario_consulta.png", UriKind.Relative));
            lblTipoUsuario.Content = "Cliente";
            lblTipoEmpleadoTag.Visibility = Visibility.Hidden;
            lblTipoEmpleado.Visibility = Visibility.Hidden;
        }

        private void MostrarDatosUsuario(UsuarioDto usuario)
        {
            Id = usuario.IdUsuario;
            lblNombre.Text = usuario.NombreCompleto.ToString();
            lblCorreo.Text = usuario.CorreoElectronico.ToString();
            String telefono = usuario.NumeroTelefono.ToString();
            lblTelefono.Text = "(+52) " + telefono.Substring(0, 3) + "-" + telefono.Substring(3,3) + "-" + telefono.Substring(6,2) + "-" + telefono.Substring(8);
            String direccion = usuario.Direccion.Ciudad + ", Col. "  + usuario.Direccion.Colonia.ToString() + " " + usuario.Direccion.CodigoPostal + ", Calle " + usuario.Direccion.Calle + " #" + usuario.Direccion.Numero.ToString();
            lblDireccion.Text = direccion;

        }

        private void MostrarSiEstaActivo(bool esActivo)
        {
            if (esActivo)
            {
                brdActivoBackGorund.Background = new SolidColorBrush(Colors.Red);
                btnEsActivo.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                brdActivoBackGorund.Background = new SolidColorBrush(Colors.Black);
                btnEsActivo.HorizontalAlignment = HorizontalAlignment.Right;
            }
        }


        private void BtnModificarUsuario_Click(object sender, MouseButtonEventArgs e)
        {
            btnModificarUusuario_Click?.Invoke(this, e);
        }

        private void BtnDesactivarActivar_Click(object sender, MouseButtonEventArgs e)
        {
            btnDesactivarActivarUsuario_Click?.Invoke(this, e);
        }
    }
}
