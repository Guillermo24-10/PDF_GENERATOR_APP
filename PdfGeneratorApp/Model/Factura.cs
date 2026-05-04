namespace PdfGeneratorApp.Model
{
    public class Factura
    {
        public string Numero { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
