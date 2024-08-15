using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParcnuAPI.CommonLib;
using CloudTable = Microsoft.Azure.Cosmos.Table.CloudTable;
using TableOperation = Microsoft.Azure.Cosmos.Table.TableOperation;
using DynamicTableEntity = Microsoft.Azure.Cosmos.Table.DynamicTableEntity;
using TableResult = Microsoft.Azure.Cosmos.Table.TableResult;
using Microsoft.Azure.Cosmos.Table;

namespace ParcnuAPI.AddDevice
{
    public static class AddDevice
    {
        [FunctionName(ParcnuEnums.FunctionName.AddDevice)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Table(ParcnuEnums.DatabaseName.Devices, Connection = "AzureWebJobsStorage")] IAsyncCollector<ParcnuDeviceDataDS> devicedataTableCollector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function DeviceAPI processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ParcnuDeviceDataDS data = JsonConvert.DeserializeObject<ParcnuDeviceDataDS>(requestBody);
            ParcnuDeviceDataDS customer = new ParcnuDeviceDataDS(data.PartitionKey, data.RowKey, data.DevEUI, data.OperatorCustomerID, data.DeviceType, data.DeviceProvider);
            string dbtablekey = System.Environment.GetEnvironmentVariable("DEVICESDBTABLEKEY", EnvironmentVariableTarget.Process);
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials("parcnustorageaccount", dbtablekey), true);
            Microsoft.Azure.Cosmos.Table.CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(ParcnuEnums.DatabaseName.Devices);
            table.CreateIfNotExists();
            //
            TableOperation op1 = TableOperation.Retrieve<DynamicTableEntity>(data.PartitionKey, data.RowKey);
            TableResult result = table.Execute(op1);
            if (result.Result != null)
            {
                return new BadRequestObjectResult("Entity alreayd exits " + data.PartitionKey + " " + data.RowKey);
            }
            else
            {

                var insertOper = TableOperation.Insert(customer);
                var resp = table.Execute(insertOper);
                if (resp.Result != null)
                {
                    return new OkObjectResult("Entity added to Table " + data.PartitionKey + " " + data.RowKey + " " + data.DevEUI +" " + data.OperatorCustomerID +" "+ data.DeviceType);
                }
                else
                {
                    return new BadRequestObjectResult("Adding entity failed " + data.PartitionKey + " " + data.RowKey);
                }
            }
        }
    }
    
}

