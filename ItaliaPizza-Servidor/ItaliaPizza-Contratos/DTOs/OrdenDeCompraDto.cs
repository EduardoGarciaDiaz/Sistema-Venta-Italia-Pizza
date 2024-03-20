using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Contratos.DTOs
{
    [DataContract]
    public class OrdenDeCompraDto
    {
        [DataMember]
        public int IdOrdenCompra { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public int IdEstadoOrdenCompra { get; set; }

        [DataMember]
        public int IdProveedor { get; set; }

        [DataMember]
        public ProveedorDto Proveedor { get; set; }

        [DataMember]
        public List<ElementoOrdenCompraDto> listaElementosOrdenCompra { get; set; }
    
    }

    [DataContract]
    public class ElementoOrdenCompraDto
    {
        [DataMember]
        public int IdElementoOrdenCompra { get; set; }

        [DataMember]
        public InsumoOrdenCompraDto InsumoOrdenCompraDto { get; set; }

        [DataMember]
        public int CantidadInsumosAdquiridos { get; set; }
    }


}
