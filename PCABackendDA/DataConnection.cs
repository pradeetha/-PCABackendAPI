using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace PCABackendDA
{

    public class DataConnection : IDataConnection
    {
        private string _connectionString;
        private string _databaseName;
        public string DeviceCollectionName;
        public IMongoDatabase _mongoDatabaseRunTime;
        MongoClient _mongoClient;
        private IConfiguration _configuration;

        public DataConnection(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IMongoDatabase MongoDatabase()
        {
            _connectionString = _configuration["Values:MongoDBURL"];
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(_connectionString);
            _mongoClient = new MongoClient(settings);
            _databaseName = _configuration["Values:MongoDBDatabase"];
            //Set the Database
            _mongoDatabaseRunTime = _mongoClient.GetDatabase(_databaseName);
            return _mongoDatabaseRunTime;
        }
    }
}
