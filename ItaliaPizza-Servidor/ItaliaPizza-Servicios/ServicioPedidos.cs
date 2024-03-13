using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public partial class ItaliaPizzaServicio : ItaliaPizza_Contratos.IServicioPedidos
    {
        public void OperacionPedidosEjemplo()
        {
            throw new NotImplementedException();
        }
    }
}
