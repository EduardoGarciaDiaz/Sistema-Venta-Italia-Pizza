using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess.Excepciones;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class RecetaDAO
    {
        public List<Receta> RecuperarRecetas()
        {
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var resultado = from p in context.Productos
                                    join pv in context.ProductosVenta on p.CodigoProducto equals pv.CodigoProducto
                                    join r in context.Recetas on pv.CodigoProducto equals r.CodigoProducto
                                    where pv.CodigoProducto == r.CodigoProducto
                                    select new Receta
                                    {
                                        Nombre = p.Nombre,
                                        Codigo = p.CodigoProducto,
                                        FotoProducto = pv.Foto,
                                        Id = r.IdReceta
                                    };

                    return resultado.ToList();
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

        public List<InsumoReceta> RecuperarInsumosReceta(int idReceta)
        {
            List<InsumoReceta> insumosReceta = new List<InsumoReceta>();

            if (idReceta > 0)
            {
                try
                {
                    using (var context = new ItaliaPizzaEntities())
                    {
                        var resultado = from p in context.Productos
                                        join i in context.Insumos on p.CodigoProducto equals i.CodigoProducto
                                        join ri in context.RecetasInsumos on i.CodigoProducto equals ri.CodigoProducto
                                        join r in context.Recetas on ri.IdReceta equals r.IdReceta
                                        join um in context.UnidadesMedida on i.IdUnidadMedida equals um.IdUnidadMedida
                                        where r.IdReceta == idReceta
                                        select new InsumoReceta
                                        {
                                            Nombre = p.Nombre,
                                            Cantidad = (double)ri.CantidadInsumo,
                                            UnidadMedida = new UnidadMedida()
                                            {
                                                Id = um.IdUnidadMedida,
                                                Nombre = um.Nombre
                                            }
                                        };

                        insumosReceta = resultado.ToList();
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

            return insumosReceta;
        }

        public int GuardarReceta(Recetas receta)
        {
            int id = -1;

            if (receta != null)
            {
                try
                {
                    using (var context = new ItaliaPizzaEntities())
                    {
                        context.Recetas.Add(receta);
                        int filasAfectadas = context.SaveChanges();

                        if (filasAfectadas > 0)
                        {
                            id = receta.IdReceta;
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
            }

            return id;
        }

        public int GuardarRecetaInsumos(List<RecetasInsumos> recetasInsumos)
        {
            int filasAfectadas = -1;

            if (recetasInsumos != null)
            {
                try
                {
                    using (var context = new ItaliaPizzaEntities())
                    {
                        context.RecetasInsumos.AddRange(recetasInsumos);
                        filasAfectadas = context.SaveChanges();
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

            return filasAfectadas;
        }

        public List<RecetasInsumos> RecuperarInsumosEnReceta(string codigoProducto)
        {
            List<RecetasInsumos> insumosDeReceta = new List<RecetasInsumos>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    insumosDeReceta = context.Recetas.FirstOrDefault(r => r.CodigoProducto == codigoProducto)
                        .RecetasInsumos.ToList();
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

            return insumosDeReceta;
        }

        public int EliminarReceta(int idReceta)
        {
            int filasAfectadas = -1;

            if (idReceta > 0)
            {
                try
                {
                    using (var context = new ItaliaPizzaEntities())
                    {
                        Recetas recetaAEliminar = context.Recetas.FirstOrDefault(r => r.IdReceta == idReceta);

                        if (recetaAEliminar != null)
                        {
                            context.Recetas.Remove(recetaAEliminar);
                            filasAfectadas = context.SaveChanges();
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
            }

            return filasAfectadas;
        }

        public int EliminarRecetasInsumos(int idReceta)
        {
            int filasAfectadas = -1;

            if (idReceta > 0)
            {
                try
                {
                    using (var context = new ItaliaPizzaEntities())
                    {
                        var recetasInsumosAEliminar = context.RecetasInsumos.Where(ri => ri.IdReceta == idReceta).ToList();

                        if (recetasInsumosAEliminar != null)
                        {
                            foreach (var receta in recetasInsumosAEliminar)
                            {
                                context.RecetasInsumos.Remove(receta);
                            }

                            filasAfectadas = context.SaveChanges();
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
            }

            return filasAfectadas;
        }
    }
}
