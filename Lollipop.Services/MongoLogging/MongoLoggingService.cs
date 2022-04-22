using Lollipop.Helpers;
using Lollipop.Helpers.Extensions;
using Lollipop.Models.MongoLogging;
using Lollipop.Models.Requests.MongoLogging;
using Lollipop.Models.Responses.MongoLogging;
using Lollipop.Services.MongoLogging.Abstractions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lollipop.Services.MongoLogging
{
    public class MongoLoggingService : IMongoLoggingService
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly IMongoDatabase _database;

        public MongoLoggingService()
        {
            _connectionString = EnvironmentVariable.ConnectionString;
            _databaseName = EnvironmentVariable.Database;
            _database = GetDatabase();
        }

        private MongoClient BuildMongoClient()
        {
            var settings = MongoClientSettings.FromConnectionString(_connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);

            return client;
        }

        private IMongoDatabase GetDatabase() => BuildMongoClient().GetDatabase(_databaseName);

        private async Task<IMongoCollection<MongoLoggingModel>> GetCollection(string name)
        {
            if (!await _database.CollectionExists(name))
            {
                var options = new CreateCollectionOptions
                {
                    Capped = true,
                    MaxSize = AppSettings.MongoLogging.MaxCollectionSize,
                    MaxDocuments = AppSettings.MongoLogging.MaxDocuments
                };
                await _database.CreateCollectionAsync(name, options);
            }

            var collection = _database.GetCollection<MongoLoggingModel>(name);

            return collection;
        }

        public async Task<GetLogsResponse> GetLogs(GetLogsRequest request)
        {
            var collection = await GetCollection(request.CollectionName);

            var sort = new BsonDocument("$natural", -1);
            var filter = GetFilterExpression(request.PivotId, request.GetAfterPivotId);

            var findFluent = collection.Find(filter).Sort(sort).Limit(request.Limit);
            var logs = await findFluent.ToListAsync();
            logs.Reverse();

            var stats = GetCollectionStats(request.CollectionName);

            var response = new GetLogsResponse
            {
                Logs = logs,
                Count = stats["count"].ToInt32(),
                Max = stats["max"].ToInt32(),
                MaxSize = stats["maxSize"].ToInt64(),
                Size = stats["size"].ToInt64(),
            };

            return response;
        }

        private BsonDocument GetFilterExpression(string id, bool getAfterId)
        {
            BsonDocument filter;

            if (id.IsNullOrWhiteSpace())
            {
                filter = new BsonDocument();
            }
            else if (getAfterId)
            {
                filter = new BsonDocument("_id", new BsonDocument("$gt", id.ToObjectId()));
            }
            else filter = new BsonDocument("_id", new BsonDocument("$lt", id.ToObjectId()));

            return filter;
        }

        private BsonDocument GetCollectionStats(string collectionName)
        {
            var command = new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "collstats", collectionName } });
            var stats = _database.RunCommand(command);
            return stats;
        }

        public async Task DeleteCollection(string collectionName)
        {
            await _database.DropCollectionAsync(collectionName);
        }

        public async Task InsertLogs(InsertLogsRequest request)
        {
            await (await GetCollection(request.CollectionName)).InsertManyAsync(request.Logs);
        }

        public async Task<IEnumerable<string>> GetCollectionNames()
        {
            return await (await _database.ListCollectionNamesAsync()).ToListAsync();
        }
    }
}
