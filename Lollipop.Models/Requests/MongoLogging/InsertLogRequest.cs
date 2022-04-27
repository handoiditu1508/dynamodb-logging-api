using Lollipop.Models.MongoLogging;

namespace Lollipop.Models.Requests.MongoLogging
{
    public class InsertLogRequest
    {
        public LoggingModel Log { get; set; }
        public string CollectionName { get; set; }
    }
}
