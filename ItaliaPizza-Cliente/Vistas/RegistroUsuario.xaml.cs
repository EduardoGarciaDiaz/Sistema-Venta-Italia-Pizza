using ItaliaPizza_Cliente.ServicioItaliaPizza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
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
using static System.Net.Mime.MediaTypeNames;

namespace ItaliaPizza_Cliente.Vistas
{
    /// <summary>
    /// Interaction logic for RegistroUsuario.xaml
    /// </summary>
    public partial class RegistroUsuario : Page
    {
        private const string CAMPO_VACIO = "* Campo obligatorio";
        private const string CORREO_INVALIDO = "* Correo no valido";
        private const string TELEFONO_INVALIDO = "* Telefono no valido";
        private readonly string EMAIL_RULES_CHAR = "^(?=.{1,90}$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
        private readonly string EMAIL_ALLOW_CHAR = "^[a-zA-Z0-9@,._=]{1,90}$";

        public RegistroUsuario()
        {
            InitializeComponent();
        }

        private void PrepareWindow()
        {

        }

        private void ObtenerTiposEmpleados()
        {
            ServicioUsuariosClient proxyServicioUsuariosClient = new ServicioUsuariosClient();
            var tiposEmpleados = proxyServicioUsuariosClient.RecuperarTiposEmpleado().ToList();
            cbmTipoEmpleado.ItemsSource = tiposEmpleados;
            cbmTipoEmpleado.DisplayMemberPath = tiposEmpleados.First().Nombre;
        }

        private bool ValidarCamposLlenosUsuario()
        {
            bool camposLlenos = true;
            List<CampoTextoConLabel> camposDeDatos = new List<CampoTextoConLabel>()
            {
                new CampoTextoConLabel(txbNombre,lblNombreError), new CampoTextoConLabel(txb1erApellido,lbl1erApellidoError), new CampoTextoConLabel(txbTelefono,lblTelefonoError), 
                new CampoTextoConLabel(txbCorreo,lblCorreoError),  new CampoTextoConLabel(txbCiudad,lblCiudadError),  new CampoTextoConLabel(txbColonia,lblColoniaError),  
                new CampoTextoConLabel(txbCalle,lblCalleError),  new CampoTextoConLabel(txbCodigoPostal,lblCodigoError),  new CampoTextoConLabel(txbNumeroExterior,lblNumeroExtError)
            };
            foreach (var campo in camposDeDatos)
            {
                if(!RevisarCampoVacio(campo.textBox.Text.Trim(), campo.labelError, CAMPO_VACIO)) {camposLlenos = false; }
            }
            if(rdbEmpleado.IsChecked == false && rdbCliente.IsChecked == false)
            {
                 lblTipoEmpleadoError.Content = CAMPO_VACIO;
            }
            else
            {
                lblTipoEmpleadoError.Content = String.Empty;
            }
            return camposLlenos;
        }

        private bool RevisarCampoVacio(String campoVerificar, Label labelDeCampo, string mensaje)
        {
            bool camposLlenos = true;
            if (String.IsNullOrEmpty(campoVerificar))
            {
                labelDeCampo.Content = mensaje;
                camposLlenos = false;
            }
            else
            {
                labelDeCampo.Content = String.Empty;
            }
            return camposLlenos;
        }

        private bool ValidarCamposLLenosEmpleado()
        {
            bool camposLlenos = true;
            if (String.IsNullOrEmpty(txbNombreUsuario.Text.Trim()))
            {
                lblNombreUsuarioError.Content = CAMPO_VACIO;
                camposLlenos = false;
            }
            else
            {
                lblNombreUsuarioError.Content = String.Empty;
            }
            if (String.IsNullOrEmpty(txbContrasena.Password.ToString().Trim()))
            {
                lblContrasena.Content = CAMPO_VACIO;
                camposLlenos = false;
            }
            else
            {
                lblContrasena.Content = String.Empty;
            }
            if (cbmTipoEmpleado.SelectedItem != null)
            {
                lblTipoEmpleadoError.Content = CAMPO_VACIO;
                camposLlenos = false;
            }
            else
            {
                lblTipoEmpleadoError.Content = String.Empty;
            }
            return camposLlenos;
        }

        private bool ValidarFormatos()
        {
            bool formatosValidos = true;
            if (!Regex.IsMatch(txbCorreo.Text.Trim(), EMAIL_RULES_CHAR) || !Regex.IsMatch(txbCorreo.Text.Trim(), EMAIL_ALLOW_CHAR))
            {
                lblCorreoError.Content = CORREO_INVALIDO;
                formatosValidos = false;
            }
            else
            {
                lblCorreoError.Content = String.Empty;
            }
            if(txbTelefono.Text.Length != 10)
            {
                lblTelefonoError.Content = TELEFONO_INVALIDO;
                formatosValidos = false;
            }
            else
            {
                lblTelefonoError.Content = String.Empty;
            }
            return formatosValidos;
        }

        private void EntryJustInteger(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true;
            }
        }

        private class CampoTextoConLabel
        {
            public TextBox textBox { get; set; }
            public Label labelError { get; set; }

            public CampoTextoConLabel(TextBox textBox, Label labelError)
            {
                this.textBox = textBox;
                this.labelError = labelError;
            }
        }

        private void BtnGuardarUsuario_Click(object sender, MouseButtonEventArgs e)
        {
            bool sePuedeGuardar = true;
            sePuedeGuardar = ValidarCamposLlenosUsuario();
            if(rdbEmpleado.IsChecked == true)
            {
                sePuedeGuardar = ValidarCamposLLenosEmpleado();
            }
            if (sePuedeGuardar)
            {
                sePuedeGuardar =  ValidarFormatos();
            }
            if (sePuedeGuardar)
            {

            }
            else
            {

            }
        }

        private void BtnCancelarRegistro(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
