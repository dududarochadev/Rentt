﻿using Rentt.Entities;
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

        public IEnumerable<Motorcycle> Get(string? licensePlate)
        {
            return _motorcycleRepository.Get(licensePlate);
        }

        public Motorcycle? GetById(string id)
        {
            return _motorcycleRepository.GetById(id);
        }

        public Motorcycle Create(Motorcycle motorcycle)
        {
            if (_motorcycleRepository.GetByLicensePlate(motorcycle.LicensePlate) is not null)
            {
                throw new Exception("Placa já cadastrada.");
            }

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
            _motorcycleRepository.Delete(id);
        }
    }
}