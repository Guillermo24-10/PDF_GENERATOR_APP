using Newtonsoft.Json;
using PdfGeneratorApp.Model;
using PdfGeneratorApp.Templates;
using QuestPDF.Companion;
using QuestPDF.Fluent;

namespace PdfGeneratorApp.Services
{
    public class PdfService
    {
        public void Generar(string json)
        {
            var data = JsonConvert.DeserializeObject<DocumentoRequest>(json);

            Directory.CreateDirectory("Output");

            var file = $"Output/{data!.Serie}-{data.Numero}.pdf";

            if (data.TipoDocumento == "FACTURA")
            {
                var doc = new FacturaA4_Diseño2_Document(data);
                doc.GeneratePdf(file);
                doc.ShowInCompanion();
            }

            Console.WriteLine($"PDF generado: {file}");
        }
    }
}
