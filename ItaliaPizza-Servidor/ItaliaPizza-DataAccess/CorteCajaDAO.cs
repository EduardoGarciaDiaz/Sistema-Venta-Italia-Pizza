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
                    CortesCaja corteCajaFechaSeleccionada = context.CortesCaja.FirstOrDefault(c => DbFunctions.TruncateTime(c.Fecha) == corteCaja.Fecha.Date);
                    if (corteCajaFechaSeleccionada != default)
                    {
                        corteCajaFechaSeleccionada.Fondo = corteCaja.Fondo;
                        corteCajaFechaSeleccionada.TotalFinal = corteCaja.DineroEnCaja;
                        corteCajaFechaSeleccionada.SalidasRegistradas = corteCaja.SalidasRegistradas;
                        corteCajaFechaSeleccionada.IngresoRegistrados = corteCaja.IngresosRegistrados;
                        corteCajaFechaSeleccionada.NombreUsuario = corteCaja.NombreUsuario;
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
                        Fecha = corteCaja.Fecha,
                        Fondo = corteCaja.Fondo,
                        IngresoRegistrados = corteCaja.IngresosRegistrados,
                        NombreUsuario = corteCaja.NombreUsuario,
                        SalidasRegistradas = corteCaja.SalidasRegistradas,
                        TotalFinal = corteCaja.DineroEnCaja
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
                            DineroEnCaja = corteCajaFechaSeleccionada.TotalFinal ?? 0,
                            Fondo = corteCajaFechaSeleccionada.Fondo ?? 0
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
