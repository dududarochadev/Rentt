using Rentt.Entities;
using Rentt.Models;

namespace Rentt.Services
{
    public interface IRentService
    {
        Rent? GetById(string id);
        IEnumerable<Rent> GetByMotorcycleId(string motorcycleId);
        ResultRentt Create(CreateRentModel createRent, User user);
        ResultRentt CalculateTotalRentalCost(string rentId);
    }
}
