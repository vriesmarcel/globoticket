namespace GloboTicket.Frontend.Models.Api;

public class OrderLine
{
    public Guid EventId { get; set; }
    public int TicketCount { get; set; }
    public decimal Price { get; set; }

}
