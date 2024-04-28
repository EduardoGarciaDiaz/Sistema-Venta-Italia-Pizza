using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using ItaliaPizza_DataAccess.Excepciones;
using ItaliaPizza_Servicios.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioGastosVarios
    {
        public int GuardarGastoVario(GastoVario gastoVario)
        {
            int filasAfectadas = -1;
            GastoVarioDAO gastoVarioDAO = new GastoVarioDAO();
            GastosVarios nuevoGastoVario = AuxiliarConversorDTOADAO.ConvertirGastoVarioAGastosVarios(gastoVario);

            try
            {
                filasAfectadas = gastoVarioDAO.GuardarGastoVario(nuevoGastoVario);

                return filasAfectadas;
            }
            catch (ExcepcionDataAccess ex)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(ex);
            }
        }

        public double RecuperarSalidasGastosVarios(DateTime fecha)
        {
            GastoVarioDAO gastoVarioDAO = new GastoVarioDAO();
            try
            {
                return gastoVarioDAO.RecuperarSalidasGastosVarios(fecha);
            }
            catch (ExcepcionDataAccess ex)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(ex);
            }
        }
    }
}
