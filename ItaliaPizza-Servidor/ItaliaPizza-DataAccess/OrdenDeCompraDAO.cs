using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess.Excepciones;

namespace ItaliaPizza_DataAccess
{
    public static class OrdenDeCompraDAO
    {

        public static int GuardarOrdenDeCompra(OrdenesCompra ordenesCompra)
        {


            int idOrdenNueva = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var estado = context.EstadosOrdenCompra.FirstOrDefault(ord => ord.Nombre.Equals("Borrador"));
                    ordenesCompra.IdEstadoOrdenCompra = estado.IdEstadoOrdenCompra;
                    ordenesCompra.EstadosOrdenCompra = estado;
                    context.OrdenesCompra.Add(ordenesCompra);
                    context.SaveChanges();
                    idOrdenNueva = ordenesCompra.IdOrdenCompra;
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
            return idOrdenNueva;
        }

        public static int GuardarInsumoOrdenDeCompra(List<OrdenesCompraInsumos> elementoInsumoOrdenesCompra)
        {
            int idOrdenNueva = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    context.OrdenesCompraInsumos.AddRange(elementoInsumoOrdenesCompra);
                    idOrdenNueva = context.SaveChanges();                     
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
            return idOrdenNueva;
        }

        

        public static OrdenesCompra RecuperarOrdenDeCompra(int idOrdenCompra)
        {
            OrdenesCompra ordenCompra = new OrdenesCompra();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    ordenCompra = context.OrdenesCompra.Include(ord => ord.Proveedores).Include(ord => ord.EstadosOrdenCompra).Include(ord => ord.OrdenesCompraInsumos).FirstOrDefault(ord => ord.IdOrdenCompra == idOrdenCompra);

                    if (ordenCompra != null)
                    {
                        var insumosRelacionados = ordenCompra.OrdenesCompraInsumos.Select(oci => oci.Insumos).ToList();                          
                        for (int i = 0; i < insumosRelacionados.Count; i++)
                        {
                            ordenCompra.OrdenesCompraInsumos.ToArray()[i].Insumos = insumosRelacionados.FirstOrDefault(ins => ins.CodigoProducto.Equals(ordenCompra.OrdenesCompraInsumos.ToArray()[i].CodigoProducto));
                        }
                        var productos = insumosRelacionados.Select(insumo => insumo.Productos).Distinct().ToList();
                        for (int i = 0; i < productos.Count; i++)
                        {
                            ordenCompra.OrdenesCompraInsumos.ToArray()[i].Insumos.Productos = productos.FirstOrDefault(ins => ins.CodigoProducto.Equals(ordenCompra.OrdenesCompraInsumos.ToArray()[i].CodigoProducto));
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
            return ordenCompra;
        }

        public static int CambiarEstadoOrdenCompraAEnviado(OrdenesCompra ordenesCompra)
        {
            int idOrdenNueva = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var estado = context.EstadosOrdenCompra.FirstOrDefault(ord => ord.Nombre.Equals("Enviada"));
                    ordenesCompra.IdEstadoOrdenCompra = estado.IdEstadoOrdenCompra;
                    ordenesCompra.EstadosOrdenCompra = estado;
                    context.OrdenesCompra.AddOrUpdate(ordenesCompra);                    
                    idOrdenNueva = context.SaveChanges(); 
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
            return idOrdenNueva;
        }

        public static double RecuperarSalidasDeOrdenesCompraPorFecha(DateTime fecha)
        {
            double salidasOrdenesCompra = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    List<OrdenesCompra> ordenesComprasFechaSeleccionada = context.OrdenesCompra.Where(p => DbFunctions.TruncateTime(p.Fecha) == fecha.Date).ToList();
                    foreach (OrdenesCompra ordenesCompraInsumos in ordenesComprasFechaSeleccionada)
                    {
                       salidasOrdenesCompra = salidasOrdenesCompra + (ordenesCompraInsumos.OrdenesCompraInsumos.Sum(p => p.Insumos.Costo ?? 0));
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
            return salidasOrdenesCompra;
        }

        public static List<OrdenDeCompraDto> RecuperarOrdenesDeCompra()
        {
            List<OrdenDeCompraDto> ordenesCompra = new List<OrdenDeCompraDto>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    ordenesCompra = context.OrdenesCompra.ToList().ConvertAll(o => new OrdenDeCompraDto()
                    {
                        IdOrdenCompra = o.IdOrdenCompra,
                        Costo = (float) o.Costo,
                        Fecha = (DateTime)o.Fecha,
                        IdEstadoOrdenCompra = o.EstadosOrdenCompra.IdEstadoOrdenCompra,
                        IdProveedor = o.Proveedores.IdProveedor,
                        Proveedor = new ProveedorDto()
                        {
                            RFC = o.Proveedores.RFC,
                            CorreoElectronico = o.Proveedores.CorreoElectronico,
                            EsActivo = o.Proveedores.EsActivo ?? false,
                            Direccion = new DireccionDto()
                            {
                                IdDireccion = o.Proveedores.Direcciones.IdDireccion,
                                Calle = o.Proveedores.Direcciones.Calle,
                                Ciudad = o.Proveedores.Direcciones.Ciudad,
                                CodigoPostal = o.Proveedores.Direcciones.CodigoPostal,
                                Colonia = o.Proveedores.Direcciones.Colonia,
                                Numero = (int)o.Proveedores.Direcciones.Numero
                            },
                            IdDireccion = o.Proveedores.Direcciones.IdDireccion,
                            IdProveedor = o.Proveedores.IdProveedor,
                            NombreCompleto = o.Proveedores.NombreCompleto,
                            NumeroTelefono = o.Proveedores.NumeroTelefono
                        },
                        ListaElementosOrdenCompra = o.OrdenesCompraInsumos.ToList().ConvertAll(l => new ElementoOrdenCompraDto()
                        {
                            CantidadInsumosAdquiridos = (int) l.CantidadInsumosAdquiridos,
                            IdElementoOrdenCompra = l.IdOrdenCompraInsumo,
                            InsumoOrdenCompraDto = new InsumoOrdenCompraDto()
                            {
                                Codigo = l.Insumos.CodigoProducto,
                                CostoUnitario = (float) l.Insumos.Costo,
                                Nombre = context.Productos.FirstOrDefault(p => p.CodigoProducto == l.Insumos.CodigoProducto).Nombre,
                                UnidadMedida = l.Insumos.UnidadesMedida.Nombre
                            }
                        })
                    });
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
            return ordenesCompra;
        }

        public static bool RegistrarPagoOrdenCompra(OrdenDeCompraDto ordenDeCompra)
        {
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    OrdenesCompra ordenesCompra = context.OrdenesCompra.FirstOrDefault(o => o.IdOrdenCompra == ordenDeCompra.IdOrdenCompra);
                    if (ordenesCompra != default)
                    {
                        ordenesCompra.IdEstadoOrdenCompra = ordenDeCompra.IdEstadoOrdenCompra;
                        ordenesCompra.Costo = ordenDeCompra.Costo;
                        InsumoDAO insumoDAO = new InsumoDAO();
                        foreach(OrdenesCompraInsumos ordenesCompraInsumos in ordenesCompra.OrdenesCompraInsumos)
                        {
                            double cantidad = ordenDeCompra.ListaElementosOrdenCompra.FirstOrDefault(i => 
                                i.IdElementoOrdenCompra == ordenesCompraInsumos.IdOrdenCompraInsumo).CantidadInsumosAdquiridos;
                            insumoDAO.ActualizarCantidadSolicitadaInsumo(ordenesCompraInsumos.IdOrdenCompraInsumo, cantidad);
                            insumoDAO.ActualizarInventarioInsumo(ordenesCompraInsumos.Insumos.CodigoProducto, cantidad);
                        }
                        if (context.SaveChanges() > 0)
                        {
                            return true;
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

            return false;
        }

    }
}



  
