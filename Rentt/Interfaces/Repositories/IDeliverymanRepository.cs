using Rentt.Entities;

namespace Rentt.Repositories
{
    public interface IDeliverymanRepository
    {
        Deliveryman? GetById(string id);
        Deliveryman? GetByUserId(string userId);
        Deliveryman? GetByCnpj(string cnpj);
        Deliveryman? GetByDriverLicenseNumber(string driverLicenseNumber);
        Deliveryman Create(Deliveryman deliveryman);
        void Update(Deliveryman deliveryman);
        void Delete(string id);
    }
}
