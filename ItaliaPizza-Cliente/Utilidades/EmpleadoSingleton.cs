using ItaliaPizza_Cliente.ServicioItaliaPizza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Cliente.Utilidades
{
    public sealed class EmpleadoSingleton
    {
        private static EmpleadoSingleton _instancia;

        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }

        public EmpleadoDto DatosEmpleado {get; set;}

        public UsuarioDto DatosUsuario{ get; set; }
        public DireccionDto DatosDireccion { get; set; }

        public TipoEmpleadoDto TipoEmpleado { get; set; }
      

        private EmpleadoSingleton(EmpleadoDto empleado, UsuarioDto usuario, DireccionDto direccion) 
        {
            this.IdUsuario = usuario.IdUsuario;
            this.NombreUsuario = empleado.NombreUsuario;
            this.DatosEmpleado = empleado;
            this.DatosUsuario = usuario;
            this.DatosDireccion = direccion;
        }
        private EmpleadoSingleton() { }

        public static EmpleadoSingleton getInstance()
        {
            if (_instancia == null)
            {
                _instancia = new EmpleadoSingleton();
            }
            return _instancia;
        }

        public static EmpleadoSingleton getInstance(EmpleadoDto empleado, UsuarioDto usuario, DireccionDto direccion)
        {
            if (_instancia == null)
            {
                _instancia = new EmpleadoSingleton(empleado, usuario, direccion);
            }
            return _instancia;
        }

        public static void LimpiarSingleton()
        {
            _instancia = null;
        }

    }
}
