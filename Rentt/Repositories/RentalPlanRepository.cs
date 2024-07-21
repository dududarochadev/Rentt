using MongoDB.Driver;
using Rentt.Data;
using Rentt.Entities;

namespace Rentt.Repositories
{
    public class RentalPlanRepository
    {
        private readonly IMongoCollection<RentalPlan> _rentalPlans;

        public RentalPlanRepository(MongoDbService mongoDbService)
        {
            _rentalPlans = mongoDbService.Database?.GetCollection<RentalPlan>("rentalPlan");
        }

        public RentalPlan? GetById(string id)
        {
            var filter = Builders<RentalPlan>.Filter.Eq(x => x.Id, id);
            return _rentalPlans.Find(filter).FirstOrDefault();
        }

        public void SeedData()
        {
            var existingPlans = _rentalPlans.Find(FilterDefinition<RentalPlan>.Empty).ToList();

            if (existingPlans.Count == 0)
            {
                var defaultPlans = new List<RentalPlan>
                {
                    new() { Days = 7, PriceByDay = 30.0 },
                    new() {  Days = 15, PriceByDay = 28.0 },
                    new() {  Days = 30, PriceByDay = 22.0 },
                    new() {  Days = 45, PriceByDay = 20.0 },
                    new() {  Days = 50, PriceByDay = 18.0 }
                };

                _rentalPlans.InsertMany(defaultPlans);
            }
        }
    }
}
