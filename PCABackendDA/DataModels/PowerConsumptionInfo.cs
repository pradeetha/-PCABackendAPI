using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendDA.DataModels
{
    public class PowerConsumptionInfo
    {
        public ObjectId _id { get; set; }
        public int DeviceId { get; set; }
        public string DeviceSerialKey { get; set; }
        public int UserProfileId { get; set; }
        public string UserCode { get; set; }
        public string ApplianceName { get; set; }
        public float ConsumedUnits { get; set; }
        public string LogTimestamp { get; set; }//Date in "yyyy-MM-dd'T'HH:mm:ss'Z" format (Ex:-"2013-09-29T18:46:19Z")
        public DateTime lastModified { get; set; }
    }
}
