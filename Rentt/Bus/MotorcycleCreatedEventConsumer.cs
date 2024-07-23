using MassTransit;
using MongoDB.Bson;
using Rentt.Events;
using Rentt.Repositories;
using Rentt.Services;

namespace Rentt.Bus
{
    public class MotorcycleCreatedEventConsumer : IConsumer<MotorcycleCreatedEvent>
    {
        private readonly ILogger<MotorcycleCreatedEventConsumer> _logger;
        private readonly IEventRepository _eventRepository;
        private readonly IEmailService _emailService;

        public MotorcycleCreatedEventConsumer(
            ILogger<MotorcycleCreatedEventConsumer> logger,
            IEventRepository eventRepository,
            IEmailService emailService)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _emailService = emailService;
        }

        public Task Consume(ConsumeContext<MotorcycleCreatedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation("Recebido evento de moto criada Id: {Id}, Ano: {Year}", message.MotorcycleId, message.MotorcycleYear);

            if (message.MotorcycleYear == 2024)
            {
                _logger.LogInformation("Notificando moto 2024 criada Id: {Id}, Ano: {Year}", message.MotorcycleId, message.MotorcycleYear);

                _emailService.Send("Moto de ano 2024 criada.", message.ToJson());
            }

            _eventRepository.Create(message);

            _logger.LogInformation("Evento salvo no banco de dados de moto Id: {Id}, Ano: {Year}", message.MotorcycleId, message.MotorcycleYear);

            return Task.CompletedTask;
        }
    }
}
