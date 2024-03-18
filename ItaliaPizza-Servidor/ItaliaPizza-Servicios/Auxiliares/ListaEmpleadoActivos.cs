using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class ListaEmpleadoActivos
    {
        private static Dictionary<int, string> empleadosActivos = new Dictionary<int, string>();


        public static void RegistrarUsuarioEnLista(int idUsuario, string nombreUsuario)
        {
            if (idUsuario != 0 && !String.IsNullOrEmpty(nombreUsuario) && !empleadosActivos.ContainsKey(idUsuario))
            {
                empleadosActivos.Add(idUsuario, nombreUsuario);
            }
        }

        public static bool EsEmpleadoNoActivo(int idUsuario)
        {
            bool respuesta = false;
            if(idUsuario != 0)
            {
                respuesta = empleadosActivos.ContainsKey(idUsuario);   
            }
            return respuesta;
        }
       
        public static void QuitarEmpleadoDeLista(int idUser)
        {
            if (idUser != 0 && empleadosActivos.ContainsKey(idUser))
            {
                empleadosActivos.Remove(idUser);
            }
        }


    }
}
