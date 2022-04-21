using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Lollipop.Models.MongoLogging
{
    public class MongoLoggingModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? Tag { get; set; }
        public LogLevel LogLevel { get; set; }
        public ErrorData Data { get; set; }
    }
}
