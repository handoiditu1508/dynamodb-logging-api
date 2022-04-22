namespace Lollipop.Models.Requests.MongoLogging
{
    public class GetLogsRequest
    {
        public string CollectionName { get; set; }
        public int Limit { get; set; } = 10;
    }
}
