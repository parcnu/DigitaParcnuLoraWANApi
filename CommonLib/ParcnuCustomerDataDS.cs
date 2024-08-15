using System;
using Microsoft.Azure.Cosmos.Table;

namespace ParcnuAPI.CommonLib
{
    public class ParcnuCustomerDataDS : TableEntity
    {

        private string _customerEmail;

        //public string PartitionKey { get; set; } //customerName
        //public string RowKey { get; set; }       //CustomID
        //       public string CustomerName { get; set; }
        public string CustomerEmail
        {
            get
            {
                return this._customerEmail;
            }
            set
            {
                if (validateEmail(value))
                {
                    this._customerEmail = value;
                }
                else
                {
                    this._customerEmail = null;
                }
            }
        }

        public ParcnuCustomerDataDS(string partitionKey, string rowKey, string customerEmail)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
            this.CustomerEmail = customerEmail;
        }
        public ParcnuCustomerDataDS()
        {
            //default constructor
        }

        public bool validateEmail(string email)
        {
            return true;
        }
    }
}