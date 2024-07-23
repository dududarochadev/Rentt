using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rentt.Entities
{
    public class Event
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
