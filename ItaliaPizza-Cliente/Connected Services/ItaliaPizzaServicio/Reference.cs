﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ItaliaPizza_Cliente.ItaliaPizzaServicio {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ItaliaPizzaServicio.IServicioOrdenesCompra")]
    public interface IServicioOrdenesCompra {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioOrdenesCompra/OperacionOrdenesEjemplo", ReplyAction="http://tempuri.org/IServicioOrdenesCompra/OperacionOrdenesEjemploResponse")]
        void OperacionOrdenesEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioOrdenesCompra/OperacionOrdenesEjemplo", ReplyAction="http://tempuri.org/IServicioOrdenesCompra/OperacionOrdenesEjemploResponse")]
        System.Threading.Tasks.Task OperacionOrdenesEjemploAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioOrdenesCompraChannel : ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioOrdenesCompra, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioOrdenesCompraClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioOrdenesCompra>, ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioOrdenesCompra {
        
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ItaliaPizzaServicio.IServicioPedidos")]
    public interface IServicioPedidos {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioPedidos/OperacionPedidosEjemplo", ReplyAction="http://tempuri.org/IServicioPedidos/OperacionPedidosEjemploResponse")]
        void OperacionPedidosEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioPedidos/OperacionPedidosEjemplo", ReplyAction="http://tempuri.org/IServicioPedidos/OperacionPedidosEjemploResponse")]
        System.Threading.Tasks.Task OperacionPedidosEjemploAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioPedidosChannel : ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioPedidos, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioPedidosClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioPedidos>, ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioPedidos {
        
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ItaliaPizzaServicio.IServicioProductos")]
    public interface IServicioProductos {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/OperacionProductosEjemplo", ReplyAction="http://tempuri.org/IServicioProductos/OperacionProductosEjemploResponse")]
        void OperacionProductosEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioProductos/OperacionProductosEjemplo", ReplyAction="http://tempuri.org/IServicioProductos/OperacionProductosEjemploResponse")]
        System.Threading.Tasks.Task OperacionProductosEjemploAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioProductosChannel : ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioProductos, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioProductosClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioProductos>, ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioProductos {
        
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
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ItaliaPizzaServicio.IServicioRecetas")]
    public interface IServicioRecetas {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioRecetas/OperacionRecetasEjemplo", ReplyAction="http://tempuri.org/IServicioRecetas/OperacionRecetasEjemploResponse")]
        void OperacionRecetasEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioRecetas/OperacionRecetasEjemplo", ReplyAction="http://tempuri.org/IServicioRecetas/OperacionRecetasEjemploResponse")]
        System.Threading.Tasks.Task OperacionRecetasEjemploAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioRecetasChannel : ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioRecetas, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioRecetasClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioRecetas>, ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioRecetas {
        
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
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ItaliaPizzaServicio.IServicioUsuarios")]
    public interface IServicioUsuarios {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioUsuarios/OperacionUsuariosEjemplo", ReplyAction="http://tempuri.org/IServicioUsuarios/OperacionUsuariosEjemploResponse")]
        void OperacionUsuariosEjemplo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicioUsuarios/OperacionUsuariosEjemplo", ReplyAction="http://tempuri.org/IServicioUsuarios/OperacionUsuariosEjemploResponse")]
        System.Threading.Tasks.Task OperacionUsuariosEjemploAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicioUsuariosChannel : ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioUsuarios, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicioUsuariosClient : System.ServiceModel.ClientBase<ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioUsuarios>, ItaliaPizza_Cliente.ItaliaPizzaServicio.IServicioUsuarios {
        
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
