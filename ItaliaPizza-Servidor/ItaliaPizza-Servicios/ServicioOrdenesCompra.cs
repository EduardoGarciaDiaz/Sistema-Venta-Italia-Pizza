﻿using ItaliaPizza_Contratos.DTOs;
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
               
        public int ActualizarOrdenDeCompra(OrdenDeCompraDto ordenDeCompraDto)
        {
            bool exito = false;
            try { 
                if (ordenDeCompraDto != null)
                {
                    var ordenCompra = AuxiliarConversorDTOADAO.ConvertirOrdenDeCompraDtoAOrdenesDeCompras(ordenDeCompraDto);                
                    List<OrdenesCompraInsumos> ordenesCompraInsumos = new List<OrdenesCompraInsumos>();
                    foreach (var item in ordenDeCompraDto.ListaElementosOrdenCompra)
                    {
                        ordenesCompraInsumos.Add(AuxiliarConversorDTOADAO.ConvertirElementoOrdenCompraAOrdenesCompraInsumos(ordenDeCompraDto.IdOrdenCompra, item));
                    }
                    ordenCompra.OrdenesCompraInsumos = ordenesCompraInsumos;
                    exito = OrdenDeCompraDAO.ActualizarOrdenDeCompraDB(ordenCompra);
                }
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return (exito) ? ordenDeCompraDto.IdOrdenCompra : 0;         
        }


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
                if (ordenDeCompraDto != null)
                {
                    var ordenNueva = AuxiliarConversorDTOADAO.ConvertirOrdenDeCompraDtoAOrdenesDeCompras(ordenDeCompraDto);
                    idOrdenCompraNueva = OrdenDeCompraDAO.GuardarOrdenDeCompra(ordenNueva);
                    List<OrdenesCompraInsumos> ordenesCompraInsumos = new List<OrdenesCompraInsumos>();
                    foreach (var item in ordenDeCompraDto.ListaElementosOrdenCompra)
                    {
                        ordenesCompraInsumos.Add(AuxiliarConversorDTOADAO.ConvertirElementoOrdenCompraAOrdenesCompraInsumos(idOrdenCompraNueva, item));
                    }
                    if(ordenDeCompraDto.ListaElementosOrdenCompra.Count != 0)
                    {
                        int resultado = OrdenDeCompraDAO.GuardarInsumoOrdenDeCompra(ordenesCompraInsumos);
                        if (resultado == 0)
                        {
                            idOrdenCompraNueva = 0;
                        }
                    }                   
                }
                return idOrdenCompraNueva;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }           
        }

        

        public List<OrdenDeCompraDto> RecuperarOrdenesDeCompra()
        {
            try
            {
                return OrdenDeCompraDAO.RecuperarOrdenesDeCompra();
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public double RecuperarSalidasDeOrdenesCompraPorFecha(DateTime fecha)
        {
            try
            {
                return OrdenDeCompraDAO.RecuperarSalidasDeOrdenesCompraPorFecha(fecha);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public bool RegistrarPagoOrdenCompra(OrdenDeCompraDto ordenCompra)
        {
            try
            {
                return OrdenDeCompraDAO.RegistrarPagoOrdenCompra(ordenCompra);
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }
    }
}
