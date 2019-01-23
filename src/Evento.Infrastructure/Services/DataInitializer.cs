using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;

namespace Evento.Infrastructure.Services
{
    public class DataInitializer : IDataInitializer
    {
        private readonly IUserService _userService;
        private readonly IEventService _eventService;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DataInitializer(IUserService userService, IEventService eventService)
        {
            _userService = userService;
            _eventService = eventService;
        }

        public async Task SeedAsync()
        {
            logger.Info("Initializing data...");
            var tasks = new List<Task>();
            tasks.Add(_userService.RegisterAsync(Guid.NewGuid(), "user@email.com", "default user", "secret"));
            tasks.Add(_userService.RegisterAsync(Guid.NewGuid(), "admin@email.com", "default user", "secret", "admin"));
            logger.Info("Created users: user, admin.");

            for (int i = 1; i < 10; i++)
            {
                var eventId = Guid.NewGuid();
                var name = $"Event {i}";
                var description = $"{name} description.";
                var startDate = DateTime.UtcNow.AddHours(3);
                var endDate = startDate.AddHours(2);
                tasks.Add(_eventService.CreateAsync(eventId, name, description, startDate, endDate));
                tasks.Add(_eventService.AddTicketAsync(eventId, 1000, 100));
                logger.Info($"Created event: {name}.");
            }

            await Task.WhenAll(tasks);
            logger.Info("Data was initialized.");
        }
    }
}