using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ItaliaPizza_Servicios
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código y en el archivo de configuración a la vez.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    public class Service1 : ItaliaPizza_Contratos.IServicioPrueba
    {
        public void DoWork()
        {

        }
    }
}
