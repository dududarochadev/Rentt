using Rentt.Entities;

namespace Rentt.Services
{
    public interface IRentalPlanService
    {
        RentalPlan? GetById(string id);
    }
}
