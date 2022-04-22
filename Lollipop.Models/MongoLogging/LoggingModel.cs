using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lollipop.Models.MongoLogging
{
    public class LoggingModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Group { get; set; }
        public string Code { get; set; }
    }
}
