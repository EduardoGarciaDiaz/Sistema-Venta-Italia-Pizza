//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ItaliaPizza_DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class RecetasInsumos
    {
        public int IdRecetaInsumo { get; set; }
        public Nullable<int> CantidadInsumo { get; set; }
        public Nullable<int> IdReceta { get; set; }
        public string CodigoProducto { get; set; }
    
        public virtual Insumos Insumos { get; set; }
        public virtual Recetas Recetas { get; set; }
    }
}
