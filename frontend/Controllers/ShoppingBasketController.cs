using System;
using System.Linq;
using System.Threading.Tasks;
using GloboTicket.Frontend.Extensions;
using GloboTicket.Frontend.Models;
using GloboTicket.Frontend.Models.Api;
using GloboTicket.Frontend.Models.View;
using GloboTicket.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.Frontend.Controllers
{
    public class ShoppingBasketController : Controller
    {
        private readonly IShoppingBasketService basketService;
        private readonly Settings settings;
        private readonly ILogger<ShoppingBasketController> logger;

        public ShoppingBasketController(IShoppingBasketService basketService, Settings settings, ILogger<ShoppingBasketController> logger)
        {
            this.basketService = basketService;
            this.settings = settings;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            var basketLines = await basketService.GetLinesForBasket(basketId);
            
            // Check if ViewBag.TotalWithDiscount exists from session
            if (TempData["PromoCodeApplied"] != null)
            {
                ViewBag.PromoCodeApplied = true;
                ViewBag.TotalWithDiscount = await basketService.GetTotalPrice(basketId, true);
            }
            
            var lineViewModels = basketLines.Select(bl => new BasketLineViewModel
            {
                LineId = bl.BasketLineId,
                EventId = bl.EventId,
                EventName = bl.Event.Name,
                Date = bl.Event.Date,
                Price = bl.Price,
                OriginalPrice = bl.Event.IsOnSpecialOffer ? bl.Event.OriginalPrice : bl.Price,
                IsOnSpecialOffer = bl.Event.IsOnSpecialOffer,
                Quantity = bl.TicketAmount
            });
            
            return View(lineViewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLine(BasketLineForCreation basketLine)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            var newLine = await basketService.AddToBasket(basketId, basketLine);
            Response.Cookies.Append(settings.BasketIdCookieName, newLine.BasketId.ToString());

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLine(BasketLineForUpdate basketLineUpdate)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            await basketService.UpdateLine(basketId, basketLineUpdate);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveLine(Guid lineId)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            await basketService.RemoveLine(basketId, lineId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyPromoCode(string promoCode)
        {
            var basketId = Request.Cookies.GetCurrentBasketId(settings);
            await basketService.ApplyPromoCode(basketId, promoCode);
            
            // Set TempData to indicate promo code has been applied
            TempData["PromoCodeApplied"] = true;
            
            return RedirectToAction("Index");
        }
    }
}
