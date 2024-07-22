using Rentt.Entities;
using Rentt.Enums;
using Rentt.Models;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class RentService
    {
        private readonly RentRepository _rentRepository;
        private readonly RentalPlanService _rentalPlanService;
        private readonly MotorcycleService _motorcycleService;
        private readonly DeliverymanService _deliverymanService;

        public RentService(
            RentRepository rentRepository,
            RentalPlanService rentalPlanService,
            MotorcycleService motorcycleService,
            DeliverymanService deliverymanService)
        {
            _rentRepository = rentRepository;
            _rentalPlanService = rentalPlanService;
            _motorcycleService = motorcycleService;
            _deliverymanService = deliverymanService;
        }

        public Rent? GetById(string id)
        {
            return _rentRepository.GetById(id);
        }

        public IEnumerable<Rent> GetByMotorcycleId(string motorcycleId)
        {
            return _rentRepository.GetByMotorcycleId(motorcycleId);
        }

        public Rent Create(CreateRentModel createRent, User user)
        {
            var rentalPlan = _rentalPlanService.GetById(createRent.RentalPlanId);

            if (rentalPlan is null)
            {
                throw new Exception("Plano de locação inexistente.");
            }

            var motorcycle = _motorcycleService.GetById(createRent.MotorcycleId);

            if (motorcycle is null)
            {
                throw new Exception("Moto não encontrada.");
            }

            var deliveryman = _deliverymanService.GetByUserId(user.Id);

            if (deliveryman is null)
            {
                throw new Exception("Usuário não é entregador.");
            }

            if (deliveryman.DriverLicenseType != DriverLicenseType.A)
            {
                throw new Exception("Somente entregadores habilitados na categoria A podem efetuar uma locação.");
            }

            var startDate = DateTime.Now.AddDays(1);

            var rent = new Rent
            {
                StartDate = startDate,
                ExpectedEndDate = startDate.AddDays(rentalPlan.Days),
                RentalPlanId = rentalPlan.Id,
                DeliverymanId = deliveryman.Id,
                MotorcycleId = motorcycle.Id
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

            var rentalPlan = _rentalPlanService.GetById(rent.RentalPlanId);

            if (rentalPlan is null)
            {
                throw new Exception("Plano de locação inexistente.");
            }

            var daysElapsed = DateTime.Now.Subtract(rent.StartDate).Days;

            if (daysElapsed > rentalPlan.Days)
            {
                var daysDifference = daysElapsed - rentalPlan.Days;

                var penalty = 50 * daysDifference;

                return rentalPlan.PriceByDay * rentalPlan.Days + penalty;
            }

            if (daysElapsed < rentalPlan.Days)
            {
                var daysDifference = rentalPlan.Days - daysElapsed;

                var penaltyRate = rentalPlan.Days == 7 ? 0.20 : 0.40;

                var penalty = rentalPlan.PriceByDay * daysDifference * penaltyRate;

                return rentalPlan.PriceByDay * daysElapsed + penalty;
            }

            return rentalPlan.PriceByDay * rentalPlan.Days;
        }
    }
}
