using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.APIEntity
{
    public class ConsumptionIndividualDetailsServiceModel
    {
        public int DeviceId { get; set; }
        public string DeviceSerialKey { get; set; }
        public float ConsumedUnits { get; set; }
        public string LogTimestamp { get; set; }
        public int UserProfileId { get; set; }
        public string UserCode { get; set; }
        public string ApplianceName { get; set; }
        public string InternalLocation { get; set; }
        public float ThresholdValue { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
