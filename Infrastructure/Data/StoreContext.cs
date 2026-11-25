using Core.Entities;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class StoreContext(DbContextOptions options,
    PsnPasswordEncryptor psnPasswordEncryptor) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Game> Games { get; set; }
    public DbSet<PsnAccount> PsnAccounts { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var encryptor = psnPasswordEncryptor;

        builder.Entity<PsnAccount>()
            .Property(a => a.Password)
            .HasConversion(
                password => encryptor.Encrypt(password),
                encrypted => encryptor.Decrypt(encrypted));

        builder.Entity<Rental>()
            .HasIndex(r => new { r.PsnAccountId, r.Type })
            .IsUnique()
            .HasFilter($"\"Status\" = {(int)RentalStatus.Active}");

        builder.Entity<Rental>()
            .HasIndex(r => r.PsnAccountId);
    }
}
