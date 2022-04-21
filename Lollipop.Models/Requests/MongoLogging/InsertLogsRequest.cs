using Lollipop.Models.MongoLogging;

namespace Lollipop.Models.Requests.MongoLogging
{
    public class InsertLogsRequest
    {
        public IEnumerable<MongoLoggingModel> Logs { get; set; }
        public string CollectionName { get; set; }
    }
}
