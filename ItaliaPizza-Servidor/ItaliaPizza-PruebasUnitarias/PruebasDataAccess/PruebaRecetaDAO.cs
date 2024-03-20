using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ItaliaPizza_PruebasUnitarias.PruebasDataAccess
{
    public class ConfiguracionPruebaRecetaDAO : IDisposable
    {
        public int IdRecetaPrueba { get; set; }
        public string CodigoProductoPrueba { get; set; }
        public string CodigoProductoVentaPrueba { get; set; }
        public string CodigoProducto2Prueba { get; set; }
        public string CodigoInsumoPrueba { get; set; }
        public int IdRecetaInsumoPrueba { get; set; }
        public string CodigoProducto3Prueba { get; set; }
        public string CodigoProductoVenta2Prueba { get; set; }
        public int IdReceta2Prueba { get; set; }


        public ConfiguracionPruebaRecetaDAO() 
        {
            try
            {
                CrearRecetasPrueba();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQLException on GlobalScoreManagementTest: " + ex.Message);
            }
            catch (EntityException ex)
            {
                Console.WriteLine("Entity Exception on GlobalScoreManagementTest: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected Exception on GlobalScoreManagementTest: " + ex.Message + ": \n" + ex.StackTrace);
            }
        }

        private void CrearRecetasPrueba()
        {
            using (var context = new ItaliaPizzaEntities())
            {
                RegistrarProductoEnBBDD(context);
                RegistrarProductoVentaEnBBDD(context);
                RegistrarRecetaEnBBDD(context, CodigoProductoVentaPrueba);
                RegistrarProducto2EnBBDD(context);
                RegistrarInsumoEnBBDD(context);
                RegistrarRecetaInsumoEnBBDD(context);
                RegistrarProducto3EnBBDD(context);
                RegistrarProductoVenta2EnBBDD(context);
                RegistrarReceta2EnBBDD(context, null);
            }
        }

        private void RegistrarProductoEnBBDD(ItaliaPizzaEntities context)
        {
            var productoNuevo = context.Productos.Add(new Productos()
            {
                CodigoProducto = "CodigoPruebaU1",
                Nombre = "Pizza prueba unitaria",
                Descripcion = "Pizza creada para la prueba unitaria",
                EsInventariado = false,
                EsActivo = true
            });

            context.SaveChanges();

            CodigoProductoPrueba = productoNuevo.CodigoProducto;
        }

        private void RegistrarProductoVentaEnBBDD(ItaliaPizzaEntities context)
        {
            int idCategoriaPizzas = 1;

            var productoVentaNuevo = context.ProductosVenta.Add(new ProductosVenta()
            {
                CodigoProducto = "CodigoPruebaU1",
                Precio = 200,
                IdCategoriaProductoVenta = idCategoriaPizzas

            });

            context.SaveChanges();

            CodigoProductoVentaPrueba = productoVentaNuevo.CodigoProducto;
        }

        private void RegistrarRecetaEnBBDD(ItaliaPizzaEntities context, string codigoProducto)
        {
            var recetaNueva = context.Recetas.Add(new Recetas()
            {
                CodigoProducto = codigoProducto
            });

            context.SaveChanges();

            IdRecetaPrueba = recetaNueva.IdReceta;
        }

        private void RegistrarProducto2EnBBDD(ItaliaPizzaEntities context)
        {
            var productoNuevo = context.Productos.Add(new Productos()
            {
                CodigoProducto = "CodigoInsumoPU1",
                Nombre = "Insumo prueba unitaria",
                Descripcion = "Insumo creado para la prueba unitaria",
                EsInventariado = true,
                EsActivo = true
            });

            context.SaveChanges();

            CodigoProducto2Prueba = productoNuevo.CodigoProducto;
        }

        private void RegistrarInsumoEnBBDD(ItaliaPizzaEntities context)
        {
            int idCategoriaInsumo = 1;
            int idUnidadMedida = 1;

            var insumoNuevo = context.Insumos.Add(new Insumos()
            {
                CodigoProducto = "CodigoInsumoPU1",
                Cantidad = 5,
                Costo = 22,
                IdCategoriaInsumo = idCategoriaInsumo,
                IdUnidadMedida = idUnidadMedida
            });

            context.SaveChanges();

            CodigoInsumoPrueba = insumoNuevo.CodigoProducto;
        }

        private void RegistrarRecetaInsumoEnBBDD(ItaliaPizzaEntities context)
        {
            var recetaInsumoNueva = context.RecetasInsumos.Add(new RecetasInsumos()
            {
                CantidadInsumo = 2,
                IdReceta = IdRecetaPrueba,
                CodigoProducto = CodigoInsumoPrueba,
            });

            context.SaveChanges();

            IdRecetaInsumoPrueba = recetaInsumoNueva.IdRecetaInsumo;
        }

        private void RegistrarProducto3EnBBDD(ItaliaPizzaEntities context)
        {
            var productoNuevo = context.Productos.Add(new Productos()
            {
                CodigoProducto = "CodigoPruebaU3",
                Nombre = "Producto 2 prueba unitaria",
                Descripcion = "Producto 2 creado para la prueba unitaria",
                EsInventariado = true,
                EsActivo = true
            });

            context.SaveChanges();

            CodigoProducto3Prueba = productoNuevo.CodigoProducto;
        }

        private void RegistrarProductoVenta2EnBBDD(ItaliaPizzaEntities context)
        {
            int idCategoriaPizzas = 1;

            var productoVentaNuevo = context.ProductosVenta.Add(new ProductosVenta()
            {
                CodigoProducto = "CodigoPruebaU3",
                Precio = 200,
                IdCategoriaProductoVenta = idCategoriaPizzas

            });

            context.SaveChanges();

            CodigoProductoVenta2Prueba = productoVentaNuevo.CodigoProducto;
        }

        private void RegistrarReceta2EnBBDD(ItaliaPizzaEntities context, string codigoProducto)
        {
            var recetaNueva = context.Recetas.Add(new Recetas()
            {
                CodigoProducto = codigoProducto
            });

            context.SaveChanges();

            IdReceta2Prueba = recetaNueva.IdReceta;
        }

        public void Dispose()
        {
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    EliminarRegistros(context);
                    context.SaveChanges();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQLException on GlobalScoreManagementTest: " + ex.Message);
            }
            catch (EntityException ex)
            {
                Console.WriteLine("Entity Exception on GlobalScoreManagementTest: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected Exception on GlobalScoreManagementTest: " + ex.Message + ": \n" + ex.StackTrace);
            }
        }

        private void EliminarRegistros(ItaliaPizzaEntities context)
        {
            EliminarRecetaPrueba(context);
            EliminarProductoVentaPrueba(context);
            EliminarProductoPrueba(context);

            EliminarInsumoPrueba(context);
            EliminarRecetasInsumoPrueba(context);
            EliminarRecetaCreada(context);
            EliminarProducto2Prueba(context);

            EliminarProductoVenta2Prueba(context);
            EliminarProducto3Prueba(context);

            EliminarRecetasInsumoCreada(context);
            EliminarReceta2Prueba(context);
        }

        private void EliminarRecetaPrueba(ItaliaPizzaEntities context)
        {
            var recetaAEliminar = context.Recetas.FirstOrDefault(r => r.IdReceta == IdRecetaPrueba);

            if (recetaAEliminar != null)
            {
                context.Recetas.Remove(recetaAEliminar);
            }
        }

        private void EliminarProductoPrueba(ItaliaPizzaEntities context)
        {
            var productoAEliminar = context.Productos.FirstOrDefault(p => p.CodigoProducto == CodigoProductoPrueba);

            if (productoAEliminar != null)
            {
                context.Productos.Remove(productoAEliminar);
            }
        }

        private void EliminarProductoVentaPrueba(ItaliaPizzaEntities context)
        {
            var productoVentaAEliminar = context.ProductosVenta.FirstOrDefault(pv => pv.CodigoProducto == CodigoProductoVentaPrueba);

            if (productoVentaAEliminar != null)
            {
                context.ProductosVenta.Remove(productoVentaAEliminar);
            }
        }

        private void EliminarInsumoPrueba(ItaliaPizzaEntities context)
        {
            var insumoAEliminar = context.Insumos.FirstOrDefault(i => i.CodigoProducto == CodigoInsumoPrueba);

            if (insumoAEliminar != null)
            {
                context.Insumos.Remove(insumoAEliminar);
            }
        }

        private void EliminarRecetasInsumoPrueba(ItaliaPizzaEntities context)
        {
            var recetasInsumoAEliminar = context.RecetasInsumos.FirstOrDefault(ri => ri.IdRecetaInsumo == IdRecetaInsumoPrueba);

            if (recetasInsumoAEliminar != null)
            {
                context.RecetasInsumos.Remove(recetasInsumoAEliminar);
            }
        }

        private void EliminarProducto2Prueba(ItaliaPizzaEntities context)
        {
            var productoAEliminar = context.Productos.FirstOrDefault(p => p.CodigoProducto == CodigoProducto2Prueba);

            if (productoAEliminar != null)
            {
                context.Productos.Remove(productoAEliminar);
            }
        }

        private void EliminarRecetaCreada(ItaliaPizzaEntities context)
        {
            var recetaAEliminar = context.Recetas.FirstOrDefault(r => r.CodigoProducto == CodigoProductoVenta2Prueba);

            if (recetaAEliminar != null)
            {
                context.Recetas.Remove(recetaAEliminar);
            }
        }

        private void EliminarProductoVenta2Prueba(ItaliaPizzaEntities context)
        {
            var productoVentaAEliminar = context.ProductosVenta.FirstOrDefault(pv => pv.CodigoProducto == CodigoProductoVenta2Prueba);

            if (productoVentaAEliminar != null)
            {
                context.ProductosVenta.Remove(productoVentaAEliminar);
            }
        }

        private void EliminarProducto3Prueba(ItaliaPizzaEntities context)
        {
            var productoAEliminar = context.Productos.FirstOrDefault(p => p.CodigoProducto == CodigoProducto3Prueba);

            if (productoAEliminar != null)
            {
                context.Productos.Remove(productoAEliminar);
            }
        }

        private void EliminarRecetasInsumoCreada(ItaliaPizzaEntities context)
        {
            var recetasInsumoAEliminar = context.RecetasInsumos.FirstOrDefault(ri => ri.IdReceta == IdReceta2Prueba);

            if (recetasInsumoAEliminar != null)
            {
                context.RecetasInsumos.Remove(recetasInsumoAEliminar);
            }
        }

        private void EliminarReceta2Prueba(ItaliaPizzaEntities context)
        {
            var recetaAEliminar = context.Recetas.FirstOrDefault(r => r.IdReceta == IdReceta2Prueba);

            if (recetaAEliminar != null)
            {
                context.Recetas.Remove(recetaAEliminar);
            }
        }
    }

    public class PruebaRecetaDAO : IClassFixture<ConfiguracionPruebaRecetaDAO>
    {
        private readonly ConfiguracionPruebaRecetaDAO _configuracion;

        public PruebaRecetaDAO(ConfiguracionPruebaRecetaDAO configuracion)
        {
            _configuracion = configuracion;
        }

        [Fact]
        public void PruebaRecuperarRecetasExitosa()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            List<Receta> resultadoEsperado = new List<Receta>
            {
                new Receta
                {
                    Codigo = "CodigoPruebaU1",
                    Nombre = "Pizza prueba unitaria",
                }    
            };

            List<Receta> resultadoObtenido = recetaDAO.RecuperarRecetas();

            Assert.True(resultadoEsperado.All(recetaEsperada => resultadoObtenido.Any(recetaObtenida => recetaObtenida.Codigo == recetaEsperada.Codigo && recetaObtenida.Nombre == recetaEsperada.Nombre)));
        }

        [Fact]
        public void PruebaRecuperarRecetasFallida()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            List<Receta> resultadoEsperado = new List<Receta>
            {
                new Receta
                {
                    Codigo = "CodigoPruebaNoInsertado",
                    Nombre = "Pizza prueba unitaria no insertada",
                }
            };

            List<Receta> resultadoObtenido = recetaDAO.RecuperarRecetas();

            Assert.NotNull(resultadoObtenido);
            Assert.NotEmpty(resultadoObtenido);
            Assert.False(resultadoEsperado.All(recetaEsperada => resultadoObtenido.Any(recetaObtenida => recetaObtenida.Codigo == recetaEsperada.Codigo 
            && recetaObtenida.Nombre == recetaEsperada.Nombre)));
        }

        [Fact]
        public void PruebaRecuperarInsumosRecetaExitosa()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            List<InsumoReceta> resultadoEsperado = new List<InsumoReceta>
            {
                new InsumoReceta
                {
                    Codigo = "CodigoInsumoPU1",
                    Nombre = "Insumo prueba unitaria",
                    Cantidad = 2,
                    UnidadMedida = new UnidadMedida()
                    {
                        Id = 1,
                        Nombre = "Kg"
                    }
                }
            };

            List<InsumoReceta> resultadoObtenido = recetaDAO.RecuperarInsumosReceta(_configuracion.IdRecetaPrueba);

            Assert.NotNull(resultadoObtenido);
            Assert.True(resultadoEsperado.All(re => resultadoObtenido.Any(
                ro => ro.Nombre == re.Nombre
                && ro.Cantidad == re.Cantidad
                && ro.UnidadMedida.Id == re.UnidadMedida.Id
                && ro.UnidadMedida.Nombre == re.UnidadMedida.Nombre)));
        }

        [Fact]
        public void PruebaRecuperarInsumosRecetaFallida()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            List<InsumoReceta> resultadoEsperado = new List<InsumoReceta>
            {
                new InsumoReceta
                {
                    Codigo = "CodigoInexistente111?",
                    Nombre = "Insumono intertado!!",
                    Cantidad = -2,
                    UnidadMedida = new UnidadMedida()
                    {
                        Id = -1,
                        Nombre = "Kiligramos0"
                    }
                }
            };

            List<InsumoReceta> resultadoObtenido = recetaDAO.RecuperarInsumosReceta(_configuracion.IdRecetaPrueba);

            Assert.NotNull(resultadoObtenido);
            Assert.False(resultadoEsperado.All(re => resultadoObtenido.Any(
                ro => ro.Nombre == re.Nombre
                && ro.Cantidad == re.Cantidad
                && ro.UnidadMedida.Id == re.UnidadMedida.Id
                && ro.UnidadMedida.Nombre == re.UnidadMedida.Nombre)));
        }

        [Fact]
        public void PruebaGuardarRecetaExitosa()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            Recetas receta = new Recetas()
            {
                CodigoProducto = _configuracion.CodigoProductoVenta2Prueba
            };

            int resultadoObtenido = recetaDAO.GuardarReceta(receta);

            Assert.True(resultadoObtenido > 0);
        }

        [Fact]
        public void PruebaGuardarRecetaFallida()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            int resultadoEsperado = -1;

            int resultadoObtenido = recetaDAO.GuardarReceta(null);

            Assert.Equal(resultadoObtenido, resultadoEsperado);
        }

        [Fact]
        public void PruebaGuardarRecetaInsumosExitosa()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            List<RecetasInsumos> listaRecetaInsumo = new List<RecetasInsumos>
            {
                new RecetasInsumos
                {
                    CantidadInsumo = 1,
                    IdReceta = _configuracion.IdReceta2Prueba,
                    CodigoProducto = _configuracion.CodigoInsumoPrueba
                }
            };

            int resultadoObtenido = recetaDAO.GuardarRecetaInsumos(listaRecetaInsumo);

            Assert.True(resultadoObtenido > 0);
        }

        [Fact]
        public void PruebaGuardarRecetaInsumosFallida()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            int resultadoEsperado = -1;

            int resultadoObtenido = recetaDAO.GuardarRecetaInsumos(null);

            Assert.Equal(resultadoEsperado, resultadoObtenido);
        }

        [Fact]
        public void PruebaRecuperarInsumosEnRecetaExitosa()
        {
            RecetaDAO recetaDAO = new RecetaDAO();
            List<RecetasInsumos> resultadoObtenido = recetaDAO.RecuperarInsumosEnReceta(_configuracion.CodigoProductoVentaPrueba);

            List<RecetasInsumos> resultadoEsperado = new List<RecetasInsumos>
            {
                new RecetasInsumos
                {
                    CantidadInsumo = 1,
                    IdReceta = _configuracion.IdRecetaPrueba,
                    CodigoProducto = "CodigoInsumoPU1"
                }
            };

            Assert.NotNull(resultadoObtenido);
            Assert.True(resultadoEsperado.All(re => resultadoObtenido.Any(
                ro => ro.CodigoProducto == re.CodigoProducto
                && ro.IdReceta == re.IdReceta)));
        }

        [Fact]
        public void PruebaRecuperarInsumosEnRecetaFallida()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            List<RecetasInsumos> resultadoEsperado = new List<RecetasInsumos>
            {
                new RecetasInsumos
                {
                    CantidadInsumo = -1,
                    IdReceta = -1,
                    CodigoProducto = "-1NoRegistrado"
                }
            };

            List<RecetasInsumos> resultadoObtenido = recetaDAO.RecuperarInsumosEnReceta(_configuracion.CodigoProductoVentaPrueba);

            Assert.NotNull(resultadoObtenido);
            Assert.False(resultadoEsperado.All(re => resultadoObtenido.Any(ro => ro.CodigoProducto == re.CodigoProducto
            && ro.IdReceta == re.IdReceta)));
        }
    }
}
