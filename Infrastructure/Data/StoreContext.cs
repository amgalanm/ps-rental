using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class StoreContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<PsnAccount> PsnAccounts { get; set; }
    public DbSet<Rental> Rentals { get; set; }
}
