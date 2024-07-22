using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rentt.Enums;

namespace Rentt.Entities
{
    public class Deliveryman
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("cnpj")]
        public string Cnpj { get; set; }

        [BsonElement("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("driverLicenseNumber")]
        public string DriverLicenseNumber { get; set; }

        [BsonElement("driverLicenseType")]
        public DriverLicenseType DriverLicenseType { get; set; }

        [BsonElement("driverLicenseImage")]
        public string? DriverLicenseImage { get; set; }
    }
}
