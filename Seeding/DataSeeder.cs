using VenueTracker.Data;
using VenueTracker.Models;
using Microsoft.EntityFrameworkCore;

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

            // Seed statuses
            var statuses = new[] { "Pending", "Confirmed", "Rejected", "Cancelled" };
            foreach (var statusName in statuses)
            {
                if (!_context.tStatuses.Any(s => s.StatusName == statusName))
                {
                    _context.tStatuses.Add(new tStatus { StatusName = statusName });
                }
            }
            await _context.SaveChangesAsync();

            // Delete shows with status "travel"
            var travelShows = _context.tShows.Include(s => s.tStatus).Where(s => s.tStatus != null && s.tStatus.StatusName == "travel").ToList();
            if (travelShows.Any())
            {
                _context.tShows.RemoveRange(travelShows);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Deleted {travelShows.Count} shows with status 'travel'");
            }

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

                var existingVenue = _context.tVenues.FirstOrDefault(v =>
                    v.VenueName == venueData.VenueEvent &&
                    v.City == venueData.City &&
                    v.State == venueData.State);

                if (existingVenue == null)
                {
                    var capacity = CsvImporter.ParseInt(venueData.Capacity);
                    var venue = new tVenue
                    {
                        VenueName = venueData.VenueEvent,
                        City = venueData.City ?? "Unknown",
                        State = venueData.State,
                        Capacity = capacity,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                    };
                    _context.tVenues.Add(venue);
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

                var existingBuyer = _context.tBuyers.FirstOrDefault(b =>
                    b.FirstName == buyerData.BuyersFirstName &&
                    b.LastName == buyerData.BuyersLastName);

                if (existingBuyer == null)
                {
                    var buyer = new tBuyer
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
                    _context.tBuyers.Add(buyer);
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

                var existingSubcontractor = _context.tSubcontractors.FirstOrDefault(s =>
                    s.Name == subData.Name && s.Role == subData.Role);

                if (existingSubcontractor == null)
                {
                    var subcontractor = new tSubcontractor
                    {
                        Name = subData.Name,
                        Role = subData.Role,
                        Email = subData.Email,
                        Phone = subData.Phone,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                    };
                    _context.tSubcontractors.Add(subcontractor);
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

                var venue = _context.tVenues.FirstOrDefault(v =>
                    v.VenueName == record.VenueEvent &&
                    v.City == record.City &&
                    v.State == record.State);

                if (venue == null)
                    continue;

                var existingShow = _context.tShows.FirstOrDefault(s =>
                    s.VenueId == venue.VenueId && s.ShowDate == showDate);

                var walkAmount = CsvImporter.ParseDecimal(record.Walk);
                var merchAmount = CsvImporter.ParseDecimal(record.Merch);
                var notes = record.ShowNotes;

                var statusName = record.Status ?? "Pending";
                var status = _context.tStatuses.FirstOrDefault(s => s.StatusName == statusName);
                if (status == null)
                {
                    status = _context.tStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                    if (status == null)
                    {
                        throw new InvalidOperationException("Pending status not found");
                    }
                }

                if (existingShow == null)
                {
                    var show = new tShow
                    {
                        VenueId = venue.VenueId,
                        ShowDate = showDate.Value,
                        ShowName = $"{record.VenueEvent} - {showDate:MM/dd/yy}",
                        StatusId = status.StatusId,
                        Deal = record.Deal,
                        WalkAmount = walkAmount,
                        MerchAmount = merchAmount > 0 ? merchAmount : null,
                        Notes = notes,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now
                    };
                    _context.tShows.Add(show);
                    Console.WriteLine($"Added show: {show.ShowName}");
                }
                else
                {
                    if (walkAmount > 0)
                    {
                        existingShow.WalkAmount = walkAmount;
                    }

                    if (merchAmount > 0)
                    {
                        existingShow.MerchAmount = merchAmount;
                    }

                    if (!string.IsNullOrWhiteSpace(notes))
                    {
                        existingShow.Notes = notes;
                    }

                    existingShow.UpdatedOn = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("Data seeding completed!");
        }
    }
}
