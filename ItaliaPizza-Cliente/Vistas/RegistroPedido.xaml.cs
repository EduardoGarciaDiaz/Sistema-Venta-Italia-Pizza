﻿using ItaliaPizza_Cliente.Recursos.ControlesUsuario;
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
    /// Lógica de interacción para RegistroPedido.xaml
    /// </summary>
    public partial class RegistroPedido : Page
    {
        public RegistroPedido()
        {
            InitializeComponent();
            BarraBusquedaClientes.AgregarClienteALista(new Cliente("Daniel", "arcosDaniel"));
            BarraBusquedaClientes.AgregarClienteALista(new Cliente("Luis", "arcosDaniel"));
            BarraBusquedaClientes.AgregarClienteALista(new Cliente("Flor", "arcosDaniel"));
            BarraBusquedaClientes.AgregarClienteALista(new Cliente("Wendy", "arcosDaniel"));
        }
    }
}
