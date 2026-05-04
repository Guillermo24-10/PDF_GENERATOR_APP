using PdfGeneratorApp.Services;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var queue = new QueueConsumerService();
var blob = new BlobReaderService();
var pdf = new PdfService();

Console.WriteLine("Escuchando cola...");

while (true)
{
    var fileName = await queue.LeerMensaje();

    if (!string.IsNullOrEmpty(fileName))
    {
        Console.WriteLine($"Procesando: {fileName}");

        var json = await blob.DescargarJson(fileName);

        pdf.Generar(json);
    }

    await Task.Delay(3000);
}