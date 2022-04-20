using MongoDB.Bson;
using MongoDB.Driver;

namespace Lollipop.Models.Extensions
{
    public static class MongoDatabaseExtension
    {
        public static async Task<bool> CollectionExists(this IMongoDatabase database, string name)
        {
            var filter = new BsonDocument("name", name);
            //filter by collection name
            var collections = await database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
            //check for existence
            return await collections.AnyAsync();
        }
    }
}
