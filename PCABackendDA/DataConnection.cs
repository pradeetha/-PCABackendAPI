using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCABackendDA
{

    public class DataConnection : IDataConnection
    {
        private string ConnectionString;
        private string DatabaseName;
        public string DeviceCollectionName;
        public IMongoDatabase _mongoDatabaseRunTime;
        MongoClient _mongoClient;

        public IMongoDatabase MongoDatabase()
        {
            ConnectionString = Environment.GetEnvironmentVariable("MongoDBURL");
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(ConnectionString);
            _mongoClient = new MongoClient(settings);
            DatabaseName = Environment.GetEnvironmentVariable("MongoDBDatabase");
            //Set the Database
            _mongoDatabaseRunTime = _mongoClient.GetDatabase(DatabaseName);
            return _mongoDatabaseRunTime;
        }
    }
}
