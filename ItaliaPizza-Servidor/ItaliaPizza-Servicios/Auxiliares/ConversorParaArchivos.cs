using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;
using System.Net.Mail;
using System.Net;
using System.Data.Entity.Core.Metadata.Edm;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using System.Web;
using System.IO;


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

    public static class GeneradorPDF
    {
       public static byte[] GenerarReproteProductosPDF(List<Insumos> insumosLista, List<ProductosVenta> productosLista)
        {

           byte[] pdfBytes  = new byte[1024];
            Document pdf = new Document();           
           try
           {
                Page pagina = pdf.Pages.Add();

                TextFragment titulo = new TextFragment("Reporte de Productos");
                titulo.TextState.FontSize = 16;
                titulo.TextState.FontStyle = FontStyles.Bold;
                pagina.Paragraphs.Add(titulo);

                TextFragment subtituloInsumos = new TextFragment("Insumo");
                subtituloInsumos.TextState.FontSize = 14;
                subtituloInsumos.TextState.FontStyle = FontStyles.Bold;
                pagina.Paragraphs.Add(subtituloInsumos);



                Table insumosTable = new Table();
                insumosTable.ColumnWidths = "100 100 100 100 100 100 100";
                Color colorCelda = Color.LightGray;
                for (int fila = 0; fila < insumosLista.Count; fila++)
                {
                    if (fila%2 == 0)
                    {
                        colorCelda = Color.LightGray;
                    }
                    else
                    {
                        colorCelda = Color.WhiteSmoke;
                    }
                    Aspose.Pdf.Row row = insumosTable.Rows.Add();
                    
                    Aspose.Pdf.Cell celda1 = row.Cells.Add(insumosLista.ToArray()[fila].Productos.CodigoProducto);
                    celda1.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda2 = row.Cells.Add(insumosLista.ToArray()[fila].Productos.Nombre);
                    celda2.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda3 = row.Cells.Add(insumosLista.ToArray()[fila].Cantidad.ToString());
                    celda3.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda4 = row.Cells.Add(insumosLista.ToArray()[fila].Costo.ToString());
                    celda4.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda5 = row.Cells.Add(insumosLista.ToArray()[fila].UnidadesMedida.Nombre);
                    celda5.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda6= row.Cells.Add(insumosLista.ToArray()[fila].CategoriasInsumo.Nombre);
                    celda6.BackgroundColor = colorCelda;
                    String activo;
                    if ((bool)insumosLista.ToArray()[fila].Productos.EsActivo)
                    {
                        activo = "Activo";
                    }
                    else
                    {
                        activo = "Inactivo";
                    }
                    Aspose.Pdf.Cell celda7 = row.Cells.Add(activo);
                    celda7.BackgroundColor = colorCelda;
                }
                pagina.Paragraphs.Add(insumosTable);

                TextFragment prodcutosVentaSubtitulo = new TextFragment("Productos");
                prodcutosVentaSubtitulo.TextState.FontSize = 14;
                prodcutosVentaSubtitulo.TextState.FontStyle = FontStyles.Bold;
                pagina.Paragraphs.Add(prodcutosVentaSubtitulo);


                Table tablaProductos = new Table();
                insumosTable.ColumnWidths = "100 100 100 100 100 100 100 100";
                for (int fila = 0; fila < productosLista.Count; fila++)
                {
                    if (fila % 2 == 0)
                    {
                        colorCelda = Color.LightGray;
                    }
                    else
                    {
                        colorCelda = Color.WhiteSmoke;
                    }
                    Aspose.Pdf.Row row = insumosTable.Rows.Add();

                    Aspose.Pdf.Cell celda1 = row.Cells.Add(productosLista.ToArray()[fila].Productos.CodigoProducto);
                    celda1.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda2 = row.Cells.Add(productosLista.ToArray()[fila].Productos.Nombre);
                    celda2.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda3 = row.Cells.Add(productosLista.ToArray()[fila].Precio.ToString());
                    celda3.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda8 = row.Cells.Add(productosLista.ToArray()[fila].CategoriasProductoVenta.Nombre);
                    celda8.BackgroundColor = colorCelda;

                    string esInventariado = "No";
                    string cantidad = "N/A";
                    string costo = "N/A";
                    if (productosLista.ToArray()[fila].Productos.Insumos != null)
                    {
                        esInventariado = "Si";
                        cantidad = productosLista.ToArray()[fila].Productos.Insumos.Cantidad.ToString();
                        costo = productosLista.ToArray()[fila].Productos.Insumos.Costo.ToString();
                    }
                    Aspose.Pdf.Cell celda4 = row.Cells.Add(esInventariado);
                    celda4.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda5 = row.Cells.Add(cantidad);
                    celda5.BackgroundColor = colorCelda;
                    Aspose.Pdf.Cell celda6 = row.Cells.Add(costo);
                    celda6.BackgroundColor = colorCelda;
                    String activo = "Activo";
                    if ((bool)productosLista.ToArray()[fila].Productos.EsActivo)
                    {                       
                        activo = "Inactivo";
                    }
                    Aspose.Pdf.Cell celda7 = row.Cells.Add(activo);
                    celda7.BackgroundColor = colorCelda;
                }
                pagina.Paragraphs.Add(tablaProductos);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    pdf.Save(memoryStream);
                    pdfBytes = memoryStream.ToArray();
                }
            }
            catch (Exception ex) 
            {
                
            }
            return pdfBytes;
        }
    
    }

}
