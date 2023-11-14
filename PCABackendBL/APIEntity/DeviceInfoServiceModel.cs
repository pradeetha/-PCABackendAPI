using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.APIEntity
{
    public class DeviceInfoServiceModel
    {
        public int DeviceId { get; set; }
        public string DeviceSerialKey { get; set; }
        public string DeviceType { get; set; }
        public string ApplianceName { get; set; }
        public string InternalLocation { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public float PowerThresholdValue { get; set; }
        public string CreatedDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss");
        public DateTime lastModified { get; set; }

    }
}
