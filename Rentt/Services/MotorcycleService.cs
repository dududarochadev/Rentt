using MassTransit;
using Rentt.Entities;
using Rentt.Events;
using Rentt.Models;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IBus _bus;

        public MotorcycleService(IMotorcycleRepository motorcycleRepository, IBus bus)
        {
            _motorcycleRepository = motorcycleRepository;
            _bus = bus;
        }

        public IEnumerable<Motorcycle> Get(string? licensePlate)
        {
            return _motorcycleRepository.Get(licensePlate);
        }

        public Motorcycle? GetById(string id)
        {
            return _motorcycleRepository.GetById(id);
        }

        public ResultRentt Create(CreateMotorcycleModel newMotorcycle)
        {
            if (_motorcycleRepository.GetByLicensePlate(newMotorcycle.LicensePlate) is not null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Placa já cadastrada."
                };
            }

            var motorcycle = new Motorcycle
            {
                Year = newMotorcycle.Year,
                Model = newMotorcycle.Model,
                LicensePlate = newMotorcycle.LicensePlate
            };

            _motorcycleRepository.Create(motorcycle);

            var eventRequest = new MotorcycleCreatedEvent
            {
                MotorcycleId = motorcycle.Id,
                MotorcycleYear = motorcycle.Year
            };

            _bus.Publish(eventRequest).Wait();

            return new ResultRentt
            {
                Success = true,
                Object = motorcycle
            };
        }

        public ResultRentt UpdateLicensePlate(string id, string newLicensePlate)
        {
            var motorcycle = _motorcycleRepository.GetById(id);

            if (motorcycle is null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Moto não encontrada."
                };
            }

            if (_motorcycleRepository.GetByLicensePlate(newLicensePlate) is not null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Placa já cadastrada."
                };
            }

            motorcycle.LicensePlate = newLicensePlate;
            _motorcycleRepository.Update(motorcycle);

            return new ResultRentt
            {
                Success = true,
                Object = motorcycle
            };
        }

        public ResultRentt Delete(string id)
        {
            if (_motorcycleRepository.HasRent(id))
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Moto possui uma ou mais locações."
                };
            }

            _motorcycleRepository.Delete(id);

            return new ResultRentt
            {
                Success = true
            };
        }
    }
}
