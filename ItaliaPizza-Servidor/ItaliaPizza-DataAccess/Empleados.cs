//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ItaliaPizza_DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Empleados
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Empleados()
        {
            this.CortesCaja = new HashSet<CortesCaja>();
            this.GastosVarios = new HashSet<GastosVarios>();
            this.UsuariosPedidos = new HashSet<UsuariosPedidos>();
        }
    
        public string NombreUsuario { get; set; }
        public string Contraseña { get; set; }
        public Nullable<int> IdTipoEmpleado { get; set; }
        public Nullable<int> IdUsuario { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CortesCaja> CortesCaja { get; set; }
        public virtual TiposEmpleado TiposEmpleado { get; set; }
        public virtual Usuarios Usuarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GastosVarios> GastosVarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuariosPedidos> UsuariosPedidos { get; set; }
    }
}
