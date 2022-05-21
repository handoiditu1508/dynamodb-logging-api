using MongoDB.Bson;

namespace Lollipop.Models.MongoLogging
{
    public class NearbyLogsFilterModel : LogsFilterModel
    {
        public string? PivotId { get; set; }
        public bool GetAfterPivotId { get; set; } = true;

        public override BsonDocument BuildBsonDocument()
        {
            var bson = base.BuildBsonDocument();

            if (!string.IsNullOrWhiteSpace(PivotId))
            {
                if (GetAfterPivotId)
                {
                    bson.Add("_id", new BsonDocument("$gt", ObjectId.Parse(PivotId)));
                }
                else bson.Add("_id", new BsonDocument("$lt", ObjectId.Parse(PivotId)));
            }

            return bson;
        }
    }
}
