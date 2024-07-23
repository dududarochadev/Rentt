using Rentt.Entities;
using Rentt.Models;

namespace Rentt.Services
{
    public interface IDeliverymanService
    {
        Deliveryman? GetById(string id);
        Deliveryman? GetByUserId(string userId);
        ResultRentt Create(CreateDeliverymanModel newDeliveryman, string userId);
        ResultRentt ValidateDeliveryman(CreateDeliverymanModel newDeliveryman);
        ResultRentt UpdateDriverLicenseImage(Deliveryman deliveryman, IFormFile newDriverLicenseImage);
    }
}
