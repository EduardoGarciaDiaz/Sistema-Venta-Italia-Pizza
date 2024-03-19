using ItaliaPizza_Cliente.ServicioItaliaPizza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Cliente.Utilidades
{
    public sealed class EmpleadoSingleton
    {
        private static EmpleadoSingleton _instance;
        public string NombreUsuario { get; set; }
        public TipoEmpleadoDto TipoEmpleado { get; set; }

        private EmpleadoSingleton() { }

        public static EmpleadoSingleton getInstance()
        {
            if (_instance == null)
            {
                _instance = new EmpleadoSingleton();
            }
            return _instance;
        }

    }
}
