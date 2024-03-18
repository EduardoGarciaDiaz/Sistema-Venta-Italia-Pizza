using ItaliaPizza_Contratos.DTOs;
using System;
using System.Collections.Generic;
﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            return insumoDisminuido;
        }

                public List<InsumoRegistroReceta> RecuperarInsumos()
        {
            using (var context = new ItaliaPizzaEntities())
            {
                var insumos = from p in context.Productos
                              join i in context.Insumos on p.CodigoProducto equals i.CodigoProducto
                              join um in context.UnidadesMedida on i.IdUnidadMedida equals um.IdUnidadMedida
                              join ci in context.CategoriasInsumo on i.IdCategoriaInsumo equals ci.IdCategoriaInsumo
                              where !context.ProductosVenta.Any(i => i.CodigoProducto == p.CodigoProducto)
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

    }
}
