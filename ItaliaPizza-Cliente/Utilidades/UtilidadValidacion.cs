using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ItaliaPizza_Cliente.Utilidades
{
    public static class UtilidadValidacion
    {
        private const string NOMBRE_PRODUCTO_VALIDO = "^[a-zA-ZáéíóúÁÉÍÓÚüÜ&\\- ]{2,50}$";
        private const string CODIGO_PRODUCTO_VALIDO = "^[a-zA-Z0-9-]{1,15}$";
        private const string DESCRIPCION_PRODUCTO_VALIDA = "^[\\w\\s\\d\\S]{1,200}$";
        private const string RESTRICCION_INSUMO_VALIDA = "^[\\w\\s\\d\\S]{0,500}$";

        public static bool EsNombreProductoValido(string nombreProducto)
        {
            Regex regex = new Regex(NOMBRE_PRODUCTO_VALIDO);

            return regex.IsMatch(nombreProducto.Trim());
        }

        public static bool EsCodigoProductoValido(string codigoProducto)
        {
            Regex regex = new Regex(CODIGO_PRODUCTO_VALIDO);

            return regex.IsMatch(codigoProducto.Trim());
        }

        public static bool EsDescripcionProductoValida(string descripcionProducto)
        {
            Regex regex = new Regex(DESCRIPCION_PRODUCTO_VALIDA);

            return regex.IsMatch(descripcionProducto.Trim());
        }

        public static bool esRestriccionInsumoValida(string restriccionInsumo)
        {
            Regex regex = new Regex(RESTRICCION_INSUMO_VALIDA);

            return regex.IsMatch(restriccionInsumo.Trim());
        }
    }
}