using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess.Excepciones;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza_Servicios.Auxiliares;

namespace ItaliaPizza_Servicios
{
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioCorteCaja
    {
        public bool ExisteCorteCaja(DateTime fecha)
        {
            CorteCajaDAO corteCajaDAO = new CorteCajaDAO();
            try
            {
                return corteCajaDAO.ExisteCorteCaja(fecha);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public int ActualizarCorteCaja(CorteCaja corteCaja)
        {
            CorteCajaDAO corteCajaDAO = new CorteCajaDAO();
            try
            {
                return corteCajaDAO.ActualizarCorteCaja(corteCaja);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public int GuardarCorteCaja(CorteCaja corteCaja)
        {
            CorteCajaDAO corteCajaDAO = new CorteCajaDAO();
            try
            {
                return corteCajaDAO.GuardarCorteCaja(corteCaja);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public CorteCaja RecuperarCorteCaja(DateTime fecha)
        {
            CorteCajaDAO corteCajaDAO = new CorteCajaDAO();
            try
            {
                return corteCajaDAO.RecuperarCorteCaja(fecha);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }
    }
}
