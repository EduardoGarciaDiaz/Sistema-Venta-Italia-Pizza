using ItaliaPizza_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using Aspose.Cells;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Element;
using System.Runtime.Remoting.Messaging;
using ItaliaPizza_DataAccess.Excepciones;


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
            byte[] pdfBytes = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    PdfWriter escritor = new PdfWriter(stream);
                    PdfDocument pdf = new PdfDocument(escritor);
                    Document document = new Document(pdf, PageSize.LETTER);
                    document.SetMargins(25, 25, 25, 25);

                    Paragraph titulo = new Paragraph("Reporte de Productos")
                        .SetFontSize(16);
                    document.Add(titulo);

                    Paragraph subtituloInsumos = new Paragraph("Insumo")
                        .SetFontSize(14);
                    document.Add(subtituloInsumos);

                    Table insumosTable = new Table(7).UseAllAvailableWidth();

                    iText.Layout.Element.Cell[] cabeceras = new iText.Layout.Element.Cell[]
                    {
                        new iText.Layout.Element.Cell().Add(new Paragraph("Codigo Producto")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Nombre")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Cantidad disponible")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Costo")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Unidad Medida")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Categoria")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Status"))
                    };
                    foreach (var cell in cabeceras)
                    {
                        insumosTable.AddCell(cell);
                    }

                    for (int fila = 0; fila < insumosLista.Count; fila++)
                    {
                        iText.Layout.Element.Cell[] cells = new iText.Layout.Element.Cell[]
                        {
                            new iText.Layout.Element.Cell().Add(new Paragraph(insumosLista[fila].Productos.CodigoProducto)),
                            new iText.Layout.Element.Cell().Add(new Paragraph(insumosLista[fila].Productos.Nombre)),
                            new iText.Layout.Element.Cell().Add(new Paragraph(insumosLista[fila].Cantidad.ToString())),
                            new iText.Layout.Element.Cell().Add(new Paragraph(insumosLista[fila].Costo.ToString())),
                            new iText.Layout.Element.Cell().Add(new Paragraph(insumosLista[fila].UnidadesMedida.Nombre)),
                            new iText.Layout.Element.Cell().Add(new Paragraph(insumosLista[fila].CategoriasInsumo.Nombre)),
                            new iText.Layout.Element.Cell().Add(new Paragraph((bool)insumosLista[fila].Productos.EsActivo ? "Activo" : "Inactivo"))
                        };

                        foreach (var cell in cells)
                        {
                            insumosTable.AddCell(cell);
                        }
                    }

                    document.Add(insumosTable);


                    Paragraph prodcutosVentaSubtitulo = new Paragraph("Productos")
                        .SetFontSize(14);
                    document.Add(prodcutosVentaSubtitulo);

                    Table tablaProductos = new Table(8).UseAllAvailableWidth();

                    iText.Layout.Element.Cell[] cabecerasProductos = new iText.Layout.Element.Cell[]
                    {
                        new iText.Layout.Element.Cell().Add(new Paragraph("Codigo Producto")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Nombre")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Precio")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Categoria")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Inventariado")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Cantidad")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Costo")),
                        new iText.Layout.Element.Cell().Add(new Paragraph("Status"))
                    };

                    foreach (var cell in cabecerasProductos)
                    {
                        tablaProductos.AddCell(cell);
                    }

                    for (int fila = 0; fila < productosLista.Count; fila++)
                    {
                        string esInventariado = "No";
                        string cantidad = "N/A";
                        string costo = "N/A";
                        if (productosLista[fila].Productos.Insumos != null)
                        {
                            esInventariado = "Si";
                            cantidad = productosLista[fila].Productos.Insumos.Cantidad.ToString();
                            costo = productosLista[fila].Productos.Insumos.Costo.ToString();
                        }
                        iText.Layout.Element.Cell[] celdasProducto = new iText.Layout.Element.Cell[]
                        {
                            new iText.Layout.Element.Cell().Add(new Paragraph(productosLista[fila].Productos.CodigoProducto)),
                            new iText.Layout.Element.Cell().Add(new Paragraph(productosLista[fila].Productos.Nombre)),
                            new iText.Layout.Element.Cell().Add(new Paragraph(productosLista[fila].Precio.ToString())),
                            new iText.Layout.Element.Cell().Add(new Paragraph(productosLista[fila].CategoriasProductoVenta.Nombre)),
                            new iText.Layout.Element.Cell().Add(new Paragraph(esInventariado)),
                            new iText.Layout.Element.Cell().Add(new Paragraph(cantidad)),
                            new iText.Layout.Element.Cell().Add(new Paragraph(costo)),
                            new iText.Layout.Element.Cell().Add(new Paragraph((bool)productosLista[fila].Productos.EsActivo ? "Activo" : "Inactivo"))
                        };

                        foreach (var cell in celdasProducto)
                        {
                            tablaProductos.AddCell(cell);
                        }
                    }

                    document.Add(tablaProductos);

                    document.Close();

                    pdfBytes = stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new ExcepcionDataAccess(ex.Message);
            }

            return pdfBytes;
        }

    }

}
