using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using VenueTracker.Models;

namespace VenueTracker.Seeding
{
    public class CsvRecord
    {
        public string? DateOfShow { get; set; }
        public string? Status { get; set; }
        public string? VenueEvent { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? BuyersFirstName { get; set; }
        public string? BuyersLastName { get; set; }
        public string? BuyerEmail { get; set; }
        public string? BuyerPhone { get; set; }
        public string? BuyerCell { get; set; }
        public string? Production { get; set; }
        public string? ProductionPhone { get; set; }
        public string? ProductionEmail { get; set; }
        public string? VenueContact { get; set; }
        public string? VenuePhone { get; set; }
        public string? VenueEmail { get; set; }
        public string? Walk { get; set; }
        public string? Merch { get; set; }
        public string? Deal { get; set; }
        public string? Capacity { get; set; }
        public string? ShowNotes { get; set; }
        public string? BookingAgent { get; set; }
        public string? Manager { get; set; }
    }

    public class CsvRecordMap : ClassMap<CsvRecord>
    {
        public CsvRecordMap()
        {
            Map(m => m.DateOfShow).Name("Date of Show");
            Map(m => m.Status).Name("Status");
            Map(m => m.VenueEvent).Name("Venue/Event");
            Map(m => m.Address).Name("Address");
            Map(m => m.City).Name("City");
            Map(m => m.State).Name("State");
            Map(m => m.BuyersFirstName).Name("Buyer's First Name");
            Map(m => m.BuyersLastName).Name("Buyer's Last Name");
            Map(m => m.BuyerEmail).Name("Buyer Email");
            Map(m => m.BuyerPhone).Name("Buyer Phone");
            Map(m => m.BuyerCell).Name("Buyer Cell");
            Map(m => m.Production).Name("Production");
            Map(m => m.ProductionPhone).Name("Production Phone");
            Map(m => m.ProductionEmail).Name("Production Email");
            Map(m => m.VenueContact).Name("Venue Contact");
            Map(m => m.VenuePhone).Name("Venue Phone");
            Map(m => m.VenueEmail).Name("Venue Email");
            Map(m => m.Walk).Name("Walk");
            Map(m => m.Merch).Name("Merch");
            Map(m => m.Deal).Name("Deal");
            Map(m => m.Capacity).Name("Capacity");
            Map(m => m.ShowNotes).Name("Show Notes");
            Map(m => m.BookingAgent).Name("Booking Agent");
            Map(m => m.Manager).Name("Manager");
        }
    }

    public class CsvImporter
    {
        public static List<CsvRecord> ImportCsv(string filePath)
        {
            var records = new List<CsvRecord>();

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<CsvRecordMap>();
                    records = csv.GetRecords<CsvRecord>().ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CSV: {ex.Message}");
            }

            return records;
        }

        public static DateTime? ParseShowDate(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr))
                return null;

            if (DateTime.TryParse(dateStr, out var date))
                return date;

            return null;
        }

        public static decimal ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0m;

            // Remove common formatting like $, commas, spaces
            value = value.Replace("$", "").Replace(",", "").Trim();

            if (decimal.TryParse(value, out var result))
                return result;

            return 0m;
        }

        public static int? ParseInt(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (int.TryParse(value, out var result))
                return result;

            return null;
        }
    }
}
