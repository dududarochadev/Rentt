using Rentt.Entities;
using Rentt.Models;

namespace Rentt.Services
{
    public interface IMotorcycleService
    {
        IEnumerable<Motorcycle> Get(string? licensePlate);
        Motorcycle? GetById(string id);
        Motorcycle Create(CreateMotorcycleModel newMotorcycle);
        void UpdateLicensePlate(string id, string newLicensePlate);
        void Delete(string id);
    }
}
