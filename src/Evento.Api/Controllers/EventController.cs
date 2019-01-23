using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Evento.Infrastructure.Commands.Events;
using Evento.Infrastructure.DTO;
using Evento.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Evento.Api.Controllers
{
    [Route("[controller]")]
    public class EventController : ApiControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMemoryCache _memoryCache;

        public EventController(IEventService eventService, IMemoryCache memoryCache)
        {
            _eventService = eventService;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            var events = _memoryCache.Get<IEnumerable<EventDTO>>("events");

            if (events == null)
            {
                Console.WriteLine("Fetching from service.");
                events = await _eventService.BrowseAsync(name);
                _memoryCache.Set("events", events, TimeSpan.FromMinutes(1));
            }
            else
            {
                Console.WriteLine("Fetching from cache.");
            }

            return Json(events);
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> Get(Guid eventId)
        {
            var @event = await _eventService.GetAsync(eventId);

            if (@event == null)
            {
                return NotFound();
            }

            return Json(@event);
        }

        [HttpPost]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<IActionResult> Post([FromBody] CreateEvent command)
        {
            command.EventId = Guid.NewGuid();

            await _eventService.CreateAsync(command.EventId, command.Name, command.Description, command.StartDate,
                command.EndDate);

            await _eventService.AddTicketAsync(command.EventId, command.Tickets, command.Price);

            return Created($"/event/{command.EventId}", null);
        }

        [HttpPut("{eventId}")]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<IActionResult> Put(Guid eventId, [FromBody] UpdateEvent command)
        {
            command.EventId = Guid.NewGuid();

            await _eventService.UpdateAsync(eventId, command.Name, command.Description);

            return NoContent();
        }

        [HttpDelete("{eventId}")]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<IActionResult> Delete(Guid eventId)
        {
            await _eventService.DeleteAsync(eventId);

            return NoContent();
        }
    }
}