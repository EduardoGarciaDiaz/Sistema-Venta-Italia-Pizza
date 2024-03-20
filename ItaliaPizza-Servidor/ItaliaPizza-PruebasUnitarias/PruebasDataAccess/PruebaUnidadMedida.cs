using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ItaliaPizza_PruebasUnitarias.PruebasDataAccess
{
    public class ConfiguracionPruebaUnidadMedidaDAO : IDisposable
    {
        public ConfiguracionPruebaUnidadMedidaDAO() { }

        public void Dispose() { }
    }

    public class PruebaUnidadMedida : IClassFixture<ConfiguracionPruebaUnidadMedidaDAO>
    {
        private readonly ConfiguracionPruebaUnidadMedidaDAO _configuracion;

        public PruebaUnidadMedida(ConfiguracionPruebaUnidadMedidaDAO configuracion)
        {
            _configuracion = configuracion;
        }

        [Fact]
        public void PruebaRecuperarUnidadesMedidaExitosa()
        {
            UnidadMedidaDAO unidadMedidaDAO = new UnidadMedidaDAO();

            List<UnidadesMedida> resultadoEsperado = new List<UnidadesMedida>
            {
                new UnidadesMedida { Nombre = "Kg" },
                new UnidadesMedida { Nombre = "Lt" },
                new UnidadesMedida { Nombre = "Unidad"},
            };

            List<UnidadesMedida> resultadoObtenido = unidadMedidaDAO.RecuperarUnidadesMedida();

            Assert.NotNull(resultadoObtenido);
            Assert.NotEmpty(resultadoObtenido);
            Assert.Equal(resultadoEsperado.Select(re => re.Nombre), resultadoObtenido.Select(ro => ro.Nombre));
        }

        [Fact]
        public void PruebaRecuperarUnidadesMedidaFallida()
        {
            UnidadMedidaDAO unidadMedidaDAO = new UnidadMedidaDAO();

            List<UnidadesMedida> resultadoEsperado = new List<UnidadesMedida>
            {
                new UnidadesMedida { Nombre = "Kg" },
                new UnidadesMedida { Nombre = "Lt" },
                new UnidadesMedida { Nombre = "Unidad"},
            };

            List<UnidadesMedida> resultadoObtenido = unidadMedidaDAO.RecuperarUnidadesMedida();

            Assert.NotEqual(resultadoEsperado.Count, resultadoObtenido.Count + 1);
        }
    }
}
