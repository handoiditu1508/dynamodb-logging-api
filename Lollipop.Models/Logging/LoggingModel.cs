using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Lollipop.Models.Logging
{
    public class LoggingModel
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Tag { get; set; }
        public LogLevel LogLevel { get; set; }
        public object? Data { get; set; }
    }
}
