using Rentt.Entities;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class MotorcycleService
    {
        private readonly MotorcycleRepository _motorcycleRepository;

        public MotorcycleService(MotorcycleRepository motorcycleRepository)
        {
            _motorcycleRepository = motorcycleRepository;
        }

        public IEnumerable<Motorcycle> GetAll()
        {
            return _motorcycleRepository.Get();
        }

        public Motorcycle? GetById(string id)
        {
            return _motorcycleRepository.GetById(id);
        }

        public Motorcycle Create(Motorcycle motorcycle)
        {
            if (_motorcycleRepository.GetByLicensePlate(motorcycle.LicensePlate) != null)
            {
                throw new Exception("Placa já cadastrada.");
            }

            return _motorcycleRepository.Create(motorcycle);
        }

        public void Update(string id, string newLicensePlate)
        {
            var motorcycle = _motorcycleRepository.GetById(id);

            if (motorcycle == null)
            {
                throw new Exception("Moto não encontrada.");
            }

            if (_motorcycleRepository.GetByLicensePlate(newLicensePlate) != null)
            {
                throw new Exception("Placa já cadastrada.");
            }

            motorcycle.LicensePlate = newLicensePlate;
            _motorcycleRepository.Update(motorcycle);
        }

        public void Delete(string id)
        {
            _motorcycleRepository.Delete(id);
        }
    }
}
