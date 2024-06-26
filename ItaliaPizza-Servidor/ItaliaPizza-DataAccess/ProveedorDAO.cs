﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza_DataAccess.Excepciones;
using System.Data.Entity.Migrations;

namespace ItaliaPizza_DataAccess
{
    public static class ProveedorDAO
    {

        public static List<Proveedores> RecuperarProveedoresBD()
        {
            List<Proveedores> proveedores = new List<Proveedores>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    proveedores = context.Proveedores.Include(ins => ins.Direcciones).ToList();

                    return proveedores.ToList();
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

        public static List<Proveedores> RecuperarProveedoresActivosBD()
        {
            List<Proveedores> proveedores = new List<Proveedores>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    proveedores = context.Proveedores.Include(ins => ins.Direcciones).Where(prove => (bool)prove.EsActivo).ToList();

                    return proveedores.ToList();
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

        public static bool GuardarProveedorNuevoBD(Proveedores proveedorNuevo)
        {
            bool exitoOperacion = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.Proveedores.Add(proveedorNuevo);
                    int filasAfectadas = context.SaveChanges();
                    if(filasAfectadas > 0)
                    {
                        exitoOperacion = true;
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
            return exitoOperacion;
        }

        public static bool ActualizarProveedorBD(Proveedores proveedor)
        {
            bool exitoOperacion = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var proveedorIgual = context.Proveedores.Include("Direcciones").Include("OrdenesCompra").FirstOrDefault(prove => prove.IdProveedor == proveedor.IdProveedor);                    
                    if(proveedorIgual != null)
                    {
                        proveedor.OrdenesCompra =  proveedorIgual.OrdenesCompra;
                        if (proveedorIgual.Equals(proveedor))
                        {
                            exitoOperacion = true;
                        }
                        else
                        {
                            context.Proveedores.AddOrUpdate(proveedor);
                            context.Direcciones.AddOrUpdate(proveedor.Direcciones);
                            context.SaveChanges();
                            exitoOperacion = true;
                        }
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
            return exitoOperacion;
        }

        public static  bool ValidarCorreoUnicoProveedorBD(string correo)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.Proveedores.Any(proveedor => proveedor.CorreoElectronico.Equals(correo));
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

        public static bool ValidarRfcUnicoProveedorBD(string rfc)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.Proveedores.Any(proveedor => proveedor.RFC.Equals(rfc));
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

        public static bool ValidarCorreoUnicoProveedorBD(string correo, int idProveedor)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.Proveedores.Any(proveedor => proveedor.CorreoElectronico.Equals(correo) && idProveedor != proveedor.IdProveedor);
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

        public static bool ValidarRfcUnicoProveedorBD(string rfc, int idProveedor)
        {
            bool resultadoOperacion;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    resultadoOperacion = !context.Proveedores.Any(proveedor => proveedor.RFC.Equals(rfc) &&  idProveedor != proveedor.IdProveedor);
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


        public static bool ActivarProveedor(int idProveedor)
        {
            bool resultadoOperacion = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    var proveedor = context.Proveedores.FirstOrDefault(prove => prove.IdProveedor == idProveedor);
                    proveedor.EsActivo = true;
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

        public static bool DesactivarProveedor(int idProveedor)
        {
            bool resultadoOperacion = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    var proveedor = context.Proveedores.FirstOrDefault(prove => prove.IdProveedor == idProveedor);
                    proveedor.EsActivo = false;
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

    }
}
