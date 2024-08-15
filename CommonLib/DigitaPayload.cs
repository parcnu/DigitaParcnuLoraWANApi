using System;
using System.Collections.Generic;

namespace ParcnuAPI.CommonLib
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Lrr
    {
        public string Lrrid { get; set; }
        public string Chain { get; set; }
        public string LrrRSSI { get; set; }
        public string LrrSNR { get; set; }
        public string LrrESP { get; set; }
    }

    public class Lrrs
    {
        public List<Lrr> Lrr { get; set; }
    }

    public class Alr
    {
        public string pro { get; set; }
        public string ver { get; set; }
    }

    public class CustomerData
    {
        public Alr alr { get; set; }
    }

    public class DevEUIUplink
    {
        public DateTime Time { get; set; }
        public string DevEUI { get; set; }
        public string FPort { get; set; }
        public string FCntUp { get; set; }
        public string ADRbit { get; set; }
        public string MType { get; set; }
        public string FCntDn { get; set; }
        public string payload_hex { get; set; }
        public string mic_hex { get; set; }
        public string Lrcid { get; set; }
        public string LrrRSSI { get; set; }
        public string LrrSNR { get; set; }
        public string SpFact { get; set; }
        public string SubBand { get; set; }
        public string Channel { get; set; }
        public string DevLrrCnt { get; set; }
        public string Lrrid { get; set; }
        public string Late { get; set; }
        public string LrrLAT { get; set; }
        public string LrrLON { get; set; }
        public Lrrs Lrrs { get; set; }
        public DateTime DevLocTime { get; set; }
        public string DevLAT { get; set; }
        public string DevLON { get; set; }
        public string DevAlt { get; set; }
        public string DevLocRadius { get; set; }
        public string DevAltRadius { get; set; }
        public string DevUlFCntUpUsed { get; set; }
        public string DevLocDilution { get; set; }
        public string DevAltDilution { get; set; }
        public string DevNorthVel { get; set; }
        public string DevEastVel { get; set; }
        public string NwGeolocAlgo { get; set; }
        public string NwGeolocAlgoUsed { get; set; }
        public string CustomerID { get; set; }
        public CustomerData CustomerData { get; set; }
        public string ModelCfg { get; set; }
        public string InstantPER { get; set; }
        public string MeanPER { get; set; }
        public string DevAddr { get; set; }
        public string TxPower { get; set; }
        public string NbTrans { get; set; }
        public string Frequency { get; set; }
        public string DynamicClass { get; set; }
    }

    public class DigitaPayload
    {
        public DevEUIUplink DevEUI_uplink { get; set; }
    }
}

