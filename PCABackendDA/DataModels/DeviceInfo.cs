using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendDA.DataModels
{
    public class DeviceInfo
    {
        public ObjectId _id { get; set; }
        public int DeviceId { get; set; }
        public int UserProfileId { get; set; }
        public string UserCode { get; set; }
        public string DeviceSerialKey { get; set; }
        public string DeviceType { get; set; }
        public string ApplianceName { get; set; }
        public string InternalLocation { get; set; }
        public string Address { get; set; }
        public float PowerThresholdValue { get; set; }
        public string CreatedDate { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss.fffffffZ");
        public DateTime lastModified { get; set; }
    }
}
