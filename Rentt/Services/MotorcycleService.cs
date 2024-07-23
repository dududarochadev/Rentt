using Rentt.Entities;
using Rentt.Models;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public IEnumerable<Motorcycle> Get(string? licensePlate)
        {
            return _motorcycleRepository.Get(licensePlate);
        }

        public Motorcycle? GetById(string id)
        {
            return _motorcycleRepository.GetById(id);
        }

        public Motorcycle Create(CreateMotorcycleModel newMotorcycle)
        {
            if (_motorcycleRepository.GetByLicensePlate(newMotorcycle.LicensePlate) is not null)
            {
                throw new Exception("Placa já cadastrada.");
            }

            var motorcycle = new Motorcycle
            {
                Year = newMotorcycle.Year,
                Model = newMotorcycle.Model,
                LicensePlate = newMotorcycle.LicensePlate
            };

            return _motorcycleRepository.Create(motorcycle);
        }

        public void UpdateLicensePlate(string id, string newLicensePlate)
        {
            var motorcycle = _motorcycleRepository.GetById(id);

            if (motorcycle is null)
            {
                throw new Exception("Moto não encontrada.");
            }

            if (_motorcycleRepository.GetByLicensePlate(newLicensePlate) is not null)
            {
                throw new Exception("Placa já cadastrada.");
            }

            motorcycle.LicensePlate = newLicensePlate;
            _motorcycleRepository.Update(motorcycle);
        }

        public void Delete(string id)
        {
            if (_motorcycleRepository.HasRent(id))
            {
                throw new Exception("Moto possui uma ou mais locações.");
            }

            _motorcycleRepository.Delete(id);
        }
    }
}
