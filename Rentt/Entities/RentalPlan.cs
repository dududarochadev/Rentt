using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rentt.Entities
{
    public class RentalPlan
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("days")]
        public int Days { get; set; }

        [BsonElement("priceByDay")]
        public double PriceByDay { get; set; }
    }
}
