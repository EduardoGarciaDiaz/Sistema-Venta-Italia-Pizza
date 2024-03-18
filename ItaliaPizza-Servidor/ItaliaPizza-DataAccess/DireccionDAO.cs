using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_DataAccess
{
    public static class DireccionDAO
    {
        public static int GuardarDireccionNuevaBD(Direcciones direccionNueva)
        {
            int resultadoOperacion = 0;
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {

                    context.Direcciones.Add(direccionNueva);
                    context.SaveChanges();
                    resultadoOperacion = direccionNueva.IdDireccion;
                }
            }
            catch (Exception ex)
            {

            }
            return resultadoOperacion;
        }

        public static List<Direcciones> RecuperarDireccionesBD(List<int?> idDirecciones)
        {
            List<Direcciones> direcciones = new List<Direcciones>();
            try
            {
                using (var context = new ItaliaPizzaEntities())
                {
                    direcciones = context.Direcciones.Where(direccion => idDirecciones.Contains(direccion.IdDireccion)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return direcciones;        
        }

    }

}
