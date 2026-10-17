namespace Core.Entities;

public class Game : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? PictureUrl { get; set; }
    public GamePlatform Platform { get; set; }
    public decimal PriceWithActivation { get; set; }
    public decimal PriceWithoutActivation { get; set; }
    public ICollection<PsnAccount> PsnAccounts { get; set; } = [];
}
