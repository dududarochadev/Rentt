using Rentt.Entities;
using Rentt.Models;

namespace Rentt.Services
{
    public interface IRentService
    {
        Rent? GetById(string id);
        IEnumerable<Rent> GetByMotorcycleId(string motorcycleId);
        Rent Create(CreateRentModel createRent, User user);
        double CalculateTotalRentalCost(string rentId);
    }
}
