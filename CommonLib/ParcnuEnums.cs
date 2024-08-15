using System;
namespace ParcnuAPI.CommonLib
{
    public static class ParcnuEnums
    {
        public static class DeviceType
        {
            public const string LGT92 = "1";
            public const string LHT65_E1 = "2";
            public const string LHT65_NO = "3";
        }

        public static class DeviceProvider
        {
            public const string Dragino = "Dragino";
        }

        public static class FunctionName
        {
            public const string AddCustomer = "AddCustomer";
            public const string GetCustomer = "GetCustomer";
            public const string AddDevice = "AddDevice";
            public const string GetDevice = "GetDevce";
            public const string CommonLib = "CommonLib";
            public const string DataAPI = "DataAPI";

        }

        public static class DatabaseName
        {
            public const string Customers = "customers";
            public const string Devices = "devices";
            public const string DeviceData = "devicedata";
        }

        public static class LHT65BatteryInfo
        {
            public const int UltraLow = 0X0000; //two highets bytes are 00
            public const int Low = 0X4000;      //two highest bytes are 01
            public const int Ok = 0X8000;       //two highest bytes are 10
            public const int Good = 0XC000;     //two highest bytes are 11
        }

        public static class LHTEXTSensor
        {
            public const int TemperatureSensor = 1;
            public const int InterruptSensor = 4;
            public const int IlluminationSensor = 5;
            public const int ADCSensor = 6;
            public const int CountingSensor16Bit = 7;
            public const int CountingSensor32bit = 8;
            public const int TemperatureSensorDataLogMod = 9;
        }
    }

    
}
