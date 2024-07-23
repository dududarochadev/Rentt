using Rentt.Entities;
using Rentt.Repositories;

namespace Rentt.Services
{
    public class RentalPlanService : IRentalPlanService
    {
        private readonly IRentalPlanRepository _rentalPlanRepository;

        public RentalPlanService(IRentalPlanRepository rentalPlanRepository)
        {
            _rentalPlanRepository = rentalPlanRepository;
        }

        public RentalPlan? GetById(string id)
        {
            return _rentalPlanRepository.GetById(id);
        }
    }
}
