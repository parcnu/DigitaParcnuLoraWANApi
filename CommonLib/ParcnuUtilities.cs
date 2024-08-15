using System;
namespace ParcnuAPI.CommonLib
{
    public class ParcnuUtilities
    {
        public ParcnuUtilities()
        {
        }

        private double HexStringToDouble(string hexstring)
        {

            int intValue = int.Parse(hexstring, System.Globalization.NumberStyles.HexNumber);
            double floatValue = Convert.ToDouble(intValue);
            return floatValue;
        }

        public double CalculateCoordinate(string lat)
        {
            double nbr = HexStringToDouble(lat);
            int inbr = Convert.ToInt32(nbr);
            return nbr / 1000000;
        }

        public int CalculateAlarm(string al)
        {
            int alarmmask = 0X40;
            double nbr = HexStringToDouble(al);
            int inbr = Convert.ToInt32(nbr);
            int alarm = inbr & alarmmask;
            return alarm;
        }

        public double CalulateBat(string bat)
        {
            int batmask = 0X3FFF; // Alarm bit will be masked away.
            double nbr = HexStringToDouble(bat);
            int inbr = Convert.ToInt32(nbr);
            double maskedbat = inbr & batmask;
            return maskedbat/1000;
        }

        /*public double CalculateLongitude(string lng)
        {
            uint mask = 0X80000000;
            
            double nbr = HexStringToDouble(lng);
            int inbr = Convert.ToInt32(nbr);
            var result = inbr & mask;
           
            return nbr / 1000000;
            
        }*/

        /* Conversion from Hex to Integer
         *
         */
        public int ToInteger(string c)
        {


            if (c == "00") return 0;
            else if (c == "01") return 1;
            else if (c == "02") return 2;
            else if (c == "03") return 3;
            else if (c == "04") return 4;
            else if (c == "05") return 5;
            else if (c == "06") return 6;
            else if (c == "07") return 7;
            else if (c == "08") return 8;
            else if (c == "09") return 9;
            else if (c == "0A") return 10;
            else if (c == "0B") return 11;
            else if (c == "0C") return 12;
            else if (c == "0D") return 13;
            else if (c == "0E") return 14;
            else if (c == "0F") return 15;
            else return -1;
        }

        public LHT65BatteryType CalulateLHT65Bat(string battery)
        {
            LHT65BatteryType BatType = new LHT65BatteryType();
            uint batmasktype = 0Xc000;  //two hight bits masked out
            uint batmaskvalue = 0X3FFF;

            double bat = HexStringToDouble(battery);
            int ibat = Convert.ToInt32(bat);
            double maskedbatinfo = ibat & batmasktype; //masking out two hights bytes
            double maskedbatvalue = ibat & batmaskvalue; // masking two lower bits from highest byte and masking out rest 3 lower bytes.
            BatType.BatteryValue = maskedbatvalue / 1000;
            switch (maskedbatinfo)
            {
                case ParcnuAPI.CommonLib.ParcnuEnums.LHT65BatteryInfo.UltraLow:
                    BatType.BatteryInfo = "Ultra Low";
                    break;
                case ParcnuAPI.CommonLib.ParcnuEnums.LHT65BatteryInfo.Low:
                    BatType.BatteryInfo = "Low";
                    break;
                case ParcnuAPI.CommonLib.ParcnuEnums.LHT65BatteryInfo.Ok:
                    BatType.BatteryInfo = "Ok";
                    break;
                case ParcnuAPI.CommonLib.ParcnuEnums.LHT65BatteryInfo.Good:
                    BatType.BatteryInfo = "Good";
                    break;
                default:
                    BatType.BatteryInfo = "NOK";
                    break;
            }

            return BatType;

        }

        public double CalculateLHT65Temperature(string BuildInTemp) //Works bot internal and external sensors
        {
            int substractor = 65536; //00001 0000 0000 0000 0000
            int mask = 0X8000; // 1000 0000 0000 0000 Cheking if number is negative.
            double buildInTemp = HexStringToDouble(BuildInTemp);
            int ibuildInTemp = Convert.ToInt32(buildInTemp);
            double posOrNeg = ibuildInTemp & mask;
            if (posOrNeg == mask)                       // negative temp
            {
                return (buildInTemp - substractor) / 100;
            }
            else                                        // positive temp
            {
                return buildInTemp / 100;
            }

        }

        public double CalculateLHT65BuildInHumidity(string BuildInHumidity)
        {
            return HexStringToDouble(BuildInHumidity) / 10;
        }

        public int CalculateLHT65Ext(string ext)
        {
             return Int32.Parse(ext);
        }

        /*public double CalculateLHT65ExtTemperature(string temp)
        {
            
            double exttemp = HexStringToDouble(temp);
            return exttemp / 100;

        }*/


    }
}
