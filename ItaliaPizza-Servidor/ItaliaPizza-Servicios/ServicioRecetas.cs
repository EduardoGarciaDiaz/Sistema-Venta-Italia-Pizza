using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using ItaliaPizza_Servicios.Auxiliares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios
{
    public partial class ServicioItaliaPizza : ItaliaPizza_Contratos.IServicioRecetas
    {
        public void OperacionRecetasEjemplo()
        {
            throw new NotImplementedException();
        }

        public List<Receta> RecuperarRecetas()
        {
            RecetaDAO recetaDAO = new RecetaDAO();

            List<Receta> recetas = recetaDAO.RecuperarRecetas();

            return recetas;
        }
    }
}
