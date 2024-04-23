using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess.Excepciones;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class CorteCajaDAO
    {

        public CorteCajaDAO() { }

        public bool ExisteCorteCaja(DateTime fecha)
        {
            bool existeCorteCaja = false;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    CortesCaja cortesCaja = context.CortesCaja.FirstOrDefault(c => DbFunctions.TruncateTime(c.Fecha) == fecha.Date);
                    existeCorteCaja = (cortesCaja != default);
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
            return existeCorteCaja;
        }

        public int ActualizarCorteCaja(CorteCaja corteCaja)
        {
            int filasAfectadas = -1;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    CortesCaja corteCajaFechaSeleccionada = context.CortesCaja.FirstOrDefault(c => DbFunctions.TruncateTime(c.Fecha) == corteCaja.fecha.Date);
                    if (corteCajaFechaSeleccionada != default)
                    {
                        corteCajaFechaSeleccionada.Fondo = corteCaja.fondo;
                        corteCajaFechaSeleccionada.TotalFinal = corteCaja.dineroEnCaja;
                        corteCajaFechaSeleccionada.SalidasRegistradas = corteCaja.salidasRegistradas;
                        corteCajaFechaSeleccionada.IngresoRegistrados = corteCaja.ingresosRegistrados;
                        corteCajaFechaSeleccionada.NombreUsuario = corteCaja.nombreUsuario;
                    }
                    filasAfectadas = context.SaveChanges();
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
            return filasAfectadas;
        }

        public int GuardarCorteCaja(CorteCaja corteCaja)
        {
            int corteCajaId = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    CortesCaja corte = new CortesCaja()
                    {
                        Fecha = corteCaja.fecha,
                        Fondo = corteCaja.fondo,
                        IngresoRegistrados = corteCaja.ingresosRegistrados,
                        NombreUsuario = corteCaja.nombreUsuario,
                        SalidasRegistradas = corteCaja.salidasRegistradas,
                        TotalFinal = corteCaja.dineroEnCaja
                    };
                    context.CortesCaja.Add(corte);
                    context.SaveChanges();
                    corteCajaId = corte.IdCorteCaja;
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
            return corteCajaId;
        }

        public CorteCaja RecuperarCorteCaja(DateTime fecha)
        {
            CorteCaja corte = new CorteCaja(); 
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    CortesCaja corteCajaFechaSeleccionada = context.CortesCaja.FirstOrDefault(c => DbFunctions.TruncateTime(c.Fecha) == fecha.Date);
                    if (corteCajaFechaSeleccionada != default)
                    {
                        corte = new CorteCaja()
                        {
                            Id = corteCajaFechaSeleccionada.IdCorteCaja,
                            dineroEnCaja = corteCajaFechaSeleccionada.TotalFinal ?? 0,
                            fondo = corteCajaFechaSeleccionada.Fondo ?? 0
                        };
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
            return corte;
        }
    }
}
