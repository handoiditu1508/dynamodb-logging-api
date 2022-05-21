using Lollipop.Models.MongoLogging;

namespace Lollipop.Models.Requests.MongoLogging
{
    public class InsertLogRequest
    {
        public string CollectionName { get; set; }
        public LoggingModel Log { get; set; }
    }
}
