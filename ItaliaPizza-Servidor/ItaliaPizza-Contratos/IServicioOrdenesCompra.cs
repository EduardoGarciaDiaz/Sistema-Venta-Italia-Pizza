﻿using ItaliaPizza_Contratos.DTOs;
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
    public interface IServicioOrdenesCompra
    { 

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int GuardarOrdenDeCompraNueva(OrdenDeCompraDto ordenDeCompraDto);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool EnviarOrdenDeCompra(int idOrdenDeCompra);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        double RecuperarSalidasDeOrdenesCompraPorFecha(DateTime fecha);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        List<OrdenDeCompraDto> RecuperarOrdenesDeCompra();

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        bool RegistrarPagoOrdenCompra(OrdenDeCompraDto ordenCompra);

        [OperationContract]
        [FaultContract(typeof(ExcepcionServidorItaliaPizza))]
        int ActualizarOrdenDeCompra(OrdenDeCompraDto ordenDeCompraDto);

    }
}
