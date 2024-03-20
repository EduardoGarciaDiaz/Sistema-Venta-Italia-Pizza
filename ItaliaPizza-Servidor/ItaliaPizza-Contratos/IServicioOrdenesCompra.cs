﻿using ItaliaPizza_Contratos.DTOs;
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
        void OperacionOrdenesEjemplo();

        [OperationContract]
        int GuardarOrdenDeCompraNueva(OrdenDeCompraDto ordenDeCompraDto);

        [OperationContract]
        bool EnviarOrdenDeCompra(int idOrdenDeCompra);
    }
}
