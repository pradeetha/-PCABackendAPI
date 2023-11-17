using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendBL.APIEntity
{
    public class DateRangeConsumptionServiceModel
    {
        public int UserProfileId { get; set; }
        public int DeviceId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
