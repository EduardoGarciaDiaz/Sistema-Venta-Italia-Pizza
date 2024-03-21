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

    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioOrdenesCompra
    {
        public bool EnviarOrdenDeCompra(int idOrdenDeCompra)
        {
            try
            {
                bool ordenEnviada = false;
                OrdenesCompra ordenCompra = OrdenDeCompraDAO.RecuperarOrdenDeCompra(idOrdenDeCompra);
                if (ordenCompra != null && ordenCompra.OrdenesCompraInsumos.Count > 0)
                {
                    ordenEnviada = ConversorAExcel.CrearExcelOrdenCompra(ordenCompra);
                    if (ordenEnviada)
                    {
                        OrdenDeCompraDAO.CambiarEstadoOrdenCompraAEnviado(ordenCompra);
                    }
                }
                return ordenEnviada;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }

        }

        public int GuardarOrdenDeCompraNueva(OrdenDeCompraDto ordenDeCompraDto)
        {
            try
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
                    int resultado = OrdenDeCompraDAO.GuardarInsumoOrdenDeCompra(ordenesCompraInsumos);
                    if (resultado == 0)
                    {
                        idOrdenCompraNueva = 0;
                    }
                }
                return idOrdenCompraNueva;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }           
        }

        

       
        public void OperacionOrdenesEjemplo()
        {
            throw new NotImplementedException();
        }


    }
}
