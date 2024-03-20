using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ItaliaPizza_PruebasUnitarias.PruebasDataAccess
{
    public class ConfiguracionPruebaCategoriaDAO : IDisposable
    {
        public ConfiguracionPruebaCategoriaDAO() { }

        public void Dispose() { }
    }


    public class PruebaCategoriaDAO : IClassFixture<ConfiguracionPruebaCategoriaDAO>
    {
        private readonly ConfiguracionPruebaCategoriaDAO _configuracion;

        public PruebaCategoriaDAO(ConfiguracionPruebaCategoriaDAO configuracion)
        {
            _configuracion = configuracion;
        }

        [Fact]
        public void PruebaRecuperarCategoriasInsumoExitosa()
        {
            CategoriaDAO categoriaDAO = new CategoriaDAO();

            List<CategoriasInsumo> resultadoEsperado = new List<CategoriasInsumo>
            {
                new CategoriasInsumo { Nombre = "Quesos" },
                new CategoriasInsumo { Nombre = "Carnes" },
                new CategoriasInsumo { Nombre = "Vegetales"},
                new CategoriasInsumo { Nombre = "Lacteos" }
            };

            List<CategoriasInsumo> resultadoObtenido = categoriaDAO.RecuperarCategoriasInsumo();

            Assert.NotNull(resultadoObtenido);
            Assert.NotEmpty(resultadoObtenido);
            Assert.Equal(resultadoEsperado.Select(rp => rp.Nombre), resultadoObtenido.Select(ro => ro.Nombre));
        }

        [Fact]
        public void PruebaRecuperarCategoriasInsumoFallida()
        {
            CategoriaDAO categoriaDAO = new CategoriaDAO();

            List<CategoriasInsumo> resultadoEsperado = new List<CategoriasInsumo>
            {
                new CategoriasInsumo { Nombre = "Quesos" },
                new CategoriasInsumo { Nombre = "Carnes" },
                new CategoriasInsumo { Nombre = "Vegetales"},
                new CategoriasInsumo { Nombre = "Lacteos" }
            };

            List<CategoriasInsumo> resultadoObtenido = categoriaDAO.RecuperarCategoriasInsumo();

            Assert.NotEmpty(resultadoObtenido);
            Assert.NotEqual(resultadoEsperado.Count, resultadoObtenido.Count + 1);
        }

        [Fact]
        public void PruebaRecuperarCategoriasProductoVentaExitosa()
        {
            CategoriaDAO categoriaDAO = new CategoriaDAO();

            List<CategoriasProductoVenta> resultadoEsperado = new List<CategoriasProductoVenta>
            {
                new CategoriasProductoVenta { Nombre = "Pizzas" },
                new CategoriasProductoVenta { Nombre = "Bebidas" },
                new CategoriasProductoVenta { Nombre = "Postres"}
            };

            List<CategoriasProductoVenta> resultadoObtenido = categoriaDAO.RecuperarCategoriasProductoVenta();

            Assert.NotNull(resultadoObtenido);
            Assert.NotEmpty(resultadoObtenido);
            Assert.Equal(resultadoEsperado.Select(rp => rp.Nombre), resultadoObtenido.Select(ro => ro.Nombre));
        }


        [Fact]
        public void PruebaRecuperarCategoriasProductoVentaFallida()
        {
            CategoriaDAO categoriaDAO = new CategoriaDAO();

            List<CategoriasProductoVenta> resultadoEsperado = new List<CategoriasProductoVenta>
            {
                new CategoriasProductoVenta { Nombre = "Pizzas" },
                new CategoriasProductoVenta { Nombre = "Bebidas" },
                new CategoriasProductoVenta { Nombre = "Postres"}
            };

            List<CategoriasProductoVenta> resultadoObtenido = categoriaDAO.RecuperarCategoriasProductoVenta();

            Assert.NotEmpty(resultadoObtenido);
            Assert.NotEqual(resultadoEsperado.Count, resultadoObtenido.Count + 1);
        }

    }
}
