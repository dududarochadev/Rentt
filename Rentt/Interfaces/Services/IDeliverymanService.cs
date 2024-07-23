using Rentt.Entities;
using Rentt.Models;

namespace Rentt.Services
{
    public interface IDeliverymanService
    {
        Deliveryman? GetById(string id);
        Deliveryman? GetByUserId(string userId);
        Deliveryman Create(CreateDeliverymanModel newDeliveryman, string userId);
        void ValidateDeliveryman(CreateDeliverymanModel newDeliveryman);
        string UpdateDriverLicenseImage(Deliveryman deliveryman, IFormFile newDriverLicenseImage);
    }
}
