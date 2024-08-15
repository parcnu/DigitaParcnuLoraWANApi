using System;
using Microsoft.Azure.Cosmos.Table;

namespace ParcnuAPI.CommonLib
{
    public class ParcnuDeviceDataDS: TableEntity
    {
        
        public string DeviceID { get; set; }
        public string DevEUI { get; set; }
        public string DeviceType { get; set; } //ParcnuEnums.DeviceType
        public string OperatorCustomerID { get; set; }
        public string DeviceProvider { get; set; }
        //methods for handlign the related data.

        public ParcnuDeviceDataDS()
        {
        }

        public ParcnuDeviceDataDS( string partitionKey, string rowKey, string devEUI, string operatorCustomerID, string deviceType, string deviceProvider)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            DevEUI = devEUI;
            OperatorCustomerID = operatorCustomerID;
            DeviceType = deviceType;
            DeviceProvider = deviceProvider;
        }
    }
}
