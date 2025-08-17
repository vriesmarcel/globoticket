using Microsoft.EntityFrameworkCore;

namespace GloboTicket.Catalog.DbContexts
{
    public class EventCatalogDbContext : DbContext
    {
        public EventCatalogDbContext(DbContextOptions<EventCatalogDbContext> options) :
        base(options)
        {
        }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision for Price and OriginalPrice
            modelBuilder.Entity<Event>()
                .Property(e => e.Price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Event>()
                .Property(e => e.OriginalPrice)
                .HasPrecision(10, 2);

            // Original 5 events
            SeedOriginalEvents(modelBuilder);

            // Add more events - 500 additional events in batches
            SeedAdditionalEvents(modelBuilder, 0, 100);
            SeedAdditionalEvents(modelBuilder, 100, 100);
            SeedAdditionalEvents(modelBuilder, 200, 100);
            SeedAdditionalEvents(modelBuilder, 300, 100);
            SeedAdditionalEvents(modelBuilder, 400, 100);
        }

        private void SeedOriginalEvents(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{EE272F8B-6096-4CB6-8625-BB4BB2D89E8B}"),
                Name = "John Egbert Live",
                Price = 64.99m,
                OriginalPrice = 64.99m,
                Artist = "John Egbert",
                Date = DateTime.Now.AddMonths(6),
                Description = "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
                ImageUrl = "/img/banjo.jpg",
                IsOnSpecialOffer = false
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{3448D5A4-0F72-4DD7-BF15-C14A46B26C00}"),
                Name = "The State of Affairs: Michael Live!",
                Price = 84.99m,
                OriginalPrice = 84.99m,
                Artist = "Michael Johnson",
                Date = DateTime.Now.AddMonths(9),
                Description = "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
                ImageUrl = "/img/michael.jpg",
                IsOnSpecialOffer = false
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{B419A7CA-3321-4F38-BE8E-4D7B6A529319}"),
                Name = "Clash of the DJs",
                Price = 84.99m,
                OriginalPrice = 84.99m,
                Artist = "DJ 'The Mike'",
                Date = DateTime.Now.AddMonths(4),
                Description = "DJs from all over the world will compete in this epic battle for eternal fame.",
                ImageUrl = "/img/dj.jpg",
                IsOnSpecialOffer = false
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{62787623-4C52-43FE-B0C9-B7044FB5929B}"),
                Name = "Spanish guitar hits with Manuel",
                Price = 24.99m,
                OriginalPrice = 24.99m,
                Artist = "Manuel Santinonisi",
                Date = DateTime.Now.AddMonths(4),
                Description = "Get on the hype of Spanish Guitar concerts with Manuel.",
                ImageUrl = "/img/guitar.jpg",
                IsOnSpecialOffer = false
            });

            modelBuilder.Entity<Event>().HasData(new Event
            {
                EventId = Guid.Parse("{ADC42C09-08C1-4D2C-9F96-2D15BB1AF299}"),
                Name = "To the Moon and Back",
                Price = 134.99m,
                OriginalPrice = 134.99m,
                Artist = "Nick Sailor",
                Date = DateTime.Now.AddMonths(8),
                Description = "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
                ImageUrl = "/img/musical.jpg",
                IsOnSpecialOffer = false
            });
        }

        private void SeedAdditionalEvents(ModelBuilder modelBuilder, int startIndex, int count)
        {
            var random = new Random(startIndex);
            var artists = new string[]
            {
                "Taylor Swift", "Ed Sheeran", "Beyoncé", "BTS", "Drake", "Adele", "The Weeknd", "Billie Eilish",
                "Harry Styles", "Lady Gaga", "Justin Bieber", "Ariana Grande", "Post Malone", "Bruno Mars",
                "Dua Lipa", "Kendrick Lamar", "Rihanna", "Coldplay", "Imagine Dragons", "Katy Perry",
                "J Balvin", "Maroon 5", "Travis Scott", "Eminem", "Bad Bunny", "Shawn Mendes", "Camila Cabello",
                "Doja Cat", "Cardi B", "Twenty One Pilots", "Halsey", "Lewis Capaldi", "Olivia Rodrigo",
                "Lil Nas X", "The Chainsmokers", "Calvin Harris", "SZA", "Marshmello", "Lizzo", "Blackpink",
                "Daddy Yankee", "Sia", "Selena Gomez", "Jonas Brothers", "Future", "The Kid LAROI", "Megan Thee Stallion",
                "Juice WRLD", "Kygo", "Miley Cyrus", "David Guetta", "Elton John", "Khalid", "Charlie Puth",
                "Sam Smith", "Martin Garrix", "Lana Del Rey", "Niall Horan", "Ava Max", "Luke Combs", "DaBaby",
                "Lil Baby", "Jack Harlow", "Tame Impala", "Maluma", "Rosalía", "Tones and I", "Glass Animals",
                "Demi Lovato", "Alan Walker", "OneRepublic", "Zara Larsson", "KAROL G", "Lil Uzi Vert", "Tiësto",
                "Alicia Keys", "5 Seconds of Summer", "Nicki Minaj", "Little Mix", "YUNGBLUD", "Kehlani",
                "Machine Gun Kelly", "Panic! At The Disco", "Tyler, The Creator", "Bastille", "Nicky Jam",
                "Anitta", "Troye Sivan", "Mabel", "G-Eazy", "Dan + Shay", "Meghan Trainor", "NCT 127",
                "Avicii", "Fall Out Boy", "SEVENTEEN", "Robin Schulz", "Jason Derulo", "Offset", "Anne-Marie",
                "Zedd", "Young Thug", "Lauv", "James Arthur", "BLACKPINK ROSÉ", "Madison Beer", "LISA", "The Script"
            };

            var tourNames = new string[]
            {
                "World Tour", "Live in Concert", "Reunion Tour", "Greatest Hits Tour", "The Experience",
                "Summer Festival", "Unplugged", "Live at Home", "The Sessions", "Acoustic Night",
                "Stadium Tour", "Farewell Tour", "Comeback Tour", "Anniversary Tour", "Legends Tour"
            };

            var descriptions = new string[]
            {
                "An unforgettable night with {0} showcasing their greatest hits and fan favorites. Don't miss this chance to see one of the most iconic performers of our time.",
                "Experience the magic of {0} in this once-in-a-lifetime concert event. With stunning visuals and incredible sound, this is a show you'll remember forever.",
                "Join {0} for an intimate evening of music and storytelling. This special event brings fans closer than ever to their favorite artist.",
                "{0} returns to the stage with a spectacular new show featuring their latest album and classic favorites. A must-see for true fans!",
                "The electrifying {0} brings their high-energy performance to town with special guests and surprises throughout the night."
            };

            var images = new string[]
            {
                "/img/banjo.jpg",
                "/img/michael.jpg",
                "/img/dj.jpg",
                "/img/guitar.jpg",
                "/img/musical.jpg"
            };

            for (int i = 0; i < count; i++)
            {
                int index = startIndex + i;
                int artistIndex = index % artists.Length;
                string artist = artists[artistIndex];
                
                if (index > artists.Length)
                {
                    int secondIndex = (index / artists.Length) % artists.Length;
                    if (secondIndex != artistIndex)
                    {
                        artist = $"{artists[artistIndex]} & {artists[secondIndex]}";
                    }
                }

                var tourName = tourNames[index % tourNames.Length];
                var eventName = $"{artist}: {tourName}";
                var description = string.Format(descriptions[index % descriptions.Length], artist);
                var imageUrl = images[index % images.Length];
                
                // Calculate base price using modulo operation (20, 30, 40, etc.)
                var basePrice = 20 + ((index % 19) * 10);
                // Convert to psychological price (e.g., 19.99, 29.99, 39.99)
                var price = basePrice - 0.01m;
                
                var date = DateTime.Now.AddDays(30 + (index * 3));

                modelBuilder.Entity<Event>().HasData(new Event
                {
                    EventId = Guid.NewGuid(),
                    Name = eventName,
                    Price = price,
                    OriginalPrice = price,
                    Artist = artist,
                    Date = date,
                    Description = description,
                    ImageUrl = imageUrl,
                    IsOnSpecialOffer = false
                });
            }
        }
    }
}