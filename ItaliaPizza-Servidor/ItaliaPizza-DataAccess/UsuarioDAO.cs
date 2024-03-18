using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
﻿using ItaliaPizza_Contratos.DTOs;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public static int GuardarUsuarioNuevoBD(Usuarios usuarioNuevo)
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

        public bool CorreoEsUnico(String correo)
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

        public Cliente RecuperarDatosClientePorId(int id)
        {
            Cliente cliente = new Cliente();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    Usuarios usuarioCliente = context.Usuarios.FirstOrDefault(c => c.IdUsuario == id);
                    if (usuarioCliente != null)
                    {
                        cliente.IdCliente = usuarioCliente.IdUsuario;
                        cliente.NombreCliente = usuarioCliente.NombreCompleto;
                        cliente.CorreoElectronicoCliente = usuarioCliente.CorreoElectronico;
                        cliente.NumeroTelefonoCliente = usuarioCliente.NumeroTelefono;
                        cliente.DireccionCliente =
                            usuarioCliente.Direcciones.Calle + ", " +
                            usuarioCliente.Direcciones.Numero + ". " +
                            usuarioCliente.Direcciones.Colonia + ". " +
                            usuarioCliente.Direcciones.CodigoPostal + ". " +
                            usuarioCliente.Direcciones.Ciudad;
                    }
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
            return cliente;
        }

    }
}
