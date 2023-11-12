using MongoDB.Driver;
using PCABackendDA.DataModels;
using PCABackendDA.DataRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PCABackendDA.DataRepository
{
    public class PowerConsumptionRepository : IPowerConsumptionRepository
    {
        private readonly IMongoDatabase _mongoDatabase;
        IDataConnection _dataConnection;
        public PowerConsumptionRepository(IDataConnection dataConnection)
        {
            this._dataConnection = dataConnection;
            _mongoDatabase = _dataConnection.MongoDatabase();
        }

        public PowerConsumptionInfo InsertConsumption(PowerConsumptionInfo powerConsumption)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    var consumptionInfoCollection = _mongoDatabase.GetCollection<PowerConsumptionInfo>("ConsumptionInfo");
                    consumptionInfoCollection.InsertOneAsync(powerConsumption);
                    scope1.Complete();
                    return powerConsumption;
                }
                catch (Exception ex)
                {
                    scope1.Dispose();
                    throw ex;
                }
            }

        }

        #region GetConsumptionBySerialKey
        public List<PowerConsumptionInfo> GetConsumptionBySerialKey(string serialKey)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    IMongoCollection<PowerConsumptionInfo> consumptionInfoCollection = _mongoDatabase.GetCollection<PowerConsumptionInfo>("ConsumptionInfo");
                    FilterDefinition<PowerConsumptionInfo> filterObj = Builders<PowerConsumptionInfo>.Filter.
                                                         Where(x => x.DeviceSerialKey.Equals(serialKey));


                    var result = consumptionInfoCollection.Find(filterObj).ToList();
                    return result;
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }
        }
        #endregion

    }
}
