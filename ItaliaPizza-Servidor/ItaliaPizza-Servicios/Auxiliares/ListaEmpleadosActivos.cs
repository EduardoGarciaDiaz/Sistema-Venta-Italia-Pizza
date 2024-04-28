using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class ListaEmpleadosActivos
    {
        private static readonly Dictionary<int, string> s_empleadosActivos = new Dictionary<int, string>();


        public static void RegistrarUsuarioEnLista(int idUsuario, string nombreUsuario)
        {
            if (idUsuario != 0 && !String.IsNullOrEmpty(nombreUsuario) && !s_empleadosActivos.ContainsKey(idUsuario))
            {
                s_empleadosActivos.Add(idUsuario, nombreUsuario);
            }
        }

        public static bool EsEmpleadoNoActivo(int idUsuario)
        {
            bool respuesta = false;
            if(idUsuario != 0)
            {
                respuesta = !s_empleadosActivos.ContainsKey(idUsuario);   
            }
            return respuesta;
        }

        public static bool EsEmpleadoNoActivo(string nombreUsuario)
        {
            bool respuesta = true;
            if (!String.IsNullOrEmpty(nombreUsuario))
            {
                foreach (var item in s_empleadosActivos)
                {
                    if (item.Value.Equals(nombreUsuario))
                    {
                        respuesta = false;
                        break;
                    }
                };
            }
            else
            {
                respuesta = true;
            }
            return respuesta;
        }

        public static void QuitarEmpleadoDeLista(int idUser)
        {
            if (idUser != 0 && s_empleadosActivos.ContainsKey(idUser))
            {
                s_empleadosActivos.Remove(idUser);
            }
        }



    }
}
