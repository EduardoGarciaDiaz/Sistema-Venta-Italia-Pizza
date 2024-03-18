using ItaliaPizza_Contratos.DTOs;
using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class AuxiliarConversorDTOADAO
    {
        public static Productos ConvertirProductoAProductos(Producto producto)
        {
            Productos productos = new Productos()
            {
                CodigoProducto = producto.Codigo,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                EsInventariado = producto.EsInventariado,
                EsActivo = producto.EsActivo
            };

            return productos;
        }

        public static Insumos ConvertirInsumoAInsumos(Insumo insumo)
        {
            Insumos insumos = new Insumos()
            {
                CodigoProducto = insumo.Codigo,
                Cantidad = insumo.Cantidad,
                Costo = insumo.CostoUnitario,
                Restricciones = insumo.Restriccion,
                IdUnidadMedida = insumo.UnidadMedida.Id,
                IdCategoriaInsumo = insumo.Categoria.Id
            };

            return insumos;
        }

        public static ProductosVenta ConvertirProductoVentaAProductosVenta(ProductoVenta productoVenta)
        {
            ProductosVenta productosVenta = new ProductosVenta()
            {
                CodigoProducto = productoVenta.Codigo,
                Precio = productoVenta.Precio,
                IdCategoriaProductoVenta = productoVenta.Categoria.Id,
                Foto = productoVenta.Foto
            };

            return productosVenta;
        }

        public static Direcciones ConvertirDireccionDtoADirecciones(DireccionDto direccionDto)
        {
            Direcciones direccion = new Direcciones() 
            {
                IdDireccion = direccionDto.IdDireccion,
                Colonia = direccionDto.Colonia,
                Ciudad = direccionDto.Ciudad,
                Calle = direccionDto.Calle,
                CodigoPostal = direccionDto.CodigoPostal,
                Numero = direccionDto.Numero,                
            };
            return direccion;
        }

        public static Usuarios ConvertirUsuarioDtoAUsuarios(UsuarioDto usuarioDto)
        {
            Usuarios usuario = new Usuarios()
            {
                IdUsuario = usuarioDto.IdUsuario,
                NombreCompleto = usuarioDto.NombreCompleto,
                NumeroTelefono = usuarioDto.NumeroTelefono,
                EsActivo = usuarioDto.EsActivo,
                CorreoElectronico = usuarioDto.CorreoElectronico,
                IdDireccion = usuarioDto.IdDireccion
            };
            return usuario;
        }

        public static Empleados ConvertirEmpleadoDtoAEmpleado(EmpleadoDto empleadoDto)
        {
            Empleados empleado = new Empleados()
            {
                IdUsuario = empleadoDto.IdUsuario,
                IdTipoEmpleado = empleadoDto.IdTipoEmpleado,
                Contraseña = empleadoDto.Contraseña,
                NombreUsuario = empleadoDto.NombreUsuario,
            };
            return empleado;
        }

        public static TipoEmpleadoDto ConvertirTiposEmpleadoATiposEmpleadoDto(TiposEmpleado tiposEmpleado)
        {
            TipoEmpleadoDto tip = new TipoEmpleadoDto() 
            {
                IdTipoEmpleado = tiposEmpleado.IdTipoEmpleado,
                Nombre = tiposEmpleado.Nombre,
            };
            return tip;

        }


        public static DireccionDto ConvertirDireccionesADireccionDto(Direcciones direccion)
        {
            DireccionDto direccionDto = new DireccionDto()
            {
                IdDireccion = direccion.IdDireccion,
                Colonia = direccion.Colonia,
                Ciudad = direccion.Ciudad,
                Calle = direccion.Calle,
                CodigoPostal = direccion.CodigoPostal,
                Numero = (int) direccion.Numero,
            };
            return direccionDto;
        }


        public static UsuarioDto ConvertirUsuariosAUsuarioDto(Usuarios usuario, Direcciones direccion)
        {
            UsuarioDto usuarioDto = new UsuarioDto()
            {
                IdUsuario = usuario.IdUsuario,
                NombreCompleto = usuario.NombreCompleto,
                NumeroTelefono = usuario.NumeroTelefono,
                EsActivo = (bool)usuario.EsActivo,
                CorreoElectronico = usuario.CorreoElectronico,
                IdDireccion = (int)usuario.IdDireccion,
                Direccion = ConvertirDireccionesADireccionDto(direccion)
            };
            return usuarioDto;
        }

        public static EmpleadoDto ConvertirEmpleadosAEmpleadoDto(Empleados empleado, String tipoEmpleado,  Usuarios usuario, Direcciones direccion)
        {
            EmpleadoDto empleadoDto = new EmpleadoDto()
            {
                IdUsuario =(int) empleado.IdUsuario,
                IdTipoEmpleado = (int) empleado.IdTipoEmpleado,
                Contraseña = empleado.Contraseña,
                NombreUsuario = empleado.NombreUsuario,
                Usuario = ConvertirUsuariosAUsuarioDto(usuario,direccion),
                TipoEmpleado = tipoEmpleado
            };
            return empleadoDto;
        }

    }
}
