using ItaliaPizza_Contratos.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class InsumoDAO
    {
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
