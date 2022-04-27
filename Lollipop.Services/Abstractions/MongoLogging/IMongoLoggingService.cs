using Lollipop.Models.Requests.MongoLogging;
using Lollipop.Models.Responses.MongoLogging;

namespace Lollipop.Services.MongoLogging.Abstractions
{
    public interface IMongoLoggingService
    {
        Task<GetLogsResponse> GetOutermostLogs(GetOutermostLogsRequest request);
        Task<GetLogsResponse> GetNearbyLogs(GetNearbyLogsRequest request);
        Task DeleteCollection(string collectionName);
        Task InsertLogs(InsertLogsRequest request);
        Task InsertLog(InsertLogRequest request);
        Task<IEnumerable<string>> GetCollectionNames();
    }
}
