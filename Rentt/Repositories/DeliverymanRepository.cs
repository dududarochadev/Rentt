using MongoDB.Driver;
using Rentt.Data;
using Rentt.Entities;

namespace Rentt.Repositories
{
    public class DeliverymanRepository
    {
        private readonly IMongoCollection<Deliveryman> _deliverymans;

        public DeliverymanRepository(MongoDbService mongoDbService)
        {
            _deliverymans = mongoDbService.Database?.GetCollection<Deliveryman>("deliveryman");
        }

        public Deliveryman? GetById(string id)
        {
            var filter = Builders<Deliveryman>.Filter.Eq(x => x.Id, id);
            return _deliverymans.Find(filter).FirstOrDefault();
        }

        public Deliveryman? GetByUserId(string userId)
        {
            var filter = Builders<Deliveryman>.Filter.Eq(x => x.UserId, userId);
            return _deliverymans.Find(filter).FirstOrDefault();
        }

        public Deliveryman? GetByCnpj(string cnpj)
        {
            var filter = Builders<Deliveryman>.Filter.Eq(x => x.Cnpj, cnpj);
            return _deliverymans.Find(filter).FirstOrDefault();
        }

        public Deliveryman? GetByDriverLicenseNumber(string driverLicenseNumber)
        {
            var filter = Builders<Deliveryman>.Filter.Eq(x => x.DriverLicenseNumber, driverLicenseNumber);
            return _deliverymans.Find(filter).FirstOrDefault();
        }

        public Deliveryman Create(Deliveryman deliveryman)
        {
            _deliverymans.InsertOne(deliveryman);
            return deliveryman;
        }

        public void Update(Deliveryman deliveryman)
        {
            var filter = Builders<Deliveryman>.Filter.Eq(x => x.Id, deliveryman.Id);
            _deliverymans.ReplaceOne(filter, deliveryman);
        }

        public void Delete(string id)
        {
            var filter = Builders<Deliveryman>.Filter.Eq(x => x.Id, id);
            _deliverymans.DeleteOne(filter);
        }
    }
}
