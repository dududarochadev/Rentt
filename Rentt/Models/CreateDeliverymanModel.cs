using Rentt.Enums;

namespace Rentt.Models
{
    public class CreateDeliverymanModel
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public required string Cnpj { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public required string DriverLicenseNumber { get; set; }
        public required DriverLicenseType DriverLicenseType { get; set; }
    }
}
