using PdfGeneratorApp.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGeneratorApp.Templates
{
    public class FacturaDocument : IDocument
    {
        private readonly DocumentoRequest _factura;

        public FacturaDocument(DocumentoRequest factura)
        {
            _factura = factura;
        }
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Verdana));

                // 1. CABECERA (Ya la tienes bien encaminada)
                page.Header().Element(ComposeHeader);

                // 2. CONTENIDO (Aquí va la lógica de los productos)
                page.Content().Element(ComposeContent);

                // 3. PIE DE PÁGINA
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Página ");
                    x.CurrentPageNumber();
                });
            });
        }

        // Usamos métodos separados para que el código sea limpio y reutilizable
        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("MI EMPRESA S.A.C.").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
                    col.Item().Text("RUC: 12345678901");
                });

                row.ConstantItem(150).Background(Colors.Grey.Lighten3).Border(1).Column(col =>
                {
                    col.Item().Padding(5).AlignCenter().Text("FACTURA ELECTRÓNICA").Bold();
                    col.Item().Padding(5).AlignCenter().Text($"{_factura.Serie}-{_factura.Numero}");
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingTop(20).Column(col =>
            {
                // Datos del cliente
                col.Item().BorderBottom(1).PaddingBottom(5).Text("DATOS DEL CLIENTE").SemiBold();
                col.Item().Text($"Cliente: {_factura.Cliente.Nombre}");

                // Espacio
                col.Item().PaddingTop(15);

                // LA TABLA: Lo más importante en formatos de negocio
                col.Item().Table(table =>
                {
                    // Definir columnas: 4 columnas con distintos anchos
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(30);  // Cantidad
                        columns.RelativeColumn();    // Descripción (toma el resto del espacio)
                        columns.ConstantColumn(80);  // Precio Unit
                        columns.ConstantColumn(80);  // Total
                    });

                    // Encabezado de la tabla
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Cant.");
                        header.Cell().Element(CellStyle).Text("Descripción");
                        header.Cell().Element(CellStyle).Text("P. Unit");
                        header.Cell().Element(CellStyle).Text("Total");

                        // Estilo rápido para el header
                        static IContainer CellStyle(IContainer container) =>
                            container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                    });

                    // Filas de productos
                    foreach (var item in _factura.Items)
                    {
                        table.Cell().Element(ContentCell).Text($"{item.Cantidad}");
                        table.Cell().Element(ContentCell).Text(item.Descripcion);
                        table.Cell().Element(ContentCell).AlignRight().Text($"{item.PrecioUnitario:N2}");
                        table.Cell().Element(ContentCell).AlignRight().Text($"{item.Total:N2}");

                        static IContainer ContentCell(IContainer container) =>
                            container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                    }
                });

                // Totales
                col.Item().AlignRight().PaddingTop(10).Text($"Total: S/ {_factura.Total:N2}").FontSize(14).Bold();
            });
        }
    }
}
