using System;

namespace Evento.Infrastructure.DTO
{
    public class TicketDto
    {
        public Guid Id { get;  set; }

        public int Seating { get;  set; }

        public decimal Price { get; }

        public Guid? UserId { get;  set; }

        public string Username { get;  set; }

        public DateTime? PurchaseAt { get;  set; }

        public bool Purchased => PurchaseAt.HasValue;
    }
}