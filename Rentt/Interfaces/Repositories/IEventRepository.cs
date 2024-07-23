using Rentt.Entities;

namespace Rentt.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> Get();
        Event Create(Event motorcycle);
    }
}
