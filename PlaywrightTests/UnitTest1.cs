using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace PlaywrightTests;

[TestClass]
public class UnitTest1 : PageTest
{
  
    [TestInitialize]
    public async Task TestInitialize()
    {
        await Context.Tracing.StartAsync(new()
        {
            Title = $"{TestContext.FullyQualifiedTestClassName}.{TestContext.TestName}",
            Screenshots = true,
            Snapshots = true,
            Sources = true
        });
    }

    [TestCleanup]
    public async Task TestCleanup()
    {
        await Context.Tracing.StopAsync(new()
        {
            Path = Path.Combine(
                Environment.CurrentDirectory,
                "playwright-traces",
                $"{TestContext.FullyQualifiedTestClassName}.{TestContext.TestName}.zip"
            )
        });
    }

    [TestMethod]
    public async Task BuyOneProduct()
    {
        
        var page = await Context.NewPageAsync();
        //await page.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot.png" });
        await StartDriver(page);
        await page.GotoAsync("https://globoticket-frontend-dpfbe7hxa6d2bdab.westeurope-01.azurewebsites.net/");
        await page.SetViewportSizeAsync(1552, 832);

        await BuyTwoProducts(page);
        await CheckOut(page);
    }

    private static async Task CheckOut(IPage page)
    {
        await page.FillAsync("#Name", "marcel");
        await page.FillAsync("#Email", "vriesmarcel@hotmail.com");
        await page.FillAsync("#Address", "Kerkhofweg 12");
        await page.ClickAsync("#Town");
        await page.FillAsync("#Town", "warnsveld");
        await page.FillAsync("#PostalCode", "1213vb");
        await page.FillAsync("#CreditCard", "1111222233334444");
        await page.ClickAsync("#CreditCardDate");
        await page.FillAsync("#CreditCardDate", "12/24");
        await page.ClickAsync(".btn");
    }

    private static async Task BuyTwoProducts(IPage page)
    {
        await page.ClickAsync("tr:nth-child(2) .btn-primary");
        await page.ClickAsync("[name='TicketAmount']");
        await page.SelectOptionAsync("[name='TicketAmount']", "2");
        await page.ClickAsync(".btn");
        await page.ClickAsync("text=CHECKOUT");
        await page.ClickAsync("#Name");
    }

    [TestMethod]
    public async Task BuyOneProductAfterAddingAndRemovingAnotherProduct()
    {
        var page = await Context.NewPageAsync();
        await StartDriver(page);
  
        await page.ClickAsync("text=PURCHASE DETAILS");
        await page.ClickAsync(".btn");
        await page.ClickAsync("text=Back to event catalog");
        await page.ClickAsync("tr:nth-child(2) .btn-primary");
        await page.ClickAsync(".btn");
        await page.ClickAsync("text=CHECKOUT");
        await page.ClickAsync("#Name");
        await page.FillAsync("#Name", "m");
        await page.FillAsync("#Email", "m@m.com");
        await page.FillAsync("#Address", "a");
        await page.FillAsync("#Town", "b");
        await page.FillAsync("#PostalCode", "1212vb");
        await page.FillAsync("#CreditCard", "1111222233334444");
        await page.FillAsync("#CreditCardDate", "12/24");
        await page.ClickAsync("#Email");
        await page.ClickAsync(".btn");
        await page.ClickAsync("#CreditCard");
        await page.FillAsync("#CreditCard", "1111222233334444");
        await page.ClickAsync(".btn");
    }

    private static async Task StartDriver(IPage page)
    {
        await page.GotoAsync("https://globoticket-frontend-dpfbe7hxa6d2bdab.westeurope-01.azurewebsites.net/");
        await page.SetViewportSizeAsync(1552, 832);
    }
}
