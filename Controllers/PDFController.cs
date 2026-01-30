using iText.Html2pdf;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using n8n_urilisSWA.Models;

namespace n8n_urilisSWA.Controllers
{
    public class PDFController : Controller
    {
        public class HtmlToPdfRequest
        {
            public string Html { get; set; }
            public string FileName { get; set; }
        }



        //[HttpPost("html-to-pdf")]
        //public IActionResult HtmlToPdf([FromBody] string html)
        //{
        //    if (string.IsNullOrEmpty(html))
        //        return BadRequest("No se proporcionó HTML.");

        //    using var ms = new MemoryStream();

        //    // Convertir HTML a PDF
        //    HtmlConverter.ConvertToPdf(html, ms);

        //    var pdfBytes = ms.ToArray();
        //    return File(pdfBytes, "application/pdf", "documento.pdf");
        //}


        [HttpPost("html-to-pdf")]
        public IActionResult HtmlToPdf([FromBody] HtmlToPdfRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Html))
                return BadRequest("No se proporcionó HTML.");

            // Fecha actual en formato dd-MM-yyyy
            var fecha = DateTime.Now.ToString("dd-MM-yyyy");

            // Nombre base (si no mandan nombre)
            var baseName = string.IsNullOrWhiteSpace(request.FileName)
                ? "documento"
                : request.FileName;

            // Limpiar caracteres inválidos
            baseName = string.Concat(baseName.Split(Path.GetInvalidFileNameChars()));

            // Nombre final
            var fileName = $"{baseName}_{fecha}.pdf";

            using var ms = new MemoryStream();

            // Convertir HTML a PDF
            HtmlConverter.ConvertToPdf(request.Html, ms);

            var pdfBytes = ms.ToArray();
            return File(pdfBytes, "application/pdf", fileName);
        }






        [HttpPost("pedidos-to-pdf")]
        public IActionResult PedidosToPdf([FromBody] List<Pedido> pedidos)
        {
            if (pedidos == null || !pedidos.Any())
                return BadRequest("No hay pedidos para procesar.");

            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            // Fuentes
            PdfFont boldFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            PdfFont regularFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);

            foreach (var pedido in pedidos)
            {
                // Título del pedido en negrita
                document.Add(new Paragraph($"Pedido: {pedido.Nombre_Cliente} ({pedido.Numero})")
                    .SetFont(boldFont)
                    .SetFontSize(14)
                    .SetFontColor(ColorConstants.BLACK));

                // Detalles
                document.Add(new Paragraph($"Dirección: {pedido.Direccion}")
                    .SetFont(regularFont)
                    .SetFontSize(12));
                document.Add(new Paragraph($"Fecha: {pedido.Fecha}")
                    .SetFont(regularFont)
                    .SetFontSize(12));

                // Tabla de productos
                var table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 2, 2 }))
                    .UseAllAvailableWidth();

                table.AddHeaderCell(new Cell().Add(new Paragraph("Producto").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Cantidad").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Unidad").SetFont(boldFont)));

                foreach (var prod in pedido.Productos)
                {
                    table.AddCell(new Cell().Add(new Paragraph(prod.Producto).SetFont(regularFont)));
                    table.AddCell(new Cell().Add(new Paragraph(prod.Cantidad.ToString()).SetFont(regularFont)));
                    table.AddCell(new Cell().Add(new Paragraph(prod.Unidad).SetFont(regularFont)));
                }

                document.Add(table);

                // Salto de página
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            }

            document.Close();

            var pdfBytes = ms.ToArray();
            return File(pdfBytes, "application/pdf", "pedidos.pdf");
        }

        [HttpPost("productos-to-pdf")]
        public IActionResult PedidosToPdf([FromBody] List<PedidoPdf> pedidos)
        {
            if (pedidos == null || !pedidos.Any())
                return BadRequest("No hay pedidos para procesar.");

            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            // Fuentes
            PdfFont boldFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            PdfFont regularFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);

            foreach (var pedidoGroup in pedidos)
            {
                // Título general del pedido
                document.Add(new Paragraph($"Pedido - Fecha: {pedidoGroup.Fecha}")
                    .SetFont(boldFont)
                    .SetFontSize(14)
                    .SetFontColor(ColorConstants.BLACK));

                // Tabla de productos
                var table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 2, 2 }))
                    .UseAllAvailableWidth();

                table.AddHeaderCell(new Cell().Add(new Paragraph("Producto").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Cantidad").SetFont(boldFont)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Unidad").SetFont(boldFont)));

                foreach (var prod in pedidoGroup.Pedido)
                {
                    table.AddCell(new Cell().Add(new Paragraph(prod.Producto).SetFont(regularFont)));
                    table.AddCell(new Cell().Add(new Paragraph(prod.Cantidad.ToString()).SetFont(regularFont)));
                    table.AddCell(new Cell().Add(new Paragraph(prod.Unidad).SetFont(regularFont)));
                }

                document.Add(table);
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            }

            document.Close();

            var pdfBytes = ms.ToArray();
            return File(pdfBytes, "application/pdf", "pedidos.pdf");
        }
    }
}
