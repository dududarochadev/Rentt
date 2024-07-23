using MongoDB.Driver;
using Rentt.Data;
using Rentt.Entities;

namespace Rentt.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IMongoCollection<Event> _events;

        public EventRepository(MongoDbService mongoDbService)
        {
            _events = mongoDbService.Database?.GetCollection<Event>("event");
        }
        public IEnumerable<Event> Get()
        {
            var filter = FilterDefinition<Event>.Empty;

            return _events.Find(filter).ToList();
        }

        public Event Create(Event newEvent)
        {
            _events.InsertOne(newEvent);
            return newEvent;
        }
    }
}
