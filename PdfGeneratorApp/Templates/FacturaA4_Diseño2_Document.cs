using PdfGeneratorApp.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfGeneratorApp.Templates
{
    public class FacturaA4_Diseño2_Document : IDocument
    {
        private readonly DocumentoRequest _factura;

        public FacturaA4_Diseño2_Document(DocumentoRequest factura)
        {
            _factura = factura;
        }

        public DocumentMetadata GetMetadata()
        {
            return new DocumentMetadata
            {
                Title = $"Factura {_factura.Serie}-{_factura.Numero}",
                Author = "Guillermo Paredes",
                Subject = "Factura",
                Keywords = "Factura, PDF, QuestPDF"
            };
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(1, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.Verdana));

                //header
                page.Header().Element(ComposeHeader);

                //content

                page.Content().Element(ComposeContent);

                // footer

                page.Footer().Element(ComposeFooter);
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                //LOGO
                row.ConstantItem(100).Height(50).Placeholder();

                //DATOS DE LA EMPRESA
                row.RelativeItem().PaddingLeft(10).Column(col =>
                {
                    col.Item().AlignCenter().Text("JDM TECNOLOGIA Y SOLUCIONES GLOBALES S.A.C.").Bold().FontSize(12);
                    col.Item().AlignCenter().Text("AV. DIRECCIÓN NRO. 123");
                    col.Item().AlignCenter().Text("LIMA - LIMA - SAN ISIDRO");
                });

                row.ConstantItem(150).Border(1).Column(col =>
                {
                    col.Item().PaddingVertical(5).AlignCenter().Text("R.U.C. 20600705785");
                    col.Item().Background(Colors.Grey.Lighten3).PaddingVertical(5).AlignCenter().Text("FACTURA ELECTRÓNICA").Bold();
                    col.Item().PaddingVertical(5).AlignCenter().Text($"{_factura.Serie} N° {_factura.Numero.PadLeft(8, '0')}");
                });
            });
        }
        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(10).Column(col =>
            {
                // BLOQUE CLIENTE
                col.Item().Border(0.5f).Padding(5).Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text(t =>
                        {
                            t.Span("SEÑOR(ES):").Bold();
                            t.Span(_factura.Cliente.Nombre);
                        });
                        c.Item().PaddingVertical(2).Text(t =>
                        {
                            t.Span("RUC:").Bold();
                            t.Span(_factura.Cliente.Ruc);
                        });
                        c.Item().Text(t =>
                        {
                            t.Span("DIRECCIÓN:").Bold();
                            t.Span(_factura.Cliente.Direccion);
                        });
                    });
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text(t =>
                        {
                            t.Span("FECHA EMISIÓN: ").Bold();
                            t.Span(_factura.Fecha);
                        });
                        c.Item().PaddingVertical(2).Text(t =>
                        {
                            t.Span("FECHA VENCIMIENTO: ").Bold();
                            t.Span(_factura.Fecha);
                        }); c.Item().PaddingVertical(2).Text(t =>
                        {
                            t.Span("FECHA PAGO: ").Bold();
                            t.Span(_factura.Fecha);
                        }); c.Item().Text(t =>
                        {
                            t.Span("MONTO NETO PEND.: ").Bold();
                            t.Span("800.00");
                        });
                    });
                });

                //TABLA ITEMS

                col.Item().Padding(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(50);//CODIGO
                        columns.ConstantColumn(60);//CANTIDAD
                        columns.ConstantColumn(60);//UNIDAD
                        columns.RelativeColumn();//DESCRIPCION
                        columns.ConstantColumn(60);//PRECIO
                        columns.ConstantColumn(60);//IMPORTE
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderStyle).Text("Código");
                        header.Cell().Element(HeaderStyle).Text("Cantidad");
                        header.Cell().Element(HeaderStyle).Text("Unidad");
                        header.Cell().Element(HeaderStyle).Text("Descripción");
                        header.Cell().Element(HeaderStyle).Text("Precio");
                        header.Cell().Element(HeaderStyle).Text("Importe");

                        static IContainer HeaderStyle(IContainer container) =>
                        container.DefaultTextStyle(x => x.Bold()).PaddingVertical(5).BorderBottom(1);
                    });

                    //FILAS
                    int i = 1;
                    foreach (var item in _factura.Items)
                    {

                        table.Cell().Element(CellStyle).Text($"{i}");
                        table.Cell().Element(CellStyle).Text($"{item.Cantidad}");
                        table.Cell().Element(CellStyle).Text("UND");
                        table.Cell().Element(CellStyle).Text($"{item.Descripcion}");
                        table.Cell().Element(CellStyle) .Text($"{item.PrecioUnitario}");
                        table.Cell().Element(CellStyle).Text($"{item.Total}");
                        i++;
                    }

                    static IContainer CellStyle(IContainer container) =>
                        container.BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                });

                //CUOTAS

                col.Item().PaddingTop(10).AlignRight().Width(250).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("IDCUOTA").FontColor(Colors.White).Bold();
                        header.Cell().Element(CellStyle).Text("Monto").FontColor(Colors.White).Bold();
                        header.Cell().Element(CellStyle).Text("Fecha pago").FontColor(Colors.White).Bold();

                        static IContainer CellStyle(IContainer container) =>
                            container.Border(0.5f).Background(Colors.Grey.Darken1).Padding(2).AlignCenter();
                    });

                    var cuotas = new[]
                    {
                        new
                        {
                            IDCUOTA="Cuota001",
                            Monto = 500.00,
                            FechaPago = "20/04/2026"
                        },
                         new
                        {
                            IDCUOTA="Cuota002",
                            Monto = 300.00,
                            FechaPago = "20/04/2026"
                        }
                    };

                    foreach (var item in cuotas)
                    {
                        table.Cell().Element(ContentStyle).Text(item.IDCUOTA);
                        table.Cell().Element(ContentStyle).Text($"S/{item.Monto}");
                        table.Cell().Element(ContentStyle).Text(item.FechaPago);
                    }

                    static IContainer ContentStyle(IContainer container) =>
                        container.Border(0.5f).Padding(5).AlignCenter();
                });

                col.Item().PaddingVertical(100).Row(row =>
                {
                    // LADO IZQUIERDO: Información adicional
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text("SON : NOVECIENTOS Y 00/100 SOLES").FontSize(8);
                        c.Item().PaddingTop(5).Text("BENEFICIARIO : JDM TECNOLOGIA Y SOL. GLOBALES").FontSize(8);
                        c.Item().Text("CUENTAS:").FontSize(8);
                        c.Item().PaddingTop(10).Text("OBSERVACIONES :").FontSize(8);
                    });

                    // LADO DERECHO: Cuadro de Totales (con fondo beige como la imagen)
                    row.ConstantItem(200).Background("#E8E2C4").Border(0.5f).Table(table =>
                    {
                        table.ColumnsDefinition(columns => {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        // Método auxiliar para filas de totales
                        void AddTotalRow(string label, string value, bool isBold = false)
                        {
                            table.Cell().Padding(2).Text(label).FontSize(8);
                            table.Cell().Padding(2).AlignRight().Text(value).FontSize(8);
                        }

                        AddTotalRow("OP. GRAVADA", $"{_factura.OpGravada}");
                        AddTotalRow("OP. INAFECTA", $"{_factura.OpInafecta}");
                        AddTotalRow("I.G.V. 18%", $"{_factura.Igv}");

                        // Fila de Total Final con color diferente
                        table.Cell().Background("#D4C9A1").Padding(2).Text("IMPORTE TOTAL").Bold().FontSize(9);
                        table.Cell().Background("#D4C9A1").Padding(2).AlignRight().Text($"{_factura.Total}").Bold().FontSize(9);
                    });
                });
            });
        }

        void ComposeFooter(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().PaddingTop(10).Border(1).BorderColor("#A291D8").Padding(10).Row(row =>
                {
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().AlignCenter().Text("Representación impresa de la Factura Electrónica").FontSize(8);
                        c.Item().AlignCenter().Text("Autorizado mediante resolución N° 0340050005820/SUNAT").FontSize(7);
                        c.Item().AlignCenter().PaddingTop(5).Text("Consulte su documento electrónico en:").FontSize(8);
                        c.Item().AlignCenter().Text("(https://consulta.sunat.pe/...)").FontSize(8).FontColor(Colors.Blue.Medium);

                        c.Item().PaddingTop(10).Row(r2 => {
                            r2.RelativeItem().Text("USUARIO: USERDEMO").FontSize(7);
                            r2.RelativeItem().AlignRight().Text("POWERED BY esavdoc").FontSize(7).FontColor(Colors.Grey.Medium);
                        });
                    });

                    // Código QR
                    row.ConstantItem(60).PaddingLeft(10).Height(60).Placeholder();
                });
            });
        }
    }
}
