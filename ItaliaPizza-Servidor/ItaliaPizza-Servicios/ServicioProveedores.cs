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
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioProveedores
    {
        public bool GuardarProveedorNuevo(ProveedorDto proveedorNuevo)
        {
            bool resultadoOperacion = false;
            try
            {
                Direcciones direccion = AuxiliarConversorDTOADAO.ConvertirDireccionDtoADirecciones(proveedorNuevo.Direccion);
                int idDireccionNueva = DireccionDAO.GuardarDireccionNuevaBD(direccion);
                if(idDireccionNueva != 0)
                {
                    proveedorNuevo.IdDireccion = idDireccionNueva;
                    Proveedores proveedores = AuxiliarConversorDTOADAO.ConvertirProveedorDtoAProveedores(proveedorNuevo);
                    resultadoOperacion = ProveedorDAO.GuardarProveedorNuevoBD(proveedores);
                }
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
            return resultadoOperacion;
        }

        public List<ProveedorDto> RecuperarProveedores()
        {
            try
            {

                List<ProveedorDto> proveedorDtos = new List<ProveedorDto>();
                var proveedoresBD = ProveedorDAO.RecuperarProveedoresBD();
                foreach (var item in proveedoresBD)
                {
                    proveedorDtos.Add(AuxiliarConversorDTOADAO.ConvertirProveedoresAProveedoresDto(item, item.Direcciones));
                }
                return proveedorDtos;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public bool ValidarCorreoUnicoProveedor(string correo)
        {
            try
            {
                bool esUnico = false;
                if (!string.IsNullOrEmpty(correo))
                {
                    try
                    {
                        esUnico = ProveedorDAO.ValidarCorreoUnicoProveedorBD(correo);
                    }
                    catch (ExcepcionDataAccess e)
                    {
                        throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
                    }
                }
                return esUnico;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

        public bool ValidarRfcUnicoProveedor(string rfc)
        {
            try
            {
                bool esUnico = false;
                if (!string.IsNullOrEmpty(rfc))
                {
                    try
                    {
                        esUnico = ProveedorDAO.ValidarRfcUnicoProveedorBD(rfc);
                    }
                    catch (ExcepcionDataAccess e)
                    {
                        throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
                    }
                }
                return esUnico;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }
    }
}
