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

        public List<InsumoReceta> RecuperarInsumosReceta(int idReceta)
        {
            List<InsumoReceta> insumosReceta = new List<InsumoReceta>();

            if (idReceta > 0)
            {
                RecetaDAO recetaDAO = new RecetaDAO();

                insumosReceta = recetaDAO.RecuperarInsumosReceta(idReceta);
            }

            return insumosReceta;
        }

        public int GuardarReceta(RecetaProducto receta)
        {
            int filasAfectadas = -1;

            RecetaDAO recetaDAO = new RecetaDAO();

            Recetas nuevaReceta = AuxiliarConversorDTOADAO.ConvertirRecetaProductoARecetas(receta);
            int id = recetaDAO.GuardarReceta(nuevaReceta);
            receta.IdReceta = id;

            List<RecetasInsumos> recetasInsumo = new List<RecetasInsumos>();

            foreach (InsumoReceta insumoReceta in receta.InsumosReceta)
            {
                recetasInsumo.Add(AuxiliarConversorDTOADAO.ConvertirInsumoRecetaARecetasInsumos(insumoReceta, receta.IdReceta));
            }

            filasAfectadas = recetaDAO.GuardarRecetaInsumos(recetasInsumo);

            return filasAfectadas;
        }
    }
}
