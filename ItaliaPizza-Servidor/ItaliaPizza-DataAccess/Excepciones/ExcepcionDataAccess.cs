using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess.Excepciones
{
    public class ExcepcionDataAccess : Exception
    {
        public ExcepcionDataAccess(string mensaje) : base(mensaje)
        {
        }
    }
}
