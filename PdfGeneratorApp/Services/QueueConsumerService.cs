using Azure.Storage.Queues;

namespace PdfGeneratorApp.Services
{
    public class QueueConsumerService
    {
        private readonly string conn = @"AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

        private readonly string queueName = "cola-pdf";

        public async Task<string?> LeerMensaje()
        {
            var queue = new QueueClient(conn, queueName, new QueueClientOptions(version:QueueClientOptions.ServiceVersion.V2019_12_12));
            var msg = await queue.ReceiveMessageAsync();

            if (msg.Value == null)
                return null;

            string contenido = msg.Value.MessageText;

            await queue.DeleteMessageAsync(
                msg.Value.MessageId,
                msg.Value.PopReceipt
            );

            return contenido;
        }
    }
}
