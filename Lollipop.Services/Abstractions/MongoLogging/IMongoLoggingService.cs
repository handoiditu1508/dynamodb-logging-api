using Lollipop.Models.Requests.MongoLogging;
using Lollipop.Models.Responses.MongoLogging;

namespace Lollipop.Services.MongoLogging.Abstractions
{
    public interface IMongoLoggingService
    {
        Task<GetLogsResponse> GetLogs(GetLogsRequest request);
        Task DeleteCollection(string collectionName);
        Task InsertLogs(InsertLogsRequest request);
        Task<IEnumerable<string>> GetCollectionNames();
    }
}
