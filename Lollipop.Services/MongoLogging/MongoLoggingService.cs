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

        private async Task<IMongoCollection<LoggingModel>> GetCollection(string name)
        {
            IMongoCollection<LoggingModel> collection;

            if (!await _database.CollectionExists(name))
            {
                var options = new CreateCollectionOptions
                {
                    Capped = true,
                    MaxSize = AppSettings.MongoLogging.MaxCollectionSize,
                    MaxDocuments = AppSettings.MongoLogging.MaxDocuments
                };
                await _database.CreateCollectionAsync(name, options);

                collection = _database.GetCollection<LoggingModel>(name);

                await CreateIndexes(collection);
            }
            else collection = _database.GetCollection<LoggingModel>(name);

            return collection;
        }

        private async Task<IEnumerable<string>> CreateIndexes(IMongoCollection<LoggingModel> collection)
        {
            var builder = Builders<LoggingModel>.IndexKeys;
            var indexModels = new List<CreateIndexModel<LoggingModel>>
            {
                new CreateIndexModel<LoggingModel>(builder.Ascending(x => x.CreatedDate)),
                new CreateIndexModel<LoggingModel>(builder.Ascending(x => x.LogLevel)),
                new CreateIndexModel<LoggingModel>(builder.Ascending(x => x.Group))
            };
            return await collection.Indexes.CreateManyAsync(indexModels);
        }

        public async Task<GetLogsResponse> GetOutermostLogs(GetOutermostLogsRequest request)
        {
            if (request.CollectionName.IsNullOrWhiteSpace())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.CollectionName));

            if (request.FilterModel == null)
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.FilterModel));

            var collection = await GetCollection(request.CollectionName);

            var sort = new BsonDocument("$natural", request.FilterModel.Latest ? -1 : 1);
            var filter = request.FilterModel.BuildBsonDocument();

            var findFluent = collection.Find(filter).Sort(sort).Limit(request.Limit);
            var logs = await findFluent.ToListAsync();
            if (request.FilterModel.Latest)
                logs.Reverse();

            var response = BuildLogsResponse(request.CollectionName, logs);

            return response;
        }

        public async Task<GetLogsResponse> GetNearbyLogs(GetNearbyLogsRequest request)
        {
            if (request.CollectionName.IsNullOrWhiteSpace())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.CollectionName));

            if (request.FilterModel == null)
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.FilterModel));

            var collection = await GetCollection(request.CollectionName);

            var sort = new BsonDocument("$natural", request.FilterModel.GetAfterPivotId ? 1 : -1);
            var filter = request.FilterModel.BuildBsonDocument();

            var findFluent = collection.Find(filter).Sort(sort).Limit(request.Limit);
            var logs = await findFluent.ToListAsync();
            if (!request.FilterModel.GetAfterPivotId)
                logs.Reverse();

            var response = BuildLogsResponse(request.CollectionName, logs);

            return response;
        }

        private GetLogsResponse BuildLogsResponse(string collectionName, IEnumerable<LoggingModel> logs)
        {
            var stats = GetCollectionStats(collectionName);

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

        private BsonDocument GetCollectionStats(string collectionName)
        {
            var command = new BsonDocumentCommand<BsonDocument>(new BsonDocument { { "collstats", collectionName } });
            var stats = _database.RunCommand(command);
            return stats;
        }

        public async Task DeleteCollection(string collectionName)
        {
            if (collectionName.IsNullOrWhiteSpace())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(collectionName));

            await _database.DropCollectionAsync(collectionName);
        }

        public async Task InsertLogs(InsertLogsRequest request)
        {
            if (request.CollectionName.IsNullOrWhiteSpace())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.CollectionName));

            if (request.Logs.IsNullOrEmpty())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.Logs));

            foreach (var log in request.Logs)
            {
                log.CreatedDate = DateTime.UtcNow;
            }

            await (await GetCollection(request.CollectionName)).InsertManyAsync(request.Logs);
        }

        public async Task InsertLog(InsertLogRequest request)
        {
            if (request.CollectionName.IsNullOrWhiteSpace())
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.CollectionName));

            if (request.Log == null)
                throw CustomException.Validation.PropertyIsNullOrEmpty(nameof(request.Log));

            request.Log.CreatedDate = DateTime.UtcNow;

            await (await GetCollection(request.CollectionName)).InsertOneAsync(request.Log);
        }

        public async Task<IEnumerable<string>> GetCollectionNames()
        {
            return await (await _database.ListCollectionNamesAsync()).ToListAsync();
        }
    }
}
