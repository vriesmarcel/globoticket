using GloboTicket.Catalog.DbContexts;
using Microsoft.EntityFrameworkCore;


namespace GloboTicket.Catalog.Repositories;

public class SqlEventRepository : IEventRepository
{
    private readonly EventCatalogDbContext _eventCatalogDbContext;


    private readonly ILogger<SqlEventRepository> logger;

    public SqlEventRepository(EventCatalogDbContext eventCatalogDbContext,
        ILogger<SqlEventRepository> logger)
    {
        this.logger = logger;
        _eventCatalogDbContext = eventCatalogDbContext;
    }

    public  Task<IEnumerable<Event>> GetEvents()
    {
        return  Task.FromResult<IEnumerable<Event>>(_eventCatalogDbContext.Events);
    }

    public Task<Event> GetEventById(Guid eventId)
    {
        var @event = _eventCatalogDbContext.Events.FirstOrDefault(e => e.EventId == eventId);
        if (@event == null)
        {
            throw new InvalidOperationException("Event not found");
        }
        return Task.FromResult(@event);
    }

    void IEventRepository.UpdateSpecialOffer()
    {
        throw new NotImplementedException();
    }
}
