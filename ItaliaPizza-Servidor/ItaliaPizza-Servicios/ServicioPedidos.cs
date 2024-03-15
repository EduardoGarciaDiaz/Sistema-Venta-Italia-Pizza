using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioPedidos
    {
        public void OperacionPedidosEjemplo()
        {
            throw new NotImplementedException();
        }

        public List<TipoServicio> RecuperarTiposServicio()
        {
            List<TipoServicio> tipoServicios = new List<TipoServicio>();
            TipoServicioDAO tipoServicioDAO = new TipoServicioDAO();
            tipoServicios = tipoServicioDAO.RecuperarTiposServicio().ConvertAll(tipoServicio =>
                new TipoServicio
                {
                    Id = tipoServicio.IdTipoServicio,
                    Nombre = tipoServicio.Nombre
                }    
            );
            return tipoServicios;
        }
    }
}
