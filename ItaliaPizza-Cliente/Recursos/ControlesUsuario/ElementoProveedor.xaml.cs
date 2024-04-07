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
        public EventHandler btnModificarProveedor_Click;
        public EventHandler btnCambiarEstadoProveedor_Click;
        public int Id;
        public ProveedorDto proveedorDto { get; set; }
        public bool esActivo { get; set; }

        public ElementoProveedor(ProveedorDto proveedor)
        {
            InitializeComponent();
            this.proveedorDto = proveedor;
            CargarInformacion();
            MostrarSiEstaActivo(esActivo);
        }

        private void CargarInformacion()
        {
            Id = proveedorDto.IdProveedor;
            lblNombre.Text = proveedorDto.NombreCompleto.ToString(); 
            String direccion = proveedorDto.Direccion.Ciudad + ", Col. " + proveedorDto.Direccion.Colonia.ToString() + " " + proveedorDto.Direccion.CodigoPostal + ", Calle " + proveedorDto.Direccion.Calle + " #" + proveedorDto.Direccion.Numero.ToString();
            lblDireccion.Text = direccion;
            lblCorreo.Text = proveedorDto.CorreoElectronico.ToString();
            String telefono = proveedorDto.NumeroTelefono.ToString();
            lblTelefono.Text = "(+52) " + telefono.Substring(0, 3) + "-" + telefono.Substring(3, 3) + "-" + telefono.Substring(6, 2) + "-" + telefono.Substring(8);
            lblRFC.Text = proveedorDto.RFC;
            this.esActivo = proveedorDto.EsActivo;
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
            this.esActivo = estadoActivo;
        }

        private void BtnModificarProveedor_Click(object sender, MouseButtonEventArgs e)
        {
            btnModificarProveedor_Click?.Invoke(this, e);
        }

        private void BtnCambiarEstadoProveedor_Click(object sender, MouseButtonEventArgs e)
        {
            btnCambiarEstadoProveedor_Click?.Invoke(this, e);
        }


    }
}
