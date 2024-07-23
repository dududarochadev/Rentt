using Rentt.Entities;

namespace Rentt.Repositories
{
    public interface IRentRepository
    {
        Rent? GetById(string id);
        IEnumerable<Rent> GetByMotorcycleId(string motorcycleId);
        Rent Create(Rent rent);
    }
}
