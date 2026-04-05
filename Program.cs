using VenueTracker.Components;
using Microsoft.EntityFrameworkCore;
using VenueTracker.Data;
using VenueTracker.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=VenueTracker.db"));

var app = builder.Build();

// Seed database from CSV if needed
if (app.Environment.IsDevelopment())
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.MigrateAsync();

            // Only seed if tables are empty
            if (!context.Venues.Any())
            {
                var csvPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Venue Buyer Database .csv");
                Console.WriteLine($"Looking for CSV at: {csvPath}");
                Console.WriteLine($"CSV exists: {File.Exists(csvPath)}");

                if (File.Exists(csvPath))
                {
                    Console.WriteLine("Starting CSV seeding...");
                    var seeder = new DataSeeder(context);
                    await seeder.SeedFromCsv(csvPath);
                    Console.WriteLine("CSV seeding completed!");
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR during seeding: {ex}");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
