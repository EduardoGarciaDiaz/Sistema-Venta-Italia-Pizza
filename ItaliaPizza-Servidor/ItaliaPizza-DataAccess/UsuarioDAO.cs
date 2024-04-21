using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
﻿using ItaliaPizza_Contratos.DTOs;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ItaliaPizza_DataAccess.Excepciones;

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
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
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
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
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
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
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
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return cliente;
        }


        public List<Usuarios> RecuperarClientesBD()
        {
            List<Usuarios> clientes = new List<Usuarios>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    clientes = context.Usuarios.Where(usuario => context.Empleados.FirstOrDefault(empleado => empleado.IdUsuario == usuario.IdUsuario) == null 
                                                      && context.Meseros.All(mesero => mesero.IdUsuario != usuario.IdUsuario)).Include(usuario => usuario.Direcciones).ToList();
                }
             }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
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
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
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
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            return resultadoOperacion;
        }

        public bool ValidarActualizacionCorreoUnico(string nuevoCorreo, int idUsuarioModificar)
        {
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    return !context.Usuarios.Any(usuario => usuario.CorreoElectronico.Equals(nuevoCorreo) && usuario.IdUsuario != idUsuarioModificar);
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
        }

        public int ActualizarUsuario(Usuarios clienteEdicion)
        {
            int filasAfectadas = -1;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var cliente = context.Usuarios.Find(clienteEdicion.IdUsuario);
                    if (cliente != null)
                    {
                        context.Entry(cliente).CurrentValues.SetValues(clienteEdicion);
                        filasAfectadas = context.SaveChanges();
                    }

                    return filasAfectadas;
                }
            }
            catch (EntityException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (SqlException ex)
            {
                ManejadorExcepcion.ManejarExcepcionError(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
            catch (Exception ex)
            {
                ManejadorExcepcion.ManejarExcepcionFatal(ex);
                throw new ExcepcionDataAccess(ex.Message);
            }
        }
    }
}
