using Azure.Storage.Blobs;

namespace PdfGeneratorApp.Services
{
    public class BlobReaderService
    {
        private readonly string conn = @"AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

        private readonly string container = "entrada";

        public async Task<string> DescargarJson(string fileName)
        {
            var client = new BlobContainerClient(conn, container,new BlobClientOptions(version:BlobClientOptions.ServiceVersion.V2019_12_12));
            var blob = client.GetBlobClient(fileName);

            var response = await blob.DownloadContentAsync();
            return response.Value.Content.ToString();
        }
    }
}
