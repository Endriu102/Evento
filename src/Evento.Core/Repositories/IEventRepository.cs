using System;
using System.Threading.Tasks;
using Evento.Core.Domain;
using System.Collections.Generic;

namespace Evento.Core.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetAsync(Guid id);

        Task<Event> GetAsync(string name);

        Task<IEnumerable<Event>> BrowseAsync(string name = "");

        Task AddAsync(Event @event);

        Task UpdateAsync(Event @event);

        Task DelteAsync(Event @event);
    }
}
