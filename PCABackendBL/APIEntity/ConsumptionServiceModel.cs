using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.APIEntity
{
    public class ConsumptionServiceModel
    {
        public string DeviceSerialKey { get; set; }
        public float ConsumedUnits { get; set; }
        public string LogTimestamp { get; set; }//Date in "yyyy-MM-dd'T'HH:mm:ss'Z" format (Ex:-"2013-09-29T18:46:19Z")
    }
}
