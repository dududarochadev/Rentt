using Rentt.Entities;
using Rentt.Models;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class DeliverymanService : IDeliverymanService
    {
        private readonly IDeliverymanRepository _deliverymanRepository;
        private readonly IFileService _fileService;

        public DeliverymanService(
            IDeliverymanRepository deliverymanRepository,
            IFileService fileService)
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

        public ResultRentt Create(CreateDeliverymanModel newDeliveryman, string userId)
        {
            var deliveryman = new Deliveryman
            {
                UserId = userId,
                Cnpj = newDeliveryman.Cnpj,
                DateOfBirth = newDeliveryman.DateOfBirth,
                DriverLicenseNumber = newDeliveryman.DriverLicenseNumber,
                DriverLicenseType = newDeliveryman.DriverLicenseType
            };

            _deliverymanRepository.Create(deliveryman);

            return new ResultRentt
            {
                Success = true,
                Object = deliveryman
            };
        }

        public ResultRentt ValidateDeliveryman(CreateDeliverymanModel newDeliveryman)
        {
            if (_deliverymanRepository.GetByCnpj(newDeliveryman.Cnpj) is not null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "CNPJ já cadastrado."
                };
            }

            if (_deliverymanRepository.GetByDriverLicenseNumber(newDeliveryman.DriverLicenseNumber) is not null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Número da CNH já cadastrado."
                };
            }

            return new ResultRentt
            {
                Success = true
            };
        }

        public ResultRentt UpdateDriverLicenseImage(Deliveryman deliveryman, IFormFile newDriverLicenseImage)
        {
            if (deliveryman.DriverLicenseImage is not null)
            {
                _fileService.DeleteFile(deliveryman.DriverLicenseImage);
            }

            var resultValidate = ValidateImage(newDriverLicenseImage);

            if (!resultValidate.Success)
            {
                return resultValidate;
            }

            var imageUrl = _fileService.SaveFile(deliveryman.Id, newDriverLicenseImage);

            deliveryman.DriverLicenseImage = imageUrl;

            _deliverymanRepository.Update(deliveryman);

            return new ResultRentt
            {
                Success = true,
                Object = imageUrl
            };
        }

        private ResultRentt ValidateImage(IFormFile newDriverLicenseImage)
        {
            var fileExtension = Path.GetExtension(newDriverLicenseImage.FileName).ToLowerInvariant();
            var mimeType = newDriverLicenseImage.ContentType.ToLowerInvariant();

            var allowedExtensions = new[] { ".png", ".bmp" };
            var allowedMimeTypes = new[] { "image/png", "image/bmp" };

            if (!allowedExtensions.Contains(fileExtension))
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Formato de imagem não suportado. Apenas PNG e BMP são permitidos."
                };
            }

            if (!allowedMimeTypes.Contains(mimeType))
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Tipo MIME não suportado. Apenas imagens PNG e BMP são permitidas."
                };
            }

            return new ResultRentt
            {
                Success = true
            };
        }
    }
}
