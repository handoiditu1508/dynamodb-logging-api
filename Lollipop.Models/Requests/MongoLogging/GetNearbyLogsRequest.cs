using Lollipop.Models.MongoLogging;

namespace Lollipop.Models.Requests.MongoLogging
{
    public class GetNearbyLogsRequest : GetLogsRequest
    {
        public NearbyLogsFilterModel FilterModel { get; set; }
    }
}
