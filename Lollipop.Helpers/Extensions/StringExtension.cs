using MongoDB.Bson;

namespace Lollipop.Helpers.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static ObjectId ToObjectId(this string id) => new ObjectId(id);
    }
}
