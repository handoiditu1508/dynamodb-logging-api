using MongoDB.Bson;
using MongoDB.Driver;

namespace Lollipop.Helpers.Extensions
{
    public static class MongoDatabaseExtension
    {
        public static async Task<bool> CollectionExists(this IMongoDatabase database, string name)
        {
            var filter = new BsonDocument("name", name);
            //filter by collection name
            var collections = await database.ListCollectionNamesAsync(new ListCollectionNamesOptions { Filter = filter });
            //check for existence
            return await collections.AnyAsync();
        }
    }
}
