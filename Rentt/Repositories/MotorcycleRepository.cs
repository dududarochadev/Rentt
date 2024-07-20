using MongoDB.Driver;
using Rentt.Data;
using Rentt.Entities;

namespace Rentt.Repositories
{
    public class MotorcycleRepository
    {
        private readonly IMongoCollection<Motorcycle> _motorcycles;

        public MotorcycleRepository(MongoDbService mongoDbService)
        {
            _motorcycles = mongoDbService.Database?.GetCollection<Motorcycle>("motorcycle");
        }

        public IEnumerable<Motorcycle> Get()
        {
            return _motorcycles.Find(FilterDefinition<Motorcycle>.Empty).ToList();
        }

        public Motorcycle? GetById(string id)
        {
            var filter = Builders<Motorcycle>.Filter.Eq(x => x.Id, id);
            return _motorcycles.Find(filter).FirstOrDefault();
        }

        public Motorcycle? GetByLicensePlate(string licensePlate)
        {
            var filter = Builders<Motorcycle>.Filter.Eq(x => x.LicensePlate, licensePlate);
            return _motorcycles.Find(filter).FirstOrDefault();
        }

        public Motorcycle Create(Motorcycle motorcycle)
        {
            _motorcycles.InsertOne(motorcycle);
            return motorcycle;
        }

        public void Update(Motorcycle motorcycle)
        {
            var filter = Builders<Motorcycle>.Filter.Eq(x => x.Id, motorcycle.Id);
            _motorcycles.ReplaceOne(filter, motorcycle);
        }

        public void Delete(string id)
        {
            var filter = Builders<Motorcycle>.Filter.Eq(x => x.Id, id);
            _motorcycles.DeleteOne(filter);
        }
    }
}
