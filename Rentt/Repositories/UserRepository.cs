using MongoDB.Driver;
using Rentt.Data;
using Rentt.Entities;

namespace Rentt.Repositories
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbService mongoDbService)
        {
            _users = mongoDbService.Database.GetCollection<User>("users");
        }

        public User? GetById(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            return _users.Find(filter).FirstOrDefault();
        }

        public User? GetByCnpj(string cnpj)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Cnpj, cnpj);
            return _users.Find(filter).FirstOrDefault();
        }

        public User? GetByDriverLicenseNumber(string driverLicenseNumber)
        {
            var filter = Builders<User>.Filter.Eq(u => u.DriverLicenseNumber, driverLicenseNumber);
            return _users.Find(filter).FirstOrDefault();
        }

        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public User Update(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var updateResult = _users.ReplaceOne(filter, user);

            if (updateResult.MatchedCount == 0)
            {
                throw new Exception("Usuário não encontrado.");
            }

            return user;
        }

        public void Delete(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var deleteResult = _users.DeleteOne(filter);

            if (deleteResult.DeletedCount == 0)
            {
                throw new Exception("Usuário não encontrado.");
            }
        }
    }
}
