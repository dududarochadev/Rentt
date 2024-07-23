using Rentt.Entities;

namespace Rentt.Repositories
{
    public interface IRentalPlanRepository
    {
        RentalPlan? GetById(string id);
        void SeedData();
    }
}
