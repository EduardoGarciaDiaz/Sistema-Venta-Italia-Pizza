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
    
    public partial class OrdenesCompra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdenesCompra()
        {
            this.OrdenesCompraInsumos = new HashSet<OrdenesCompraInsumos>();
        }
    
        public int IdOrdenCompra { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<int> IdEstadoOrdenCompra { get; set; }
        public Nullable<int> IdProveedor { get; set; }
    
        public virtual EstadosOrdenCompra EstadosOrdenCompra { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesCompraInsumos> OrdenesCompraInsumos { get; set; }
        public virtual Proveedores Proveedores { get; set; }
    }
}
