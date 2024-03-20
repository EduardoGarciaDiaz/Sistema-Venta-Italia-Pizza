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
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioProveedores
    {
        public List<ProveedorDto> RecuperarProveedores()
        {
            List<ProveedorDto> proveedorDtos = new List<ProveedorDto>();
            var proveedoresBD = ProveedorDAO.RecuperarProveedoresBD();
            foreach (var item in proveedoresBD)
            {
                proveedorDtos.Add(AuxiliarConversorDTOADAO.ConvertirProveedoresAProveedoresDto(item, item.Direcciones));
            }
            return proveedorDtos;
        }
    }
}
