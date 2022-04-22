using Lollipop.Models.MongoLogging;

namespace Lollipop.Models.Requests.MongoLogging
{
    public class GetOutermostLogsRequest : GetLogsRequest
    {
        public OutermostLogsFilterModel FilterModel { get; set; }
    }
}
