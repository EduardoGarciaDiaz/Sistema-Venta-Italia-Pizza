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
    /// Interaction logic for ElementoProveedor.xaml
    /// </summary>
    public partial class ElementoProveedor : UserControl
    {
        public EventHandler BtnModificarProveedorClicked;
        public EventHandler BtnCambiarEstadoProveedorClicked;
        public int Id { get; set; }
        public ProveedorDto ProveedorDto { get; set; }
        public bool EsActivo { get; set; }

        public ElementoProveedor(ProveedorDto proveedor)
        {
            InitializeComponent();
            this.ProveedorDto = proveedor;
            CargarInformacion();
            MostrarSiEstaActivo(EsActivo);
        }

        private void BtnModificarProveedor_Click(object sender, MouseButtonEventArgs e)
        {
            BtnModificarProveedorClicked?.Invoke(this, e);
        }

        private void BtnCambiarEstadoProveedor_Click(object sender, MouseButtonEventArgs e)
        {
            BtnCambiarEstadoProveedorClicked?.Invoke(this, e);
        }

        public void CambiarEstado(bool estadoActivo)
        {
            if (estadoActivo)
            {
                brdActivoBackGorund.Background = new SolidColorBrush(Colors.Black);
                btnEsActivo.HorizontalAlignment = HorizontalAlignment.Right;
                lblModificarEstado.Content = "Desactivar";
            }
            else
            {
                brdActivoBackGorund.Background = new SolidColorBrush(Colors.Red);
                btnEsActivo.HorizontalAlignment = HorizontalAlignment.Left;
                lblModificarEstado.Content = "Activar";
            }
            this.EsActivo = estadoActivo;
        }

        private void CargarInformacion()
        {
            Id = ProveedorDto.IdProveedor;
            lblNombre.Text = ProveedorDto.NombreCompleto.ToString(); 
            String direccion = ProveedorDto.Direccion.Ciudad + ", Col. " + ProveedorDto.Direccion.Colonia.ToString() + " " + ProveedorDto.Direccion.CodigoPostal + ", Calle " + ProveedorDto.Direccion.Calle + " #" + ProveedorDto.Direccion.Numero.ToString();
            lblDireccion.Text = direccion;
            lblCorreo.Text = ProveedorDto.CorreoElectronico.ToString();
            String telefono = ProveedorDto.NumeroTelefono.ToString();
            lblTelefono.Text = "(+52) " + telefono.Substring(0, 3) + "-" + telefono.Substring(3, 3) + "-" + telefono.Substring(6, 2) + "-" + telefono.Substring(8);
            lblRFC.Text = ProveedorDto.RFC;
            this.EsActivo = ProveedorDto.EsActivo;
        }

        private void MostrarSiEstaActivo(bool esActivo)
        {
            if (esActivo)
            {
                brdActivoBackGorund.Background = new SolidColorBrush(Colors.Black);
                btnEsActivo.HorizontalAlignment = HorizontalAlignment.Right;
                lblModificarEstado.Content = "Desactivar";
            }
            else
            {
                brdActivoBackGorund.Background = new SolidColorBrush(Colors.Red);
                btnEsActivo.HorizontalAlignment = HorizontalAlignment.Left;
                lblModificarEstado.Content = "Activar";
            }
        }               


    }
}
