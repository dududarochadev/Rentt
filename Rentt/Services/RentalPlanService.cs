using Rentt.Entities;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class RentalPlanService
    {
        private readonly RentalPlanRepository _rentalPlanRepository;

        public RentalPlanService(RentalPlanRepository rentalPlanRepository)
        {
            _rentalPlanRepository = rentalPlanRepository;
        }

        public RentalPlan? GetById(string id)
        {
            return _rentalPlanRepository.GetById(id);
        }
    }
}
