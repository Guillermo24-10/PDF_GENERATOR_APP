using PdfGeneratorApp.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGeneratorApp.Templates
{
    public class FacturaTicketDocument : IDocument
    {
        private readonly DocumentoRequest _request;

        public FacturaTicketDocument(DocumentoRequest request)
        {
            _request = request;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(new PageSize(80, 297, Unit.Millimetre));
                page.Margin(4, Unit.Millimetre);

                // CABECERA

                page.Header().Element(ComposeHeader);

                // CONTENIDO

                page.Content().Element(ComposeContent);
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Padding(5).Column(col =>
            {
                //DATA EMPRESA
                col.Item().Column(c =>
                {
                    c.Item().Text("LOGO / EMPRESA").SemiBold().FontSize(14).AlignCenter().FontFamily(Fonts.Courier);
                    c.Item().Text("Nombre de la Empresa").FontSize(10).AlignCenter().FontFamily(Fonts.Courier);
                    c.Item().Text("RUC: 123456789").FontSize(10).AlignCenter().FontFamily(Fonts.Courier);
                    c.Item().Text("Direccion:").FontSize(10).AlignCenter().FontFamily(Fonts.Courier);
                    c.Item().Text("Teléfono").FontSize(10).AlignCenter().FontFamily(Fonts.Courier);
                });

                col.Item().PaddingVertical(15).AlignCenter().Text("- - - - - - - - - - - - - - - - - - - - - - -");
                
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Padding(5).Column(col =>
            {
                //COMPROBANTE
                col.Item().Column(c =>
                {
                    c.Item().Text("FACTURA ELECTRÓNICA").SemiBold().FontSize(14).AlignCenter().FontFamily(Fonts.Courier);
                    c.Item().PaddingVertical(5).Text($"{_request.Serie} N° {_request.Numero}").FontSize(10).AlignCenter();
                });

                col.Item().PaddingVertical(15).AlignCenter().Text("- - - - - - - - - - - - - - - - - - - - - - -");
                //DATA CLIENTE
                col.Item().Column(c =>
                {
                    c.Item().Text($"CLIENTE :  {_request.Cliente.Nombre}").FontSize(12).AlignLeft().FontFamily(Fonts.Courier);
                    c.Item().PaddingVertical(5).Text($"DOC :  {_request.Cliente.Ruc}").FontSize(12).AlignLeft().FontFamily(Fonts.Courier);
                    c.Item().Text($"FECHA :  20/04/2026 10:30").FontSize(12).AlignLeft().FontFamily(Fonts.Courier);
                });

                col.Item().PaddingVertical(15).AlignCenter().Text("- - - - - - - - - - - - - - - - - - - - - - -");

                //DETALLE
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Descripcion
                        columns.RelativeColumn(1); //cant
                        columns.RelativeColumn(1); //P.U
                        columns.RelativeColumn(1); //Tot
                    });
                    // Encabezado tabla
                    table.Header(header =>
                    {
                        header.Cell().Text("DESCRIPCION").FontSize(6).Bold();
                        header.Cell().AlignRight().Text("CANT").FontSize(6).Bold();
                        header.Cell().AlignRight().Text("P.U.").FontSize(6).Bold();
                        header.Cell().AlignRight().Text("TOT").FontSize(6).Bold();
                        header.Cell().ColumnSpan(4).PaddingVertical(2).LineHorizontal(0.5f).LineColor(Colors.Black);
                    });

                    //items

                    foreach (var item in _request.Items)
                    {
                        table.Cell().Text($"{item.Descripcion}").FontSize(6);
                        table.Cell().AlignRight().Text($"{item.Cantidad}").FontSize(6);
                        table.Cell().AlignRight().Text($"{item.PrecioUnitario}").FontSize(6);
                        table.Cell().AlignRight().Text($"{item.Total}").FontSize(6);
                    }
                });

                col.Item().PaddingVertical(15).AlignCenter().Text("- - - - - - - - - - - - - - - - - - - - - - -");

                //totales
                col.Item().AlignRight().Column(c =>
                {

                    c.Item().Text($"OP. GRAVADA S/ {_request.OpGravada}").FontSize(8);
                    c.Item().PaddingVertical(5).Text($"I.G.V 18% S/ {_request.Igv}").FontSize(8);

                    c.Item().PaddingVertical(2).LineHorizontal(0.5f).LineColor(Colors.Black);

                    c.Item().PaddingVertical(5).Text($"IMPORTE TOTAL S/ {_request.Total}").SemiBold().FontSize(8);

                    c.Item().PaddingVertical(2).LineHorizontal(0.5f).LineColor(Colors.Black);
                });
                col.Item().PaddingTop(2).Text("SON: OCHENTA Y CINCO SOLES").FontSize(7).Italic();

                col.Item().PaddingVertical(15).AlignCenter().Text("- - - - - - - - - - - - - - - - - - - - - - -");
                //LEGAL

                col.Item().AlignCenter().Text("Representación impresa de la Boleta Electrónica").FontSize(8);

                //QR
                col.Item().AlignCenter().PaddingVertical(5).Width(50).Height(50).Background(Colors.Grey.Lighten3); // Espacio para QR

                col.Item().AlignCenter().Text("USUARIO: USERDEMO").FontSize(7);
                col.Item().AlignCenter().PaddingTop(5).Text("¡Gracias por su compra!").FontSize(8).Italic();

                // Línea de corte
                col.Item().PaddingTop(10).Text("- - - - - - - - - - - - - - - - - - - -  corte - - - - - - - - - - - - - - - - - - - -").FontSize(7).FontColor(Colors.Grey.Medium);
            });
        }
    }
}
