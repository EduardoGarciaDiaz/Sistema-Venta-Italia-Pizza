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
            // TODO: TRY-CATCH

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

        public List<InsumoReceta> RecuperarInsumosReceta(int idReceta)
        {
            // TODO: TRY-CATCH

            List<InsumoReceta> insumosReceta = new List<InsumoReceta>();

            if (idReceta > 0)
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

                    return resultado.ToList();
                }
            }

            return insumosReceta;
        }

        public int GuardarReceta(Recetas receta)
        {
            int id = -1;

            using (var context = new ItaliaPizzaEntities())
            {
                context.Recetas.Add(receta);
                int filasAfectadas = context.SaveChanges();

                if (filasAfectadas > 0)
                {
                    id = receta.IdReceta;
                }
            }

            return id;
        }

        public int GuardarRecetaInsumos(List<RecetasInsumos> recetasInsumos)
        {
            int filasAfectadas = -1;

            using (var context = new ItaliaPizzaEntities())
            {
                context.RecetasInsumos.AddRange(recetasInsumos);
                filasAfectadas = context.SaveChanges();
            }

            return filasAfectadas;
        }

    }
}
