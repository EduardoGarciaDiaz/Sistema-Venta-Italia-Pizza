using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public class Class1
    {
        public static List<TiposEmpleado> ProbarBD()
        {
            List<TiposEmpleado> tiposEmpleados = new List<TiposEmpleado>();
            using (var context = new ItaliaPizzaEntities())
            {
                tiposEmpleados = context.TiposEmpleado.ToList();
            }
            return tiposEmpleados;
        }
    }
}
