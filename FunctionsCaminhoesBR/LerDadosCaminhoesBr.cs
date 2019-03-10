using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionsCaminhoesBR
{
    public static class LerDadosCaminhoesBr
    {
        private static HttpClient client = new HttpClient();

        [FunctionName("LerDadosCaminhoesBr")]
        public static void Run([IoTHubTrigger("messages/events", Connection = "ConexaoCaminhoesBR")]EventData message, [Table("tblCaminhoesBR", Connection="AzureWebJobsStorage")]ICollector<Mensagem> tabela, ILogger log)
        {
            var UrlPowerBi = "https://api.powerbi.com/beta/b4d4d774-3cb5-45ae-b59f-ebd99c20a5ea/datasets/bdb9bbaa-e6ad-43b6-8f67-c2af7656e15d/rows?key=PdBKYxnPCALRiqytFv7DiIVNESCQkXF6GgdCkybl8g6Ah4J2pcdGGhN%2BN%2FPPp0ZUS6EBvgOpNJ6NOe9VdBRrPQ%3D%3D";
            client.PostAsync(UrlPowerBi, new StringContent(Encoding.UTF8.GetString(message.Body.Array), Encoding.UTF8, "application/json"));
            tabela.Add(new Mensagem(Encoding.UTF8.GetString(message.Body.Array)));    
            log.LogInformation($"C# IoT Hub trigger function processed a message: {Encoding.UTF8.GetString(message.Body.Array)}");                
        }
    }

    public class Mensagem: TableEntity
    {
        public Mensagem(string payLoad)
        {
            this.PartitionKey = "CaminhoesBR";
            this.RowKey = System.Guid.NewGuid().ToString();
            this.PayLoad = payLoad;
        }
        public string PayLoad { get; set; }
    }

}