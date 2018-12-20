using System;

namespace Evento.Core.Domain
{
    public class Ticket : Entity
    {
        public Guid EventId { get; protected set; }

        public int Seating { get; protected set; }

        public decimal Price { get; }

        public Guid? UserId { get; protected set; }

        public string Username { get; protected set; }

        public DateTime? PurchaseAt { get; protected set; }

        public bool Purchased => PurchaseAt.HasValue;

        protected Ticket()
        {

        }

        public Ticket(Event @event, int seating, decimal price)
        {
            EventId = @event.Id;
            Seating = seating;
            Price = price;
        }
    }
}
