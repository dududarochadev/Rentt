using MongoDB.Driver;
using Rentt.Data;
using Rentt.Entities;

namespace Rentt.Repositories
{
    public class RentRepository
    {
        private readonly IMongoCollection<Rent> _rents;

        public RentRepository(MongoDbService mongoDbService)
        {
            _rents = mongoDbService.Database?.GetCollection<Rent>("rent");
        }
        
        public Rent? GetById(string id)
        {
            var filter = Builders<Rent>.Filter.Eq(x => x.Id, id);
            return _rents.Find(filter).FirstOrDefault();
        }

        public IEnumerable<Rent> GetByMotorcycleId(string motorcycleId)
        {
            var filter = Builders<Rent>.Filter.Eq(x => x.Motorcycle.Id, motorcycleId);
            return _rents.Find(filter).ToList();
        }

        public Rent Create(Rent motorcycle)
        {
            _rents.InsertOne(motorcycle);
            return motorcycle;
        }
    }
}
