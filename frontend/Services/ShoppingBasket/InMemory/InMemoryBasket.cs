using GloboTicket.Frontend.Models.Api;
using System;
using System.Collections.Generic;

namespace GloboTicket.Frontend.Services
{
    class InMemoryBasket
    {
        public InMemoryBasket(Guid userId)
        {
            BasketId = Guid.NewGuid();
            Lines = new List<BasketLine>();
            UserId = userId;
        }
        public Guid BasketId { get; }
        public List<BasketLine> Lines { get; }
        public Guid UserId { get;  }
        public string PromoCode { get; set; }

        public BasketLine Add(BasketLineForCreation line, Event @event)
        {
            var basketLine = new BasketLine() { 
                EventId = line.EventId, 
                TicketAmount = line.TicketAmount,
                Event = @event,
                BasketId = this.BasketId, 
                BasketLineId = Guid.NewGuid(), 
                Price = line.Price 
            };
            Lines.Add(basketLine);
            return basketLine;
        }
        public void Remove(Guid lineId)
        {
            var index = Lines.FindIndex(bl => bl.BasketLineId == lineId);
            if (index >= 0) Lines.RemoveAt(index);
        }

        public void Update(BasketLineForUpdate basketLineForUpdate)
        {
            var index = Lines.FindIndex(bl => bl.BasketLineId == basketLineForUpdate.LineId);
            Lines[index].TicketAmount = basketLineForUpdate.TicketAmount;
        }
        public void Clear()
        {
            Lines.Clear();
        }

        public decimal CalculateTotalPrice(bool applyPromoCode = false)
        {
            decimal totalPrice = 0;
            foreach (var line in Lines)
            {
                // Calculate price for each line
                decimal linePrice = line.Price * line.TicketAmount;
                
                // Store if this item is on special offer (will help debugging)
                bool isOnSpecialOffer = line.Event.IsOnSpecialOffer;
                
                // Apply promotional code discount (bug: applying to already discounted price)
                if (applyPromoCode && !string.IsNullOrEmpty(PromoCode))
                {
                    decimal promoDiscount = GetPromoCodeDiscount(PromoCode);
                    linePrice = linePrice * (1 - promoDiscount);
                    
                    // Bug: Incorrect rounding - truncating instead of rounding properly
                    linePrice = Math.Truncate(linePrice * 100) / 100;
                }
                
                totalPrice += linePrice;
            }
            
            return totalPrice;
        }

        private decimal GetPromoCodeDiscount(string promoCode)
        {
            // Simple promo code logic
            switch (promoCode.ToUpper())
            {
                case "SAVE10": return 0.10m;
                case "SAVE15": return 0.15m;
                case "SAVE25": return 0.25m;
                default: return 0;
            }
        }
    }
}
