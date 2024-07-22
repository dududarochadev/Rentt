using Rentt.Entities;
using Rentt.Models;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class DeliverymanService
    {
        private readonly DeliverymanRepository _deliverymanRepository;
        private readonly FileService _fileService;

        public DeliverymanService(DeliverymanRepository deliverymanRepository,
            FileService fileService)
        {
            _deliverymanRepository = deliverymanRepository;
            _fileService = fileService;
        }

        public Deliveryman? GetById(string id)
        {
            return _deliverymanRepository.GetById(id);
        }

        public Deliveryman? GetByUserId(string userId)
        {
            return _deliverymanRepository.GetByUserId(userId);
        }

        public Deliveryman Create(CreateDeliverymanModel newDeliveryman, string userId)
        {
            var deliveryman = new Deliveryman
            {
                UserId = userId,
                Cnpj = newDeliveryman.Cnpj,
                DateOfBirth = newDeliveryman.DateOfBirth,
                DriverLicenseNumber = newDeliveryman.DriverLicenseNumber,
                DriverLicenseType = newDeliveryman.DriverLicenseType
            };

            return _deliverymanRepository.Create(deliveryman);
        }

        public void ValidateDeliveryman(CreateDeliverymanModel newDeliveryman)
        {
            if (_deliverymanRepository.GetByCnpj(newDeliveryman.Cnpj) is not null)
            {
                throw new Exception("CNPJ já cadastrado.");
            }

            if (_deliverymanRepository.GetByDriverLicenseNumber(newDeliveryman.DriverLicenseNumber) is not null)
            {
                throw new Exception("Número da CNH já cadastrado.");
            }
        }

        public string UpdateDriverLicenseImage(Deliveryman deliveryman, IFormFile newDriverLicenseImage)
        {
            if (deliveryman.DriverLicenseImage is not null)
            {
                _fileService.DeleteFile(deliveryman.DriverLicenseImage);
            }

            ValidateImage(newDriverLicenseImage);

            var imageUrl = _fileService.SaveFile(deliveryman.Id, newDriverLicenseImage);

            deliveryman.DriverLicenseImage = imageUrl;

            _deliverymanRepository.Update(deliveryman);

            return imageUrl;
        }

        private void ValidateImage(IFormFile newDriverLicenseImage)
        {
            var fileExtension = Path.GetExtension(newDriverLicenseImage.FileName).ToLowerInvariant();
            var mimeType = newDriverLicenseImage.ContentType.ToLowerInvariant();

            var allowedExtensions = new[] { ".png", ".bmp" };
            var allowedMimeTypes = new[] { "image/png", "image/bmp" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Formato de imagem não suportado. Apenas PNG e BMP são permitidos.");
            }

            if (!allowedMimeTypes.Contains(mimeType))
            {
                throw new Exception("Tipo MIME não suportado. Apenas imagens PNG e BMP são permitidas.");
            }
        }
    }
}
