namespace Core.Entities;

public class PsnAccount : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int GameId { get; set; }
    public AccountStatus Status { get; set; }
    public Game Game { get; set; } = null!;
    public string? Note { get; set; }
    public ICollection<Rental> Rentals { get; set; } = [];
}
