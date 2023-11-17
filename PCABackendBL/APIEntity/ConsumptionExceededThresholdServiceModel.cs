using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.APIEntity
{
    public class ConsumptionExceededThresholdServiceModel
    {
        public int UserProfileId { get; set; }
        public string UserCode { get; set; }
        public int DeviceId { get; set; }
        public string DeviceSerialKey { get; set; }
        public string ApplianceName { get; set; }
        public string InternalLocation { get; set; }
        public float TotalPowerConsumption { get; set; }
        public float ThresholdValue { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
