using PCABackendDA.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendDA.DataRepository.Interfaces
{
    public interface IPowerConsumptionRepository
    {
        PowerConsumptionInfo InsertConsumption(PowerConsumptionInfo powerConsumption);
        List<PowerConsumptionInfo> GetConsumptionBySerialKey(string serialKey);
        List<PowerConsumptionInfo> GetConsumptionByDeviceId(int deviceId);
        List<PowerConsumptionInfo> GetConsumptionByUserProfileId(int userProfileId);
    }
}
