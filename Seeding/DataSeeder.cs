using VenueTracker.Data;
using VenueTracker.Models;

namespace VenueTracker.Seeding
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedFromCsv(string csvPath)
        {
            if (!File.Exists(csvPath))
            {
                Console.WriteLine($"CSV file not found: {csvPath}");
                return;
            }

            var records = CsvImporter.ImportCsv(csvPath);
            if (records.Count == 0)
            {
                Console.WriteLine("No records found in CSV");
                return;
            }

            Console.WriteLine($"Processing {records.Count} records from CSV...");

            // Extract unique venues
            var uniqueVenues = records
                .GroupBy(r => new { r.VenueEvent, r.City, r.State })
                .Select(g => new { g.Key.VenueEvent, g.Key.City, g.Key.State, Capacity = g.FirstOrDefault()?.Capacity })
                .ToList();

            // Seed venues
            foreach (var venueData in uniqueVenues)
            {
                if (string.IsNullOrWhiteSpace(venueData.VenueEvent) || string.IsNullOrWhiteSpace(venueData.State))
                    continue;

                var existingVenue = _context.Venues.FirstOrDefault(v =>
                    v.VenueName == venueData.VenueEvent &&
                    v.City == venueData.City &&
                    v.State == venueData.State);

                if (existingVenue == null)
                {
                    var capacity = CsvImporter.ParseInt(venueData.Capacity);
                    var venue = new Venue
                    {
                        VenueName = venueData.VenueEvent,
                        City = venueData.City ?? "Unknown",
                        State = venueData.State,
                        Capacity = capacity,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                    };
                    _context.Venues.Add(venue);
                    Console.WriteLine($"Added venue: {venue.VenueName}");
                }
            }

            await _context.SaveChangesAsync();

            // Extract unique buyers
            var uniqueBuyers = records
                .Where(r => !string.IsNullOrWhiteSpace(r.BuyersFirstName) || !string.IsNullOrWhiteSpace(r.BuyersLastName))
                .GroupBy(r => new { r.BuyersFirstName, r.BuyersLastName, r.BuyerEmail })
                .Select(g => new { g.Key.BuyersFirstName, g.Key.BuyersLastName, g.Key.BuyerEmail, Phone = g.FirstOrDefault()?.BuyerPhone, Cell = g.FirstOrDefault()?.BuyerCell })
                .ToList();

            // Seed buyers
            foreach (var buyerData in uniqueBuyers)
            {
                if (string.IsNullOrWhiteSpace(buyerData.BuyersFirstName) && string.IsNullOrWhiteSpace(buyerData.BuyersLastName))
                    continue;

                var existingBuyer = _context.Buyers.FirstOrDefault(b =>
                    b.FirstName == buyerData.BuyersFirstName &&
                    b.LastName == buyerData.BuyersLastName);

                if (existingBuyer == null)
                {
                    var buyer = new Buyer
                    {
                        FirstName = buyerData.BuyersFirstName ?? "Unknown",
                        LastName = buyerData.BuyersLastName ?? "Unknown",
                        Email = buyerData.BuyerEmail,
                        Phone = buyerData.Phone,
                        Cell = buyerData.Cell,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                    };
                    _context.Buyers.Add(buyer);
                    Console.WriteLine($"Added buyer: {buyer.FirstName} {buyer.LastName}");
                }
            }

            await _context.SaveChangesAsync();

            // Extract unique subcontractors (Production, Venue Contact, Manager, Booking Agent)
            var uniqueSubcontractors = new List<(string? Name, string? Role, string? Email, string? Phone)>();

            // Production
            foreach (var record in records.Where(r => !string.IsNullOrWhiteSpace(r.Production)))
            {
                uniqueSubcontractors.Add((record.Production, "Production", record.ProductionEmail, record.ProductionPhone));
            }

            // Venue Contact
            foreach (var record in records.Where(r => !string.IsNullOrWhiteSpace(r.VenueContact)))
            {
                uniqueSubcontractors.Add((record.VenueContact, "Venue Contact", record.VenueEmail, record.VenuePhone));
            }

            // Manager
            foreach (var record in records.Where(r => !string.IsNullOrWhiteSpace(r.Manager)))
            {
                uniqueSubcontractors.Add((record.Manager, "Manager", null, null));
            }

            // Booking Agent
            foreach (var record in records.Where(r => !string.IsNullOrWhiteSpace(r.BookingAgent)))
            {
                uniqueSubcontractors.Add((record.BookingAgent, "Booking Agent", null, null));
            }

            var distinctSubcontractors = uniqueSubcontractors
                .GroupBy(s => new { s.Name, s.Role })
                .Select(g => g.First())
                .ToList();

            foreach (var subData in distinctSubcontractors)
            {
                if (string.IsNullOrWhiteSpace(subData.Name))
                    continue;

                var existingSubcontractor = _context.Subcontractors.FirstOrDefault(s =>
                    s.Name == subData.Name && s.Role == subData.Role);

                if (existingSubcontractor == null)
                {
                    var subcontractor = new Subcontractor
                    {
                        Name = subData.Name,
                        Role = subData.Role,
                        Email = subData.Email,
                        Phone = subData.Phone,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                    };
                    _context.Subcontractors.Add(subcontractor);
                    Console.WriteLine($"Added subcontractor: {subcontractor.Name} ({subcontractor.Role})");
                }
            }

            await _context.SaveChangesAsync();

            // Seed shows
            foreach (var record in records)
            {
                var showDate = CsvImporter.ParseShowDate(record.DateOfShow);
                if (showDate == null || string.IsNullOrWhiteSpace(record.VenueEvent))
                    continue;

                var venue = _context.Venues.FirstOrDefault(v =>
                    v.VenueName == record.VenueEvent &&
                    v.City == record.City &&
                    v.State == record.State);

                if (venue == null)
                    continue;

                var existingShow = _context.Shows.FirstOrDefault(s =>
                    s.VenueId == venue.VenueId && s.ShowDate == showDate);

                if (existingShow == null)
                {
                    var show = new Show
                    {
                        VenueId = venue.VenueId,
                        ShowDate = showDate.Value,
                        ShowName = $"{record.VenueEvent} - {showDate:MM/dd/yy}",
                        Status = record.Status ?? "Pending",
                        Deal = record.Deal,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                    };
                    _context.Shows.Add(show);
                    Console.WriteLine($"Added show: {show.ShowName}");
                }
            }

            await _context.SaveChangesAsync();

            // Seed walks
            foreach (var record in records)
            {
                var showDate = CsvImporter.ParseShowDate(record.DateOfShow);
                if (showDate == null || string.IsNullOrWhiteSpace(record.VenueEvent))
                    continue;

                var venue = _context.Venues.FirstOrDefault(v =>
                    v.VenueName == record.VenueEvent &&
                    v.City == record.City &&
                    v.State == record.State);

                if (venue == null)
                    continue;

                var show = _context.Shows.FirstOrDefault(s =>
                    s.VenueId == venue.VenueId && s.ShowDate == showDate);

                if (show == null)
                    continue;

                var walkAmount = CsvImporter.ParseDecimal(record.Walk);
                var merchAmount = CsvImporter.ParseDecimal(record.Merch);

                if (walkAmount > 0 || merchAmount > 0)
                {
                    var existingWalk = _context.Walks.FirstOrDefault(w => w.ShowId == show.ShowId);

                    if (existingWalk == null)
                    {
                        var walk = new Walk
                        {
                            ShowId = show.ShowId,
                            WalkAmount = walkAmount,
                            MerchAmount = merchAmount > 0 ? merchAmount : null,
                            Notes = record.ShowNotes,
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now
                        };
                        _context.Walks.Add(walk);
                        Console.WriteLine($"Added walk for show: {show.ShowName}");
                    }
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("Data seeding completed!");
        }
    }
}
