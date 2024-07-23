using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rentt.Entities
{
    public class Motorcycle
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("identificador")]
        public int Identificador { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("model")]
        public required string Model { get; set; }

        [BsonElement("licensePlate")]
        public required string LicensePlate { get; set; }
    }
}
