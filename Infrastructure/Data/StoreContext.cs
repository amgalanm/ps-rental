using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class StoreContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<PsnAccount> PsnAccounts { get; set; }
    public DbSet<Rental> Rentals { get; set; }
}
