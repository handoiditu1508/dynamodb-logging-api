using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Lollipop.Models.MongoLogging
{
    public class LogsFilterModel
    {
        public string? Group { get; set; }
        public IEnumerable<LogLevel>? LogLevels { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual BsonDocument BuildBsonDocument()
        {
            var bson = new BsonDocument();

            if (!string.IsNullOrWhiteSpace(Group))
            {
                bson.Add(nameof(LoggingModel.Group), Group);
            }

            if (LogLevels != null && LogLevels.Any())
            {
                bson.Add(nameof(LoggingModel.LogLevel), new BsonDocument("$in", new BsonArray(LogLevels)));
            }

            if (StartDate.HasValue || EndDate.HasValue)
            {
                var createdDateFilter = new BsonDocument();
                if (StartDate.HasValue)
                {
                    createdDateFilter.Add("$gte", new BsonDateTime(StartDate.Value.Date));
                }
                if (EndDate.HasValue)
                {
                    createdDateFilter.Add("$lt", new BsonDateTime(EndDate.Value.Date.AddDays(1)));
                }
                bson.Add(nameof(LoggingModel.CreatedDate), createdDateFilter);
            }

            return bson;
        }
    }
}
