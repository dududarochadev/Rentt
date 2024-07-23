using Rentt.Entities;
using Rentt.Models;

namespace Rentt.Services
{
    public interface IMotorcycleService
    {
        IEnumerable<Motorcycle> Get(string? licensePlate);
        Motorcycle? GetById(string id);
        ResultRentt Create(CreateMotorcycleModel newMotorcycle);
        ResultRentt UpdateLicensePlate(string id, string newLicensePlate);
        ResultRentt Delete(string id);
    }
}
