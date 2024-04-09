using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using ItaliaPizza_DataAccess.Excepciones;
using ItaliaPizza_Servicios.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioInicioSesion
    {
        public void CerrarSesion(int idEmpleado)
        {
            ListaEmpleadoActivos.QuitarEmpleadoDeLista(idEmpleado);
        }

        public int ValidarCredenciales(string nombreUsuario, string contraseña)
        {
            try
            {
                int resultado = 0;
                if (!String.IsNullOrEmpty(nombreUsuario) || !String.IsNullOrEmpty(contraseña))
                {
                    resultado = EmpleadoDAO.ValidarCredencialesBD(nombreUsuario, contraseña);
                    if (resultado == 1)
                    {
                        if (!ListaEmpleadoActivos.EsEmpleadoNoActivo(nombreUsuario))
                        {
                            resultado = -1;
                        }
                    }
                }
                return resultado;
            }
            catch (ExcepcionDataAccess e)
            {
                throw ExcepcionServidorItaliaPizzaManager.ManejarExcepcionDataAccess(e);
            }
        }

    }
}
