using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.APIEntity
{
    public class DeviceInfoServiceInsertModel
    {
        public int DeviceId { get; set; }
        public int UserProfileId { get; set; }
        public string DeviceSerialKey { get; set; }
        public string DeviceType { get; set; }
        public string ApplianceName { get; set; }
        public string InternalLocation { get; set; }
        public string Address { get; set; }
        public float PowerThresholdValue { get; set; } 

    }
}
