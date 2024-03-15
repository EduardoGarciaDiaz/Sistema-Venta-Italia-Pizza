using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class AuxiliarUnificacionDatos
    {
        public static List<Categoria> UnificarListaCategorias(List<Categoria> categorias,
            List<CategoriasProductoVenta> categoriasProductoVenta,
            List<CategoriasInsumo> categoriasInsumo)
        {
            categorias.AddRange(categoriasProductoVenta.Select(categoriaProductoVenta => new Categoria
            {
                Id = categoriaProductoVenta.IdCategoriaProductoVenta,
                Nombre = categoriaProductoVenta.Nombre
            }));

            categorias.AddRange(categoriasInsumo.Select(categoriaInsumo => new Categoria
            {
                Id = categoriaInsumo.IdCategoriaInsumo,
                Nombre = categoriaInsumo.Nombre
            }));

            return categorias;
        }
    }
}
