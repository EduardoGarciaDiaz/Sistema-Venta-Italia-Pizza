﻿using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
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
    /// Lógica de interacción para RegistroPagoOrdenCompra.xaml
    /// </summary>
    public partial class RegistroPagoOrdenCompra : Page
    {

        private OrdenDeCompraDto _ordenCompra;
        private const int VENTANA_INFORMACION = 2;

        public RegistroPagoOrdenCompra(OrdenDeCompraDto ordenCompra)
        {
            InitializeComponent();
            this._ordenCompra = ordenCompra;
            this.Loaded += PrepararPagina_Loaded;
        }

        private void PrepararPagina_Loaded (object sender, RoutedEventArgs e)
        {
            MostrarDatosOrdenCompra(_ordenCompra);
            MostrarProveedor(_ordenCompra.Proveedor);
            MostrarInsumos(_ordenCompra.listaElementosOrdenCompra.ToList());
            CalcularYMostrarTotales();
        }

        private void MostrarInsumos(List<ElementoOrdenCompraDto> insumos)
        {
            insumos?.ForEach(insumo =>
            {
                var insumoOrdenCompra = new ElementoInsumoRegistroPagoOrden
                { 
                      lblNombreInsumo = { Content = insumo.InsumoOrdenCompraDto.Nombre },
                      lblCodigoInsumo = { Content = insumo.InsumoOrdenCompraDto.Codigo },
                      lblNombreUnidadMedida = { Content = insumo.InsumoOrdenCompraDto.UnidadMedida },
                      lblCostoInsumo = { Content = insumo.InsumoOrdenCompraDto.CostoUnitario },
                      lblTotalInsumos = { Content = insumo.CantidadInsumosAdquiridos * insumo.InsumoOrdenCompraDto.CostoUnitario },
                      tbxCantidadInsumo = { Text = insumo.CantidadInsumosAdquiridos.ToString("F2") },
                      Insumo = insumo.InsumoOrdenCompraDto
                };
                insumoOrdenCompra.TextChanged += TextCantidad_Changed;

                SkpContenedorOrdenesCompra.Children.Add(insumoOrdenCompra);
            });
        }

        private void TextCantidad_Changed(object sender, EventArgs e)
        {
            double cantidad = 0;
            ElementoInsumoRegistroPagoOrden elementoUI = sender as ElementoInsumoRegistroPagoOrden;
            if (!string.IsNullOrEmpty(elementoUI.tbxCantidadInsumo.Text))
            {
                cantidad = double.Parse((sender as ElementoInsumoRegistroPagoOrden).tbxCantidadInsumo.Text);
            }
            ActualizarTotalInsumo(cantidad, sender as ElementoInsumoRegistroPagoOrden);
            CalcularYMostrarTotales();
        }

        private void CalcularYMostrarTotales()
        {
            List<ElementoInsumoRegistroPagoOrden> insumos = SkpContenedorOrdenesCompra.Children.OfType<ElementoInsumoRegistroPagoOrden>().ToList();
            double total = insumos.Sum(i => double.Parse(i.lblTotalInsumos.Content.ToString()));
            double subtotal = total / 1.16;
            double iva = subtotal * 0.16;

            lblSubtotal.Content = subtotal.ToString("F2");
            lblIVA.Content = iva.ToString("F2");
            lblTotal.Content = total.ToString("F2");
        }

        private void ActualizarTotalInsumo(double cantidad, ElementoInsumoRegistroPagoOrden elementoInsumoRegistroPagoOrden)
        {
            elementoInsumoRegistroPagoOrden.lblTotalInsumos.Content = (cantidad * elementoInsumoRegistroPagoOrden.Insumo.CostoUnitario);
        }

        private void MostrarProveedor(ProveedorDto proveedor)
        {
            lblNombreProveedor.Content = proveedor.NombreCompleto;
            lblCorreoProveedor.Content = proveedor.CorreoElectronico;
            lblRFCProveedor.Content = proveedor.RFC;
            lblNumeroTelefonoProveedor.Content = proveedor.NumeroTelefono;
            DireccionDto direccion = proveedor.Direccion;
            tbxDireccionProveedor.Text = direccion.Calle + " #" +
                direccion.Numero + ", " +
                direccion.Colonia + ". " +
                direccion.CodigoPostal + ". " +
                direccion.Ciudad + ", ";
        }

        private void MostrarDatosOrdenCompra(OrdenDeCompraDto ordenCompra)
        {
            lblNumeroOrden.Content = ordenCompra.IdOrdenCompra;
            lblFecha.Content = ordenCompra.Fecha.ToShortDateString();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            ConsultaOrdenesDeCompra consultaOrdenesDeCompra = new ConsultaOrdenesDeCompra();
            NavigationService.Navigate(consultaOrdenesDeCompra);
        }

        private void BtnRegistrarPago_Click(object sender, RoutedEventArgs e)
        {
            _ordenCompra.IdEstadoOrdenCompra = (int)EnumEstadosOrdenCompra.Surtida;
            List<ElementoInsumoRegistroPagoOrden> insumos = SkpContenedorOrdenesCompra.Children.OfType<ElementoInsumoRegistroPagoOrden>().ToList();
            insumos?.ForEach(i =>
            {
                _ordenCompra.listaElementosOrdenCompra.FirstOrDefault(insumo =>
                    insumo.InsumoOrdenCompraDto.Codigo == i.lblCodigoInsumo.Content.ToString())
                    .CantidadInsumosAdquiridos = int.Parse(i.tbxCantidadInsumo.Text.ToString());
            });
            ServicioOrdenesCompraClient servicioOrdenesCompraCliente = new ServicioOrdenesCompraClient();
            bool ordenActualizada = servicioOrdenesCompraCliente.RegistrarPagoOrdenCompra(_ordenCompra);
            if (ordenActualizada)
            {
                ManejarRegistroExitoso();   
            }

        }

        private void ManejarRegistroExitoso()
        {
            string tituloExito = "Registro de pago exitoso";
            string mensajeExito = "¡El pago de la orden se registró con éxito!";

            VentanaEmergente ventanaEmergente = new VentanaEmergente(tituloExito, mensajeExito, Window.GetWindow(this), VENTANA_INFORMACION);
            ventanaEmergente.ShowDialog();

            ConsultaOrdenesDeCompra consultaOrdenesDeCompra = new ConsultaOrdenesDeCompra();
            NavigationService.Navigate(consultaOrdenesDeCompra);
        }
    }
}