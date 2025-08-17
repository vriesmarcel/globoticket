namespace GloboTicket.Catalog.Repositories;

public class EventRepository : IEventRepository
{
    private List<Event> events = new List<Event>();
    private readonly ILogger<EventRepository> logger;

    public EventRepository(ILogger<EventRepository> logger)
    {
        this.logger = logger;

        LoadSampleData();
    }

    private void LoadSampleData()
    {
        var johnEgbertGuid = Guid.Parse("{CFB88E29-4744-48C0-94FA-B25B92DEA317}");
        var nickSailorGuid = Guid.Parse("{CFB88E29-4744-48C0-94FA-B25B92DEA318}");
        var michaelJohnsonGuid = Guid.Parse("{CFB88E29-4744-48C0-94FA-B25B92DEA319}");

        events.Add(new Event
        {
            EventId = johnEgbertGuid,
            Name = "John Egbert Live",
            Price = 65,
            OriginalPrice = 65,
            Artist = "John Egbert",
            Date = DateTime.Now.AddMonths(6),
            Description = "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
            ImageUrl = "/img/banjo.jpg",
            IsOnSpecialOffer = false
        });

        events.Add(new Event
        {
            EventId = michaelJohnsonGuid,
            Name = "The State of Affairs: Michael Live!",
            Price = 85,
            OriginalPrice = 85,
            Artist = "Michael Johnson",
            Date = DateTime.Now.AddMonths(9),
            Description = "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
            ImageUrl = "/img/michael.jpg",
            IsOnSpecialOffer = false
        });

        events.Add(new Event
        {
            EventId = nickSailorGuid,
            Name = "To the Moon and Back",
            Price = 135,
            OriginalPrice = 135,
            Artist = "Nick Sailor",
            Date = DateTime.Now.AddMonths(8),
            Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
            ImageUrl = "/img/musical.jpg",
            IsOnSpecialOffer = false
        });
    }

    public Task<IEnumerable<Event>> GetEvents()
    {
        // Sort events by promotion status (promotions first) and then by date
        var sortedEvents = events.ToList()
            .OrderByDescending(e => e.IsOnSpecialOffer)
            .ThenBy(e => e.Date)
            .ToList();
            
        // Return the sorted list
        return Task.FromResult((IEnumerable<Event>)sortedEvents);
    }


    public Task<Event> GetEventById(Guid eventId)
    {
        var @event = events.ToList().FirstOrDefault(e => e.EventId == eventId);
        if (@event == null)
        {
            throw new InvalidOperationException("Event not found");
        }
        return Task.FromResult(@event);
    }

    // scheduled task calls this periodically to put one item on special offer
    public void UpdateSpecialOffer()
    {
        // reset all tickets to their default
        events.Clear();
        LoadSampleData();
        // pick a random one to put on special offer
        var random = new Random();
        var specialOfferEvent = events[random.Next(0, events.Count)];
        
        // Store the original price
        specialOfferEvent.OriginalPrice = specialOfferEvent.Price;
        
        // Apply 20 percent discount and round to psychological price
        var discountedPrice = specialOfferEvent.Price * 0.8m;
        specialOfferEvent.Price = Math.Round(discountedPrice - 0.01m, 2);
        
        // Mark as special offer
        specialOfferEvent.IsOnSpecialOffer = true;
        
        logger.LogInformation($"Event {specialOfferEvent.Name} is now on special offer at ${specialOfferEvent.Price} (was ${specialOfferEvent.OriginalPrice})");
    }
}
