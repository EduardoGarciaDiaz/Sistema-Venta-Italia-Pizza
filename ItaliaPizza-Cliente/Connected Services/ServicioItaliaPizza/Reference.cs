﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ItaliaPizza_Cliente.ServicioItaliaPizza {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Categoria", Namespace="http://schemas.datacontract.org/2004/07/ItaliaPizza_Contratos.DTOs")]
    [System.SerializableAttribute()]
    public partial class Categoria : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                if ((object.ReferenceEquals(this.NombreField, value) != true)) {
                    this.NombreField = value;
                    this.RaisePropertyChanged("Nombre");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UnidadMedida", Namespace="http://schemas.datacontract.org/2004/07/ItaliaPizza_Contratos.DTOs")]
    [System.SerializableAttribute()]
    public partial class UnidadMedida : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id {
            get {
                return this.IdField;
            }
            set {
                if ((this.IdField.Equals(value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                if ((object.ReferenceEquals(this.NombreField, value) != true)) {
                    this.NombreField = value;
                    this.RaisePropertyChanged("Nombre");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Producto", Namespace="http://schemas.datacontract.org/2004/07/ItaliaPizza_Contratos.DTOs")]
    [System.SerializableAttribute()]
    public partial class Producto : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DescripcionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool EsActivoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool EsInventariadoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ItaliaPizza_Cliente.ServicioItaliaPizza.Insumo InsumoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ItaliaPizza_Cliente.ServicioItaliaPizza.ProductoVenta ProductoVentaField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Codigo {
            get {
                return this.CodigoField;
            }
            set {
                if ((object.ReferenceEquals(this.CodigoField, value) != true)) {
                    this.CodigoField = value;
                    this.RaisePropertyChanged("Codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Descripcion {
            get {
                return this.DescripcionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescripcionField, value) != true)) {
                    this.DescripcionField = value;
                    this.RaisePropertyChanged("Descripcion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool EsActivo {
            get {
                return this.EsActivoField;
            }
            set {
                if ((this.EsActivoField.Equals(value) != true)) {
                    this.EsActivoField = value;
                    this.RaisePropertyChanged("EsActivo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool EsInventariado {
            get {
                return this.EsInventariadoField;
            }
            set {
                if ((this.EsInventariadoField.Equals(value) != true)) {
                    this.EsInventariadoField = value;
                    this.RaisePropertyChanged("EsInventariado");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ItaliaPizza_Cliente.ServicioItaliaPizza.Insumo Insumo {
            get {
                return this.InsumoField;
            }
            set {
                if ((object.ReferenceEquals(this.InsumoField, value) != true)) {
                    this.InsumoField = value;
                    this.RaisePropertyChanged("Insumo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                if ((object.ReferenceEquals(this.NombreField, value) != true)) {
                    this.NombreField = value;
                    this.RaisePropertyChanged("Nombre");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ItaliaPizza_Cliente.ServicioItaliaPizza.ProductoVenta ProductoVenta {
            get {
                return this.ProductoVentaField;
            }
            set {
                if ((object.ReferenceEquals(this.ProductoVentaField, value) != true)) {
                    this.ProductoVentaField = value;
                    this.RaisePropertyChanged("ProductoVenta");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Insumo", Namespace="http://schemas.datacontract.org/2004/07/ItaliaPizza_Contratos.DTOs")]
    [System.SerializableAttribute()]
    public partial class Insumo : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private float CantidadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ItaliaPizza_Cliente.ServicioItaliaPizza.Categoria CategoriaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private float CostoUnitarioField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RestriccionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ItaliaPizza_Cliente.ServicioItaliaPizza.UnidadMedida UnidadMedidaField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float Cantidad {
            get {
                return this.CantidadField;
            }
            set {
                if ((this.CantidadField.Equals(value) != true)) {
                    this.CantidadField = value;
                    this.RaisePropertyChanged("Cantidad");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ItaliaPizza_Cliente.ServicioItaliaPizza.Categoria Categoria {
            get {
                return this.CategoriaField;
            }
            set {
                if ((object.ReferenceEquals(this.CategoriaField, value) != true)) {
                    this.CategoriaField = value;
                    this.RaisePropertyChanged("Categoria");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Codigo {
            get {
                return this.CodigoField;
            }
            set {
                if ((object.ReferenceEquals(this.CodigoField, value) != true)) {
                    this.CodigoField = value;
                    this.RaisePropertyChanged("Codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float CostoUnitario {
            get {
                return this.CostoUnitarioField;
            }
            set {
                if ((this.CostoUnitarioField.Equals(value) != true)) {
                    this.CostoUnitarioField = value;
                    this.RaisePropertyChanged("CostoUnitario");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Restriccion {
            get {
                return this.RestriccionField;
            }
            set {
                if ((object.ReferenceEquals(this.RestriccionField, value) != true)) {
                    this.RestriccionField = value;
                    this.RaisePropertyChanged("Restriccion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ItaliaPizza_Cliente.ServicioItaliaPizza.UnidadMedida UnidadMedida {
            get {
                return this.UnidadMedidaField;
            }
            set {
                if ((object.ReferenceEquals(this.UnidadMedidaField, value) != true)) {
                    this.UnidadMedidaField = value;
                    this.RaisePropertyChanged("UnidadMedida");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ProductoVenta", Namespace="http://schemas.datacontract.org/2004/07/ItaliaPizza_Contratos.DTOs")]
    [System.SerializableAttribute()]
    public partial class ProductoVenta : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private ItaliaPizza_Cliente.ServicioItaliaPizza.Categoria CategoriaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] FotoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private float PrecioField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public ItaliaPizza_Cliente.ServicioItaliaPizza.Categoria Categoria {
            get {
                return this.CategoriaField;
            }
            set {
                if ((object.ReferenceEquals(this.CategoriaField, value) != true)) {
                    this.CategoriaField = value;
                    this.RaisePropertyChanged("Categoria");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Codigo {
            get {
                return this.CodigoField;
            }
            set {
                if ((object.ReferenceEquals(this.CodigoField, value) != true)) {
                    this.CodigoField = value;
                    this.RaisePropertyChanged("Codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Foto {
            get {
                return this.FotoField;
            }
            set {
                if ((object.ReferenceEquals(this.FotoField, value) != true)) {
                    this.FotoField = value;
                    this.RaisePropertyChanged("Foto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public float Precio {
            get {
                return this.PrecioField;
            }
            set {
                if ((this.PrecioField.Equals(value) != true)) {
                    this.PrecioField = value;
                    this.RaisePropertyChanged("Precio");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Receta", Namespace="http://schemas.datacontract.org/2004/07/ItaliaPizza_Contratos.DTOs")]
    [System.SerializableAttribute()]
    public partial class Receta : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodigoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] FotoProductoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NombreField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Codigo {
            get {
                return this.CodigoField;
            }
            set {
                if ((object.ReferenceEquals(this.CodigoField, value) != true)) {
                    this.CodigoField = value;
                    this.RaisePropertyChanged("Codigo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] FotoProducto {
            get {
                return this.FotoProductoField;
            }
            set {
                if ((object.ReferenceEquals(this.FotoProductoField, value) != true)) {
                    this.FotoProductoField = value;
                    this.RaisePropertyChanged("FotoProducto");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Nombre {
            get {
                return this.NombreField;
            }
            set {
                if ((object.ReferenceEquals(this.NombreField, value) != true)) {
                    this.NombreField = value;
                    this.RaisePropertyChanged("Nombre");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServicioItaliaPizza.IServicioOrdenesCompra")]
    public interface IServicioOrdenesCompra {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioOrdenesCompra/OperacionOrdenesEjemplo", ReplyAction="http://tempuri.org/IServicioOrdenesCompra/OperacionOrdenesEjemploResponse")]
        void OperacionOrdenesEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioOrdenesCompra/OperacionOrdenesEjemplo", ReplyAction="http://tempuri.org/IServicioOrdenesCompra/OperacionOrdenesEjemploResponse")]
        System.Threading.Tasks.Task OperacionOrdenesEjemploAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioOrdenesCompraChannel : ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioOrdenesCompra, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioOrdenesCompraClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioOrdenesCompra>, ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioOrdenesCompra {
        
        public ServicioOrdenesCompraClient() {
        }
        
        public ServicioOrdenesCompraClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicioOrdenesCompraClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioOrdenesCompraClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioOrdenesCompraClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void OperacionOrdenesEjemplo() {
            base.Channel.OperacionOrdenesEjemplo();
        }
        
        public System.Threading.Tasks.Task OperacionOrdenesEjemploAsync() {
            return base.Channel.OperacionOrdenesEjemploAsync();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServicioItaliaPizza.IServicioPedidos")]
    public interface IServicioPedidos {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioPedidos/OperacionPedidosEjemplo", ReplyAction="http://tempuri.org/IServicioPedidos/OperacionPedidosEjemploResponse")]
        void OperacionPedidosEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioPedidos/OperacionPedidosEjemplo", ReplyAction="http://tempuri.org/IServicioPedidos/OperacionPedidosEjemploResponse")]
        System.Threading.Tasks.Task OperacionPedidosEjemploAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioPedidosChannel : ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioPedidos, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioPedidosClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioPedidos>, ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioPedidos {
        
        public ServicioPedidosClient() {
        }
        
        public ServicioPedidosClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicioPedidosClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioPedidosClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioPedidosClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void OperacionPedidosEjemplo() {
            base.Channel.OperacionPedidosEjemplo();
        }
        
        public System.Threading.Tasks.Task OperacionPedidosEjemploAsync() {
            return base.Channel.OperacionPedidosEjemploAsync();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServicioItaliaPizza.IServicioProductos")]
    public interface IServicioProductos {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/OperacionProductosEjemplo", ReplyAction="http://tempuri.org/IServicioProductos/OperacionProductosEjemploResponse")]
        void OperacionProductosEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/OperacionProductosEjemplo", ReplyAction="http://tempuri.org/IServicioProductos/OperacionProductosEjemploResponse")]
        System.Threading.Tasks.Task OperacionProductosEjemploAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/RecuperarCategorias", ReplyAction="http://tempuri.org/IServicioProductos/RecuperarCategoriasResponse")]
        ItaliaPizza_Cliente.ServicioItaliaPizza.Categoria[] RecuperarCategorias();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/RecuperarCategorias", ReplyAction="http://tempuri.org/IServicioProductos/RecuperarCategoriasResponse")]
        System.Threading.Tasks.Task<ItaliaPizza_Cliente.ServicioItaliaPizza.Categoria[]> RecuperarCategoriasAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/RecuperarUnidadesMedida", ReplyAction="http://tempuri.org/IServicioProductos/RecuperarUnidadesMedidaResponse")]
        ItaliaPizza_Cliente.ServicioItaliaPizza.UnidadMedida[] RecuperarUnidadesMedida();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/RecuperarUnidadesMedida", ReplyAction="http://tempuri.org/IServicioProductos/RecuperarUnidadesMedidaResponse")]
        System.Threading.Tasks.Task<ItaliaPizza_Cliente.ServicioItaliaPizza.UnidadMedida[]> RecuperarUnidadesMedidaAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/ValidarCodigoProducto", ReplyAction="http://tempuri.org/IServicioProductos/ValidarCodigoProductoResponse")]
        bool ValidarCodigoProducto(string codigoProducto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/ValidarCodigoProducto", ReplyAction="http://tempuri.org/IServicioProductos/ValidarCodigoProductoResponse")]
        System.Threading.Tasks.Task<bool> ValidarCodigoProductoAsync(string codigoProducto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/GuardarProducto", ReplyAction="http://tempuri.org/IServicioProductos/GuardarProductoResponse")]
        int GuardarProducto(ItaliaPizza_Cliente.ServicioItaliaPizza.Producto producto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/GuardarProducto", ReplyAction="http://tempuri.org/IServicioProductos/GuardarProductoResponse")]
        System.Threading.Tasks.Task<int> GuardarProductoAsync(ItaliaPizza_Cliente.ServicioItaliaPizza.Producto producto);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioProductosChannel : ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioProductos, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioProductosClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioProductos>, ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioProductos {
        
        public ServicioProductosClient() {
        }
        
        public ServicioProductosClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicioProductosClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioProductosClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioProductosClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void OperacionProductosEjemplo() {
            base.Channel.OperacionProductosEjemplo();
        }
        
        public System.Threading.Tasks.Task OperacionProductosEjemploAsync() {
            return base.Channel.OperacionProductosEjemploAsync();
        }
        
        public ItaliaPizza_Cliente.ServicioItaliaPizza.Categoria[] RecuperarCategorias() {
            return base.Channel.RecuperarCategorias();
        }
        
        public System.Threading.Tasks.Task<ItaliaPizza_Cliente.ServicioItaliaPizza.Categoria[]> RecuperarCategoriasAsync() {
            return base.Channel.RecuperarCategoriasAsync();
        }
        
        public ItaliaPizza_Cliente.ServicioItaliaPizza.UnidadMedida[] RecuperarUnidadesMedida() {
            return base.Channel.RecuperarUnidadesMedida();
        }
        
        public System.Threading.Tasks.Task<ItaliaPizza_Cliente.ServicioItaliaPizza.UnidadMedida[]> RecuperarUnidadesMedidaAsync() {
            return base.Channel.RecuperarUnidadesMedidaAsync();
        }
        
        public bool ValidarCodigoProducto(string codigoProducto) {
            return base.Channel.ValidarCodigoProducto(codigoProducto);
        }
        
        public System.Threading.Tasks.Task<bool> ValidarCodigoProductoAsync(string codigoProducto) {
            return base.Channel.ValidarCodigoProductoAsync(codigoProducto);
        }
        
        public int GuardarProducto(ItaliaPizza_Cliente.ServicioItaliaPizza.Producto producto) {
            return base.Channel.GuardarProducto(producto);
        }
        
        public System.Threading.Tasks.Task<int> GuardarProductoAsync(ItaliaPizza_Cliente.ServicioItaliaPizza.Producto producto) {
            return base.Channel.GuardarProductoAsync(producto);
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServicioItaliaPizza.IServicioRecetas")]
    public interface IServicioRecetas {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioRecetas/OperacionRecetasEjemplo", ReplyAction="http://tempuri.org/IServicioRecetas/OperacionRecetasEjemploResponse")]
        void OperacionRecetasEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioRecetas/OperacionRecetasEjemplo", ReplyAction="http://tempuri.org/IServicioRecetas/OperacionRecetasEjemploResponse")]
        System.Threading.Tasks.Task OperacionRecetasEjemploAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioRecetas/RecuperarRecetas", ReplyAction="http://tempuri.org/IServicioRecetas/RecuperarRecetasResponse")]
        ItaliaPizza_Cliente.ServicioItaliaPizza.Receta[] RecuperarRecetas();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioRecetas/RecuperarRecetas", ReplyAction="http://tempuri.org/IServicioRecetas/RecuperarRecetasResponse")]
        System.Threading.Tasks.Task<ItaliaPizza_Cliente.ServicioItaliaPizza.Receta[]> RecuperarRecetasAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioRecetasChannel : ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioRecetas, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioRecetasClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioRecetas>, ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioRecetas {
        
        public ServicioRecetasClient() {
        }
        
        public ServicioRecetasClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicioRecetasClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioRecetasClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioRecetasClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void OperacionRecetasEjemplo() {
            base.Channel.OperacionRecetasEjemplo();
        }
        
        public System.Threading.Tasks.Task OperacionRecetasEjemploAsync() {
            return base.Channel.OperacionRecetasEjemploAsync();
        }
        
        public ItaliaPizza_Cliente.ServicioItaliaPizza.Receta[] RecuperarRecetas() {
            return base.Channel.RecuperarRecetas();
        }
        
        public System.Threading.Tasks.Task<ItaliaPizza_Cliente.ServicioItaliaPizza.Receta[]> RecuperarRecetasAsync() {
            return base.Channel.RecuperarRecetasAsync();
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServicioItaliaPizza.IServicioUsuarios")]
    public interface IServicioUsuarios {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioUsuarios/OperacionUsuariosEjemplo", ReplyAction="http://tempuri.org/IServicioUsuarios/OperacionUsuariosEjemploResponse")]
        void OperacionUsuariosEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioUsuarios/OperacionUsuariosEjemplo", ReplyAction="http://tempuri.org/IServicioUsuarios/OperacionUsuariosEjemploResponse")]
        System.Threading.Tasks.Task OperacionUsuariosEjemploAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioUsuariosChannel : ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioUsuarios, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioUsuariosClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioUsuarios>, ItaliaPizza_Cliente.ServicioItaliaPizza.IServicioUsuarios {
        
        public ServicioUsuariosClient() {
        }
        
        public ServicioUsuariosClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicioUsuariosClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioUsuariosClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicioUsuariosClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void OperacionUsuariosEjemplo() {
            base.Channel.OperacionUsuariosEjemplo();
        }
        
        public System.Threading.Tasks.Task OperacionUsuariosEjemploAsync() {
            return base.Channel.OperacionUsuariosEjemploAsync();
        }
    }
}
