namespace Rentt.Models
{
    public class CreateMotorcycleModel
    {
        public int Year { get; set; }
        public required string Model { get; set; }
        public required string LicensePlate { get; set; }
    }
}
