using Rentt.Entities;
using Rentt.Enums;
using Rentt.Models;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class RentService : IRentService
    {
        private readonly IRentRepository _rentRepository;
        private readonly IRentalPlanService _rentalPlanService;
        private readonly IMotorcycleService _motorcycleService;
        private readonly IDeliverymanService _deliverymanService;

        public RentService(
            IRentRepository rentRepository,
            IRentalPlanService rentalPlanService,
            IMotorcycleService motorcycleService,
            IDeliverymanService deliverymanService)
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

        public ResultRentt Create(CreateRentModel createRent, User user)
        {
            var rentalPlan = _rentalPlanService.GetById(createRent.RentalPlanId);

            if (rentalPlan is null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Plano de locação inexistente."
                };
            }

            var motorcycle = _motorcycleService.GetById(createRent.MotorcycleId);

            if (motorcycle is null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Moto não encontrada."
                };
            }

            var deliveryman = _deliverymanService.GetByUserId(user.Id);

            if (deliveryman is null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Usuário não é entregador."
                };
            }

            if (deliveryman.DriverLicenseType != DriverLicenseType.A)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Somente entregadores habilitados na categoria A podem efetuar uma locação."
                };
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

            _rentRepository.Create(rent);

            return new ResultRentt
            {
                Success = true,
                Object = rent
            };
        }

        public ResultRentt CalculateTotalRentalCost(string rentId)
        {
            var rent = _rentRepository.GetById(rentId);

            if (rent == null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Locação não encontrada."
                };
            }

            var rentalPlan = _rentalPlanService.GetById(rent.RentalPlanId);

            if (rentalPlan is null)
            {
                return new ResultRentt
                {
                    Success = false,
                    Details = "Plano de locação inexistente."
                };
            }

            var daysElapsed = DateTime.Now.Subtract(rent.StartDate).Days;

            if (daysElapsed > rentalPlan.Days)
            {
                var daysDifference = daysElapsed - rentalPlan.Days;

                var penalty = 50 * daysDifference;

                return new ResultRentt
                {
                    Success = true,
                    Object = rentalPlan.PriceByDay * rentalPlan.Days + penalty
                };
            }

            if (daysElapsed < rentalPlan.Days)
            {
                var daysDifference = rentalPlan.Days - daysElapsed;

                var penaltyRate = rentalPlan.Days == 7 ? 0.20 : 0.40;

                var penalty = rentalPlan.PriceByDay * daysDifference * penaltyRate;

                return new ResultRentt
                {
                    Success = true,
                    Object = rentalPlan.PriceByDay * daysElapsed + penalty
                };
            }

            return new ResultRentt
            {
                Success = true,
                Object = rentalPlan.PriceByDay * rentalPlan.Days
            };
        }
    }
}
