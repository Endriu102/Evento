using System;
using System.Collections.Generic;
using System.Text;

namespace Evento.Infrastructure.DTO
{
    public class EventDetailsDto : EventDTO
    {
        public IEnumerable<TicketDto> Tickets { get; set; }
    }
}
