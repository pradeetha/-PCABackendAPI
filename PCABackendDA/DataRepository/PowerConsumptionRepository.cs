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
        IDeviceRepository _deviceRepository;
        public PowerConsumptionRepository(IDataConnection dataConnection, IDeviceRepository deviceRepository)
        {
            this._dataConnection = dataConnection;
            _mongoDatabase = _dataConnection.MongoDatabase();
            _deviceRepository = deviceRepository;
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

        #region GetConsumptionByDeviceId
        public List<PowerConsumptionInfo> GetConsumptionByDeviceId(int deviceId)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    IMongoCollection<PowerConsumptionInfo> consumptionInfoCollection = _mongoDatabase.GetCollection<PowerConsumptionInfo>("ConsumptionInfo");
                    FilterDefinition<PowerConsumptionInfo> filterObj = Builders<PowerConsumptionInfo>.Filter.
                                                         Where(x => x.DeviceId.Equals(deviceId));


                    var result = consumptionInfoCollection.Find(filterObj).ToList();
                    return result;
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }
        }
        #endregion

        #region GetConsumptionByUserProfileId
        public List<PowerConsumptionInfo> GetConsumptionByUserProfileId(int userProfileId)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    IMongoCollection<PowerConsumptionInfo> consumptionInfoCollection = _mongoDatabase.GetCollection<PowerConsumptionInfo>("ConsumptionInfo");
                    FilterDefinition<PowerConsumptionInfo> filterObj = Builders<PowerConsumptionInfo>.Filter.
                                                         Where(x => x.UserProfileId.Equals(userProfileId));


                    var result = consumptionInfoCollection.Find(filterObj).ToList();
                    return result;
                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }
        }

        public List<PowerConsumptionInfo> GetConsumptionForUserandDevice(int userProfileId, int deviceId)
        {
            using (TransactionScope scope1 = new TransactionScope())
            {
                try
                {
                    IMongoCollection<PowerConsumptionInfo> consumptionInfoCollection = _mongoDatabase.GetCollection<PowerConsumptionInfo>("ConsumptionInfo");
                    FilterDefinition<PowerConsumptionInfo> filterObj;
                    List<PowerConsumptionInfo> result = new List<PowerConsumptionInfo>();

                    if (deviceId == -99)
                    {
                        filterObj = Builders<PowerConsumptionInfo>.Filter
                            .Where(x => x.UserProfileId.Equals(userProfileId));

                        result = consumptionInfoCollection .Find(filterObj).ToList();
                    }
                    else
                    {
                        filterObj = Builders<PowerConsumptionInfo>.Filter
                            .Where(x => x.DeviceId.Equals(deviceId) && x.UserProfileId.Equals(userProfileId));

                        result = consumptionInfoCollection.Find(filterObj).ToList();
                    }

                        return result;

                }
                catch (Exception ex) { scope1.Dispose(); throw ex; }
            }
        }
        #endregion



    }
}
