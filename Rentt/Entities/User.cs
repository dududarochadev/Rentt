using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Rentt.Enums;

namespace Rentt.Entities
{
    public class User
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; } = default!;

        [BsonElement("password")]
        public string Password { get; set; } = default!;

        [BsonElement("isAdmin")]
        public bool IsAdmin { get; set; }

        [BsonElement("isDeliveryman")]
        public bool IsDeliveryman { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("cnpj")]
        public string? Cnpj { get; set; }

        [BsonElement("dateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        [BsonElement("driverLicenseNumber")]
        public string? DriverLicenseNumber { get; set; }

        [BsonElement("driverLicenseType")]
        public DriverLicenseType? DriverLicenseType { get; set; }

        [BsonElement("driverLicenseImage")]
        public string? DriverLicenseImage { get; set; }
    }
}
