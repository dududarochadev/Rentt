using MongoDB.Driver;
using Rentt.Data;
using Rentt.Entities;

namespace Rentt.Repositories
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly IMongoCollection<Motorcycle> _motorcycles;
        private readonly IRentRepository _rentRepository;

        public MotorcycleRepository(
            MongoDbService mongoDbService,
            IRentRepository rentRepository
            )
        {
            _motorcycles = mongoDbService.Database?.GetCollection<Motorcycle>("motorcycle");
            _rentRepository = rentRepository;
        }

        public IEnumerable<Motorcycle> Get(string? licensePlate)
        {
            var filter = FilterDefinition<Motorcycle>.Empty;

            if (!string.IsNullOrEmpty(licensePlate))
            {
                filter = Builders<Motorcycle>.Filter.Eq(x => x.LicensePlate, licensePlate);
            }

            return _motorcycles.Find(filter).ToList();
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

        public bool HasRent(string id)
        {
            return _rentRepository.GetByMotorcycleId(id).Any();
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
