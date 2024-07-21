using Rentt.Entities;
using Rentt.Enums;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class RentService
    {
        private readonly RentRepository _rentRepository;
        private readonly RentalPlanRepository _rentalPlanRepository;
        private readonly MotorcycleRepository _motorcycleRepository;
        private readonly UserRepository _userRepository;

        public RentService(
            RentRepository rentRepository,
            RentalPlanRepository rentalPlanRepository,
            MotorcycleRepository motorcycleRepository,
            UserRepository userRepository)
        {
            _rentRepository = rentRepository;
            _rentalPlanRepository = rentalPlanRepository;
            _motorcycleRepository = motorcycleRepository;
            _userRepository = userRepository;
        }

        public Rent? GetById(string id)
        {
            return _rentRepository.GetById(id);
        }

        public IEnumerable<Rent> GetByMotorcycleId(string motorcycleId)
        {
            return _rentRepository.GetByMotorcycleId(motorcycleId);
        }

        public Rent Create(CreateRent createRent)
        {
            var rentalPlan = _rentalPlanRepository.GetById(createRent.RentalPlanId);

            if (rentalPlan is null)
            {
                throw new Exception("Plano de locação inexistente.");
            }
            var motorcycle = _motorcycleRepository.GetById(createRent.MotorcycleId);

            if (motorcycle is null)
            {
                throw new Exception("Moto não encontrada.");
            }

            var user = _userRepository.GetById(createRent.UserId);

            if (user is null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            if (!user.IsDeliveryman)
            {
                throw new Exception("Somente entregadores podem efetuar uma locação.");
            }

            if (user.DriverLicenseType != DriverLicenseType.A)
            {
                throw new Exception("Somente entregadores habilitados na categoria A podem efetuar uma locação.");
            }

            var startDate = DateTime.Now.AddDays(1);

            var rent = new Rent
            {
                StartDate = startDate,
                ExpectedEndDate = startDate.AddDays(rentalPlan.Days),
                RentalPlan = rentalPlan,
                User = user,
                Motorcycle = motorcycle
            };

            return _rentRepository.Create(rent);
        }

        public double CalculateTotalRentalCost(string rentId)
        {
            var rent = _rentRepository.GetById(rentId);

            if (rent == null)
            {
                throw new Exception("Locação não encontrada.");
            }

            var daysElapsed = DateTime.Now.Subtract(rent.StartDate).Days;

            if (daysElapsed > rent.RentalPlan.Days)
            {
                var daysDifference = daysElapsed - rent.RentalPlan.Days;

                var penalty = 50 * daysDifference;

                return rent.RentalPlan.PriceByDay * rent.RentalPlan.Days + penalty;
            }

            if (daysElapsed < rent.RentalPlan.Days)
            {
                var daysDifference = rent.RentalPlan.Days - daysElapsed;

                var penaltyRate = rent.RentalPlan.Days == 7 ? 0.20 : 0.40;

                var penalty = rent.RentalPlan.PriceByDay * daysDifference * penaltyRate;

                return rent.RentalPlan.PriceByDay * daysElapsed + penalty;
            }

            return rent.RentalPlan.PriceByDay * rent.RentalPlan.Days;
        }
    }
}
