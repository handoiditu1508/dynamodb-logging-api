using Lollipop.Models.MongoLogging;

namespace Lollipop.Models.Requests.MongoLogging
{
    public class InsertLogsRequest
    {
        public string CollectionName { get; set; }
        public IEnumerable<LoggingModel> Logs { get; set; }
    }
}
