using System;
namespace ParcnuAPI.CommonLib
{
    public class LHT65Decoder
    {

        private string _payload;
        public string Payload
        {
            get
            {
                return _payload;
            }
            set
            {
                _payload = value;
                DeCodePayload(_payload);
            }
        }

        private string BatS { get; set; }               // 2 bytes
        private string BuildInTempS { get; set; }        // 2 byte
        private string BuildInHumidityS { get; set; }    // 2 byte
        private string EXTS { get; set; }               // 1 byte optional 
        private string ExtTempS { get; set; }           // 4 bytes optional

        //Numeric values
        
        public LHT65BatteryType Bat { get; set; }
        public double BuildInTemp { get; set; }
        public double BuildInHumidity { get; set; }
        public double ExtTemp { get; set; }

        public LHT65Decoder()
        {
        }

        public LHT65Decoder(string payload)
        {
            Payload = payload;
        }

        private void DeCodePayload(string payload)
        {
            ParcnuUtilities parcnuUtil = new ParcnuUtilities();
            LHT65BatteryType bat = new LHT65BatteryType();
            int strLen = payload.Length;

            BatS = payload.Substring(0, 4); // 2 byte 0 - 3
            bat = parcnuUtil.CalulateLHT65Bat(BatS);
            Bat = bat;

            BuildInTempS = payload.Substring(4, 4); //2  bytes 4 - 7
            BuildInTemp = parcnuUtil.CalculateLHT65Temperature(BuildInTempS);
            BuildInHumidityS = payload.Substring(8, 4); //2 bytes 8 - 11
            BuildInHumidity = parcnuUtil.CalculateLHT65BuildInHumidity(BuildInHumidityS);
            EXTS = payload.Substring(12, 2); // 1 byte 12 - 13
            
            int Ext = parcnuUtil.CalculateLHT65Ext(EXTS);


            switch (Ext)
            {
                case ParcnuAPI.CommonLib.ParcnuEnums.LHTEXTSensor.TemperatureSensor:
                    ExtTempS = payload.Substring(14, 4);    //2 bytes 14 - 17 bytes 18-21 are not used.
                    ExtTemp = parcnuUtil.CalculateLHT65Temperature(ExtTempS);
                    break;
                default:
                    break;
            }
            

            
            
        }

    }
}
