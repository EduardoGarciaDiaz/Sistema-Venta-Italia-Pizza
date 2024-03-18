using ItaliaPizza_Contratos.DTOs;
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
        void OperacionRecetasEjemplo();

        [OperationContract]
        List<Receta> RecuperarRecetas();

        [OperationContract]
        List<InsumoReceta> RecuperarInsumosReceta(int idReceta);

        [OperationContract]
        int GuardarReceta(RecetaProducto receta);
    }
}
