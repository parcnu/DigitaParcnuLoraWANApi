using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using CloudTable = Microsoft.Azure.Cosmos.Table.CloudTable;
using TableOperation = Microsoft.Azure.Cosmos.Table.TableOperation;
using DynamicTableEntity = Microsoft.Azure.Cosmos.Table.DynamicTableEntity;
using TableResult = Microsoft.Azure.Cosmos.Table.TableResult;
using ParcnuAPI.CommonLib;

namespace ParcnuAPI.DataAPI
{
    public static class DataAPI
    {
        [FunctionName(ParcnuAPI.CommonLib.ParcnuEnums.FunctionName.DataAPI)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Table(ParcnuEnums.DatabaseName.DeviceData, Connection = "AzureWebJobsStorage")] IAsyncCollector<ParcnuCustomerDataDS> deviceDataTableCollector,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function DataAPI processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            DigitaPayload data = JsonConvert.DeserializeObject<DigitaPayload>(requestBody);
            log.LogInformation("data" + data.ToString());
            
            ParcnuDeviceStorageData payloaddata = new ParcnuDeviceStorageData("Parcnu", data);
            string dbtablekey = System.Environment.GetEnvironmentVariable("DATATBTABLEKEY", EnvironmentVariableTarget.Process);
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials("parcnustorageaccount", dbtablekey), true);
            Microsoft.Azure.Cosmos.Table.CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(ParcnuEnums.DatabaseName.DeviceData);
            table.CreateIfNotExists();
            //
            TableOperation op1 = TableOperation.Retrieve<DynamicTableEntity>(payloaddata.PartitionKey, payloaddata.RowKey);
            TableResult result = table.Execute(op1);
            if (result.Result != null)
            {
                return new BadRequestObjectResult("Entity alreayd exits " + payloaddata.DigitaStringPayload);
            }
            else
            {

                var insertOper = TableOperation.Insert(payloaddata);
                var resp = table.Execute(insertOper);
                if (resp.Result != null)
                {
                    return new OkObjectResult("Entity added to Table " + payloaddata.PartitionKey + " " + payloaddata.RowKey + " " + payloaddata.DigitaStringPayload);
                }
                else
                {
                    return new BadRequestObjectResult("Adding entity failed " + payloaddata.PartitionKey + " " + payloaddata.RowKey + " " + payloaddata.DigitaStringPayload);
                }
            }
        }
    }
}
