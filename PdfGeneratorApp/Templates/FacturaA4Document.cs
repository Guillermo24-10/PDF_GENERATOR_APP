using PdfGeneratorApp.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PdfGeneratorApp.Templates
{
    public class FacturaA4Document : IDocument
    {
        private readonly DocumentoRequest _factura;

        public FacturaA4Document(DocumentoRequest factura)
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

                // 1. CABECERA
                page.Header().Element(ComposeHeader);

                // 2. CONTENIDO

                page.Content().Element(ComposeContent);
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().AlignLeft().Text("LOGO DE LA EMPRESA.").FontSize(18).SemiBold().FontColor(Colors.Blue.Medium);
                });

                row.RelativeItem().PaddingHorizontal(10).Column(col =>
                {
                    col.Item().AlignCenter().Text("JDM TECNOLOGIA Y SOLUCIONES GLOBALES S.A.C.")
                        .FontSize(10).SemiBold().FontColor(Colors.Blue.Medium);

                    col.Item().AlignCenter().Text("AV. PZ SOLDAN NRO. 170 INT 205 II")
                        .FontSize(8).SemiBold().FontColor(Colors.Blue.Medium);

                    col.Item().AlignCenter().Text("LIMA - LIMA - SAN ISIDRO")
                        .FontSize(8).SemiBold().FontColor(Colors.Blue.Medium);
                });

                row.ConstantItem(160).Border(1).PaddingVertical(5).Column(col =>
                {
                    col.Item().AlignCenter().Text("R.U.C. 20606248971")
                        .FontSize(11).SemiBold().FontColor(Colors.Blue.Medium);

                    col.Item().AlignCenter().Text("FACTURA ELECTRÓNICA")
                        .FontSize(13).Bold();

                    col.Item().AlignCenter().Text($"{_factura.Serie} N° {_factura.Numero}")
                        .FontSize(11).SemiBold();
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingTop(20).Column(col =>
            {
                // 1. DATOS DEL CLIENTE + FECHAS
                col.Item().PaddingTop(8).Row(row =>
                {
                    //LADO IZQ: DATOS DEL RECEPTOR
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Row(r =>
                        {
                            r.ConstantItem(90).Text("SEÑOR(ES)").SemiBold();
                            r.RelativeItem().Text($": {_factura.Cliente.Nombre}");
                        });

                        c.Item().Row(r =>
                        {
                            r.ConstantItem(90).Text("RUC").SemiBold();
                            r.RelativeItem().Text($": {_factura.Cliente.Ruc}");
                        });

                        c.Item().Row(r =>
                        {
                            r.ConstantItem(90).Text("DIRECCIÓN").SemiBold();
                            r.RelativeItem().Text($": {_factura.Cliente.Direccion}");
                        });
                        c.Item().Row(r =>
                        {
                            r.ConstantItem(90).Text("MONEDA").SemiBold();
                            r.RelativeItem().Text($": SOLES");
                        });
                        c.Item().Row(r =>
                        {
                            r.ConstantItem(90).Text("FORMA PAGO").SemiBold();
                            r.RelativeItem().Text($": CRÉDITO");
                        });
                    });

                    // LADO DERECHO: FECHAS
                    row.ConstantItem(220).Column(c =>
                    {
                        c.Item().Row(r =>
                        {
                            r.ConstantItem(130).Text("FECHA EMISIÓN").SemiBold();
                            r.RelativeItem().Text($": {_factura.Fecha:dd/MM/yyyy}");
                        });
                        c.Item().Row(r =>
                        {
                            r.ConstantItem(130).Text("FECHA VENCIMIENTO").SemiBold();
                            r.RelativeItem().Text($": {_factura.Fecha:dd/MM/yyyy}");
                        });
                        c.Item().Row(r =>
                        {
                            r.ConstantItem(130).Text("FECHA PAGO").SemiBold();
                            r.RelativeItem().Text($": {_factura.Fecha:dd/MM/yyyy}");
                        });
                        c.Item().Row(r =>
                        {
                            r.ConstantItem(130).Text("MONTO NETO PEND.").SemiBold();
                            r.RelativeItem().Text($": 800.0000");
                        });
                    });
                });

                // 2. TABLA DE ITEMS

                col.Item().PaddingTop(10).Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.ConstantColumn(50); // CODIGO
                        cols.ConstantColumn(60); // CANTIDAD 
                        cols.ConstantColumn(60); // UNIDAD 
                        cols.RelativeColumn(); // DESCRIPCION 
                        cols.ConstantColumn(70); // PRECIO 
                        cols.ConstantColumn(70); // VALOR UNIT. 
                        cols.ConstantColumn(70); // IMPORTE. 
                    });

                    // ENCABEZADO
                    static IContainer HeaderCell(IContainer c) =>
                        c.Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Medium)
                        .PaddingVertical(4).PaddingHorizontal(4);

                    table.Header(h =>
                    {
                        h.Cell().Element(HeaderCell).AlignCenter().Text("Código").SemiBold();
                        h.Cell().Element(HeaderCell).AlignCenter().Text("Cantidad").SemiBold();
                        h.Cell().Element(HeaderCell).AlignCenter().Text("Unidad").SemiBold();
                        h.Cell().Element(HeaderCell).AlignCenter().Text("Descripción").SemiBold();
                        h.Cell().Element(HeaderCell).AlignCenter().Text("Precio").SemiBold();
                        h.Cell().Element(HeaderCell).AlignCenter().Text("Valor Unit.").SemiBold();
                        h.Cell().Element(HeaderCell).AlignCenter().Text("Importe").SemiBold();
                    });

                    // FILAS

                    static IContainer DataCell(IContainer c) =>
                        c.Border(1).BorderColor(Colors.Grey.Lighten1)
                            .PaddingVertical(4).PaddingHorizontal(4);

                    int i = 1;
                    foreach (var item in _factura.Items)
                    {
                        table.Cell().Element(DataCell).AlignCenter().Text($"{i}");
                        table.Cell().Element(DataCell).AlignRight().Text($"{item.Cantidad:F4}");
                        table.Cell().Element(DataCell).AlignCenter().Text("UND");
                        table.Cell().Element(DataCell).Text(item.Descripcion);
                        table.Cell().Element(DataCell).AlignRight().Text($"{item.PrecioUnitario:F4}");
                        table.Cell().Element(DataCell).AlignRight().Text($"{item.PrecioUnitario:F4}");
                        table.Cell().Element(DataCell).AlignRight().Text($"{item.Total:F4}");
                        i++;
                    }
                });

                // 3. TABLA DE CUOTAS
                col.Item().PaddingTop(150).AlignRight().Width(350).Table(table =>
                {
                    table.ColumnsDefinition(cols =>
                    {
                        cols.RelativeColumn(); //IDCuota
                        cols.RelativeColumn(); //MONTO
                        cols.RelativeColumn(); //FECHA DE PAGO
                    });

                    static IContainer CuotaHeader(IContainer c) =>
                        c.Background("#4a4a4a").BorderColor(Colors.Grey.Medium)
                            .PaddingVertical(4).PaddingHorizontal(6);

                    table.Header(h =>
                    {
                        h.Cell().Element(CuotaHeader).AlignCenter()
                            .Text("IDCuota").SemiBold().FontColor(Colors.White);
                        h.Cell().Element(CuotaHeader).AlignCenter()
                            .Text("Monto").SemiBold().FontColor(Colors.White);
                        h.Cell().Element(CuotaHeader).AlignCenter()
                            .Text("Fecha de pago").SemiBold().FontColor(Colors.White);
                    });

                    static IContainer CuotaCell(IContainer c) =>
                        c.Border(1).BorderColor(Colors.Grey.Lighten1)
                            .PaddingVertical(4).PaddingHorizontal(6);

                    var cuotas = new[]
                    {
                        new
                        {
                            Id = "Cuota001",
                            Monto = "500",
                            FechaPago = "17/04/2026"
                        },
                        new
                        {
                            Id = "Cuota002",
                            Monto = "600",
                            FechaPago = "16/04/2026"
                        },
                        new
                        {
                            Id = "Cuota003",
                            Monto = "400",
                            FechaPago = "15/04/2026"
                        }
                    };
                    foreach (var item in cuotas)
                    {
                        table.Cell().Element(CuotaCell).AlignCenter().Text(item.Id);
                        table.Cell().Element(CuotaCell).AlignCenter().Text($"S/ {item.Monto}");
                        table.Cell().Element(CuotaCell).AlignCenter().Text(item.FechaPago);
                    }
                });

                // LÍNEA SEPARADORA DESPUÉS DE CUOTAS
                col.Item().PaddingTop(8).BorderBottom(2).BorderColor(Colors.Black).Width(0);
                col.Item().PaddingTop(2).BorderBottom(1).BorderColor(Colors.Black).Width(0).Height(2);

                // 4. SON + TOTALES

                col.Item().PaddingTop(50).Row(row =>
                {
                    // IZQUIERDA: SON, BENEFICIARIO, CUENTAS, ETC.
                    row.RelativeItem().Column(c =>
                    {
                        c.Item().Text($"SON : {_factura.Total}").SemiBold();
                        c.Item().PaddingTop(4).Row(r =>
                        {
                            r.ConstantItem(90).Text("BENEFICIARIO :").SemiBold();
                            r.RelativeItem().Text("JDM TECNOLOGIA Y SOLUCIONES GLOBALES S.A.C.");
                        });
                        c.Item().PaddingTop(4).Text("CUENTAS:").SemiBold();
                        c.Item().PaddingTop(4).Text("COMENTARIOS LEGALES :").SemiBold();
                        c.Item().PaddingTop(4).Text("OBSERVACIONES :").SemiBold();
                    });

                    // Derecha: tabla de totales
                    row.ConstantItem(200).Table(table =>
                    {
                        table.ColumnsDefinition(cols =>
                        {
                            cols.RelativeColumn(3);
                            cols.ConstantColumn(22);   // "S/"
                            cols.ConstantColumn(75);   // monto
                        });

                        var bgNormal = "#d6cfa8"; // color beige/oliva del original
                        var bgDestacado = "#c8c090";


                        void TotalRow(TableDescriptor t, string label, decimal value,
                           bool destacado = false, bool conBorde = false)
                            {
                                var bg = destacado ? bgDestacado : bgNormal;

                                IContainer Cell(IContainer c) => conBorde
                                    ? c.Background(bg).Border(1).BorderColor(Colors.Black)
                                       .PaddingVertical(3).PaddingHorizontal(4)
                                    : c.Background(bg)
                                       .PaddingVertical(3).PaddingHorizontal(4);

                                t.Cell().Element(Cell).Text(label).SemiBold();
                                t.Cell().Element(Cell).AlignCenter().Text("S/").SemiBold();
                                t.Cell().Element(Cell).AlignRight().Text($"{value:F4}");
                            }

                        TotalRow(table, "OP. GRAVADA", _factura.OpGravada);
                        TotalRow(table, "OP. INAFECTA", _factura.OpInafecta);
                        TotalRow(table, "OP. EXONERADA", _factura.OpExonerada);
                        TotalRow(table, "OP. GRATUITA", _factura.OpGratuita);
                        TotalRow(table, "TOT. DSCTO", _factura.TotalDescuento);                        
                        TotalRow(table, "I.G.V 18%", _factura.Igv, destacado:true);                        
                        TotalRow(table, "IMPORTE TOTAL", _factura.Total,destacado:true,conBorde:true);                        
                    });
                });

                // LÍNEA SEPARADORA DESPUÉS DE SON + TOTALES
                col.Item().PaddingTop(8).BorderBottom(1).BorderColor(Colors.Black).Width(0);
                col.Item().PaddingTop(2).BorderBottom(2).BorderColor(Colors.Black).Width(0).Height(2);
            });
        }
    }
}
