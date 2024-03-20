using ItaliaPizza_Contratos.Excepciones;
using ItaliaPizza_DataAccess.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class ExcepcionServidorItaliaPizzaManager
    {
        public static FaultException<ExcepcionServidorItaliaPizza> ManejarExcepcionDataAccess(ExcepcionDataAccess ex)
        {
            ExcepcionServidorItaliaPizza respuesta = new ExcepcionServidorItaliaPizza
            {
                Mensaje = ex.Message,
                StackTrace = ex.StackTrace
            };

            return new FaultException<ExcepcionServidorItaliaPizza>(respuesta, new FaultReason(respuesta.Mensaje));
        }
    }
}
