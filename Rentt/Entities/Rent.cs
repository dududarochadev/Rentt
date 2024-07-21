using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rentt.Entities
{
    public class Rent
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("expectedEndDate")]
        public DateTime ExpectedEndDate { get; set; }

        [BsonElement("endDate")]
        public DateTime? EndDate { get; set; }

        [BsonElement("amount")]
        public double? Amount { get; set; }

        [BsonElement("user")]
        public required User User { get; set; }

        [BsonElement("motorcycle")]
        public required Motorcycle Motorcycle { get; set; }

        [BsonElement("rentalPlan")]
        public required RentalPlan RentalPlan { get; set; }
    }

    public class CreateRent
    {
        public required string UserId { get; set; }
        public required string RentalPlanId { get; set; }
        public required string MotorcycleId { get; set; }
    }
}
