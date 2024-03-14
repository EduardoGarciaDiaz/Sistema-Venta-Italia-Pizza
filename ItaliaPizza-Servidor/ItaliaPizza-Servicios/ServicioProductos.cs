using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioProductos
    {
        public void OperacionProductosEjemplo()
        {
            throw new NotImplementedException();
        }

        public List<Categoria> RecuperarCategorias()
        {
            List<Categoria> categorias = new List<Categoria>();
            GestionProducto gestionProducto = new GestionProducto();

            List<CategoriasInsumo> categoriasInsumo = new List<CategoriasInsumo>();
            List<CategoriasProductoVenta> categoriasProductoVenta = new List<CategoriasProductoVenta>();

            categoriasInsumo = gestionProducto.RecuperarCategoriasInsumo();
            categoriasProductoVenta = gestionProducto.RecuperarCategoriasProductoVenta();

            categorias = PrepararListaCategorias(categorias, categoriasProductoVenta, categoriasInsumo);

            return categorias;
        }

        private List<Categoria> PrepararListaCategorias(List<Categoria> categorias,
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


        public bool ValidarCodigoProducto(string codigoProducto)
        {
            throw new NotImplementedException();
        }

        public int GuardarProducto(Producto producto)
        {
            throw new NotImplementedException();
        }




    }
}
