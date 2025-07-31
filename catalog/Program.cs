using GloboTicket.Catalog.DbContexts;
using GloboTicket.Catalog.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IEventRepository, EventRepository>();
//using "real" database
//builder.Services.AddDbContext<EventCatalogDbContext>(options =>
  //        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddScoped<IEventRepository, SqlEventRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    try
    {
        using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<EventCatalogDbContext>();
            context.Database.EnsureCreated();
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
