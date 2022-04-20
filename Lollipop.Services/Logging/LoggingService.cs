using Lollipop.Models.Extensions;
using Lollipop.Models.Logging;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Lollipop.Services.Logging
{
    public class LoggingService
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        public LoggingService()
        {
            _connectionString = "mongodb+srv://admin:admin123456@mycluster.h1t4z.mongodb.net/myFirstDatabase?retryWrites=true&w=majority";// get from env
            _databaseName = "MyLogs";// get from env
        }

        private MongoClient BuildMongoClient()
        {
            var settings = MongoClientSettings.FromConnectionString(_connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);

            return client;
        }

        private IMongoDatabase GetDatabase() => BuildMongoClient().GetDatabase(_databaseName);

        private async Task<IMongoCollection<LoggingModel>> GetCollection(string name)
        {
            var database = GetDatabase();
            if (!await database.CollectionExists(name))
            {
                var options = new CreateCollectionOptions
                {
                    Capped = true,
                    MaxSize = 104857600,// get from app settings
                    MaxDocuments = 1000// get from app settings
                };
                await database.CreateCollectionAsync(name, options);
            }

            var collection = database.GetCollection<LoggingModel>(name);

            return collection;
        }

        public async Task Log(string collectionName, LoggingModel document)
        {
            var collection = await GetCollection(collectionName);

            await collection.InsertOneAsync(document);
        }

        public async Task<IEnumerable<LoggingModel>> Get(string collectionName, int limit = 10, string id = null, bool getAfterId = false)
        {
            var collection = await GetCollection(collectionName);

            var sort = Builders<LoggingModel>.Sort.Descending("$natural");
            var filter = GetFilterExpression(id, getAfterId);

            var findFluent = collection.Find(filter).Sort(sort).Limit(limit);

            return await findFluent.ToListAsync();
        }

        private Expression<Func<LoggingModel, bool>> GetFilterExpression(string id, bool getAfterId)
        {
            Expression<Func<LoggingModel, bool>> filter;

            if (id.IsNullOrWhiteSpace())
            {
                filter = d => true;
            }
            else if (getAfterId)
            {
                filter = d => d.Id > id.ToObjectId();
            }
            else filter = d => d.Id < id.ToObjectId();

            return filter;
        }
    }
}
