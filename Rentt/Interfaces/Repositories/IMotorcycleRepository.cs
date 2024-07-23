using Rentt.Entities;

namespace Rentt.Repositories
{
    public interface IMotorcycleRepository
    {
        IEnumerable<Motorcycle> Get(string? licensePlate);
        Motorcycle? GetById(string id);
        Motorcycle? GetByLicensePlate(string licensePlate);
        bool HasRent(string id);
        Motorcycle Create(Motorcycle motorcycle);
        void Update(Motorcycle motorcycle);
        void Delete(string id);
    }
}
