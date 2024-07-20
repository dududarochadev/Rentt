using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rentt.Entities
{
    public class Motorcycle
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("model")]
        public string Model { get; set; } = default!;

        [BsonElement("licensePlate")]
        public string LicensePlate { get; set; } = default!;
    }
}
