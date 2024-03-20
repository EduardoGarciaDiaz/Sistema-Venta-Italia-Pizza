using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using ItaliaPizza_Servicios.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{

    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioOrdenesCompra
    {
        public bool EnviarOrdenDeCompra(int idOrdenDeCompra)
        {
            bool ordenEnviada = false;
            return ordenEnviada;

        }

        public int GuardarOrdenDeCompraNueva(OrdenDeCompraDto ordenDeCompraDto)
        {
            int idOrdenCompraNueva = 0;
            if (ordenDeCompraDto != null && ordenDeCompraDto.listaElementosOrdenCompra.Count > 0)
            {
                var ordenNueva = AuxiliarConversorDTOADAO.ConvertirOrdenDeCompraDtoAOrdenesDeCompras(ordenDeCompraDto);
                idOrdenCompraNueva = OrdenDeCompraDAO.GuardarOrdenDeCompra(ordenNueva);
                List<OrdenesCompraInsumos> ordenesCompraInsumos = new List<OrdenesCompraInsumos>();
                foreach (var item in ordenDeCompraDto.listaElementosOrdenCompra)
                {
                    ordenesCompraInsumos.Add(AuxiliarConversorDTOADAO.ConvertirElementoOrdenCompraAOrdenesCompraInsumos(idOrdenCompraNueva, item));
                }
                int resultado = OrdenDeCompraDAO.GuardarElementoInsumoDeOrdenDeCompra(ordenesCompraInsumos);
                if(resultado == 0)
                {
                    idOrdenCompraNueva = 0;
                }
            }
            return idOrdenCompraNueva;
        }

        

       
        public void OperacionOrdenesEjemplo()
        {
            throw new NotImplementedException();
        }


    }
}
