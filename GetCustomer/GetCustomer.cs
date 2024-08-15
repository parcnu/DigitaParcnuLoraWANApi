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



//test
namespace ParcnuAPI.GetCustomer
{
    public static class AddCustomer
    {
        [FunctionName("GetCustomer")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetCustomer")] HttpRequest req,
            [Table("customers", Connection = "AzureWebJobsStorage")] IAsyncCollector<ParcnuCustomerDataDS> customerTableCollector,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request. Adding new customer ");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ParcnuCustomerDataDS data = JsonConvert.DeserializeObject<ParcnuCustomerDataDS>(requestBody);
            ParcnuCustomerDataDS customer = new ParcnuCustomerDataDS(data.PartitionKey, data.RowKey, data.CustomerEmail);
            string dbtablekey = System.Environment.GetEnvironmentVariable("CUSTOMERSDBTABLEKEY", EnvironmentVariableTarget.Process);
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials("parcnustorageaccount", dbtablekey), true);
            Microsoft.Azure.Cosmos.Table.CloudTableClient client = storageAccount.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("customers");
            //table.CreateIfNotExists();
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
                    return new OkObjectResult("Entity added to Table " + data.PartitionKey + " " + data.RowKey + " " + data.CustomerEmail);
                }
                else
                {
                    return new BadRequestObjectResult("Adding entity failed " + data.PartitionKey + " " + data.RowKey);
                }
            }
        }
    }
}