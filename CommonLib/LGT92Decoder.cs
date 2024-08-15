using System;
using System.Text;
using ParcnuAPI.CommonLib;

namespace ParcnuAPI.CommonLib
{
    public class LGT92Decoder
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

        private string LatitudeS { get; set; }  //4 byte
        private string LongitudeS { get; set; } //4 byte
        private string AlarmS { get; set; } // 1 byte
        private string BatS { get; set; }   // 1 byte
        private string FlagS { get; set; }  // 1 byte
        private string RollS { get; set; }  // 2 byte optional
        private string PitchS { get; set; } // 2 byte optional 
        private string HDOPS { get; set;  } // 1 byte optional
        private string AltitudeS { get; set; } //2 byte optional

        private double _latitude { get; set; }
        private double _longitude { get; set; }

        //Numeric values
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public double Bat { get; set; }
        public int Alarm { get; set; }
        public double Roll { get; set; }
        public double Pitch { get; set; }

        

        public LGT92Decoder()
        {

        }

        public LGT92Decoder(string payload)
        {
            Payload = payload;
            
        }

        private void DeCodePayload(string payload)
        {
            ParcnuUtilities parcnuUtil = new ParcnuUtilities();
            int strLen = payload.Length;
            LatitudeS = payload.Substring(0, 8); //4 first bytes
            Latitude = parcnuUtil.CalculateCoordinate(LatitudeS);
            LongitudeS = payload.Substring(8, 8); //4 bytes
            Longitude = parcnuUtil.CalculateCoordinate(LongitudeS);
            AlarmS = payload.Substring(16, 2); //1 byte
            Alarm = parcnuUtil.CalculateAlarm(AlarmS);
            BatS = payload.Substring(16, 4); // 2 byte / Contains also alarm which is masked away.
            Bat = parcnuUtil.CalulateBat(BatS);
            FlagS = payload.Substring(20, 2); //1 byte
            if (strLen > 22)
            {
                RollS = payload.Substring(22, 4); //2 bytes optional
                //Roll = parcnuUtil.HexStringToDouble(RollS);
                PitchS = payload.Substring(26, 4); //2 bytes optional
                //Pitch = parcnuUtil.HexStringToDouble(PitchS);
                HDOPS = payload.Substring(30, 2); //1 bytes optional
                AltitudeS = payload.Substring(32, 4); //2 bytes optional
                //Altitude = parcnuUtil.HexStringToDouble(AltitudeS);
            }
        }

    }
}
