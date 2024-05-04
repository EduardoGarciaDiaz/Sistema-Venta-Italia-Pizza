using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_Contratos.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos
{
    [ServiceContract]
    public interface IServicioRecetas
    {

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<Receta> RecuperarRecetas();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<InsumoReceta> RecuperarInsumosReceta(int idReceta);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int GuardarReceta(RecetaProducto receta);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int ActualizarReceta(RecetaProducto recetaEdicion);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int EliminarReceta(int idReceta);
    }
}
