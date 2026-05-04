namespace PdfGeneratorApp.Model
{
    public class DocumentoRequest
    {
        public string TipoDocumento { get; set; } = String.Empty;
        public string Serie { get; set; } = String.Empty;
        public string Numero { get; set; } = String.Empty;
        public string Fecha { get; set; } = String.Empty;
        public Cliente Cliente { get; set; } = new Cliente();
        public List<Item> Items { get; set; } = new List<Item>();
        public decimal OpGravada { get; set; } = 762.7119M;
        public decimal OpInafecta { get; set; } = 0M;
        public decimal OpGratuita { get; set; } = 0M;
        public decimal OpExonerada { get; set; } = 0M;
        public decimal TotalDescuento { get; set; } = 0M;
        public decimal Igv { get; set; } = 137.2881M;
        public decimal Total { get; set; } 
    }
    public class Cliente
    {
        public string Nombre { get; set; } = String.Empty;
        public string Ruc { get; set; } = String.Empty;
        public string Direccion { get; set; } = String.Empty;
    }

    public class Item
    {
        public string Descripcion { get; set; } = String.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
