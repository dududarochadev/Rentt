using Rentt.Entities;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly FileService _fileService;

        public UserService(UserRepository userRepository, FileService fileService)
        {
            _userRepository = userRepository;
            _fileService = fileService;
        }

        public User? GetById(string id)
        {
            return _userRepository.GetById(id);
        }

        public User Create(User user)
        {
            if (user.IsDeliveryman)
            {
                ValidateDeliveryman(user);
            }

            return _userRepository.Create(user);
        }

        private void ValidateDeliveryman(User user)
        {
            if (string.IsNullOrEmpty(user.Cnpj))
            {
                throw new Exception("CNPJ é obrigatório.");
            }

            if (string.IsNullOrEmpty(user.DriverLicenseNumber))
            {
                throw new Exception("Número da CNH é obrigatório.");
            }

            if (user.DriverLicenseType is null)
            {
                throw new Exception("Tipo da CNH é obrigatório.");
            }

            if (_userRepository.GetByCnpj(user.Cnpj) is not null)
            {
                throw new Exception("CNPJ já cadastrado.");
            }

            if (_userRepository.GetByDriverLicenseNumber(user.DriverLicenseNumber) is not null)
            {
                throw new Exception("Número da CNH já cadastrado.");
            }
        }

        public string UpdateDriverLicenseImage(string id, IFormFile newDriverLicenseImage)
        {
            var user = _userRepository.GetById(id);

            if (user is null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            if (!user.IsDeliveryman)
            {
                throw new Exception("Usuário não é entregador.");
            }

            if (user.DriverLicenseImage is not null)
            {
                _fileService.DeleteFile(user.DriverLicenseImage);
            }

            ValidateImage(newDriverLicenseImage);

            var imageUrl = _fileService.SaveFile(id, newDriverLicenseImage);

            user.DriverLicenseImage = imageUrl;
            
            _userRepository.Update(user);

            return imageUrl;
        }

        private static void ValidateImage(IFormFile newDriverLicenseImage)
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

        public void Delete(string id)
        {
            var existingUser = _userRepository.GetById(id);

            if (existingUser is null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            _userRepository.Delete(id);
        }
    }
}
