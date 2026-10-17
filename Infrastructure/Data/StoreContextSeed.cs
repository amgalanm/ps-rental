using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() }
            };

        if (!await context.Games.AnyAsync())
        {
            var gamesData = await File.ReadAllTextAsync(
                Path.Combine(path!, "Data", "SeedData", "games.json"));

            var games = JsonSerializer.Deserialize<List<Game>>(gamesData, options);

            if (games == null) return;

            context.Games.AddRange(games);

            await context.SaveChangesAsync();
        }

        if (!await context.PsnAccounts.AnyAsync())
        {
            var accountsData = await File.ReadAllTextAsync(
                Path.Combine(path!, "Data", "SeedData", "accounts.json"));

            var accounts = JsonSerializer.Deserialize<List<PsnAccount>>(accountsData, options);

            if (accounts == null) return;
            
            context.PsnAccounts.AddRange(accounts);

            await context.SaveChangesAsync();
        }

        if (!await context.Rentals.AnyAsync())
        {
            var rentalsData = await File.ReadAllTextAsync(
                Path.Combine(path!, "Data", "SeedData", "rentals.json"));

            var rentals = JsonSerializer.Deserialize<List<Rental>>(rentalsData, options);

            if (rentals == null) return;
            
            context.Rentals.AddRange(rentals);

            await context.SaveChangesAsync();
        }
    }
}
