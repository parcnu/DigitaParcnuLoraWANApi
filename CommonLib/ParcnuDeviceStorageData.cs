using System;
using System.Globalization;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace ParcnuAPI.CommonLib
{
    public class ParcnuDeviceStorageData : TableEntity
    {
        public DigitaPayload DigitaPayload { get; set; }
        public string DevEUI { get; set; }
        public string DeviceType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double InternalTemp { get; set; }
        public double ExternalTemp { get; set; }
        public double InternalHumidity { get; set; }
        public int Alarm { get; set; }
        public double Battery { get; set; }
        public string DevicePayload { get; set; }
        public string DigitaStringPayload { get; set; }
        


        public ParcnuDeviceStorageData()
        {
        }

        public ParcnuDeviceStorageData(string partitionKey, DigitaPayload payload)
        {
            Latitude = 0;
            Longitude = 0;
            InternalTemp = 0;
            ExternalTemp = 0;
            Alarm = 0;
            Battery = 0;

            PartitionKey = partitionKey;
            RowKey = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff");
            //RowKey = payload.DevEUI_uplink.FCntUp;
            DigitaPayload = payload;
            DigitaStringPayload = JsonConvert.SerializeObject(payload).ToString();
            DevEUI = payload.DevEUI_uplink.DevEUI;
            DeviceType = GetDeviceType(payload.DevEUI_uplink.DevEUI);
            
            DevicePayload = payload.DevEUI_uplink.payload_hex;
            if (DeviceType == ParcnuAPI.CommonLib.ParcnuEnums.DeviceType.LGT92)
            {
                LGT92Decoder decoder = new LGT92Decoder(DevicePayload);
                Longitude = decoder.Longitude;
                Latitude = decoder.Latitude;
                Battery = decoder.Bat;
                Alarm = decoder.Alarm;
            }
            else if (DeviceType == ParcnuAPI.CommonLib.ParcnuEnums.DeviceType.LHT65_E1)
            {
                //Longitude = payload.DevEUI_uplink.DevLON;
                LHT65Decoder decoder = new LHT65Decoder(DevicePayload);

                Battery = decoder.Bat.BatteryValue;
                InternalTemp = decoder.BuildInTemp;
                InternalHumidity = decoder.BuildInHumidity;
                ExternalTemp = decoder.ExtTemp;
                double coordinate;
                bool validNum = double.TryParse(payload.DevEUI_uplink.LrrLAT, NumberStyles.Any, CultureInfo.InvariantCulture, out coordinate);
                Latitude = coordinate;
                validNum = double.TryParse(payload.DevEUI_uplink.LrrLON, NumberStyles.Any, CultureInfo.InvariantCulture, out coordinate);
                Longitude = coordinate;
            }
        }

        public string GetDeviceType(string devEUI)
        {
            //get from payload DevEUI which is used to find out DviceType from devices TableStorage.
            //Now hardcoded to GPS
            if (devEUI == "A84041BCD1827272")
                return ParcnuAPI.CommonLib.ParcnuEnums.DeviceType.LHT65_E1;
            else if (devEUI == "A84041000181E9C8")
                return ParcnuAPI.CommonLib.ParcnuEnums.DeviceType.LGT92;
            else return "";
        }
    }
}
