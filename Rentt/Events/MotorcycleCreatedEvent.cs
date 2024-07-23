using Rentt.Entities;

namespace Rentt.Events
{
    public class MotorcycleCreatedEvent : Event
    {
        public string MotorcycleId { get; set; }
        public int MotorcycleYear { get; set; }
    }
}
