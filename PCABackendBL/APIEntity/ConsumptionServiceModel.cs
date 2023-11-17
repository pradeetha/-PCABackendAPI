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
        public string LogTimestamp { get; set; }//Date in "yyyy-MM-ddThh:mm:ss.fffffffZ" format (Ex:-"2023-11-17T06:34:47.8721234Z")
    }
}
