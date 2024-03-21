using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using System.Net.Mail;
using System.Net;

namespace ItaliaPizza_Servicios.Auxiliares
{
    public static class ConversorAExcel
    {

        public static bool CrearExcelOrdenCompra(OrdenesCompra ordenes)
        {
            
            bool exito = false;
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];
            worksheet.Cells[0, 0].PutValue("Orden de compra numero: " + ordenes.IdOrdenCompra);
            worksheet.Cells[1, 0].PutValue("Dirigida a el proveedor: " + ordenes.Proveedores.NombreCompleto);
            worksheet.Cells[2, 0].PutValue("Insumo");
            worksheet.Cells[2, 1].PutValue("Cantidad");
            worksheet.Cells[2, 2].PutValue("Precio");
            worksheet.Cells[2, 3].PutValue("Subtotal");

            int index = 0;
            for (int i = 3; i < ordenes.OrdenesCompraInsumos.Count+3; i++)
            {
                int j = 0;
                worksheet.Cells[i, j].PutValue(ordenes.OrdenesCompraInsumos.ToArray()[index].Insumos.Productos.Nombre);
                int cantida =(int) ordenes.OrdenesCompraInsumos.ToArray()[index].CantidadInsumosAdquiridos;
                worksheet.Cells[i, j+1].PutValue(cantida);
                double costo = (double) ordenes.OrdenesCompraInsumos.ToArray()[index].Insumos.Costo;
                worksheet.Cells[i, j + 2].PutValue(costo);
                worksheet.Cells[i, j + 3].PutValue(cantida * costo);
                index++;
            }
            string ordenGuardada = "ordenCompra" + "_" + ordenes.IdOrdenCompra.ToString() + ".xlsx";
            workbook.Save(ordenGuardada);
            exito = DispachadorCorreos.EnviarCorreo(ordenes.Proveedores.CorreoElectronico, "Orden de compra", "Envio orden de compra", ordenGuardada);
            return exito;
        }




    }
}
