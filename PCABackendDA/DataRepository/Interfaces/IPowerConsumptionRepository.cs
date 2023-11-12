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
        PowerConsumptionInfo GetConsumptionBySerialKey(string serialKey);
    }
}
