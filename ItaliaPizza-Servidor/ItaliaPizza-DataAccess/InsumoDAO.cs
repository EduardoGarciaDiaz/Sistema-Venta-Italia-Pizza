using ItaliaPizza_Contratos.DTOs;
using System;
using System.Collections.Generic;
﻿using System;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ItaliaPizza_DataAccess.Excepciones;

namespace ItaliaPizza_DataAccess
{
    public class InsumoDAO
    {
        public InsumoDAO() { }

        public bool ValidarDisponibilidadInsumo(string codigoInsumo, int cantidadRequerida)
        {
            bool insumoDisponible = false;

            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    Insumos insumo = context.Insumos.FirstOrDefault(i => i.CodigoProducto == codigoInsumo);
                    if (insumo != default)
                    {
                        insumoDisponible = insumo.Cantidad >= cantidadRequerida;
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
            return insumoDisponible;
        }

        public bool DisminuirCantidadInsumo(string codigoInsumo, int cantidadParaDisminuir)
        {
            bool insumoDisminuido = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    Insumos insumo = context.Insumos.FirstOrDefault(i => i.CodigoProducto == codigoInsumo);
                    if (insumo != default)
                    {
                        insumo.Cantidad = insumo.Cantidad - cantidadParaDisminuir;
                    }
                    int registrosAfectados = context.SaveChanges();
                    if (registrosAfectados > 0)
                    {
                        insumoDisminuido = true;
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

            return insumoDisminuido;
        }

        public List<InsumoRegistroReceta> RecuperarInsumos()
        {
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    var insumos = from p in context.Productos
                                  join i in context.Insumos on p.CodigoProducto equals i.CodigoProducto
                                  join um in context.UnidadesMedida on i.IdUnidadMedida equals um.IdUnidadMedida
                                  join ci in context.CategoriasInsumo on i.IdCategoriaInsumo equals ci.IdCategoriaInsumo
                                  where !context.ProductosVenta.Any(i => i.CodigoProducto == p.CodigoProducto)
                                  && p.EsActivo == true
                                  select new InsumoRegistroReceta
                                  {
                                      Codigo = p.CodigoProducto,
                                      Nombre = p.Nombre,
                                      Categoria = new Categoria()
                                      {
                                          Id = ci.IdCategoriaInsumo,
                                          Nombre = ci.Nombre
                                      },
                                      UnidadMedida = new UnidadMedida()
                                      {
                                          Id = um.IdUnidadMedida,
                                          Nombre = um.Nombre
                                      }
                                  };

                    return insumos.ToList();
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

        public List<Insumos> RecuperarInsumosActivos()
        {
            List<Insumos> insumos = new List<Insumos>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    insumos = context.Insumos.Where(insumo => insumo.Productos.EsActivo == true).Include(ins => ins.Productos).Include(ins => ins.UnidadesMedida).ToList();

                return insumos.ToList();
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
            return insumos;
        }

    }
}
