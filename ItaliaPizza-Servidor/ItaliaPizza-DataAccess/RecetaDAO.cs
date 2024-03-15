using ItaliaPizza_Contratos.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class RecetaDAO
    {
        public List<Receta> RecuperarRecetas()
        {
            using (var context = new ItaliaPizzaEntities())
            {
                var resultado = from p in context.Productos
                                join r in context.Recetas on p.CodigoProducto equals r.CodigoProducto into recetasGroup
                                from r in recetasGroup.DefaultIfEmpty()
                                join pv in context.ProductosVenta on p.CodigoProducto equals pv.CodigoProducto into productosVentaGroup
                                from pv in productosVentaGroup.DefaultIfEmpty()
                                where pv != null && pv.CodigoProducto == "eee"
                                select new Receta
                                {
                                    Nombre = p.Nombre,
                                    Codigo = p.CodigoProducto,
                                    FotoProducto = pv != null ? pv.Foto : null
                                };

                return resultado.ToList();
            }

        }
    }
}
