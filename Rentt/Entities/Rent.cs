using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rentt.Entities
{
    public class Rent
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("expectedEndDate")]
        public DateTime ExpectedEndDate { get; set; }

        [BsonElement("endDate")]
        public DateTime? EndDate { get; set; }

        [BsonElement("amount")]
        public double? Amount { get; set; }

        [BsonElement("deliverymanId")]
        public required string DeliverymanId { get; set; }

        [BsonElement("motorcycleId")]
        public required string MotorcycleId { get; set; }

        [BsonElement("rentalPlanId")]
        public required string RentalPlanId { get; set; }
    }
}
