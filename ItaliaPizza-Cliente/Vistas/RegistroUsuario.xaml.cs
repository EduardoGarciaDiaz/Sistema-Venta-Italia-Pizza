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
    /// Interaction logic for RegistroUsuario.xaml
    /// </summary>
    public partial class RegistroUsuario : Page
    {
        public RegistroUsuario()
        {
            InitializeComponent();
        }

        private void PrepareWindow()
        {

        }

        private void ObtenerTiposEmpleados()
        {
            //Recuperar Empleados de servicio
        }

        private bool ValidarCamposLlenos()
        {
            bool camposLlenos = true;
            if (String.IsNullOrEmpty(txbNombre.Text.Trim()))
            {
                camposLlenos = false;
            }
            return camposLlenos;

        }

        private void MostrarLabelError(Label labelAMostrar, string mensaje)
        {
            labelAMostrar.Content = mensaje;
            labelAMostrar.Visibility = Visibility.Visible;
        }

        private void BtnGuardarUsuario_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnCancelarRegistro(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
