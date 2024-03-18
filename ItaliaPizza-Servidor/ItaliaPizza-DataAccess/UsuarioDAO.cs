using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
﻿using ItaliaPizza_Contratos.DTOs;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ItaliaPizza_DataAccess
{
    public class UsuarioDAO
    {

        public UsuarioDAO() { }



        public List<Usuarios> RecuperarClientesPorNombre(string nombre)
        {
            List<Usuarios> clientes = new List<Usuarios>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    clientes = context.Usuarios.Where(usuario =>
                    usuario.NombreCompleto.Contains(nombre)
                    && usuario.EsActivo == true
                    && context.Empleados.FirstOrDefault(empleado =>
                        empleado.IdUsuario == usuario.IdUsuario) == null)
                        .ToList();
                }
            }
            catch (EntityException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (SqlException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }

            return clientes;
        }


        public  int GuardarUsuarioNuevoBD(Usuarios usuarioNuevo)
        {
            int resultadoOperacion = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.Usuarios.Add(usuarioNuevo);
                    context.SaveChanges();
                    resultadoOperacion = usuarioNuevo.IdUsuario;
                }
            }
            catch (EntityException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (SqlException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            return resultadoOperacion;
        }

        public  bool CorreoEsUnico(String correo)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.Usuarios.Any(usuario => usuario.CorreoElectronico.Equals(correo));
                }
            }
            catch (Exception ex)
            {
                resultadoOperacion = false;
            }
            return resultadoOperacion;
        }

        public List<Usuarios> RecuperarClientesBD()
        {
            List<Usuarios> clientes = new List<Usuarios>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    clientes = context.Usuarios.Where(usuario => context.Empleados.FirstOrDefault(empleado =>
                    empleado.IdUsuario == usuario.IdUsuario) == null).ToList();
                    
                }
            }
            catch (EntityException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (SqlException ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                //TODO: Manejar excepcion
                Console.WriteLine(ex.StackTrace);
            }
            return clientes;
        }

        public bool DesactivarUsuario(int idUsuario)
        {
            bool resultadoOperacion = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    var usuario = context.Usuarios.FirstOrDefault(usuruio => usuruio.IdUsuario == idUsuario);
                    usuario.EsActivo = false;
                    int resultado = context.SaveChanges();
                    if(resultado != 0)
                    {
                        resultadoOperacion = true;
                    }
                }
            }
            catch (Exception ex)
            {
                resultadoOperacion = false;
            }
            return resultadoOperacion;
        }

        public bool ActivarUsuario(int idUsuario)
        {
            bool resultadoOperacion = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    var usuario = context.Usuarios.FirstOrDefault(usuruio => usuruio.IdUsuario == idUsuario);
                    usuario.EsActivo = true;
                    int resultado = context.SaveChanges();
                    if (resultado != 0)
                    {
                        resultadoOperacion = true;
                    }
                }
            }
            catch (Exception ex)
            {
                resultadoOperacion = false;
            }
            return resultadoOperacion;
        }


        //ES DEL DAO PEDIDOS
        public bool ValidarClienteTienePedidosPendientes(int idUsuario)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.UsuariosPedidos.Any(pedido => pedido.IdCliente == idUsuario &&
                                                              context.Pedidos.FirstOrDefault(ped => ped.NumeroPedido == pedido.NumeroPedido).IdEstadoPedido ==
                                                              context.EstadosPedido.FirstOrDefault(estado => estado.Nombre.Equals("En proceso")).IdEstadoPedido);                   
                }
            }
            catch (Exception ex)
            {
                resultadoOperacion = false;
            }
            return resultadoOperacion;
        }

        //ES DEL DAO PEIDIDOS
       

    }
}
